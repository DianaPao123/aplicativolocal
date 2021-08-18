using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneradorCfdi;
using log4net;
using System.IO;
using AddendaAmece;
using System.Globalization;

namespace TxtToCfdi
{
    public class ParserDupont : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserNtLink));



        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);
            if (comp != null)
            {
                // parsear parte de la addenda
                var requestForPaymentIdentification =
                    data.Where(p => p[0] == "requestForPaymentIdentification").FirstOrDefault();
                var referenceIdentification = data.Where(p => p[0] == "referenceIdentification[]").ToList();
                var orderIdentification = data.Where(p => p[0] == "orderIdentification").FirstOrDefault();
                var personOrDepartmentName = data.Where(p => p[0] == "personOrDepartmentName").FirstOrDefault();
                var contactInformation = data.Where(p => p[0] == "contactInformation").FirstOrDefault();
                var buyer = data.Where(p => p[0] == "buyer").FirstOrDefault();
                var nameAndAddress = data.Where(p => p[0] == "nameAndAddress").FirstOrDefault();
                var currency = data.Where(p => p[0] == "currency[]").FirstOrDefault();
                var InvoiceCreator = data.Where(p => p[0] == "InvoiceCreator").FirstOrDefault();
                var requestForPayment = data.Where(p => p[0] == "requestForPayment").FirstOrDefault();
                if (requestForPayment != null && requestForPayment.Length > 0)
                {
                    requestForPayment rfp = new requestForPayment();
                    try
                    {
                        rfp.type = ParserNtLink.GetValue(requestForPayment[1]);
                        rfp.contentVersion = ParserNtLink.GetValue(requestForPayment[2]);
                        rfp.documentStructureVersion = ParserNtLink.GetValue(requestForPayment[3]);
                        var documentStatus =
                            (requestForPaymentDocumentStatus)
                                Enum.Parse(typeof(requestForPaymentDocumentStatus), requestForPayment[4]);
                        rfp.documentStatus = documentStatus;
                        if (!string.IsNullOrEmpty(requestForPayment[5]))
                        {
                            rfp.DeliveryDate = DateTime.ParseExact(requestForPayment[5], "yyyy-MM-dd",
                                CultureInfo.InvariantCulture);
                            rfp.DeliveryDateSpecified = true;
                        }

                        rfp.currency = new requestForPaymentCurrency[] { new requestForPaymentCurrency() };

                        if (!string.IsNullOrEmpty(currency[1]) && currency[2].ToLower() == "true")
                        {
                            rfp.currency[0].rateOfChange = Decimal.Parse(currency[1]);
                            rfp.currency[0].rateOfChangeSpecified = true;
                        }
                        else rfp.currency[0].rateOfChangeSpecified = false;
                        rfp.currency[0].currencyISOCode =
                            (requestForPaymentCurrencyCurrencyISOCode)
                                Enum.Parse(typeof(requestForPaymentCurrencyCurrencyISOCode), currency[3]);

                        rfp.InvoiceCreator = new requestForPaymentInvoiceCreator();
                        rfp.InvoiceCreator.gln = ParserNtLink.GetValue(InvoiceCreator[1]);
                        rfp.InvoiceCreator.nameAndAddress = new requestForPaymentInvoiceCreatorNameAndAddress();
                        rfp.InvoiceCreator.nameAndAddress.name = ParserNtLink.GetValue(nameAndAddress[1]);
                        rfp.InvoiceCreator.nameAndAddress.streetAddressOne = ParserNtLink.GetValue(nameAndAddress[2]);
                        rfp.InvoiceCreator.nameAndAddress.city = ParserNtLink.GetValue(nameAndAddress[3]);
                        rfp.InvoiceCreator.nameAndAddress.postalCode = ParserNtLink.GetValue(nameAndAddress[4]);

                        rfp.buyer = new requestForPaymentBuyer();
                        rfp.buyer.gln = ParserNtLink.GetValue(buyer[1]);
                        rfp.buyer.contactInformation = new requestForPaymentBuyerContactInformation();
                        rfp.buyer.contactInformation.personOrDepartmentName =
                            new requestForPaymentBuyerContactInformationPersonOrDepartmentName();
                        rfp.buyer.contactInformation.personOrDepartmentName.text =
                            ParserNtLink.GetValue(personOrDepartmentName[1]);

                        rfp.orderIdentification = new requestForPaymentOrderIdentification();
                        if (!string.IsNullOrEmpty(orderIdentification[1]))
                        {
                            rfp.orderIdentification.ReferenceDate = DateTime.ParseExact(orderIdentification[1], "yyyy-MM-dd",
                                CultureInfo.InvariantCulture);
                            rfp.orderIdentification.ReferenceDateSpecified = true;
                        }
                        else rfp.orderIdentification.ReferenceDateSpecified = false;
                        var ris = new List<requestForPaymentOrderIdentificationReferenceIdentification>();
                        foreach (var ri in referenceIdentification)
                        {
                            var referenceId = new requestForPaymentOrderIdentificationReferenceIdentification();
                            referenceId.type = requestForPaymentOrderIdentificationReferenceIdentificationType.ON;
                            referenceId.Value = ParserNtLink.GetValue(ri[2]);
                            ris.Add(referenceId);
                        }
                        rfp.orderIdentification.referenceIdentification = ris.ToArray();

                        rfp.requestForPaymentIdentification = new requestForPaymentRequestForPaymentIdentification();
                        rfp.requestForPaymentIdentification.entityType =
                            (requestForPaymentRequestForPaymentIdentificationEntityType)
                                Enum.Parse(typeof(requestForPaymentRequestForPaymentIdentificationEntityType),
                                    requestForPaymentIdentification[1]);
                        ;
                        rfp.requestForPaymentIdentification.uniqueCreatorIdentification =
                            ParserNtLink.GetValue(requestForPaymentIdentification[2]);

                    }
                    catch (Exception e)
                    {
                        Logger.Error(e.Message);
                    }


                    comp.AddendaAmece = rfp;

                    comp.AddendaAmece = rfp;
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(rfp, typeof(requestForPayment), null,
                        null);
                    comp.XmlAdenda =
                        comp.XmlAdenda.Replace(
                            "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ",
                            "");
                }
                

                List<Comprobante> lista = new List<Comprobante>();
                lista.Add(comp);
                return lista;
            }
            else return null;



        }
    }
}
