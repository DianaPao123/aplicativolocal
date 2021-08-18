using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ConvertidorNtlink;
using GeneradorCfdi;
using GeneradorCfdi.ServicioValidador;
using TxtToCfdi;
using log4net;
using log4net.Config;
//using ComprobanteTipoDeComprobante = GeneradorCfdi.ComprobanteTipoDeComprobante;
using Timer = System.Threading.Timer;
using GeneradorCfdi.ServicioTimbrado;
using System.Configuration;


namespace ConvertidorCfdi
{
    public partial class MainForm : Form
    {
        private bool ac;
        private static ILog Logger = LogManager.GetLogger(typeof(FrmEmisor));
        private Poller _poller = null;
        private List<Configuracion> _emisores; 
        public MainForm()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
            Activo();
          
        }


        public void Activo()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + "KEY.txt"))
            {
                var lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + "KEY.txt");
                if (lines.Length < 1)
                { btnIniciar.Enabled = false; ac = false; }
                else
                {
                    var datos = lines[0].Split('-');
                    int x = datos.Count();
                    if (x == 5)
                    { btnIniciar.Enabled = true; ac = true; }
                    else
                    { btnIniciar.Enabled = false; ac = false; }
                }
            }
            else
            { btnIniciar.Enabled = false; ac = false; }

        }
       
        
        
       

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtEntrada.Text) && 
            //    !string.IsNullOrEmpty(txtSalida.Text) && 
            //    !string.IsNullOrEmpty(txtRespaldo.Text) && 
            //    !string.IsNullOrEmpty(txtError.Text))
            //{
            //    if (Directory.Exists(txtEntrada.Text))
            //    {
            //        if (ValidarInput())
            //        {
            //            XmlSerializer ser = new XmlSerializer(typeof(List<Configuracion>));
            //            Configuracion conf = new Configuracion
            //            {
            //                RutaCer = txtCer.Text,
            //                RutaKey = txtKey.Text,
            //                RutaEntrada = txtEntrada.Text,
            //                RutaSalida = txtSalida.Text,
            //                RutaError = txtError.Text,
            //                RutaRespaldo = txtRespaldo.Text,
            //                PasswordKey = txtPass.Text,
            //                GenerarPdf = chkGeneraPdf.Checked,
            //                RutaLogo = txtLogo.Text,
            //                EmailUserName = txtUsuarioMail.Text,
            //                EnviarCorreo = cbMail.Checked,
            //                EmailPassword = txtPassMail.Text,
            //                EmailServer = txtServidorMail.Text,
            //                EmailPort = txtPuertoMail.Text,
            //                Remitente = txtRemitente.Text
            //            };
            //            int segundos;
            //            if (int.TryParse(txtSegundos.Text, out segundos))
            //                conf.Segundos = segundos;

            //            XmlTextWriter txt = new XmlTextWriter(AppDomain.CurrentDomain.BaseDirectory + "\\" + "conf.xml", Encoding.UTF8);
            //            iniciarToolStripMenuItem.Enabled = false;
            //            detenerToolStripMenuItem.Enabled = true;
            //            menuItemStatus.Text = "Monitoreando ";
            //            ser.Serialize(txt, conf);
            //            txt.Close();
            //            if (_poller != null && _poller.Activo)
            //            {    
            //                _poller.Detener();
            //                _poller = null;
            //            }

            //            _poller = new Poller(txtEntrada.Text);
            //            _poller.Segundos = conf.Segundos == 0 ? 10 : conf.Segundos;
            //            _poller.Trabajador = ProcesarLista;
            //            try
            //            {
            //                _poller.Iniciar();
            //                btnDetener.Enabled = true;
            //                btnIniciar.Enabled = false;
            //            }
            //            catch (Exception eee)
            //            {
            //                Logger.Error(eee);
            //                if (eee.InnerException!= null)
            //                    Logger.Error(eee.InnerException);
            //            }
                        
            //        }

                   

            //    }
                
            //}
            //else
            //{
            //    MessageBox.Show("Revisa los directorios");
            //}

            
        }




        private void MainForm_Load(object sender, EventArgs e)
        {
            Activo();
        
            try
            {
                timer1.Enabled = true;
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + "conf.xml"))
                {
                    XmlTextReader reader = new XmlTextReader(AppDomain.CurrentDomain.BaseDirectory + "\\" + "conf.xml");
                    XmlSerializer ser = new XmlSerializer(typeof(List<Configuracion>));
                    _emisores = (List<Configuracion>)ser.Deserialize(reader);
                    if (_emisores == null)
                        _emisores = new List<Configuracion>();
                    reader.Close();
                    gridEmisores.AutoGenerateColumns = false;
                    gridEmisores.DataSource = _emisores;
                    
                    gridEmisores.ReadOnly = true;
                }
                else
                {
                    _emisores = new List<Configuracion>();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
            }
            
        }

       
        private void cbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            //if (_poller != null && _poller.Activo)
            //{
            //    _poller.Detener();

            //}
            //iniciarToolStripMenuItem.Enabled = true;
            //detenerToolStripMenuItem.Enabled = false;
            //menuItemStatus.Text = "Detenido ";
        }

     

        private void btnDetener_Click(object sender, EventArgs e)
        {

            //iniciarToolStripMenuItem.Enabled = true;
            //detenerToolStripMenuItem.Enabled = false;
            //menuItemStatus.Text = "Detenido ";
            //btnIniciar.Enabled = true;
            //btnDetener.Enabled = false;
        }

        


        private void MainForm_Shown(object sender, EventArgs e)
        {
           
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Configuracion conf = new Configuracion();
            FrmEmisor emisor = new FrmEmisor(conf);
            emisor.Text = "Agregar Emisor";
            emisor.ShowDialog();
            if (emisor.Ok)
                _emisores.Add(emisor.Emisor);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                XmlTextWriter writer = new XmlTextWriter(AppDomain.CurrentDomain.BaseDirectory + "\\" + "conf.xml",
                                                         new UTF8Encoding(false));
                XmlSerializer ser = new XmlSerializer(typeof (List<Configuracion>));
                ser.Serialize(writer, _emisores);
                writer.Close();

            }
            catch (Exception ee)
            {

                Logger.Error(ee);
            }

        }

        private void gridEmisores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 3) //editar
                {
                    var emisor = gridEmisores.Rows[e.RowIndex].DataBoundItem as Configuracion;
                    FrmEmisor femisor = new FrmEmisor(emisor);
                    femisor.Text = emisor.EmisorRfc;
                    femisor.ShowDialog();
                    
                }
                if (e.ColumnIndex == 4) //Eliminar
                {
                    //var emisor = gridEmisores.Rows[e.RowIndex].DataBoundItem as Configuracion;
                    //_emisores.Remove(emisor);

                }
            }
            
        }

        private void StatusServicio()
        {
            try
            {
                ServiceController service = new ServiceController("ConvertidorNtLink");
                if (service.Status == ServiceControllerStatus.Running)
                {
                    lblStatus.Text = "Iniciado";
                    btnDetener.Enabled = true;
                    btnIniciar.Enabled = false;
                }
                else if (service.Status == ServiceControllerStatus.Paused)
                    lblStatus.Text = "Pausado";
                else if (service.Status == ServiceControllerStatus.Stopped)
                {
                    lblStatus.Text = "Detenido";
                    btnDetener.Enabled = false;
                    if(ac==true)
                    btnIniciar.Enabled = true;
                }
                    
                else if (service.Status == ServiceControllerStatus.StartPending)
                    lblStatus.Text = "Iniciando";
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                timer1.Enabled = false;
            }
        }

        private void btnDetener_Click_1(object sender, EventArgs e)
        {
            try
            {
                ServiceController service = new ServiceController("ConvertidorNtLink");
                if (service.Status != ServiceControllerStatus.Stopped &&
                    service.Status != ServiceControllerStatus.StopPending)
                {
                    service.Stop();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
            }
        }

        private void btnIniciar_Click_1(object sender, EventArgs e)
        {
            try
            {
                ServiceController service = new ServiceController("ConvertidorNtLink");
                if (service.Status != ServiceControllerStatus.Running &&
                    service.Status != ServiceControllerStatus.StartPending)
                {
                    service.Start();
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            StatusServicio();
        }
        private List<Poller> _pollers;
        private void button2_Click(object sender, EventArgs e)
        {
           
        }



        

        

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var poller in _pollers)
            {
                poller.Detener();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Activar act = new Activar();
            act.ShowDialog(this);
            Activo();
        }
        


    }
}
