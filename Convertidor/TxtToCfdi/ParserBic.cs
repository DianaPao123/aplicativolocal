using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AddendaBic;
using GeneradorCfdi;

namespace TxtToCfdi
{
    public class ParserBic : IParser
    {
        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);
            
            if (comp != null)
            {
                var addenda = GetBicAddendaInfo(data, comp);

                if (addenda != null)
                {
                    //comp.AddendaSoriana = adenda;
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(addenda, typeof(requestForPayment), null,null);
                }
                res.Add(comp);
            }
            return res;
        }

        private requestForPayment GetBicAddendaInfo(string[][] data , Comprobante comprobante)
        {

            
            var reqForPayment = data.FirstOrDefault(p => p[0] == "RFP");
            var oid = data.FirstOrDefault(j => j[0] == "OID");
            var adin = data.FirstOrDefault(j => j[0] == "ADIN");
            var deliveryNotification = data.FirstOrDefault(p => p[0] == "DN");
            var buy = data.FirstOrDefault(j => j[0] == "BUY");
            var sel = data.FirstOrDefault(j => j[0] == "SEL");
            var shipto = data.FirstOrDefault(j => j[0] == "SHIPTO");
            var invoiceCreator = data.FirstOrDefault(p => p[0] == "IC");
            var pt = data.FirstOrDefault(j => j[0] == "PT");
            

            if (reqForPayment == null )
            {
                return null;
            }
            
            var addenda = new requestForPayment();
            addenda.requestForPaymentIdentification = new requestForPaymentRequestForPaymentIdentification
            {
                entityType = requestForPaymentRequestForPaymentIdentificationEntityType.INVOICE,
                uniqueCreatorIdentification = reqForPayment[1]
            };
           
            addenda.specialInstruction = new requestForPaymentSpecialInstruction[]
                                                 {
                                                     new requestForPaymentSpecialInstruction()
                                                         {
                                                             code = requestForPaymentSpecialInstructionCode.ZZZ,
                                                             text = new string[] {comprobante.CantidadLetra}
                                                         }
                                                 };
            addenda.orderIdentification = new requestForPaymentOrderIdentification()
            {
                referenceIdentification = new requestForPaymentOrderIdentificationReferenceIdentification[]
                                                                                    {
                                                                                        new requestForPaymentOrderIdentificationReferenceIdentification()
                                                                                            {
                                                                                                type = requestForPaymentOrderIdentificationReferenceIdentificationType.ON, 
                                                                                                Value = oid[1]
                                                                                            }
                                                                                    },
                ReferenceDate = DateTime.ParseExact(oid[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                ReferenceDateSpecified = true
            };
            addenda.AdditionalInformation = new requestForPaymentReferenceIdentification[]
                                                    {
                                                        new requestForPaymentReferenceIdentification()
                                                            {
                                                                type = requestForPaymentReferenceIdentificationType.ATZ,
                                                                Value = adin[1]
                                                            }
                                                    };
            addenda.DeliveryNote = new requestForPaymentDeliveryNote();
            addenda.DeliveryNote = new requestForPaymentDeliveryNote
            {
                ReferenceDateSpecified = true,
                referenceIdentification = new[] { deliveryNotification[1] },
                ReferenceDate = DateTime.Now
            };
            
            addenda.buyer = new requestForPaymentBuyer();
            addenda.buyer.gln = buy[1];
            addenda.buyer.contactInformation = new requestForPaymentBuyerContactInformation();
            addenda.buyer.contactInformation.personOrDepartmentName =
                new requestForPaymentBuyerContactInformationPersonOrDepartmentName
                {
                    text = buy[2]
                };
            addenda.seller = new requestForPaymentSeller();
            addenda.seller.gln = sel[1];
            addenda.seller.alternatePartyIdentification = new requestForPaymentSellerAlternatePartyIdentification
            {
                type = requestForPaymentSellerAlternatePartyIdentificationType.SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY,
                Value = sel[2]
            };
            addenda.shipTo = new requestForPaymentShipTo()
            {
                gln = shipto[1],
                nameAndAddress = new requestForPaymentShipToNameAndAddress()
                {
                    name = new string[] { ParserNtLink.GetValue(shipto[2]) },
                    streetAddressOne = new string[] { ParserNtLink.GetValue(shipto[3]) },
                    city = new string[] { ParserNtLink.GetValue(shipto[4]) },
                    postalCode = new string[] { ParserNtLink.GetValue(shipto[5]) }
                }
            };
            addenda.InvoiceCreator = new requestForPaymentInvoiceCreator();
            addenda.InvoiceCreator.gln = ParserNtLink.GetValue(invoiceCreator[1]);
            addenda.InvoiceCreator.alternatePartyIdentification = new requestForPaymentInvoiceCreatorAlternatePartyIdentification();
            addenda.InvoiceCreator.alternatePartyIdentification.type =
                requestForPaymentInvoiceCreatorAlternatePartyIdentificationType.VA;
            addenda.InvoiceCreator.alternatePartyIdentification.Value = invoiceCreator[2];
            addenda.InvoiceCreator.nameAndAddress = new requestForPaymentInvoiceCreatorNameAndAddress();
            addenda.InvoiceCreator.nameAndAddress.name = ParserNtLink.GetValue(invoiceCreator[3]);
            addenda.InvoiceCreator.nameAndAddress.streetAddressOne = ParserNtLink.GetValue(invoiceCreator[4]);
            addenda.InvoiceCreator.nameAndAddress.city = ParserNtLink.GetValue(invoiceCreator[5]);
            addenda.InvoiceCreator.nameAndAddress.postalCode = ParserNtLink.GetValue(invoiceCreator[6]);
            //para la moneda
            var currencyISOCodexx = requestForPaymentCurrencyCurrencyISOCode.MXN;
            if (comprobante.Moneda == "USD")
                currencyISOCodexx = requestForPaymentCurrencyCurrencyISOCode.USD;
            if (comprobante.Moneda == "MXN")
                currencyISOCodexx = requestForPaymentCurrencyCurrencyISOCode.MXN;
            if (comprobante.Moneda == "XEU")
                currencyISOCodexx = requestForPaymentCurrencyCurrencyISOCode.XEU;

            addenda.currency = new requestForPaymentCurrency[]
                                       {
                                           new requestForPaymentCurrency()
                                               {
                                                   currencyISOCode = currencyISOCodexx,
                                                   currencyFunction = new requestForPaymentCurrencyCurrencyFunction[]{requestForPaymentCurrencyCurrencyFunction.BILLING_CURRENCY, },
                                                   rateOfChange = comprobante.TipoCambio,
                                                   rateOfChangeSpecified = true
                                                   
                                               }
                                       };

            addenda.paymentTerms = new requestForPaymentPaymentTerms()
            {
                netPayment = new requestForPaymentPaymentTermsNetPayment()
                {
                    paymentTimePeriod = new requestForPaymentPaymentTermsNetPaymentPaymentTimePeriod()
                    {
                        timePeriodDue = new requestForPaymentPaymentTermsNetPaymentPaymentTimePeriodTimePeriodDue()
                        {
                            timePeriod = requestForPaymentPaymentTermsNetPaymentPaymentTimePeriodTimePeriodDueTimePeriod.DAYS,
                            value = pt[1]
                        }
                    },


                }

            };
            addenda.paymentTerms.paymentTermsEvent = requestForPaymentPaymentTermsPaymentTermsEvent.DATE_OF_INVOICE;
            addenda.paymentTerms.paymentTermsEventSpecified = true;
            addenda.paymentTerms.PaymentTermsRelationTime =
                requestForPaymentPaymentTermsPaymentTermsRelationTime.REFERENCE_AFTER;
            addenda.paymentTerms.PaymentTermsRelationTimeSpecified = true;
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
                    li.alternateTradeItemIdentification = new[]
                    {
                        new requestForPaymentLineItemAlternateTradeItemIdentification
                        {
                            type = requestForPaymentLineItemAlternateTradeItemIdentificationType.BUYER_ASSIGNED,
                            Text = new[] {comprobanteConcepto.NoIdentificacion}
                        }
                    };
                    li.tradeItemDescriptionInformation = new requestForPaymentLineItemTradeItemDescriptionInformation()
                    {
                        language = requestForPaymentLineItemTradeItemDescriptionInformationLanguage.ES,
                        languageSpecified = true,
                        longText = comprobanteConcepto.Descripcion
                    };
                    li.invoicedQuantity = new requestForPaymentLineItemInvoicedQuantity()
                    {
                        unitOfMeasure = comprobanteConcepto.Unidad,
                        Text = new string[] { comprobanteConcepto.Cantidad.ToString("F2", CultureInfo.InvariantCulture) }
                    };
                    li.aditionalQuantity = new[]
                    {
                        new requestForPaymentLineItemAditionalQuantity()
                        {
                            QuantityType = requestForPaymentLineItemAditionalQuantityQuantityType.NUM_CONSUMER_UNITS,
                            Text = new string[] {comprobanteConcepto.Cantidad.ToString("F2", CultureInfo.InvariantCulture)}
                        }
                    };
                    li.grossPrice = new requestForPaymentLineItemGrossPrice()
                    {
                        Amount =Convert.ToDecimal( comprobanteConcepto.ValorUnitario)
                    };
                    li.netPrice = new requestForPaymentLineItemNetPrice()
                    {
                        Amount =Convert.ToDecimal( comprobanteConcepto.ValorUnitario)
                    };
                    /*
                    li.tradeItemTaxInformation = new requestForPaymentLineItemTradeItemTaxInformation[]
                    {
                        new requestForPaymentLineItemTradeItemTaxInformation()
                        {
                            taxTypeDescription = requestForPaymentLineItemTradeItemTaxInformationTaxTypeDescription.VAT,
                        
                            tradeItemTaxAmount = new requestForPaymentLineItemTradeItemTaxInformationTradeItemTaxAmount()
                                                {
                                                    taxPercentage = decimal.Parse(comprobanteConcepto.PorcentajeIva),
                                                    taxAmount = decimal.Parse(comprobanteConcepto.Iva)
                                                    
                                                }
                         
                        }
                    };
                     */ 
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
            addenda.lineItem = lineItems.ToArray();
            addenda.totalAmount = new requestForPaymentTotalAmount()
            {
                Amount = comprobante.SubTotal
            };
            addenda.TotalAllowanceCharge = new requestForPaymentTotalAllowanceCharge[]
                                                   {
                                                       new requestForPaymentTotalAllowanceCharge()
                                                           {
                                                               
                                                               Amount = Convert.ToDecimal(comprobante.Descuento),
                                                               AmountSpecified = true,
                                                               allowanceOrChargeType = requestForPaymentTotalAllowanceChargeAllowanceOrChargeType.ALLOWANCE
                                                           }
                                                   };
            addenda.baseAmount = new requestForPaymentBaseAmount()
            {
                Amount = comprobante.SubTotal
            };

            var rfpTaxes = new List<requestForPaymentTax>();
            foreach (var tras in comprobante.Impuestos.Traslados)
            {
                var tax = new requestForPaymentTax()
                {
                    type = requestForPaymentTaxType.VAT,
                    typeSpecified = true,
                    taxAmount =Convert.ToDecimal( comprobante.Impuestos.TotalImpuestosTrasladados),
                    taxAmountSpecified = true,
                    taxPercentage =Convert.ToDecimal( tras.TasaOCuota),
                    taxPercentageSpecified = true,
                };
                rfpTaxes.Add(tax);
            }
            addenda.tax = rfpTaxes.ToArray();
            addenda.payableAmount = new requestForPaymentPayableAmount()
            {
                Amount = comprobante.Total
            };
            return addenda;
        }


       
    }
}
