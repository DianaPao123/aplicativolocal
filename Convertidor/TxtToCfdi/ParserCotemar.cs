using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using GeneradorCfdi;
using log4net;

namespace TxtToCfdi
{
    public class ParserCotemar : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserCotemar));



        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);//leer el archivo 
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetCotemarAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(Cotemar), "", "https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar/Addenda.xsd");
                   
                   // var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(AddendaCotemar), "cot", "https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar/Addenda.xsd");
                    //    xmlAdenda = xmlAdenda.Replace("AddendaTridonex xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:cfdi=\"http://tempuri.org/Tridonex.xsd\"", "Tridonex");
                   // xmlAdenda = xmlAdenda.Replace("<cfdi:TridonexPODetalle>", "");
                   // xmlAdenda = xmlAdenda.Replace("</cfdi:TridonexPODetalle>", "");

                    
                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        //-------------------------------------------------------------
        private Cotemar GetCotemarAdenda(string[][] data)
        {
            var Detalle = data.Where(j => j[0] == "Cotemar");
                                   
            if (Detalle.Any())
            {

                //List<Cotemar> Lista = new List<Cotemar>();
                Cotemar cotemar = new Cotemar();
                foreach (var detalle in Detalle)
                {
                    //Cotemar cotemar = new Cotemar();
                    cotemar.NumProveedor = double.Parse(ParserNtLink.GetValue(detalle[1]));  //sacar datos de factura
                    cotemar.NumPedido=double.Parse(ParserNtLink.GetValue(detalle[2]));
                    cotemar.NumEntMercancia=ParserNtLink.GetValue(detalle[3]);
                    cotemar.ContactoCompra=ParserNtLink.GetValue(detalle[4]);
                    if(ParserNtLink.GetValue(detalle[5])=="MXN")
                    cotemar.Moneda=CotemarMoneda.MXN;
                    if(ParserNtLink.GetValue(detalle[5])=="USD")
                    cotemar.Moneda=CotemarMoneda.USD;
                    if (ParserNtLink.GetValue(detalle[5]) == "EUR")
                    cotemar.Moneda=CotemarMoneda.EUR;
                    
                  //  Lista.Add(cotemar);
                    
                }

               // AddendaCotemar A = new AddendaCotemar();
               // A.cotemar = cotemar;
                return cotemar;
                
            }
            return null;

        }
        //_----------------------------------------------------------------------------

    }
}
