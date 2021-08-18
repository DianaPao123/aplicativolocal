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
//using ServicioLocal.Business;
using ConvertidorCfdi;
using GeneradorCfdi.Complementos;

namespace TxtToCfdi
{
    public class ParserNtLink : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (ParserNtLink));

        public static string GetValue(string input)
        {
            if (input == "")
                return null;
            return input;
        }

        public static string GetValue2(string input)
        {
            if (input == "")
                return null;
            return input.Replace("\\","\r\n");
        }

        public static string[][] GetFileData(string fileName)
        {
            if (!File.Exists(fileName))
                throw new ApplicationException("El archivo no existe");
            var datos = File.ReadAllLines(fileName, Encoding.Default).Select(p => p.Split('|')).ToArray();

            if (datos.Length == 0)
            {
                throw new Exception("El archivo " + fileName + " está vacío");
            }
            return datos;
        }

        public Comprobante ParseData(string[][] datos)
        {
            #region Obtención de datos
            var comprob = datos.FirstOrDefault(p => p[0] == "COMP");
            var CfdiRelacionados = datos.FirstOrDefault(p => p[0] == "CRT");//nuevo
            var UUID = datos.Where(p => p[0] == "CRU");//nuevo
            var emisor = datos.FirstOrDefault(p => p[0] == "E");
            var receptor = datos.FirstOrDefault(p => p[0] == "R");
            var concepts = datos.Where(p => p[0] == "C");
            var InformAdua = datos.Where(p => p[0] == "IAC");//nuevo
            var Traslados = datos.Where(p => p[0] == "ITC");//nuevo
            var Retenciones = datos.Where(p => p[0] == "IRC");//nuevo
            var Parte = datos.Where(p => p[0] == "PC");//nuevo
            var IAParte = datos.Where(p => p[0] == "IAPC");//nuevo
            var ITraslados = datos.Where(p => p[0] == "IT");//nuevo
            var IRetenciones = datos.Where(p => p[0] == "IR");//nuevo
            var ITotales = datos.FirstOrDefault(p => p[0] == "TIMP");//nuevo
                    
            //var emisorDomicilio = datos.FirstOrDefault(p => p[0] == "DE");
            //var emitidoEn = datos.FirstOrDefault(p => p[0] == "EE");
            //var receptorDomicilio = datos.FirstOrDefault(p => p[0] == "DR");
           
            var impuestosTraslado = datos.Where(p => p[0] == "IT");
            var impuestosRetencion = datos.Where(p => p[0] == "IR");
            //---------nuevos
            var impuestosLocales = datos.FirstOrDefault(p => p[0] == "IL");
            var impuestosLocalesretenciones = datos.Where(p => p[0] == "ILR");
            var impuestosLocalestraslados = datos.Where(p => p[0] == "ILTL");

            //---------------------------------------------
            var cabeceraLeyendasFiscales = datos.FirstOrDefault(j => j[0] == "LeyendasFiscales");
            var disposicionFiscal = datos.Where(j => j[0] == "LeyendasFiscLeyenda");

            //-----------complementos--------------------------------
            var pagos = datos.Where(p => p[0] == "PAG");
            var documentos = datos.Where(p => p[0] == "DRPAG");
            var impPagosDocu = datos.Where(p => p[0] == "TIMPPAG");
            var impPagosRetencionDocu = datos.Where(p => p[0] == "IRPAG");
            var impPagosTrasladoDocu = datos.Where(p => p[0] == "ITPAG");

            var ComercioExterior = datos.FirstOrDefault(p => p[0] == "CCE");
            var EmisorCE = datos.FirstOrDefault(p => p[0] == "ECCE");
            var PropietarioCE = datos.Where(p => p[0] == "PROCCE");
            var ReceptorCE = datos.FirstOrDefault(p => p[0] == "RCCE");
            var DestinatarioCE = datos.Where(p => p[0] == "DESCCE");
            var DestinatarioCEDomi = datos.Where(p => p[0] == "DESDOMCCE");

            var MercanciaCE = datos.Where(p => p[0] == "MCCE");
            var DescripcionesCE = datos.Where(p => p[0] == "MDCCE");

            var INE = datos.FirstOrDefault(j => j[0] == "INE");
            var EntidadINE = datos.Where(p => p[0] == "EntidadINE");
            var ContabilidadINE = datos.Where(p => p[0] == "ContabilidadINE");

            var Donat = datos.FirstOrDefault(j => j[0] == "Donat11");

            var CP = datos.FirstOrDefault(j => j[0] == "CARTAPORTE");
            var UCP= datos.Where(p => p[0] == "UBICACIONCP");
            var OCP = datos.Where(p => p[0] == "ORIGENCP");
            var DCP = datos.Where(p => p[0] == "DESTINOCP");
            var DOCP = datos.Where(p => p[0] == "DOMICILIOCP");
            var MCP = datos.FirstOrDefault(j => j[0] == "MERCANCIASCP");
            var MECP = datos.Where(j => j[0] == "MERCANCIACP");
            var DMCP = datos.Where(j => j[0] == "DETALLEMERCANCIACP");
            var CTCP = datos.Where(j => j[0] == "CANTIDADTRANSPORTACP");
            var ATCP = datos.FirstOrDefault(j => j[0] == "ATFEDERALCP");
            var ATRCP = datos.Where(j => j[0] == "ATFEDERALREMOLQUECP");
            var TMCP = datos.FirstOrDefault(j => j[0] == "TMARITIMOCP");
            var TMCCP = datos.Where(j => j[0] == "TMARITIMOCONTENEDORCP");
            var TACP = datos.FirstOrDefault(j => j[0] == "TAEREOCP");
            var TFCP = datos.FirstOrDefault(j => j[0] == "TFERROVIARIOCP");
            var TFDCCP = datos.Where(j => j[0] == "TFERROVIARIODERECHOCP");
            var TFCCCP = datos.Where(j => j[0] == "TFERROVIARIOCARROCP");
            var TFCCCCP = datos.Where(j => j[0] == "TFERROVIARIOCARROCCP");
            var FTCP = datos.FirstOrDefault(j => j[0] == "FIGURATRANSPORTECP");
            var OPCP = datos.Where(j => j[0] == "OPERADORCP");
            var PRCP = datos.Where(j => j[0] == "PROPIETARIOCP");
            var ARCP = datos.Where(j => j[0] == "ARRENDATARIOCP");
            var NOCP = datos.Where(j => j[0] == "NOTIFICADOCP");
            //----------------------------------------------
            var datosAdicionales = datos.FirstOrDefault(j => j[0] == "AD");
            var fin = datos.FirstOrDefault(j => j[0] == "FIN");
            if (fin == null)
            {
                throw new IncompleteException();
            }
            Comprobante comprobante = new Comprobante();
            #endregion
            #region Emisor
            comprobante.Emisor = new GeneradorCfdi.ComprobanteEmisor();
            comprobante.Emisor.Nombre = emisor[2];
            comprobante.Emisor.RegimenFiscal = emisor[3];
                                                   
            comprobante.Emisor.Rfc = emisor[1];

           /* comprobante.Emisor.DomicilioFiscal = new GeneradorCfdi.t_UbicacionFiscal();
            comprobante.Emisor.DomicilioFiscal.calle = GetValue(emisorDomicilio[1]);
            comprobante.Emisor.DomicilioFiscal.noExterior = GetValue(emisorDomicilio[2]);
            comprobante.Emisor.DomicilioFiscal.noInterior = GetValue(emisorDomicilio[3]);
            comprobante.Emisor.DomicilioFiscal.colonia = GetValue(emisorDomicilio[4]);
            comprobante.Emisor.DomicilioFiscal.codigoPostal = GetValue(emisorDomicilio[8]);
            comprobante.Emisor.DomicilioFiscal.municipio = GetValue(emisorDomicilio[5]);
            comprobante.Emisor.DomicilioFiscal.pais = GetValue(emisorDomicilio[7]);
            comprobante.Emisor.DomicilioFiscal.estado = GetValue(emisorDomicilio[6]);
            comprobante.Emisor.DomicilioFiscal.localidad = GetValue(emisorDomicilio[9]);
           */
          //   if(!string.IsNullOrEmpty(emisor[4]))
          //  comprobante.CURPEmisor = emisor[4];
            comprobante.Emisor.direccion = emisor[4];

            #endregion
            #region Propiedades documento
            comprobante.Titulo = "Factura";
            if (comprob[5].Equals("I", StringComparison.InvariantCultureIgnoreCase))
                comprobante.TipoDeComprobante = "I";
            else
            {
                comprobante.TipoDeComprobante = "E";
                comprobante.Titulo = "Nota de Crédito";
            }
            #endregion
            #region Receptor
            comprobante.Receptor = new GeneradorCfdi.ComprobanteReceptor();
            comprobante.Receptor.Nombre = receptor[2];
            if (!String.IsNullOrEmpty(comprobante.Receptor.Nombre))
            {
                comprobante.Receptor.Nombre = receptor[2];
            }
            else
            {
                comprobante.Receptor.Nombre = " ";
            }
            comprobante.Receptor.Rfc = receptor[1];
            comprobante.Receptor.Nombre = GetValue(receptor[2]);
            comprobante.Receptor.UsoCFDI = GetValue(receptor[3]);
            if (!String.IsNullOrEmpty(receptor[4]))
            {
                comprobante.Receptor.ResidenciaFiscal = GetValue(receptor[4]);
                comprobante.Receptor.ResidenciaFiscalSpecified = true;
            }
            else
                comprobante.Receptor.ResidenciaFiscalSpecified = false;
            if (!String.IsNullOrEmpty(receptor[5]))
            comprobante.Receptor.NumRegIdTrib = GetValue(receptor[5]);

            comprobante.Receptor.Emails = receptor[6];
            comprobante.Receptor.Bcc = receptor[7];
            comprobante.Receptor.direccion = receptor[8];

          /*
            comprobante.Receptor.Domicilio = new GeneradorCfdi.t_Ubicacion();
            comprobante.Receptor.Domicilio.noExterior = GetValue(receptorDomicilio[2]);
            comprobante.Receptor.Domicilio.noInterior = GetValue(receptorDomicilio[3]);
            comprobante.Receptor.Domicilio.pais = GetValue(receptorDomicilio[7]);
            comprobante.Receptor.Domicilio.calle = GetValue(receptorDomicilio[1]);


            comprobante.Receptor.Domicilio.municipio = GetValue(receptorDomicilio[5]);
            comprobante.Receptor.Domicilio.estado = GetValue(receptorDomicilio[6]);
            comprobante.Receptor.Domicilio.colonia = GetValue(receptorDomicilio[4]);
            comprobante.Receptor.Domicilio.codigoPostal = GetValue(receptorDomicilio[8]);
            comprobante.Receptor.Domicilio.localidad = GetValue(receptorDomicilio[9]);
           */ 
            #endregion
            #region Expedido en
            /*
            if (emitidoEn != null)
            {
               
                comprobante.Emisor.ExpedidoEn = new GeneradorCfdi.t_Ubicacion();
                comprobante.Emisor.ExpedidoEn.calle = GetValue(emitidoEn[1]);
                comprobante.Emisor.ExpedidoEn.noExterior = GetValue(emitidoEn[2]);
                comprobante.Emisor.ExpedidoEn.noInterior = GetValue(emitidoEn[3]);
                comprobante.Emisor.ExpedidoEn.colonia = GetValue(emitidoEn[4]);
                comprobante.Emisor.ExpedidoEn.codigoPostal = GetValue(emitidoEn[8]);
                comprobante.Emisor.ExpedidoEn.municipio = GetValue(emitidoEn[5]);
                comprobante.Emisor.ExpedidoEn.pais = GetValue(emitidoEn[7]);
                comprobante.Emisor.ExpedidoEn.estado = GetValue(emitidoEn[6]);
                comprobante.Emisor.ExpedidoEn.localidad = GetValue(emitidoEn[9]);
                comprobante.LugarExpedicion = "";
                if (comprobante.Emisor.ExpedidoEn.localidad != null)
                    comprobante.LugarExpedicion = comprobante.LugarExpedicion +
                                                    comprobante.Emisor.ExpedidoEn.localidad;
                if (comprobante.Emisor.ExpedidoEn.estado != null)
                    comprobante.LugarExpedicion = comprobante.LugarExpedicion + "," +
                                                    comprobante.Emisor.ExpedidoEn.estado;
                 
            }*/
            #endregion
            #region Reemplazar fecha
            //comprobante.fecha = Convert.ToDateTime(DateTime.Now.ToString("s"));//entrada.fecha;
            if (ConfigurationManager.AppSettings["ReemplazarFecha"] == "1")
                comprobante.Fecha = DateTime.Now.ToString("s");
            else
            {
                try
                {
                    comprobante.Fecha = DateTime.ParseExact(comprob[3], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToString();
                    comprobante.Fecha = Convert.ToDateTime(comprobante.Fecha).ToString("s");
                }
                catch (Exception ee)
                {
                    Logger.Error("Fecha inválida: " + comprob[3]);
                    Logger.Error(ee);
                    throw;
                }
            }
            #endregion
            #region Total, subtotal, moneda, forma de pago, serie, leyendas, notas, descuentos
            if(!string.IsNullOrEmpty(comprob[1]))
            comprobante.Serie = GetValue(comprob[1]);
            if (!string.IsNullOrEmpty(comprob[2]))
            comprobante.Folio = GetValue(comprob[2]);
            comprobante.LugarExpedicion = GetValue(comprob[4]);
            comprobante.TipoDeComprobante = GetValue(comprob[5]);
            if (!string.IsNullOrEmpty(comprob[6]))
            {
                comprobante.FormaPagoSpecified = true;
                comprobante.FormaPago = GetValue(comprob[6]);
            }
            else
                comprobante.FormaPagoSpecified = false;
            if (!string.IsNullOrEmpty(comprob[7]))
            {
                comprobante.MetodoPagoSpecified = true;
                comprobante.MetodoPago = GetValue(comprob[7]);
            }
            else
                comprobante.MetodoPagoSpecified = false;
            if (!string.IsNullOrEmpty(comprob[8]))
                comprobante.CondicionesDePago = GetValue(comprob[8]);
            comprobante.SubTotal = decimal.Parse(comprob[9]);// factura.Factura.Total.Value - factura.Factura.IVA.Value + factura.Factura.RetenciónIva;
            if (!string.IsNullOrEmpty(comprob[10]))
            {
                comprobante.Descuento = comprob[10];
                comprobante.DescuentoSpecified = true;
            }else
                comprobante.DescuentoSpecified = false;
           
            comprobante.Total = decimal.Parse(comprob[11]);
            comprobante.Moneda = GetValue(comprob[12]);
            if (!string.IsNullOrEmpty(comprob[13]))
            {
                comprobante.TipoCambio = Convert.ToDecimal(comprob[13]);
                comprobante.TipoCambioSpecified = true;
            }
            else
                comprobante.TipoCambioSpecified = false;
            if (!string.IsNullOrEmpty(comprob[14]))
                comprobante.Confirmacion = GetValue2(comprob[14]);
                      

           // comprobante.Regimen = GetValue(emisor[3]);
            if (!string.IsNullOrEmpty(comprob[15]))
            comprobante.LeyendaSuperior = GetValue2(comprob[15]);
            if (!string.IsNullOrEmpty(comprob[16]))
                comprobante.Leyenda = GetValue2(comprob[16]);
            if (!string.IsNullOrEmpty(comprob[17]))
              comprobante.Proyecto = GetValue2(comprob[17]);
            if (!string.IsNullOrEmpty(comprob[18]))
            comprobante.nota1 = GetValue2(comprob[18]);
            if (!string.IsNullOrEmpty(comprob[19]))
            comprobante.nota2 = GetValue2(comprob[19]);
                                  
           
            #endregion
            #region CfdiRelacionados
            if(CfdiRelacionados!=null)
            if (CfdiRelacionados.Any())
            { 
                if (comprobante.CfdiRelacionados == null)
                    comprobante.CfdiRelacionados = new ComprobanteCfdiRelacionados();
                comprobante.CfdiRelacionados.TipoRelacion = CfdiRelacionados[1];
            }
            #endregion
            #region CfdiRelacionadosUUDI
            if (UUID.Any())
            {
                if (comprobante.CfdiRelacionados == null)
                    comprobante.CfdiRelacionados = new ComprobanteCfdiRelacionados();
                if (comprobante.CfdiRelacionados.CfdiRelacionado == null)
                    comprobante.CfdiRelacionados.CfdiRelacionado = new List<ComprobanteCfdiRelacionadosCfdiRelacionado>();
                foreach (var uudi in UUID)
                {
                    ComprobanteCfdiRelacionadosCfdiRelacionado u=new ComprobanteCfdiRelacionadosCfdiRelacionado();
                    u.UUID=uudi[1];
                    comprobante.CfdiRelacionados.CfdiRelacionado.Add(u);
                }
            }
            #endregion
            #region Datos adicionales
            if (datosAdicionales != null)
            {
                DatosAdicionales da = new DatosAdicionales();
                da.CalleEmisor = datosAdicionales[1];
                da.NumExterior = datosAdicionales[2];
                da.NumInterior = datosAdicionales[3];
                da.Colonia = datosAdicionales[4];
                da.Municipio = datosAdicionales[5];
                da.Estado = datosAdicionales[6];
                da.Pais = datosAdicionales[7];
                da.CodigoPostal = datosAdicionales[8];
                da.Localidad = datosAdicionales[9];
                da.CondicionesPago = datosAdicionales[10];
                da.NumOportunidad = datosAdicionales[11];
                da.OrdenCompra = datosAdicionales[12];
                da.NombreContacto = datosAdicionales[13];
                da.Vendedor = datosAdicionales[14];
                comprobante.DatosAdicionales = da;

            }
            //if (comprob[10] != null)
            //    comprobante.descuento = decimal.Parse(comprob[10]);
            //comprobante.VoBoNombre = factura.Factura.VoBoNombre;
            //comprobante.VoBoPuesto = factura.Factura.VoBoPuesto;
            //comprobante.VoBoArea = factura.Factura.VoBoArea;
            //comprobante.AutorizoNombre = factura.Factura.AutorizoNombre;
            //comprobante.AutorizoPuesto = factura.Factura.AutorizoPuesto;
            //comprobante.AutorizoArea = factura.Factura.AutorizoArea;
            //comprobante.RecibiNombre = factura.Factura.RecibiNombre;
            //comprobante.RecibiPuesto = factura.Factura.RecibiPuesto;
            //comprobante.RecibiArea = factura.Factura.RecibiArea;
            //comprobante.VoBoTitulo = factura.Factura.VoBoTitulo;
            //comprobante.RecibiTitulo = factura.Factura.RecibiTitulo;
            //comprobante.AutorizoTitulo = factura.Factura.AutorizoTitulo;
            //comprobante.AgregadoArea = factura.Factura.AgregadoArea;
            //comprobante.AgregadoNombre = factura.Factura.AgregadoNombre;
            //comprobante.AgregadoPuesto = factura.Factura.AgregadoPuesto;
            //comprobante.AgregadoTitulo = factura.Factura.AgregadoTitulo;
            //comprobante.condicionesDePago = factura.Factura.FormaPago;
            //comprobante.FechaPago = factura.Factura.FechaPago;
            //comprobante.CURPEmisor = emp.CURP;
            //comprobante.TituloOtros = factura.Factura.TituloOtros;
            #endregion
            #region Conceptos
            List<GeneradorCfdi.ComprobanteConcepto> conceptos = new List<GeneradorCfdi.ComprobanteConcepto>();
            // List<GeneradorCfdi.t_InformacionAduanera> informacionAduanera = new List<GeneradorCfdi.t_InformacionAduanera>();
            foreach (var detalle in concepts)
            {
                GeneradorCfdi.ComprobanteConcepto con = new GeneradorCfdi.ComprobanteConcepto();
                con.ClaveProdServ = GetValue2(detalle[2]);
                if (!string.IsNullOrEmpty(detalle[3]))
                 con.NoIdentificacion = detalle[3];
                con.Cantidad = decimal.Parse(detalle[4]);
                con.ClaveUnidad = detalle[5];
                if (!string.IsNullOrEmpty(detalle[6]))
                    con.Unidad = detalle[6];
                con.Descripcion = GetValue2(detalle[7]);
                con.ValorUnitario =detalle[8];
                con.Importe = detalle[9];
                if (!string.IsNullOrEmpty(detalle[10]))
                {
                    con.DescuentoSpecified = true;
                    con.Descuento = detalle[10];
                }
                else
                    con.DescuentoSpecified = false;
                if (!string.IsNullOrEmpty(detalle[11]))
                {
                    if (con.CuentaPredial == null)
                        con.CuentaPredial = new GeneradorCfdi.ComprobanteConceptoCuentaPredial();
                    con.CuentaPredial.Numero = detalle[11];
                    con.CuentaPredialString = detalle[11];
                }
                if(detalle.Count()>12)
                if (!string.IsNullOrEmpty(detalle[12]))
                    con.Detalles = detalle[12].Replace("\\", "\n");
                if (detalle.Count() > 13)
                if (!string.IsNullOrEmpty(detalle[13]))
                    con.Adicional1 = detalle[13];
                if (detalle.Count() > 14)
                if (!string.IsNullOrEmpty(detalle[14]))
                    con.Adicional2 = detalle[14];
                if (detalle.Count() > 15)
                if (!string.IsNullOrEmpty(detalle[15]))
                    con.Adicional3 = detalle[15];
                if (detalle.Count() > 16)
                if (!string.IsNullOrEmpty(detalle[16]))
                    con.Adicional4 = detalle[16];
                if (detalle.Count() > 17)
                if (!string.IsNullOrEmpty(detalle[17]))
                    con.Adicional5 = detalle[17];

                #region informacionAduanera

                //-------------------------informacion aduanera------------------
                var IA = InformAdua.Where(j => (j[1] == detalle[1]));
                if (IA.Any())
                {
                    List<ComprobanteConceptoInformacionAduanera> infAdu = new List<ComprobanteConceptoInformacionAduanera>();
                    foreach (var ia in IA)
                    {
                        ComprobanteConceptoInformacionAduanera i = new ComprobanteConceptoInformacionAduanera();
                        i.NumeroPedimento = ia[2];
                        infAdu.Add(i);
                    }
                    con.InformacionAduanera = infAdu.ToArray();

                }
                #endregion
                #region traslados

                //-----------------------------traslados-------------------------
                var Tras = Traslados.Where(j => (j[1] == detalle[1]));
                if (Tras.Any())
                {
                    if(con.Impuestos==null)
                    con.Impuestos = new ComprobanteConceptoImpuestos();
                    List<ComprobanteConceptoImpuestosTraslado> LT = new List<ComprobanteConceptoImpuestosTraslado>();
                    foreach (var t in Tras)
                    {
                        ComprobanteConceptoImpuestosTraslado lt = new ComprobanteConceptoImpuestosTraslado();
                        lt.Base =Convert.ToDecimal( t[2]);
                        lt.Impuesto = t[3];
                        lt.TipoFactor = t[4];
                        if (!string.IsNullOrEmpty(t[6]))
                        {
                            lt.TasaOCuota = t[5];
                            lt.TasaOCuotaSpecified = true;
                        }
                        else
                            lt.TasaOCuotaSpecified = false;
                        if (!string.IsNullOrEmpty(t[6]))
                        {
                            lt.ImporteSpecified = true;
                            lt.Importe = t[6];
                        }
                        else
                            lt.ImporteSpecified = false;
                        LT.Add(lt);

                    }
                    con.Impuestos.Traslados = LT.ToArray();

                }
                #endregion
                #region Retenciones

                //----------------------------Retenciones-----------------------------
                var Ret = Retenciones.Where(j => (j[1] == detalle[1]));
                if (Ret.Any())
                {
                    if (con.Impuestos == null)
                        con.Impuestos = new ComprobanteConceptoImpuestos();
                    List<ComprobanteConceptoImpuestosRetencion> LR = new List<ComprobanteConceptoImpuestosRetencion>();
                    foreach (var r in Ret)
                    {
                        ComprobanteConceptoImpuestosRetencion lr = new ComprobanteConceptoImpuestosRetencion();
                        lr.Base = Convert.ToDecimal(r[2]);
                        lr.Impuesto = r[3];
                        lr.TipoFactor = r[4];
                        lr.TasaOCuota = r[5];
                        lr.Importe = r[6];
                        LR.Add(lr);

                    }
                    con.Impuestos.Retenciones = LR.ToArray();


                }
                #endregion
                #region Parte

                //------------------------------Parte---------------------------
                var P = Parte.Where(j => (j[2] == detalle[1]));
                if (P.Any())
                {
                    List<GeneradorCfdi.ComprobanteConceptoParte> LP = new List<GeneradorCfdi.ComprobanteConceptoParte>();
                    foreach (var p in P)
                    {
                        GeneradorCfdi.ComprobanteConceptoParte lp = new GeneradorCfdi.ComprobanteConceptoParte();
                        lp.ClaveProdServ = p[3];
                        if (!string.IsNullOrEmpty(p[4]))
                          lp.NoIdentificacion = p[4];
                        lp.Cantidad =Convert.ToDecimal( p[5]);
                        lp.Unidad = p[6];
                        lp.Descripcion = p[7];
                        if (!string.IsNullOrEmpty(p[8]))
                        {
                            lp.ValorUnitario =Convert.ToDecimal( p[8]);
                            lp.ValorUnitarioSpecified = true;
                        }
                        else
                            lp.ValorUnitarioSpecified = false;
                        if (!string.IsNullOrEmpty(p[9]))
                        {

                            lp.Importe =Convert.ToDecimal( p[9]);
                            lp.ImporteSpecified = true;
                        }
                        else
                            lp.ImporteSpecified = false;
                        //-------------------------------------------
                        var IAP= IAParte.Where(j => (j[1] == p[1]));
                        if (IAP.Any())
                        {
                            if (lp.InformacionAduanera==null)
                            lp.InformacionAduanera = new List<ComprobanteConceptoParteInformacionAduanera>();
                            List<ComprobanteConceptoParteInformacionAduanera> infAduP = new List<ComprobanteConceptoParteInformacionAduanera>();
                            foreach (var iap in IAP)
                            {
                                ComprobanteConceptoParteInformacionAduanera i = new ComprobanteConceptoParteInformacionAduanera();
                                i.NumeroPedimento = iap[2];
                                infAduP.Add(i);
                            }
                           lp.InformacionAduanera = infAduP;
                  
                        }
              
                        //-----------------------------
                        LP.Add(lp);
                       
                    }
                    con.Parte = LP.ToArray();

                }
                #endregion

                //--------------------------------------------------------------------
              
                conceptos.Add(con);

            }
            comprobante.Conceptos = conceptos.ToArray();
            #endregion
            #region Retencion 
            if (IRetenciones.Any())
            {
                if (comprobante.Impuestos == null)
                    comprobante.Impuestos = new GeneradorCfdi.ComprobanteImpuestos();

                List<GeneradorCfdi.ComprobanteImpuestosRetencion> LT = new List<GeneradorCfdi.ComprobanteImpuestosRetencion>();
                foreach (var t in IRetenciones)
                {
                    GeneradorCfdi.ComprobanteImpuestosRetencion  lt = new GeneradorCfdi.ComprobanteImpuestosRetencion();
                    lt.Impuesto = t[1];
                   lt.Importe = t[2];
                    LT.Add(lt);

                }
                comprobante.Impuestos.Retenciones = LT.ToArray();
            }

            #endregion
            #region Traslados

            if (ITraslados.Any())
            {
                if (comprobante.Impuestos == null)
                    comprobante.Impuestos = new GeneradorCfdi.ComprobanteImpuestos();

                List<GeneradorCfdi.ComprobanteImpuestosTraslado> LT = new List<GeneradorCfdi.ComprobanteImpuestosTraslado>();
                foreach (var t in ITraslados)
                {
                    GeneradorCfdi.ComprobanteImpuestosTraslado lt = new GeneradorCfdi.ComprobanteImpuestosTraslado();
                    lt.Impuesto = t[1];
                    lt.TipoFactor = t[2];
                    lt.TasaOCuota = t[3];
                    lt.Importe = t[4];
                    LT.Add(lt);

                }
                comprobante.Impuestos.Traslados = LT.ToArray();
            }
            #endregion
            #region Totales Retencion Traslados
            if (ITotales != null)
            {
                if (comprobante.Impuestos == null)
                    comprobante.Impuestos = new GeneradorCfdi.ComprobanteImpuestos();
                if (!string.IsNullOrEmpty(ITotales[2]))
                {
                    comprobante.Impuestos.TotalImpuestosRetenidosSpecified = true;
                    comprobante.Impuestos.TotalImpuestosRetenidos = ITotales[2];
                }
                else
                    comprobante.Impuestos.TotalImpuestosRetenidosSpecified = false;
                if (!string.IsNullOrEmpty(ITotales[1]))
                {
                    comprobante.Impuestos.TotalImpuestosTrasladadosSpecified = true;
                    comprobante.Impuestos.TotalImpuestosTrasladados = ITotales[1];
                }
                else
                    comprobante.Impuestos.TotalImpuestosTrasladadosSpecified = false;
            }

            #endregion
            #region Impuestos, cantidad en letra
            /*
            GeneradorCfdi.ComprobanteImpuestos impuestos = new GeneradorCfdi.ComprobanteImpuestos();
            if (impuestosTraslado.Any())
            {

                List<GeneradorCfdi.ComprobanteImpuestosTraslado> listaTraslados = new List<GeneradorCfdi.ComprobanteImpuestosTraslado>();
                foreach (var tr in impuestosTraslado)
                {
                    GeneradorCfdi.ComprobanteImpuestosTraslado traslado = new GeneradorCfdi.ComprobanteImpuestosTraslado();
                    traslado.importe = decimal.Parse(tr[3]);
                    if (tr[1].Equals("IVA", StringComparison.InvariantCultureIgnoreCase))
                        traslado.impuesto = GeneradorCfdi.ComprobanteImpuestosTrasladoImpuesto.IVA;
                    else traslado.impuesto = GeneradorCfdi.ComprobanteImpuestosTrasladoImpuesto.IEPS;
                    traslado.tasa = decimal.Parse(tr[2]);
                    comprobante.TituloOtros = traslado.tasa.ToString() + "%";
                    listaTraslados.Add(traslado);
                }
                impuestos.Traslados = listaTraslados.ToArray();
                impuestos.totalImpuestosTrasladados = listaTraslados.Any() ? listaTraslados.Sum(p => p.importe) : 0;
                impuestos.totalImpuestosTrasladadosSpecified = true;
                comprobante.Traslados = impuestos.Traslados;
            }

            if (impuestosRetencion.Any())
            {

                List<GeneradorCfdi.ComprobanteImpuestosRetencion> listaTraslados = new List<GeneradorCfdi.ComprobanteImpuestosRetencion>();
                foreach (var tr in impuestosRetencion)
                {
                    GeneradorCfdi.ComprobanteImpuestosRetencion retencion = new GeneradorCfdi.ComprobanteImpuestosRetencion();
                    retencion.importe = decimal.Parse(tr[2]);
                    if (tr[1].Equals("IVA", StringComparison.InvariantCultureIgnoreCase))
                    {
                        retencion.impuesto = GeneradorCfdi.ComprobanteImpuestosRetencionImpuesto.IVA;
                        comprobante.RetencionIva = retencion.importe;
                    }
                    else
                    {
                        retencion.impuesto = GeneradorCfdi.ComprobanteImpuestosRetencionImpuesto.ISR;
                        comprobante.RetencionIsr = retencion.importe;
                    }
                    listaTraslados.Add(retencion);
                }
                impuestos.Retenciones = listaTraslados.ToArray();
                impuestos.totalImpuestosRetenidos = listaTraslados.Any() ? listaTraslados.Sum(p => p.importe) : 0;
                impuestos.totalImpuestosRetenidosSpecified = true;
            }
            */
            comprobante.CantidadLetra = CantidadLetra.Enletras(comprobante.Total.ToString(), comprobante.Moneda, comprobante.Emisor.Rfc);

            
            //comprobante.Impuestos = impuestos;
            #endregion

            #region Obtención de leyendas fiscales

            //----------------------esto es nuevo para los leyendasFisc
            if (cabeceraLeyendasFiscales != null)
            {
                LeyendasFiscales H = new LeyendasFiscales();
                H.version = cabeceraLeyendasFiscales[1];
                List<LeyendasFiscalesLeyenda> LFL = new List<LeyendasFiscalesLeyenda>();


                foreach (var can in disposicionFiscal)
                {
                    LeyendasFiscalesLeyenda L = new LeyendasFiscalesLeyenda();

                    L.disposicionFiscal = can[1];
                    L.norma = can[2];
                    L.textoLeyenda = can[3];
                    LFL.Add(L);

                }
                H.Leyenda = LFL.ToArray();
                if (comprobante.Complemento == null)
                    comprobante.Complemento = new GeneradorCfdi.ComprobanteComplemento();
                comprobante.Complemento.leyendasFicales = new LeyendasFiscales();
                comprobante.Complemento.leyendasFicales = H;
            }
            #endregion

            //----------------------esto es nuevo para los impuestos locales
            #region Impuestos Locales

            if (impuestosLocales != null)

            {
                comprobante.Complemento = new GeneradorCfdi.ComprobanteComplemento();
                comprobante.Complemento.implocal = new ImpuestosLocales();

                comprobante.Complemento.implocal.TotaldeRetenciones = Convert.ToDecimal(impuestosLocales[1]);
                comprobante.Complemento.implocal.TotaldeTraslados = Convert.ToDecimal(impuestosLocales[2]);
                comprobante.Complemento.implocal.version = impuestosLocales[3];
              //  comprobante.Complemento.imlocal.TrasladosLocales = new ImpuestosLocalesTrasladosLocales();
                //--------------------------------------------------------
                if (impuestosLocalestraslados.Any())
                {
                    List<ImpuestosLocalesTrasladosLocales> LITL = new List<ImpuestosLocalesTrasladosLocales>();
                    foreach (var imt in impuestosLocalestraslados)
                    {
                        ImpuestosLocalesTrasladosLocales litl = new ImpuestosLocalesTrasladosLocales();
                        litl.ImpLocTrasladado = imt[1];
                        litl.Importe = Convert.ToDecimal(imt[2]);
                        litl.TasadeTraslado = Convert.ToDecimal(imt[3]);

                        LITL.Add(litl);
                    }
                    comprobante.Complemento.implocal.TrasladosLocales = LITL.ToArray();

                }
                //-------------------------------------------------------------
                if (impuestosLocalesretenciones.Any())
                {
                    //comprobante.Complemento.imlocal.RetencionesLocales = new ImpuestosLocalesRetencionesLocales();
                    List<ImpuestosLocalesRetencionesLocales> LIL = new List<ImpuestosLocalesRetencionesLocales>();
                    foreach (var im in impuestosLocalesretenciones)
                    {
                        ImpuestosLocalesRetencionesLocales lil = new ImpuestosLocalesRetencionesLocales();
                        lil.ImpLocRetenido = im[1];
                        lil.Importe = Convert.ToDecimal(im[2]);
                        lil.TasadeRetencion = Convert.ToDecimal(im[3]);
                        LIL.Add(lil);
                    }
                    comprobante.Complemento.implocal.RetencionesLocales = LIL.ToArray();
                }
            }
            //---------------fin de impuestos locales (nuevo)
            #endregion

            //---------------------------complementos-----------------------------------
              #region complementoPagos

            if (pagos.Any())
            {
                Pagos P= new Pagos();
                P.Version = "1.0";
                foreach(var pa in pagos)
                {
                  PagosPago PP = new PagosPago();
            
                  PP.FechaPago = pa[2];  
                  PP.FormaDePagoP=pa[3];
                  PP.MonedaP=pa[4];
                  if (!string.IsNullOrEmpty(pa[5]))
                  {
                      PP.TipoCambioP=Convert.ToDecimal( pa[5]);
                      PP.TipoCambioPSpecified=true;
                  }
                  else
                      PP.TipoCambioPSpecified=false;
                    PP.Monto=Convert.ToDecimal(pa[6]);
                     if (!string.IsNullOrEmpty(pa[7]))
                     PP.NumOperacion=pa[7];
                     if (!string.IsNullOrEmpty(pa[8]))
                   PP.RfcEmisorCtaOrd=pa[8];
                     if (!string.IsNullOrEmpty(pa[9]))
                   PP.NomBancoOrdExt=pa[9];
                  if (!string.IsNullOrEmpty(pa[10]))
                   PP.CtaOrdenante=pa[10];
                   if (!string.IsNullOrEmpty(pa[11]))
                   PP.RfcEmisorCtaBen=pa[11];
                   if (!string.IsNullOrEmpty(pa[12]))
                   PP.CtaBeneficiario=pa[12];
                   if (!string.IsNullOrEmpty(pa[13]))
                   { PP.TipoCadPagoSpecified=true;
                   PP.TipoCadPago=pa[13];
                   }
                   else
                    PP.TipoCadPagoSpecified=false;
                     if (!string.IsNullOrEmpty(pa[14]))
                   PP.CertPago=pa[14];
                     if (!string.IsNullOrEmpty(pa[15]))
                   PP.CadPago=pa[15];
                  if (!string.IsNullOrEmpty(pa[16]))
                   PP.SelloPago=pa[16];
                    
                  //------------------------------------
                  #region documentos

                  var doc = documentos.Where(j => (j[1] == pa[1]));
                    if (doc.Any())
                    {
                       List< PagosPagoDoctoRelacionado> D = new  List<PagosPagoDoctoRelacionado>();
                        foreach (var d in doc)
                        {
                           PagosPagoDoctoRelacionado docu = new PagosPagoDoctoRelacionado();
                            docu.IdDocumento = d[2];
                            if (!string.IsNullOrEmpty(d[3]))
                                docu.Serie = d[3];
                            if (!string.IsNullOrEmpty(d[4]))
                                docu.Folio = d[4];
                            docu.MonedaDR = d[5];
                            if (!string.IsNullOrEmpty(d[6]))
                            {
                                docu.TipoCambioDRSpecified = true;
                                docu.TipoCambioDR =Convert.ToDecimal( d[6]);
                            }
                            else
                                docu.TipoCambioDRSpecified = false;
                            docu.MetodoDePagoDR = d[7];
                            if (!string.IsNullOrEmpty(d[8]))
                                docu.NumParcialidad = d[8];
                            if (!string.IsNullOrEmpty(d[9]))
                            {
                                docu.ImpSaldoAntSpecified = true;
                                docu.ImpSaldoAnt = Convert.ToDecimal(d[9]);
                            }
                            else
                                docu.ImpSaldoAntSpecified = false;
                            if (!string.IsNullOrEmpty(d[10]))
                            {
                                docu.ImpPagadoSpecified = true;
                                docu.ImpPagado = Convert.ToDecimal(d[10]);
                            }
                            else
                            docu.ImpPagadoSpecified = false;
                            if (!string.IsNullOrEmpty(d[11]))
                            {
                                docu.ImpSaldoInsolutoSpecified = true;
                                docu.ImpSaldoInsoluto =  Convert.ToDecimal(d[11]);
                            }
                            else
                                docu.ImpSaldoInsolutoSpecified = false;

                            D.Add(docu);
                        }
                        if (D != null)
                            if (D.Count() > 0)
                                PP.DoctoRelacionado = D.ToArray();
                    }
                  #endregion
                    //----------------------------------
                       #region TotalImpuestosPagos

                    var IPD = impPagosDocu.Where(j => (j[1] == pa[1]));
                    if (IPD.Any())
                  {
                      List<PagosPagoImpuestos> PI = new List<PagosPagoImpuestos>();
                      
                        foreach (var ip in IPD)
                      {
                          PagosPagoImpuestos pi = new PagosPagoImpuestos();
                          if (!string.IsNullOrEmpty(ip[3]))
                          {
                              pi.TotalImpuestosTrasladadosSpecified = true;
                              pi.TotalImpuestosTrasladados = Convert.ToDecimal(ip[3]);
                          }
                          else
                              pi.TotalImpuestosTrasladadosSpecified = false;
                          if (!string.IsNullOrEmpty(ip[4]))
                          {
                              pi.TotalImpuestosRetenidosSpecified = true;
                              pi.TotalImpuestosRetenidos = Convert.ToDecimal(ip[4]);
                          }
                          else
                              pi.TotalImpuestosRetenidosSpecified = false;
                          //------------------------------------------
                          #region ImpuestosRetenidosPagos
                          var RP = impPagosRetencionDocu.Where(j => (j[1] == ip[2]));
                                if (RP.Any())
                                {
                                    List<PagosPagoImpuestosRetencion> R = new List<PagosPagoImpuestosRetencion>();
                                    foreach (var rp in RP)
                                    {
                                        PagosPagoImpuestosRetencion r = new PagosPagoImpuestosRetencion();
                                      
                                        r.Impuesto = rp[2];
                                        r.Importe = Convert.ToDecimal(rp[3]);
                                        R.Add(r);
                                    }
                                    if (R != null)
                                        if (R.Count() > 0)
                                            pi.Retenciones = R;
                                }
                          #endregion
                          //---------------------------------------

                                #region ImpuestosTrasladosPagos
                                var TP = impPagosTrasladoDocu.Where(j => (j[1] == ip[2]));
                                if (TP.Any())
                                {
                                    List<PagosPagoImpuestosTraslado> T = new List<PagosPagoImpuestosTraslado>();
                                    foreach (var tp in TP)
                                    {
                                        PagosPagoImpuestosTraslado t = new PagosPagoImpuestosTraslado();

                                        t.Impuesto = tp[2];
                                        t.TipoFactor = tp[3];
                                        t.TasaOCuota = Convert.ToDecimal( tp[4]);
                                        t.Importe = Convert.ToDecimal(tp[5]);
                                        T.Add(t);
                                    }
                                    if (T != null)
                                        if (T.Count() > 0)
                                            pi.Traslados = T;
                                }
                                #endregion
                            //------------------------------------
                          PI.Add(pi);
                      }
                        if (PI != null)
                            if (PI.Count() > 0)
                                PP.Impuestos = PI.ToArray();
                  }

                       #endregion
                           if(P.Pago==null)
                            P.Pago = new List<PagosPago>();
                            P.Pago.Add(PP);
                        
                }
                if (comprobante.Complemento == null)
                    comprobante.Complemento = new GeneradorCfdi.ComprobanteComplemento();
                if (comprobante.Complemento.Pag == null)
                {
                    comprobante.Complemento.Pag = new Pagos();
                    comprobante.Complemento.Pag = P;
                }
            }
                #endregion
            #region complementoINE
            if (INE != null)
            {
                if (comprobante.Complemento == null)
                    comprobante.Complemento = new GeneradorCfdi.ComprobanteComplemento();
                if (comprobante.Complemento.ine == null)
                     comprobante.Complemento.ine = new INE();
                if (!string.IsNullOrEmpty(INE[4]))
                {
                    comprobante.Complemento.ine.IdContabilidadSpecified = true;
                    comprobante.Complemento.ine.IdContabilidad = INE[4];
                }
                else
                    comprobante.Complemento.ine.IdContabilidadSpecified = false;
                if (!string.IsNullOrEmpty(INE[3]))
                {
                    comprobante.Complemento.ine.TipoComiteSpecified = true;
                    comprobante.Complemento.ine.TipoComite = INE[3];
                }
                else

                    comprobante.Complemento.ine.TipoComiteSpecified = false;
                comprobante.Complemento.ine.TipoProceso = INE[2];

                if (EntidadINE.Any())
                {

                    List<INEEntidad> Entidad = new List<INEEntidad>();
                    foreach (var i in EntidadINE)
                    {
                        INEEntidad E = new INEEntidad();
                        if (!string.IsNullOrEmpty(i[3]))
                        {
                            E.AmbitoSpecified = true;
                            E.Ambito = i[3];
                        }
                        else
                            E.AmbitoSpecified = false;
                        E.ClaveEntidad = i[2];

                        if (ContabilidadINE.Any())
                            {
                                List<INEEntidadContabilidad> Contabilidad = new List<INEEntidadContabilidad>();

                                var cont = ContabilidadINE.Where(j => (j[1] == i[1]));
                                if (cont.Any())
                                {

                                    foreach (var s in cont)
                                    {
                                        INEEntidadContabilidad c = new INEEntidadContabilidad();
                                      //  c.IdContabilidad = Convert.ToInt64(s[2]);
                                        c.IdContabilidad = s[2];
                                        Contabilidad.Add(c);
                                    }
                                    E.Contabilidad = Contabilidad.ToArray();
                                }
                            }
                        Entidad.Add(E);
                    }
                    if (Entidad != null)
                        comprobante.Complemento.ine.Entidad = Entidad.ToArray();
                }
            }

            #endregion

            #region complementoComercioExterior
            if (ComercioExterior != null)
            {
                ComercioExterior CE=new ComercioExterior();
                CE.Version = ComercioExterior[1];
                if (!string.IsNullOrEmpty(ComercioExterior[2]))
                {
                    CE.MotivoTrasladoSpecified = true;
                    CE.MotivoTraslado = ComercioExterior[2];
                }
                else
                    CE.MotivoTrasladoSpecified = false;
                CE.TipoOperacion = ComercioExterior[3];
                if (!string.IsNullOrEmpty(ComercioExterior[4]))
                {
                    CE.ClaveDePedimentoSpecified = true;
                    CE.ClaveDePedimento = ComercioExterior[4];
                }
                else
                    CE.ClaveDePedimentoSpecified = false;
                if (!string.IsNullOrEmpty(ComercioExterior[5]))
                {
                    CE.CertificadoOrigenSpecified = true;
                    CE.CertificadoOrigen =Convert.ToInt32( ComercioExterior[5]);
                }
                else CE.CertificadoOrigenSpecified = false;
                if (!string.IsNullOrEmpty(ComercioExterior[6]))
                {
                    CE.NumCertificadoOrigen = ComercioExterior[6];
                }
                if (!string.IsNullOrEmpty(ComercioExterior[7]))
                {
                    CE.NumeroExportadorConfiable = ComercioExterior[7];
                }
                if (!string.IsNullOrEmpty(ComercioExterior[8]))
                {
                    CE.IncotermSpecified = true;
                    CE.Incoterm = ComercioExterior[8];
                }
                else CE.IncotermSpecified = false;
                  if (!string.IsNullOrEmpty(ComercioExterior[9]))
                {
                    CE.SubdivisionSpecified = true;
                    CE.Subdivision = Convert.ToInt32(ComercioExterior[9]);
                }
                else CE.SubdivisionSpecified = false;

                  if (!string.IsNullOrEmpty(ComercioExterior[10]))
                  CE.Observaciones = ComercioExterior[10];

                  if (!string.IsNullOrEmpty(ComercioExterior[11]))
                  {
                      CE.TipoCambioUSDSpecified = true;
                      CE.TipoCambioUSD = Convert.ToDecimal(ComercioExterior[11]);
                  }
                  else CE.TipoCambioUSDSpecified = false;
                
                       if (!string.IsNullOrEmpty(ComercioExterior[12]))
                  {
                      CE.TotalUSDSpecified = true;
                      CE.TotalUSD = Convert.ToDecimal(ComercioExterior[12]);
                  }
                  else CE.TotalUSDSpecified = false;
                       //-------------------------------------------
                       #region Emisor

                       if (EmisorCE != null)
                     {
                         CE.Emisor = new ComercioExteriorEmisor();
                         if(!string.IsNullOrEmpty(EmisorCE[1]))   
                         CE.Emisor.Curp=EmisorCE[1];
                         CE.Emisor.Domicilio=new ComercioExteriorEmisorDomicilio();
                         CE.Emisor.Domicilio.Calle = EmisorCE[2];
                         if (!string.IsNullOrEmpty(EmisorCE[3]))   
                         CE.Emisor.Domicilio.NumeroExterior = EmisorCE[3];
                         if(!string.IsNullOrEmpty(EmisorCE[4]))
                         CE.Emisor.Domicilio.NumeroInterior = EmisorCE[4];
                         if (!string.IsNullOrEmpty(EmisorCE[5]))
                         {
                             CE.Emisor.Domicilio.ColoniaSpecified = true;
                             CE.Emisor.Domicilio.Colonia = EmisorCE[5];
                         }
                         else CE.Emisor.Domicilio.ColoniaSpecified = false;
                         if (!string.IsNullOrEmpty(EmisorCE[6]))
                         {
                             CE.Emisor.Domicilio.LocalidadSpecified = true;
                             CE.Emisor.Domicilio.Localidad = EmisorCE[6];
                         }
                         else CE.Emisor.Domicilio.LocalidadSpecified = false;
                         if (!string.IsNullOrEmpty(EmisorCE[7]))
                         {
                             CE.Emisor.Domicilio.Referencia = EmisorCE[7];
                         }
                         if (!string.IsNullOrEmpty(EmisorCE[8]))
                         {
                             CE.Emisor.Domicilio.MunicipioSpecified = true;
                             CE.Emisor.Domicilio.Municipio = EmisorCE[8];
                         }
                         else CE.Emisor.Domicilio.MunicipioSpecified = false;
                         CE.Emisor.Domicilio.Estado = EmisorCE[9];
                         CE.Emisor.Domicilio.Pais = EmisorCE[10];
                         CE.Emisor.Domicilio.CodigoPostal = EmisorCE[11];


                     }
                 #endregion
                  #region Receptor
                  if (ReceptorCE != null)
                  {
                      CE.Receptor = new ComercioExteriorReceptor();
                      CE.Receptor.Domicilio = new ComercioExteriorReceptorDomicilio();
                      if (!string.IsNullOrEmpty(ReceptorCE[1]))
                          CE.Receptor.NumRegIdTrib = ReceptorCE[1];
                      CE.Receptor.Domicilio.Calle = ReceptorCE[2];
                      if (!string.IsNullOrEmpty(ReceptorCE[3]))
                       CE.Receptor.Domicilio.NumeroExterior = ReceptorCE[3];
                      if (!string.IsNullOrEmpty(ReceptorCE[4]))
                          CE.Receptor.Domicilio.NumeroInterior = ReceptorCE[4];
                      if (!string.IsNullOrEmpty(ReceptorCE[5]))
                          CE.Receptor.Domicilio.Colonia = ReceptorCE[5];
                      if (!string.IsNullOrEmpty(ReceptorCE[6]))
                          CE.Receptor.Domicilio.Localidad = ReceptorCE[6];
                      if (!string.IsNullOrEmpty(ReceptorCE[7]))
                          CE.Receptor.Domicilio.Referencia = ReceptorCE[7];
                      if (!string.IsNullOrEmpty(ReceptorCE[8]))
                          CE.Receptor.Domicilio.Municipio = ReceptorCE[8];
                      CE.Receptor.Domicilio.Estado = ReceptorCE[9];
                      CE.Receptor.Domicilio.Pais = ReceptorCE[10];
                      CE.Receptor.Domicilio.CodigoPostal = ReceptorCE[11];
                  }
                 #endregion
                  //-------------------------------------------
                  #region Propietario
                  if (PropietarioCE.Any())
                  {
                      List<ComercioExteriorPropietario> CPR = new List<ComercioExteriorPropietario>();
                      
                      foreach (var pr in PropietarioCE)
                      {
                          ComercioExteriorPropietario cp = new ComercioExteriorPropietario();
                          cp.NumRegIdTrib = pr[1];
                          cp.ResidenciaFiscal = pr[2];
                          CPR.Add(cp);
                      }
                      if (CP != null)
                          if (CP.Count() > 0)
                              CE.Propietario = CPR.ToArray();
                  }
                  //-------------------------------------------
            #endregion

                  #region Destinatario

                  if (DestinatarioCE.Any())
                  {
                      CE.Destinatario = new List<ComercioExteriorDestinatario>();
                      foreach (var de in DestinatarioCE)
                      {
                          ComercioExteriorDestinatario d = new ComercioExteriorDestinatario();
                          if(!string.IsNullOrEmpty( de[2]))
                          d.NumRegIdTrib = de[2];
                          if (!string.IsNullOrEmpty(de[3]))
                              d.Nombre = de[3];
                            var DCE = DestinatarioCEDomi.Where(j => (j[1] == de[1]));
                            if (DCE.Any())
                            {

                                d.Domicilio = new List<ComercioExteriorDestinatarioDomicilio>();
                                foreach (var dce in DCE)
                                {

                                    ComercioExteriorDestinatarioDomicilio dom = new ComercioExteriorDestinatarioDomicilio();
                                    dom.Calle = dce[2];
                                    if (!string.IsNullOrEmpty(dce[3]))
                                        dom.NumeroExterior = dce[3];
                                    if (!string.IsNullOrEmpty(dce[4]))
                                        dom.NumeroInterior= dce[4];
                                    if (!string.IsNullOrEmpty(dce[5]))
                                        dom.Colonia = dce[5];
                                    if (!string.IsNullOrEmpty(dce[6]))
                                        dom.Localidad = dce[6];
                                    if (!string.IsNullOrEmpty(dce[7]))
                                        dom.Referencia = dce[7];
                                    if (!string.IsNullOrEmpty(dce[8]))
                                        dom.Municipio = dce[8];
                                    dom.Estado = dce[9];
                                    dom.Pais = dce[10];
                                    dom.CodigoPostal = dce[11];

                                    d.Domicilio.Add(dom);
                                }
                                
                            }
                          CE.Destinatario.Add(d);
                      }


                  }

                  #endregion
                  //---------------------------------------------
                  #region mercancia

                  if (MercanciaCE.Any())
                  {
                      CE.Mercancias = new List<ComercioExteriorMercancia>();
                      foreach (var m in MercanciaCE)
                      {
                          ComercioExteriorMercancia me = new ComercioExteriorMercancia();
                          me.NoIdentificacion = m[2];
                          if (!string.IsNullOrEmpty(m[3]))
                          {
                              me.FraccionArancelariaSpecified = true;
                              me.FraccionArancelaria = m[3];
                          }
                          else me.FraccionArancelariaSpecified = false;
                          if (!string.IsNullOrEmpty(m[4]))
                          {
                              me.CantidadAduanaSpecified = true;
                              me.CantidadAduana =Convert.ToDecimal( m[4]);
                          }
                          else me.CantidadAduanaSpecified = false;
                          if (!string.IsNullOrEmpty(m[5]))
                          {
                              me.UnidadAduanaSpecified = true;
                              me.UnidadAduana = m[5];
                          }
                          else me.UnidadAduanaSpecified = false;
                          if (!string.IsNullOrEmpty(m[6]))
                          {
                              me.ValorUnitarioAduanaSpecified = true;
                              me.ValorUnitarioAduana = Convert.ToDecimal(m[6]);
                          }
                          else me.ValorUnitarioAduanaSpecified = false;

                          me.ValorDolares = Convert.ToDecimal(m[7]);

                          var ME = DescripcionesCE.Where(j => (j[1] == m[1]));
                          if (ME.Any())
                          {
                             // me.DescripcionesEspecificas = new List<ComercioExteriorMercanciaDescripcionesEspecificas>();
                              List<ComercioExteriorMercanciaDescripcionesEspecificas> DescripcionEspecificas = new List<ComercioExteriorMercanciaDescripcionesEspecificas>();
                              foreach (var mer in ME)
                              {
                                  ComercioExteriorMercanciaDescripcionesEspecificas des = new ComercioExteriorMercanciaDescripcionesEspecificas();
                                  des.Marca = mer[2];
                                  if(!string.IsNullOrEmpty(mer[3]))
                                  des.Modelo = mer[3];
                                  if (!string.IsNullOrEmpty(mer[4]))
                                      des.SubModelo= mer[4];
                                  if (!string.IsNullOrEmpty(mer[5]))
                                      des.NumeroSerie = mer[5];
                                 DescripcionEspecificas.Add(des);
                              }
                              //
                              if (DescripcionEspecificas!=null)
                                  if(DescripcionEspecificas.Count()>0)
                              me.DescripcionesEspecificas = DescripcionEspecificas.ToArray();
                              //
                          }

                          CE.Mercancias.Add(me);
                      }
                  }
                  #endregion

                  if (comprobante.Complemento == null)
                    comprobante.Complemento = new GeneradorCfdi.ComprobanteComplemento();

                  comprobante.Complemento.comercioExterior = CE;
            }

            #endregion
               
            #region donatarias
            if (Donat != null)
            {
                      if (comprobante.Complemento == null)
                    comprobante.Complemento = new GeneradorCfdi.ComprobanteComplemento();

                      comprobante.Complemento.Donat = new Donatarias();
                      comprobante.Complemento.Donat.leyenda = Donat[1];
                    //  comprobante.Complemento.Donat.fechaAutorizacion = DateTime.ParseExact(Donat[2], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToString();
                    //  comprobante.Complemento.Donat.fechaAutorizacion = Convert.ToDateTime(comprobante.Complemento.Donat.fechaAutorizacion).ToString("s");
                      comprobante.Complemento.Donat.fechaAutorizacion =Convert.ToDateTime( Donat[2]);
                      comprobante.Complemento.Donat.noAutorizacion = Donat[3];
                      comprobante.Complemento.Donat.version = "1.1";


                      comprobante.DonatAprobacion = Donat[3];
                      comprobante.DonatFecha = Donat[2];
                      comprobante.DonatLeyenda = Donat[1];
            }
            #endregion

            #region cartaPorte
            if (CP != null)
            {
                if (comprobante.Complemento == null)
                    comprobante.Complemento = new GeneradorCfdi.ComprobanteComplemento();

                comprobante.Complemento.cartaPorte = new CartaPorte();
                comprobante.Complemento.cartaPorte.Version = CP[5];
                comprobante.Complemento.cartaPorte.TranspInternac = CP[1];
                //comprobante.Complemento.cartaPorte.EntradaSalidaMerc = CP[2];

                if (!string.IsNullOrEmpty(CP[2]))
                {
                    comprobante.Complemento.cartaPorte.EntradaSalidaMerc = CP[2];
                    comprobante.Complemento.cartaPorte.EntradaSalidaMercSpecified = true;
                }
                else
                    comprobante.Complemento.cartaPorte.EntradaSalidaMercSpecified = false;

                if (!string.IsNullOrEmpty(CP[3]))
                {
                    comprobante.Complemento.cartaPorte.ViaEntradaSalida = CP[3];
                    comprobante.Complemento.cartaPorte.ViaEntradaSalidaSpecified = true;
                }
                else
                    comprobante.Complemento.cartaPorte.ViaEntradaSalidaSpecified = false;

                if (!string.IsNullOrEmpty(CP[4]))
                {
                    comprobante.Complemento.cartaPorte.TotalDistRec =Convert.ToDecimal(CP[4]);
                    comprobante.Complemento.cartaPorte.TotalDistRecSpecified = true;
                }
                else
                    comprobante.Complemento.cartaPorte.TotalDistRecSpecified = false;
                //------------
                #region complementoCartaPorte_Ubicacion
                if (UCP.Any())
                  {
                                
                    List<CartaPorteUbicacion> LU = new List<CartaPorteUbicacion>();
                    foreach (var u in UCP)
                    {
                        CartaPorteUbicacion C = new CartaPorteUbicacion();
                        if (!string.IsNullOrEmpty(u[2]))
                        {
                            C.DistanciaRecorrida = Convert.ToDecimal(u[2]);
                            C.DistanciaRecorridaSpecified = true;
                        }
                        else
                            C.DistanciaRecorridaSpecified = false;
                        if (!string.IsNullOrEmpty(u[3]))
                        {
                            C.TipoEstacionSpecified = true;
                            C.TipoEstacion = u[3];
                        }
                        else
                            C.TipoEstacionSpecified = false;

                        if (OCP.Any())
                        {
                            var ocp = OCP.FirstOrDefault(j => (j[1] == u[1]));
                            if (ocp != null)
                            {
                                C.Origen = new CartaPorteUbicacionOrigen();
                                if (!string.IsNullOrEmpty(ocp[2]))
                                    C.Origen.IDOrigen = ocp[2];
                                C.Origen.FechaHoraSalida = Convert.ToDateTime(ocp[3]).ToString("s");
                                if (!string.IsNullOrEmpty(ocp[4]))
                                {
                                    C.Origen.NavegacionTrafico = ocp[4];
                                    C.Origen.NavegacionTraficoSpecified = true;
                                }
                                else
                                    C.Origen.NavegacionTraficoSpecified = false;
                                if (!string.IsNullOrEmpty(ocp[5]))
                                    C.Origen.NombreEstacion = ocp[5];
                                if (!string.IsNullOrEmpty(ocp[6]))
                                    C.Origen.NombreRemitente = ocp[6];
                                if (!string.IsNullOrEmpty(ocp[7]))
                                {
                                    C.Origen.NumEstacion = ocp[7];
                                    C.Origen.NumEstacionSpecified = true;
                                }
                                else
                                    C.Origen.NumEstacionSpecified = false;
                                if (!string.IsNullOrEmpty(ocp[8]))
                                    C.Origen.NumRegIdTrib = ocp[8];
                                if (!string.IsNullOrEmpty(ocp[9]))
                                {
                                    C.Origen.ResidenciaFiscal = ocp[9];
                                    C.Origen.ResidenciaFiscalSpecified = true;
                                }
                                else
                                    C.Origen.ResidenciaFiscalSpecified = false;
                                if (!string.IsNullOrEmpty(ocp[10]))
                                    C.Origen.RFCRemitente = ocp[10];
                            }
                        }

                        if (DCP.Any())
                        {
                            var dcp = DCP.FirstOrDefault(j => (j[1] == u[1]));
                            if (dcp != null)
                            {

                                C.Destino = new CartaPorteUbicacionDestino();
                                if (!string.IsNullOrEmpty(dcp[2]))
                                    C.Destino.IDDestino = dcp[2];
                                C.Destino.FechaHoraProgLlegada = Convert.ToDateTime(dcp[3]).ToString("s"); ;
                                if (!string.IsNullOrEmpty(dcp[4]))
                                {
                                    C.Destino.NavegacionTrafico = dcp[4];
                                    C.Destino.NavegacionTraficoSpecified = true;
                                }
                                else
                                    C.Destino.NavegacionTraficoSpecified = false;
                                if (!string.IsNullOrEmpty(dcp[5]))
                                    C.Destino.NombreEstacion = dcp[5];
                                if (!string.IsNullOrEmpty(dcp[6]))
                                    C.Destino.NombreDestinatario = dcp[6];
                                if (!string.IsNullOrEmpty(dcp[7]))
                                {
                                    C.Destino.NumEstacion = dcp[7];
                                    C.Destino.NumEstacionSpecified = true;
                                }
                                else
                                    C.Destino.NumEstacionSpecified = false;
                                if (!string.IsNullOrEmpty(dcp[8]))
                                    C.Destino.NumRegIdTrib = dcp[8];
                                if (!string.IsNullOrEmpty(dcp[9]))
                                {
                                    C.Destino.ResidenciaFiscal = dcp[9];
                                    C.Destino.ResidenciaFiscalSpecified = true;
                                }
                                else
                                    C.Destino.ResidenciaFiscalSpecified = false;
                                if (!string.IsNullOrEmpty(dcp[10]))
                                    C.Destino.RFCDestinatario = dcp[10];
                            }
                        }
                        if (DOCP.Any())
                        {
                            var docp = DOCP.FirstOrDefault(j => (j[1] == u[1]));
                            if (docp != null)
                            {
                                C.Domicilio = new CartaPorteUbicacionDomicilio();
                                C.Domicilio.Calle = docp[2];
                                C.Domicilio.CodigoPostal = docp[3];
                                if (!string.IsNullOrEmpty(docp[4]))
                                    C.Domicilio.Colonia = docp[4];
                                C.Domicilio.Estado = docp[5];
                                if (!string.IsNullOrEmpty(docp[6]))
                                    C.Domicilio.Localidad = docp[6];
                                if (!string.IsNullOrEmpty(docp[7]))
                                    C.Domicilio.Municipio = docp[7];
                                if (!string.IsNullOrEmpty(docp[8]))
                                    C.Domicilio.NumeroExterior = docp[8];
                                if (!string.IsNullOrEmpty(docp[9]))
                                    C.Domicilio.NumeroInterior = docp[9];
                                C.Domicilio.Pais = docp[10];
                                if (!string.IsNullOrEmpty(docp[11]))
                                    C.Domicilio.Referencia = docp[11];


                            }
                        }
                        LU.Add(C);
                    }

                    comprobante.Complemento.cartaPorte.Ubicaciones = LU.ToArray();
                  }
                #endregion

                #region complementoCartaPorte_Mercancias

                ///--------------
                if (MCP != null)
                {
                    comprobante.Complemento.cartaPorte.Mercancias = new CartaPorteMercancias();
                    if (!string.IsNullOrEmpty(MCP[1]))
                    {
                        comprobante.Complemento.cartaPorte.Mercancias.CargoPorTasacion = Convert.ToDecimal(MCP[1]);
                        comprobante.Complemento.cartaPorte.Mercancias.CargoPorTasacionSpecified = true;
                    }
                    else
                        comprobante.Complemento.cartaPorte.Mercancias.CargoPorTasacionSpecified = false;
                    comprobante.Complemento.cartaPorte.Mercancias.NumTotalMercancias = Convert.ToInt32(MCP[2]);
                    if (!string.IsNullOrEmpty(MCP[3]))
                    {
                        comprobante.Complemento.cartaPorte.Mercancias.PesoBrutoTotal = Convert.ToDecimal(MCP[3]);
                        comprobante.Complemento.cartaPorte.Mercancias.PesoBrutoTotalSpecified = true;
                    }
                    else
                        comprobante.Complemento.cartaPorte.Mercancias.PesoBrutoTotalSpecified = false;
                    if (!string.IsNullOrEmpty(MCP[4]))
                    {
                        comprobante.Complemento.cartaPorte.Mercancias.PesoNetoTotal = Convert.ToDecimal(MCP[4]);
                        comprobante.Complemento.cartaPorte.Mercancias.PesoNetoTotalSpecified = true;
                    }
                    else
                        comprobante.Complemento.cartaPorte.Mercancias.PesoNetoTotalSpecified = false;
                    if (!string.IsNullOrEmpty(MCP[5]))
                    {
                        comprobante.Complemento.cartaPorte.Mercancias.UnidadPeso = MCP[5];
                        comprobante.Complemento.cartaPorte.Mercancias.UnidadPesoSpecified = true;
                    }
                    else
                        comprobante.Complemento.cartaPorte.Mercancias.UnidadPesoSpecified = false;

                    //----------------      
                    #region complementoCartaPorte_Mercancia
                    if (MECP.Any())
                    {

                        List<CartaPorteMercanciasMercancia> LM = new List<CartaPorteMercanciasMercancia>();
                        foreach (var m in MECP)
                        {
                            CartaPorteMercanciasMercancia M = new CartaPorteMercanciasMercancia();
                            if (!string.IsNullOrEmpty(m[2]))
                            {
                                M.BienesTransp = m[2];
                                M.BienesTranspSpecified = true;
                            }
                            else
                            {
                                M.BienesTranspSpecified = false;
                            }
                            if (!string.IsNullOrEmpty(m[3]))
                                M.Cantidad = m[3];

                            if (!string.IsNullOrEmpty(m[4]))
                            {
                                M.ClaveSTCC = m[4];
                                M.ClaveSTCCSpecified = true;
                            }
                            else
                                M.ClaveSTCCSpecified = false;
                            if (!string.IsNullOrEmpty(m[5]))
                                M.ClaveUnidad = m[5];
                            if (!string.IsNullOrEmpty(m[6]))
                            {
                                M.CveMaterialPeligroso = m[6];
                                M.CveMaterialPeligrosoSpecified = true;
                            }
                            else
                                M.CveMaterialPeligrosoSpecified = false;
                            if (!string.IsNullOrEmpty(m[7]))
                                M.Descripcion = m[7];
                            if (!string.IsNullOrEmpty(m[8]))
                                M.DescripEmbalaje = m[8];
                            if (!string.IsNullOrEmpty(m[9]))
                                M.Dimensiones = m[9];
                            if (!string.IsNullOrEmpty(m[10]))
                            {
                                M.Embalaje = m[10];
                                M.EmbalajeSpecified = true;
                            }
                            else
                                M.EmbalajeSpecified = false;
                            if (!string.IsNullOrEmpty(m[11]))
                            {
                                M.FraccionArancelaria = m[11];
                                M.FraccionArancelariaSpecified = true;
                            }
                            else
                                M.FraccionArancelariaSpecified = false;
                            if (!string.IsNullOrEmpty(m[12]))
                            {
                                M.MaterialPeligroso = m[12];
                                M.MaterialPeligrosoSpecified = true;
                            }
                            else
                                M.MaterialPeligrosoSpecified = false;
                            if (!string.IsNullOrEmpty(m[13]))
                            {
                                M.Moneda = m[13];
                                M.MonedaSpecified = true;
                            }
                            else
                                M.MonedaSpecified = false;
                            M.PesoEnKg = Convert.ToDecimal(m[14]);
                            if (!string.IsNullOrEmpty(m[15]))
                                M.Unidad = m[15];
                            if (!string.IsNullOrEmpty(m[16]))
                                M.UUIDComercioExt = m[16];
                            if (!string.IsNullOrEmpty(m[17]) )
                            {
                                M.ValorMercancia = Convert.ToDecimal(m[17]);
                                M.ValorMercanciaSpecified = true;
                            }
                            else
                                M.ValorMercanciaSpecified = false;
                            if (DMCP.Any())
                            {
                                var dmcp = DMCP.FirstOrDefault(j => (j[1] == m[1]));
                                if (dmcp != null)
                                {
                                    M.DetalleMercancia = new CartaPorteMercanciasMercanciaDetalleMercancia();
                                    if (!string.IsNullOrEmpty(dmcp[2]))
                                    {
                                        M.DetalleMercancia.NumPiezas = Convert.ToInt16(dmcp[2]);
                                        M.DetalleMercancia.NumPiezasSpecified = true;
                                    }
                                    else
                                        M.DetalleMercancia.NumPiezasSpecified = false;

                                    M.DetalleMercancia.PesoBruto = Convert.ToDecimal(dmcp[3]);
                                    M.DetalleMercancia.PesoNeto = Convert.ToDecimal(dmcp[4]);
                                    M.DetalleMercancia.PesoTara = Convert.ToDecimal(dmcp[5]);
                                    M.DetalleMercancia.UnidadPeso = dmcp[6];
                                }

                            }
                            if (CTCP.Any())
                            {
                                var ctcp = CTCP.Where(j => (j[1] == m[1]));
                                if (ctcp != null)
                                {
                                    List<CartaPorteMercanciasMercanciaCantidadTransporta> CANT = new List<CartaPorteMercanciasMercanciaCantidadTransporta>();
                                    foreach (var ctc in ctcp)
                                    {
                                        CartaPorteMercanciasMercanciaCantidadTransporta ct = new CartaPorteMercanciasMercanciaCantidadTransporta();
                                        ct.Cantidad = Convert.ToDecimal(ctc[2]);
                                        if (!string.IsNullOrEmpty(ctc[3]))
                                        {
                                            ct.CvesTransporte = ctc[3];
                                            ct.CvesTransporteSpecified = true;
                                        }
                                        else
                                            ct.CvesTransporteSpecified = false;
                                        ct.IDDestino = ctc[4];
                                        ct.IDOrigen = ctc[5];
                                        CANT.Add(ct);

                                    }
                                    M.CantidadTransporta = CANT.ToArray();
                                }
                            }

                            LM.Add(M);
                        }

                        comprobante.Complemento.cartaPorte.Mercancias.Mercancia = LM.ToArray();

                    }
                    #endregion
                    //--------------------------------
                    #region complementoCartaPorte_AutoTransporteFederal

                    if (ATCP != null)
                    {
                        comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal = new CartaPorteMercanciasAutotransporteFederal();
                        comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal.NombreAseg = ATCP[1];
                        comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal.NumPermisoSCT = ATCP[2];
                        comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal.NumPolizaSeguro = ATCP[3];
                        comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal.PermSCT = ATCP[4];
                        //               
                        comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal.IdentificacionVehicular = new CartaPorteMercanciasAutotransporteFederalIdentificacionVehicular();
                        comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal.IdentificacionVehicular.AnioModeloVM = Convert.ToInt16(ATCP[5]);
                        comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal.IdentificacionVehicular.ConfigVehicular = ATCP[6];
                        comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal.IdentificacionVehicular.PlacaVM = ATCP[7];
                        if (ATRCP.Any())
                        {
                            List<CartaPorteMercanciasAutotransporteFederalRemolque> LR = new List<CartaPorteMercanciasAutotransporteFederalRemolque>();
                            foreach (var a in ATRCP)
                            {
                                CartaPorteMercanciasAutotransporteFederalRemolque R = new CartaPorteMercanciasAutotransporteFederalRemolque();
                                R.Placa = a[1];
                                R.SubTipoRem = a[2];
                                LR.Add(R);

                            }
                            comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal.Remolques = LR.ToArray();
                        }

                    }

                    #endregion

                    #region complementoCartaPorte_TransporteMaritimo

                    if (TMCP != null)
                    {
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo = new CartaPorteMercanciasTransporteMaritimo();
                        if (!string.IsNullOrEmpty(TMCP[1]))
                        {
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.AnioEmbarcacion = Convert.ToInt32(TMCP[1]);
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.AnioEmbarcacionSpecified = true;
                        }
                        else
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.AnioEmbarcacionSpecified = false;
                        if (!string.IsNullOrEmpty(TMCP[2]))
                        {
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.Calado = Convert.ToDecimal(TMCP[2]);
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.CaladoSpecified = true;
                        }
                        else
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.CaladoSpecified = false;
                        if (!string.IsNullOrEmpty(TMCP[3]))
                        {
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.Eslora = Convert.ToDecimal(TMCP[3]);
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.EsloraSpecified = true;
                        }
                        else
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.EsloraSpecified = false;
                        if (!string.IsNullOrEmpty(TMCP[4]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.LineaNaviera = TMCP[4];
                        if (!string.IsNullOrEmpty(TMCP[5]))
                        {
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.Manga = Convert.ToDecimal(TMCP[5]);
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.MangaSpecified = true;
                        }
                        else
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.MangaSpecified = false;

                        comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.Matricula = TMCP[6];
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.NacionalidadEmbarc = TMCP[7];
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.NombreAgenteNaviero = TMCP[8];
                        if (!string.IsNullOrEmpty(TMCP[9]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.NombreAseg = TMCP[9];
                        if (!string.IsNullOrEmpty(TMCP[10]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.NombreEmbarc = TMCP[10];
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.NumAutorizacionNaviero = TMCP[11];
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.NumCertITC = TMCP[12];
                        if (!string.IsNullOrEmpty(TMCP[13]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.NumConocEmbarc = TMCP[13];
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.NumeroOMI = TMCP[14];
                        if (!string.IsNullOrEmpty(TMCP[15]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.NumPermisoSCT = TMCP[15];
                        if (!string.IsNullOrEmpty(TMCP[16]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.NumPolizaSeguro = TMCP[16];
                        if (!string.IsNullOrEmpty(TMCP[17]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.NumViaje = TMCP[17];
                        if (!string.IsNullOrEmpty(TMCP[18]))
                        {
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.PermSCT = TMCP[18];
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.PermSCTSpecified = true;
                        }
                        else
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.PermSCTSpecified = false;
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.TipoCarga = TMCP[19];
                        if (!string.IsNullOrEmpty(TMCP[20]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.TipoEmbarcacion = TMCP[20];
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.UnidadesDeArqBruto = Convert.ToDecimal(TMCP[21]);

                        if (TMCCP.Any())
                        {
                            List<CartaPorteMercanciasTransporteMaritimoContenedor> CON = new List<CartaPorteMercanciasTransporteMaritimoContenedor>();
                            foreach (var con in TMCCP)
                            {
                                CartaPorteMercanciasTransporteMaritimoContenedor conte = new CartaPorteMercanciasTransporteMaritimoContenedor();
                                conte.MatriculaContenedor = con[1];
                                if (!string.IsNullOrEmpty(con[2]))
                                    conte.NumPrecinto = con[2];
                                conte.TipoContenedor = con[3];
                                CON.Add(conte);
                            }

                            comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo.Contenedor = CON.ToArray();
                        }



                    }

                    #endregion

                    //-----------------------------------
                    #region complementoCartaPorte_TransporteAereo

                    if (TACP != null)
                    {
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo = new CartaPorteMercanciasTransporteAereo();
                        if (!string.IsNullOrEmpty(TACP[1]))
                        {
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.CodigoTransportista = TACP[1];
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.CodigoTransportistaSpecified = true;
                        }
                        else
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.CodigoTransportistaSpecified = false;
                        if (!string.IsNullOrEmpty(TACP[2]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.LugarContrato = TACP[2];
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.MatriculaAeronave = TACP[3];
                        if (!string.IsNullOrEmpty(TACP[4]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.NombreAseg = TACP[4];
                        if (!string.IsNullOrEmpty(TACP[5]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.NombreEmbarcador = TACP[5];
                        if (!string.IsNullOrEmpty(TACP[6]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.NombreTransportista = TACP[6];
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.NumeroGuia = TACP[7];
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.NumPermisoSCT = TACP[8];
                        if (!string.IsNullOrEmpty(TACP[9]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.NumPolizaSeguro = TACP[9];
                        if (!string.IsNullOrEmpty(TACP[10]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.NumRegIdTribEmbarc = TACP[10];
                        if (!string.IsNullOrEmpty(TACP[11]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.NumRegIdTribTranspor = TACP[11];
                        if (!string.IsNullOrEmpty(TACP[12]))
                        {
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.PermSCT = TACP[12];
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.PermSCTSpecified = true;
                        }
                        else
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.PermSCTSpecified = false;
                        if (!string.IsNullOrEmpty(TACP[13]))
                        {
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.ResidenciaFiscalEmbarc = TACP[13];
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.ResidenciaFiscalEmbarcSpecified = true;
                        }
                        else
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.ResidenciaFiscalEmbarcSpecified = false;
                        if (!string.IsNullOrEmpty(TACP[14]))
                        {
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.ResidenciaFiscalTranspor = TACP[14];
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.ResidenciaFiscalTransporSpecified = true;
                        }
                        else
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.ResidenciaFiscalTransporSpecified = false;
                        if (!string.IsNullOrEmpty(TACP[15]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.RFCEmbarcador = TACP[15];
                        if (!string.IsNullOrEmpty(TACP[16]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo.RFCTransportista = TACP[16];


                    }
                    #endregion
                    //---------------------------------
                    #region complementoCartaPorte_TransporteFerroviario
                    if (TFCP != null)
                    {
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteFerroviario = new CartaPorteMercanciasTransporteFerroviario();
                        comprobante.Complemento.cartaPorte.Mercancias.TransporteFerroviario.TipoDeServicio = TFCP[1];
                        if (!string.IsNullOrEmpty(TFCP[2]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteFerroviario.Concesionario = TFCP[2];
                        if (!string.IsNullOrEmpty(TFCP[3]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteFerroviario.NombreAseg = TFCP[3];
                        if (!string.IsNullOrEmpty(TFCP[4]))
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteFerroviario.NumPolizaSeguro = TFCP[4];

                        if (TFDCCP.Any())
                        {
                            List<CartaPorteMercanciasTransporteFerroviarioDerechosDePaso> D = new List<CartaPorteMercanciasTransporteFerroviarioDerechosDePaso>();
                            foreach (var des in TFDCCP)
                            {
                                CartaPorteMercanciasTransporteFerroviarioDerechosDePaso d = new CartaPorteMercanciasTransporteFerroviarioDerechosDePaso();
                                d.KilometrajePagado = Convert.ToDecimal(des[1]);
                                d.TipoDerechoDePaso = des[2];
                                D.Add(d);
                            }
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteFerroviario.DerechosDePaso = D.ToArray();
                        }

                        if (TFCCCP.Any())
                        {
                            List<CartaPorteMercanciasTransporteFerroviarioCarro> CA = new List<CartaPorteMercanciasTransporteFerroviarioCarro>();
                            foreach (var ca in TFCCCP)
                            {
                                CartaPorteMercanciasTransporteFerroviarioCarro cr = new CartaPorteMercanciasTransporteFerroviarioCarro();
                                cr.GuiaCarro = ca[2];
                                cr.MatriculaCarro = ca[3];
                                cr.TipoCarro = ca[4];
                                cr.ToneladasNetasCarro = Convert.ToDecimal(ca[5]);
                                if (TFCCCCP.Any())
                                {
                                    var tfc = TFCCCCP.Where(j => (j[1] == ca[1]));
                                    if (tfc != null)
                                    {
                                        List<CartaPorteMercanciasTransporteFerroviarioCarroContenedor> CAC = new List<CartaPorteMercanciasTransporteFerroviarioCarroContenedor>();
                                        foreach (var cc in tfc)
                                        {
                                            CartaPorteMercanciasTransporteFerroviarioCarroContenedor cac = new CartaPorteMercanciasTransporteFerroviarioCarroContenedor();
                                            cac.PesoContenedorVacio = Convert.ToDecimal(cc[2]);
                                            cac.PesoNetoMercancia = Convert.ToDecimal(cc[3]);
                                            cac.TipoContenedor = cc[4];
                                            CAC.Add(cac);
                                        }
                                        cr.Contenedor = CAC.ToArray();
                                    }
                                }
                                CA.Add(cr);
                            }
                            comprobante.Complemento.cartaPorte.Mercancias.TransporteFerroviario.Carro = CA.ToArray();
                        }

                    }
                    #endregion
                    //-----------------------------
                }
            #endregion
               
                 if (FTCP!=null)
                {
                    comprobante.Complemento.cartaPorte.FiguraTransporte = new CartaPorteFiguraTransporte();
                    comprobante.Complemento.cartaPorte.FiguraTransporte.CveTransporte = FTCP[1];

                   #region complementoCartaPorte_Operador
                   if (OPCP.Any())
                    {
                        List<CartaPorteFiguraTransporteOperadores> OO = new List<CartaPorteFiguraTransporteOperadores>();
                        CartaPorteFiguraTransporteOperadores O1 = new CartaPorteFiguraTransporteOperadores();
                        List<CartaPorteFiguraTransporteOperadoresOperador> O = new List<CartaPorteFiguraTransporteOperadoresOperador>();
                        foreach (var op in OPCP)
                        {
                            CartaPorteFiguraTransporteOperadoresOperador o = new CartaPorteFiguraTransporteOperadoresOperador();
                            if (!string.IsNullOrEmpty(op[1]))
                                o.NombreOperador = op[1];
                            if (!string.IsNullOrEmpty(op[2]))
                                o.NumLicencia = op[2];
                            if (!string.IsNullOrEmpty(op[3]))
                                o.NumRegIdTribOperador = op[3];
                            if (!string.IsNullOrEmpty(op[4]))
                            {
                                o.ResidenciaFiscalOperador = op[4];
                                o.ResidenciaFiscalOperadorSpecified = true;
                            }
                            else
                                o.ResidenciaFiscalOperadorSpecified = false;
                            if (!string.IsNullOrEmpty(op[5]))
                                o.RFCOperador = op[5];

                            if (!string.IsNullOrEmpty( op[6]))
                            {
                                o.Domicilio = new CartaPorteFiguraTransporteOperadoresOperadorDomicilio();
                                o.Domicilio.Calle = op[6];
                                o.Domicilio.CodigoPostal = op[7];
                                if (!string.IsNullOrEmpty(op[8]))
                                    o.Domicilio.Colonia = op[8];
                                o.Domicilio.Estado = op[9];
                                if (!string.IsNullOrEmpty(op[10]))
                                    o.Domicilio.Localidad =op[10] ;
                                if (!string.IsNullOrEmpty(op[11]))
                                    o.Domicilio.Municipio = op[11];
                                if (!string.IsNullOrEmpty(op[12]))
                                    o.Domicilio.NumeroExterior = op[12];
                                if (!string.IsNullOrEmpty(op[13]))
                                    o.Domicilio.NumeroInterior = op[13];
                                o.Domicilio.Pais = op[14];
                                if (!string.IsNullOrEmpty(op[15]))
                                    o.Domicilio.Referencia = op[15];

                            }
                            O.Add(o);
                        }
                        O1.Operador = O.ToArray();
                        OO.Add(O1);
                         comprobante.Complemento.cartaPorte.FiguraTransporte.Operadores = OO.ToArray();
                    }
                        #endregion
                    //---------------------------------------------
                      #region complementoCartaPorte_Propietario
                    if (PRCP.Any())
                    {
                        List<CartaPorteFiguraTransportePropietario> O = new List<CartaPorteFiguraTransportePropietario>();
                        foreach (var op in PRCP)
                        {
                            CartaPorteFiguraTransportePropietario o = new CartaPorteFiguraTransportePropietario();
                            if (!string.IsNullOrEmpty(op[1]))
                                o.NombrePropietario = op[1];
                            if (!string.IsNullOrEmpty(op[2]))
                                o.NumRegIdTribPropietario = op[2];
                            if (!string.IsNullOrEmpty(op[3]))
                            {
                                o.ResidenciaFiscalPropietario = op[3];
                                o.ResidenciaFiscalPropietarioSpecified = true;
                            }
                            else
                                o.ResidenciaFiscalPropietarioSpecified = false;
                            if (!string.IsNullOrEmpty(op[4]))
                                o.RFCPropietario = op[4];

                            if (!string.IsNullOrEmpty(op[5]))
                            {
                                o.Domicilio = new CartaPorteFiguraTransportePropietarioDomicilio();
                                o.Domicilio.Calle = op[5];
                                o.Domicilio.CodigoPostal = op[6];
                                if (!string.IsNullOrEmpty(op[7]))
                                    o.Domicilio.Colonia = op[7];
                                o.Domicilio.Estado = op[8];
                                if (!string.IsNullOrEmpty(op[9]))
                                    o.Domicilio.Localidad = op[9];
                                if (!string.IsNullOrEmpty(op[10]))
                                    o.Domicilio.Municipio = op[10];
                                if (!string.IsNullOrEmpty(op[11]))
                                    o.Domicilio.NumeroExterior = op[11];
                                if (!string.IsNullOrEmpty(op[12]))
                                    o.Domicilio.NumeroInterior = op[12];
                                o.Domicilio.Pais = op[13];
                                if (!string.IsNullOrEmpty(op[14]))
                                    o.Domicilio.Referencia = op[14];

                            }
                            O.Add(o);
                        }
                        comprobante.Complemento.cartaPorte.FiguraTransporte.Propietario = O.ToArray();
                    }
                        #endregion
                    //---------------------------------------------
                    #region complementoCartaPorte_Arrendatario
              
                     if (ARCP.Any())
                    {
                        List<CartaPorteFiguraTransporteArrendatario> O = new List<CartaPorteFiguraTransporteArrendatario>();
                        foreach (var op in ARCP)
                        {
                            CartaPorteFiguraTransporteArrendatario o = new CartaPorteFiguraTransporteArrendatario();
                            if (!string.IsNullOrEmpty(op[1]))
                                o.NombreArrendatario = op[1];
                            if (!string.IsNullOrEmpty(op[2]))
                                o.NumRegIdTribArrendatario = op[2];
                            if (!string.IsNullOrEmpty(op[3]))
                            {
                                o.ResidenciaFiscalArrendatario = op[3];
                                o.ResidenciaFiscalArrendatarioSpecified = true;
                            }
                            else
                                o.ResidenciaFiscalArrendatarioSpecified = false;
                            if (!string.IsNullOrEmpty(op[4]))
                                o.RFCArrendatario = op[4];

                            if (!string.IsNullOrEmpty(op[5]))
                            {
                                o.Domicilio = new CartaPorteFiguraTransporteArrendatarioDomicilio();
                                o.Domicilio.Calle = op[5];
                                o.Domicilio.CodigoPostal = op[6];
                                if (!string.IsNullOrEmpty(op[7]))
                                    o.Domicilio.Colonia = op[7];
                                o.Domicilio.Estado = op[8];
                                if (!string.IsNullOrEmpty(op[9]))
                                    o.Domicilio.Localidad = op[9];
                                if (!string.IsNullOrEmpty(op[10]))
                                    o.Domicilio.Municipio = op[10];
                                if (!string.IsNullOrEmpty(op[11]))
                                    o.Domicilio.NumeroExterior = op[11];
                                if (!string.IsNullOrEmpty(op[12]))
                                    o.Domicilio.NumeroInterior = op[12];
                                o.Domicilio.Pais = op[13];
                                if (!string.IsNullOrEmpty(op[14]))
                                    o.Domicilio.Referencia = op[14];

                            }
                            O.Add(o);
                        }
                        comprobante.Complemento.cartaPorte.FiguraTransporte.Arrendatario = O.ToArray();
                    }
                         #endregion
                     //---------------------------------------------
                     #region complementoCartaPorte_Notificado

                     if (NOCP.Any())
                    {
                        List<CartaPorteFiguraTransporteNotificado> O = new List<CartaPorteFiguraTransporteNotificado>();
                        foreach (var op in NOCP)
                        {
                            CartaPorteFiguraTransporteNotificado o = new CartaPorteFiguraTransporteNotificado();
                            if (!string.IsNullOrEmpty(op[1]))
                                o.NombreNotificado = op[1];
                            if (!string.IsNullOrEmpty(op[2]))
                                o.NumRegIdTribNotificado = op[2];
                            if (!string.IsNullOrEmpty(op[3]))
                            {
                                o.ResidenciaFiscalNotificado = op[3];
                                o.ResidenciaFiscalNotificadoSpecified = true;
                            }
                            else
                                o.ResidenciaFiscalNotificadoSpecified = false;
                            if (!string.IsNullOrEmpty(op[4]))
                                o.RFCNotificado = op[4];

                            if (!string.IsNullOrEmpty(op[5]))
                            {
                                o.Domicilio = new CartaPorteFiguraTransporteNotificadoDomicilio();
                                o.Domicilio.Calle = op[5];
                                o.Domicilio.CodigoPostal = op[6];
                                if (!string.IsNullOrEmpty(op[7]))
                                    o.Domicilio.Colonia = op[7];
                                o.Domicilio.Estado = op[8];
                                if (!string.IsNullOrEmpty(op[9]))
                                    o.Domicilio.Localidad = op[9];
                                if (!string.IsNullOrEmpty(op[10]))
                                    o.Domicilio.Municipio = op[10];
                                if (!string.IsNullOrEmpty(op[11]))
                                    o.Domicilio.NumeroExterior = op[11];
                                if (!string.IsNullOrEmpty(op[12]))
                                    o.Domicilio.NumeroInterior = op[12];
                                o.Domicilio.Pais = op[13];
                                if (!string.IsNullOrEmpty(op[14]))
                                    o.Domicilio.Referencia = op[14];

                            }
                            O.Add(o);
                        }
                        comprobante.Complemento.cartaPorte.FiguraTransporte.Notificado = O.ToArray();
                    }
                    //---------------------------------------------
                     #endregion

                }
                //-----------------------------------
               
                
            }
            #endregion

            //-------------------------fin complementos---------------------------------
            return comprobante;

        }

        public Comprobante ParseNominaData(string[][] datos)
        {
            #region Obtención de datos
            var comprob = datos.FirstOrDefault(p => p[0] == "COMP");

            var CfdiRelacionados = datos.FirstOrDefault(p => p[0] == "CRT");//nuevo
            var UUID = datos.Where(p => p[0] == "CRU");//nuevo
            var emisor = datos.FirstOrDefault(p => p[0] == "E");
            var receptor = datos.FirstOrDefault(p => p[0] == "R");
            //var concepts = datos.Where(p => p[0] == "C");
            var InformAdua = datos.Where(p => p[0] == "IAC");//nuevo
            var Traslados = datos.Where(p => p[0] == "ITC");//nuevo
            var Retenciones = datos.Where(p => p[0] == "IRC");//nuevo
            var Parte = datos.Where(p => p[0] == "PC");//nuevo
            var IAParte = datos.Where(p => p[0] == "IAPC");//nuevo
            var ITraslados = datos.Where(p => p[0] == "IT");//nuevo
            var IRetenciones = datos.Where(p => p[0] == "IR");//nuevo
            var ITotales = datos.FirstOrDefault(p => p[0] == "TIMP");//nuevo

            //var emisorDomicilio = datos.FirstOrDefault(p => p[0] == "DE");
            //var emitidoEn = datos.FirstOrDefault(p => p[0] == "EE");
            //var receptorDomicilio = datos.FirstOrDefault(p => p[0] == "DR");


            //---------nuevos
            var impuestosLocales = datos.FirstOrDefault(p => p[0] == "IL");
            var impuestosLocalesretenciones = datos.Where(p => p[0] == "ILR");
            var impuestosLocalestraslados = datos.Where(p => p[0] == "ILTL");
            //---------------Nomina
            var Nomina = datos.FirstOrDefault(p => p[0] == "DETALLENOMINA");
            var NPercepcion = datos.Where(p => p[0] == "NPERCEPCION");
            var NPercepcionT = datos.FirstOrDefault(p => p[0] == "NPERCEPCIONT");
            var NDeduccion = datos.Where(p => p[0] == "NDEDUCCION");
            var NDeduccionT = datos.FirstOrDefault(p => p[0] == "NDEDUCCIONT");
            var Incapacidades = datos.Where(p => p[0] == "NINCAPACIDAD");
            var HorasExtra = datos.Where(p => p[0] == "NHORASEXTRA");
            //var AccionesOTitulos = datos.FirstOrDefault(p => p[0] == "NACCIONESOTITULOS");
            var JubilacionPensionRetiro = datos.FirstOrDefault(p => p[0] == "NJUBILACIONPENSIONRETIRO");
            var SeparacionIndemnizacion = datos.FirstOrDefault(p => p[0] == "NSEPARACIONINDEMNIZACIONT");
            // var Totales = datos.FirstOrDefault(p => p[0] == "NPERCEPCIONT");
            //var SeparacionIndemnizacionT = datos.FirstOrDefault(p => p[0] == "NSEPARACIONINDEMNIZACIONT");
            //var JubilacionPensionRetiroT = datos.FirstOrDefault(p => p[0] == "NJUBILACIONPENSIONRETIROT");
            var Otros = datos.Where(p => p[0] == "NOTROSPAGOS");
            var subContratacion = datos.Where(p => p[0] == "SUBCONTRATACION");

            //---------------
            var datosAdicionales = datos.FirstOrDefault(j => j[0] == "AD");
            var fin = datos.FirstOrDefault(j => j[0] == "FIN");
            if (fin == null)
            {
                throw new IncompleteException();
            }
            Comprobante comprobante = new Comprobante();
            #endregion
            #region Emisor
            comprobante.Emisor = new GeneradorCfdi.ComprobanteEmisor();
            comprobante.Emisor.Nombre = emisor[2];
            comprobante.Emisor.RegimenFiscal = emisor[3];

            comprobante.Emisor.Rfc = emisor[1];
         
            comprobante.Emisor.direccion = emisor[4];

            #endregion
            #region Propiedades documento
            comprobante.Titulo = "Factura";
            if (comprob[5].Equals("I", StringComparison.InvariantCultureIgnoreCase))
                comprobante.TipoDeComprobante = "I";
            else
            {
                comprobante.TipoDeComprobante = "E";
                comprobante.Titulo = "Nota de Crédito";
            }
            #endregion
            #region Receptor
            comprobante.Receptor = new GeneradorCfdi.ComprobanteReceptor();
            comprobante.Receptor.Nombre = receptor[2];
            if (!String.IsNullOrEmpty(comprobante.Receptor.Nombre))
            {
                comprobante.Receptor.Nombre = receptor[2];
            }
            else
            {
                comprobante.Receptor.Nombre = " ";
            }
            comprobante.Receptor.Rfc = receptor[1];
            comprobante.Receptor.Nombre = GetValue(receptor[2]);
            comprobante.Receptor.UsoCFDI = GetValue(receptor[3]);
            if (!String.IsNullOrEmpty(receptor[4]))
            {
                comprobante.Receptor.ResidenciaFiscal = GetValue(receptor[4]);
                comprobante.Receptor.ResidenciaFiscalSpecified = true;
            }
            else
                comprobante.Receptor.ResidenciaFiscalSpecified = false;
            if (!String.IsNullOrEmpty(receptor[5]))
                comprobante.Receptor.NumRegIdTrib = GetValue(receptor[5]);

            comprobante.Receptor.Emails = receptor[6];
            comprobante.Receptor.Bcc = receptor[7];
            comprobante.Receptor.direccion = receptor[8];

      
            #endregion
            #region Expedido en
         
            #endregion
            #region Reemplazar fecha
            //comprobante.fecha = Convert.ToDateTime(DateTime.Now.ToString("s"));//entrada.fecha;
            if (ConfigurationManager.AppSettings["ReemplazarFecha"] == "1")
                comprobante.Fecha = DateTime.Now.ToString("s");
            else
            {
                try
                {
                    //  comprobante.Fecha = DateTime.ParseExact(comprob[3], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).ToString();
                    comprobante.Fecha = Convert.ToDateTime(comprob[3]).ToString("s");
                }
                catch (Exception ee)
                {
                    Logger.Error("Fecha inválida: " + comprob[3]);
                    Logger.Error(ee);
                    throw;
                }
            }
            #endregion
            #region Total, subtotal, moneda, forma de pago, serie, leyendas, notas, descuentos
            if (!string.IsNullOrEmpty(comprob[1]))
                comprobante.Serie = GetValue(comprob[1]);
            if (!string.IsNullOrEmpty(comprob[2]))
                comprobante.Folio = GetValue(comprob[2]);
            comprobante.LugarExpedicion = GetValue(comprob[4]);
            comprobante.TipoDeComprobante = GetValue(comprob[5]);
            if (!string.IsNullOrEmpty(comprob[6]))
            {
                comprobante.FormaPagoSpecified = true;
                comprobante.FormaPago = GetValue(comprob[6]);
            }
            else
                comprobante.FormaPagoSpecified = false;
            if (!string.IsNullOrEmpty(comprob[7]))
            {
                comprobante.MetodoPagoSpecified = true;
                comprobante.MetodoPago = GetValue(comprob[7]);
            }
            else
                comprobante.MetodoPagoSpecified = false;
            if (!string.IsNullOrEmpty(comprob[8]))
                comprobante.CondicionesDePago = GetValue(comprob[8]);
            comprobante.SubTotal = decimal.Parse(comprob[9]);// factura.Factura.Total.Value - factura.Factura.IVA.Value + factura.Factura.RetenciónIva;
            if (!string.IsNullOrEmpty(comprob[10]))
            {
                comprobante.Descuento = comprob[10];
                comprobante.DescuentoSpecified = true;
            }
            else
                comprobante.DescuentoSpecified = false;

            comprobante.Total = decimal.Parse(comprob[11]);
            comprobante.Moneda = GetValue(comprob[12]);
            if (!string.IsNullOrEmpty(comprob[13]))
            {
                comprobante.TipoCambio = Convert.ToDecimal(comprob[13]);
                comprobante.TipoCambioSpecified = true;
            }
            else
                comprobante.TipoCambioSpecified = false;
            if (!string.IsNullOrEmpty(comprob[14]))
                comprobante.Confirmacion = GetValue2(comprob[14]);


            // comprobante.Regimen = GetValue(emisor[3]);
            if (!string.IsNullOrEmpty(comprob[15]))
                comprobante.LeyendaSuperior = GetValue2(comprob[15]);
            if (!string.IsNullOrEmpty(comprob[16]))
                comprobante.Leyenda = GetValue2(comprob[16]);
            if (!string.IsNullOrEmpty(comprob[17]))
                comprobante.Proyecto = GetValue2(comprob[17]);
            if (!string.IsNullOrEmpty(comprob[18]))
                comprobante.nota1 = GetValue2(comprob[18]);
            if (!string.IsNullOrEmpty(comprob[19]))
                comprobante.nota2 = GetValue2(comprob[19]);

            #endregion
            #region Datos adicionales
            if (datosAdicionales != null)
            {
                DatosAdicionales da = new DatosAdicionales();
                da.CalleEmisor = datosAdicionales[1];
                da.NumExterior = datosAdicionales[2];
                da.NumInterior = datosAdicionales[3];
                da.Colonia = datosAdicionales[4];
                da.Municipio = datosAdicionales[5];
                da.Estado = datosAdicionales[6];
                da.Pais = datosAdicionales[7];
                da.CodigoPostal = datosAdicionales[8];
                da.Localidad = datosAdicionales[9];
                da.CondicionesPago = datosAdicionales[10];
                da.NumOportunidad = datosAdicionales[11];
                da.OrdenCompra = datosAdicionales[12];
                da.NombreContacto = datosAdicionales[13];
                da.Vendedor = datosAdicionales[14];
                comprobante.DatosAdicionales = da;

            }

            #endregion
            #region CfdiRelacionados
            if (CfdiRelacionados != null)
                if (CfdiRelacionados.Any())
                {
                    if (comprobante.CfdiRelacionados == null)
                        comprobante.CfdiRelacionados = new ComprobanteCfdiRelacionados();
                    comprobante.CfdiRelacionados.TipoRelacion = CfdiRelacionados[1];
                }
            #endregion
            #region CfdiRelacionadosUUDI
            if (UUID.Any())
            {
                if (comprobante.CfdiRelacionados == null)
                    comprobante.CfdiRelacionados = new ComprobanteCfdiRelacionados();
                if (comprobante.CfdiRelacionados.CfdiRelacionado == null)
                    comprobante.CfdiRelacionados.CfdiRelacionado = new List<ComprobanteCfdiRelacionadosCfdiRelacionado>();
                foreach (var uudi in UUID)
                {
                    ComprobanteCfdiRelacionadosCfdiRelacionado u = new ComprobanteCfdiRelacionadosCfdiRelacionado();
                    u.UUID = uudi[1];
                    comprobante.CfdiRelacionados.CfdiRelacionado.Add(u);
                }
            }
            #endregion

            #region Conceptos
            List<GeneradorCfdi.ComprobanteConcepto> conceptos = new List<GeneradorCfdi.ComprobanteConcepto>();
            {
                GeneradorCfdi.ComprobanteConcepto con = new GeneradorCfdi.ComprobanteConcepto();
                con.ClaveProdServ = "84111505";
                //if (!string.IsNullOrEmpty(detalle[3]))
                //    con.NoIdentificacion = detalle[3];
                con.Cantidad = 1;
                con.ClaveUnidad = "ACT";
                //if (!string.IsNullOrEmpty(detalle[6]))
                //    con.Unidad = detalle[6];
                con.Descripcion = "Pago de nómina";
                con.ValorUnitario = comprob[9];
                con.Importe = comprob[9];
                if (!string.IsNullOrEmpty(comprob[10]))
                {
                    con.DescuentoSpecified = true;
                    con.Descuento = comprob[10];
                }
                else
                    con.DescuentoSpecified = false;
             
                conceptos.Add(con);

            }
            comprobante.Conceptos = conceptos.ToArray();
            #endregion
            #region Retencion
            if (IRetenciones.Any())
            {
                if (comprobante.Impuestos == null)
                    comprobante.Impuestos = new GeneradorCfdi.ComprobanteImpuestos();

                List<GeneradorCfdi.ComprobanteImpuestosRetencion> LT = new List<GeneradorCfdi.ComprobanteImpuestosRetencion>();
                foreach (var t in IRetenciones)
                {
                    GeneradorCfdi.ComprobanteImpuestosRetencion lt = new GeneradorCfdi.ComprobanteImpuestosRetencion();
                    lt.Impuesto = t[1];
                    lt.Importe = t[2];
                    LT.Add(lt);

                }
                comprobante.Impuestos.Retenciones = LT.ToArray();
            }

            #endregion
            #region Traslados

            if (ITraslados.Any())
            {
                if (comprobante.Impuestos == null)
                    comprobante.Impuestos = new GeneradorCfdi.ComprobanteImpuestos();

                List<GeneradorCfdi.ComprobanteImpuestosTraslado> LT = new List<GeneradorCfdi.ComprobanteImpuestosTraslado>();
                foreach (var t in ITraslados)
                {
                    GeneradorCfdi.ComprobanteImpuestosTraslado lt = new GeneradorCfdi.ComprobanteImpuestosTraslado();
                    lt.Impuesto = t[1];
                    lt.TipoFactor = t[2];
                    lt.TasaOCuota = t[3];
                    lt.Importe = t[4];
                    LT.Add(lt);

                }
                comprobante.Impuestos.Traslados = LT.ToArray();
            }
            #endregion
            #region Totales Retencion Traslados
            if (ITotales != null)
            {
                if (comprobante.Impuestos == null)
                    comprobante.Impuestos = new GeneradorCfdi.ComprobanteImpuestos();
                if (!string.IsNullOrEmpty(ITotales[2]))
                {
                    comprobante.Impuestos.TotalImpuestosRetenidosSpecified = true;
                    comprobante.Impuestos.TotalImpuestosRetenidos = ITotales[2];
                }
                else
                    comprobante.Impuestos.TotalImpuestosRetenidosSpecified = false;
                if (!string.IsNullOrEmpty(ITotales[1]))
                {
                    comprobante.Impuestos.TotalImpuestosTrasladadosSpecified = true;
                    comprobante.Impuestos.TotalImpuestosTrasladados = ITotales[1];
                }
                else
                    comprobante.Impuestos.TotalImpuestosTrasladadosSpecified = false;
            }

            #endregion


            #region Impuestos, cantidad en letra
            comprobante.CantidadLetra = CantidadLetra.Enletras(comprobante.Total.ToString(), comprobante.Moneda, comprobante.Emisor.Rfc);


            //comprobante.Impuestos = impuestos;
            #endregion
            #region impuestos locales
            //----------------------esto es nuevo para los impuestos locales
            if (impuestosLocales != null)
            {
                comprobante.Complemento = new GeneradorCfdi.ComprobanteComplemento();
                comprobante.Complemento.implocal = new ImpuestosLocales();

                comprobante.Complemento.implocal.TotaldeRetenciones = Convert.ToDecimal(impuestosLocales[1]);
                comprobante.Complemento.implocal.TotaldeTraslados = Convert.ToDecimal(impuestosLocales[2]);
                comprobante.Complemento.implocal.version = impuestosLocales[3];
                //  comprobante.Complemento.imlocal.TrasladosLocales = new ImpuestosLocalesTrasladosLocales();
                //--------------------------------------------------------
                if (impuestosLocalestraslados.Any())
                {
                    List<ImpuestosLocalesTrasladosLocales> LITL = new List<ImpuestosLocalesTrasladosLocales>();
                    foreach (var imt in impuestosLocalestraslados)
                    {
                        ImpuestosLocalesTrasladosLocales litl = new ImpuestosLocalesTrasladosLocales();
                        litl.ImpLocTrasladado = imt[1];
                        litl.Importe = Convert.ToDecimal(imt[2]);
                        litl.TasadeTraslado = Convert.ToDecimal(imt[3]);

                        LITL.Add(litl);
                    }
                    comprobante.Complemento.implocal.TrasladosLocales = LITL.ToArray();

                }
                //-------------------------------------------------------------
                if (impuestosLocalesretenciones.Any())
                {
                    //comprobante.Complemento.imlocal.RetencionesLocales = new ImpuestosLocalesRetencionesLocales();
                    List<ImpuestosLocalesRetencionesLocales> LIL = new List<ImpuestosLocalesRetencionesLocales>();
                    foreach (var im in impuestosLocalesretenciones)
                    {
                        ImpuestosLocalesRetencionesLocales lil = new ImpuestosLocalesRetencionesLocales();
                        lil.ImpLocRetenido = im[1];
                        lil.Importe = Convert.ToDecimal(im[2]);
                        lil.TasadeRetencion = Convert.ToDecimal(im[3]);
                        LIL.Add(lil);
                    }
                    comprobante.Complemento.implocal.RetencionesLocales = LIL.ToArray();
                }
            }
            //---------------fin de impuestos locales (nuevo)
            #endregion

            #region Nomina
            //----------------------esto es nuevo para la nomina
            if (Nomina != null)
            {
                Nomina nomina = new Nomina();

                nomina.Version = "1.2";
                nomina.TipoNomina = Nomina[23];
                nomina.FechaPago = Nomina[4];
                nomina.FechaInicialPago = Nomina[14];
                nomina.FechaFinalPago = Nomina[12];
                nomina.NumDiasPagados = Nomina[13];
                if (!string.IsNullOrEmpty(Nomina[24]))
                {
                    nomina.TotalPercepcionesSpecified = true;
                    nomina.TotalPercepciones = Convert.ToDecimal(Nomina[24]);
                }
                else
                    nomina.TotalPercepcionesSpecified = false;
                if (!string.IsNullOrEmpty(Nomina[25]))
                {
                    nomina.TotalDeduccionesSpecified = true;
                    nomina.TotalDeducciones = Convert.ToDecimal(Nomina[25]);
                }
                else
                    nomina.TotalDeduccionesSpecified = false;

                if (!string.IsNullOrEmpty(Nomina[26]))
                {
                    nomina.TotalOtrosPagosSpecified = true;
                    nomina.TotalOtrosPagos = Convert.ToDecimal(Nomina[26]);
                }
                else
                    nomina.TotalOtrosPagosSpecified = false;
                //------------------------------------EMISOR
                if (emisor != null)
                {
                    if (!string.IsNullOrEmpty(emisor[4]) || !string.IsNullOrEmpty(emisor[5])
                        || !string.IsNullOrEmpty(emisor[8]) || !string.IsNullOrEmpty(emisor[6]))
                    {
                        nomina.Emisor = new NominaEmisor();
                        if (!string.IsNullOrEmpty(emisor[4]))
                            nomina.Emisor.Curp = emisor[4];
                        if (!string.IsNullOrEmpty(emisor[5]))
                            nomina.Emisor.RegistroPatronal = emisor[5];
                        if (!string.IsNullOrEmpty(emisor[8]))
                            nomina.Emisor.RfcPatronOrigen = emisor[8];

                        if (!string.IsNullOrEmpty(emisor[6]))
                        {
                            nomina.Emisor.EntidadSNCF = new NominaEmisorEntidadSNCF();
                            if (!string.IsNullOrEmpty(emisor[7]))
                            {
                                nomina.Emisor.EntidadSNCF.MontoRecursoPropio = Convert.ToDecimal(emisor[7]);
                                nomina.Emisor.EntidadSNCF.MontoRecursoPropioSpecified = true;
                            }
                            else
                                nomina.Emisor.EntidadSNCF.MontoRecursoPropioSpecified = false;
                            if (emisor[6] == "IF")
                                nomina.Emisor.EntidadSNCF.OrigenRecurso = c_OrigenRecurso.IF;
                            if (emisor[6] == "IM")
                                nomina.Emisor.EntidadSNCF.OrigenRecurso = c_OrigenRecurso.IM;
                            if (emisor[6] == "IP")
                                nomina.Emisor.EntidadSNCF.OrigenRecurso = c_OrigenRecurso.IP;

                        }
                    }
                    //-----------------------
                }
                //--------------------------------------EMISOR
                //----------------------RECEPTOR---------------

                nomina.Receptor = new NominaReceptor();

                if (subContratacion.Any())
                {
                    List<NominaReceptorSubContratacion> SUB = new List<NominaReceptorSubContratacion>();
                    foreach (var s in subContratacion)
                    {
                        NominaReceptorSubContratacion sub = new NominaReceptorSubContratacion();
                        sub.PorcentajeTiempo = s[2];
                        sub.RfcLabora = s[1];
                        SUB.Add(sub);

                    }
                    nomina.Receptor.SubContratacion = SUB;
                }

                nomina.Receptor.Curp = Nomina[1];
                if (!string.IsNullOrEmpty(Nomina[16]))
                    nomina.Receptor.NumSeguridadSocial = Nomina[16];

                if (!string.IsNullOrEmpty(Nomina[18]))
                {
                    nomina.Receptor.FechaInicioRelLaboralSpecified = true;
                    nomina.Receptor.FechaInicioRelLaboral = Nomina[18];
                    nomina.Receptor.Antigüedad = Nomina[5];
                }
                else
                    nomina.Receptor.FechaInicioRelLaboralSpecified = false;

                ///..........convert enum-----------------
                nomina.Receptor.TipoContrato = Nomina[11];


                if (!string.IsNullOrEmpty(Nomina[22]))
                {
                    nomina.Receptor.SindicalizadoSpecified = true;
                    if (Nomina[22] == "Si")
                        nomina.Receptor.Sindicalizado = NominaReceptorSindicalizado.Sí;
                    else
                        nomina.Receptor.Sindicalizado = NominaReceptorSindicalizado.No;
                }
                else
                {

                    nomina.Receptor.SindicalizadoSpecified = false;
                }

                if (!string.IsNullOrEmpty(Nomina[7]))
                {
                    nomina.Receptor.TipoJornadaSpecified = true;
                    ///..........convert enum-----------------
                    nomina.Receptor.TipoJornada = Nomina[7];

                }
                else
                    nomina.Receptor.TipoJornadaSpecified = false;

                //--------------------------------------------
                nomina.Receptor.TipoRegimen = Nomina[8];

                nomina.Receptor.NumEmpleado = Nomina[6];
                if (!string.IsNullOrEmpty(Nomina[9]))
                    nomina.Receptor.Departamento = Nomina[9];
                if (!string.IsNullOrEmpty(Nomina[3]))
                    nomina.Receptor.Puesto = Nomina[3];

                if (!string.IsNullOrEmpty(Nomina[10]))
                {
                    nomina.Receptor.RiesgoPuesto = Nomina[10];
                    nomina.Receptor.RiesgoPuestoSpecified = true;
                }
                else
                    nomina.Receptor.RiesgoPuestoSpecified = false;


                nomina.Receptor.PeriodicidadPago = Nomina[15];

                if (!string.IsNullOrEmpty(Nomina[2]))
                {
                    nomina.Receptor.BancoSpecified = true;
                    nomina.Receptor.Banco = Nomina[2];
                }
                else
                    nomina.Receptor.BancoSpecified = false;

                if (!string.IsNullOrEmpty(Nomina[20]))
                    nomina.Receptor.CuentaBancaria = Nomina[20];

                if (!string.IsNullOrEmpty(Nomina[17]))
                {
                    nomina.Receptor.SalarioBaseCotApor = Convert.ToDecimal(Nomina[17]);
                    nomina.Receptor.SalarioBaseCotAporSpecified = true;
                }
                else
                    nomina.Receptor.SalarioBaseCotAporSpecified = false;

                if (!string.IsNullOrEmpty(Nomina[19]))
                {
                    nomina.Receptor.SalarioDiarioIntegrado = Nomina[19];
                    nomina.Receptor.SalarioDiarioIntegradoSpecified = true;
                }
                else
                    nomina.Receptor.SalarioDiarioIntegradoSpecified = false;

                nomina.Receptor.ClaveEntFed = Nomina[21];

                //---------------------FINRECEPTOR-------------
                //---------------------percepciones---------------------
                if (NPercepcion.Any())
                {
                    nomina.Percepciones = new NominaPercepciones();
                    //---
                    if (NPercepcion != null)
                    {
                        nomina.Percepciones.Percepcion = new List<NominaPercepcionesPercepcion>();
                        //int i = 0;
                        foreach (var p in NPercepcion)
                        {
                            NominaPercepcionesPercepcion per = new NominaPercepcionesPercepcion();
                            if (!string.IsNullOrEmpty(p[6]) && !string.IsNullOrEmpty(p[7]))
                            {
                                per.AccionesOTitulos = new NominaPercepcionesPercepcionAccionesOTitulos();
                                per.AccionesOTitulos.ValorMercado = Convert.ToDecimal(p[6]);
                                per.AccionesOTitulos.PrecioAlOtorgarse = Convert.ToDecimal(p[7]);

                            }
                            if (HorasExtra.Any())
                            {
                                bool existe = HorasExtra.Select(x => x[5]).Any(value => value == p[1]);//para unir nodos

                                if (existe)
                                {
                                    per.HorasExtra = new List<NominaPercepcionesPercepcionHorasExtra>();
                                    List<NominaPercepcionesPercepcionHorasExtra> H = new List<NominaPercepcionesPercepcionHorasExtra>();

                                    foreach (var ho in HorasExtra)
                                    {
                                        NominaPercepcionesPercepcionHorasExtra h = new NominaPercepcionesPercepcionHorasExtra();
                                        h.Dias = Convert.ToInt16(ho[1]);
                                        h.HorasExtra = Convert.ToInt16(ho[2]);
                                        h.ImportePagado = Convert.ToDecimal(ho[3]);
                                        h.TipoHoras = ho[4];
                                        H.Add(h);
                                    }
                                    per.HorasExtra = H;
                                }
                            }
                            per.TipoPercepcion = p[5];

                            per.Clave = p[1];
                            per.Concepto = p[2];
                            per.ImporteExento = p[3];
                            per.ImporteGravado = p[4];

                            nomina.Percepciones.Percepcion.Add(per);

                        }
                    }
                    //----

                    if (JubilacionPensionRetiro != null)
                    {
                        nomina.Percepciones.JubilacionPensionRetiro = new NominaPercepcionesJubilacionPensionRetiro();
                        nomina.Percepciones.JubilacionPensionRetiro.IngresoAcumulable = Convert.ToDecimal(JubilacionPensionRetiro[1]);
                        nomina.Percepciones.JubilacionPensionRetiro.IngresoNoAcumulable = Convert.ToDecimal(JubilacionPensionRetiro[2]);

                        if (!string.IsNullOrEmpty(JubilacionPensionRetiro[3]))
                        {
                            nomina.Percepciones.JubilacionPensionRetiro.TotalUnaExhibicion = Convert.ToDecimal(JubilacionPensionRetiro[3]);
                            nomina.Percepciones.JubilacionPensionRetiro.TotalUnaExhibicionSpecified = true;
                        }
                        else
                            nomina.Percepciones.JubilacionPensionRetiro.TotalUnaExhibicionSpecified = false;
                        if (!string.IsNullOrEmpty(JubilacionPensionRetiro[4]))
                        {

                            nomina.Percepciones.JubilacionPensionRetiro.TotalParcialidad = Convert.ToDecimal(JubilacionPensionRetiro[4]);
                            nomina.Percepciones.JubilacionPensionRetiro.TotalParcialidadSpecified = true;
                        }
                        else
                            nomina.Percepciones.JubilacionPensionRetiro.TotalParcialidadSpecified = false;
                        if (!string.IsNullOrEmpty(JubilacionPensionRetiro[5]))
                        {

                            nomina.Percepciones.JubilacionPensionRetiro.MontoDiario = Convert.ToDecimal(JubilacionPensionRetiro[5]);
                            nomina.Percepciones.JubilacionPensionRetiro.MontoDiarioSpecified = true;
                        }
                        else
                            nomina.Percepciones.JubilacionPensionRetiro.MontoDiarioSpecified = false;

                    }
                    if (SeparacionIndemnizacion != null)
                    {
                        nomina.Percepciones.SeparacionIndemnizacion = new NominaPercepcionesSeparacionIndemnizacion();
                        nomina.Percepciones.SeparacionIndemnizacion.IngresoAcumulable = Convert.ToDecimal(SeparacionIndemnizacion[1]);
                        nomina.Percepciones.SeparacionIndemnizacion.IngresoNoAcumulable = Convert.ToDecimal(SeparacionIndemnizacion[2]);
                        nomina.Percepciones.SeparacionIndemnizacion.NumAñosServicio = Convert.ToInt16(SeparacionIndemnizacion[4]);
                        nomina.Percepciones.SeparacionIndemnizacion.TotalPagado = Convert.ToDecimal(SeparacionIndemnizacion[3]);
                        nomina.Percepciones.SeparacionIndemnizacion.UltimoSueldoMensOrd = Convert.ToDecimal(SeparacionIndemnizacion[5]);
                    }



                    if (NPercepcionT != null)
                        if (!string.IsNullOrEmpty(NPercepcionT[3]))
                        {
                            nomina.Percepciones.TotalSueldos = Convert.ToDecimal(NPercepcionT[3]);
                            nomina.Percepciones.TotalSueldosSpecified = true;
                        }
                        else
                            nomina.Percepciones.TotalSueldosSpecified = false;
                    if (NPercepcionT != null)
                        if (!string.IsNullOrEmpty(NPercepcionT[4]))
                        {

                            nomina.Percepciones.TotalSeparacionIndemnizacion = Convert.ToDecimal(NPercepcionT[4]);
                            nomina.Percepciones.TotalSeparacionIndemnizacionSpecified = true;
                        }
                        else
                            nomina.Percepciones.TotalSeparacionIndemnizacionSpecified = false;
                    if (NPercepcionT != null)
                        if (!string.IsNullOrEmpty(NPercepcionT[5]))
                        {
                            nomina.Percepciones.TotalJubilacionPensionRetiro = Convert.ToDecimal(NPercepcionT[5]);
                            nomina.Percepciones.TotalJubilacionPensionRetiroSpecified = true;

                        }
                        else
                            nomina.Percepciones.TotalJubilacionPensionRetiroSpecified = false;

                    nomina.Percepciones.TotalGravado = Convert.ToDecimal(NPercepcionT[2]);
                    nomina.Percepciones.TotalExento = Convert.ToDecimal(NPercepcionT[1]);


                }
                //-----------------------------fin percepciones------------
                //---------------------deducciones
                if (NDeduccion.Any())
                {
                    nomina.Deducciones = new NominaDeducciones();
                    nomina.Deducciones.Deduccion = new List<NominaDeduccionesDeduccion>();
                    List<NominaDeduccionesDeduccion> DED = new List<NominaDeduccionesDeduccion>();
                    foreach (var d in NDeduccion)
                    {
                        NominaDeduccionesDeduccion ded = new NominaDeduccionesDeduccion();
                        ded.TipoDeduccion = d[4];

                        ded.Clave = d[1];
                        ded.Concepto = d[2];
                        ded.Importe = d[3];
                        DED.Add(ded);
                    }
                    nomina.Deducciones.Deduccion = DED;
                    if (NDeduccionT != null)
                    {
                        if (!string.IsNullOrEmpty(NDeduccionT[1]))
                        {
                            nomina.Deducciones.TotalOtrasDeducciones = Convert.ToDecimal(NDeduccionT[1]);
                            nomina.Deducciones.TotalOtrasDeduccionesSpecified = true;
                        }
                        else
                            nomina.Deducciones.TotalOtrasDeduccionesSpecified = false;
                        if (!string.IsNullOrEmpty(NDeduccionT[2]))
                        {

                            nomina.Deducciones.TotalImpuestosRetenidos = Convert.ToDecimal(NDeduccionT[2]);
                            nomina.Deducciones.TotalImpuestosRetenidosSpecified = true;
                        }
                        else
                            nomina.Deducciones.TotalImpuestosRetenidosSpecified = false;
                    }
                    else
                    {
                        nomina.Deducciones.TotalOtrasDeduccionesSpecified = false;
                        nomina.Deducciones.TotalImpuestosRetenidosSpecified = false;

                    }
                }
                //-------------------fin dedducciones
                //--------------------otrosPagos----------------------------------
                if (Otros.Any())
                {
                    nomina.OtrosPagos = new List<NominaOtroPago>();
                    List<NominaOtroPago> OTROS = new List<NominaOtroPago>();
                    foreach (var o in Otros)
                    {
                        NominaOtroPago otros = new NominaOtroPago();
                        if (!string.IsNullOrEmpty(o[5]))
                        {
                            otros.SubsidioAlEmpleo = new NominaOtroPagoSubsidioAlEmpleo();
                            otros.SubsidioAlEmpleo.SubsidioCausado = Convert.ToDecimal(o[5]);
                        }
                        if (!string.IsNullOrEmpty(o[6]))
                        {
                            otros.CompensacionSaldosAFavor = new NominaOtroPagoCompensacionSaldosAFavor();
                            otros.CompensacionSaldosAFavor.SaldoAFavor = Convert.ToDecimal(o[6]);
                            otros.CompensacionSaldosAFavor.Año = Convert.ToInt16(o[7]);
                            otros.CompensacionSaldosAFavor.RemanenteSalFav = Convert.ToDecimal(o[8]);

                        }
                        otros.Clave = o[1];
                        otros.TipoOtroPago = o[4];
                        otros.Concepto = o[2];
                        otros.Importe = o[3];
                        OTROS.Add(otros);
                    }
                    nomina.OtrosPagos = OTROS;

                }
                //--------------------fin otrosPagos--------------------------------------
                //--------------------------incapacidades-----------------------------
                if (Incapacidades.Any())
                {
                    nomina.Incapacidades = new List<NominaIncapacidad>();
                    List<NominaIncapacidad> INC = new List<NominaIncapacidad>();
                    foreach (var i in Incapacidades)
                    {
                        NominaIncapacidad inc = new NominaIncapacidad();
                        inc.DiasIncapacidad = Convert.ToInt16(i[2]);
                        inc.TipoIncapacidad = i[1];

                        if (!string.IsNullOrEmpty(i[3]))
                        {
                            inc.ImporteMonetario = i[3];
                            inc.ImporteMonetarioSpecified = true;
                        }
                        else
                            inc.ImporteMonetarioSpecified = false;
                        INC.Add(inc);
                    }
                    nomina.Incapacidades = INC;

                }

                if (comprobante.Complemento == null)
                    comprobante.Complemento = new GeneradorCfdi.ComprobanteComplemento();
                comprobante.Complemento.Nomina = new Nomina();
                comprobante.Complemento.Nomina = nomina;
                // comprobante.Titulo = "Recibo de Nomina";


            }
            //------------fin de nomina----------------------------
            #endregion
            return comprobante;
        }



        public List<Comprobante> Parse(string fileName)
        {
            var datos = GetFileData(fileName);
            var comp =  ParseData(datos);
            return new List<Comprobante>(){comp};
            
        }

    }
}
