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
    public class ParserGM : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserGM));



        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetGMAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(ADDENDAGM), "", "");
                    xmlAdenda = xmlAdenda.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                                                     
                       comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private ADDENDAGM GetGMAdenda(string[][] data)
        {

            var HEADER = data.FirstOrDefault(j => j[0] == "HEADERGM");
            var ITEM = data.Where(j => j[0] == "ITEMGM");

            if (HEADER != null)
            {
                ADDENDAGM H = new ADDENDAGM();
                H.HEADER=new ADDENDAGMHEADER();
                H.HEADER.FECHARECIBO=HEADER[1];
                H.HEADER.FOLIOINTERNO=HEADER[2];
                if (HEADER[3] == "1")
                    H.HEADER.MONEDA = ADDENDAGMHEADERMONEDA.Item1;
                if (HEADER[3] == "2")
                    H.HEADER.MONEDA = ADDENDAGMHEADERMONEDA.Item2;
                if (HEADER[3] == "3")
                    H.HEADER.MONEDA = ADDENDAGMHEADERMONEDA.Item3;
                if (HEADER[3] == "4")
                    H.HEADER.MONEDA = ADDENDAGMHEADERMONEDA.Item4;
                if (HEADER[3] == "5")
                    H.HEADER.MONEDA = ADDENDAGMHEADERMONEDA.Item5;

                H.HEADER.NUMEROREMISION=HEADER[4];
               

              
                
                if (ITEM.Any())
                {
                    // H.HEADER.ITEM

                    
                  List<ADDENDAGMHEADERITEM> lista = new List<ADDENDAGMHEADERITEM>();
                                                                       
                  foreach (var i in ITEM)
                  {
                      ADDENDAGMHEADERITEM C = new ADDENDAGMHEADERITEM();
                      C.CANTIDAD =  decimal.Parse(ParserNtLink.GetValue(i[1]));
                      C.DESCRIPCION= ParserNtLink.GetValue(i[2]);
                      if (i[3] == "1")
                          C.MATERIAL = ADDENDAGMHEADERITEMMATERIAL.Item1;
                      if (i[3] == "2")
                          C.MATERIAL = ADDENDAGMHEADERITEMMATERIAL.Item2;
                      if (i[3] == "3")
                          C.MATERIAL = ADDENDAGMHEADERITEMMATERIAL.Item3;

                      C.NUMEROPARTE= ParserNtLink.GetValue(i[4]);
                      C.ORDENCOMPRA = ParserNtLink.GetValue(i[5]);
                      C.PRECIOUNITARIO = decimal.Parse(ParserNtLink.GetValue(i[6]));
                 
                      lista.Add(C);
                                       
                  }
                  H.HEADER.ITEM = lista.ToArray();

                }

                return H;


            }
            return null;

        }
    }
}
