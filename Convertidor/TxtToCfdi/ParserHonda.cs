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
    public class ParserHonda : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserHonda));



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
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(Honda), "GPC", "http://www.honda.net.mx/GPC");

                        xmlAdenda = xmlAdenda.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");


                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private Honda GetTridonexAdenda(string[][] data)
        {

            var cabeceraHonda = data.FirstOrDefault(j => j[0] == "Honda");
            var Proveedor = data.FirstOrDefault(j => j[0] == "Proveedor");
            var Conceptos = data.Where(j => j[0] == "Concepto");

            if (cabeceraHonda != null)
            {
                Honda H = new Honda();
                H.fecha = cabeceraHonda[1];
                H.folio = cabeceraHonda[2];
                H.ASNNumber = cabeceraHonda[3];
                if(cabeceraHonda[4]=="GDL")
                H.PlantCode =HondaPlantCode.GDL;
                if (cabeceraHonda[4] == "HCL")
                H.PlantCode = HondaPlantCode.HCL;
                if (cabeceraHonda[4] == "MPS")
                H.PlantCode = HondaPlantCode.MPS;
                if (cabeceraHonda[4] == "MTP")
                H.PlantCode = HondaPlantCode.MTP;

                H.moneda = cabeceraHonda[5];
                if (cabeceraHonda[6]=="E")
                H.tipoComprobante = HondaTipoComprobante.E ;
                if (cabeceraHonda[6] == "I")
                H.tipoComprobante = HondaTipoComprobante.I;
                if (cabeceraHonda[7] == "GPC")
                H.tipoDocumento = HondaTipoDocumento.GPC;
                if (!string.IsNullOrEmpty(cabeceraHonda[8]))//opcional
                H.ReferenceNumber = cabeceraHonda[8];
                //----------------------------
                if (Proveedor != null)
                {
                    //H.Proveedor = new HondaProveedor();
                    HondaProveedor p = new HondaProveedor();
                    p.rfc = Proveedor[1];
                    p.ShipTo = Proveedor[2];
                    p.numeroProveedor = Proveedor[3];
                    H.Proveedor = p;
                                   
                }
                
                if (Conceptos.Any())
                {
                  List<HondaConcepto> listaHondaConceptos = new List<HondaConcepto>();
                                                                       
                  foreach (var concepto in Conceptos)
                  {
                      HondaConcepto C = new HondaConcepto();
                      C.importe =  decimal.Parse(ParserNtLink.GetValue(concepto[1]));
                      C.valorUnitario = decimal.Parse(ParserNtLink.GetValue(concepto[2]));
                      C.descripcion = ParserNtLink.GetValue(concepto[3]);
                      C.partnumber = ParserNtLink.GetValue(concepto[4]);
                      C.cantidad = int.Parse(ParserNtLink.GetValue(concepto[5]));
                      C.nolinea = int.Parse(ParserNtLink.GetValue(concepto[6]));
                      if (!string.IsNullOrEmpty(concepto[7]))
                      C.partcolor = ParserNtLink.GetValue(concepto[7]); //opcional

                      listaHondaConceptos.Add(C);
                                       
                  }
                   H.Conceptos=listaHondaConceptos.ToArray();

                }

                return H;


            }
            return null;

        }
    }
}
