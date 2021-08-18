using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using AddendaAmece;
using GeneradorCfdi.ServicioTimbrado;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Microsoft.Reporting.WinForms;
using log4net;
using log4net.Config;
//using ServicioLocal.Business;
using ConvertidorCfdi;
using System.Collections;
using System.Net.NetworkInformation;
using GeneradorCfdi.Complementos;
using GeneradorCfdi.ComplementoCartaPorte;


namespace GeneradorCfdi
{


    public class Generador
    {
        private string clave = "rgv123";
    
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Generador));
        public Generador()
        {
            XmlConfigurator.Configure();
        }



        public class SidetecStringWriter : StringWriter
        {
            private Encoding _encoding;

            public override Encoding Encoding
            {
                get { return _encoding; }
            }

            public SidetecStringWriter(Encoding encoding)
            {
                this._encoding = encoding;
            }


        }

        private string GetXml(Comprobante p)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Comprobante));
            using (MemoryStream memStream = new MemoryStream())
            {
                var sw = new StreamWriter(memStream, Encoding.UTF8);
                using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { Indent = false, Encoding = Encoding.UTF8 }))
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    namespaces.Add("cfdi", "http://www.sat.gob.mx/cfd/3");

                    ser.Serialize(xmlWriter, p, namespaces);
                    string xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                    xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                    xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
                    if (p.XmlComplemento != null)
                    {
                        XElement comprobante = XElement.Parse(xml);
                        var comp = comprobante.Elements(_ns + "Complemento").FirstOrDefault();
                        if (comp == null)
                        {
                            comprobante.Add(new XElement(_ns + "Complemento"));
                            comp = comprobante.Elements(_ns + "Complemento").FirstOrDefault();
                        }
                        comp.Add(XElement.Parse(p.XmlComplemento));
                        SidetecStringWriter swriter = new SidetecStringWriter(Encoding.UTF8);
                        comprobante.Save(swriter, SaveOptions.DisableFormatting);
                        xml= swriter.ToString();
                    }
                    //nuevo pata impuestos locales
                    int x = xml.IndexOf("cfdi:implocal");
                    if (x != -1)
                    {
                        xml = xml.Replace("cfdi:implocal", "implocal:ImpuestosLocales");
                        xml = xml.Replace("RetencionesLocales", "implocal:RetencionesLocales");
                        xml = xml.Replace("TrasladosLocales", "implocal:TrasladosLocales");
                        xml = xml.Replace("xmlns=\"http://www.sat.gob.mx/implocal\"", "");
                        xml = xml.Replace("<cfdi:Complemento", "<cfdi:Complemento xmlns:implocal=\"http://www.sat.gob.mx/implocal\" xsi:schemaLocation=\"http://www.sat.gob.mx/implocal http://www.sat.gob.mx/sitio_internet/cfd/implocal/implocal.xsd\"");
                    }

                    int x2 = xml.IndexOf("cartaporte:CartaPorte");
                    if (x2 != -1)
                    {
                        xml = xml.Replace("xmlns:cartaporte=\"http://www.sat.gob.mx/CartaPorte\"", "");
                        xml = xml.Replace("xmlns:cfdi=\"http://www.sat.gob.mx/cfd/3\"", "xmlns:cfdi=\"http://www.sat.gob.mx/cfd/3\" xmlns:cartaporte=\"http://www.sat.gob.mx/CartaPorte\"");
                    
                    }
                    //int x2 = xml.IndexOf("<nomina12:Nomina");
                    //if (x2 != -1)
                    //{
                    //    xml = xml.Replace("<nomina12:Nomina xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.sat.gob.mx/nomina12 http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina12.xsd\"", "<nomina12:Nomina");
                    //    xml = xml.Replace("xmlns:nomina12=\"http://www.sat.gob.mx/nomina12\"","");
                    //    xml = xml.Replace("xmlns:cfdi=\"http://www.sat.gob.mx/cfd/3\"","xmlns:cfdi=\"http://www.sat.gob.mx/cfd/3\" xmlns:nomina12=\"http://www.sat.gob.mx/nomina12\"");
                    //}
                    //fin impuestos locales
                    return xml;
                }
            }
        }


        private string GetXmlTimbre(TimbreFiscalDigital p)
        {
            XmlSerializer ser = new XmlSerializer(typeof(TimbreFiscalDigital));
            using (MemoryStream memStream = new MemoryStream())
            {
                var sw = new StreamWriter(memStream, Encoding.UTF8);
                using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { Indent = false, Encoding = Encoding.UTF8 }))
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    namespaces.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                    ser.Serialize(xmlWriter, p, namespaces);
                    string xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                    xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                    xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
                    return xml;
                }
            }
        }


        public Byte[] GetPdfFromComprobante(string xmlComprobante, string logoEmpresa)
        {
            Comprobante comprobante = GetComprobanteFromString(xmlComprobante);
            comprobante.CadenaOriginalTimbre = @"||1.0|6D586938-1A02-44A1-B015-CC1B37D56BCF|2012-08-11T21:46:41|kB0Caoa3gtsqo8klGTHaOgDLCOX1mjT84vaTm0l9iM82sSfTlhLrqEd5o+X3lzETlxaLmQogX27N+tD+Izc/BsqFWHax5Ln2krh9ER0feWD4CglUqGZwnu7BWnFcLNgN8OtcmvrRibjBTsAEOvcfZu4q80aXb/b2LxEHbqM3yuw=|00001000000201614141||";
            comprobante.CantidadLetra = "UNO MXN 16/100";
            return this.GetPdfFromComprobante(comprobante, logoEmpresa);
        }

        public void GeneraCodigoBidimensional(string cadena, string ruta)
        {
            QrEncoder encoder = new QrEncoder();
            QrCode qrCode;
            if (!encoder.TryEncode(cadena, out qrCode))
            {
                throw new Exception("Error al generar codigo bidimensional: " + cadena);
            }
            GraphicsRenderer gRenderer = new GraphicsRenderer(new FixedModuleSize(2, QuietZoneModules.Two), Brushes.Black, Brushes.White);

            MemoryStream ms = new MemoryStream();
            gRenderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
            File.WriteAllBytes(ruta,ms.GetBuffer());
            
            
        }

        //-
        public Byte[] GetPdfFromAcuseCancelacion(AcuseCan can, string estatus, string estatusCancelacion, string rfcReceptor, string StatusCancelacion)
        {
            Logger.Debug("Generando PDF del acuse");
            byte[] byteViewer = new byte[0];
            using (ReportViewer reportViewer1 = new ReportViewer())
            {
                //AcuseCan can = ParseAcuse(respuesta.Acuse);

                string ruta = GetRutaPdfAcuseCancelacion();

                ReportDataSource ds = new ReportDataSource("Cancelacion");
                ds.Value = new[] { can }.ToList();
                reportViewer1.LocalReport.EnableHyperlinks = true;
                reportViewer1.LocalReport.ReportPath = ruta;

                reportViewer1.LocalReport.EnableExternalImages = true;
                //reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Cancelacion"));
                List<ReportParameter> Parametros = new List<ReportParameter>();
                Parametros.Add(new ReportParameter("estatus", estatus));
                Parametros.Add(new ReportParameter("estatusCancelacion", estatusCancelacion));
                Parametros.Add(new ReportParameter("rfcReceptor", rfcReceptor));
                Parametros.Add(new ReportParameter("StatusCancelacion", StatusCancelacion));

                reportViewer1.LocalReport.DataSources.Add(ds);
                reportViewer1.LocalReport.SetParameters(Parametros);
             
                //reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Folios", respuesta.StatusUuids));
                reportViewer1.RefreshReport();

                byteViewer = reportViewer1.LocalReport.Render("PDF");
                Logger.Debug(byteViewer.Length + " bytes");
            }


            return byteViewer;
        }


        public class AcuseCan
        {
            public string Sello { get; set; }
            public string Fecha { get; set; }
            public string RfcEmisor { get; set; }
            public string Folio { get; set; }
            public string Status { get; set; }

        }

        public Byte[] GetPdfFromComprobante(Comprobante comprobante, string logoEmpresa)
        {
            try
            {
                Logger.Debug("Generando PDF del comprobante");
                byte[] byteViewer = new byte[0];
                using (ReportViewer reportViewer1 = new ReportViewer())
                {
                    //LocalReport objRDLC = new LocalReport();

                    string ruta = GetRutaPdfCustomizado(comprobante);
                    if (!File.Exists(ruta))
                    {
                        ruta = GetRutaPdf(comprobante);
                    }
                    //objRDLC.DataSources.Clear();
                    reportViewer1.LocalReport.EnableHyperlinks = true;
                    reportViewer1.LocalReport.ReportPath = ruta;

                    if (comprobante.CfdiRelacionados != null)
                    {
                        comprobante.PDFCFDIRelacionadosTipoRelacion=comprobante.CfdiRelacionados.TipoRelacion;
                    }
                    ReportDataSource ds = new ReportDataSource("Factura");
                    ds.Value = new[] { comprobante }.ToList();
                    string enteros;
                    string decimales;
                    string totalLetra = comprobante.Total.ToString();
                    if (totalLetra.IndexOf('.') == -1)
                    {
                        enteros = "0";
                        decimales = "0";
                    }
                    else
                    {
                        enteros = totalLetra.Substring(0, totalLetra.IndexOf('.'));
                        decimales = totalLetra.Substring(totalLetra.IndexOf('.') + 1);
                    }

                    //string total = enteros.PadLeft(10, '0') + "." + decimales.PadRight(6, '0');


                    string total = enteros + "." + decimales;

                    int tam_var = comprobante.Sello.Length;
                    string Var_Sub = comprobante.Sello.Substring((tam_var - 8), 8);

                    //para CFDI
                    string URL = @"https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx";
                    //para retenciones
                    //string URL = @"https://prodretencionverificacion.clouda.sat.gob.mx/";


                    string cadenaCodigo = URL + "?" + "&id=" + comprobante.Complemento.timbreFiscalDigital.UUID.ToUpper() + "&re=" + comprobante.Emisor.Rfc + "&rr=" + comprobante.Receptor.Rfc + "&tt=" + total + "&fe=" + Var_Sub;

            

                          
                    //string cadenaCodigo = "?re=" + comprobante.Emisor.Rfc + "&rr=" + comprobante.Receptor.Rfc + "&tt=" + total + "&id=" +
                     //                     comprobante.Complemento.timbreFiscalDigital.UUID.ToUpper();

                    string rutaTmp = Path.Combine(ConfigurationManager.AppSettings["TmpFiles"], comprobante.Complemento.timbreFiscalDigital.UUID + ".png");
                    //string logoEmpresa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", comprobante.Emisor.rfc, "Logo.png");
                    if (string.IsNullOrEmpty(logoEmpresa) || !File.Exists(logoEmpresa))
                    {
                        logoEmpresa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "LogoGenerico.png");
                    }
                    string rutaxx = ConfigurationManager.AppSettings["TmpFiles"];
                    if (!Directory.Exists(rutaxx))
                        Directory.CreateDirectory(rutaxx);
              


                    GeneraCodigoBidimensional(cadenaCodigo, rutaTmp);
                    //DataSet ds = new DataSet("DataSet1");
                    List<ReportParameter> Parametros = new List<ReportParameter>();
                    Parametros.Add(new ReportParameter("CodigoQR", "file://" + rutaTmp));
                    Parametros.Add(new ReportParameter("LogoEmpresa", "file://" + logoEmpresa));
                    reportViewer1.LocalReport.EnableExternalImages = true;
                    reportViewer1.LocalReport.DataSources.Add(ds);
                    reportViewer1.LocalReport.SetParameters(Parametros);
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Conceptos", comprobante.Conceptos));
                 
                   // comprobante.Complemento.timbreFiscalDigital.FechaTimbrado
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Timbre", new[] { comprobante.Complemento.timbreFiscalDigital }));
                    if (comprobante.ListaConceptoInformativo!=null)
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Informativos", comprobante.ListaConceptoInformativo));
                    else
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Informativos", new object[] { null }));

                    //----------------
                    List<ComprobanteConceptoInformacionAduanera> CIA = new List<ComprobanteConceptoInformacionAduanera>();
                    if (comprobante.Conceptos!=null)
                    {
                          foreach (var con in comprobante.Conceptos)
                            {

                                if (con.InformacionAduanera != null)
                                {
                                    foreach (var inf in con.InformacionAduanera)
                                    {
                                        ComprobanteConceptoInformacionAduanera cia = new ComprobanteConceptoInformacionAduanera();
                                        cia.NumeroPedimento = inf.NumeroPedimento;
                                        CIA.Add(cia);
                                    }
                                }
                            }
                    }
                    if (CIA != null)
                    {
                        if (CIA.Count > 0)
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("InformacionAduanera", CIA));
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("InformacionAduanera", new object[] { null }));
                    }else
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("InformacionAduanera", new object[] { null }));
             
                    //-------------------------------
                    if (comprobante.CfdiRelacionados != null)
                    {
                        //ReportDataSource dst = new ReportDataSource("CfdiRelacionadoTipoRelacion");
                        //dst.Value =  comprobante.CfdiRelacionados.TipoRelacion.ToList();
                        //reportViewer1.LocalReport.DataSources.Add(dst);
                    
                       // reportViewer1.LocalReport.1DataSources.Add(new ReportDataSource("CfdiRelacionadoTipoRelacion", comprobante.CfdiRelacionados.TipoRelacion));

                        if (comprobante.CfdiRelacionados.CfdiRelacionado != null)
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CfdiRelacionado", comprobante.CfdiRelacionados.CfdiRelacionado));
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CfdiRelacionado", new object[] { null }));

                    }
                    else
                    {
                        //reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CfdiRelacionadoTipoRelacion", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CfdiRelacionado", new object[] { null }));
                    }
                    
                    if (comprobante.Complemento.leyendasFicales != null)
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("LeyendasFiscales", new[] { comprobante.Complemento.leyendasFicales }));
                        if (comprobante.Complemento.leyendasFicales.Leyenda != null)
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Leyenda", comprobante.Complemento.leyendasFicales.Leyenda));
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Leyenda", new object[] { null }));

                    }
                    else
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("LeyendasFiscales", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Leyenda", new object[] { null }));
                    
                    }

                    if (comprobante.Complemento.implocal != null)
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ImpuestosLocales", new[] { comprobante.Complemento.implocal }));
                        if (comprobante.Complemento.implocal.RetencionesLocales != null)
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ImpuestosLocalesRetencionesLocales", comprobante.Complemento.implocal.RetencionesLocales));
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ImpuestosLocalesRetencionesLocales", new object[] { null }));

                        if (comprobante.Complemento.implocal.TrasladosLocales != null)
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ImpuestosLocalesTraladosLocales", comprobante.Complemento.implocal.TrasladosLocales));
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ImpuestosLocalesTraladosLocales", new object[] { null }));


                    }
                    else
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ImpuestosLocales", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ImpuestosLocalesTraladosLocales", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ImpuestosLocalesRetencionesLocales", new object[] { null }));

                    }

                    if (comprobante.Complemento.Pag != null)
                    {
                        if (comprobante.Complemento.Pag.Pago != null)
                        {
                            List<ComplementoPAGO> Comple = new List<ComplementoPAGO>();

                            foreach (var pa in comprobante.Complemento.Pag.Pago)
                            {
                                ComplementoPAGO c = new ComplementoPAGO();
                                c.ctaBeneficiario = pa.CtaBeneficiario;
                                c.ctaOrdenante = pa.CtaOrdenante;
                                c.fechaPago = pa.FechaPago;
                                c.monedaP = pa.MonedaP;
                                c.formaDePagoP = pa.FormaDePagoP;
                                c.monto = pa.Monto;
                                c.nomBancoOrd = pa.NomBancoOrdExt;
                                c.numOperacion = pa.NumOperacion;
                                c.rfcEmisorCtaBen = pa.RfcEmisorCtaBen;
                                c.rfcEmisorCtaOrd = pa.RfcEmisorCtaOrd;
                                c.tipoCadPago = pa.TipoCadPago;
                                c.tipoCambioP = pa.TipoCambioP;
                                Comple.Add(c);
                            }
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ComplementoPago", Comple ));
                            //---------
                            List<PagosPagoDoctoRelacionado> P = new List<PagosPagoDoctoRelacionado>();
                            foreach (var pa in comprobante.Complemento.Pag.Pago)
                            {
                                if (pa.DoctoRelacionado != null)
                                    if (pa.DoctoRelacionado.Count() > 0)
                                    {
                                        foreach (var d in pa.DoctoRelacionado)
                                        {
                                            PagosPagoDoctoRelacionado p = new PagosPagoDoctoRelacionado();
                                            p.Folio = d.Folio;
                                            p.IdDocumento = d.IdDocumento;
                                            if (d.ImpPagado != null)
                                            {
                                                p.ImpPagado = d.ImpPagado;
                                                p.ImpPagadoSpecified = true;
                                            }
                                            else
                                                p.ImpPagadoSpecified = false;
                                            if (d.ImpSaldoAnt != null)
                                            {
                                                p.ImpSaldoAntSpecified = true;
                                                p.ImpSaldoAnt = d.ImpSaldoAnt;
                                            }
                                            else
                                                p.ImpSaldoAntSpecified = false;
                                            if (d.ImpSaldoInsoluto != null)
                                            {
                                                p.ImpSaldoInsolutoSpecified = true;
                                                p.ImpSaldoInsoluto = d.ImpSaldoInsoluto;
                                            }
                                            else
                                                p.ImpSaldoInsolutoSpecified = false;
                                            p.MetodoDePagoDR = d.MetodoDePagoDR;
                                            p.MonedaDR = d.MonedaDR;
                                            p.NumParcialidad = d.NumParcialidad;
                                            p.Serie = d.Serie;
                                            if (d.ImpSaldoAnt != null)
                                            {
                                                p.TipoCambioDRSpecified = true;
                                                p.TipoCambioDR = d.TipoCambioDR;
                                            }
                                            else
                                                p.TipoCambioDRSpecified = false;
                                            P.Add(p);

                                        }
                                    }

                            }
                            if (P != null && P.Count > 0)
                                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DoctoRelacionado",  P ));
                            else

                                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DoctoRelacionado", new object[] { null }));

                            //------------------------------
                        }
                        else
                        {
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DoctoRelacionado", new object[] { null }));
                
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ComplementoPago", new object[] { null }));
                        }
                    }
                    else
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DoctoRelacionado", new object[] { null }));
                
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ComplementoPago", new object[] { null }));

                    }
                    //-------------------------
                    if (comprobante.Impuestos != null)
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Impuestos",new[] { comprobante.Impuestos}));

                        if (comprobante.Impuestos.Traslados != null)
                        {
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Traslados", comprobante.Impuestos.Traslados));
                        }
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Traslados", new object[] { null }));
                  
                        if (comprobante.Impuestos.Retenciones != null)
                        {
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Retenciones", comprobante.Impuestos.Retenciones));
                        }
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Retenciones", new object[] { null }));

                    }
                    else
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Impuestos", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Traslados", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Retenciones", new object[] { null }));

                    }
                    if(comprobante.Receptor!=null)
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Receptor", new[] { comprobante.Receptor }));
                    else
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Receptor", new object[] { null }));

                    if (comprobante.Emisor != null)
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Emisor", new[] { comprobante.Emisor }));
                    else
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Emisor", new object[] { null }));
         
                    if (comprobante.DatosAdicionales != null)
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DatosAdicionales", new[] { comprobante.DatosAdicionales }));
                    }
                    else
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DatosAdicionales", new object[] { null }));
                    }




                    if (comprobante.DbfDatosVenta != null)
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DbfDatosVenta", new[] { comprobante.DbfDatosVenta }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DbfDatosCliente", new[] { comprobante.DbfDatosCliente }));
                    }
                    else
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DbfDatosVenta", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DbfDatosCliente", new object[] { null }));
                    }
                    //***********************************************
                    if (comprobante.Complemento.comercioExterior != null)
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ComercioExterior", new[] { comprobante.Complemento.comercioExterior }));
                        if (comprobante.Complemento.comercioExterior.Destinatario != null)
                        {
                            //  reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEDestinatario", comprobante.Complemento.comercioExterior.Destinatario));
                            List<ComercioExteriorDestinatarioDatos> D = new List<ComercioExteriorDestinatarioDatos>();
                            foreach (var de in comprobante.Complemento.comercioExterior.Destinatario)
                            {
                                if (de.Domicilio == null)
                                {
                                    ComercioExteriorDestinatarioDatos d = new ComercioExteriorDestinatarioDatos();
                                    d.Nombre = de.Nombre;
                                    d.NumRegIdTrib = de.NumRegIdTrib;
                                    D.Add(d);
                                }
                                else
                                {
                                    foreach (var dom in de.Domicilio)
                                    {
                                        ComercioExteriorDestinatarioDatos d2 = new ComercioExteriorDestinatarioDatos();

                                        d2.Nombre = de.Nombre;
                                        d2.NumRegIdTrib = de.NumRegIdTrib;
                                        d2.Calle = dom.Calle;
                                        d2.CodigoPostal = dom.CodigoPostal;
                                        d2.Colonia = dom.Colonia;
                                        d2.Estado = dom.Estado;
                                        d2.Localidad = dom.Localidad;
                                        d2.Municipio = dom.Municipio;
                                        d2.NumeroExterior = dom.NumeroExterior;
                                        d2.NumeroInterior = dom.NumeroInterior;
                                        d2.Pais = dom.Pais;
                                        d2.Referencia = dom.Referencia;
                                        D.Add(d2);
                                    }
                                }
                            }
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEDestinatario", D));

                        }
                        else
                        {
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEDestinatario", new object[] { null }));
                        }
                        //--------------------------------------------
                        if (comprobante.Complemento.comercioExterior.Emisor != null)
                        {
                            ComercioExteriorEmisorDatos F = new ComercioExteriorEmisorDatos();
                            F.Curp = comprobante.Complemento.comercioExterior.Emisor.Curp;
                            if (comprobante.Complemento.comercioExterior.Emisor.Domicilio != null)
                                F.Calle = comprobante.Complemento.comercioExterior.Emisor.Domicilio.Calle;
                                F.CodigoPostal = comprobante.Complemento.comercioExterior.Emisor.Domicilio.CodigoPostal;
                                F.Colonia = comprobante.Complemento.comercioExterior.Emisor.Domicilio.Colonia;
                            F.Estado = comprobante.Complemento.comercioExterior.Emisor.Domicilio.Estado;
                            F.Localidad = comprobante.Complemento.comercioExterior.Emisor.Domicilio.Localidad;
                            F.Municipio = comprobante.Complemento.comercioExterior.Emisor.Domicilio.Municipio;
                            F.NumeroExterior = comprobante.Complemento.comercioExterior.Emisor.Domicilio.NumeroExterior;
                            F.NumeroInterior = comprobante.Complemento.comercioExterior.Emisor.Domicilio.NumeroInterior;
                            F.Pais = comprobante.Complemento.comercioExterior.Emisor.Domicilio.Pais;
                            F.Referencia = comprobante.Complemento.comercioExterior.Emisor.Domicilio.Referencia;
                   
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEEmisor", new[] { F }));
                     
                        }
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEEmisor", new object[] { null }));
                        //----------------------------------------------------------
                        if (comprobante.Complemento.comercioExterior.Propietario != null)
                        {
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEPropietario", comprobante.Complemento.comercioExterior.Propietario));

                        }else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEPropietario", new object[] { null }));
                        //--------------------------------------------------------------
                        if (comprobante.Complemento.comercioExterior.Receptor != null)
                        {
                            ComercioExteriorReceptorDatos F = new ComercioExteriorReceptorDatos();
                            F.NumRegIdTrib = comprobante.Complemento.comercioExterior.Receptor.NumRegIdTrib;
                            if (comprobante.Complemento.comercioExterior.Receptor.Domicilio != null)
                            {
                                F.Calle = comprobante.Complemento.comercioExterior.Receptor.Domicilio.Calle;
                                F.CodigoPostal = comprobante.Complemento.comercioExterior.Receptor.Domicilio.CodigoPostal;
                                F.Colonia = comprobante.Complemento.comercioExterior.Receptor.Domicilio.Colonia;
                                F.Estado = comprobante.Complemento.comercioExterior.Receptor.Domicilio.Estado;
                                F.Localidad = comprobante.Complemento.comercioExterior.Receptor.Domicilio.Localidad;
                                F.Municipio = comprobante.Complemento.comercioExterior.Receptor.Domicilio.Municipio;
                                F.NumeroExterior = comprobante.Complemento.comercioExterior.Receptor.Domicilio.NumeroExterior;
                                F.NumeroInterior = comprobante.Complemento.comercioExterior.Receptor.Domicilio.NumeroInterior;
                                F.Pais = comprobante.Complemento.comercioExterior.Receptor.Domicilio.Pais;
                                F.Referencia = comprobante.Complemento.comercioExterior.Receptor.Domicilio.Referencia;
                            }
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEReceptor", new[] { F }));

                        }
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEReceptor", new object[] { null }));
                        //----------------------------------------------------------
                        if (comprobante.Complemento.comercioExterior.Mercancias != null)
                        {
                            //  reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEDestinatario", comprobante.Complemento.comercioExterior.Destinatario));
                            List<ComercioExteriorMercanciaDatos> D = new List<ComercioExteriorMercanciaDatos>();
                            foreach (var de in comprobante.Complemento.comercioExterior.Mercancias)
                            {
                                if (de.DescripcionesEspecificas == null)
                                {
                                    ComercioExteriorMercanciaDatos d = new ComercioExteriorMercanciaDatos();
                                    d.NoIdentificacion=de.NoIdentificacion;
                                    d.FraccionArancelaria=de.FraccionArancelaria;
                                    if(de.CantidadAduanaSpecified==true)
                                    d.CantidadAduana=de.CantidadAduana.ToString();
                                    d.UnidadAduana =de.UnidadAduana;
                                    if(de.ValorUnitarioAduanaSpecified==true)
                                    d.ValorUnitarioAduana=de.ValorUnitarioAduana.ToString();
                                    d.ValorDolares = de.ValorDolares.ToString();
                                    D.Add(d);
                                }
                                else
                                {
                                    foreach (var dom in de.DescripcionesEspecificas)
                                    {
                                        ComercioExteriorMercanciaDatos d2 = new ComercioExteriorMercanciaDatos();

                                        d2.NoIdentificacion = de.NoIdentificacion;
                                        d2.FraccionArancelaria = de.FraccionArancelaria;
                                        if (de.CantidadAduanaSpecified == true)
                                            d2.CantidadAduana = de.CantidadAduana.ToString();
                                        d2.UnidadAduana = de.UnidadAduana;
                                        if (de.ValorUnitarioAduanaSpecified == true)
                                            d2.ValorUnitarioAduana = de.ValorUnitarioAduana.ToString();
                                        d2.ValorDolares = de.ValorDolares.ToString();
                                        
                                        d2.Marca = dom.Marca;
                                        d2.Modelo = dom.Modelo;
                                        d2.NumeroSerie = dom.NumeroSerie;
                                        d2.SubModelo = dom.SubModelo;
                                        
                                        D.Add(d2);
                                    }
                                }
                            }
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEMercancia", D.ToArray()));
                           
                        }
                        else
                        {
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEMercancia", new object[] { null }));
                        }
                        


                    }
                    else
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ComercioExterior", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEDestinatario", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEEmisor", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEPropietario", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEReceptor", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CEMercancia", new object[] { null }));
                      
                    }
                    //-------------------------------------------------------------
                    if (comprobante.Complemento.ine != null)
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ine", new[] { comprobante.Complemento.ine }));
                        if (comprobante.Complemento.ine.Entidad != null)
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("IneEntidad", comprobante.Complemento.ine.Entidad));
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("IneEntidad", new object[] { null }));
                        List<INEEntidadContabilidad> EntidadContabilidad = new List<INEEntidadContabilidad>();
                         if (comprobante.Complemento.ine.Entidad != null)
                        {
                            foreach (var ent in comprobante.Complemento.ine.Entidad)
                            {
                                if (ent.Contabilidad != null && ent.Contabilidad.Count() > 0)
                                    foreach (var cont in ent.Contabilidad)
                                    {
                                        if (cont.IdContabilidad != null)
                                        {
                                            INEEntidadContabilidad I = new INEEntidadContabilidad();
                                            I.IdContabilidad = cont.IdContabilidad;
                                            EntidadContabilidad.Add(I);
                                        }
                                    }
                            }
                        }
                         if (EntidadContabilidad != null && EntidadContabilidad.Count()>0)
                             reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("EntidadContabilidad", EntidadContabilidad));
                        else
                             reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("EntidadContabilidad", new object[] { null }));

              

                    }
                    else
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Ine", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("IneEntidad", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("EntidadContabilidad", new object[] { null }));

                    }

                    if (comprobante.Complemento.cartaPorte != null)
                    {
                        var C = LLenadoCartaPorteCP(comprobante.Complemento.cartaPorte);

                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CARTAPORTE", new[] { C }));
                        var D= LLenadoUbicacion(comprobante.Complemento.cartaPorte.Ubicaciones);
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("UBICACIONESCP", D.ToArray()));
                        var M = LLenadoMercancias(comprobante.Complemento.cartaPorte.Mercancias.Mercancia,comprobante.Complemento.cartaPorte.Mercancias);
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("MERCANCIACP", M.ToArray()));
                        if (comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal != null)
                        {
                            var T = LLenadoAutotransporteFederalCP(comprobante.Complemento.cartaPorte.Mercancias.AutotransporteFederal);
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATFCP", new[] { T}));
                        }
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATFCP", new object[] { null }));

                        if (comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo != null)
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATMCP", new[] { comprobante.Complemento.cartaPorte.Mercancias.TransporteMaritimo }));
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATMCP", new object[] { null }));

                        if (comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo != null)
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATACP", new[] { comprobante.Complemento.cartaPorte.Mercancias.TransporteAereo }));
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATACP", new object[] { null }));

                        if (comprobante.Complemento.cartaPorte.Mercancias.TransporteFerroviario != null)
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATFRCP", new[] { comprobante.Complemento.cartaPorte.Mercancias.TransporteFerroviario }));
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATFRCP", new object[] { null }));
                        if (comprobante.Complemento.cartaPorte.FiguraTransporte != null)
                        {
                            
                            if (comprobante.Complemento.cartaPorte.FiguraTransporte.Operadores != null)
                            {
                                var O = LLenadoOPeradorCP(comprobante.Complemento.cartaPorte.FiguraTransporte.Operadores);
                                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("OPERADORCP", O.ToArray()));
                            }
                            else
                               reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("OPERADORCP", new object[] { null }));
                            if (comprobante.Complemento.cartaPorte.FiguraTransporte.Propietario != null)
                            {
                                var P = LLenadoPropietarioCP(comprobante.Complemento.cartaPorte.FiguraTransporte.Propietario);
                                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("PROPIETARIOCP", P.ToArray()));
                            }
                            else
                                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("PROPIETARIOCP", new object[] { null }));
                            if (comprobante.Complemento.cartaPorte.FiguraTransporte.Arrendatario != null)
                            {
                                var A = LLenadoArrendatarioCP(comprobante.Complemento.cartaPorte.FiguraTransporte.Arrendatario);
                                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ARRENDATARIOCP", A.ToArray()));
                            }
                            else
                                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ARRENDATARIOCP", new object[] { null }));
                            if (comprobante.Complemento.cartaPorte.FiguraTransporte.Notificado != null)
                            {
                                var N = LLenadoNotificadoCP(comprobante.Complemento.cartaPorte.FiguraTransporte.Notificado);
                                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("NOTIFICADOCP", N.ToArray()));
                            }
                            else
                                reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("NOTIFICADOCP", new object[] { null }));
                            
                        }
                        else
                        {
                               reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("OPERADORCP", new object[] { null }));
                               reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("PROPIETARIOCP", new object[] { null }));
                               reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ARRENDATARIOCP", new object[] { null }));
                               reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("NOTIFICADOCP", new object[] { null }));
                         
                        }
               
         
                    }
                    else
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("CARTAPORTE", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("UBICACIONESCP", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("MERCANCIACP", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATFCP", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATMCP", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATACP", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ATFRCP", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("OPERADORCP", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("PROPIETARIOCP", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ARRENDATARIOCP", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("NOTIFICADOCP", new object[] { null }));
                  
                    }



                    try
                    {
                        reportViewer1.RefreshReport();
                        byteViewer = reportViewer1.LocalReport.Render("PDF");
                        Logger.Debug(byteViewer.Length + " bytes");
                    }
                    catch (Exception ee)
                    {
                        Logger.Fatal(ee);
                        //Logger.Fatal("Terminando el servicio...");
                        //Environment.Exit(1);
                    }

                }

                return byteViewer;
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                return null;
            }

        }
        private ComplementoCP LLenadoCartaPorteCP(CartaPorte U)
        {
            ComplementoCP d = new ComplementoCP();
            d.entradaSalidaMerc= U.EntradaSalidaMerc;
            if(U.TotalDistRecSpecified==true)
            d.totalDistRec = U.TotalDistRec.ToString();
            d.transpInternac = U.TranspInternac;
            d.viaEntradaSalida = U.ViaEntradaSalida;
            return d;
        }


        private List<Notificado> LLenadoNotificadoCP(CartaPorteFiguraTransporteNotificado[] U)
        {
            List<Notificado> D = new List<Notificado>();
            foreach (var de in U)
            {
                Notificado d = new Notificado();
                if (de.Domicilio != null)
                {
                    d.codigoPostal = de.Domicilio.CodigoPostal;
                    d.estado = de.Domicilio.Estado;
                    d.pais = de.Domicilio.Pais;
                }
                d.nombreNotificado = de.NombreNotificado;
                d.numRegIdTribNotificado = de.NumRegIdTribNotificado;
                d.residenciaFiscalNotificado = de.ResidenciaFiscalNotificado;
                d.rFCNotificado = de.RFCNotificado;
                D.Add(d);
            }
            return D;
        }
        private List<Arrendatario> LLenadoArrendatarioCP(CartaPorteFiguraTransporteArrendatario[] U)
        {
            List<Arrendatario> D = new List<Arrendatario>();
            foreach (var de in U)
            {
                Arrendatario d = new Arrendatario();
                if (de.Domicilio != null)
                {
                    d.codigoPostal = de.Domicilio.CodigoPostal;
                    d.estado = de.Domicilio.Estado;
                    d.pais = de.Domicilio.Pais;
                }
                d.nombreArrendatario = de.NombreArrendatario;
                d.numRegIdTribArrendatario = de.NumRegIdTribArrendatario;
                d.residenciaFiscalArrendatario = de.ResidenciaFiscalArrendatario;
                d.rFCArrendatario = de.RFCArrendatario;
                D.Add(d);
            }
            return D;
        }
        private List<Propietario> LLenadoPropietarioCP(CartaPorteFiguraTransportePropietario[] U)
        {
            List<Propietario> D = new List<Propietario>();
            foreach (var de in U)
            {
                Propietario d = new Propietario();
                if (de.Domicilio != null)
                {
                    d.codigoPostal = de.Domicilio.CodigoPostal;
                    d.estado = de.Domicilio.Estado;
                    d.pais = de.Domicilio.Pais;
                }
                d.nombrePropietario = de.NombrePropietario;
                d.numRegIdTribPropietario = de.NumRegIdTribPropietario;
                d.residenciaFiscalPropietario = de.ResidenciaFiscalPropietario;
                d.rFCPropietario = de.RFCPropietario;
                D.Add(d);
            }
            return D;
        }

        private List<Operador> LLenadoOPeradorCP(CartaPorteFiguraTransporteOperadores[] U)
        {
            List<Operador> D = new List<Operador>();
             foreach (var de in U[0].Operador)
            {
                 Operador d = new Operador();
                 if (de.Domicilio != null)
                 {
                     d.codigoPostal = de.Domicilio.CodigoPostal;
                     d.estado = de.Domicilio.Estado;
                     d.pais = de.Domicilio.Pais;
                 }
                 d.nombreOperador = de.NombreOperador;
                 d.numLicencia = de.NumLicencia;
                 d.numRegIdTribOperador = de.NumRegIdTribOperador;
                 d.residenciaFiscalOperador = de.ResidenciaFiscalOperador;
                 d.rFCOperador = de.RFCOperador;
                 D.Add(d);
            }
            return D;
        }
        private AutotransporteFederalCP LLenadoAutotransporteFederalCP(CartaPorteMercanciasAutotransporteFederal U)
        {
            AutotransporteFederalCP d = new AutotransporteFederalCP();
            if (U.IdentificacionVehicular != null)
            {
                d.anioModeloVM = U.IdentificacionVehicular.AnioModeloVM.ToString();
                d.configVehicular = U.IdentificacionVehicular.ConfigVehicular;
                d.placaVM = U.IdentificacionVehicular.PlacaVM;

            }
            d.nombreAseg = U.NombreAseg;
            d.numPermisoSCT = U.NumPermisoSCT;
            d.numPolizaSeguro = U.NumPolizaSeguro;
            d.permSCT = U.PermSCT;
            
            return d;
        }
        private List<MercanciaCP> LLenadoMercancias(CartaPorteMercanciasMercancia[] U,CartaPorteMercancias U2)
        {
            List<MercanciaCP> D = new List<MercanciaCP>();
         foreach (var de in U)
         {
             MercanciaCP d = new MercanciaCP();
             if(U2.PesoBrutoTotalSpecified==true)
             d.pesoBrutoTotal=U2.PesoBrutoTotal.ToString();
             if(U2.UnidadPesoSpecified==true)
             d.unidadPeso=U2.UnidadPeso;
             if(U2.PesoNetoTotalSpecified==true)
             d.pesoNetoTotal=U2.PesoNetoTotal.ToString();
             if(U2.NumTotalMercancias!=null)
             d.numTotalMercancias=U2.NumTotalMercancias.ToString();
             //if(U2.CargoPorTasacionSpecified==true)
             //d.cargoPorTasacion=U2.CargoPorTasacion.ToString();
             //------------
             d.bienesTransp = de.BienesTransp;
             d.claveSTCC = de.ClaveSTCC;
             d.claveUnidad = de.ClaveUnidad;
             d.fraccionArancelaria=de.FraccionArancelaria;
             d.materialPeligroso = de.MaterialPeligroso;
             d.moneda = de.Moneda;
             d.pesoEnKg = de.PesoEnKg.ToString();
             if(de.ValorMercanciaSpecified==true)
             d.valorMercancia = de.ValorMercancia.ToString();

             D.Add(d);
         }
         return D;
        }

        private List<UbicacionCP> LLenadoUbicacion(CartaPorteUbicacion[] U)
        {
            List<UbicacionCP> D = new List<UbicacionCP>();
            foreach (var de in U)
            {
                UbicacionCP d = new UbicacionCP();
                if (de.DistanciaRecorridaSpecified == true)
                    d.distanciaRecorrida = de.DistanciaRecorrida.ToString();
                d.tipoEstacion = de.TipoEstacion;
                if (de.Origen != null)
                {
                    d.iDOrigen = de.Origen.IDOrigen;
                    d.fechaHoraSalida = de.Origen.FechaHoraSalida;
                  //  d.navegacionTraficoOrigen = de.Origen.NavegacionTrafico;
                  //  d.nombreEstacionOrigen = de.Origen.NombreEstacion;
                  //  d.nombreRemitente = de.Origen.NombreRemitente;
                    d.numEstacionOrigen = de.Origen.NumEstacion;
                  //  d.numRegIdTribOrigen = de.Origen.NumRegIdTrib;
                  //  d.residenciaFiscalOrigen = de.Origen.ResidenciaFiscal;
                    d.rFCRemitente = de.Origen.RFCRemitente;
                }
                if (de.Destino != null)
                {
                    d.iDDestino = de.Destino.IDDestino;
                    d.rFCDestinatario = de.Destino.RFCDestinatario;
                   // d.nombreDestinatario = de.Destino.NombreDestinatario;
                   // d.numRegIdTribDestino = de.Destino.NumRegIdTrib;
                   // d.residenciaFiscalDestino = de.Destino.ResidenciaFiscal;
                    d.numEstacionDestino = de.Destino.NumEstacion;
                   // d.nombreEstacionDestino = de.Destino.NombreEstacion;
                   // d.navegacionTraficoDestino = de.Destino.NavegacionTrafico;
                    d.fechaHoraProgLlegada = de.Destino.FechaHoraProgLlegada;
                }
                if (de.Domicilio != null)
                {
                   // d.calle = de.Domicilio.Calle;
                   // d.numeroExterior = de.Domicilio.NumeroExterior;
                   // d.numeroInterior = de.Domicilio.NumeroInterior;
                   // d.colonia = de.Domicilio.Colonia;
                   // d.localidad = de.Domicilio.Localidad;
                   // d.referencia = de.Domicilio.Referencia;
                   // d.municipio = de.Domicilio.Municipio;
                    d.estado = de.Domicilio.Estado;
                    d.pais = de.Domicilio.Pais;
                    d.codigoPostal = de.Domicilio.CodigoPostal;
                }
                D.Add(d);
            }

            return D;
        }

        public Byte[] GetPdfFromRetencion(Retenciones comprobante, string logoEmpresa)
        {
            try
            {
                Logger.Debug("Generando PDF del comprobante");
                byte[] byteViewer = new byte[0];
                using (ReportViewer reportViewer1 = new ReportViewer())
                {
                    //LocalReport objRDLC = new LocalReport();
                    string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", comprobante.Emisor.RFCEmisor, "Retenciones.rdlc");
     
                   // string ruta = GetRutaPdfCustomizado(comprobante);
                    if (!File.Exists(ruta))
                    {
                        ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "Retenciones.rdlc");
                    }
                    //objRDLC.DataSources.Clear();
                    reportViewer1.LocalReport.EnableHyperlinks = true;
                    reportViewer1.LocalReport.ReportPath = ruta;

                    ReportDataSource ds = new ReportDataSource("retencion");
                    ds.Value = new[] { comprobante }.ToList();
                    string enteros;
                    string decimales;
                    string totalLetra = comprobante.Totales.montoTotRet.ToString();
                    if (totalLetra.IndexOf('.') == -1)
                    {
                        enteros = "0";
                        decimales = "0";
                    }
                    else
                    {
                        enteros = totalLetra.Substring(0, totalLetra.IndexOf('.'));
                        decimales = totalLetra.Substring(totalLetra.IndexOf('.') + 1);
                    }
                    string rfcRec = "";
                    if (comprobante.Receptor.Nacionalidad == RetencionesReceptorNacionalidad.Nacional)
                        rfcRec = ((RetencionesReceptorNacional)comprobante.Receptor.Item).RFCRecep;
                    else rfcRec = ((RetencionesReceptorExtranjero)comprobante.Receptor.Item).NumRegIdTrib;

                    string total = enteros + "." + decimales;
                    int tam_var = comprobante.Sello.Length;
                    string Var_Sub = comprobante.Sello.Substring((tam_var - 8), 8);
                    string URL = @"https://prodretencionverificacion.clouda.sat.gob.mx/";

                    string cadenaCodigo = URL + "?" + "&id=" + comprobante.Complemento.timbreFiscalDigital.UUID.ToUpper() + "&re=" + comprobante.Emisor.RFCEmisor + "&rr=" + rfcRec + "&tt=" + total + "&fe=" + Var_Sub;
                        
               
                    string rutaTmp = Path.Combine(ConfigurationManager.AppSettings["TmpFiles"], comprobante.Complemento.timbreFiscalDigital.UUID + ".png");
                    //string logoEmpresa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", comprobante.Emisor.rfc, "Logo.png");
                    if (string.IsNullOrEmpty(logoEmpresa) || !File.Exists(logoEmpresa))
                    {
                        logoEmpresa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "LogoGenerico.png");
                    }
                    string rutaxx = ConfigurationManager.AppSettings["TmpFiles"];
                    if (!Directory.Exists(rutaxx))
                        Directory.CreateDirectory(rutaxx);



                    GeneraCodigoBidimensional(cadenaCodigo, rutaTmp);
                    //DataSet ds = new DataSet("DataSet1");
                    List<ReportParameter> Parametros = new List<ReportParameter>();
                    Parametros.Add(new ReportParameter("CodigoQR", "file://" + rutaTmp));
                    Parametros.Add(new ReportParameter("LogoEmpresa", "file://" + logoEmpresa));
                    reportViewer1.LocalReport.EnableExternalImages = true;
                    reportViewer1.LocalReport.DataSources.Add(ds);
                    reportViewer1.LocalReport.SetParameters(Parametros);
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("PeriodoRetencion",new[] { comprobante.Periodo}));

                    // comprobante.Complemento.timbreFiscalDigital.FechaTimbrado
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Timbre", new[] { comprobante.Complemento.timbreFiscalDigital }));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("TotalesRetencion", new[] { comprobante.Totales }));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ReceptorRetencion", new[] { comprobante.Receptor }));
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("EmisorRetencion", new[] { comprobante.Emisor }));
             
                    if(comprobante.Totales.ImpRetenidos!=null)
                    reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ImpuestosRetencion", comprobante.Totales.ImpRetenidos));
                     
                    else
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("ImpuestosRetencion", new object[] { null }));
                    
                    if (comprobante.Receptor.Nacionalidad == RetencionesReceptorNacionalidad.Nacional)
                    {
                        RetencionesReceptorNacional na = ((RetencionesReceptorNacional)comprobante.Receptor.Item);
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DsReceptorNacional", new[] { na }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DsReceptorExtranjero", new object[] { null }));

                    }
                    else
                    {
                        RetencionesReceptorExtranjero na = ((RetencionesReceptorExtranjero)comprobante.Receptor.Item);
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DsReceptorExtranjero", new[] { na }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DsReceptorNacional", new object[] { null }));

                    }

                    
                    //----------------

                    if (comprobante.Complemento.dividendos != null)
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Dividendos", new[] { comprobante.Complemento.dividendos }));
                        if (comprobante.Complemento.dividendos.DividOUtil != null)
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DividUtil",new[] {  comprobante.Complemento.dividendos.DividOUtil}));
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DividUtil", new object[] { null }));

                        if (comprobante.Complemento.dividendos.Remanente != null)
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Remanente",new[] {  comprobante.Complemento.dividendos.Remanente}));
                        else
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Remanente", new object[] { null }));


                    }
                    else
                    {
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Dividendos", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DividUtil", new object[] { null }));
                        reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("Remanente", new object[] { null }));

                    }

                  
                 
                    try
                    {
                        reportViewer1.RefreshReport();
                        byteViewer = reportViewer1.LocalReport.Render("PDF");
                        Logger.Debug(byteViewer.Length + " bytes");
                    }
                    catch (Exception ee)
                    {
                        Logger.Fatal(ee);
                        //Logger.Fatal("Terminando el servicio...");
                        //Environment.Exit(1);
                    }

                }

                return byteViewer;
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                return null;
            }

        }

      

        private string GetRutaPdf(Comprobante comprobante)
        {
            var ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "CFDI3.3.rdlc");
            //var ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "pdf.rdlc");
           
            return ruta;
        }
        //-
        private string GetRutaPdfAcuseCancelacion()
        {
            var ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "AcuseCancelacion.rdlc");
            return ruta;
        }

        private string GetRutaPdfCustomizado(Comprobante comprobante)
        {
            string ruta = null;
            
            if (comprobante.DbfDatosVenta != null)
            {
                ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "Ret_FTP_Canadian.rdlc");
            }
            else
            {
                //TODO
                ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", comprobante.Emisor.Rfc, "CFDI3.3.rdlc");
                //ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "ASR040203J74", "Pdf.rdlc");
            }

            return ruta;
        }

        public static Retenciones GetTimbreRetencion(string xmlContent, Retenciones comprobante)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Retenciones));
            StringReader sr = new StringReader(xmlContent);
            object obj = ser.Deserialize(sr);
            var c = obj as Comprobante;
            if (c != null && c.Complemento != null && c.Complemento.Any.Count() > 0)
            {
                var d = ""; int i = 0;
                do
                {
                    d = c.Complemento.Any[i++].OuterXml;
                }
                while (!d.Contains("tfd:TimbreFiscalDigital"));


                XmlSerializer des = new XmlSerializer(typeof(TimbreRetenciones.TimbreFiscalDigital));
                TimbreRetenciones.TimbreFiscalDigital tim = (TimbreRetenciones.TimbreFiscalDigital)des.Deserialize(new XmlTextReader(new StringReader(d)));
                GeneradorCadenasTimbre gcad = new GeneradorCadenasTimbre();
                var cadenaTimbre = gcad.CadenaOriginal(d);
               // comprobante.Fecha = c.Fecha;
                comprobante.Sello = c.Sello;
                comprobante.Complemento = new RetencionesComplemento();
                comprobante.XmlString = xmlContent;
                comprobante.CadenaOriginalTimbre = cadenaTimbre;
                comprobante.Complemento.timbreFiscalDigital = tim;
              //  comprobante.NoCertificado = c.NoCertificado;
                return comprobante;
            }
            return null;
        }

        public static Comprobante GetTimbre(string xmlContent, Comprobante comprobante)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Comprobante));
            StringReader sr = new StringReader(xmlContent);
            object obj = ser.Deserialize(sr);
            var c = obj as Comprobante;
            if (c != null && c.Complemento != null && c.Complemento.Any.Count() > 0)
            {
                var d = ""; int i = 0;
                do
                {
                    d = c.Complemento.Any[i++].OuterXml;
                }
                while (!d.Contains("tfd:TimbreFiscalDigital"));


                XmlSerializer des = new XmlSerializer(typeof(TimbreFiscalDigital));
                TimbreFiscalDigital tim = (TimbreFiscalDigital)des.Deserialize(new XmlTextReader(new StringReader(d)));
                GeneradorCadenasTimbre gcad = new GeneradorCadenasTimbre();
                var cadenaTimbre = gcad.CadenaOriginal(d);
                comprobante.Fecha = c.Fecha;
                comprobante.Sello = c.Sello;
                comprobante.Complemento = new ComprobanteComplemento();
                comprobante.XmlString = xmlContent;
                comprobante.CadenaOriginalTimbre = cadenaTimbre;
                comprobante.Complemento.timbreFiscalDigital = tim;
                comprobante.NoCertificado = c.NoCertificado;
              //if (comprobante.Impuestos != null && comprobante.Impuestos.Traslados != null)
                //    comprobante.Traslados = comprobante.Impuestos.Traslados;
                return comprobante;
            }
            return null;
        }

        public static Comprobante GetComprobanteFromString(string xmlContent)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Comprobante));
            StringReader sr = new StringReader(xmlContent);
            object obj = ser.Deserialize(sr);
            var c = obj as Comprobante;

            if (c != null && c.Complemento != null && c.Complemento.Any.Count() > 0)
            {
                var d = c.Complemento.Any[0].OuterXml;
                XmlSerializer des = new XmlSerializer(typeof(TimbreFiscalDigital));
                TimbreFiscalDigital tim = (TimbreFiscalDigital)des.Deserialize(new XmlTextReader(new StringReader(d)));
                GeneradorCadenasTimbre gcad = new GeneradorCadenasTimbre();
                var cadenaTimbre = gcad.CadenaOriginal(xmlContent);
                c.CadenaOriginalTimbre = cadenaTimbre;
                c.Complemento.timbreFiscalDigital = tim;
            }
            return c;
        }

        public static string FirmarRetencion(string cadenaOriginal, string rutaLlave, string pass)
        {
            if (!File.Exists(rutaLlave))
            {
                throw new Exception("No se encontró el archivo: rutallave");
            }

            string formatoLlave = Path.GetExtension(rutaLlave).ToLower();
            byte[] llave = File.ReadAllBytes(rutaLlave);
            RSACryptoServiceProvider rsa = OpensslKey.DecodePrivateKey(llave, pass, formatoLlave);
            HashAlgorithm cryp = new SHA1CryptoServiceProvider();
            byte[] b = rsa.SignData(Encoding.UTF8.GetBytes(cadenaOriginal), cryp);
            Logger.Info(cadenaOriginal);
            byte[] hash = cryp.ComputeHash(Encoding.UTF8.GetBytes(cadenaOriginal));
            Logger.Info(BitConverter.ToString(hash).Replace("-", ""));
            Logger.Info(Convert.ToBase64String(b));
            return Convert.ToBase64String(b);
            /*
            
          byte[] llave = File.ReadAllBytes(rutaLlave);
          if (File.Exists(rutaLlave + ".pem"))
          {
              rutaLlave = rutaLlave + ".pem";
          }
          string ext = Path.GetExtension(rutaLlave);
          //if (string.IsNullOrEmpty(pass))
          //    pass = "12345678a";
          RSACryptoServiceProvider privateKey1 = OpensslKey.DecodePrivateKey(llave, pass, ext);
          UTF8Encoding e = new UTF8Encoding(true);
          byte[] signature = privateKey1.SignData(e.GetBytes(cadenaOriginal), "SHA256");
          string sello256 = Convert.ToBase64String(signature);

          return sello256;
          */


        }

        public static string Firmar(string cadenaOriginal, string rutaLlave, string pass)
        {
            
            
            byte[] llave = File.ReadAllBytes(rutaLlave);
            //RSACryptoServiceProvider rsa = OpensslKey.DecodeEncryptedPrivateKeyInfo(llave, pass);
            //if (rsa == null)
            //{
            //    throw new ApplicationException("Error al descifrar la llave privada");
            //}
            //HashAlgorithm cryp = new SHA1CryptoServiceProvider();
           // byte[] b = rsa.SignData(Encoding.UTF8.GetBytes(cadenaOriginal), cryp);
            //return Convert.ToBase64String(b);
            string ext = Path.GetExtension(rutaLlave);
            RSACryptoServiceProvider privateKey1 = OpensslKey.DecodePrivateKey(llave, pass, ext);
            if (privateKey1 == null)
            {
                throw new ApplicationException("Error al descifrar la llave privada");
            }
            UTF8Encoding e = new UTF8Encoding(true);
            byte[] signature = privateKey1.SignData(e.GetBytes(cadenaOriginal), "SHA256");
            string sello256 = Convert.ToBase64String(signature);

            return sello256;

        }

        private readonly XNamespace _nsRet = "http://www.sat.gob.mx/esquemas/retencionpago/1";
       
        private readonly XNamespace _ns = "http://www.sat.gob.mx/cfd/3";

        private string ConcatenaTimbreRef(XElement entrada, string xmlTimbre, string xmlDonat, string xmlAddenda)
        {
            XElement timbre = XElement.Load(new StringReader(xmlTimbre));
            var complemento = entrada.Elements(_ns + "Complemento").FirstOrDefault();
            if (complemento == null)
            {
                entrada.Add(new XElement(_ns + "Complemento"));
                complemento = entrada.Elements(_ns + "Complemento").FirstOrDefault();
            }
            complemento.Add(timbre);
            if (xmlDonat != null)
            {
                XElement donat = XElement.Load(new StringReader(xmlDonat));
                complemento.Add(donat);
            }

            if (xmlAddenda != null)
            {
                XElement add = XElement.Load(new StringReader(xmlAddenda));
                if (add.Name == "Addenda")
                {
                    entrada.Add(add);
                }
                else
                {
                    entrada.Add(new XElement(_ns + "Addenda"));
                    var addenda = entrada.Elements(_ns + "Addenda").FirstOrDefault();
                    addenda.Add(add);
                }
            }

            MemoryStream mem = new MemoryStream();
            StreamWriter tw = new StreamWriter(mem, Encoding.UTF8);
            //XmlWriter xmlWriter = XmlWriter.Create(tw,
            //                                     new XmlWriterSettings() {Indent = false, Encoding = Encoding.UTF8});
            entrada.Save(tw, SaveOptions.DisableFormatting);
            string xml = Encoding.UTF8.GetString(mem.GetBuffer());
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            //para tridonex
            xml = xml.Replace("AddendaTridonex xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:cfdi=\"http://tempuri.org/Tridonex.xsd\"", "Tridonex");
            xml = xml.Replace("AddendaTridonex", "Tridonex");
            if (xmlDonat != null)
            {
                xml = xml.Replace("xmlns:donat=\"http://www.sat.gob.mx/donat\"", "");
                xml = xml.Replace("xmlns:cfdi=\"http://www.sat.gob.mx/cfd/3\"", "xmlns:cfdi=\"http://www.sat.gob.mx/cfd/3\" xmlns:donat=\"http://www.sat.gob.mx/donat\"");
            
            }
            //para cotemar
            bool re;

            re = xml.Contains("<Cotemar xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar\">");
            if (re == true)
            {
                xml = xml.Replace("<NumProveedor>", "<cot:NumProveedor>");
                xml = xml.Replace("</NumProveedor>", "</cot:NumProveedor>");
                xml = xml.Replace("<NumPedido>", "<cot:NumPedido>");
                xml = xml.Replace("</NumPedido>", "</cot:NumPedido>");
                xml = xml.Replace("<NumEntMercancia>", "<cot:NumEntMercancia>");
                xml = xml.Replace("</NumEntMercancia>", "</cot:NumEntMercancia>");
                xml = xml.Replace("<ContactoCompra>", "<cot:ContactoCompra>");
                xml = xml.Replace("</ContactoCompra>", "</cot:ContactoCompra>");
                xml = xml.Replace("<Moneda>", "<cot:Moneda>");
                xml = xml.Replace("</Moneda>", "</cot:Moneda>");
                xml = xml.Replace("<Cotemar xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar\">", "<cot:Cotemar>");
                xml = xml.Replace("</Cotemar>", "</cot:Cotemar>");
                xml = xml.Replace("<cfdi:Addenda>", "<cfdi:Addenda xmlns:cot=\"https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar\" xsi:schemaLocation=\"https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar/Addenda.xsd\">");

            }


            return xml;
        }

        private string ConcatenaTimbreRet(XElement entrada, string xmlTimbre, string xmlDonat, string xmlAddenda)
        {
            XElement timbre = XElement.Load(new StringReader(xmlTimbre));
            var complemento = entrada.Elements(_nsRet + "Complemento").FirstOrDefault();
            if (complemento == null)
            {
                entrada.Add(new XElement(_nsRet + "Complemento"));
                complemento = entrada.Elements(_nsRet + "Complemento").FirstOrDefault();
            }
            complemento.Add(timbre);
            if (xmlDonat != null)
            {
                XElement donat = XElement.Load(new StringReader(xmlDonat));
                complemento.Add(donat);
            }

            if (xmlAddenda != null)
            {
                XElement add = XElement.Load(new StringReader(xmlAddenda));
                if (add.Name == "Addenda")
                {
                    entrada.Add(add);
                }
                else
                {
                    entrada.Add(new XElement(_nsRet + "Addenda"));
                    var addenda = entrada.Elements(_nsRet + "Addenda").FirstOrDefault();
                    addenda.Add(add);
                }
            }

            MemoryStream mem = new MemoryStream();
            StreamWriter tw = new StreamWriter(mem, Encoding.UTF8);
            //XmlWriter xmlWriter = XmlWriter.Create(tw,
            //                                     new XmlWriterSettings() {Indent = false, Encoding = Encoding.UTF8});
            entrada.Save(tw, SaveOptions.DisableFormatting);
            string xml = Encoding.UTF8.GetString(mem.GetBuffer());
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));


            return xml;
        }
        

        private string ConcatenaTimbreOriginal(XElement entrada, string xmlTimbre, string xmlDonat, string xmlAddenda, bool addendaRepetida)
        {
            XElement timbre = XElement.Load(new StringReader(xmlTimbre));
            var complemento = entrada.Elements(_ns + "Complemento").FirstOrDefault();
            if (complemento == null)
            {
                entrada.Add(new XElement(_ns + "Complemento"));
                complemento = entrada.Elements(_ns + "Complemento").FirstOrDefault();
            }
            complemento.Add(timbre);
            if (xmlDonat != null)
            {
                XElement donat = XElement.Load(new StringReader(xmlDonat));
                complemento.Add(donat);
            }


            if (xmlAddenda != null)
            {
                XElement add = XElement.Load(new StringReader(xmlAddenda));
                if(addendaRepetida)
                {
                    entrada.Add(add);
                }
                else
                {
                    entrada.Add(new XElement(_ns + "Addenda"));
                    var addenda = entrada.Elements(_ns + "Addenda").FirstOrDefault();
                    addenda.Add(add);
                }
            }


            MemoryStream mem = new MemoryStream();
            StreamWriter tw = new StreamWriter(mem, Encoding.UTF8);
            //XmlWriter xmlWriter = XmlWriter.Create(tw,
            //                                     new XmlWriterSettings() {Indent = false, Encoding = Encoding.UTF8});
            entrada.Save(tw, SaveOptions.DisableFormatting);
            string xml = Encoding.UTF8.GetString(mem.GetBuffer());
            xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
            xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
            xml = xml.Replace("xmlns:donat=\"http://www.sat.gob.mx/donat\"", "");

            return xml;
        }


        public string GetXmlDonat(Donatarias donat)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Donatarias));
            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    var sw = new StreamWriter(memStream, Encoding.UTF8);
                    using (
                        XmlWriter xmlWriter = XmlWriter.Create(sw,
                                                               new XmlWriterSettings() { Indent = false, Encoding = Encoding.UTF8, OmitXmlDeclaration = true }))
                    {
                        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                        namespaces.Add("donat", "http://www.sat.gob.mx/donat");
                        ser.Serialize(xmlWriter, donat, namespaces);
                        string xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                        xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                        xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
                        //xml = xml.Replace("xmlns:donat=\"http://www.sat.gob.mx/donat\"", "");
                        return xml;
                    }
                }
            }
            catch (Exception ee)
            {

                Logger.Error(ee);
                return null;
            }

        }

        public static ArrayList getMacAddress()
        {
            // Contador para un ciclo
            int i = 0;
            // Colección de direcciones MAC
            ArrayList DireccionesMAC = new ArrayList();
            // Información de las tarjetas de red
            NetworkInterface[] interfaces = null;
            // Obtener todas las interfaces de red de la PC
            interfaces = NetworkInterface.GetAllNetworkInterfaces();
            // Validar la cantidad de tarjetas de red que tiene
            if (interfaces != null && interfaces.Length > 0)
            {
                // Recorrer todas las interfaces de red
                foreach (NetworkInterface adaptador in interfaces)
                {
                    // Obtener la dirección fisica
                    PhysicalAddress direccion = adaptador.GetPhysicalAddress();
                    // Obtener en modo de arreglo de bytes la dirección
                    byte[] bytes = direccion.GetAddressBytes();
                    // Variable que tendra la dirección visible
                    string mac_address = string.Empty;
                    // Recorrer todos los bytes de la direccion
                    for (i = 0; i < bytes.Length; i++)
                    {
                        // Pasar el byte a un formato legible para el usuario
                        mac_address += bytes[i].ToString("X2");
                        if (i != bytes.Length - 1)
                        {
                            // Agregar un separador, por formato
                            mac_address += "-";
                        }
                    }
                    // Agregar la direccion MAC a la lista
                    DireccionesMAC.Add(mac_address);
                }
            }
            // Valor de retorno, la lista de direcciones MAC
            return DireccionesMAC;
        }
         public string cifrar(string cadena)
        {
            byte[] llave; //Arreglo donde guardaremos la llave para el cifrado 3DES.
            byte[] arreglo = UTF8Encoding.UTF8.GetBytes(cadena); //Arreglo donde guardaremos la cadena descifrada.
            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();
            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateEncryptor(); // Iniciamos la conversión de la cadena
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length); //Arreglo de bytes donde guardaremos la cadena cifrada.
            tripledes.Clear();
            return Convert.ToBase64String(resultado, 0, resultado.Length); // Convertimos la cadena y la regresamos.
        }
       

        public TimbreRetenciones.TimbreFiscalDigital TimbrarRetenciones(Retenciones comp, out string cadenaTimbre,string usuario,string contraseña)
      
        {
            try
            {
                Logger.Debug("Timbrando comprobante");
                ClienteTimbradoNtlink cliente = new ClienteTimbradoNtlink();
                XmlSerializer ser = new XmlSerializer(typeof(TimbreRetenciones.TimbreFiscalDigital));
                var str = this.GetXmlRetenciones(comp);

                //---------------------------------
                var MAC = getMacAddress();
                String Mac =MAC[0].ToString();
                string Key="";
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + "KEY.txt"))
                {
                    var lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + "KEY.txt");
                    if (lines.Length > 0)
                    {
                        Key = lines[0];
                    }
                }
                String llave = cifrar(Mac + "|" + Key);
         
                //---------------------------------

                string timbreString = cliente.TimbraRetencion(str,usuario,contraseña,llave,"CON");
                TimbreRetenciones.TimbreFiscalDigital timbre = null;
                try
                {
                    timbre = (TimbreRetenciones.TimbreFiscalDigital)ser.Deserialize(new XmlTextReader(new StringReader(timbreString)));
                    cadenaTimbre = timbreString;
                }
                catch (Exception ee)
                {
                    Logger.Error(timbreString);
                    throw new FaultException(timbreString);
                }
                if (timbreString == null)
                {
                    throw new Exception("Ocurrió un error en el timbrado");
                }
                GeneradorCadenasTimbre generadorCadenasTimbre = new GeneradorCadenasTimbre();
                comp.CadenaOriginalTimbre = generadorCadenasTimbre.CadenaOriginal(cadenaTimbre);
                string cfdiString = str;// GetXml(comp);
                StringReader sr = new StringReader(cfdiString);
                XElement element = XElement.Load(sr);
                string dividendos = null;
                if (comp.Complemento != null && comp.Complemento.dividendos != null)
                {
 //                   dividendos = GetXmlDonat(comp.Complemento.dividendos);
                }

                string xmlFinal = ConcatenaTimbreRet(element, cadenaTimbre, null, null);
                if (comp.Complemento == null)
                {
                    comp.Complemento = new RetencionesComplemento() { timbreFiscalDigital = timbre };
                }
                else
                {
                    comp.Complemento.timbreFiscalDigital = timbre;
                }
                comp.SelloSAT = comp.Complemento.timbreFiscalDigital.selloSAT;
                comp.NoCertificadoSAT = comp.Complemento.timbreFiscalDigital.noCertificadoSAT;
               

                comp.XmlString = xmlFinal;
                return timbre;
            }
            catch (FaultException fe)
            {

                Logger.Info(fe);
                throw;
            }
            catch (SoapException exception)
            {
                Logger.Error(exception.Detail.InnerText.Trim());
                throw new ApplicationException("Error al timbrar el comprobante:" + exception.Detail.InnerText.Trim(), exception);
            }
            catch (Exception exception)
            {
                Logger.Error((exception.InnerException == null ? exception.Message : exception.InnerException.Message));
                throw new Exception("Error al timbrar el comprobante", exception);
            }
        }

        //--------------------------------------------------
       

        public TimbreFiscalDigital TimbrarComprobanteNtLinkRef(Comprobante comp, out string cadenaTimbre,string usuario,string contraseña)
        {
            try
            {
                Logger.Debug("Timbrando comprobante");
                ClienteTimbradoNtlink cliente = new ClienteTimbradoNtlink();
                XmlSerializer ser = new XmlSerializer(typeof(TimbreFiscalDigital));
                var str = this.GetXml(comp);

                //---------------------------------
                var MAC = getMacAddress();
                String Mac = MAC[0].ToString();
                string Key = "";
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + "KEY.txt"))
                {
                    var lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + "KEY.txt");
                    if (lines.Length > 0)
                    {
                        Key = lines[0];
                    }
                }
                String llave = cifrar(Mac + "|" + Key);

                //---------------------------------

                
                string timbreString = cliente.TimbraCfdi(str,usuario,contraseña,llave,"CON");
              //  string timbreString ="<tfd:TimbreFiscalDigital xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/sitio_internet/cfd/TimbreFiscalDigital/TimbreFiscalDigitalv11.xsd\" Version=\"1.1\" UUID=\"E81645A4-3E9E-4FB6-AF93-8A59D2B703E0\" FechaTimbrado=\"2017-10-21T13:22:45\" RfcProvCertif=\"DAL050601L35\" SelloCFD=\"X1VTQ8ykANzxsw9Ok9nw0ppDy/XIqE6/XzLPIhXjwJhsww7gQTBPWaKPOyX2ZT4y608nDCuJ+Gaow8pSLyRlha7BY9a814CC4QALhtl8v2+vxXbUDIMHDkquE5MzKvSNv3gfcaCPdFZlGQ+FNAsjJ9AGaeCVCn6uFXfypa5q/awvLZ0ppISU+xEZimOdfCaab3rVIp+gZ0PJ6/p+IcbKbp9T0/2PzfBz304o7mv4fyij0mP42/jGmv0SiEEvkBzEfEDNB3KDTU7qcxO19baF++AHLOSbqxALJWB3+LmQdifOjql03KlZOBpahmV8cmGwtDnN9K2YSae8Ce4ExtQ4Ig==\" NoCertificadoSAT=\"20001000000300022323\" SelloSAT=\"C4mh7Eun8EM8Z9CbzBTbhdVPUhWrbXrWTjIsdmEU0W9GHi2ryHXcD1WdbxRSNUs4EcKw72ql2Slwycy3e3kzd7Gu649JW1og8xySs65yNUNjfIuTp4oYnM/HNkt3oYz6g2XACwnjhKKHzLSH2Aj8cmHY36QLLZzAOuRbkugjDrWgBLGDZShjg1/43kLzM30GQPnNcVb0cPHbn/nAKb/5AOOujWRyEvtumzSXJqRdsXApPQlne4pz2LXhyQzHLvVVmO1TjIn0AW4U/1St+7KvgCiwYaq/+S36RH5ktfxSeGCBXJH02YxsZs1+9zGXmZetmmrRo0eiIdfI/UDBsVRmNQ==\" xmlns:tfd=\"http://www.sat.gob.mx/TimbreFiscalDigital\" />" ;
                //Logger.Debug(timbreString);
                TimbreFiscalDigital timbre = null;
                try
                {
                    timbre = (TimbreFiscalDigital)ser.Deserialize(new XmlTextReader(new StringReader(timbreString)));
                    cadenaTimbre = timbreString;
                }
                catch (Exception ee)
                {
                    Logger.Error(timbreString);
                    throw new FaultException(timbreString);
                }
                if (timbreString == null)
                {
                    throw new Exception("Ocurrió un error en el timbrado");
                }

                GeneradorCadenasTimbre generadorCadenasTimbre = new GeneradorCadenasTimbre();
                comp.CadenaOriginalTimbre = generadorCadenasTimbre.CadenaOriginal(cadenaTimbre);
                string cfdiString = GetXml(comp);
                StringReader sr = new StringReader(cfdiString);
                XElement element = XElement.Load(sr);
                string donat = null;
                if (comp.Complemento != null && comp.Complemento.Donat != null)
                {
                    donat = GetXmlDonat(comp.Complemento.Donat);
                }
                try //quitar el error cuando no lleve la addenda
                {
                    if (comp.XmlAdenda.Contains("AddendaTridonex"))//para tridonex agregar uuid
                        comp.XmlAdenda = comp.XmlAdenda.Replace("UUID=\"\"", "UUID=\"" + timbre.UUID + "\"");
                }
                catch (Exception) { }

                string xmlFinal = ConcatenaTimbreRef(element, cadenaTimbre, donat, comp.XmlAdenda);
                if (comp.Complemento == null)
                {
                    comp.Complemento = new ComprobanteComplemento() { timbreFiscalDigital = timbre };
                }
                else
                {
                    comp.Complemento.timbreFiscalDigital = timbre;                 
                }
                try //quitar el error cuando no lleve la addenda
                {
                    if (xmlFinal.Contains("AddendaJumex"))//para tridonex agregar uuid
                    {
                        xmlFinal = xmlFinal.Replace("<AddendaJumex>", "");
                        xmlFinal = xmlFinal.Replace("</AddendaJumex>", "");
                    }
                    if (xmlFinal.Contains("<DatosNadro>"))//para tridonex agregar uuid
                    {

                        xmlFinal = xmlFinal.Replace("<Adenda xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">", "");
                        xmlFinal = xmlFinal.Replace("</Adenda>", "");
                    }
                }
                catch (Exception) { }
                comp.XmlString = xmlFinal;
                return timbre;
            }
            catch (FaultException fe)
            {

                Logger.Info(fe);
                throw;
            }
            catch (SoapException exception)
            {
                Logger.Error(exception.Detail.InnerText.Trim());
                throw new ApplicationException("Error al timbrar el comprobante:" + exception.Detail.InnerText.Trim(), exception);
            }
            catch (Exception exception)
            {
                Logger.Error((exception.InnerException == null ? exception.Message : exception.InnerException.Message));
                throw new Exception("Error al timbrar el comprobante", exception);
            }
        }

        

        public void TimbrarComprobantePreview(Comprobante comp)
        {
            //   ClienteTimbradoNtlink cliente = new ClienteTimbradoNtlink();
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(TimbreFiscalDigital));
                var str = GetXml(comp);
            
             
                TimbreFiscalDigital timbre = null;
                try
                {
                    string fecha = DateTime.Now.ToString("s");
                    timbre = new TimbreFiscalDigital()
                                 {
                                     UUID = "No Timbrado",
                                     FechaTimbrado = fecha,
                                     NoCertificadoSAT = "000",
                                     SelloCFD = comp.Sello,
                                     SelloSAT = "Inválido",
                                     Version = "1.1"
                                 };

                }
                catch (Exception ee)
                {
                    Logger.Error(ee);
                }
                GeneradorCadenasTimbre generadorCadenasTimbre = new GeneradorCadenasTimbre();
                comp.CadenaOriginalTimbre = "Inválido";
                string cfdiString = GetXml(comp);
                StringReader sr = new StringReader(cfdiString);
                var sw = new StringWriter();
                XElement element = XElement.Load(sr);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = false;
                settings.OmitXmlDeclaration = false;
                XmlWriter xmlWriter = XmlWriter.Create(sw, settings);
                ser.Serialize(xmlWriter, timbre);
                string xmlFinal = ConcatenaTimbreOriginal(element, sw.ToString(), null, null,false);

                comp.Complemento = new ComprobanteComplemento() { timbreFiscalDigital = timbre };
                comp.XmlString = xmlFinal;

            }
            catch (FaultException fe)
            {
                throw;
            }
            catch (SoapException exception)
            {
                Logger.Error(exception.Detail.InnerText.Trim());
                throw new ApplicationException("Error al timbrar el comprobante:" + exception.Detail.InnerText.Trim(), exception);
            }
            catch (Exception exception)
            {
                Logger.Error((exception.InnerException == null ? exception.Message : exception.InnerException.Message));
                throw new Exception("Error al timbrar el comprobante", exception);
            }


        }

        public void GenerarCfdPreview(Comprobante comprobante, X509Certificate2 cert, string rutaLlave, string passLlave)
        {
            try
            {
                comprobante.Certificado = Convert.ToBase64String(cert.RawData);
                comprobante.NoCertificado = NoCert(cert.SerialNumber);
                GeneradorCadenas gen = new GeneradorCadenas();
                string comp = GetXml(comprobante);
                comprobante.CadenaOriginal = gen.CadenaOriginal(comp);
                comprobante.Sello = Firmar(comprobante.CadenaOriginal, rutaLlave, passLlave);
                TimbrarComprobantePreview(comprobante);
            }
            catch (FaultException fe)
            {
                Logger.Error(fe);
                throw;
            }
            catch (Exception exception)
            {
                Logger.Error((exception.InnerException == null ? exception.Message : exception.InnerException.Message));
                throw;
            }
        }


        private string GetXmlRetenciones(Retenciones p)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Retenciones));
            using (MemoryStream memStream = new MemoryStream())
            {
                var sw = new StreamWriter(memStream, Encoding.UTF8);
                using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings() { Indent = false, Encoding = Encoding.UTF8 }))
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

                    namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    namespaces.Add("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");


                    //if (p.Complemento.intereses != null)
                    //    namespaces.Add("intereses", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereses");
                    if (p.Complemento.dividendos != null)
                        namespaces.Add("dividendos", "http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos");
                    //if (p.Complemento.arrendamientoenfideicomiso != null)
                    //    namespaces.Add("arrendamientoenfideicomiso", "http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso");
                    //if (p.Complemento.enajenaciondeAcciones != null)
                    //    namespaces.Add("enajenaciondeAcciones", "http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones");

                    //if (p.Complemento.fideicomisonoempresarial != null)
                    //    namespaces.Add("fideicomisonoempresarial", "http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial");
                    //if (p.Complemento.intereseshipotecarios != null)
                    //    namespaces.Add("intereseshipotecarios", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios");
                    //if (p.Complemento.pagosaextranjeros != null)
                    //    namespaces.Add("pagosaextranjeros", "http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros");

                    //if (p.Complemento.operacionesconderivados != null)
                    //    namespaces.Add("operacionesconderivados", "http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados");
                    //if (p.Complemento.planesderetiro != null)
                    //    namespaces.Add("planesderetiro11", "http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro11");
                    //if (p.Complemento.premios != null)
                    //    namespaces.Add("premios", "http://www.sat.gob.mx/esquemas/retencionpago/1/premios");
                    //if (p.Complemento.sectorFinanciero != null)
                    //    namespaces.Add("sectorfinanciero", "http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero");



                    ser.Serialize(xmlWriter, p, namespaces);
                    string xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                    xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                    xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));


                    if (xml.Contains("retenciones:intereses "))//para intereses agregar 
                    {
                        xml = xml.Replace("retenciones:intereses Version=\"1.0\"", "intereses:Intereses Version=\"1.0\" xsi:schemaLocation=\" http://www.sat.gob.mx/esquemas/retencionpago/1/intereses http://www.sat.gob.mx/esquemas/retencionpago/1/intereses/intereses.xsd\" xmlns:intereses=\"http://www.sat.gob.mx/esquemas/retencionpago/1/intereses\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
                    }
                    if (xml.Contains("retenciones:dividendos"))//para dividendos agregar 
                    {
                        xml = xml.Replace("retenciones:dividendos", "dividendos:Dividendos");
                        xml = xml.Replace("dividendos:Dividendos Version=\"1.0\"", "dividendos:Dividendos Version=\"1.0\" xsi:schemaLocation=\" http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos/dividendos.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:dividendos=\"http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos\"");
                    }
                    if (xml.Contains("retenciones:arrendamientoenfideicomiso"))//para arrendamientosenfideicomiso agregar 
                    {
                        xml = xml.Replace("retenciones:arrendamientoenfideicomiso", "arrendamientoenfideicomiso:Arrendamientoenfideicomiso");
                        xml = xml.Replace("arrendamientoenfideicomiso:Arrendamientoenfideicomiso Version=\"1.0\"", "arrendamientoenfideicomiso:Arrendamientoenfideicomiso Version=\"1.0\" xsi:schemaLocation=\"http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso/arrendamientoenfideicomiso.xsd\" xmlns:arrendamientoenfideicomiso=\"http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
                    }
                    if (xml.Contains("retenciones:enajenaciondeAcciones"))//para arrendamientosenfideicomiso agregar 
                    {
                        xml = xml.Replace("retenciones:enajenaciondeAcciones", "enajenaciondeacciones:EnajenaciondeAcciones");
                        xml = xml.Replace("enajenaciondeacciones:EnajenaciondeAcciones Version=\"1.0\"", "enajenaciondeacciones:EnajenaciondeAcciones Version=\"1.0\" xsi:schemaLocation=\"http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones/enajenaciondeacciones.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:enajenaciondeacciones=\"http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones\"");
                    }
                    //-----se agrego el espacio de nombre asu clase
                    if (xml.Contains("retenciones:fideicomisonoempresarial"))//para fideicomisonoempresarial agregar 
                    {
                        xml = xml.Replace("retenciones:fideicomisonoempresarial", "fideicomisonoempresarial:Fideicomisonoempresarial");
                        xml = xml.Replace("<fideicomisonoempresarial:Fideicomisonoempresarial", "<fideicomisonoempresarial:Fideicomisonoempresarial xmlns:fideicomisonoempresarial=\"http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");

                    }
                    if (xml.Contains("retenciones:intereseshipotecarios"))//para intereseshipotecarios agregar 
                    {
                        xml = xml.Replace("retenciones:intereseshipotecarios", "intereseshipotecarios:Intereseshipotecarios");
                        xml = xml.Replace("<intereseshipotecarios:Intereseshipotecarios", "<intereseshipotecarios:Intereseshipotecarios xmlns:intereseshipotecarios=\"http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");

                    }
                    if (xml.Contains("retenciones:operacionesconderivados"))//para operacionesconderivados agregar 
                    {
                        xml = xml.Replace("retenciones:operacionesconderivados", "operacionesconderivados:Operacionesconderivados");
                        xml = xml.Replace("<operacionesconderivados:Operacionesconderivados", "<operacionesconderivados:Operacionesconderivados xmlns:operacionesconderivados=\"http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");

                    }
                    if (xml.Contains("retenciones:pagosaextranjeros"))//para pagosaextranjeros agregar 
                    {
                        xml = xml.Replace("retenciones:pagosaextranjeros", "pagosaextranjeros:Pagosaextranjeros");
                        xml = xml.Replace("<pagosaextranjeros:Pagosaextranjeros", "<pagosaextranjeros:Pagosaextranjeros xmlns:pagosaextranjeros=\"http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");

                    }
                    if (xml.Contains("retenciones:planesderetiro"))//para planesderetiro agregar 
                    {
                        xml = xml.Replace("retenciones:planesderetiro", "planesderetiro11:Planesderetiro");
                        xml = xml.Replace("<planesderetiro11:Planesderetiro", "<planesderetiro11:Planesderetiro xmlns:planesderetiro11=\"http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro11\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");

                    }
                    if (xml.Contains("retenciones:premios"))//para premios agregar 
                    {
                        xml = xml.Replace("retenciones:premios", "premios:Premios");
                        xml = xml.Replace("<premios:Premios", "<premios:Premios xmlns:premios=\"http://www.sat.gob.mx/esquemas/retencionpago/1/premios\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");

                    }
                    if (xml.Contains("retenciones:sectorFinanciero"))//para sectorfinanciero agregar 
                    {
                        xml = xml.Replace("retenciones:sectorFinanciero", "sectorfinanciero:SectorFinanciero");
                        xml = xml.Replace("<sectorfinanciero:SectorFinanciero", "<sectorfinanciero:SectorFinanciero xmlns:sectorfinanciero=\"http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");

                    }
                    return xml;
                }
            }
        }

        public TimbreRetenciones.TimbreFiscalDigital GenerarCfdRetenciones(Retenciones comprobante, X509Certificate2 cert, string rutaLlave, string passKey,string usuario,string contraseña)
        {
            try
            {
               

                Logger.Debug("Generando xml");
                comprobante.NumCert = NoCert(cert.SerialNumber);
                comprobante.Cert = Convert.ToBase64String(cert.RawData);
                var now = DateTime.Now;
                now = now.AddTicks(-(now.Ticks % TimeSpan.TicksPerSecond));
                comprobante.FechaExp = now;
                var gen = new GeneradorCadenasRetenciones();

                string comp = GetXmlRetenciones(comprobante);

                XElement xeComprobantexx = XElement.Parse(comp);
                SidetecStringWriter swxx = new SidetecStringWriter(Encoding.UTF8);
                xeComprobantexx.Save(swxx, SaveOptions.DisableFormatting);

                comprobante.CadenaOriginal = gen.CadenaOriginal(swxx.ToString());
                comprobante.Sello = FirmarRetencion(comprobante.CadenaOriginal, rutaLlave, passKey);
                string cadenaTimbre;
                var comprobanteTimbrado = TimbrarRetenciones(comprobante, out cadenaTimbre,usuario,contraseña);
                if (comprobanteTimbrado != null)
                {
                    Logger.Info(comprobanteTimbrado.UUID);
                    // comprobante.NoCertificadoSAT = comprobanteTimbrado.NoCertificadoSAT;
                    // comprobante.SelloSAT = comprobanteTimbrado.SelloSAT;
                    return comprobanteTimbrado;
                }
                else
                    return null;
            }
            catch (FaultException fe)
            {
                Logger.Error(fe);
                throw;
            }
            catch (Exception exception)
            {
                Logger.Error((exception.InnerException == null ? exception.Message : exception.InnerException.Message));
                throw;
            }
        }

        public void GenerarCfd(Comprobante comprobante, X509Certificate2 cert, string rutaLlave, string passLlave,string usuario,string contraseña)
        {
            try
            {
                Logger.Debug("Generando xml");
                comprobante.Certificado = Convert.ToBase64String(cert.RawData);
                comprobante.NoCertificado = NoCert(cert.SerialNumber);
                GeneradorCadenas gen = new GeneradorCadenas();


                string complemento = null;
                if (comprobante.Complemento != null)
                {
                    if (comprobante.Complemento.Nomina != null)// nuevo para nomina-----------------------------------------------------
                    {
                        comprobante.xsiSchemaLocation = comprobante.xsiSchemaLocation + " http://www.sat.gob.mx/nomina12 http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina12.xsd";
                        complemento = GetXmlAddenda(comprobante.Complemento.Nomina, typeof(Nomina), "nomina12", "http://www.sat.gob.mx/nomina12");
                        if (comprobante.XmlComplemento == null)
                            comprobante.XmlComplemento = complemento;
                        else
                            comprobante.XmlComplemento = comprobante.XmlComplemento + complemento;


                    }
                    if (comprobante.Complemento.cartaPorte != null)
                    {
                        comprobante.xsiSchemaLocation = comprobante.xsiSchemaLocation + " http://www.sat.gob.mx/CartaPorte http://www.sat.gob.mx/sitio_internet/cfd/CartaPorte/CartaPorte.xsd";
                        complemento = GetXmlCartaPorte(comprobante.Complemento.cartaPorte);
                        if (comprobante.XmlComplemento == null)
                            comprobante.XmlComplemento = complemento;
                        else
                            comprobante.XmlComplemento = comprobante.XmlComplemento + complemento;
                    }
                    if (comprobante.Complemento != null && comprobante.Complemento.Pag != null)
                    {
                        //comprobante.xsiSchemaLocation = comprobante.xsiSchemaLocation + " http://www.sat.gob.mx/Pagos http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos10.xsd";
                        //complemento = GetXmlAddenda(comprobante.Complemento.Pag, typeof(Pagos), "Pagos", " http://www.sat.gob.mx/Pagos");
                        comprobante.xsiSchemaLocation = comprobante.xsiSchemaLocation + " http://www.sat.gob.mx/Pagos http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos10.xsd";
                        complemento = GetXmlPagos(comprobante.Complemento.Pag);

                        if (comprobante.XmlComplemento == null)
                            comprobante.XmlComplemento = complemento;
                        else
                            comprobante.XmlComplemento = comprobante.XmlComplemento + complemento;
                    }

                    if (comprobante.Complemento != null && comprobante.Complemento.ine != null)
                    {
                        comprobante.xsiSchemaLocation = comprobante.xsiSchemaLocation + " http://www.sat.gob.mx/ine http://www.sat.gob.mx/sitio_internet/cfd/ine/ine11.xsd";
                        //complemento = GetXmlAddenda(comprobante.Complemento.ine, typeof(INE), "ine", " http://www.sat.gob.mx/ine");
                        complemento = GetXmlINE(comprobante.Complemento.ine);
                        if (comprobante.XmlComplemento == null)
                            comprobante.XmlComplemento = complemento;
                        else
                            comprobante.XmlComplemento = comprobante.XmlComplemento + complemento;

                    }

                    if (comprobante.Complemento.leyendasFicales != null)// nuevo para -----------------------------------------------------
                    {
                        comprobante.xsiSchemaLocation = comprobante.xsiSchemaLocation + " http://www.sat.gob.mx/leyendasFiscales http://www.sat.gob.mx/sitio_internet/cfd/leyendasFiscales/leyendasFisc.xsd";
                        complemento = GetXmlAddenda(comprobante.Complemento.leyendasFicales, typeof(LeyendasFiscales), "leyendasFisc", "http://www.sat.gob.mx/leyendasFiscales");
                        if (comprobante.XmlComplemento == null)
                            comprobante.XmlComplemento = complemento;
                        else
                            comprobante.XmlComplemento = comprobante.XmlComplemento + complemento;

                    }
                    if (comprobante.Complemento != null && comprobante.Complemento.Donat != null)
                    {
                        comprobante.xsiSchemaLocation = comprobante.xsiSchemaLocation + " http://www.sat.gob.mx/donat http://www.sat.gob.mx/sitio_internet/cfd/donat/donat11.xsd";
                        complemento = GetXmlDonat(comprobante.Complemento.Donat);
                        if (comprobante.XmlComplemento == null)
                            comprobante.XmlComplemento = complemento;
                        else
                            comprobante.XmlComplemento = comprobante.XmlComplemento + complemento;

                    }
    
                    if (comprobante.Complemento != null && comprobante.Complemento.implocal != null)
                    {
                        //comprobante.xsiSchemaLocation = comprobante.xsiSchemaLocation + " http://www.sat.gob.mx/implocal http://www.sat.gob.mx/sitio_internet/cfd/implocal/implocal.xsd";
                        //complemento = GetXmlAddenda(comprobante.Complemento.implocal, typeof(ImpuestosLocales), "implocal", " http://www.sat.gob.mx/implocal");

                        complemento = GetXmlImpuestosLocales(comprobante.Complemento.implocal);
                        if (comprobante.XmlComplemento == null)
                            comprobante.XmlComplemento = complemento;
                        else
                            comprobante.XmlComplemento = comprobante.XmlComplemento + complemento;
                    }
                    if (comprobante.Complemento.comercioExterior != null)// nuevo para -----------------------------------------------------
                    {
                        comprobante.xsiSchemaLocation = comprobante.xsiSchemaLocation + " http://www.sat.gob.mx/ComercioExterior11 http://www.sat.gob.mx/sitio_internet/cfd/ComercioExterior11/ComercioExterior11.xsd";
                        complemento = GetXmlAddenda(comprobante.Complemento.comercioExterior, typeof(ComercioExterior), "cce11", "http://www.sat.gob.mx/ComercioExterior11");
                        if (comprobante.XmlComplemento == null)
                            comprobante.XmlComplemento = complemento;
                        else
                            comprobante.XmlComplemento = comprobante.XmlComplemento + complemento;

                    }
                                   
                    
                }

                string comp = GetXml(comprobante);
               
                comprobante.CadenaOriginal = gen.CadenaOriginal(comp);
                comprobante.Sello = Firmar(comprobante.CadenaOriginal, rutaLlave, passLlave);
                string cadenaTimbre;
                var comprobanteTimbrado = TimbrarComprobanteNtLinkRef(comprobante, out cadenaTimbre,usuario,contraseña);
                if (comprobanteTimbrado != null)
                {
                    Logger.Info(comprobanteTimbrado.UUID);
                }
            }
            catch (FaultException fe)
            {
                Logger.Error(fe);
                throw;
            }
            catch (Exception exception)
            {
                Logger.Error((exception.InnerException == null ? exception.Message : exception.InnerException.Message));
                throw;
            }
        }

        private string NoCert(string cert)
        {
            int count = 0;
            StringBuilder sb = new StringBuilder();
            foreach (char c in cert)
            {
                if (count % 2 != 0)
                    sb.Append(c);
                count++;
            }
            return sb.ToString();
        }

        public string GetXmlPagos(Pagos impuestos)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Pagos));
            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    var sw = new StreamWriter(memStream, Encoding.UTF8);
                    using (
                        XmlWriter xmlWriter = XmlWriter.Create(sw,
                                                               new XmlWriterSettings() { Indent = false, Encoding = Encoding.UTF8 }))
                    {
                        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                        namespaces.Add("pago10", "http://www.sat.gob.mx/Pagos");
                        ser.Serialize(xmlWriter, impuestos, namespaces);
                        string xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                        xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                        xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
                        //xml = xml.Replace("xmlns:donat=\"http://www.sat.gob.mx/donat\"", "");
                        xml = xml.Replace("p1:schemaLocation=\"http://www.sat.gob.mx/Pagos http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos10.xsd\"", "");
                        xml = xml.Replace("xmlns:p1=\"http://www.w3.org/2001/XMLSchema-instance\"", "");

                        return xml;
                    }
                }
            }
            catch (Exception ee)
            {

                Logger.Error(ee);
                return null;
            }

        }
        public string GetXmlINE(INE impuestos)
        {
            XmlSerializer ser = new XmlSerializer(typeof(INE));
            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    var sw = new StreamWriter(memStream, Encoding.UTF8);
                    using (
                        XmlWriter xmlWriter = XmlWriter.Create(sw,
                                                               new XmlWriterSettings() { Indent = false, Encoding = Encoding.UTF8 }))
                    {
                        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                        namespaces.Add("ine", "http://www.sat.gob.mx/ine");
                        ser.Serialize(xmlWriter, impuestos, namespaces);
                        string xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                        xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                        xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
                        //xml = xml.Replace("xmlns:donat=\"http://www.sat.gob.mx/donat\"", "");
                        xml = xml.Replace("p1:schemaLocation=\"http://www.sat.gob.mx/Pagos http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos10.xsd\"", "");
                        xml = xml.Replace("xmlns:p1=\"http://www.w3.org/2001/XMLSchema-instance\"", "");

                        return xml;
                    }
                }
            }
            catch (Exception ee)
            {

                Logger.Error(ee);
                return null;
            }

        }


        public string GetXmlImpuestosLocales(ImpuestosLocales impuestos)
        {
            XmlSerializer ser = new XmlSerializer(typeof(ImpuestosLocales));
            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    var sw = new StreamWriter(memStream, Encoding.UTF8);
                    using (
                        XmlWriter xmlWriter = XmlWriter.Create(sw,
                                                               new XmlWriterSettings() { Indent = false, Encoding = Encoding.UTF8 }))
                    {
                        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                        namespaces.Add("implocal", "http://www.sat.gob.mx/implocal");
                        ser.Serialize(xmlWriter, impuestos, namespaces);
                        string xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                        xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                        xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
                        xml = xml.Replace("xmlns:p1=\"http://www.w3.org/2001/XMLSchema-instance\"", "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
                        xml = xml.Replace("p1:schemaLocation", "xsi:schemaLocation");
                        return xml;
                    }
                }
            }
            catch (Exception ee)
            {

                Logger.Error(ee);
                return null;
            }

        }

        public string GetXmlCartaPorte(CartaPorte impuestos)
        {
            XmlSerializer ser = new XmlSerializer(typeof(CartaPorte));
            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    var sw = new StreamWriter(memStream, Encoding.UTF8);
                    using (
                        XmlWriter xmlWriter = XmlWriter.Create(sw,
                                                               new XmlWriterSettings() { Indent = false, Encoding = Encoding.UTF8 }))
                    {
                        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                        namespaces.Add("cartaporte", "http://www.sat.gob.mx/CartaPorte");
                        ser.Serialize(xmlWriter, impuestos, namespaces);
                        string xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                        xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                        xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));

                        return xml;
                    }
                }
            }
            catch (Exception ee)
            {

                Logger.Error(ee);
                return null;
            }

        }
        public string GetXmlAddenda(object addenda, Type tipoAddenda, string prefijo, string ns)
        {
            XmlSerializer ser;
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            if (string.IsNullOrEmpty(prefijo))
            {
                ser = new XmlSerializer(tipoAddenda, ns);
            }
            else if (!string.IsNullOrEmpty(ns))
            {
                ser = new XmlSerializer(tipoAddenda);
                
                namespaces.Add(prefijo, ns);
                namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            }
            else
            {
                ser = new XmlSerializer(tipoAddenda);
            }

            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    var sw = new StreamWriter(memStream, Encoding.UTF8);
                    using (
                        XmlWriter xmlWriter = XmlWriter.Create(sw,
                                                               new XmlWriterSettings() { Indent = false, Encoding = Encoding.UTF8 }))
                    {
                        if (namespaces.Count > 0)
                            ser.Serialize(xmlWriter, addenda, namespaces);
                        else
                        {
                            ser.Serialize(xmlWriter, addenda);
                        }
                        string xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                        xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                        xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
                        return xml;
                    }
                }
            }
            catch (Exception ee)
            {

                Logger.Error(ee);
                return null;
            }
        }

    }
}