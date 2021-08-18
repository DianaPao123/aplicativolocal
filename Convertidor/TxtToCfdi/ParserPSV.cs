using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AddendaBic;
using GeneradorCfdi;

namespace TxtToCfdi
{
    public class ParserPSV : IParser
    {
        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetPSVAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(PSV.Factura), "PSV", "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV");
                   // xmlAdenda = xmlAdenda.Replace("ASONIOSCOC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "ASONIOSCOC");
                    xmlAdenda = xmlAdenda.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private PSV.Factura GetPSVAdenda(string[][] data)
        {
            #region GetData
            var adde = data.FirstOrDefault(j => j[0] == "PSVFACTURA");//uni-req
            var mon = data.FirstOrDefault(j => j[0] == "PSVMONEDA");//uni-req
            var pro = data.FirstOrDefault(j => j[0] == "PSVPROVEEDOR");//uni-req
            var refe = data.FirstOrDefault(j => j[0] == "PSVREFERENCIAS");//uni-req
            var sol = data.FirstOrDefault(j => j[0] == "PSVSOLICITANTE");//uni-req
            var can = data.FirstOrDefault(j => j[0] == "PSVCANCELACIONES");//uni-req
            var ori = data.FirstOrDefault(j => j[0] == "PSVORIGEN");//uni-req
            var des = data.FirstOrDefault(j => j[0] == "PSVDESTINO");//uni-req
            var med = data.FirstOrDefault(j => j[0] == "PSVMEDIDAS");//uni-req
           
            var not = data.Where(j => j[0] == "PSVNOTA");//mul-op
            var arc = data.Where(j => j[0] == "PSVARCHIVO");//mul-op
            var par = data.Where(j => j[0] == "PSVPARTE");//mul-op
            var pn = data.Where(j => j[0] == "PSVPARTENOTA");//mul-op
            
            #endregion
            
            if (adde != null)
            {

                PSV.Factura A = new PSV.Factura();
                A.version = "1.0";
                A.tipoDocumentoFiscal = (PSV.FacturaTipoDocumentoFiscal)Enum.Parse(typeof(PSV.FacturaTipoDocumentoFiscal), ParserNtLink.GetValue(adde[1]));
                A.tipoDocumentoVWM = (PSV.FacturaTipoDocumentoVWM)Enum.Parse(typeof(PSV.FacturaTipoDocumentoVWM), ParserNtLink.GetValue(adde[2]));
                A.division = (PSV.FacturaDivision)Enum.Parse(typeof(PSV.FacturaDivision), ParserNtLink.GetValue(adde[3]));

                
                //-------------------------------------------
                // if (mon != null)
                {
                    PSV.FacturaMoneda M = new PSV.FacturaMoneda();
                    M.codigoImpuesto = mon[1];
                    if (string.IsNullOrEmpty(mon[2]))
                        M.tipoCambioSpecified = false;
                    else
                    {
                        M.tipoCambioSpecified = true;
                        M.tipoCambio = Convert.ToDecimal(mon[2]);
                    }
                    M.tipoMoneda = mon[3];

                    A.Moneda = M;
                }
                //   if (pro != null)
                {
                    PSV.FacturaProveedor P = new PSV.FacturaProveedor();
                    P.codigo = pro[1];
                    P.correoContacto = pro[2];
                    P.nombre = pro[3];
                    A.Proveedor = P;
                }
                // if (refe != null)
                {
                    PSV.FacturaReferencias R = new PSV.FacturaReferencias();
                    R.numeroASN = refe[1];
                    R.referenciaProveedor = refe[2];
                    if (!string.IsNullOrEmpty(refe[3]))
                        R.remision = refe[3];
                    R.unidadNegocios = refe[4];
                    A.Referencias = R;
                }
                // if (sol != null)
                {
                    PSV.FacturaSolicitante S = new PSV.FacturaSolicitante();
                    S.correo = sol[1];
                    S.nombre = sol[2];
                    A.Solicitante = S;
                }
                 if (can != null)
                {
                    PSV.FacturaCancelaciones C = new PSV.FacturaCancelaciones();
                    C.cancelaSustituye = can[1];
                    A.Cancelaciones = C;
                }
                 if (ori != null)
                 {
                     PSV.Locacion L = new PSV.Locacion();
                     L.codigo = ori[1];
                     L.nombre = ori[2];
                     A.Origen = L;
                 }
                 if (des != null)
                 {
                     PSV.FacturaDestino D = new PSV.FacturaDestino();
                     D.codigo = des[1];
                     D.naveReciboMaterial = des[2];
                     
                     A.Destino = D;
                 }
                 if (med != null)
                 {
                     PSV.FacturaMedidas M=new PSV.FacturaMedidas();
                     M.descripcion = med[1];
                     if (!string.IsNullOrEmpty(med[2]))
                     {
                         M.numeroPiezasSpecified = true;
                         M.numeroPiezas =Convert.ToDecimal( med[2]);
                     }
                     else
                         M.numeroPiezasSpecified = false;
                     if (!string.IsNullOrEmpty(med[3]))
                     {
                         M.pesoBrutoSpecified = true;
                         M.pesoBruto =Convert.ToDecimal(  med[3]);
                     }
                     else
                         M.pesoBrutoSpecified = false;
                     if (!string.IsNullOrEmpty(med[4]))
                     {
                         M.pesoNetoSpecified = true;
                         M.pesoNeto = Convert.ToDecimal(med[4]);
                     }
                     else
                         M.pesoNetoSpecified = false;
                     if (!string.IsNullOrEmpty(med[5]))
                     {
                         M.volumenSpecified = true;
                         M.volumen = Convert.ToDecimal(med[5]);
                     }
                     else
                         M.volumenSpecified = false;
                     A.Medidas=M;
                 }

                if (not.Any())
                {

                    List<string> Lista = new List<string>();
                    foreach (var n in not)
                    {
                        string p = "";
                        p = n[1];
                        Lista.Add(p);

                    }

                    A.Nota = Lista.ToArray();
                }
                if (arc.Any())
                {
                  List<  PSV.FacturaArchivo> AR = new List<PSV.FacturaArchivo>();
                  foreach (var a in arc)
                  {
                      PSV.FacturaArchivo ar = new PSV.FacturaArchivo();
                      ar.datos = a[1];
                      ar.tipo = (PSV.FacturaArchivoTipo)Enum.Parse(typeof(PSV.FacturaArchivoTipo), ParserNtLink.GetValue(a[2]));

                      AR.Add(ar);
                  }

                  A.Archivo = AR.ToArray();
                }

                if (par.Any())
                {
                    List<PSV.FacturaParte> P = new List<PSV.FacturaParte>();
                    foreach (var pa in par)
                    {
                        PSV.FacturaParte p = new PSV.FacturaParte();
                        p.posicion =pa[1];
                        p.codigoImpuesto = pa[2];
                        p.descripcionMaterial = pa[3];
                        p.montoLinea =Convert.ToDecimal( pa[4]);
                        if(!string.IsNullOrEmpty( pa[5]))
                        p.numeroMaterial = pa[5];
                        if (!string.IsNullOrEmpty(pa[6]))
                        {
                            p.pesoBruto = Convert.ToDecimal(pa[6]);
                            p.pesoBrutoSpecified = true;
                        }
                        else
                            p.pesoBrutoSpecified = false;
                        if (!string.IsNullOrEmpty(pa[7]))
                        {
                            p.pesoNeto = Convert.ToDecimal(pa[7]);
                            p.pesoNetoSpecified = true;
                        }
                        else
                            p.pesoNetoSpecified = false;
                        p.cantidadMaterial =Convert.ToDecimal( pa[8]);
                        p.precioUnitario = Convert.ToDecimal(pa[9]);
                        p.unidadMedida = pa[10];
                       
                       
                        PSV.FacturaParteReferencias RE = new PSV.FacturaParteReferencias();
                        RE.ordenCompra = pa[11];
                         
                        p.Referencias = RE;
                          if (pn.Any())
                          {
                           var notass = pn.Where(j => (j[1] == pa[1]));
                              if (notass.Any())
                               {
                                    List<string> Listas = new List<string>();
                                       foreach (var n in notass)
                                       {
                                          string pp = "";
                                           pp = n[2];
                                         Listas.Add(pp);

                                        }
                                   p.Nota = Listas.ToArray();
                               }
                          }

                        P.Add(p);
                    }

                    A.Partes = P.ToArray();
                }

                return A;

            }
            else
                return null;
        }
    }
}
