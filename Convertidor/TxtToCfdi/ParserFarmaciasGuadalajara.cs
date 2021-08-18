using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Fragua;
using GeneradorCfdi;

namespace TxtToCfdi
{
    public class ParserFarmaciasGuadalajara : IParser
    {

        public List<GeneradorCfdi.Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comprobante = new ParserNtLink().ParseData(data);

            if (comprobante != null)
            {
                var rfp = data.FirstOrDefault(p => p[0] == "requestForPayment");
                var payableAmount = data.FirstOrDefault(p => p[0] == "payableAmount");
                var identification = data.FirstOrDefault(p => p[0] == "requestForPaymentIdentification");
                var referenceIdentification = data.FirstOrDefault(p => p[0] == "referenceIdentification");
                var additionalInformation = data.FirstOrDefault(p => p[0] == "AdditionalInformation[]");
                var deliveryNote = data.FirstOrDefault(p => p[0] == "DeliveryNote");
                var buyer = data.FirstOrDefault(p => p[0] == "buyer");
                var currency = data.FirstOrDefault(p => p[0] == "currency");
                var seller = data.FirstOrDefault(p => p[0] == "seller");
                var shipTo = data.FirstOrDefault(p => p[0] == "shipTo");
                if (rfp != null && rfp.Length > 0)
                {
                    var request = new Fragua.requestForPayment();
                    //<requestForPayment type="SimpleInvoiceType" contentVersion="1.3.1" documentStructureVersion="AMC006" documentStatus="ORIGINAL" DeliveryDate="2014-10-12">
                    request.type = "SimpleInvoiceType";
                    request.contentVersion = "1.3.1";
                    request.documentStructureVersion = "AMC006";
                    request.documentStatus = "ORIGINAL";
                    request.DeliveryDate = DateTime.ParseExact(rfp[5], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    request.requestForPaymentIdentification = new requestForPaymentRequestForPaymentIdentification();
                    request.requestForPaymentIdentification.entityType =
                        requestForPaymentRequestForPaymentIdentificationEntityType.INVOICE;
                    request.requestForPaymentIdentification.uniqueCreatorIdentification = identification[2];
                    request.orderIdentification = new requestForPaymentOrderIdentification();
                    request.orderIdentification.referenceIdentification = new[]
                                                                      {
                                                                          new requestForPaymentOrderIdentificationReferenceIdentification
                                                                              ()
                                                                          {
                                                                              type =
                                                                                  requestForPaymentOrderIdentificationReferenceIdentificationType
                                                                                  .ON,
                                                                              Value = referenceIdentification[2]
                                                                          }
                                                                      };
                    request.AdditionalInformation = new[]
                                                {
                                                    new requestForPaymentReferenceIdentification()
                                                    {
                                                        type = requestForPaymentReferenceIdentificationType.IV,
                                                        Value = additionalInformation[2]
                                                    }
                                                };
                    request.DeliveryNote = new requestForPaymentDeliveryNote();
                    request.DeliveryNote.ReferenceDate = DateTime.ParseExact(deliveryNote[2], "yyyy-MM-dd",
                        CultureInfo.InvariantCulture);
                    request.DeliveryNote.ReferenceDateSpecified = true;
                    request.DeliveryNote.referenceIdentification = new string[] { deliveryNote[1] };
                    request.buyer = new requestForPaymentBuyer();
                    request.buyer.gln = buyer[1];
                    request.seller = new requestForPaymentSeller();
                    request.seller.gln = seller[1];
                    request.shipTo = new requestForPaymentShipTo();
                    request.shipTo.gln = shipTo[1];
                    request.currency = new[]
                                   {
                                       new requestForPaymentCurrency()
                                       {
                                           rateOfChange = decimal.Parse(currency[1]),
                                           rateOfChangeSpecified = true,
                                           currencyISOCode = requestForPaymentCurrencyCurrencyISOCode.MXN

                                       }
                                   };
                    var lineItems = new List<requestForPaymentLineItem>();
                    int countConceptos = 0;
                    foreach (var comprobanteConcepto in comprobante.Conceptos)
                    {
                        requestForPaymentLineItem li = new requestForPaymentLineItem();
                        li.type = "SimpleInvoiceLineItemType";
                        li.number = (++countConceptos).ToString();
                        li.tradeItemIdentification = new requestForPaymentLineItemTradeItemIdentification()
                        {
                            gtin = comprobanteConcepto.NoIdentificacion
                        };
                        li.tradeItemDescriptionInformation = new requestForPaymentLineItemTradeItemDescriptionInformation()
                        {
                            language =
                                requestForPaymentLineItemTradeItemDescriptionInformationLanguage
                                .ES,
                            languageSpecified = true,
                            longText = comprobanteConcepto.Descripcion
                        };
                        li.invoicedQuantity = new requestForPaymentLineItemInvoicedQuantity()
                        {
                            unitOfMeasure =
                                (requestForPaymentLineItemInvoicedQuantityUnitOfMeasure)
                                Enum.Parse(
                                    typeof(requestForPaymentLineItemInvoicedQuantityUnitOfMeasure),
                                    comprobanteConcepto.Unidad, true),
                            Text =
                                new string[]
                                                  {
                                                      comprobanteConcepto.Cantidad.ToString("F2",
                                                          CultureInfo.InvariantCulture)
                                                  }
                        };
                        li.grossPrice = new requestForPaymentLineItemGrossPrice()
                        {
                            Amount =Convert.ToDecimal( comprobanteConcepto.ValorUnitario)
                        };

                        //var iva = 1.16;
                        li.totalLineAmount = new requestForPaymentLineItemTotalLineAmount()
                        {
                            grossAmount = new requestForPaymentLineItemTotalLineAmountGrossAmount()
                            {
                                Amount =Convert.ToDecimal( comprobanteConcepto.Importe),
                            },
                            netAmount = new requestForPaymentLineItemTotalLineAmountNetAmount()
                            {

                                Amount = Convert.ToDecimal(comprobanteConcepto.Importe)
                            }


                        };
                        lineItems.Add(li);
                    }
                    request.lineItem = lineItems.ToArray();
                    request.totalAmount = new requestForPaymentTotalAmount()
                    {
                        Amount = comprobante.SubTotal
                    };

                    request.TotalAllowanceCharge = new requestForPaymentTotalAllowanceCharge[]
                                               {
                                                   new requestForPaymentTotalAllowanceCharge()
                                                   {
                                                       Amount =Convert.ToDecimal( comprobante.Descuento)
                                                   }
                                               };
                    request.baseAmount = new requestForPaymentBaseAmount()
                    {
                        Amount = comprobante.SubTotal
                    };
                    var rfpTaxes = new List<requestForPaymentTax>();
                    foreach (var tras in comprobante.Impuestos.Traslados)
                    {
                        var tax = new requestForPaymentTax()
                        {
                            type = requestForPaymentTaxType.VAT,
                            taxAmount = Convert.ToDecimal(comprobante.Impuestos.TotalImpuestosTrasladados),
                            taxAmountSpecified = true,
                            taxPercentage = Convert.ToDecimal(tras.TasaOCuota),
                            taxPercentageSpecified = true,
                            taxCategory = requestForPaymentTaxTaxCategory.TRANSFERIDO,
                            taxCategorySpecified = true,
                            typeSpecified = true
                        };
                        rfpTaxes.Add(tax);
                    }
                    request.tax = rfpTaxes.ToArray();
                    request.payableAmount = new requestForPaymentPayableAmount()
                    {
                        Amount = comprobante.Total
                    };
                    if (request != null)
                    {

                        comprobante.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(request,
                            typeof(Fragua.requestForPayment), null, null);
                    }
                }
                
                res.Add(comprobante);
            }

            return res;

        }

    
    }
}
