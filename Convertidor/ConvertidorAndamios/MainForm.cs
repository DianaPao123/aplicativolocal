using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using GeneradorCfdi;
using log4net;
using log4net.Config;
using TxtToCfdi;
//using ComprobanteTipoDeComprobante = GeneradorCfdi.ComprobanteTipoDeComprobante;

namespace ConvertidorAndamios
{
    public partial class MainForm : Form
    {

        private static ILog Logger = LogManager.GetLogger(typeof(MainForm));
        public MainForm()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
        }

        private string GetFolderDialog()
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                return fd.SelectedPath;
            }
            return null;
        }

        private string GetFileDialog()
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                return fd.FileName;
            }
            return null;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.txtEntrada.Text = GetFolderDialog();
            if (Directory.Exists(txtEntrada.Text))
            {
                this.lblStatus.Text = "Se encontraron " + Directory.EnumerateFiles(txtEntrada.Text, "*.txt").Count() +
                                      " Archivos";
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.txtSalida.Text = GetFolderDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.txtRespaldo.Text = GetFolderDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.txtError.Text = GetFolderDialog();
        }


        private bool ValidarInput()
        {
            if (string.IsNullOrEmpty(txtCer.Text))
            {
                MessageBox.Show("Escribe la ruta del certificado");
                return false;
            }
            if (string.IsNullOrEmpty(txtKey.Text))
            {
                MessageBox.Show("Escribe la ruta de la llave privada");
                return false;
            }
            if (string.IsNullOrEmpty(txtPass.Text))
            {
                MessageBox.Show("Escribe el password de la llave privada");
                return false;
            }
            if (string.IsNullOrEmpty(txtEntrada.Text))
            {
                MessageBox.Show("Escribe el directorio de entrada");
                return false;
            }
            if (string.IsNullOrEmpty(txtSalida.Text))
            {
                MessageBox.Show("Escribe el directorio de salida");
                return false;
            }
            if (string.IsNullOrEmpty(txtRespaldo.Text))
            {
                MessageBox.Show("Escribe el directorio de respaldo");
                return false;
            }
            if (string.IsNullOrEmpty(txtError.Text))
            {
                MessageBox.Show("Escribe el directorio de error");
                return false;
            }
            if (txtEntrada.Text == txtSalida.Text || txtRespaldo.Text == txtError.Text 
                || txtEntrada.Text == txtError.Text || txtSalida.Text == txtError.Text
                || txtRespaldo.Text == txtSalida.Text)
            {
                MessageBox.Show("Los directorios no pueden ser los mismos");
                return false;
            }
            return true;
        }


        private void ProcesarArchivos()
        {
            if (ValidarInput())
            {
                XmlSerializer ser = new XmlSerializer(typeof(Configuracion));
                Configuracion conf = new Configuracion
                                         {
                                             RutaCer = txtCer.Text, RutaKey = txtKey.Text,
                                             RutaEntrada = txtEntrada.Text, RutaSalida = txtSalida.Text,
                                             RutaError = txtError.Text, RutaRespaldo = txtRespaldo.Text,
                                             PasswordKey = txtPass.Text, GenerarPdf = chkGeneraPdf.Checked,
                                             EmisorRfc = txtEmisorRfc.Text

                                         };
                XmlTextWriter txt = new XmlTextWriter(AppDomain.CurrentDomain.BaseDirectory + "\\" + "confAndamios.xml", Encoding.UTF8);

                ser.Serialize(txt,conf);
                txt.Close();
            }
            var archivos = Directory.EnumerateFiles(txtEntrada.Text, "*.txt");
            Logger.Info("Se encontraron " + archivos.Count() + " Archivos");
            this.Invoke(new Action(() =>
                                       {
                                           this.lblStatus.Text = "Se encontraron " + archivos.Count() +
                                                                 " Archivos";
                                           this.ProgressBar.Maximum = archivos.Count();
                                           this.ProgressBar.Minimum = 0;
                                       }));
            int i = 0;
            foreach (var archivo in archivos)
            {
                Logger.Info(archivo);

                Interlocked.Increment(ref i);
                this.Invoke(new Action(() =>
                {
                    this.ProgressBar.Value = i;
                }));
                this.Invoke(new Action(() =>
                {

                    this.lblStatus.Text = "Procesando " + archivo;

                }));
                List<Comprobante> x = null;
                try
                {
                    ParserTxtAndamios p = new ParserTxtAndamios();
                    if (radioTipo1.Checked)
                        p.esFactura = true;
                    else if (radioTipo2.Checked)
                        p.esFactura = false;
                    x = p.Parse(archivo);

                }
                catch(ApplicationException apx)
                {
                    Logger.Error(apx);
                    if(apx.InnerException != null)
                    {
                        Logger.Error(apx.InnerException);
                        if (File.Exists(Path.Combine(txtError.Text, Path.GetFileName(archivo))))
                            File.Delete(Path.Combine(txtError.Text, Path.GetFileName(archivo)));
                        File.Move(archivo, Path.Combine(txtError.Text, Path.GetFileName(archivo)));
                    }
                }
                catch (Exception ee)
                {
                    Logger.Error(ee);
                    if (File.Exists(Path.Combine(txtError.Text, Path.GetFileName(archivo))))
                        File.Delete(Path.Combine(txtError.Text, Path.GetFileName(archivo)));
                    File.Move(archivo, Path.Combine(txtError.Text, Path.GetFileName(archivo)));
                }
                if (x == null)
                    continue;

                Dictionary<string, string> dSalida = new Dictionary<string, string>();
                foreach (var comprobante in x)
                {
                    try
                    {/*
                        if (txtEmisorRfc.Text.Equals("SID080303VE0"))
                        {

                            //Eliminar para produccion
                            comprobante.Emisor.rfc = "SID080303VE0";
                            comprobante.noCertificado = "00001000000104602374";
                            //Fin de Eliminar para produccion
                            comprobante.Emisor.nombre = "ANDAMIOS Y SERVICIOS REPRESENTACIONES, SA DE CV";
                            comprobante.Emisor.DomicilioFiscal = new GeneradorCfdi.t_UbicacionFiscal();
                            comprobante.Emisor.DomicilioFiscal.calle = "PENSYLVANIA";
                            comprobante.Emisor.DomicilioFiscal.noExterior = "46";
                            comprobante.Emisor.DomicilioFiscal.colonia = "PARQUE SAN ANDRES";
                            comprobante.Emisor.DomicilioFiscal.codigoPostal = "04040";
                            comprobante.Emisor.DomicilioFiscal.municipio = "COYOACAN";
                            comprobante.Emisor.DomicilioFiscal.pais = "Mexico";
                            comprobante.Emisor.DomicilioFiscal.estado = "DF";
                            //TODO Cambiar Datos para SIDETEC
                            comprobante.TelefonoEmisor = "5264-8447";
                            comprobante.Emisor.RegimenFiscal = new[]
                                                                   {
                                                                       new GeneradorCfdi.ComprobanteEmisorRegimenFiscal
                                                                           {Regimen = "General Personas Morales"}

                                                                   };
                        }
                        else*/ 
                       if (txtEmisorRfc.Text.Equals("ASR040203J74"))
                        {
                            // Emisor Hardcodeado
                            comprobante.Emisor.Rfc = "ASR040203J74";
                            comprobante.Emisor.Nombre = "ANDAMIOS Y SERVICIOS REPRESENTACIONES, SA DE CV";
                           /* 
                           comprobante.Emisor.DomicilioFiscal = new GeneradorCfdi.t_UbicacionFiscal();
                            comprobante.Emisor.DomicilioFiscal.calle = "PENSYLVANIA";
                            comprobante.Emisor.DomicilioFiscal.noExterior = "46";
                            comprobante.Emisor.DomicilioFiscal.colonia = "PARQUE SAN ANDRES";
                            comprobante.Emisor.DomicilioFiscal.codigoPostal = "04040";
                            comprobante.Emisor.DomicilioFiscal.municipio = "COYOACAN";
                            comprobante.Emisor.DomicilioFiscal.pais = "Mexico";
                            comprobante.Emisor.DomicilioFiscal.estado = "CDMX";
                            */ 
                            //TODO Cambiar Datos para ANDAMIOS
                            comprobante.TelefonoEmisor = "5668-6868";
                            comprobante.Emisor.RegimenFiscal = "General Personas Morales";
                            // Fin Emisor Hardcodeado
                        }
                        else if (txtEmisorRfc.Text.Equals("NLC091211KC6"))
                        {
                            // Emisor Hardcodeado
                            comprobante.Emisor.Rfc = "NLC091211KC6";
                            comprobante.Emisor.Nombre = "NT LINK COMUNICACIONES S.A. DE C.V.";
                            /*
                            comprobante.Emisor.DomicilioFiscal = new GeneradorCfdi.t_UbicacionFiscal();
                            comprobante.Emisor.DomicilioFiscal.calle = "Xicotencatl";
                            comprobante.Emisor.DomicilioFiscal.noExterior = "103";
                            comprobante.Emisor.DomicilioFiscal.colonia = "DEL CARMEN";
                            comprobante.Emisor.DomicilioFiscal.codigoPostal = "04100";
                            comprobante.Emisor.DomicilioFiscal.municipio = "COYOACAN";
                            comprobante.Emisor.DomicilioFiscal.pais = "Mexico";
                            comprobante.Emisor.DomicilioFiscal.estado = "CDMX";
                             */ 
                            //TODO Cambiar Datos para NTLink
                            comprobante.TelefonoEmisor = "5668-6868";
                            comprobante.Emisor.RegimenFiscal =  "General Personas Morales";
                            // Fin Emisor Hardcodeado
                        } else
                        {
                            throw new ApplicationException("Rfc no registrado");
                        }
                        dSalida.Add(comprobante.Folio,null);
                        comprobante.Certificado = Convert.ToBase64String(File.ReadAllBytes(txtCer.Text));
                        Generador gen = new Generador();
                        gen.GenerarCfd(comprobante, new X509Certificate2(txtCer.Text), txtKey.Text, txtPass.Text,txtEmisorUsuario.Text,txtEmisorContraseña.Text);
                        dSalida[comprobante.Folio] = comprobante.Complemento.timbreFiscalDigital.UUID;
                        File.WriteAllText(Path.Combine(txtSalida.Text, Path.GetFileNameWithoutExtension(archivo)) + "_" + comprobante.Folio + ".xml", comprobante.XmlString);
                        if(chkGeneraPdf.Checked)
                        {
                            if (comprobante.TipoDeComprobante == "E")
                                comprobante.Titulo = "Nota de Crédito";
                            else comprobante.Titulo = "Factura";
                            byte[] pdf = gen.GetPdfFromComprobante(comprobante, txtEmisorRfc.Text);
                            Thread.Sleep(5);
                            DoEscribePdf(Path.Combine(txtSalida.Text, Path.GetFileNameWithoutExtension(archivo)) + "_" + comprobante.Folio,pdf);
                        }
                        Logger.Info(comprobante.XmlString);
                    }
                    catch (FaultException fe)
                    {
                        Logger.Info(fe.Message);
                        dSalida[comprobante.Folio] = fe.Message;
                    }
                    catch (Exception fe)
                    {
                        Logger.Info(fe.Message);
                        dSalida[comprobante.Folio] = fe.Message;
                    }
                }
                Logger.Info("Escribiendo Respaldo");
                if (File.Exists(Path.Combine(txtRespaldo.Text, Path.GetFileName(archivo))))
                    File.Delete(Path.Combine(txtRespaldo.Text, Path.GetFileName(archivo)));
                File.Move(archivo, Path.Combine(txtRespaldo.Text, Path.GetFileName(archivo)));
                Logger.Info("Escribiendo Resumen");                
                // Valores para resumen
                foreach (KeyValuePair<string, string> kvp in dSalida)
                {
                    //// Original _RESUMEN
                    //File.AppendAllText(Path.Combine(txtSalida.Text, Path.GetFileNameWithoutExtension(archivo)) + "_RESUMEN.txt", string.Format("|{0}|{1}|{2}", kvp.Key, kvp.Value, Environment.NewLine));                    
                    if (radioTipo1.Checked)
                        File.AppendAllText(Path.Combine(txtSalida.Text, "fcfdi.txt"), string.Format("|{0}|{1}|{2}", kvp.Key, kvp.Value, Environment.NewLine));    
                    else if (radioTipo2.Checked)
                        File.AppendAllText(Path.Combine(txtSalida.Text, "ncfdi.txt"), string.Format("|{0}|{1}|{2}", kvp.Key, kvp.Value, Environment.NewLine));    
                }
                Logger.Info("Termino Archivo");
            }
            Logger.Info("Termino Lista de Archivos");
            this.Invoke(new Action(() =>
            {
                MessageBox.Show("Proceso Terminado");
            }));
            this.Invoke(new Action(() =>
            {
                this.ProgressBar.Value = 0;
            }));
        }



        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEntrada.Text) && 
                !string.IsNullOrEmpty(txtSalida.Text) && 
                !string.IsNullOrEmpty(txtRespaldo.Text) && 
                !string.IsNullOrEmpty(txtError.Text))
            {
                if (radioTipo1.Checked)
                {
                    if (File.Exists(Path.Combine(txtSalida.Text, "fcfdi.txt")))
                        File.Delete(Path.Combine(txtSalida.Text, "fcfdi.txt"));
                }
                else if (radioTipo2.Checked)
                {
                    if (File.Exists(Path.Combine(txtSalida.Text, "ncfdi.txt")))
                        File.Delete(Path.Combine(txtSalida.Text, "ncfdi.txt"));
                }
                if (Directory.Exists(txtEntrada.Text))
                {
                    Thread t = new Thread(ProcesarArchivos);
                    t.Start();
                    
                }
            }
            else
            {
                MessageBox.Show("Revisa los directorios");
            }

            
        }

        public static void DoEscribePdf(string ruta, byte[] pdf)
        {
            string pdfFile = ruta + ".pdf";
            File.WriteAllBytes(pdfFile, pdf);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.txtCer.Text = GetFileDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.txtKey.Text = GetFileDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Logger.Info("Iniciando");
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + "confAndamios.xml"))
            {
                XmlTextReader reader = new XmlTextReader(AppDomain.CurrentDomain.BaseDirectory + "\\" + "confAndamios.xml");
                XmlSerializer ser = new XmlSerializer(typeof(Configuracion));
                Configuracion con = (Configuracion) ser.Deserialize(reader);
                txtCer.Text = con.RutaCer;
                txtKey.Text = con.RutaKey;
                txtEntrada.Text = con.RutaEntrada;
                txtSalida.Text = con.RutaSalida;
                txtRespaldo.Text = con.RutaRespaldo;
                txtError.Text = con.RutaError;
                txtPass.Text = con.PasswordKey;
                chkGeneraPdf.Checked = con.GenerarPdf;
                txtEmisorRfc.Text = con.EmisorRfc;
                txtEmisorUsuario.Text = con.EmisorUsuario;
                txtEmisorContraseña.Text = con.EmisorContraseña;
                reader.Close();
            }
        }
    }
}
