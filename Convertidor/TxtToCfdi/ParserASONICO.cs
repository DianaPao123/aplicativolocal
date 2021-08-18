using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AddendaBic;
using GeneradorCfdi;

namespace TxtToCfdi
{
    public class ParserASONICO : IParser
    {
        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetASONICOAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(ASONIOSCOC), "", "");
                    xmlAdenda = xmlAdenda.Replace("ASONIOSCOC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "ASONIOSCOC");
               
                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private ASONIOSCOC GetASONICOAdenda(string[][] data)
        {
            #region GetData
            var adde = data.FirstOrDefault(j => j[0] == "ASONICO");//uni-req
            var partidas = data.Where(j => j[0] == "PartidaASON");//mul-op


            #endregion
            if (adde != null)
            {

                ASONIOSCOC A = new ASONIOSCOC();
                A.tipoProveedor = Convert.ToInt32(adde[1]);
                A.noProveedor = adde[2];
                A.serie = adde[3];
                A.folio = adde[4];
                A.ordenCompra = adde[5];
                //-------------------------------------------
                if (partidas.Any())
                {

                    List<ASONIOSCOCPartida> PAR = new List<ASONIOSCOCPartida>();

                    foreach (var p in partidas)
                    {
                        ASONIOSCOCPartida par = new ASONIOSCOCPartida();
                        par.noPartida = Convert.ToInt32(p[1]);
                        par.ivaAcreditable = Convert.ToDecimal(p[2]);
                        par.ivaDevengado = Convert.ToDecimal(p[3]);
                        par.Otros = p[4];
                        PAR.Add(par);

                    }

                    A.Partidas = PAR.ToArray();
                }
                return A;

            }
            else
                return null;
        }
    }
}
