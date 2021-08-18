using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using GeneradorCfdi;
using log4net;
using log4net.Config;

namespace TxtToCfdi
{
    public class Cfd22Parser :IParser
    {

        private static ILog Logger = LogManager.GetLogger(typeof(Cfd22Parser));

        public Cfd22Parser()
        {
            XmlConfigurator.Configure();
        }

        public void ValidarEntrada(string xml)
        {
            try
            {
                StringBuilder errores = new StringBuilder();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                //settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.ValidationEventHandler += new ValidationEventHandler((o,e)=>
                                                                                  {
                                                                                      XmlReader r = (XmlReader)o;
                                                                                      errores.AppendLine(r.Name + " - " + e.Message);

                                                                                  });
                
                settings.Schemas.Add(null,Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfdv22.xsd" ));
                
                XmlReader reader = XmlReader.Create(new StringReader(xml), settings);
                while (reader.Read())
                {
                }

                if (!errores.ToString().Equals(""))
                {
                    throw new ApplicationException(errores.ToString());

                }
               
            }
            catch (Exception ee)
            {
                throw ;
            }
        }
        private readonly XNamespace _ns = "http://www.sat.gob.mx/cfd/2";

        public List<Comprobante> Parse(string fileName)
        {
            Logger.Info(fileName);
            Comprobante2 entrada = null;
            XElement xe = XElement.Load(fileName);
            string venta = null;
            string cliente = null;
            string sucursal = null;
            string vendedor = null;
            string nota1 = null;
            string nota2 = null;
            string nota3 = null;
            string fpago = null;

            var addenda = xe.Elements(_ns + "Addenda").FirstOrDefault();
            if (addenda != null && addenda.Value != null)
            {
                Logger.Info(addenda.Value);
                StringReader sr = new StringReader(addenda.Value);
                string linea = null;
                while((linea = sr.ReadLine()) != null)
                {
                    string[] valores = linea.Split('=');
                    if (valores.Length >= 2)
                    {
                        if (valores[0].Trim() == "venta")
                            venta = valores[1].Replace("\"", string.Empty);
                        if (valores[0].Trim() == "cliente")
                            cliente = valores[1].Replace("\"", string.Empty); ;
                        if (valores[0].Trim() == "sucursal")
                            sucursal = valores[1].Replace("\"", string.Empty); ;
                        if (valores[0].Trim() == "vendedor")
                            vendedor = valores[1].Replace("\"", string.Empty); ;
                        if (valores[0].Trim() == "nota1")
                            nota1 = valores[1].Replace("\"", string.Empty); ;
                        if (valores[0].Trim() == "nota2")
                            nota2 = valores[1].Replace("\"", string.Empty); ;
                        if (valores[0].Trim() == "nota3")
                            nota3 = valores[1].Replace("\"", string.Empty); ;
                        if (valores[0].Trim() == "FPAGO")
                            fpago = valores[1].Replace("\"", string.Empty); ;
                    }
                }
                addenda.Remove();
            }
            
           

            try
            {

                //ValidarEntrada(xe.ToString());
                XmlSerializer ser = new XmlSerializer(typeof(Comprobante2));

                string xmlContent = File.ReadAllText(fileName);
                StringReader sr = new StringReader(xmlContent);
                object obj = ser.Deserialize(sr);
                entrada = obj as Comprobante2;
                

            }
            catch (Exception ee)
            {
                throw;
                
            }
            
            Comprobante comprobante = new Comprobante();

            comprobante.venta = venta;
            comprobante.vendedor = vendedor;
            comprobante.cliente = cliente;
            comprobante.nota1 = nota1;
            comprobante.nota2 = nota2;
            comprobante.nota3 = nota3;
            comprobante.fpago = fpago;
            comprobante.sucursal = sucursal;

            comprobante.Emisor = new GeneradorCfdi.ComprobanteEmisor();
            comprobante.Emisor.Nombre = entrada.Emisor.nombre;
            //comprobante.Emisor.RegimenFiscal = 
            if (entrada.descuento > 0)
            {
                comprobante.Descuento = entrada.descuento.ToString();
                comprobante.DescuentoSpecified = true;
            }
            comprobante.Emisor.Rfc = entrada.Emisor.rfc;
          
          /*  if (entrada.Emisor.ExpedidoEn != null)
            {

                    comprobante.LugarExpedicion = comprobante.cl
            }
            */


            comprobante.Titulo = "Factura";
            comprobante.TipoDeComprobante = entrada.tipoDeComprobante.ToString();
            comprobante.Titulo = "Factura";

            comprobante.Receptor = new GeneradorCfdi.ComprobanteReceptor();
            comprobante.Receptor.Nombre = entrada.Receptor.nombre;
            comprobante.Receptor.Rfc = entrada.Receptor.rfc;
           /* comprobante.Receptor.Domicilio = new GeneradorCfdi.t_Ubicacion();
            comprobante.Receptor.Domicilio.pais = "México";
            comprobante.Receptor.Domicilio.calle = entrada.Receptor.Domicilio.calle;
            comprobante.Receptor.Domicilio.municipio = entrada.Receptor.Domicilio.municipio;
            comprobante.Receptor.Domicilio.estado = entrada.Receptor.Domicilio.estado;
            comprobante.Receptor.Domicilio.colonia = entrada.Receptor.Domicilio.colonia;
            comprobante.Receptor.Domicilio.codigoPostal = entrada.Receptor.Domicilio.codigoPostal;
            comprobante.Receptor.Domicilio.noInterior = entrada.Receptor.Domicilio.noInterior;
            comprobante.Receptor.Domicilio.noExterior = entrada.Receptor.Domicilio.noExterior;
            comprobante.Receptor.Domicilio.referencia = entrada.Receptor.Domicilio.referencia;
            */
            comprobante.LugarExpedicion = entrada.LugarExpedicion;
            comprobante.Fecha = entrada.fecha.ToString();//Convert.ToDateTime(DateTime.Now.ToString("s"));//entrada.fecha;
            comprobante.Total = entrada.total;

            //comprobante.Leyenda = factura.Factura.Leyenda;
            //comprobante.LeyendaInferior = emp.LeyendaInferior;
            //comprobante.LeyendaSuperior = emp.LeyendaSuperior;
            comprobante.Folio = entrada.folio;
            comprobante.LugarExpedicion = entrada.LugarExpedicion;
            comprobante.MetodoPago = entrada.metodoDePago;
         //   comprobante.NumCtaPago = entrada.NumCtaPago;
            comprobante.Moneda = entrada.Moneda;
          //  comprobante.Regimen = entrada.Emisor.RegimenFiscal[0].Regimen;
            comprobante.SubTotal = entrada.subTotal;// factura.Factura.Total.Value - factura.Factura.IVA.Value + factura.Factura.RetenciónIva;
            comprobante.Serie = entrada.serie;
            comprobante.FormaPago = entrada.formaDePago;
           
            List<GeneradorCfdi.ComprobanteConcepto> conceptos = new List<GeneradorCfdi.ComprobanteConcepto>();
            foreach (var detalle in entrada.Conceptos)
            {
                GeneradorCfdi.ComprobanteConcepto con = new GeneradorCfdi.ComprobanteConcepto();
                con.Descripcion = detalle.descripcion;
                if (!string.IsNullOrEmpty(detalle.noIdentificacion))
                    con.NoIdentificacion = detalle.noIdentificacion;
                con.Detalles = detalle.tipoprod;
                con.Cantidad = detalle.cantidad;
                con.ValorUnitario = detalle.valorUnitario.ToString();
                con.Importe = detalle.importe.ToString();
                con.Unidad = detalle.unidad;
               
                 
                
                //con.OrdenCompra = detalle.OrdenCompra;
                //if (detalle.Items != null && detalle.Items.Count() > 0)
                //{

                //    var predial = new GeneradorCfdi.ComprobanteConceptoCuentaPredial
                //    {
                //        numero = detalle.CuentaPredial
                //    };
                //    con.Items = new object[] { predial };
                //}
                conceptos.Add(con);
            }
            comprobante.Conceptos = conceptos.ToArray();
            GeneradorCfdi.ComprobanteImpuestos impuestos = new GeneradorCfdi.ComprobanteImpuestos();
            if (entrada.Impuestos != null && entrada.Impuestos.Traslados != null && entrada.Impuestos.Traslados.Length > 0)
            {

                List<GeneradorCfdi.ComprobanteImpuestosTraslado> listaTraslados = new List<GeneradorCfdi.ComprobanteImpuestosTraslado>();
                foreach (ComprobanteImpuestosTraslado tr in entrada.Impuestos.Traslados)
                {
                    GeneradorCfdi.ComprobanteImpuestosTraslado traslado = new GeneradorCfdi.ComprobanteImpuestosTraslado();
                    traslado.Importe = tr.importe.ToString();

                    traslado.Impuesto = tr.impuesto.ToString();
                    traslado.TasaOCuota = tr.tasa.ToString();
                    listaTraslados.Add(traslado);
                }
                impuestos.Traslados = listaTraslados.ToArray();
              //  impuestos.TotalImpuestosTrasladados = listaTraslados.Sum(p => p.Importe);
              //  impuestos.TotalImpuestosTrasladadosSpecified = true;
            }

            if (entrada.Impuestos != null && entrada.Impuestos.Retenciones != null && entrada.Impuestos.Retenciones.Length > 0)
            {

                List<GeneradorCfdi.ComprobanteImpuestosRetencion> listaTraslados = new List<GeneradorCfdi.ComprobanteImpuestosRetencion>();
                foreach (ComprobanteImpuestosRetencion tr in entrada.Impuestos.Retenciones)
                {
                    GeneradorCfdi.ComprobanteImpuestosRetencion retencion = new GeneradorCfdi.ComprobanteImpuestosRetencion();
                    retencion.Importe = tr.importe.ToString();

                    retencion.Impuesto =  tr.impuesto.ToString();
                    listaTraslados.Add(retencion);
                }
                impuestos.Retenciones = listaTraslados.ToArray();
               // impuestos.TotalImpuestosRetenidos = listaTraslados.Sum(p => p.importe);
               // impuestos.TotalImpuestosRetenidosSpecified = true;
            }

            comprobante.CantidadLetra = CantidadLetra.Enletras(comprobante.Total.ToString(), comprobante.Moneda, comprobante.Emisor.Rfc);

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
            if (entrada.Complemento != null && entrada.Complemento.Any != null)
            {
                comprobante.Complemento.Any = entrada.Complemento.Any;
            }
            comprobante.Impuestos = impuestos;
            return new List<Comprobante>(){comprobante};
        }
    }
}
