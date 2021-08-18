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
    public class ParserSoriana : IParser
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

                var cabeceraSoriana = datos.FirstOrDefault(j => j[0] == "SOR");
                var cabeceraSorianaPedidos = datos.Where(p => p[0] == "SORPS");
                var cabeceraSorianaPedimento = datos.Where(p => p[0] == "SORPTO");
                var cabeceraSorianaArticulo = datos.Where(p => p[0] == "SORART");
                Comprobante comprobante = new Comprobante();

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
                comprobante.Receptor.Domicilio.pais = ParserNtLink.GetValue(receptorDomicilio[7]);
                comprobante.Receptor.Domicilio.calle = ParserNtLink.GetValue(receptorDomicilio[1]);


                comprobante.Receptor.Domicilio.municipio = ParserNtLink.GetValue(receptorDomicilio[5]);
                comprobante.Receptor.Domicilio.estado = ParserNtLink.GetValue(receptorDomicilio[6]);
                comprobante.Receptor.Domicilio.colonia = ParserNtLink.GetValue(receptorDomicilio[4]);
                comprobante.Receptor.Domicilio.codigoPostal = ParserNtLink.GetValue(receptorDomicilio[8]);
                comprobante.Receptor.Domicilio.localidad = ParserNtLink.GetValue(receptorDomicilio[9]);
                if (emitidoEn != null)
                {

                    comprobante.Emisor.ExpedidoEn = new GeneradorCfdi.t_Ubicacion();
                    comprobante.Emisor.ExpedidoEn.calle = ParserNtLink.GetValue(emitidoEn[1]);
                    comprobante.Emisor.ExpedidoEn.noExterior = ParserNtLink.GetValue(emitidoEn[2]);
                    comprobante.Emisor.ExpedidoEn.noInterior = ParserNtLink.GetValue(emitidoEn[3]);
                    comprobante.Emisor.ExpedidoEn.colonia = ParserNtLink.GetValue(emitidoEn[4]);
                    comprobante.Emisor.ExpedidoEn.codigoPostal = ParserNtLink.GetValue(emitidoEn[8]);
                    comprobante.Emisor.ExpedidoEn.municipio = ParserNtLink.GetValue(emitidoEn[5]);
                    comprobante.Emisor.ExpedidoEn.pais = ParserNtLink.GetValue(emitidoEn[7]);
                    comprobante.Emisor.ExpedidoEn.estado = ParserNtLink.GetValue(emitidoEn[6]);
                    comprobante.Emisor.ExpedidoEn.localidad = ParserNtLink.GetValue(emitidoEn[9]);
                    comprobante.LugarExpedicion = "";
                    if (comprobante.Emisor.ExpedidoEn.localidad != null)
                        comprobante.LugarExpedicion = comprobante.LugarExpedicion +
                                                      comprobante.Emisor.ExpedidoEn.localidad;
                    if (comprobante.Emisor.ExpedidoEn.estado != null)
                        comprobante.LugarExpedicion = comprobante.LugarExpedicion + "," +
                                                      comprobante.Emisor.ExpedidoEn.estado;
                }
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
                comprobante.folio = ParserNtLink.GetValue(comprob[2]);
                comprobante.metodoDePago = ParserNtLink.GetValue(comprob[7]);
                comprobante.NumCtaPago = ParserNtLink.GetValue(comprob[8]);
                comprobante.Moneda = ParserNtLink.GetValue(comprob[12]);
                comprobante.Regimen = ParserNtLink.GetValue(emisor[3]);
                comprobante.subTotal = decimal.Parse(comprob[9]);// factura.Factura.Total.Value - factura.Factura.IVA.Value + factura.Factura.RetenciónIva;
                comprobante.serie = ParserNtLink.GetValue(comprob[1]);
                comprobante.formaDePago = ParserNtLink.GetValue(comprob[6]);
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
                        if (ee.InnerException != null)
                            Logger.Error(ee.InnerException);
                    }


                }
                if (comprob.Length > 20)
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
                            var split = detalle[8].Split(new char[] { '}', '{' });
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
                comprobante.Conceptos = conceptos.ToArray();
                #endregion
                #region Impuestos
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
                #endregion
          
                comprobante.Impuestos = impuestos;
                #endregion
                #region Soriana Addenda
                if (cabeceraSoriana != null)
                {
                    //REMISION ADDENDA SORIANA
                    #region Remision Soriana Addenda
                    DSCargaRemisionProvRemision addendaRemision = new DSCargaRemisionProvRemision
                    {
                        Proveedor = int.Parse(ParserNtLink.GetValue(cabeceraSoriana[1])),
                        Remision = ParserNtLink.GetValue(cabeceraSoriana[2]),
                        Consecutivo = short.Parse(ParserNtLink.GetValue(cabeceraSoriana[3])),
                        FechaRemision = DateTime.ParseExact(cabeceraSoriana[4], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Tienda = short.Parse(cabeceraSoriana[5]),
                        TipoMoneda = short.Parse(cabeceraSoriana[6]),
                        TipoBulto = short.Parse(cabeceraSoriana[7]),
                        EntregaMercancia = short.Parse(cabeceraSoriana[8]),
                        CumpleReqFiscales = Convert.ToBoolean(cabeceraSoriana[9]),
                        CantidadBultos = short.Parse(cabeceraSoriana[10]),
                        Subtotal = decimal.Parse(cabeceraSoriana[11]),
                        IEPS = decimal.Parse(cabeceraSoriana[12]),
                        IVA = decimal.Parse(cabeceraSoriana[13]),
                        OtrosImpuestos = decimal.Parse(cabeceraSoriana[14]),
                        Total = decimal.Parse(cabeceraSoriana[15]),
                        CantidadPedidos = Int32.Parse(cabeceraSoriana[16]),
                        FechaEntregaMercancia = DateTime.ParseExact(cabeceraSoriana[17], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Cita = int.Parse(cabeceraSoriana[18]),
                        FolioNotaEntrada = Int32.Parse(cabeceraSoriana[19])
                    };
                    #endregion
                    #region Remision_Pedidos Sorianna Addenda
                    List<DSCargaRemisionProvPedidos> listaPedidos = new List<DSCargaRemisionProvPedidos>();
                    foreach (var Sorpedidos in cabeceraSorianaPedidos)
                    {
                        DSCargaRemisionProvPedidos addendaPedido = new DSCargaRemisionProvPedidos
                        {
                            Proveedor = Int32.Parse(Sorpedidos[1]),
                            Remision = Sorpedidos[2],
                            FolioPedido = int.Parse(Sorpedidos[3]),
                            Tienda = short.Parse(Sorpedidos[4]),
                            CantidadArticulos = int.Parse(Sorpedidos[5]),
                            PedidoEmitidoProveedorSpecified = true,
                            PedidoEmitidoProveedor = (DSCargaRemisionProvPedidosPedidoEmitidoProveedor)Enum.Parse(typeof(DSCargaRemisionProvPedidosPedidoEmitidoProveedor), Sorpedidos[6])
                        };
                        listaPedidos.Add(addendaPedido);
                    }

                    #endregion
                    #region Pedimento
                    List<DSCargaRemisionProvPedimento> listaPedimento = new List<DSCargaRemisionProvPedimento>();
                    foreach (var SorPedimentos in cabeceraSorianaPedimento)
                    {
                        DSCargaRemisionProvPedimento addendaPedimento = new DSCargaRemisionProvPedimento
                                                                            {
                                                                                Proveedor = int.Parse(SorPedimentos[1]),
                                                                                Remision = SorPedimentos[2],
                                                                                Pedimento = int.Parse(SorPedimentos[3]),
                                                                                Aduana = short.Parse(SorPedimentos[4]),
                                                                                AgenteAduanal = short.Parse(SorPedimentos[5]),
                                                                                TipoPedimento = SorPedimentos[6],
                                                                                FechaPedimento = DateTime.ParseExact(SorPedimentos[7], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                                                                FechaReciboLaredo = DateTime.ParseExact(SorPedimentos[8], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                                                                                FechaBillOfLading = DateTime.ParseExact(SorPedimentos[9], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                                                            };
                        listaPedimento.Add(addendaPedimento);
                    }
                    #endregion
                    #region Articulos

                    List<DSCargaRemisionProvArticulos> listArticulos = new List<DSCargaRemisionProvArticulos>();

                    foreach (var articulo in cabeceraSorianaArticulo)
                    {
                        listArticulos.Add(
                            new DSCargaRemisionProvArticulos
                        {
                            Proveedor = Convert.ToInt32(articulo[1]),
                            Remision = articulo[2],
                            FolioPedido = int.Parse(articulo[3]),
                            Tienda = short.Parse(articulo[4]),
                            Codigo = Decimal.Parse(articulo[5]),
                            CantidadUnidadCompra = decimal.Parse(articulo[6]),
                            CostoNetoUnidadCompra = decimal.Parse(articulo[7]),
                            PorcentajeIEPS = decimal.Parse(articulo[8]),
                            PorcentajeIVA = decimal.Parse(articulo[9])
                        });
                    }

                    #endregion
                    #region Set Addenda Soriana
                    DSCargaRemisionProv addenda = new DSCargaRemisionProv();
                    addenda.Items = new object[listArticulos.Count+listaPedidos.Count+listaPedimento.Count+1];
                    addenda.Items[0] = addendaRemision;
                    int index = 1;
                    foreach (var art in listArticulos)
                    {
                        addenda.Items[index] = art;
                        index++;
                    }
                    
                    foreach (var pedidos in listaPedidos)
                    {
                        addenda.Items[index] = pedidos;
                        index++;
                    }

                    foreach (var pedimento in listaPedimento)
                    {
                        addenda.Items[index] = pedimento;
                        index++;
                    }


                    comprobante.AddendaSoriana = addenda;
                    #endregion
                }
                #endregion
                return new List<Comprobante>() { comprobante };
            }
            else
            {
                throw new Exception("El archivo " + fileName + " está vacío");
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
                var adenda = this.GetSorianaAdenda(data);
                if (adenda != null)
                {
                    comp.AddendaSoriana = adenda;
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(DSCargaRemisionProv), null, null);
                }
                res.Add(comp);
            }
            return res;
        }

        private DSCargaRemisionProv GetSorianaAdenda(string[][] datosArchivo)
        {
            #region GetData
            var cabeceraSoriana = datosArchivo.FirstOrDefault(j => j[0] == "SOR");
            var cabeceraSorianaPedidos = datosArchivo.Where(p => p[0] == "SORPS");
            var cabeceraSorianaPedimento = datosArchivo.Where(p => p[0] == "SORPTO");
            var cabeceraSorianaArticulo = datosArchivo.Where(p => p[0] == "SORART");
            #endregion
            if (cabeceraSoriana != null)
            {
                #region Parse adenda
                //REMISION ADDENDA SORIANA
                #region Remision Soriana Addenda
                DSCargaRemisionProvRemision addendaRemision = new DSCargaRemisionProvRemision
                {
                    Proveedor = int.Parse(ParserNtLink.GetValue(cabeceraSoriana[1])),
                    Remision = ParserNtLink.GetValue(cabeceraSoriana[2]),
                    Consecutivo = short.Parse(ParserNtLink.GetValue(cabeceraSoriana[3])),
                    FechaRemision = DateTime.ParseExact(cabeceraSoriana[4], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Tienda = short.Parse(cabeceraSoriana[5]),
                    TipoMoneda = short.Parse(cabeceraSoriana[6]),
                    TipoBulto = short.Parse(cabeceraSoriana[7]),
                    EntregaMercancia = short.Parse(cabeceraSoriana[8]),
                    CumpleReqFiscales = Convert.ToBoolean(cabeceraSoriana[9]),
                    CantidadBultos = short.Parse(cabeceraSoriana[10]),
                    Subtotal = decimal.Parse(cabeceraSoriana[11]),
                    IEPS = decimal.Parse(cabeceraSoriana[12]),
                    IVA = decimal.Parse(cabeceraSoriana[13]),
                    OtrosImpuestos = decimal.Parse(cabeceraSoriana[14]),
                    Total = decimal.Parse(cabeceraSoriana[15]),
                    CantidadPedidos = Int32.Parse(cabeceraSoriana[16]),
                    FechaEntregaMercancia = DateTime.ParseExact(cabeceraSoriana[17], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Cita = int.Parse(cabeceraSoriana[18]),
                    FolioNotaEntrada = Int32.Parse(cabeceraSoriana[19])
                };
                #endregion
                #region Remision_Pedidos Sorianna Addenda
                List<DSCargaRemisionProvPedidos> listaPedidos = new List<DSCargaRemisionProvPedidos>();
                foreach (var Sorpedidos in cabeceraSorianaPedidos)
                {
                    DSCargaRemisionProvPedidos addendaPedido = new DSCargaRemisionProvPedidos
                    {
                        Proveedor = Int32.Parse(Sorpedidos[1]),
                        Remision = Sorpedidos[2],
                        FolioPedido = int.Parse(Sorpedidos[3]),
                        Tienda = short.Parse(Sorpedidos[4]),
                        CantidadArticulos = int.Parse(Sorpedidos[5]),
                        PedidoEmitidoProveedorSpecified = true,
                        PedidoEmitidoProveedor = (DSCargaRemisionProvPedidosPedidoEmitidoProveedor)Enum.Parse(typeof(DSCargaRemisionProvPedidosPedidoEmitidoProveedor), Sorpedidos[6])
                    };
                    listaPedidos.Add(addendaPedido);
                }

                #endregion
                #region Pedimento
                List<DSCargaRemisionProvPedimento> listaPedimento = new List<DSCargaRemisionProvPedimento>();
                foreach (var SorPedimentos in cabeceraSorianaPedimento)
                {
                    DSCargaRemisionProvPedimento addendaPedimento = new DSCargaRemisionProvPedimento
                    {
                        Proveedor = int.Parse(SorPedimentos[1]),
                        Remision = SorPedimentos[2],
                        Pedimento = int.Parse(SorPedimentos[3]),
                        Aduana = short.Parse(SorPedimentos[4]),
                        AgenteAduanal = short.Parse(SorPedimentos[5]),
                        TipoPedimento = SorPedimentos[6],
                        FechaPedimento = DateTime.ParseExact(SorPedimentos[7], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        FechaReciboLaredo = DateTime.ParseExact(SorPedimentos[8], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        FechaBillOfLading = DateTime.ParseExact(SorPedimentos[9], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                    };
                    listaPedimento.Add(addendaPedimento);
                }
                #endregion
                #region Articulos

                List<DSCargaRemisionProvArticulos> listArticulos = new List<DSCargaRemisionProvArticulos>();

                foreach (var articulo in cabeceraSorianaArticulo)
                {
                    listArticulos.Add(
                        new DSCargaRemisionProvArticulos
                        {
                            Proveedor = Convert.ToInt32(articulo[1]),
                            Remision = articulo[2],
                            FolioPedido = int.Parse(articulo[3]),
                            Tienda = short.Parse(articulo[4]),
                            Codigo = Decimal.Parse(articulo[5]),
                            CantidadUnidadCompra = decimal.Parse(articulo[6]),
                            CostoNetoUnidadCompra = decimal.Parse(articulo[7]),
                            PorcentajeIEPS = decimal.Parse(articulo[8]),
                            PorcentajeIVA = decimal.Parse(articulo[9])
                        });
                }

                #endregion
                #region Set Addenda Soriana
                DSCargaRemisionProv addenda = new DSCargaRemisionProv();
                addenda.Items = new object[listArticulos.Count + listaPedidos.Count + listaPedimento.Count + 1];
                addenda.Items[0] = addendaRemision;
                int index = 1;
                foreach (var art in listArticulos)
                {
                    addenda.Items[index] = art;
                    index++;
                }

                foreach (var pedidos in listaPedidos)
                {
                    addenda.Items[index] = pedidos;
                    index++;
                }

                foreach (var pedimento in listaPedimento)
                {
                    addenda.Items[index] = pedimento;
                    index++;
                }
                #endregion
                #endregion
                return addenda;
            }
            else
            {
                return null;
            }
        }
    }
}
