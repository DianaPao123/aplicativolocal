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
    public class ParserleyendasFisc : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LeyendasFiscales));
        

        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetLeyendasFiscalesAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(LeyendasFiscales), null, null);

                       xmlAdenda = xmlAdenda.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                    
                       comp.XmlAdenda = xmlAdenda;

                }
                res.Add(comp);
            }
            return res;
        }
        //-------------------------------------------------------------------
        private LeyendasFiscales GetLeyendasFiscalesAdenda(string[][] data)
        {

            var cabeceraLeyendasFiscales = data.FirstOrDefault(j => j[0] == "LeyendasFiscales");
            var disposicionFiscal = data.Where(j => j[0] == "LeyendasFiscLeyenda");
           
            if (cabeceraLeyendasFiscales != null)
            {
                LeyendasFiscales H = new LeyendasFiscales();
                H.version = cabeceraLeyendasFiscales[1];
                List<LeyendasFiscalesLeyenda> LFL = new List<LeyendasFiscalesLeyenda>();


                foreach (var can in disposicionFiscal)
                {
                    LeyendasFiscalesLeyenda L= new LeyendasFiscalesLeyenda();

                    L.disposicionFiscal = can[1];
                    L.norma = can[2];
                    L.textoLeyenda = can[3];
                    LFL.Add(L);
   
                }
                    H.Leyenda = LFL.ToArray();

                
                return H;


            }
            return null;

        }
        //---------------------------------------------------------------------------------------
    }
}
