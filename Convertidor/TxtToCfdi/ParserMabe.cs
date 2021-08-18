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
using System.Xml.Linq;

namespace TxtToCfdi
{
    public class ParserMabe : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserMabe));

        

        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetMabeAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(Factura), "mabe", "https://recepcionfe.mabempresa.com/cfd/addenda/v1");
                    
                    comp.XmlAdenda = xmlAdenda;

                 
                }
                res.Add(comp);
            }
            return res;
        }

        private Factura GetMabeAdenda(string[][] data)
        {
            #region GetData
            var cabeceraMabe = data.FirstOrDefault(j => j[0] == "MABE");
            var mabeDetalle = data.Where(j => j[0] == "MABEDT");
            var mabeDesc = data.FirstOrDefault(j => j[0] == "MABEDESC");
            var mabeTotal = data.FirstOrDefault(j => j[0] == "MABETOT");
            var mabeTranslados = data.Where(j => j[0] == "MABETRAS");
            var mabeRetenciones = data.Where(j => j[0] == "MABERET");
            #endregion
            if (cabeceraMabe != null)
            {
                #region Parse adenda
                #region Datos generales
                Factura aMabe = new Factura();
                aMabe.version = decimal.Parse(ParserNtLink.GetValue(cabeceraMabe[1]));
                aMabe.tipoDocumento = (FacturaTipoDocumento)Enum.Parse(typeof(FacturaTipoDocumento), ParserNtLink.GetValue(cabeceraMabe[2]));
                aMabe.folio = ParserNtLink.GetValue(cabeceraMabe[3]);
                aMabe.fecha = DateTime.ParseExact(cabeceraMabe[4], "dd-MM-yyyy", CultureInfo.InvariantCulture);
                aMabe.ordenCompra = ParserNtLink.GetValue(cabeceraMabe[5]);
                aMabe.referencia1 = ParserNtLink.GetValue(cabeceraMabe[6]);
                aMabe.referencia2 = ParserNtLink.GetValue(cabeceraMabe[7]);

                FacturaMoneda aMabeMoneda = new FacturaMoneda();
                aMabeMoneda.tipoMoneda = (FacturaMonedaTipoMoneda)Enum.Parse(typeof(FacturaMonedaTipoMoneda), ParserNtLink.GetValue(cabeceraMabe[8]));
                if(!string.IsNullOrEmpty(cabeceraMabe[9]))
                {
                    aMabeMoneda.tipoCambio = decimal.Parse(ParserNtLink.GetValue(cabeceraMabe[9]));
                    aMabeMoneda.tipoCambioSpecified=true;
                }
                else
                {
                    aMabeMoneda.tipoCambioSpecified=false;
                }
                aMabeMoneda.importeConLetra = ParserNtLink.GetValue(cabeceraMabe[10]);

                aMabe.Moneda = aMabeMoneda;
                #endregion
                #region Provedor, entrega, detalle
                FacturaProveedor aMabeProveedor = new FacturaProveedor()
                {
                    codigo = ParserNtLink.GetValue(cabeceraMabe[11])
                };

                aMabe.Proveedor = aMabeProveedor;

                FacturaEntrega aMabeEntrega = new FacturaEntrega()
                {
                    plantaEntrega = ParserNtLink.GetValue(cabeceraMabe[12]),
                    calle = ParserNtLink.GetValue(cabeceraMabe[13]),
                    noExterior = ParserNtLink.GetValue(cabeceraMabe[14]),
                    noInterior = ParserNtLink.GetValue(cabeceraMabe[15]),
                    codigoPostal = ParserNtLink.GetValue(cabeceraMabe[16])
                };

                aMabe.Entrega = aMabeEntrega;
                if (mabeDetalle.Any())
                {
                    List<FacturaDetalle> listaMabeDetalle = new List<FacturaDetalle>();
                    foreach (var detalle in mabeDetalle)
                    {
                        FacturaDetalle aMabeDetalle = new FacturaDetalle()
                        {
                            noLineaArticulo = ParserNtLink.GetValue(detalle[1]),
                            codigoArticulo = ParserNtLink.GetValue(detalle[2]),
                            descripcion = ParserNtLink.GetValue(detalle[3]),
                            unidad = ParserNtLink.GetValue(detalle[4]),
                            cantidad = decimal.Parse(ParserNtLink.GetValue(detalle[5])),
                            precioSinIva = decimal.Parse(ParserNtLink.GetValue(detalle[6])),
                            precioConIva = decimal.Parse(ParserNtLink.GetValue(detalle[7])),
                            precioConIvaSpecified = !string.IsNullOrEmpty(detalle[7]),
                            importeSinIva = decimal.Parse(ParserNtLink.GetValue(detalle[8])),
                            importeConIva = decimal.Parse(ParserNtLink.GetValue(detalle[9])),
                            importeConIvaSpecified = !string.IsNullOrEmpty(detalle[9]),
                        };
                        listaMabeDetalle.Add(aMabeDetalle);
                    }
                    aMabe.Detalles = listaMabeDetalle.ToArray();
                }
                #endregion
                #region Descuento, subtotal, total
                FacturaDescuentos aMabeDesc = new FacturaDescuentos();
                aMabeDesc.tipo = (FacturaDescuentosTipo)Enum.Parse(typeof(FacturaDescuentosTipo), ParserNtLink.GetValue(mabeDesc[1]));
                aMabeDesc.descripcion = mabeDesc[2];
                aMabeDesc.importe = decimal.Parse(ParserNtLink.GetValue(mabeDesc[3]));

                aMabe.Descuentos = aMabeDesc;

                FacturaSubtotal aMabeSubtot = new FacturaSubtotal()
                {
                    importe = decimal.Parse(ParserNtLink.GetValue(mabeTotal[1]))
                };

                aMabe.Subtotal = aMabeSubtot;

                FacturaTotal aMabeTot = new FacturaTotal()
                {
                    importe = decimal.Parse(ParserNtLink.GetValue(mabeTotal[2]))
                };

                aMabe.Total = aMabeTot;
                #endregion
                #region Traslados, retenciones
                if (mabeTranslados.Any())
                {
                    List<FacturaTraslado> listaTranslados = new List<FacturaTraslado>();
                    foreach (var mabeTranslado in mabeTranslados)
                    {
                        FacturaTraslado aMabeTrans = new FacturaTraslado()
                        {
                            tipo = ParserNtLink.GetValue(mabeTranslado[1]),
                            tasa = decimal.Parse(ParserNtLink.GetValue(mabeTranslado[2])),
                            importe = decimal.Parse(ParserNtLink.GetValue(mabeTranslado[3]))
                        };
                        listaTranslados.Add(aMabeTrans);
                    }
                    aMabe.Traslados = listaTranslados.ToArray();
                }

                if (mabeRetenciones.Any())
                {
                    List<FacturaRetencion> listaRetenciones = new List<FacturaRetencion>();
                    foreach (var mabeRetencion in mabeRetenciones)
                    {
                        FacturaRetencion aMabeReten = new FacturaRetencion()
                        {
                            tipo = ParserNtLink.GetValue(mabeRetencion[1]),
                            tasa = decimal.Parse(ParserNtLink.GetValue(mabeRetencion[2])),
                            importe = decimal.Parse(ParserNtLink.GetValue(mabeRetencion[3]))
                        };
                        listaRetenciones.Add(aMabeReten);
                    }
                    aMabe.Retenciones = listaRetenciones.ToArray();
                }
                #endregion
                #endregion
                return aMabe;
            }
            else
            {
                return null;
            }
        }
    }
}
