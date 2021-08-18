using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneradorCfdi;
using Vallen;

namespace TxtToCfdi
{
    public class ParserVallen : IParser
    {
        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);
            
            if (comp != null)
            {
                var addenda = GetVallenAddendaInfo(data,comp.Conceptos);

                if (addenda != null)
                {
                    //comp.AddendaSoriana = adenda;
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(addenda, typeof(requestForPayment), null,null);
                   
                    comp.XmlAdenda = comp.XmlAdenda.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                    //algo raro que se grega la etiqueta </currency> 
                   
                    /* //se quito porque no era necesario
                    comp.XmlAdenda = comp.XmlAdenda.Replace("<currency currencyISOCode=\"MXN\" />", "<currency xml:space=\"preserve\" currencyISOCode=\"MXN\" > </currency>");
                    comp.XmlAdenda = comp.XmlAdenda.Replace("<currency currencyISOCode=\"USD\" />", "<currency xml:space=\"preserve\" currencyISOCode=\"USD\" > </currency>");
                    comp.XmlAdenda = comp.XmlAdenda.Replace("<currency currencyISOCode=\"XEU\" />", "<currency xml:space=\"preserve\" currencyISOCode=\"XEU\" > </currency>");
                    */
                    //comp.XmlAdenda = comp.XmlAdenda.Replace("xml:space=\"preserve\"", ""); 
       
                
                }
                res.Add(comp);
            }
            return res;
        }

        private requestForPayment GetVallenAddendaInfo(string[][] data,GeneradorCfdi.ComprobanteConcepto[] comCop)
        {
            // Lets get the information from de layout
            // We will use the order:
            // requestForPayment|DeliveryDate|documentStatus|documentStructureVersion|contentVersion|type
            // orderIdentification|referenceIdentification|type
            // currency|currencyISOCode

            var reqForPayment = data.FirstOrDefault(p => p[0] == "requestForPayment");
            var curr = data.FirstOrDefault(p => p[0] == "currency");
            var orderId = data.Where(p => p[0] == "referenceIdentification").ToList();

            if (reqForPayment == null && curr == null && orderId.Count == 0)
            {
                return null;
            }

            var addenda = new requestForPayment();
            addenda.deliveryDate = DateTime.Parse(reqForPayment[1]);
            addenda.DeliveryDateSpecified = true;

            switch (reqForPayment[2])
            {
                case "COPY": addenda.documentStatus = requestForPaymentDocumentStatus.COPY; break;
                case "DELETE": addenda.documentStatus = requestForPaymentDocumentStatus.DELETE; break;
                case "ORIGINAL": addenda.documentStatus = requestForPaymentDocumentStatus.ORIGINAL; break;
                case "REEMPLAZA": addenda.documentStatus = requestForPaymentDocumentStatus.REEMPLAZA; break;
            }

            addenda.documentStructureVersion = reqForPayment[3];
            addenda.contentVersion = reqForPayment[4];
            addenda.type = reqForPayment[5];


            List<requestForPaymentReferenceIdentification> referenceIds = new List<requestForPaymentReferenceIdentification>();

            foreach (var id in orderId)
            {
                var refId = new requestForPaymentReferenceIdentification();
                refId.Value = id[1];


                requestForPaymentReferenceIdentificationType type;
                if (id[2] == "CS")
                {
                    type = requestForPaymentReferenceIdentificationType.CS;
                }
                else
                {
                    type = requestForPaymentReferenceIdentificationType.ON;
                }
                refId.type = type;
                referenceIds.Add(refId);
            }

            addenda.orderIdentification = referenceIds.ToArray();

            /*
            requestForPaymentCurrencyCurrencyISOCode currencyISO;
            
            switch (curr[1])
            {
                case "XEU": currencyISO = requestForPaymentCurrencyCurrencyISOCode.XEU; break;
                case "USD": currencyISO = requestForPaymentCurrencyCurrencyISOCode.USD; break;
                default: currencyISO = requestForPaymentCurrencyCurrencyISOCode.MXN; break;
            }

            requestForPaymentCurrency currency = new requestForPaymentCurrency { currencyISOCode = currencyISO };
            */
            addenda.currency = new requestForPaymentCurrency();
            addenda.currency.currencyISOCode = new requestForPaymentCurrencyCurrencyISOCode();
            if (curr[1] == "MXN")
            addenda.currency.currencyISOCode = requestForPaymentCurrencyCurrencyISOCode.MXN;
            if (curr[1] == "USD")
            addenda.currency.currencyISOCode = requestForPaymentCurrencyCurrencyISOCode.USD;
            if (curr[1] == "XEU")
            addenda.currency.currencyISOCode = requestForPaymentCurrencyCurrencyISOCode.XEU;

            if(comCop.Any())
            {
                List<requestForPaymentConcepto> COMP=new List<requestForPaymentConcepto>();
            foreach (var cp in comCop)
            {
                requestForPaymentConcepto c = new requestForPaymentConcepto();
                c.cantidad = cp.Cantidad;
                c.articulo = cp.NoIdentificacion;
                c.destino = curr[2];
                c.valorUnitario =Convert.ToDecimal( cp.ValorUnitario);
                c.importe =Convert.ToDecimal( cp.Importe);
                COMP.Add(c);

            }
            addenda.Conceptos = COMP.ToArray();
            }
            return addenda;
        }
    }
}
