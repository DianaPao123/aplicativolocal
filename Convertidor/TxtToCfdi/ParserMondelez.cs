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
    public class ParserMondelez : IParser
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserMondelez));



        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetMondelezAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(Inicial), "", "");

                        xmlAdenda = xmlAdenda.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");


                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private Inicial GetMondelezAdenda(string[][] data)
        {

            var inicio = data.FirstOrDefault(j => j[0] == "INICIO");
            var emisor = data.FirstOrDefault(j => j[0] == "E");
            var emisorD = data.FirstOrDefault(j => j[0] == "DE");

            var entrega = data.FirstOrDefault(j => j[0] == "R");
            var entregaD = data.FirstOrDefault(j => j[0] == "DR");
            var Conceptos = data.Where(j => j[0] == "C");

            if (inicio != null)
            {
                Inicial I = new Inicial();
                I.OrdenCompra = inicio[1];
                I.NumeroGr =Convert.ToInt64( inicio[2]);
             
                if (!string.IsNullOrEmpty(inicio[3]))
                        I.NotaEntrega=inicio[3];
                if (!string.IsNullOrEmpty(inicio[4]))
                {
                    I.FechaCreacionOrdenCompraSpecified = true;
                    I.FechaCreacionOrdenCompra = Convert.ToInt64(inicio[4]);
                }
                else
                    I.FechaCreacionOrdenCompraSpecified = false;
                if (!string.IsNullOrEmpty(inicio[5]))
                  I.NumeroFacturaOriginal = inicio[5];
                I.NombreContactoCliente = inicio[6];
                I.CorreoContactoCliente = inicio[7];
            
                //----------------------------
                if (emisor != null && emisorD!=null)
                {
                    I.DireccionEmision = new InicialDireccionEmision();
                    I.DireccionEmision.CalleEmision = emisorD[1]+" "+ emisorD[2]+" "+emisorD[3];
                    I.DireccionEmision.EstadoEmision = emisorD[6];
                    I.DireccionEmision.MunicipioEmision = emisorD[5];
                    I.DireccionEmision.ColoniaEmision = emisorD[4];
                    I.DireccionEmision.CodigoPostal = emisorD[8];
                    I.DireccionEmision.NombreProveedor = emisor[2];

                                   
                }
                //----------------------------
                if (entrega != null)
                {
                    I.DireccionEntrega = new  InicialDireccionEntrega();
                    I.DireccionEntrega.CalleEntrega = entregaD[1] + " " + entregaD[2] + " " + entregaD[3];
                    I.DireccionEntrega.EstadoEntrega = entregaD[6];
                    I.DireccionEntrega.MunicipioEntrega = entregaD[5];
                    I.DireccionEntrega.ColoniaEntrega = entregaD[4];
                    I.DireccionEntrega.CodigoPostal =entregaD[8];
                    I.DireccionEntrega.NombreCliente = entrega[2];


                }
                //----------------------------------------------------------
                if (Conceptos.Any())
                {
                    List<InicialDetalle> listaConceptos = new List<InicialDetalle>();
                                                                                       
                  foreach (var concepto in Conceptos)
                  {
                      InicialDetalle C = new InicialDetalle();
                      C.CodigoProductoCliente =  ParserNtLink.GetValue(concepto[12]);
                      C.CodigoProductoProveedor = ParserNtLink.GetValue(concepto[1]);
                      if (!string.IsNullOrEmpty(concepto[3]))
                      {
                          C.NoItemSpecified = true;
                          C.NoItem = int.Parse(ParserNtLink.GetValue(concepto[11]));
                      }
                      else
                          C.NoItemSpecified = false;

                     
                      listaConceptos.Add(C);
                                       
                  }
                   I.Detalle=listaConceptos.ToArray();

                }

                return I;


            }
            return null;

        }
    }
}
