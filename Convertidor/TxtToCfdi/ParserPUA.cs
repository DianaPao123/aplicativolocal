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
    public class ParserPUA : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserPUA));



        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetPUAAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(factura), "", "http://www.dfdchryslerdemexico.com.mx/Addenda/PUA");

                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private factura GetPUAAdenda(string[][] data)
        {
            #region GetData
            var pua=data.FirstOrDefault(j => j[0] == "PUA");//uni-req
            var Cancelacion = data.Where(j => j[0] == "CANCEL");//mul-op
            var moneda = data.FirstOrDefault(j => j[0] == "MONEDA");//uni-req
            var proveedor = data.FirstOrDefault(j => j[0] == "PROVEEDOR");//uni-req
            var origen = data.FirstOrDefault(j => j[0] == "ORIGEN");//uni-op
            var destino = data.FirstOrDefault(j => j[0] == "DESTINO");//uni-req
            var receiving = data.FirstOrDefault(j => j[0] == "RECEIVING");//uni-op
            var nota = data.Where(j => j[0] == "NOTAPUA");//mul-op
            var cargosCreditos=data.Where(j => j[0] == "CARGOSCREDITOS");//mul-op
            var otrosCargos=data.Where(j => j[0] == "OTROSCARGOS");//mul-req
            var partes=data.Where(j => j[0] == "PARTES");//mul-req ---numero identificador de parte
            var partesOC = data.Where(j => j[0] == "PARTESOC");//mul-op
            var partesNota = data.Where(j => j[0] == "PARTESNOTA");//mul-op

            #endregion
            if(pua!=null)
            {
               
                factura PUA = new factura();

            if (Cancelacion.Any())
            {
               
                List<facturaCancelaciones> CAN = new List<facturaCancelaciones>();

                foreach (var can in Cancelacion)
                {
                    facturaCancelaciones c = new facturaCancelaciones();
                    c.CancelaSustituye = can[1];
                    CAN.Add(c);               
            
                }

                PUA.Cancelaciones = CAN.ToArray();
            }
            //-------------------------------------------
                PUA.moneda=new facturaMoneda();
                PUA.moneda.tipoMoneda=moneda[1];
                
                if(!string.IsNullOrEmpty(moneda[2]))
                {
                PUA.moneda.tipoCambioSpecified=true;
                PUA.moneda.tipoCambio=Convert.ToDecimal(moneda[2]);
                }
                else
                    PUA.moneda.tipoCambioSpecified=false;
                //---------------------------------------------
                PUA.proveedor=new Locacion();
                PUA.proveedor.codigo=proveedor[1];
                PUA.proveedor.nombre=proveedor[2];
                if(!string.IsNullOrEmpty(proveedor[3]))
                PUA.proveedor.sufijo=proveedor[3];
                //--------------------------------------------
                if(origen!=null)
                {
                   PUA.origen=new Locacion();
                   PUA.origen.codigo=origen[1];
                    PUA.origen.nombre=origen[2];
                    if(!string.IsNullOrEmpty(origen[3]))
                     PUA.origen.sufijo=origen[3];

                }
                //------------------------------------------------
                 PUA.destino=new Locacion();
                PUA.destino.codigo=destino[1];
                PUA.destino.nombre=destino[2];
                if(!string.IsNullOrEmpty(destino[3]))
                PUA.destino.sufijo=destino[3];
                //------------------------------------------------------
                  if(receiving!=null)
                {
                   PUA.receiving=new Locacion();
                   PUA.receiving.codigo=receiving[1];
                    PUA.receiving.nombre=receiving[2];
                    if(!string.IsNullOrEmpty(receiving[3]))
                     PUA.receiving.sufijo=receiving[3];

                }
                //------------------------------------------------
              if (nota.Any())
            {
               
                List<string> NOT = new List<string>();

                foreach (var not in nota)
                {
                    string no=not[0];
                      NOT.Add(no);               
            
                }

                PUA.nota = NOT.ToArray();
            }
                //------------------------------------------------
                if(cargosCreditos.Any())
                {
                   List<facturaCargosCreditos>CC=new List<facturaCargosCreditos>();

                   foreach (var cc in cargosCreditos)
                   {
                    facturaCargosCreditos c = new facturaCargosCreditos();
                    c.referenciaChrysler = cc[1];
                    c.consecutivo=cc[2];
                    c.montoLinea=Convert.ToDecimal(cc[3]);
                     if(!string.IsNullOrEmpty(cc[4]))
                       c.factura=cc[4];
                       c.archivo=cc[5];

                      CC.Add(c);               
            
                    }
                    PUA.cargosCreditos=CC.ToArray();
                   
                }
                //-------------------------------------------------------
                if(otrosCargos.Any())
                {
                  
                     List<facturaOtrosCargos>OC=new List<facturaOtrosCargos>();
                     foreach (var oc in otrosCargos)
                     {
                         facturaOtrosCargos O=new facturaOtrosCargos();
                         if(oc[1]=="V0")
                         O.codigo= facturaOtrosCargosCodigo.V0;
                                                  if(oc[1]=="V1")
                         O.codigo= facturaOtrosCargosCodigo.V1;
                                                  if(oc[1]=="V4")
                         O.codigo= facturaOtrosCargosCodigo.V4;
                                                  if(oc[1]=="V6")
                         O.codigo= facturaOtrosCargosCodigo.V6;
                                                     
                         O.monto=Convert.ToDecimal(oc[2]);
                         OC.Add(O);
                     }
                    PUA.otrosCargos=OC.ToArray();
                }
                //-------------------------------------------------------------
                if (partes.Any())
                {
                 
                    List<facturaPart> P = new List<facturaPart>();
                    foreach (var p in partes)
                    {
                        facturaPart FP = new facturaPart();
                        FP.references = new facturaPartReferences();
                        FP.references.ordenCompra = p[2];
                        if (!string.IsNullOrEmpty(p[3]))
                            FP.references.releaseRequisicion = p[3];
                        FP.references.ammendment = p[4];
                        if (!string.IsNullOrEmpty(p[5]))
                            FP.references.packingList = p[5];

                        //------------------------------------------------------
                        #region Definicion de partes otros cargos

                        if (partesOC.Any())
                        {
                            var pratesOCx = partesOC.Where(j => (j[1] == p[1]));
                            if (pratesOCx.Count() > 0) //opciional
                            {
                                List<facturaPartOtrosCargos> fpoc = new List<facturaPartOtrosCargos>();
                                foreach (var poc in pratesOCx)
                                {
                                    facturaPartOtrosCargos fpoc1 = new facturaPartOtrosCargos();
                                    fpoc1.codigo = facturaPartOtrosCargosCodigo.P ;
                                    fpoc1.monto =Convert.ToDecimal(poc[2]);

                                    fpoc.Add(fpoc1);
                                }

                                FP.otrosCargos = fpoc.ToArray();
                            }
                        }
                        #endregion
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
                    PUA.partes = P.ToArray();
                }
                //-------------------------------------------------
                PUA.tipoDocumento =  facturaTipoDocumento.PUA;
                PUA.TipoDocumentoFiscal = pua[1];
                PUA.version = pua[2];
                PUA.serie=pua[3];
                PUA.folioFiscal=pua[4];
                PUA.fecha=Convert.ToDateTime(pua[5]);
                PUA.montoTotal=Convert.ToDecimal(pua[6]);
                PUA.referenciaProveedor=pua[7];

                return PUA;
            }
            else
            return null;

        }
    }
}
