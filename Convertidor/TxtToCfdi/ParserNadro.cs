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
using PPY;


namespace TxtToCfdi
{
    public class ParserNadro : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserNadro));



        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetNadroAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(AddendaNadro.Adenda), "", "");
                  //  xmlAdenda = xmlAdenda.Replace("<Adenda xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "");
                  //  xmlAdenda = xmlAdenda.Replace("</Adenda>","");
                   
                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private AddendaNadro.Adenda GetNadroAdenda(string[][] data)
        {
            #region GetData
            var Datos = data.Where(j => j[0] == "DatosNadro");//mul-op
          

            #endregion
           
            
            if (Datos.Any())
            {
                AddendaNadro.Adenda A=new AddendaNadro.Adenda();
                List<AddendaNadro.AdendaDatosNadro> Nadro = new List<AddendaNadro.AdendaDatosNadro>();

                foreach (var d in Datos)
                {
                    AddendaNadro.AdendaDatosNadro c = new AddendaNadro.AdendaDatosNadro();
                    c.Orden = d[1];
                    c.Plazo = d[2];
                    c.EntregaEntrante = d[3];
                    c.PosicionOC = d[4];
                    c.TotalOC = d[5];
                    c.CodEAN = d[6];
                    Nadro.Add(c);               
            
                }

                A.DatosNadro = Nadro.ToArray();
                return A;
            }
            else
            return null;

        }
    }
}
