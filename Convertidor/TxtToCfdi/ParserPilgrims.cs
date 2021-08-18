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
  public  class ParserPilgrims:IParser
    {

      private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserPilgrims));



        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetPilgrimsAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(Pilgrims), "", "");

                        xmlAdenda = xmlAdenda.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");


                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private Pilgrims GetPilgrimsAdenda(string[][] data)
        {

            var PIL = data.FirstOrDefault(j => j[0] == "PILGRIMS");
            var Conceptos = data.Where(j => j[0] == "PARTIDA"); 
            var   boletas = data.Where(j => j[0] == "BOLETAS");

            if (PIL != null)
            {
                Pilgrims P = new Pilgrims();

                if(!string.IsNullOrEmpty(PIL[2]))
                P.Comprador=PIL[2];
                P.Proceso=PIL[3];
                P.Proveedor=PIL[1];
        
                if(Conceptos.Any())
                {
                    
                    List<PilgrimsPartida> listaConceptos = new List<PilgrimsPartida>();
                                                                                       
                  foreach (var concepto in Conceptos)
                  {
                      PilgrimsPartida C = new PilgrimsPartida();
                      C.Pedido =  ParserNtLink.GetValue(concepto[1]);
                      C.Posicion= ParserNtLink.GetValue(concepto[2]);
                      if (!string.IsNullOrEmpty(concepto[3]))
                      C.Material=ParserNtLink.GetValue(concepto[3]);
                      if(!string.IsNullOrEmpty(concepto[4]))
                      {
                      C.CantidadSpecified=true;
                      C.Cantidad=decimal.Parse(ParserNtLink.GetValue(concepto[4]));
                      }
                      else
                          C.CantidadSpecified=false;
                      if(!string.IsNullOrEmpty(concepto[5]))
                      {
                      
                      C.PrecioSpecified=true;
                      C.Precio=decimal.Parse(ParserNtLink.GetValue(concepto[5]));
                      }
                      else
                      C.PrecioSpecified=false;
                      if (!string.IsNullOrEmpty(concepto[6]))
                      C.Entrada=ParserNtLink.GetValue(concepto[6]);

                      C.Referencia=ParserNtLink.GetValue(concepto[7]);
                      if (!string.IsNullOrEmpty(concepto[8]))
                      C.Pedimento=ParserNtLink.GetValue(concepto[8]);
                      if (!string.IsNullOrEmpty(concepto[9]))
                      C.UdeM=ParserNtLink.GetValue(concepto[9]);
                      if (!string.IsNullOrEmpty(concepto[10]))
                      C.FacturaPedimento=ParserNtLink.GetValue(concepto[10]);

                      
                      PilgrimsPartidaBoletas bolet = new PilgrimsPartidaBoletas();
                      bolet.Text = new string[1];
                      bolet.Text[0] = concepto[11];
                      
                      /*
                      foreach (var boleta in boletas)
                      {
                          if (boleta[1] == concepto[1])
                          {
                              bolet.Text = new string[1];

                              bolet.Text[0]= boleta[2];
                             
                          }
                      }
                      if (bolet.Boleta.Count() > 0)
                      
                       */
                      C.Boletas = bolet;
                      

                      listaConceptos.Add(C);
                                       
                  }
                   P.Partida=listaConceptos.ToArray();

                 
                }//fin_conceptos
                return P;
            }
            return null;

        
        }
    }
}
