using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using AddendaAmece;
using GeneradorCfdi;
using log4net;

namespace TxtToCfdi
{
    public class ParserAmece71 : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserNtLink));

        private List<Comprobante> ParseOriginal(string fileName)
        {

            if (!File.Exists(fileName))
                throw new ApplicationException("El archivo no existe");
            var datos = File.ReadAllLines(fileName, Encoding.Default).Select(p => p.Split('|')).ToArray();
            if (datos.Length > 0)
            {
                return null;
                /*
                var concepts = datos.Where(p => p[0] == "C");

                var comprob = datos.FirstOrDefault(p => p[0] == "COMP");
                var emisor = datos.FirstOrDefault(p => p[0] == "E");
                var emisorDomicilio = datos.FirstOrDefault(p => p[0] == "DE");
                var emitidoEn = datos.FirstOrDefault(p => p[0] == "EE");
                var receptor = datos.FirstOrDefault(p => p[0] == "R");
                var receptorDomicilio = datos.FirstOrDefault(p => p[0] == "DR");
                var impuestosTraslado = datos.Where(p => p[0] == "IT");
                var impuestosRetencion = datos.Where(p => p[0] == "IR");
                var datosAdicionales = datos.FirstOrDefault(j => j[0] == "AD");
                var rfp = datos.FirstOrDefault(j => j[0] == "RFP");
                var rfpId = datos.FirstOrDefault(j => j[0] == "RFPID");
                var oid = datos.FirstOrDefault(j => j[0] == "OID");
                var adin = datos.FirstOrDefault(j => j[0] == "ADIN");
                var buy = datos.FirstOrDefault(j => j[0] == "BUY");
                var sel = datos.FirstOrDefault(j => j[0] == "SEL");
                var shipto = datos.FirstOrDefault(j => j[0] == "SHIPTO");
                var pt = datos.FirstOrDefault(j => j[0] == "PT");
                Comprobante comprobante = new Comprobante();

                requestForPayment request = new requestForPayment();
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


                comprobante.Receptor.rfc = receptor[1];
                comprobante.Receptor.nombre = ParserNtLink.GetValue(receptor[2]);
                comprobante.Receptor.Emails = receptor[3];
                comprobante.Receptor.Bcc = ParserNtLink.GetValue(receptor[4]);
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
                comprobante.TipoCambio = ParserNtLink.GetValue(comprob[13]);
                comprobante.Regimen = ParserNtLink.GetValue(emisor[3]);
                comprobante.subTotal = decimal.Parse(comprob[9]);
                    // factura.Factura.Total.Value - factura.Factura.IVA.Value + factura.Factura.RetenciónIva;
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

                        if (detalle.Length > 11)
                        {
                            con.FechaPedido = ParserNtLink.GetValue(detalle[9]);
                            con.CajasPiezas = ParserNtLink.GetValue(detalle[10]);
                        }
                    }

                    //detalle[8].Split(Convert.ToChar("}{"));

                    //split("%");

                    //if (!string.IsNullOrEmpty(detalle[9]))
                    //{ }
                    //detalle[9].Split(Convert.ToChar("}{"));

                    //split("%");
                    //if (!string.IsNullOrEmpty(detalle[10]))
                    //{ }
                    //detalle[10].Split(Convert.ToChar("}{"));

                    //split("%");
                    //if (!string.IsNullOrEmpty(detalle[8]))
                    //    informacionAduanera.numero = detalle[8];
                    //if (!string.IsNullOrEmpty(detalle[9]))
                    //    informacionAduanera.fecha = DateTime.Parse(detalle[9]);
                    //if (!string.IsNullOrEmpty(detalle[10]))
                    //    informacionAduanera.aduana = detalle[10];
                    if (pedimentos.Count > 0)
                        con.Items = pedimentos.ToArray();


                    conceptos.Add(con);

                }
                comprobante.Conceptos = conceptos.ToArray();
                GeneradorCfdi.ComprobanteImpuestos impuestos = new GeneradorCfdi.ComprobanteImpuestos();
                if (impuestosTraslado.Any())
                {

                    List<GeneradorCfdi.ComprobanteImpuestosTraslado> listaTraslados =
                        new List<GeneradorCfdi.ComprobanteImpuestosTraslado>();
                    foreach (var tr in impuestosTraslado)
                    {
                        GeneradorCfdi.ComprobanteImpuestosTraslado traslado =
                            new GeneradorCfdi.ComprobanteImpuestosTraslado();
                        traslado.importe = decimal.Parse(tr[3]);
                        if (tr[1].Equals("IVA", StringComparison.InvariantCultureIgnoreCase))
                            traslado.impuesto = GeneradorCfdi.ComprobanteImpuestosTrasladoImpuesto.IVA;
                        else traslado.impuesto = GeneradorCfdi.ComprobanteImpuestosTrasladoImpuesto.IEPS;
                        traslado.tasa = decimal.Parse(tr[2]);
                        comprobante.TituloOtros = traslado.tasa.ToString() + "%";
                        listaTraslados.Add(traslado);
                    }
                    impuestos.Traslados = listaTraslados.ToArray();
                    impuestos.totalImpuestosTrasladados = listaTraslados.Sum(p => p.importe);
                    impuestos.totalImpuestosTrasladadosSpecified = true;

                }

                if (impuestosRetencion.Any())
                {

                    List<GeneradorCfdi.ComprobanteImpuestosRetencion> listaTraslados =
                        new List<GeneradorCfdi.ComprobanteImpuestosRetencion>();
                    foreach (var tr in impuestosRetencion)
                    {
                        GeneradorCfdi.ComprobanteImpuestosRetencion retencion =
                            new GeneradorCfdi.ComprobanteImpuestosRetencion();
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
                    impuestos.totalImpuestosRetenidos = listaTraslados.Sum(p => p.importe);
                    impuestos.totalImpuestosRetenidosSpecified = true;
                }

                comprobante.CantidadLetra = CantidadLetra.Enletras(comprobante.total.ToString(), comprobante.Moneda,
                                                                   comprobante.Emisor.rfc);

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
                #region Addenda AMECE XML 7.1
                if (rfp != null && rfp.Length > 0)
                {
                    request.DeliveryDate = DateTime.ParseExact(rfp[1], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    request.requestForPaymentIdentification = new requestForPaymentRequestForPaymentIdentification();
                    request.requestForPaymentIdentification.entityType = requestForPaymentRequestForPaymentIdentificationEntityType.INVOICE;
                    request.requestForPaymentIdentification.uniqueCreatorIdentification = ParserNtLink.GetValue(rfpId[1]);
                    request.orderIdentification = new requestForPaymentOrderIdentification()
                    {
                        referenceIdentification = new requestForPaymentOrderIdentificationReferenceIdentification[]
                                                                                    {
                                                                                        new requestForPaymentOrderIdentificationReferenceIdentification()
                                                                                            {
                                                                                                type = requestForPaymentOrderIdentificationReferenceIdentificationType.ON, 
                                                                                                Value = oid[1]
                                                                                            }
                                                                                    },
                        ReferenceDate = DateTime.ParseExact(oid[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        ReferenceDateSpecified = true
                    };
                    request.specialInstruction = new requestForPaymentSpecialInstruction[]
                                                 {
                                                     new requestForPaymentSpecialInstruction()
                                                         {
                                                             code = requestForPaymentSpecialInstructionCode.ZZZ,
                                                             text = new string[] {comprobante.CantidadLetra}
                                                         }
                                                 };
                    request.AdditionalInformation = new requestForPaymentReferenceIdentification[]
                                                    {
                                                        new requestForPaymentReferenceIdentification()
                                                            {
                                                                type = requestForPaymentReferenceIdentificationType.ATZ,
                                                                Value = adin[1]
                                                            },
                                                        new requestForPaymentReferenceIdentification()
                                                            {
                                                                type = requestForPaymentReferenceIdentificationType.IV,
                                                                Value = adin[2]
                                                            }
                                                    };
                    request.buyer = new requestForPaymentBuyer();
                    request.buyer.gln = buy[1];
                    request.seller = new requestForPaymentSeller();
                    request.seller.gln = sel[1];
                    request.seller.alternatePartyIdentification = new requestForPaymentSellerAlternatePartyIdentification();
                    request.seller.alternatePartyIdentification.type = requestForPaymentSellerAlternatePartyIdentificationType.SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY;
                    request.seller.alternatePartyIdentification.Value = sel[2];
                    request.shipTo = new requestForPaymentShipTo()
                    {
                        gln = shipto[1],
                        nameAndAddress = new requestForPaymentShipToNameAndAddress()
                        {
                            name = new string[] { ParserNtLink.GetValue(shipto[2]) },
                            streetAddressOne = new string[] { ParserNtLink.GetValue(shipto[3]) },
                            city = new string[] { ParserNtLink.GetValue(shipto[4]) },
                            postalCode = new string[] { ParserNtLink.GetValue(shipto[5]) }
                        }
                    };
                    request.currency = new requestForPaymentCurrency[]
                                       {
                                           new requestForPaymentCurrency()
                                               {
                                                   currencyISOCode = requestForPaymentCurrencyCurrencyISOCode.MXN,
                                                   currencyFunction = new requestForPaymentCurrencyCurrencyFunction[]{requestForPaymentCurrencyCurrencyFunction.BILLING_CURRENCY, },
                                                   rateOfChange = decimal.Parse(comprobante.TipoCambio),
                                                   rateOfChangeSpecified = true
                                                   
                                               }
                                       };
                    request.paymentTerms = new requestForPaymentPaymentTerms()
                    {
                        paymentTermsEvent = requestForPaymentPaymentTermsPaymentTermsEvent.DATE_OF_INVOICE,
                        PaymentTermsRelationTime = requestForPaymentPaymentTermsPaymentTermsRelationTime.REFERENCE_AFTER,
                        PaymentTermsRelationTimeSpecified = true,
                        paymentTermsEventSpecified = true,
                        netPayment = new requestForPaymentPaymentTermsNetPayment()
                        {
                            netPaymentTermsType = requestForPaymentPaymentTermsNetPaymentNetPaymentTermsType.BASIC_NET,
                            paymentTimePeriod = new requestForPaymentPaymentTermsNetPaymentPaymentTimePeriod()
                            {
                                timePeriodDue = new requestForPaymentPaymentTermsNetPaymentPaymentTimePeriodTimePeriodDue()
                                {
                                    timePeriod = requestForPaymentPaymentTermsNetPaymentPaymentTimePeriodTimePeriodDueTimePeriod.DAYS,
                                    value = pt[1]
                                }
                            },


                        }

                    };
                    request.allowanceCharge = new requestForPaymentAllowanceCharge[]
                                              {
                                                  new requestForPaymentAllowanceCharge()
                                                      {
                                                          settlementType = requestForPaymentAllowanceChargeSettlementType.BILL_BACK,
                                                          allowanceChargeType = requestForPaymentAllowanceChargeAllowanceChargeType.ALLOWANCE_GLOBAL,
                                                          monetaryAmountOrPercentage = new requestForPaymentAllowanceChargeMonetaryAmountOrPercentage()
                                                                                           {
                                                                                               rate = new requestForPaymentAllowanceChargeMonetaryAmountOrPercentageRate()
                                                                                                          {
                                                                                                              @base = requestForPaymentAllowanceChargeMonetaryAmountOrPercentageRateBase.INVOICE_VALUE,
                                                                                                              percentage = comprobante.descuento

                                                                                                          }
                                                                                           }
                                                      }
                                              };
                    var lineItems = new List<requestForPaymentLineItem>();
                    int countConceptos = 0;
                    foreach (var comprobanteConcepto in conceptos)
                    {
                        requestForPaymentLineItem li = new requestForPaymentLineItem();
                        li.type = "SimpleInvoiceLineItemType";
                        li.number = (++countConceptos).ToString();
                        li.tradeItemIdentification = new requestForPaymentLineItemTradeItemIdentification()
                        {
                            gtin = comprobanteConcepto.noIdentificacion
                        };
                        li.tradeItemDescriptionInformation = new requestForPaymentLineItemTradeItemDescriptionInformation()
                        {
                            language = requestForPaymentLineItemTradeItemDescriptionInformationLanguage.ES,
                            languageSpecified = true,
                            longText = comprobanteConcepto.descripcion
                        };
                        li.invoicedQuantity = new requestForPaymentLineItemInvoicedQuantity()
                        {
                            unitOfMeasure = comprobanteConcepto.unidad,
                            Text = new string[] { comprobanteConcepto.cantidad.ToString("F2", CultureInfo.InvariantCulture) }
                        };
                        li.grossPrice = new requestForPaymentLineItemGrossPrice()
                        {
                            Amount = comprobanteConcepto.valorUnitario
                        };
                        li.AdditionalInformation = new requestForPaymentLineItemAdditionalInformation()
                        {
                            referenceIdentification = new requestForPaymentLineItemAdditionalInformationReferenceIdentification()
                            {
                                type = requestForPaymentLineItemAdditionalInformationReferenceIdentificationType.ON,
                                Value = oid[1]
                            }
                        };
                        //var iva = 1.16;
                        li.totalLineAmount = new requestForPaymentLineItemTotalLineAmount()
                                                 {
                                                     grossAmount = new requestForPaymentLineItemTotalLineAmountGrossAmount()
                                                                       {
                                                                           Amount = comprobanteConcepto.importe,
                                                                       },
                                                     netAmount = new requestForPaymentLineItemTotalLineAmountNetAmount()
                                                                     {
                                                                         
                                                                         Amount = comprobanteConcepto.importe
                                                                     }
                                                                       
                                                                    
                                                 };
                        lineItems.Add(li);
                    }
                    request.lineItem = lineItems.ToArray();
                    request.totalAmount = new requestForPaymentTotalAmount()
                    {
                        Amount = comprobante.subTotal
                    };
                    
                    request.TotalAllowanceCharge = new requestForPaymentTotalAllowanceCharge[]
                                                   {
                                                       new requestForPaymentTotalAllowanceCharge()
                                                           {
                                                               Amount = comprobante.descuento
                                                           }
                                                   };
                    request.baseAmount = new requestForPaymentBaseAmount()
                    {
                        Amount = comprobante.subTotal
                    };
                    var rfpTaxes = new List<requestForPaymentTax>();
                    foreach (var tras in comprobante.Impuestos.Traslados)
                    {
                        var tax = new requestForPaymentTax()
                        {
                            type = requestForPaymentTaxType.VAT,
                            taxAmount = comprobante.Impuestos.totalImpuestosTrasladados,
                            taxAmountSpecified = true,
                            taxPercentage = tras.tasa,
                            taxPercentageSpecified = true,
                            taxCategory = requestForPaymentTaxTaxCategory.TRANSFERIDO,
                            taxCategorySpecified = true,
                            typeSpecified = true
                        };
                        rfpTaxes.Add(tax);
                    }
                    request.tax = rfpTaxes.ToArray();
                    request.payableAmount = new requestForPaymentPayableAmount()
                    {
                        Amount = comprobante.total
                    };
                    comprobante.AddendaAmece = request;
                }
                #endregion

                return new List<Comprobante>() { comprobante };
              */ 
            }
            else
            {
                throw new Exception("El archivo " + fileName + " está vacío");
            }
               
        }

        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetAmece71Adenda(data, comp);
                if (adenda != null)
                {
                    comp.AddendaAmece = adenda;
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(AddendaAmece.requestForPayment), null, null);
                }
                res.Add(comp);
            }
            return res;
        }

        private requestForPayment GetAmece71Adenda(string[][] data, Comprobante comprobante)
        {
            #region GetData
            var rfp = data.FirstOrDefault(j => j[0] == "RFP");
            var rfpId = data.FirstOrDefault(j => j[0] == "RFPID");
            var oid = data.FirstOrDefault(j => j[0] == "OID");
            var adin = data.FirstOrDefault(j => j[0] == "ADIN");
            var buy = data.FirstOrDefault(j => j[0] == "BUY");
            var sel = data.FirstOrDefault(j => j[0] == "SEL");
            var shipto = data.FirstOrDefault(j => j[0] == "SHIPTO");
            var pt = data.FirstOrDefault(j => j[0] == "PT");

            requestForPayment request = new requestForPayment();
            #endregion
            if (rfp != null && rfp.Length > 0)
            {
                #region Parse addenda
                #region DeliveryDate, paymentIdentification
                request.DeliveryDate = DateTime.ParseExact(rfp[1], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                request.requestForPaymentIdentification = new requestForPaymentRequestForPaymentIdentification();
                request.requestForPaymentIdentification.entityType = requestForPaymentRequestForPaymentIdentificationEntityType.INVOICE;
                request.requestForPaymentIdentification.uniqueCreatorIdentification = ParserNtLink.GetValue(rfpId[1]);
                request.orderIdentification = new requestForPaymentOrderIdentification()
                {
                    referenceIdentification = new requestForPaymentOrderIdentificationReferenceIdentification[]
                                                                                    {
                                                                                        new requestForPaymentOrderIdentificationReferenceIdentification()
                                                                                            {
                                                                                                type = requestForPaymentOrderIdentificationReferenceIdentificationType.ON, 
                                                                                                Value = oid[1]
                                                                                            }
                                                                                    },
                    ReferenceDate = DateTime.ParseExact(oid[2], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    ReferenceDateSpecified = true
                };
                request.specialInstruction = new requestForPaymentSpecialInstruction[]
                                                 {
                                                     new requestForPaymentSpecialInstruction()
                                                         {
                                                             code = requestForPaymentSpecialInstructionCode.ZZZ,
                                                             text = new string[] {comprobante.CantidadLetra}
                                                         }
                                                 };
                request.AdditionalInformation = new requestForPaymentReferenceIdentification[]
                                                    {
                                                        new requestForPaymentReferenceIdentification()
                                                            {
                                                                type = requestForPaymentReferenceIdentificationType.ATZ,
                                                                Value = adin[1]
                                                            },
                                                        new requestForPaymentReferenceIdentification()
                                                            {
                                                                type = requestForPaymentReferenceIdentificationType.IV,
                                                                Value = adin[2]
                                                            }
                                                    };
                #endregion
                #region Buyer, seller, payment terms
                request.buyer = new requestForPaymentBuyer();
                request.buyer.gln = buy[1];
                request.seller = new requestForPaymentSeller();
                request.seller.gln = sel[1];
                request.seller.alternatePartyIdentification = new requestForPaymentSellerAlternatePartyIdentification();
                request.seller.alternatePartyIdentification.type = requestForPaymentSellerAlternatePartyIdentificationType.SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY;
                request.seller.alternatePartyIdentification.Value = sel[2];
                request.shipTo = new requestForPaymentShipTo()
                {
                    gln = shipto[1],
                    nameAndAddress = new requestForPaymentShipToNameAndAddress()
                    {
                        name = new string[] { ParserNtLink.GetValue(shipto[2]) },
                        streetAddressOne = new string[] { ParserNtLink.GetValue(shipto[3]) },
                        city = new string[] { ParserNtLink.GetValue(shipto[4]) },
                        postalCode = new string[] { ParserNtLink.GetValue(shipto[5]) }
                    }
                };
                request.currency = new requestForPaymentCurrency[]
                                       {
                                           new requestForPaymentCurrency()
                                               {
                                                   currencyISOCode = requestForPaymentCurrencyCurrencyISOCode.MXN,
                                                   currencyFunction = new requestForPaymentCurrencyCurrencyFunction[]{requestForPaymentCurrencyCurrencyFunction.BILLING_CURRENCY, },
                                                   rateOfChange = comprobante.TipoCambio,
                                                   rateOfChangeSpecified = true
                                                   
                                               }
                                       };
                request.paymentTerms = new requestForPaymentPaymentTerms()
                {
                    paymentTermsEvent = requestForPaymentPaymentTermsPaymentTermsEvent.DATE_OF_INVOICE,
                    PaymentTermsRelationTime = requestForPaymentPaymentTermsPaymentTermsRelationTime.REFERENCE_AFTER,
                    PaymentTermsRelationTimeSpecified = true,
                    paymentTermsEventSpecified = true,
                    netPayment = new requestForPaymentPaymentTermsNetPayment()
                    {
                        netPaymentTermsType = requestForPaymentPaymentTermsNetPaymentNetPaymentTermsType.BASIC_NET,
                        paymentTimePeriod = new requestForPaymentPaymentTermsNetPaymentPaymentTimePeriod()
                        {
                            timePeriodDue = new requestForPaymentPaymentTermsNetPaymentPaymentTimePeriodTimePeriodDue()
                            {
                                timePeriod = requestForPaymentPaymentTermsNetPaymentPaymentTimePeriodTimePeriodDueTimePeriod.DAYS,
                                value = pt[1]
                            }
                        },


                    }

                };
                #endregion
                #region AllowanceCharge
                request.allowanceCharge = new requestForPaymentAllowanceCharge[]
                                              {
                                                  new requestForPaymentAllowanceCharge()
                                                      {
                                                          settlementType = requestForPaymentAllowanceChargeSettlementType.BILL_BACK,
                                                          allowanceChargeType = requestForPaymentAllowanceChargeAllowanceChargeType.ALLOWANCE_GLOBAL,
                                                          monetaryAmountOrPercentage = new requestForPaymentAllowanceChargeMonetaryAmountOrPercentage()
                                                                                           {
                                                                                               rate = new requestForPaymentAllowanceChargeMonetaryAmountOrPercentageRate()
                                                                                                          {
                                                                                                              @base = requestForPaymentAllowanceChargeMonetaryAmountOrPercentageRateBase.INVOICE_VALUE,
                                                                                                              percentage =Convert.ToDecimal( comprobante.Descuento)

                                                                                                          }
                                                                                           }
                                                      }
                                              };
                #endregion
                #region Conceptos
                var lineItems = new List<requestForPaymentLineItem>();
                int countConceptos = 0;
                foreach (var comprobanteConcepto in comprobante.Conceptos)
                {
                    requestForPaymentLineItem li = new requestForPaymentLineItem();
                    li.type = "SimpleInvoiceLineItemType";
                    li.number = (++countConceptos).ToString();
                    li.tradeItemIdentification = new requestForPaymentLineItemTradeItemIdentification()
                    {
                        gtin = comprobanteConcepto.NoIdentificacion
                    };
                    li.tradeItemDescriptionInformation = new requestForPaymentLineItemTradeItemDescriptionInformation()
                    {
                        language = requestForPaymentLineItemTradeItemDescriptionInformationLanguage.ES,
                        languageSpecified = true,
                        longText = comprobanteConcepto.Descripcion
                    };
                    li.invoicedQuantity = new requestForPaymentLineItemInvoicedQuantity()
                    {
                        unitOfMeasure = comprobanteConcepto.Unidad,
                        Text = new string[] { comprobanteConcepto.Cantidad.ToString("F2", CultureInfo.InvariantCulture) }
                    };
                    li.grossPrice = new requestForPaymentLineItemGrossPrice()
                    {
                        Amount =Convert.ToDecimal( comprobanteConcepto.ValorUnitario)
                    };
                    //-------
                    li.netPrice = new requestForPaymentLineItemNetPrice()
                    {
                        Amount = Convert.ToDecimal(comprobanteConcepto.ValorUnitario)
                    };
                    //---
                    li.AdditionalInformation = new requestForPaymentLineItemAdditionalInformation()
                    {
                        referenceIdentification = new requestForPaymentLineItemAdditionalInformationReferenceIdentification()
                        {
                            type = requestForPaymentLineItemAdditionalInformationReferenceIdentificationType.ON,
                            Value = oid[1]
                        }
                    };
                    //var iva = 1.16;
                    li.totalLineAmount = new requestForPaymentLineItemTotalLineAmount()
                    {
                        grossAmount = new requestForPaymentLineItemTotalLineAmountGrossAmount()
                        {
                            Amount = Convert.ToDecimal( comprobanteConcepto.Importe),
                        },
                        netAmount = new requestForPaymentLineItemTotalLineAmountNetAmount()
                        {

                            Amount =Convert.ToDecimal( comprobanteConcepto.Importe)
                        }


                    };
                    lineItems.Add(li);
                }
                #endregion
                #region Amounts & taxes
                request.lineItem = lineItems.ToArray();
                request.totalAmount = new requestForPaymentTotalAmount()
                {
                    Amount = comprobante.SubTotal
                };

                request.TotalAllowanceCharge = new requestForPaymentTotalAllowanceCharge[]
                                                   {
                                                       new requestForPaymentTotalAllowanceCharge()
                                                           {
                                                               Amount =Convert.ToDecimal( comprobante.Descuento)
                                                           }
                                                   };
                request.baseAmount = new requestForPaymentBaseAmount()
                {
                    Amount = comprobante.SubTotal
                };
                var rfpTaxes = new List<requestForPaymentTax>();
                foreach (var tras in comprobante.Impuestos.Traslados)
                {
                    var tax = new requestForPaymentTax()
                    {
                        type = requestForPaymentTaxType.VAT,
                        taxAmount =Convert.ToDecimal( comprobante.Impuestos.TotalImpuestosTrasladados),
                        taxAmountSpecified = true,
                        taxPercentage =Convert.ToDecimal( tras.TasaOCuota),
                        taxPercentageSpecified = true,
                        taxCategory = requestForPaymentTaxTaxCategory.TRANSFERIDO,
                        taxCategorySpecified = true,
                        typeSpecified = true
                    };
                    rfpTaxes.Add(tax);
                }
                request.tax = rfpTaxes.ToArray();
                request.payableAmount = new requestForPaymentPayableAmount()
                {
                    Amount = comprobante.Total
                };
                #endregion
                #endregion
                return request;
            }
            else
            {
                return null;
            }
        }
    }
}
