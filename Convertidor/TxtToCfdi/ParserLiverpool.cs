using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GeneradorCfdi;
using Liverpool;

namespace TxtToCfdi
{
    public class ParserLiverpool :IParser
    {
        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);


            if (comp != null)
            {
                // Let's try to get the Addenda info

                var request = this.GetAddendaLiverpoolInfo(data, comp);
                if (request != null)
                {
                    comp.xsiSchemaLocation = comp.xsiSchemaLocation + " http://www.sat.gob.mx/detallista http://www.sat.gob.mx/sitio_internet/cfd/detallista/detallista.xsd";
                    var complemento = AddendaSerializer.GetXmlStringFromAddendaObject(request, typeof (detallista), "detallista", "http://www.sat.gob.mx/detallista");
                     comp.XmlComplemento = complemento;
                }
                res.Add(comp);
            }
            return res;
        }

        private detallista GetAddendaLiverpoolInfo(string[][] data, Comprobante comp)
        {
            var requestData = data.FirstOrDefault(p => p[0] == "detallista");

            if (requestData == null)
            {
                return null;
            }

            detallista request = new detallista
            {
                contentVersion = "1.3.1",
                documentStatus = detallistaDocumentStatus.ORIGINAL,
                documentStructureVersion = "AMC8.1",
                requestForPaymentIdentification = new detallistaRequestForPaymentIdentification
                {
                    entityType = detallistaRequestForPaymentIdentificationEntityType.INVOICE
                },
                specialInstruction = new[]
                {
                    new detallistaSpecialInstruction()
                    {
                        code = detallistaSpecialInstructionCode.ZZZ,
                        text =
                            new[]
                            {CantidadLetra.Enletras(comp.Total.ToString(), "MXN", "")}
                    },
                },
                orderIdentification = new detallistaOrderIdentification
                {
                    ReferenceDate =Convert.ToDateTime( comp.Fecha),
                    ReferenceDateSpecified = true,
                    referenceIdentification = new[]
                    {
                        new detallistaOrderIdentificationReferenceIdentification
                        {
                            type = detallistaOrderIdentificationReferenceIdentificationType.ON,
                            Value = requestData[1]
                        },
                    }
                },
                AdditionalInformation = new[]
                {
                    new detallistaReferenceIdentification
                    {
                        type = detallistaReferenceIdentificationType.ACE,
                        Value = "1"
                    }
                },
                DeliveryNote = new detallistaDeliveryNote
                {
                    ReferenceDate = DateTime.Now,
                    ReferenceDateSpecified = true,
                    referenceIdentification = new[] {requestData[2]}
                },
                buyer = new detallistaBuyer
                {
                    gln = requestData[3],
                    contactInformation = new detallistaBuyerContactInformation
                    {
                        personOrDepartmentName =
                            new detallistaBuyerContactInformationPersonOrDepartmentName {text = requestData[4]}
                    }
                },
                seller = new detallistaSeller
                {
                    gln = requestData[5],
                    alternatePartyIdentification = new detallistaSellerAlternatePartyIdentification
                    {
                        type = detallistaSellerAlternatePartyIdentificationType.SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY,
                        Value = requestData[6]
                    }
                },
                allowanceCharge = new[]
                {
                    new detallistaAllowanceCharge
                    {
                        allowanceChargeType = detallistaAllowanceChargeAllowanceChargeType.ALLOWANCE_GLOBAL,
                        settlementType = detallistaAllowanceChargeSettlementType.OFF_INVOICE,
                        specialServicesType = detallistaAllowanceChargeSpecialServicesType.AJ,
                        specialServicesTypeSpecified = true,
                        monetaryAmountOrPercentage =
                            new detallistaAllowanceChargeMonetaryAmountOrPercentage()
                            {
                                rate =
                                    new detallistaAllowanceChargeMonetaryAmountOrPercentageRate
                                    {
                                        percentage = 0
                                    }
                            }
                    },
                }
            };
            var lineItems = new List<detallistaLineItem>();

            int countConceptos = 0;
            foreach (var comprobanteConcepto in comp.Conceptos)
            {
                string descrip="";
                if (!string.IsNullOrEmpty(comprobanteConcepto.Descripcion))
                {
                    if (comprobanteConcepto.Descripcion.Count() > 35)
                        descrip = comprobanteConcepto.Descripcion.Substring(0, 35);
                    else
                        descrip = comprobanteConcepto.Descripcion;
                }
                detallistaLineItem li = new detallistaLineItem
                {
                    type = "SimpleInvoiceLineItemType",
                    number = (++countConceptos).ToString(),
                    tradeItemIdentification = new detallistaLineItemTradeItemIdentification()
                    {
                        gtin = comprobanteConcepto.NoIdentificacion
                    },
                    alternateTradeItemIdentification = new[]
                    {
                        new detallistaLineItemAlternateTradeItemIdentification
                        {
                            type =
                                detallistaLineItemAlternateTradeItemIdentificationType
                                    .BUYER_ASSIGNED,
                            Text = new[] {comprobanteConcepto.NoIdentificacion}
                        }
                    },
                    tradeItemDescriptionInformation = new detallistaLineItemTradeItemDescriptionInformation()
                    {
                        language = detallistaLineItemTradeItemDescriptionInformationLanguage.ES,
                        languageSpecified = true,
                        longText = descrip
                    },
                    invoicedQuantity = new detallistaLineItemInvoicedQuantity()
                    {
                        unitOfMeasure = comprobanteConcepto.Unidad,
                        Text = new[]
                            {
                                comprobanteConcepto.Cantidad.ToString("F2", CultureInfo.InvariantCulture)
                            }
                    }
                };

                li.invoicedQuantity = new detallistaLineItemInvoicedQuantity
                {
                    Text = new[] {comprobanteConcepto.Cantidad.ToString()},
                    unitOfMeasure = comprobanteConcepto.Unidad
                };
                li.grossPrice = new detallistaLineItemGrossPrice
                {
                    Amount = Convert.ToDecimal(comprobanteConcepto.ValorUnitario)
                };

                li.totalLineAmount = new detallistaLineItemTotalLineAmount
                {
                    netAmount = new detallistaLineItemTotalLineAmountNetAmount
                    {
                        //! Verify / ... 
                        Amount = Convert.ToDecimal(comprobanteConcepto.Importe)
                    },
                    grossAmount = new detallistaLineItemTotalLineAmountGrossAmount
                    {
                        Amount = Convert.ToDecimal(comprobanteConcepto.Importe),
                    }
                };
                //---------------------------------------------------
                if (comprobanteConcepto.ValorUnitario != null)
                {
                    li.netPrice = new detallistaLineItemNetPrice();
                    li.netPrice.Amount = Convert.ToDecimal(comprobanteConcepto.ValorUnitario);
                }
                //---------------------------------
                //UNSET IF ADDENDA HOME DEPOT IS USED

                lineItems.Add(li);
            }
            request.lineItem = lineItems.ToArray();
            request.TotalAllowanceCharge = new[]
                                               {
                                                   new detallistaTotalAllowanceCharge
                                                   {
                                                       specialServicesType = detallistaTotalAllowanceChargeSpecialServicesType.AJ,
                                                        specialServicesTypeSpecified=true,
                                                       Amount = 0,
                                                       AmountSpecified = true,
                                                       allowanceOrChargeType = detallistaTotalAllowanceChargeAllowanceOrChargeType.ALLOWANCE

                                                   }
                                               };
            request.totalAmount = new detallistaTotalAmount
            {
                Amount = comp.Total
            };
            return request;
        }
    }
}
