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
    public class ParserPPY : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserPPY));



        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetPPYAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(PPY.factura), "", "http://www.dfdchryslerdemexico.com.mx/Addenda/PPY");

                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private PPY.factura GetPPYAdenda(string[][] data)
        {
            #region GetData
            var pua=data.FirstOrDefault(j => j[0] == "PPY");//uni-req
            var Cancelacion = data.Where(j => j[0] == "CANCEL");//mul-op
            var moneda = data.FirstOrDefault(j => j[0] == "MONEDA");//uni-req
            var proveedor = data.FirstOrDefault(j => j[0] == "PROVEEDOR");//uni-req
            var origen = data.FirstOrDefault(j => j[0] == "ORIGEN");//uni-op
            var destino = data.FirstOrDefault(j => j[0] == "DESTINO");//uni-req
            var receiving = data.FirstOrDefault(j => j[0] == "RECEIVING");//uni-op
            var proyecto = data.FirstOrDefault(j => j[0] == "PROYECTOPPY");//uni-op
            var nota = data.Where(j => j[0] == "NOTAPPY");//mul-op
            var cargosCreditos=data.Where(j => j[0] == "CARGOSCREDITOS");//mul-op
            var otrosCargos=data.Where(j => j[0] == "OTROSCARGOS");//mul-req
            var partes=data.Where(j => j[0] == "PARTES");//mul-req ---numero identificador de parte
            var partesNota = data.Where(j => j[0] == "PARTESNOTA");//mul-op

            #endregion
            if(pua!=null)
            {

                PPY.factura PPY1 = new PPY.factura();

            if (Cancelacion.Any())
            {

                List<PPY.facturaCancelaciones> CAN = new List<PPY.facturaCancelaciones>();

                foreach (var can in Cancelacion)
                {
                    PPY.facturaCancelaciones c = new PPY.facturaCancelaciones();
                    c.CancelaSustituye = can[1];
                    CAN.Add(c);               
            
                }

                PPY1.Cancelaciones = CAN.ToArray();
            }
            //-------------------------------------------
                PPY1.moneda=new PPY.facturaMoneda();
                PPY1.moneda.tipoMoneda = moneda[1];
                
                if(!string.IsNullOrEmpty(moneda[2]))
                {
                    PPY1.moneda.tipoCambioSpecified = true;
                    PPY1.moneda.tipoCambio = Convert.ToDecimal(moneda[2]);
                }
                else
                    PPY1.moneda.tipoCambioSpecified = false;
                //---------------------------------------------
                PPY1.proveedor = new PPY.Locacion();
                PPY1.proveedor.codigo = proveedor[1];
                PPY1.proveedor.nombre = proveedor[2];
                if(!string.IsNullOrEmpty(proveedor[3]))
                    PPY1.proveedor.sufijo = proveedor[3];
                //--------------------------------------------
                if(origen!=null)
                {
                    PPY1.origen = new PPY.Locacion();
                   PPY1.origen.codigo=origen[1];
                   PPY1.origen.nombre = origen[2];
                    if(!string.IsNullOrEmpty(origen[3]))
                        PPY1.origen.sufijo = origen[3];

                }
                //------------------------------------------------
                PPY1.destino = new PPY.Locacion();
                PPY1.destino.codigo = destino[1];
                PPY1.destino.nombre = destino[2];
                if(!string.IsNullOrEmpty(destino[3]))
                    PPY1.destino.sufijo = destino[3];
                //------------------------------------------------------
                  if(receiving!=null)
                {
                    PPY1.receiving = new PPY.Locacion();
                    PPY1.receiving.codigo = receiving[1];
                    PPY1.receiving.nombre = receiving[2];
                    if(!string.IsNullOrEmpty(receiving[3]))
                        PPY1.receiving.sufijo = receiving[3];

                }
                //------------------------------------------------
                  if (proyecto != null)
                  {
                      PPY1.proyecto = new PPY.facturaProyecto();
                      PPY1.proyecto.numero=proyecto[1];
                      PPY1.proyecto.numeroTrabajo = proyecto[2];
                      if(!string.IsNullOrEmpty(proyecto[3]))
                      PPY1.proyecto.chargeUnit = proyecto[3];
                  }
                
                //----------------------------------------------
              if (nota.Any())
            {
               
                List<string> NOT = new List<string>();

                foreach (var not in nota)
                {
                    string no=not[0];
                      NOT.Add(no);               
            
                }

                PPY1.nota = NOT.ToArray();
            }
                //------------------------------------------------
                if(cargosCreditos.Any())
                {
                   List<PPY.facturaCargosCreditos>CC=new List<PPY.facturaCargosCreditos>();

                   foreach (var cc in cargosCreditos)
                   {
                    PPY.facturaCargosCreditos c = new PPY.facturaCargosCreditos();
                    c.referenciaChrysler = cc[1];
                    c.consecutivo=cc[2];
                    c.montoLinea=Convert.ToDecimal(cc[3]);
                     if(!string.IsNullOrEmpty(cc[4]))
                       c.factura=cc[4];
                       c.archivo=cc[5];

                      CC.Add(c);               
            
                    }
                    PPY1.cargosCreditos=CC.ToArray();
                   
                }
                //-------------------------------------------------------
                if(otrosCargos.Any())
                {
                  
                     List<PPY.facturaOtrosCargos>OC=new List<PPY.facturaOtrosCargos>();
                     foreach (var oc in otrosCargos)
                     {
                         PPY.facturaOtrosCargos O=new PPY.facturaOtrosCargos();
                         if(oc[1]=="V0")
                             O.codigo = PPY.facturaOtrosCargosCodigo.V0;
                                                  if(oc[1]=="V1")
                         O.codigo= PPY.facturaOtrosCargosCodigo.V1;
                                                  if(oc[1]=="V4")
                         O.codigo= PPY.facturaOtrosCargosCodigo.V4;
                                                  if(oc[1]=="V6")
                         O.codigo= PPY.facturaOtrosCargosCodigo.V6;
                                                     
                         O.monto=Convert.ToDecimal(oc[2]);
                         OC.Add(O);
                     }
                    PPY1.otrosCargos=OC.ToArray();
                }
                //-------------------------------------------------------------
                if (partes.Any())
                {
                 
                    List<PPY.facturaPart> P = new List<PPY.facturaPart>();
                    foreach (var p in partes)
                    {
                        PPY.facturaPart FP = new PPY.facturaPart();
                        FP.references = new PPY.facturaPartReferences();
                        FP.references.ordenCompra = p[2];
                        if (!string.IsNullOrEmpty(p[3]))
                            FP.references.releaseRequisicion = p[3];
                        FP.references.ammendment = p[4];
                        if (!string.IsNullOrEmpty(p[5]))
                            FP.references.packingList = p[5];

                        //------------------------------------------------------
                        
                        //---------------------------------------
                        #region Definicion de partes nota
                        if (partesNota.Any())
                        {
                            var partesNotax = partesNota.Where(j => (j[1] == p[1]));
                            if (partesNotax.Count() > 0) //opciional
                            {
                                List<string> NOT2 = new List<string>();

                                foreach (var not2 in partesNotax)
                                {
                                    string no = not2[0];
                                    NOT2.Add(no);

                                }

                                FP.nota = NOT2.ToArray();
                            }

                        }
                        #endregion
                        //----------------------------------------------
                        if(!string.IsNullOrEmpty(p[6]))
                        FP.numero = p[6];
                        FP.cantidad = Convert.ToDecimal(p[7]);
                        FP.unidadDeMedida = p[8];
                        FP.precioUnitario = Convert.ToDecimal(p[9]);
                        FP.montoDeLinea = Convert.ToDecimal(p[10]);
                        if (!string.IsNullOrEmpty(p[11]))
                        FP.fechaRecibo =Convert.ToDateTime(p[11]);

                        P.Add(FP);
                    }
                    PPY1.partes = P.ToArray();
                }
                //-------------------------------------------------
                PPY1.tipoDocumento = PPY.facturaTipoDocumento.PPY;
                PPY1.TipoDocumentoFiscal = pua[1];
                PPY1.version = pua[2];
                PPY1.serie=pua[3];
                PPY1.folioFiscal=pua[4];
                PPY1.fecha=Convert.ToDateTime(pua[5]);
                PPY1.montoTotal=Convert.ToDecimal(pua[6]);
                PPY1.referenciaProveedor=pua[7];

                return PPY1;
            }
            else
            return null;

        }
    }
}
