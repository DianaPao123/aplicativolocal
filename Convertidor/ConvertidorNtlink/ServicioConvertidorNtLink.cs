using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using ConvertidorCfdi;
using GeneradorCfdi;
using GeneradorCfdi.ServicioTimbrado;
using GeneradorCfdi.ServicioValidador;
using TxtToCfdi;
using log4net;
using log4net.Config;
using System.Configuration;
//using ComprobanteImpuestosTrasladoImpuesto = GeneradorCfdi.ComprobanteImpuestosTrasladoImpuesto;
//using ComprobanteTipoDeComprobante = GeneradorCfdi.ComprobanteTipoDeComprobante;

namespace ConvertidorNtlink
{
    partial class ServicioConvertidorNtLink : ServiceBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (ServicioConvertidorNtLink));
        private List<Poller> _pollers;

        public ServicioConvertidorNtLink()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
        }

        protected override void OnStop()
        {
            foreach (var pol in _pollers)
            {
                pol.Detener();  
            }
            base.OnStop();
        }

        protected override void OnStart(string[] args)
       {


//#if DEBUG
//           Debugger.Launch();
//#endif   
        

            Logger.Info(AppDomain.CurrentDomain.BaseDirectory + "\\" + "conf.xml");

            _pollers = new List<Poller>();
            try
            {
                
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + "conf.xml"))
                {
                    
                    XmlTextReader reader = new XmlTextReader(AppDomain.CurrentDomain.BaseDirectory + "\\" + "conf.xml");
                    XmlSerializer ser = new XmlSerializer(typeof(List<Configuracion>));
                    var emisores = (List<Configuracion>)ser.Deserialize(reader) ?? new List<Configuracion>();
                    reader.Close();
                    string entrada = "";
                    
                    foreach (var configuracion in emisores)
                    {
                        var polls = new List<Poller>();
                        Logger.Info(configuracion.RutaEntrada);
                        if (entrada != configuracion.RutaEntrada)
                        {
                            entrada = configuracion.RutaEntrada;
                        }
                        else
                        {
                            Logger.Error("Error, hay rutas de entrada duplicadas, " + entrada + " -> " + configuracion.RutaEntrada);
                            Environment.Exit(1);
                        }
                            
                        var pdfPath = Path.Combine(configuracion.RutaEntrada, "pdfTmp");
                        if (!Directory.Exists(pdfPath))
                            Directory.CreateDirectory(pdfPath);
                        Thread t = new Thread(()=> { ProcesarListaPdf(Directory.EnumerateFiles(pdfPath),configuracion);});
                        t.Start();
                        Poller poller = new Poller(configuracion.RutaEntrada, configuracion);
                        Poller pollerCancelar = new Poller(configuracion.RutaEntradaCancelar, configuracion);
                        //Poller pollerPdf = new Poller(pdfPath, configuracion);
                        //pollerPdf.Segundos = configuracion.Segundos;
                        //pollerPdf.Trabajador = ProcesarListaPdf;
                        poller.Segundos = configuracion.Segundos;
                        poller.Trabajador = ProcesarLista;
                        pollerCancelar.Segundos = configuracion.Segundos;
                        pollerCancelar.Trabajador = ProcesarListaCancelador;

                        polls.Add(poller);
                        polls.Add(pollerCancelar);
                        //_pollers.Add(pollerPdf);
                        foreach (var pol in polls)
                        {                            
                            pol.Iniciar();
                        }
                        _pollers.AddRange(polls);

                        
                    }
                }
                
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
            }
        }


        private void ProcesarListaPdf(IEnumerable<string> archivos, Configuracion conf)
        {

            if (conf.GenerarPdf)
            {
                foreach (var archivo in archivos)
                {
                    List<Comprobante> x = null;
                    List<Retenciones> x2 = null;
                    try
                    {
                        string tipo = conf.TipoEntrada;
                        IParser p = null; IParserR p2 = null;
                        if (tipo.Equals("Layout NT LINK"))
                            p = new ParserNtLink();
                        else if (tipo.Equals("AMECE7.1"))
                        {
                            p = new ParserAmece71();
                        }
                        else if (tipo.Equals("FTPCanadian"))
                        {
                            p = new DbfParser(conf.RutaDbfs);
                        }
                        else if (tipo.Equals("AddendaAHM"))
                        {
                            p = new ParserAHM();
                        }
                        else if (tipo.Equals("AddendaSoriana"))
                        {
                            p = new ParserSoriana();
                        }
                        else if (tipo.Equals("AddendaAdo"))
                        {
                            p = new ParserAdo();
                        }
                        else if (tipo.Equals("AddendaDisney"))
                        {
                            p = new ParserDisney();
                        }
                        else if (tipo.Equals("AddendaFemsa"))
                        {
                            p = new ParserFemsa();
                        }
                        else if (tipo.Equals("AddendaMabe"))
                        {
                            p = new ParserMabe();
                        }
                        else if (tipo.Equals("AddendaNissanAmece"))
                        {
                            p = new ParserNissan();
                        }
                        else if (tipo.Equals("AddendaDupont"))
                        {
                            p = new ParserDupont();
                        }
                        else if (tipo.Equals("AddendaIusacell"))
                        {
                            p = new ParserIusacell();
                        }
                        else if (tipo.Equals("AddendaCinepolis")) 
                        {
                            p = new ParserCinepolis();
                        }
                        else if (tipo.Equals("AddendaSky"))
                        {
                            p = new ParserSky();
                        }
                        else if (tipo.Equals("AddendaElektra"))
                        {
                            p = new ParserElektra();
                        }
                        else if (tipo.Equals("AddendaDimesa"))
                        {
                            p = new ParserDimesa();
                        }
                        else if (tipo.Equals("FarmaciasGuadalajara"))
                        {
                            p = new ParserFarmaciasGuadalajara();
                        }
                        else if (tipo.Equals("AddendaPemex"))
                        {
                            p = new ParserPemex();
                        }
                        else if (tipo.Equals("Neto"))
                        {
                            p = new ParserNeto();
                        }
                        else if (tipo.Equals("AdendaVallen"))
                        {
                            p = new ParserVallen();
                        }
                        else if (tipo.Equals("Liverpool"))
                        {
                            p = new ParserLiverpool();
                        }
                        else if (tipo.Equals("Bic"))
                        {
                            p = new ParserBic();
                        }
                        else if (tipo.Equals("AddendaTridonex"))
                        {
                            p = new ParserTridonex();
                        }
                        else if (tipo.Equals("AddendaCotemar"))
                        {
                            p = new ParserCotemar();
                        }
                        else if (tipo.Equals("AddendaHonda"))
                        {
                            p = new ParserHonda();
                        }
                        else if (tipo.Equals("AddendaLowes"))
                        {
                            p = new ParserLowes();
                        }
                        else if (tipo.Equals("AddendaJumex"))
                        {
                            p = new ParserJUMEX();
                        }
                        else if (tipo.Equals("AddendaMondelez"))
                        {
                            p = new ParserMondelez();
                        }
                        else if (tipo.Equals("AddendaPilgrims"))
                        {
                            p = new ParserPilgrims();
                        }
                        else if (tipo.Equals("AddendaPUA"))
                        {
                            p = new ParserPUA();
                        }
                        else if (tipo.Equals("AddendaPPY"))
                        {
                            p = new ParserPPY();
                        }
                        else if (tipo.Equals("AddendaASONICO"))
                        {
                            p = new ParserASONICO();
                        }
                        else if (tipo.Equals("AddendaNadro"))
                        {
                            p = new ParserNadro();
                        }
                        else if (tipo.Equals("AddendaGM"))
                        {
                            p = new ParserGM();
                        }    
                        else if (tipo.Equals("Nomina"))
                        {
                            p = new ParserNomina();
                        }
                        else if (tipo.Equals("AddendaExtemporanea"))
                        {
                            p = new ParserSorianaExtemporanea();
                        }

                        else if (tipo.Equals("AddendaDetallista"))
                        {
                            p = new ParserDetallista();
                        }
                        else if (tipo.Equals("AddendaSunchemical"))
                        {
                            p = new ParserSunchemical();
                        }
                        else if (tipo.Equals("AddendaPSV"))
                        {
                            p = new ParserPSV();
                        }
                        
                        else if (tipo.Equals("RetencionDividendos"))
                        {
                            p2 = new ParserRetenciones();

                        }
                        else
                        {
                            p = new Cfd22Parser();
                            conf.EnviarCorreo = false;
                        }
                        if (p != null)
                        { x = p.Parse(archivo); }
                        else
                        {
                            x2 = p2.Parse(archivo);

                        }
                    }
                    catch (IncompleteException )
                    {
                        Logger.Error("El archivo " + archivo + " está incompleto");
                        Thread.Sleep(1000);
                    }
                    catch (Exception ee)
                    {
                        Logger.Error(ee);
                        if (File.Exists(Path.Combine(conf.RutaError, Path.GetFileName(archivo))))
                            File.Delete(Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                        File.Move(archivo, Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                        x = null;
                        x2 = null;
                    }
                    if (x == null&&x2==null)
                        continue;

                    var fileName = Path.Combine(conf.RutaSalida, Path.GetFileName(archivo).Replace(".txt", ".xml"));
                    if (File.Exists(fileName) && x.Count == 1)
                    {
                            Comprobante comprobante = Generador.GetTimbre(File.ReadAllText(fileName), x.First());
                            ProcesaPdf(conf, archivo, comprobante);
                       
                    }
                    if (File.Exists(fileName) && x2.Count == 1)
                    {
                        Retenciones comprobante = Generador.GetTimbreRetencion(File.ReadAllText(fileName), x2.First());
                        if(comprobante.Complemento!=null)
                            if (comprobante.Complemento.timbreFiscalDigital != null)
                            {
                                comprobante.SelloSAT = comprobante.Complemento.timbreFiscalDigital.selloSAT;
                                comprobante.NoCertificadoSAT = comprobante.Complemento.timbreFiscalDigital.noCertificadoSAT;
                            }
                        ProcesaRetencionPdf(conf, archivo, comprobante);

                    }

                }
            }

        }

        private static void ProcesaPdf(Configuracion conf, string archivo, Comprobante comprobante)
        {
            if (comprobante.TipoDeComprobante == "E")
                comprobante.Titulo = "Nota de Crédito";
            else comprobante.Titulo = "Factura";
            byte[] pdf = null;
            try
            {
                var gen = new GeneradorCfdi.Generador();
                pdf = gen.GetPdfFromComprobante(comprobante, conf.RutaLogo);
                var ruta = Path.Combine(conf.RutaSalida, Path.GetFileNameWithoutExtension(archivo));
                string pdfFile = ruta + ".pdf";
                File.WriteAllBytes(pdfFile, pdf);
                if (!string.IsNullOrEmpty(conf.SalidaAdicional))
                {
                    try
                    {
                        var ra = Path.Combine(conf.SalidaAdicional, Path.GetFileNameWithoutExtension(archivo));
                        string adicional = ra + ".pdf";
                        File.WriteAllBytes(adicional, pdf);
                    }
                    catch (Exception ee)
                    {
                        Logger.Error(ee);
                    }
                }
            }

            catch (Exception ee)
            {
                Logger.Error(ee);
                return;
            }
            if (File.Exists(Path.Combine(conf.RutaRespaldo, Path.GetFileName(archivo))))
                File.Delete(Path.Combine(conf.RutaRespaldo, Path.GetFileName(archivo)));
            File.Move(archivo, Path.Combine(conf.RutaRespaldo, Path.GetFileName(archivo)));
            //Guardar en base de datos

            //Voy a enviar el correo
            if (conf.EnviarCorreo)
            {
                try
                {
                    var xmlBytes = Encoding.UTF8.GetBytes(comprobante.XmlString);
                    EmailAttachment xmlAtt = new EmailAttachment()
                                                 {
                                                     Name =
                                                         comprobante.Complemento.timbreFiscalDigital.UUID.
                                                             ToString() + ".xml",
                                                     Attachment = xmlBytes
                                                 };
                    EmailAttachment pdfAtt = new EmailAttachment()
                                                 {
                                                     Name =
                                                         comprobante.Complemento.timbreFiscalDigital.UUID.
                                                             ToString() + ".pdf",
                                                     Attachment = pdf
                                                 };
                    Mailer mailer = new Mailer(conf.EmailServer, conf.EmailPort, conf.EmailUserName,
                                               conf.EmailPassword);
                    var atts = new[] {xmlAtt, pdfAtt}.ToList();
                    if(conf.TipoEntrada.Equals("FTPCanadian"))
                    {
                        string canadianEmail = comprobante.DbfDatosCliente.EMAIL;
                        if (!canadianEmail.Equals("."))
                        {
                            comprobante.Receptor.Emails = canadianEmail + ";";
                        } else
                        {
                            comprobante.Receptor.Emails = "";
                        }
                    }
                    var receptores = comprobante.Receptor.Emails.Split(';').ToList();
                    string contenido;
                    string subject;

                    subject = "Envío de Factura";

                    if (comprobante.TipoDeComprobante == "I")
                        subject = "Envío de CFDI Ingreso";
                    if (comprobante.TipoDeComprobante == "P")
                        subject = "Envío de Pago";
                    if (comprobante.TipoDeComprobante == "E")
                        subject = "Envío de CFDI Egreso";
                    if (comprobante.TipoDeComprobante == "T")
                        subject = "Envío de CFDI Traslado";
                   
                        contenido = "Se adjunta la factura con folio " + comprobante.Serie + " " + comprobante.Folio + " " +
                                    comprobante.Complemento.timbreFiscalDigital.UUID +
                                    " en formato XML y PDF.";
                        
              
                    //if (comprobante.TipoDeComprobante == "I")
                    //{
                    //    contenido = "Se adjunta la factura con folio " + comprobante.Serie + " " + comprobante.Folio + " " +
                    //                comprobante.Complemento.timbreFiscalDigital.UUID +
                    //                " en formato XML y PDF.";
                    //    subject = "Envío de Factura";
                    //}

                    //else
                    //{
                    //    if (comprobante.TipoDeComprobante == "P")
                    //    {
                    //        contenido = "Se adjunta el Pago con folio " + comprobante.Serie + " " + comprobante.Folio + " " +
                    //                    comprobante.Complemento.timbreFiscalDigital.UUID +
                    //                    " en formato XML y PDF.";
                    //        subject = "Envío de Pago";
                    //    }
                    //    else
                    //    {
                    //        contenido = "Se adjunta la nota de crédito con folio " +
                    //                    comprobante.Complemento.timbreFiscalDigital.UUID +
                    //                    " en formato XML y PDF.";
                    //        subject = "Envío de Nota de Crédito";
                    //    }
                    //}
                    try
                    {
                        mailer.Send(receptores, atts,
                                    contenido,
                                    subject, conf.Remitente, comprobante.Emisor.Nombre, comprobante.Receptor.Bcc
                            );
                    }
                    catch (Exception eee)
                    {
                        Logger.Error("Error en envío de correo electrónico\n" +
                                     "Factura: " + comprobante.Complemento.timbreFiscalDigital.UUID, eee);
                    }
                }
                catch (Exception ee)
                {
                    Logger.Error(ee);
                }
            }
        }
        private static void ProcesaRetencionPdf(Configuracion conf, string archivo, Retenciones comprobante)
        {
            byte[] pdf = null;
            try
            {
                var gen = new GeneradorCfdi.Generador();
                pdf = gen.GetPdfFromRetencion(comprobante, conf.RutaLogo);
                var ruta = Path.Combine(conf.RutaSalida, Path.GetFileNameWithoutExtension(archivo));
                string pdfFile = ruta + ".pdf";
                File.WriteAllBytes(pdfFile, pdf);
                if (!string.IsNullOrEmpty(conf.SalidaAdicional))
                {
                    try
                    {
                        var ra = Path.Combine(conf.SalidaAdicional, Path.GetFileNameWithoutExtension(archivo));
                        string adicional = ra + ".pdf";
                        File.WriteAllBytes(adicional, pdf);
                    }
                    catch (Exception ee)
                    {
                        Logger.Error(ee);
                    }
                }
            }

            catch (Exception ee)
            {
                Logger.Error(ee);
                return;
            }
            if (File.Exists(Path.Combine(conf.RutaRespaldo, Path.GetFileName(archivo))))
                File.Delete(Path.Combine(conf.RutaRespaldo, Path.GetFileName(archivo)));
            File.Move(archivo, Path.Combine(conf.RutaRespaldo, Path.GetFileName(archivo)));
            //Guardar en base de datos

            //Voy a enviar el correo
            if (conf.EnviarCorreo)
            {
                try
                {
                    var xmlBytes = Encoding.UTF8.GetBytes(comprobante.XmlString);
                    EmailAttachment xmlAtt = new EmailAttachment()
                    {
                        Name =
                            comprobante.Complemento.timbreFiscalDigital.UUID.
                                ToString() + ".xml",
                        Attachment = xmlBytes
                    };
                    EmailAttachment pdfAtt = new EmailAttachment()
                    {
                        Name =
                            comprobante.Complemento.timbreFiscalDigital.UUID.
                                ToString() + ".pdf",
                        Attachment = pdf
                    };
                    Mailer mailer = new Mailer(conf.EmailServer, conf.EmailPort, conf.EmailUserName,
                                               conf.EmailPassword);
                    var atts = new[] { xmlAtt, pdfAtt }.ToList();
                    //if (conf.TipoEntrada.Equals("FTPCanadian"))
                    //{
                    //    string canadianEmail = comprobante.DbfDatosCliente.EMAIL;
                    //    if (!canadianEmail.Equals("."))
                    //    {
                    //        comprobante.Receptor.Emails = canadianEmail + ";";
                    //    }
                    //    else
                    //    {
                    //        comprobante.Receptor.Emails = "";
                    //    }
                    //}
                    var receptores = comprobante.Receptor.Emails.Split(';').ToList();
                    string contenido;
                    string subject;


                        contenido = "Se adjunta la factura con folio " + comprobante.FolioInt + " " + comprobante.CveRetenc + " " +
                                    comprobante.Complemento.timbreFiscalDigital.UUID +
                                    " en formato XML y PDF.";
                        subject = "Envío de Retencion";
                    

                    
                    try
                    {
                        mailer.Send(receptores, atts,
                                    contenido,
                                    subject, conf.Remitente, comprobante.Emisor.NomDenRazSocE, comprobante.Receptor.Bcc
                            );
                    }
                    catch (Exception eee)
                    {
                        Logger.Error("Error en envío de correo electrónico\n" +
                                     "Retencion: " + comprobante.Complemento.timbreFiscalDigital.UUID, eee);
                    }
                }
                catch (Exception ee)
                {
                    Logger.Error(ee);
                }
            }
        }

        private Generador.AcuseCan ParseAcuse(string acuseXml)
        {
            //System.Diagnostics.Debugger.Launch();
            Generador.AcuseCan acuseCan = new Generador.AcuseCan();
            XmlReader reader = new XmlTextReader(new StringReader(acuseXml));
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.LocalName == "SignatureValue")
                    {
                        reader.Read();
                        acuseCan.Sello = reader.Value;

                    }

                    if (reader.LocalName == "UUID")
                    {
                        reader.Read();
                        acuseCan.Folio = reader.Value;
                    }
                    if (reader.LocalName == "EstatusUUID")
                    {
                        reader.Read();
                        acuseCan.Status = reader.Value;
                    }




                    if (reader.LocalName == "Acuse")
                    {
                        acuseCan.Fecha = reader.GetAttribute("Fecha");
                        acuseCan.RfcEmisor = reader.GetAttribute("RfcEmisor");

                    }
                }

            }
            return acuseCan;
        }

        private void ProcesarListaCancelador(List<string> archivos, Configuracion conf)
        {
            string nombreUsuario = ConfigurationManager.AppSettings["userName"];
            string pass = ConfigurationManager.AppSettings["password"];
            Logger.Info("Se encontraron " + archivos.Count() + " Archivos para cancelar");
            foreach (var archivo in archivos)
            {
                List<string> uuids = new List<string>();
                Logger.Info("Procesando " + archivo);
                try
                {
                    var lines = File.ReadAllLines(archivo);
                    if(lines.Length < 1)
                        throw new ApplicationException("El archivo está vacío");
                    var datos = lines[0].Split('|');
                    uuids.Add(datos[0]);
                    string requestCancelacion = GeneradorMensajeCancelacion.GetMensajeCancelacion(uuids, conf.RutaCer, conf.RutaKey, conf.PasswordKey, conf.EmisorRfc);
                    ServicioTimbradoClient servicioCancelacion = new ServicioTimbradoClient();


                    RespuestaCancelacion result = servicioCancelacion.CancelaCfdiRequest(nombreUsuario, pass, requestCancelacion, datos[2], datos[0], datos[3]);
                    Logger.Info("Estatus Acuse" + result.Acuse);
                    Logger.Info("Estatus Uuids" + result.StatusUuids);
                    Logger.Info("Mensaje de Error" + result.MensajeError);
                    var acuse = result.Acuse;
                    if (acuse != null)
                    {
                        File.WriteAllText(
                            Path.Combine(conf.RutaSalidaCancelar, Path.GetFileName(archivo) + "_acuse.txt"), acuse);
                        var can = ParseAcuse(acuse);
                        int statusCan = 0;
                        StringBuilder sb = new StringBuilder();
                        foreach (var statusUuid in result.StatusUuids)
                        {
                            if (statusUuid.Status == "205")
                            {
                                statusCan = 2;
                            }

                            if (statusUuid.Status == "201")
                            {
                                can.Status = "201 - Cancelado correctamente";
                                statusCan = 1;
                            }

                            if (statusUuid.Status == "202")
                            {
                                can.Status = "202 - Previamente cancelado";
                                statusCan = 1;
                            }

                            sb.AppendLine(statusUuid.Uuid + " - " + statusUuid.Status);
                        }
                        DataAccess da = new DataAccess();
                        if (!string.IsNullOrEmpty(conf.CadenaConexion) && statusCan == 1)
                        {
                            if (!string.IsNullOrEmpty(conf.CadenaConexion))
                                da.ActualizarCancelacion(can.Folio, can.Sello, Convert.ToDateTime(can.Fecha),
                                                         conf.CadenaConexion);

                            if (!string.IsNullOrEmpty(conf.CadenaConexion))
                                da.ActualizarEstatusCancelacion(datos[0], statusCan, conf.CadenaConexion);
                        }
                        if (statusCan == 1)
                        {
                            //-----------------------------------------------
                          
                                               
                      
                           var res= servicioCancelacion.ConsultaEstatusCFDI(nombreUsuario, pass,datos[2]);
                              string[] status = res.Split('|');
                            //---------------------------------------------------------
                            Generador gen = new Generador();
                            byte[] pdf = gen.GetPdfFromAcuseCancelacion(can, status[0], status[1], datos[3], status[2]);
                            var ruta = Path.Combine(conf.RutaSalidaCancelar, Path.GetFileNameWithoutExtension(archivo));
                            string pdfFile = ruta + ".pdf";
                            File.WriteAllBytes(pdfFile, pdf);
                            string bcc = null;
                            if (conf.EnviarCorreo && !string.IsNullOrEmpty( datos[1]) && statusCan == 1)
                            {
                                Logger.Info("enviando a: " + datos[1]);

                                var mailer = new Mailer(conf.EmailServer, conf.EmailPort, conf.EmailUserName,
                                                        conf.EmailPassword);
                                if (datos.Length > 4)
                                {
                                    bcc = datos[4];
                                    Logger.Info("bcc: " + datos[4]);
                                }
                                var attachments = new List<EmailAttachment>();
                                attachments.Add(new EmailAttachment() { Attachment = pdf, Name = can.Folio + ".pdf" });
                                mailer.Send(datos[1].Split(';').ToList(), attachments, "Se adjunta el archivo de acuse de cancelación", "Envío de cancelación de factura", conf.Remitente, conf.Remitente, bcc);
                            } 
                        }
                        
                        if (File.Exists(Path.Combine(conf.RutaRespaldo, Path.GetFileName(archivo))))
                            File.Delete(Path.Combine(conf.RutaRespaldo, Path.GetFileName(archivo)));
                        File.Move(archivo, Path.Combine(conf.RutaRespaldo, Path.GetFileName(archivo)));
                    }
                    else
                    {
                        Logger.Error(result.MensajeError);
                        var archivoError = Path.Combine(conf.RutaErrorCancelar, Path.GetFileName(archivo));
                        if (File.Exists(archivoError))
                            File.Delete(archivoError);
                        File.Move(archivo, archivoError);
                        try
                        {
                            DataAccess da = new DataAccess();
                            da.ActualizarEstatusCancelacion(datos[0], 3, conf.CadenaConexion);
                        }
                        catch (Exception ww)
                        {
                            Logger.Error(ww);
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("Error en procesamiento de Cancelación" + e);
                    var archivoError = Path.Combine(conf.RutaErrorCancelar, Path.GetFileName(archivo));
                    if (File.Exists(archivoError))
                        File.Delete(archivoError);
                    File.Move(archivo, archivoError);

                }
            }


        }
        private void ProcesarLista(List<string> archivos, Configuracion conf)
        {
            bool leerSalida = ConfigurationManager.AppSettings["LeerSalida"] == "1";
            Logger.Info("Se encontraron " + archivos.Count() + " Archivos");
            var pdfPath = Path.Combine(conf.RutaEntrada, "pdfTmp");
            if (!Directory.Exists(pdfPath))
                Directory.CreateDirectory(pdfPath);
            int i = 0;
            DataAccess da = new DataAccess();
            foreach (var archivo in archivos)
            {
                FileInfo fi =new FileInfo(archivo);
                
                var sha1 = new SHA1Managed();
                byte[] bytes = File.ReadAllBytes(archivo);
                var hash = sha1.ComputeHash(bytes);
                Logger.Debug(Encoding.UTF8.GetString(bytes));
                Trace.WriteLine(BitConverter.ToString(hash) + "\t" + archivo + "\t" + fi.LastWriteTimeUtc.ToString() + "\t" + fi.Length);
                Logger.Info(archivo);
                var salida = new StringBuilder();
                Interlocked.Increment(ref i);
                Logger.Info("Procesando " + archivo);
                List<Comprobante> x = null; List<Retenciones> x2 = null;
                try
                {
                    string tipo = conf.TipoEntrada;
                    IParser p = null; IParserR p2 = null;
                    if (tipo.Equals("Layout NT LINK"))
                        p = new ParserNtLink();
                    else if (tipo.Equals("AMECE7.1"))
                    {
                        p = new ParserAmece71();
                    }
                    else if (tipo.Equals("FTPCanadian"))
                    {
                        p = new DbfParser(conf.RutaDbfs);
                    }
                    else if (tipo.Equals("AddendaAHM"))
                    {
                        p = new ParserAHM();
                    }
                    else if (tipo.Equals("AddendaSoriana"))
                    {
                        p = new ParserSoriana();
                    }
                    else if (tipo.Equals("AddendaDisney"))
                    {
                        p = new ParserDisney();
                    }
                    else if (tipo.Equals("AddendaFemsa"))
                    {
                        p = new ParserFemsa();
                    }
                    else if (tipo.Equals("AddendaMabe"))
                    {
                        p = new ParserMabe();
                    }
                    else if (tipo.Equals("AddendaAdo"))
                    {
                        p = new ParserAdo();
                    }
                    else if (tipo.Equals("AddendaNissanAmece"))
                    {
                        p = new ParserNissan();
                    }
                    else if (tipo.Equals("AddendaDupont"))
                    {
                        p = new ParserDupont();
                    }
                    else if (tipo.Equals("AddendaIusacell"))
                    {
                        p = new ParserIusacell();
                    }
                    else if (tipo.Equals("AddendaCinepolis"))
                    {
                        p = new ParserCinepolis();
                    }
                    else if (tipo.Equals("AddendaSky"))
                    {
                        p = new ParserSky();
                    }
                    else if (tipo.Equals("AddendaElektra"))
                    {
                        p = new ParserElektra();
                    }
                    else if (tipo.Equals("AddendaDimesa"))
                    {
                        p = new ParserDimesa();
                    }
                    else if (tipo.Equals("FarmaciasGuadalajara"))
                    {
                        p = new ParserFarmaciasGuadalajara();
                    }
                    else if (tipo.Equals("AddendaPemex"))
                    {
                        p = new ParserPemex();
                    }
                    else if (tipo.Equals("Neto"))
                    {
                        p = new ParserNeto();
                    }
                    else if (tipo.Equals("AddendaVallen"))
                    {
                        p = new ParserVallen();
                    }
                    else if (tipo.Equals("Liverpool"))
                    {
                        p = new ParserLiverpool();
                    }
                    else if (tipo.Equals("Bic"))
                    {
                        p = new ParserBic();
                    }
                    else if (tipo.Equals("AddendaTridonex"))
                    {
                        p = new ParserTridonex();
                    }
                    else if (tipo.Equals("AddendaCotemar"))
                    {
                        p = new ParserCotemar();
                    }
                    else if (tipo.Equals("AddendaHonda"))
                    {
                        p = new ParserHonda();
                    }
                    else if (tipo.Equals("AddendaLowes"))
                    {
                        p = new ParserLowes();
                    }
                    else if (tipo.Equals("AddendaJumex"))
                    {
                        p = new ParserJUMEX();
                    }
                    else if (tipo.Equals("AddendaMondelez"))
                    {
                        p = new ParserMondelez();
                    }
                    else if (tipo.Equals("AddendaPilgrims"))
                    {
                        p = new ParserPilgrims();
                    }
                    else if (tipo.Equals("AddendaPUA"))
                    {
                        p = new ParserPUA();
                    }
                    else if (tipo.Equals("AddendaPPY"))
                    {
                        p = new ParserPPY();
                    }
                    else if (tipo.Equals("AddendaASONICO"))
                    {
                        p = new ParserASONICO();
                    }
                    else if (tipo.Equals("AddendaNadro"))
                    {
                        p = new ParserNadro();
                    }
                    else if (tipo.Equals("AddendaGM"))
                    {
                        p = new ParserGM();
                    }   
                    else if (tipo.Equals("Nomina"))
                    {
                        p = new ParserNomina();
                    }
                    else if (tipo.Equals("AddendaExtemporanea"))
                    {
                        p = new ParserSorianaExtemporanea();
                    }
                    else if (tipo.Equals("AddendaDetallista"))
                    {
                        p = new ParserDetallista();
                    }
                    else if (tipo.Equals("AddendaSunchemical"))
                    {
                        p = new ParserSunchemical();
                    }
                    else if (tipo.Equals("AddendaPSV"))
                    {
                        p = new ParserPSV();
                    }
                        
                    else if (tipo.Equals("RetencionDividendos"))
                    {
                        p2 = new ParserRetenciones();
                    }
                    else
                    {
                        p = new Cfd22Parser();
                        conf.EnviarCorreo = false;
                    }
                    if (p != null)
                    { x = p.Parse(archivo); }
                    else
                    {
                        
                        x2 = p2.Parse(archivo);

                    }
                }
                catch (IncompleteException)
                {
                    Logger.Error("El archivo " + archivo + " está incompleto");
                    Thread.Sleep(1000);
                }
                catch (Exception ee)
                {
                    Logger.Error(ee);
                    if (File.Exists(Path.Combine(conf.RutaError, Path.GetFileName(archivo))))
                        File.Delete(Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                    File.Move(archivo, Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                    x = null; x2 = null;

                }
                if (x == null && x2==null)
                    continue;

                #region CFDI
                if(x!=null)
                foreach (var comprobante in x)
                {
                    try
                    {
                        var compSalida = Path.Combine(conf.RutaSalida, Path.GetFileName(archivo).Replace(".txt", ".xml"));
                        if (File.Exists(compSalida))
                        {
                            Logger.Error("Archivo " + compSalida + " Duplicado");
                            throw new Exception("Archivo " + compSalida + " Duplicado");
                        }
                       // if (comprobante.Impuestos != null && comprobante.Impuestos.Traslados != null && comprobante.Impuestos.Traslados.Length > 0)
                       //     comprobante.Impuestos.Traslados = comprobante.Impuestos.Traslados;
                        //Comentar para produccion
/*
#if DEBUG
                        comprobante.Emisor.rfc = "AAD990814BP7";
                        comprobante.noCertificado = "00001000000104602374";
                        comprobante.fecha = DateTime.Now;
#endif
 */ 
                        //Fin de Comentar para produccion
                        comprobante.Certificado = Convert.ToBase64String(File.ReadAllBytes(conf.RutaCer));
                        int status = 1;
                        
                        Generador gen = new Generador();
                        try
                        {
                            gen.GenerarCfd(comprobante, new X509Certificate2(conf.RutaCer), conf.RutaKey, conf.PasswordKey,conf.EmisorUsuario,conf.EmisorContraseña);
                            if (!string.IsNullOrEmpty(conf.CadenaConexion))
                                da.ActualizarEstatusTimbrado(comprobante.Folio, comprobante.Serie, 1, conf.CadenaConexion);

                        }
                        catch (Exception ee)
                        {
                            if (!string.IsNullOrEmpty(conf.CadenaConexion))
                                da.ActualizarEstatusTimbrado(comprobante.Folio, comprobante.Serie, 2, conf.CadenaConexion);
                            throw;
                        }
                        
                        File.WriteAllText(
                            Path.Combine(conf.RutaSalida, Path.GetFileName(archivo).Replace(".txt", ".xml")),
                            comprobante.XmlString);
                        if (!string.IsNullOrEmpty(conf.SalidaAdicional))
                        {
                            try
                            {
                                File.WriteAllText(
                                Path.Combine(conf.SalidaAdicional, Path.GetFileName(archivo).Replace(".txt", ".xml")),
                                comprobante.XmlString);
                            }
                            catch(Exception ee)
                            {
                                Logger.Error(ee);
                            }
                            
                        }
                        if (!leerSalida)
                        {
                            ProcesaPdf(conf, archivo, comprobante);
                        }
                        string SerieF = comprobante.Serie;
                        string FolioF = comprobante.Folio;
                        string UUDIF = "";
                        try { UUDIF = comprobante.Complemento.timbreFiscalDigital.UUID; }
                        catch (Exception ee) { }
                        string noCertificadoEmisorF= comprobante.NoCertificado;
                        string noCertificadoPacF = "";
                        try { noCertificadoPacF = comprobante.Complemento.timbreFiscalDigital.NoCertificadoSAT; }
                        catch (Exception ee) { }
                        string fechaF = comprobante.Fecha;
                        string FechaTimbreF = "";
                        try
                        {
                            FechaTimbreF = comprobante.Complemento.timbreFiscalDigital.FechaTimbrado;//comprobante.Complemento.timbreFiscalDigital.FechaTimbrado.ToString("s"); 
                        }
                        catch (Exception ee) { }
                        string selloEmisorF = comprobante.Sello;
                        string selloSatF = "";
                        try { selloSatF = comprobante.Complemento.timbreFiscalDigital.SelloSAT; }
                        catch (Exception ee) { }
                        string cadenaOriginalTimbreF = comprobante.CadenaOriginalTimbre;

                        //------------------------------
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

                        string total = enteros + "." + decimales;
                        int tam_var = comprobante.Sello.Length;
                        string Var_Sub = comprobante.Sello.Substring((tam_var - 8), 8);
                        string URL = @"https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx";

                        string cadenaCodigo = URL + "?" + "&id=" + comprobante.Complemento.timbreFiscalDigital.UUID.ToUpper() + "&re=" + comprobante.Emisor.Rfc + "&rr=" + comprobante.Receptor.Rfc + "&tt=" + total + "&fe=" + Var_Sub;



                        string Salida = SerieF + "|" + FolioF + "|" + UUDIF + "|" + noCertificadoEmisorF + "|" + noCertificadoPacF + "|" + fechaF + "|" + FechaTimbreF + "|" + selloEmisorF + "|" + selloSatF + "|" + cadenaOriginalTimbreF + "|" + cadenaCodigo;
                        salida.AppendLine(Salida.ToString());
                        Logger.Info(comprobante.ToString());
                        Logger.Info(comprobante.XmlString);
                        if (!string.IsNullOrEmpty(conf.CadenaConexion))
                        {
                            try
                            {

                                da.ActualizarCfdi(comprobante.Complemento.timbreFiscalDigital.UUID,
                                                  comprobante.XmlString, comprobante.Serie, comprobante.Folio,
                                                  conf.CadenaConexion);
                            }
                            catch (Exception ee)
                            {
                                Logger.Error(ee);
                                if (ee.InnerException != null)
                                    Logger.Error(ee.InnerException);
                            }

                        }
                        if (leerSalida)
                        {
                            if (File.Exists(Path.Combine(pdfPath, Path.GetFileName(archivo))))
                                File.Delete(Path.Combine(pdfPath, Path.GetFileName(archivo)));
                            File.Move(archivo, Path.Combine(pdfPath, Path.GetFileName(archivo)));
                        }
                        
                        
                    }
                    catch (FaultException fe)
                    {
                        if (File.Exists(Path.Combine(conf.RutaError, Path.GetFileName(archivo))))
                            File.Delete(Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                        File.Move(archivo, Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                        Logger.Error(fe.Message);
                    }
                    catch (Exception fe)
                    {
                        if (File.Exists(Path.Combine(conf.RutaError, Path.GetFileName(archivo))))
                            File.Delete(Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                        File.Move(archivo, Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                        Logger.Error(fe.Message);
                    }

                }
  #endregion

                #region retenciones
                if(x2!=null)
                foreach (var comprobante in x2)
                {
                    try
                    {
                        var compSalida = Path.Combine(conf.RutaSalida, Path.GetFileName(archivo).Replace(".txt", ".xml"));
                        if (File.Exists(compSalida))
                        {
                            Logger.Error("Archivo " + compSalida + " Duplicado");
                            throw new Exception("Archivo " + compSalida + " Duplicado");
                        }
          
                       // comprobante.Certificado = Convert.ToBase64String(File.ReadAllBytes(conf.RutaCer));
                        int status = 1;

                        Generador gen = new Generador();
                        try
                        {
                         var timbre=   gen.GenerarCfdRetenciones(comprobante, new X509Certificate2(conf.RutaCer), conf.RutaKey, conf.PasswordKey,conf.EmisorUsuario,conf.EmisorContraseña);
                         comprobante.SelloSAT = timbre.selloCFD;
                         comprobante.NoCertificadoSAT = timbre.noCertificadoSAT;
                            //if (!string.IsNullOrEmpty(conf.CadenaConexion))
                            //    da.ActualizarEstatusTimbrado(comprobante.Folio, comprobante.Serie, 1, conf.CadenaConexion);

                        }
                        catch (Exception ee)
                        {
                           // if (!string.IsNullOrEmpty(conf.CadenaConexion))
                           //     da.ActualizarEstatusTimbrado(comprobante.Folio, comprobante.Serie, 2, conf.CadenaConexion);
                            throw;
                        }

                        File.WriteAllText(Path.Combine(conf.RutaSalida, Path.GetFileName(archivo).Replace(".txt", ".xml")),comprobante.XmlString);
                        if (!string.IsNullOrEmpty(conf.SalidaAdicional))
                        {
                            try
                            {
                                File.WriteAllText(
                                Path.Combine(conf.SalidaAdicional, Path.GetFileName(archivo).Replace(".txt", ".xml")),
                                comprobante.XmlString);
                            }
                            catch (Exception ee)
                            {
                                Logger.Error(ee);
                            }

                        }
                        if (!leerSalida)
                        {
                            ProcesaRetencionPdf(conf, archivo, comprobante);
                        }
                     //   string SerieF = comprobante.Serie;
                     //   string FolioF = comprobante.Folio;
                        string UUDIF = "";
                        try { UUDIF = comprobante.Complemento.timbreFiscalDigital.UUID; }
                        catch (Exception ee) { }
                        string noCertificadoEmisorF = comprobante.NumCert;
                        string noCertificadoPacF = "";
                        try { noCertificadoPacF = comprobante.Complemento.timbreFiscalDigital.noCertificadoSAT; }
                        catch (Exception ee) { }
                        string fechaF = comprobante.FechaExp.ToString();
                        string FechaTimbreF = "";
                        try
                        {
                            FechaTimbreF = comprobante.Complemento.timbreFiscalDigital.FechaTimbrado.ToString();//comprobante.Complemento.timbreFiscalDigital.FechaTimbrado.ToString("s"); 
                        }
                        catch (Exception ee) { }
                        string selloEmisorF = comprobante.Sello;
                        string selloSatF = "";
                        try { selloSatF = comprobante.Complemento.timbreFiscalDigital.selloSAT; }
                        catch (Exception ee) { }
                        string cadenaOriginalTimbreF = comprobante.CadenaOriginalTimbre;

                        //------------------------------
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
                            rfcRec = ((RetencionesReceptorNacional) comprobante.Receptor.Item).RFCRecep;
                            else rfcRec = ((RetencionesReceptorExtranjero) comprobante.Receptor.Item).NumRegIdTrib;

                         string total = enteros + "." + decimales;
                        int tam_var = comprobante.Sello.Length;
                        string Var_Sub = comprobante.Sello.Substring((tam_var - 8), 8);
                        string URL = @"https://prodretencionverificacion.clouda.sat.gob.mx/";

                        string cadenaCodigo = URL + "?" + "&id=" + comprobante.Complemento.timbreFiscalDigital.UUID.ToUpper() + "&re=" + comprobante.Emisor.RFCEmisor + "&rr=" + rfcRec + "&tt=" + total + "&fe=" + Var_Sub;
                        
                        string Salida =  UUDIF + "|" + noCertificadoEmisorF + "|" + noCertificadoPacF + "|" + fechaF + "|" + FechaTimbreF + "|" + selloEmisorF + "|" + selloSatF + "|" + cadenaOriginalTimbreF + "|" + cadenaCodigo;
                        salida.AppendLine(Salida.ToString());
                        Logger.Info(comprobante.ToString());
                        Logger.Info(comprobante.XmlString);
                        //if (!string.IsNullOrEmpty(conf.CadenaConexion))
                        //{
                        //    try
                        //    {

                        //        da.ActualizarCfdi(comprobante.Complemento.timbreFiscalDigital.UUID,
                        //                          comprobante.XmlString, comprobante.Serie, comprobante.Folio,
                        //                          conf.CadenaConexion);
                        //    }
                        //    catch (Exception ee)
                        //    {
                        //        Logger.Error(ee);
                        //        if (ee.InnerException != null)
                        //            Logger.Error(ee.InnerException);
                        //    }

                        //}
                        if (leerSalida)
                        {
                            if (File.Exists(Path.Combine(pdfPath, Path.GetFileName(archivo))))
                                File.Delete(Path.Combine(pdfPath, Path.GetFileName(archivo)));
                            File.Move(archivo, Path.Combine(pdfPath, Path.GetFileName(archivo)));
                        }


                    }
                    catch (FaultException fe)
                    {
                        if (File.Exists(Path.Combine(conf.RutaError, Path.GetFileName(archivo))))
                            File.Delete(Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                        File.Move(archivo, Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                        Logger.Error(fe.Message);
                    }
                    catch (Exception fe)
                    {
                        if (File.Exists(Path.Combine(conf.RutaError, Path.GetFileName(archivo))))
                            File.Delete(Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                        File.Move(archivo, Path.Combine(conf.RutaError, Path.GetFileName(archivo)));
                        Logger.Error(fe.Message);
                    }

                }
                #endregion
          
                var sal = Path.GetFileName(archivo).Replace(".xml", ".txt").Replace(".XML",".txt");
                File.WriteAllText(Path.Combine(conf.RutaSalida, "Salida_" + sal), salida.ToString());
                if (!string.IsNullOrEmpty(conf.SalidaAdicional))
                {
                    try
                    {
                        File.WriteAllText(Path.Combine(conf.SalidaAdicional, "Salida_" + sal), salida.ToString());
                    }
                    catch (Exception ee)
                    {
                        Logger.Info(ee);
                    }
                    
                }
            }

            if (leerSalida)
            {
                ProcesarListaPdf(Directory.EnumerateFiles(pdfPath), conf);
            }
                
        }

    }
}
