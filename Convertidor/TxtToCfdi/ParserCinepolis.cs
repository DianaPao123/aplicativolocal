using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using GeneradorCfdi;

namespace TxtToCfdi
{
    public class ParserCinepolis: IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserNtLink));

        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);
            // parsear parte de la addenda

            if (comp != null)
            {
                var CinepolisRecepcion = data.FirstOrDefault(p => p[0] == "CinepolisRecepcion");
                if (CinepolisRecepcion != null && CinepolisRecepcion.Length > 0)
                {
                    CinepolisRecepcion cinRep = new CinepolisRecepcion();
                    try
                    {
                        cinRep.idProveedor = ParserNtLink.GetValue(CinepolisRecepcion[1]);
                        cinRep.NotaRecepcion = ParserNtLink.GetValue(CinepolisRecepcion[2]);
                        cinRep.Contrato = ParserNtLink.GetValue(CinepolisRecepcion[3]);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e.Message);
                    }
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(cinRep, typeof (CinepolisRecepcion),
                        null, null);
                    comp.XmlAdenda =
                        comp.XmlAdenda.Replace(
                            "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ",
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
