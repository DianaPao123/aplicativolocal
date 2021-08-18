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
    public class ParserAHM : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserNtLink));

        private List<Comprobante> ParseOriginal(string fileName)
        {
            return null;
            /*
            if (!File.Exists(fileName))
                throw new ApplicationException("El archivo no existe");
            var datos = File.ReadAllLines(fileName, Encoding.Default).Select(p => p.Split('|')).ToArray();
            if (datos.Length > 0)
            {
                #region Get data
                var concepts = datos.Where(p => p[0] == "C");
                
                var comprob = datos.FirstOrDefault(p => p[0] == "COMP");
                var emisor = datos.FirstOrDefault(p => p[0] == "E");
                var emisorDomicilio = datos.FirstOrDefault(p => p[0] == "DE");
                var emitidoEn = datos.FirstOrDefault(p => p[0] == "EE");
                var receptor = datos.FirstOrDefault(p => p[0] == "R");
                var receptorDomicilio = datos.FirstOrDefault(p => p[0] == "DR");
                var impuestosTraslado = datos.Where(p => p[0] == "IT");
                var impuestosRetencion = datos.Where(p => p[0] == "IR");
                var addendaPemex = datos.FirstOrDefault(p => p[0] == "AP");
                var datosAdicionales = datos.FirstOrDefault(j => j[0] == "AD");
                var cabeceraAhm = datos.FirstOrDefault(j => j[0] == "AHM");
                var cabeceraAhmAnexos = datos.Where(j => j[0] == "AHMA");
                var cabeceraAhmDetalle = datos.FirstOrDefault(j => j[0] == "AHMD");
                var cabeceraAhmPedido = datos.Where(j => j[0] == "AHMP");
                Comprobante comprobante = new Comprobante();
                #endregion
                #region Documento
                comprobante.Emisor = new GeneradorCfdi.ComprobanteEmisor();
                comprobante.Emisor.nombre = emisor[2];
                comprobante.Emisor.RegimenFiscal = new[]
                                                       {
                                                           new GeneradorCfdi.ComprobanteEmisorRegimenFiscal
                                                               {Regimen = emisor[3]}
                                                       };
                comprobante.Emisor.rfc = emisor[1];
                
                comprobante.Emisor.DomicilioFiscal = new GeneradorCfdi.t_UbicacionFiscal();
                comprobante.Emisor.DomicilioFiscal.calle = ParserNtLink.GetValue(emisorDomicilio[1]);
                comprobante.Emisor.DomicilioFiscal.noExterior = ParserNtLink.GetValue(emisorDomicilio[2]);
                comprobante.Emisor.DomicilioFiscal.noInterior = ParserNtLink.GetValue(emisorDomicilio[3]);
                comprobante.Emisor.DomicilioFiscal.colonia = ParserNtLink.GetValue(emisorDomicilio[4]);
                comprobante.Emisor.DomicilioFiscal.codigoPostal = ParserNtLink.GetValue(emisorDomicilio[8]);
                comprobante.Emisor.DomicilioFiscal.municipio = ParserNtLink.GetValue(emisorDomicilio[5]);
                comprobante.Emisor.DomicilioFiscal.pais = ParserNtLink.GetValue(emisorDomicilio[7]);
                comprobante.Emisor.DomicilioFiscal.estado = ParserNtLink.GetValue(emisorDomicilio[6]);
                comprobante.Emisor.DomicilioFiscal.localidad = ParserNtLink.GetValue(emisorDomicilio[9]);

                comprobante.Titulo = "Factura";
                if (comprob[5].Equals("ingreso", StringComparison.InvariantCultureIgnoreCase))
                    comprobante.tipoDeComprobante = GeneradorCfdi.ComprobanteTipoDeComprobante.ingreso;
                else
                {
                    comprobante.tipoDeComprobante = GeneradorCfdi.ComprobanteTipoDeComprobante.egreso;
                    comprobante.Titulo = "Nota de Crédito";
                }
                    
                comprobante.Receptor = new GeneradorCfdi.ComprobanteReceptor();
                comprobante.Receptor.nombre = receptor[2];
                if (!String.IsNullOrEmpty(comprobante.Receptor.nombre))
                {
                    comprobante.Receptor.nombre = receptor[2];
                }
                else
                {
                    comprobante.Receptor.nombre = " ";
                }
                //-
                comprobante.Receptor.Bcc = receptor[4];
                comprobante.Receptor.rfc = receptor[1];
                comprobante.Receptor.Emails = receptor[3];
                comprobante.Receptor.Domicilio = new GeneradorCfdi.t_Ubicacion();
                comprobante.Receptor.Domicilio.noExterior = ParserNtLink.GetValue(receptorDomicilio[2]);
                comprobante.Receptor.Domicilio.noInterior = ParserNtLink.GetValue(receptorDomicilio[3]);
                comprobante.Receptor.Domicilio.pais =  ParserNtLink.GetValue(receptorDomicilio[7]);
                comprobante.Receptor.Domicilio.calle = ParserNtLink.GetValue(receptorDomicilio[1]);
                
                
                comprobante.Receptor.Domicilio.municipio =  ParserNtLink.GetValue(receptorDomicilio[5]);
                comprobante.Receptor.Domicilio.estado =  ParserNtLink.GetValue(receptorDomicilio[6]);
                comprobante.Receptor.Domicilio.colonia = ParserNtLink.GetValue(receptorDomicilio[4]);
                comprobante.Receptor.Domicilio.codigoPostal = ParserNtLink.GetValue(receptorDomicilio[8]);
                comprobante.Receptor.Domicilio.localidad = ParserNtLink.GetValue(receptorDomicilio[9]);
                if (emitidoEn != null)
                {
                    
                    comprobante.Emisor.ExpedidoEn = new GeneradorCfdi.t_Ubicacion();
                    comprobante.Emisor.ExpedidoEn.calle =  ParserNtLink.GetValue(emitidoEn[1]);
                    comprobante.Emisor.ExpedidoEn.noExterior = ParserNtLink.GetValue(emitidoEn[2]);
                    comprobante.Emisor.ExpedidoEn.noInterior = ParserNtLink.GetValue(emitidoEn[3]);
                    comprobante.Emisor.ExpedidoEn.colonia =  ParserNtLink.GetValue(emitidoEn[4]);
                    comprobante.Emisor.ExpedidoEn.codigoPostal = ParserNtLink.GetValue( emitidoEn[8]);
                    comprobante.Emisor.ExpedidoEn.municipio = ParserNtLink.GetValue( emitidoEn[5]);
                    comprobante.Emisor.ExpedidoEn.pais =  ParserNtLink.GetValue(emitidoEn[7]);
                    comprobante.Emisor.ExpedidoEn.estado =  ParserNtLink.GetValue(emitidoEn[6]);
                    comprobante.Emisor.ExpedidoEn.localidad = ParserNtLink.GetValue(emitidoEn[9]);
                    comprobante.LugarExpedicion = "";
                    if (comprobante.Emisor.ExpedidoEn.localidad != null)
                        comprobante.LugarExpedicion = comprobante.LugarExpedicion +
                                                      comprobante.Emisor.ExpedidoEn.localidad;
                    if (comprobante.Emisor.ExpedidoEn.estado != null)
                        comprobante.LugarExpedicion = comprobante.LugarExpedicion + "," +
                                                      comprobante.Emisor.ExpedidoEn.estado;
                }
                //comprobante.fecha = Convert.ToDateTime(DateTime.Now.ToString("s"));//entrada.fecha;
                if (ConfigurationManager.AppSettings["ReemplazarFecha"] == "1")
                    comprobante.fecha = Convert.ToDateTime(DateTime.Now.ToString("s"));
                else
                {
                    try
                    {
                        comprobante.fecha = DateTime.ParseExact(comprob[3], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    catch (Exception ee)
                    {
                        Logger.Error("Fecha inválida: " + comprob[3]);
                        Logger.Error(ee);
                        throw;
                    }


                }
                comprobante.total = decimal.Parse(comprob[11]);

                //comprobante.Leyenda = factura.Factura.Leyenda;
                //comprobante.LeyendaInferior = emp.LeyendaInferior;
                //comprobante.LeyendaSuperior = emp.LeyendaSuperior;
                comprobante.folio =  ParserNtLink.GetValue(comprob[2]);
                comprobante.metodoDePago =  ParserNtLink.GetValue(comprob[7]);
                comprobante.NumCtaPago =  ParserNtLink.GetValue(comprob[8]);
                comprobante.Moneda =  ParserNtLink.GetValue(comprob[12]);
                comprobante.Regimen =  ParserNtLink.GetValue(emisor[3]);
                comprobante.subTotal = decimal.Parse(comprob[9]);// factura.Factura.Total.Value - factura.Factura.IVA.Value + factura.Factura.RetenciónIva;
                comprobante.serie =  ParserNtLink.GetValue(comprob[1]);
                comprobante.formaDePago =  ParserNtLink.GetValue(comprob[6]);
                comprobante.LeyendaSuperior = ParserNtLink.GetValue2(comprob[15]);
                comprobante.Proyecto = ParserNtLink.GetValue2(comprob[16]);
                //Se agregan tres campos 
                comprobante.Leyenda = ParserNtLink.GetValue2(comprob[17]);
                comprobante.nota1 = ParserNtLink.GetValue2(comprob[18]);
                comprobante.nota2 = ParserNtLink.GetValue2(comprob[19]);
                
                if (!string.IsNullOrEmpty(comprob[10]))
                {
                    try
                    {
                        comprobante.descuento = decimal.Parse(comprob[10]);
                        comprobante.descuentoSpecified = true;
                    }
                    catch (Exception ee)
                    {
                        Logger.Error(ee);
                        if(ee.InnerException != null)
                            Logger.Error(ee.InnerException);
                    }


                }
                if (comprob.Length > 20 )
                {
                    comprobante.LeyendaAnticipo = ParserNtLink.GetValue(comprob[20]);
                    comprobante.PorcentajeAnticipo = ParserNtLink.GetValue(comprob[21]);
                    string anticipo = ParserNtLink.GetValue(comprob[22]);
                    //string porcentaje = ParserNtLink.GetValue(comprob[21]);
                    decimal dAnticipo = 0;
                    if (decimal.TryParse(anticipo, out dAnticipo))
                        comprobante.Anticipo = dAnticipo;


                }
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
                #region Conceptos
                List<GeneradorCfdi.ComprobanteConcepto> conceptos = new List<GeneradorCfdi.ComprobanteConcepto>();
               // List<GeneradorCfdi.t_InformacionAduanera> informacionAduanera = new List<GeneradorCfdi.t_InformacionAduanera>();
                foreach (var detalle in concepts)
                {
                    GeneradorCfdi.ComprobanteConcepto con = new GeneradorCfdi.ComprobanteConcepto();
                    con.descripcion = ParserNtLink.GetValue(detalle[3]);
                    if (!string.IsNullOrEmpty(detalle[1]))
                        con.noIdentificacion = detalle[1];
                    con.cantidad = decimal.Parse(detalle[4]);
                    con.valorUnitario = decimal.Parse(detalle[5]);
                    con.importe = decimal.Parse(detalle[6]);
                    con.unidad = detalle[2];
                    con.Detalles = detalle[7];
                    con.FechaPedido = detalle[10];
                    con.CajasPiezas = detalle[11];
                    if (!string.IsNullOrEmpty(con.Detalles))
                        con.Detalles = con.Detalles.Replace("\\", "\n");
                    string informacionAduanera = "";
                    List<GeneradorCfdi.t_InformacionAduanera> pedimentos =
                        new List<GeneradorCfdi.t_InformacionAduanera>();
                    if (detalle.Length > 9 && !string.IsNullOrEmpty(detalle[8]))
                    {
                        try
                        {
                            var split = detalle[8].Split(new char[] {'}', '{'});
                            foreach (string pedimento in split)
                            {
                                if (!string.IsNullOrEmpty(pedimento))
                                {
                                    var datosPedimento = pedimento.Split(';');
                                    GeneradorCfdi.t_InformacionAduanera info = new GeneradorCfdi.t_InformacionAduanera();
                                    var numero =
                                        datosPedimento.FirstOrDefault(
                                            p => p.StartsWith("numero", StringComparison.InvariantCultureIgnoreCase));
                                    info.numero = numero.Substring(numero.IndexOf("=") + 1);
                                    var dfecha =
                                        datosPedimento.FirstOrDefault(
                                            p => p.StartsWith("fecha", StringComparison.InvariantCultureIgnoreCase));
                                    var fecha = dfecha.Substring(dfecha.IndexOf("=") + 1);
                                    info.fecha = DateTime.ParseExact(fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                    var aduana =
                                        datosPedimento.FirstOrDefault(
                                            p => p.StartsWith("aduana", StringComparison.InvariantCultureIgnoreCase));
                                    info.aduana = aduana.Substring(aduana.IndexOf("=") + 1);
                                    pedimentos.Add(info);
                                    informacionAduanera = informacionAduanera + "Pedimento: " + info.numero +
                                                          " Aduana: " + info.aduana + " Fecha: " +
                                                          info.fecha.ToString("yyyy-MM-dd") + "\r\n";
                                }

                            }
                            con.InformacionAduanera = informacionAduanera;
                        }
                        catch (Exception ee)
                        {
                            Logger.Error(ee);
                            if (ee.InnerException != null)
                                Logger.Error(ee.InnerException);
                            throw new ApplicationException("Error al intentar leer la información aduanera: " +
                                                           conceptos[8]);
                        }


                    }

                       if (pedimentos.Count > 0)
                        con.Items = pedimentos.ToArray();


                    conceptos.Add(con);

                }
                #endregion
                comprobante.Conceptos = conceptos.ToArray();
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

                comprobante.CantidadLetra = CantidadLetra.Enletras(comprobante.total.ToString(), comprobante.Moneda, comprobante.Emisor.rfc);

                //if (factura.Factura.TipoDocumento == TipoDocumento.Donativo)
                //{
                //    Donatarias donat = new Donatarias
                //    {
                //        fechaAutorizacion = factura.Factura.DonativoFechaAutorizacion,
                //        leyenda =
                //            "Este comprobante ampara un donativo, el cual será destinado por la donataria a los fines propios de su objeto social. En el caso de que los bienes donados hayan sido deducidos previamente para los efectos del impuesto sobre la renta, este donativo no es deducible. La reproducción no autorizada de este comprobante constituye un delito en los términos de las disposiciones fiscales.",
                //        noAutorizacion = factura.Factura.DonativoAutorizacion,
                //        version = "1.1"
                //    };
                //    if (comprobante.Complemento == null)
                //        comprobante.Complemento = new GeneradorCfdi.ComprobanteComplemento();
                //    comprobante.Complemento.Donat = donat;
                //}

                comprobante.Impuestos = impuestos;
                #endregion
                #region AHM Addenda
                if (cabeceraAhm != null)
                {
                    //PArse .. 
                    AddendaAHM addenda = new AddendaAHM();
                    ///VERSION Y DOCUMENTO
                    addenda.Documento = new AddendaAHMDocumento();
                    addenda.Version = ParserNtLink.GetValue(cabeceraAhm[1]);//

                    addenda.Documento.Tipo = ParserNtLink.GetValue(cabeceraAhm[2]);//
                    addenda.Documento.Clase = ParserNtLink.GetValue(cabeceraAhm[3]);//
                    
                    ///ENCABEZADO ADDENDA AHM
                    addenda.Documento.Encabezado = new AddendaAHMDocumentoEncabezado();
                    addenda.Documento.Encabezado.NumSociedad = ParserNtLink.GetValue(cabeceraAhm[4]);//
                    addenda.Documento.Encabezado.NumDivision = ParserNtLink.GetValue(cabeceraAhm[5]);//
                    addenda.Documento.Encabezado.NumProveedor = ParserNtLink.GetValue(cabeceraAhm[6]);//
                    addenda.Documento.Encabezado.Correo = ParserNtLink.GetValue(cabeceraAhm[7]);//
                    addenda.Documento.Encabezado.Moneda = ParserNtLink.GetValue(cabeceraAhm[8]);//
                    
                    ///ADDENDA DOCUMENTO DETALLE 
                    
                    addenda.Documento.Detalle = new AddendaAHMDocumentoDetalle();
                    
                    addenda.Documento.Detalle.HojaServicio = new AddendaAHMDocumentoDetalleHojaServicio();
                    addenda.Documento.Detalle.HojaServicio.Num = ParserNtLink.GetValue(cabeceraAhmDetalle[1]);

                    addenda.Documento.Detalle.Transporte = new AddendaAHMDocumentoDetalleTransporte();
                    addenda.Documento.Detalle.Transporte.Num = ParserNtLink.GetValue(cabeceraAhmDetalle[2]);

                    addenda.Documento.Detalle.CtaxPag = new AddendaAHMDocumentoDetalleCtaxPag();
                    addenda.Documento.Detalle.CtaxPag.Num = ParserNtLink.GetValue(cabeceraAhmDetalle[3]);
                    addenda.Documento.Detalle.CtaxPag.Ejercicio = ParserNtLink.GetValue(cabeceraAhmDetalle[4]);

                    addenda.Documento.Detalle.Liquidacion = new AddendaAHMDocumentoDetalleLiquidacion();
                    addenda.Documento.Detalle.Liquidacion.FechaInicio = ParserNtLink.GetValue(cabeceraAhmDetalle[5]);
                    addenda.Documento.Detalle.Liquidacion.FechaFin = ParserNtLink.GetValue(cabeceraAhmDetalle[6]);
                    

                    ///ADDENDA DOCUMENTO PEDIDO Multiples Dividido por ','
                    List<AddendaAHMDocumentoDetallePedido> pedidoslist = new List<AddendaAHMDocumentoDetallePedido>();

                    foreach (var pedido in cabeceraAhmPedido)
                    {
                        AddendaAHMDocumentoDetallePedido tempPedido = new AddendaAHMDocumentoDetallePedido();
                        tempPedido.Num = pedido[1];
                        tempPedido.Recepcion = ParserNtLink.GetValue(pedido[2]).Split(',');
                        pedidoslist.Add(tempPedido);
                    }
                    addenda.Documento.Detalle.Pedido = pedidoslist.ToArray();
                    pedidoslist.Clear();
                    

                    ///ANEXOS
                    List<AddendaAHMDocumentoAnexos> anexosListAhm = new List<AddendaAHMDocumentoAnexos>();

                    foreach (var anexos in cabeceraAhmAnexos)
                    {
                        AddendaAHMDocumentoAnexos tempAnexo= new AddendaAHMDocumentoAnexos();
                        tempAnexo.Anexo = ParserNtLink.GetValue(anexos[1]);
                        anexosListAhm.Add(tempAnexo);
                    }
                    addenda.Documento.Anexos = anexosListAhm.ToArray();
                    anexosListAhm.Clear();
                    ///


                    comprobante.AddendaAHM = addenda;
                }
                #endregion
                return new List<Comprobante>() { comprobante };
            }
            else
            {
                throw new Exception("El archivo "+ fileName + " está vacío");
            }
             */ 
             
        }

        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetAHMAdenda(data);
                if (adenda != null)
                {
                    comp.AddendaAHM = adenda;
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(AddendaAHM), "ahmsa", "http://www.ahmsa.com/xsd/AddendaAHM1");
                }
                res.Add(comp);
            }
            return res;
        }

        private AddendaAHM GetAHMAdenda(string[][] datosArchivo)
        {
            #region GetData
            var cabeceraAhm = datosArchivo.FirstOrDefault(j => j[0] == "AHM");
            var cabeceraAhmAnexos = datosArchivo.Where(j => j[0] == "AHMA");
            var cabeceraAhmDetalle = datosArchivo.FirstOrDefault(j => j[0] == "AHMD");
            var cabeceraAhmPedido = datosArchivo.Where(j => j[0] == "AHMP");
            #endregion
            if (cabeceraAhm != null)
            {
                #region Parse addenda
                #region Versión y documento
                AddendaAHM addenda = new AddendaAHM();
                addenda.Documento = new AddendaAHMDocumento();
                addenda.Version = ParserNtLink.GetValue(cabeceraAhm[1]);//

                addenda.Documento.Tipo = ParserNtLink.GetValue(cabeceraAhm[2]);//
                addenda.Documento.Clase = ParserNtLink.GetValue(cabeceraAhm[3]);//
                #endregion
                #region ENCABEZADO ADDENDA AHM
                addenda.Documento.Encabezado = new AddendaAHMDocumentoEncabezado();
                addenda.Documento.Encabezado.NumSociedad = ParserNtLink.GetValue(cabeceraAhm[4]);//
                addenda.Documento.Encabezado.NumDivision = cabeceraAhm[5];//
                addenda.Documento.Encabezado.NumProveedor = ParserNtLink.GetValue(cabeceraAhm[6]);//
                addenda.Documento.Encabezado.Correo = ParserNtLink.GetValue(cabeceraAhm[7]);//
                addenda.Documento.Encabezado.Moneda = ParserNtLink.GetValue(cabeceraAhm[8]);//
                #endregion
                #region ADDENDA DOCUMENTO DETALLE 
                addenda.Documento.Detalle = new AddendaAHMDocumentoDetalle();

                addenda.Documento.Detalle.HojaServicio = new AddendaAHMDocumentoDetalleHojaServicio();
                addenda.Documento.Detalle.HojaServicio.Num = ParserNtLink.GetValue(cabeceraAhmDetalle[1]);

                addenda.Documento.Detalle.Transporte = new AddendaAHMDocumentoDetalleTransporte();
                addenda.Documento.Detalle.Transporte.Num = ParserNtLink.GetValue(cabeceraAhmDetalle[2]);

                addenda.Documento.Detalle.CtaxPag = new AddendaAHMDocumentoDetalleCtaxPag();
                addenda.Documento.Detalle.CtaxPag.Num = ParserNtLink.GetValue(cabeceraAhmDetalle[3]);

                decimal valor = 0; decimal.TryParse(cabeceraAhmDetalle[4], out valor);
                addenda.Documento.Detalle.CtaxPag.Ejercicio = (valor != 0)
                    ? ParserNtLink.GetValue(cabeceraAhmDetalle[4])
                    : ParserNtLink.GetValue(cabeceraAhmDetalle[4]).Length > 0 
                        ? ParserNtLink.GetValue(cabeceraAhmDetalle[4]) 
                        : "";

                addenda.Documento.Detalle.Liquidacion = new AddendaAHMDocumentoDetalleLiquidacion();
                addenda.Documento.Detalle.Liquidacion.FechaInicio = ParserNtLink.GetValue(cabeceraAhmDetalle[5]);
                addenda.Documento.Detalle.Liquidacion.FechaFin = ParserNtLink.GetValue(cabeceraAhmDetalle[6]);
                #endregion
                #region ADDENDA DOCUMENTO PEDIDO Multiples Dividido por ','
                List<AddendaAHMDocumentoDetallePedido> pedidoslist = new List<AddendaAHMDocumentoDetallePedido>();

                foreach (var pedido in cabeceraAhmPedido)
                {
                    AddendaAHMDocumentoDetallePedido tempPedido = new AddendaAHMDocumentoDetallePedido();
                    tempPedido.Num = pedido[1];
                    if (!string.IsNullOrEmpty(pedido[2]))
                    {
                        tempPedido.Recepcion = ParserNtLink.GetValue(pedido[2]).Split(',');
                    }
                    else tempPedido.Recepcion = new string[]{""};
                    pedidoslist.Add(tempPedido);
                }
                addenda.Documento.Detalle.Pedido = pedidoslist.ToArray();
                pedidoslist.Clear();
                #endregion
                #region ANEXOS
                List<AddendaAHMDocumentoAnexos> anexosListAhm = new List<AddendaAHMDocumentoAnexos>();

                foreach (var anexos in cabeceraAhmAnexos)
                {
                    AddendaAHMDocumentoAnexos tempAnexo = new AddendaAHMDocumentoAnexos();
                    tempAnexo.Anexo = ParserNtLink.GetValue(anexos[1]);
                    anexosListAhm.Add(tempAnexo);
                }
                addenda.Documento.Anexos = anexosListAhm.ToArray();
                anexosListAhm.Clear();
                #endregion
                #endregion
                return addenda;
            }
            return null;
        }

    }
}
