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
    public class Parser1888 : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Parser1888));



        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.Get1888Adenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(NUMEROPEDIMENTO), "", "");
                    xmlAdenda = xmlAdenda.Replace("<NUMEROPEDIMENTO xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "<NUMEROPEDIMENTO>");
                    xmlAdenda = xmlAdenda.Replace("<GENERAL_x0020_NUMPED>"+adenda.GENERAL_NUMPED+"</GENERAL_x0020_NUMPED>", "<GENERAL NUMPED=\""+adenda.GENERAL_NUMPED+"\"/>");
                   
                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private NUMEROPEDIMENTO Get1888Adenda(string[][] data)
        {
            #region GetData
            var Datos = data.FirstOrDefault(j => j[0] == "NUMEROPEDIMENTO");//mul-op
          

            #endregion
           
            
            if (Datos!=null)
            {
                NUMEROPEDIMENTO A = new NUMEROPEDIMENTO();
              
                A.GENERAL_NUMPED = Datos[1];
                return A;
            }
            else
            return null;

        }
    }
}
