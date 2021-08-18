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
    public class ParserTridonex : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserTridonex));

        

        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetTridonexAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(AddendaTridonex), "cfdi", "http://tempuri.org/Tridonex.xsd");
                //    xmlAdenda = xmlAdenda.Replace("AddendaTridonex xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:cfdi=\"http://tempuri.org/Tridonex.xsd\"", "Tridonex");
                    xmlAdenda = xmlAdenda.Replace("<cfdi:TridonexPODetalle>", "");
                    xmlAdenda = xmlAdenda.Replace("</cfdi:TridonexPODetalle>", "");
                //    xmlAdenda = xmlAdenda.Replace("AddendaTridonex", "Tridonex");


                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private AddendaTridonex GetTridonexAdenda(string[][] data)
        {
            var mabeDetalle = data.Where(j => j[0] == "PODetalle");

            AddendaTridonex aTridonex = new AddendaTridonex();
           
            if (mabeDetalle.Any())
                    {

                        AddendaTridonex Tri = new AddendaTridonex();
                        Tri.TridonexPODetalle = new List<PODetalle>();
                        

                        foreach ( var detalle in mabeDetalle)
                        {
                            PODetalle pod = new PODetalle();
                            pod.PO = ParserNtLink.GetValue(detalle[1]);  //sacar datos de factura
                            pod.IVA = decimal.Parse(ParserNtLink.GetValue(detalle[6]));
                            pod.Line = ParserNtLink.GetValue(detalle[2]);
                            pod.noIdentificacion =ParserNtLink.GetValue(detalle[3]);
                            pod.SubTotal = decimal.Parse(ParserNtLink.GetValue(detalle[5]));
                            pod.Total = decimal.Parse(ParserNtLink.GetValue(detalle[7]));
                            pod.UUID = "";//comprobante.Complemento.timbreFiscalDigital.UUID;//?
                            pod.Cantidad = int.Parse(ParserNtLink.GetValue(detalle[4]));

                            Tri.TridonexPODetalle.Add(pod);
                        }

                        return Tri;

                        }
            return null;
            
        }
    }
}
