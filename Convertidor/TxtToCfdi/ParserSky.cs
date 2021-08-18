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
using System.Xml.Linq;

namespace TxtToCfdi
{
    public class ParserSky : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserNtLink));

       
        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);
             var sky = data.FirstOrDefault(p => p[0] == "SKY_RecepcionFacturas");
            if (comp != null)
            {
                if (sky != null && sky.Length > 0)
                {
                    SKY_RecepcionFacturas addenda = new SKY_RecepcionFacturas();
                    addenda.NumAcreedor = ParserNtLink.GetValue(sky[1]);

                    addenda.TipoFacturaProveedor =
                        (SKY_RecepcionFacturasTipoFacturaProveedor)
                            Enum.Parse(typeof(SKY_RecepcionFacturasTipoFacturaProveedor), sky[2]);

                    addenda.CodigoFacturacion = ParserNtLink.GetValue(sky[3]);
                    addenda.NumOrdenCompras = ParserNtLink.GetValue(sky[4]);
                    if (!string.IsNullOrEmpty(sky[5]))
                    {
                        addenda.Sistema = (SKY_RecepcionFacturasSistema)
                            Enum.Parse(typeof(SKY_RecepcionFacturasSistema), sky[5]);
                        addenda.SistemaSpecified = true;
                    }
                    addenda.PersonaContacto = ParserNtLink.GetValue(sky[7]);
                    addenda.NumRefSis = ParserNtLink.GetValue(sky[8]);
                    addenda.NumPedimento = ParserNtLink.GetValue(sky[9]);
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(addenda, typeof(SKY_RecepcionFacturas), null, null);
                    comp.XmlAdenda = comp.XmlAdenda.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ", "");
                }
                
                res.Add(comp);
            }
            return res;
        }

        
    }
}
