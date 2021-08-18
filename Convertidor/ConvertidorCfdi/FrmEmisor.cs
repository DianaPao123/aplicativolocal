using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace ConvertidorCfdi
{
    public partial class FrmEmisor : Form
    {


        private static ILog Logger = LogManager.GetLogger(typeof(FrmEmisor));

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



        private bool ValidarInput()
        {
            if (string.IsNullOrEmpty(txtUsuarioNtlink.Text))
            {
                MessageBox.Show("Escribe el usuario proporcionado por ntlink");
                return false;
            }
            if (string.IsNullOrEmpty(txtContraseñaNtlink.Text))
            {
                MessageBox.Show("Escribe la contraseña proporcionada por ntlink");
                return false;
            }
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
            //-
            if(string.IsNullOrEmpty(txtEntradaC.Text))
            {
                MessageBox.Show("Escribe el directorio de entrada de Cancelar");
                return false;
            }
            if (string.IsNullOrEmpty(txtSalidaC.Text))
            {
                MessageBox.Show("Escribe el directorio de salida de Cancelar");
                return false;
            }
           
            if (txtEntradaC.Text == txtSalidaC.Text                 
                || txtEntrada.Text == txtEntradaC.Text 
                || txtSalida.Text == txtSalidaC.Text )
            {
                MessageBox.Show("Los directorios no pueden ser los mismos");
                return false;
            }
            if (txtEntrada.Text == txtSalida.Text || txtRespaldo.Text == txtError.Text
                || txtEntrada.Text == txtError.Text || txtSalida.Text == txtError.Text
                || txtRespaldo.Text == txtSalida.Text)
            {
                MessageBox.Show("Los directorios no pueden ser los mismos");
                return false;
            }
            if (!Directory.Exists(txtEntrada.Text))
            {
                MessageBox.Show("No existe la ruta de entrada");
                return false;
            }
            if (!Directory.Exists(txtSalida.Text))
            {
                MessageBox.Show("No existe la ruta de salida");
                return false;
            }
            if (!Directory.Exists(txtRespaldo.Text))
            {
                MessageBox.Show("No existe la ruta de respaldo");
                return false;
            }
            if (!Directory.Exists(txtError.Text))
            {
                MessageBox.Show("No existe la ruta de error");
                return false;
            }

            return true;
        }



        public FrmEmisor()
        {
            InitializeComponent();
        }


        public FrmEmisor(Configuracion emisor)
        {
            InitializeComponent();
            this.Emisor = emisor;
            TxtRfc.Text = _emisor.EmisorRfc;
            txtCer.Text = _emisor.RutaCer;
            txtKey.Text = _emisor.RutaKey;
            txtEntrada.Text = _emisor.RutaEntrada;
            txtSalida.Text = _emisor.RutaSalida;
            txtError.Text = _emisor.RutaError;
            txtRespaldo.Text = _emisor.RutaRespaldo;
            txtPass.Text = _emisor.PasswordKey;
            chkGeneraPdf.Checked = _emisor.GenerarPdf;
            txtLogo.Text = _emisor.RutaLogo;
            txtUsuarioMail.Text = _emisor.EmailUserName;
            cbMail.Checked = _emisor.EnviarCorreo;
            txtPassMail.Text = _emisor.EmailPassword;
            txtServidorMail.Text = _emisor.EmailServer;
            txtPuertoMail.Text = _emisor.EmailPort;
            txtRemitente.Text = _emisor.Remitente;
            txtSegundos.Text = _emisor.Segundos.ToString();
            TxtConexion.Text = _emisor.CadenaConexion;
            //-
            txtEntradaC.Text = _emisor.RutaEntradaCancelar;
            txtErrorCancelar.Text = _emisor.RutaErrorCancelar;
            txtSalidaC.Text = _emisor.RutaSalidaCancelar;

            txtSalida2.Text = _emisor.SalidaAdicional;
            cbTipo.SelectedItem = _emisor.TipoEntrada;
            //cbTipo.SelectedItem = "CFD 2.2";
            txtContraseñaNtlink.Text = _emisor.EmisorContraseña;
            txtUsuarioNtlink.Text = _emisor.EmisorUsuario;

        }

        private Configuracion _emisor;
        public bool Ok;

        public Configuracion Emisor
        {
            get { return _emisor; }
            set { _emisor = value; }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Ok = false;
            Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidarInput())
            {
                _emisor.EmisorRfc = TxtRfc.Text;
                _emisor.RutaCer = txtCer.Text;
                _emisor.RutaKey = txtKey.Text;
                _emisor.RutaEntrada = txtEntrada.Text;
                _emisor.RutaSalida = txtSalida.Text;
                _emisor.RutaError = txtError.Text;
                _emisor.RutaRespaldo = txtRespaldo.Text;
                _emisor.PasswordKey = txtPass.Text;
                _emisor.GenerarPdf = chkGeneraPdf.Checked;
                _emisor.RutaLogo = txtLogo.Text;
                _emisor.EmailUserName = txtUsuarioMail.Text;
                _emisor.EnviarCorreo = cbMail.Checked;
                _emisor.EmailPassword = txtPassMail.Text;
                _emisor.EmailServer = txtServidorMail.Text;
                _emisor.EmailPort = txtPuertoMail.Text;
                _emisor.Remitente = txtRemitente.Text;
                _emisor.Segundos = int.Parse(txtSegundos.Text);
                //-
                _emisor.RutaEntradaCancelar = txtEntradaC.Text;
                _emisor.RutaErrorCancelar = txtErrorCancelar.Text;
                _emisor.RutaSalidaCancelar = txtSalidaC.Text;
                _emisor.CadenaConexion = TxtConexion.Text;
                _emisor.TipoEntrada = cbTipo.SelectedItem.ToString();
                _emisor.SalidaAdicional = txtSalida2.Text;

                _emisor.EmisorUsuario = txtUsuarioNtlink.Text;
                _emisor.EmisorContraseña = txtContraseñaNtlink.Text;

                Ok = true;
                Close();
            }
            
        }

        private void FrmEmisor_Load(object sender, EventArgs e)
        {
            var pdf = ConfigurationManager.AppSettings["Pdf"];
            if (pdf == "false")
            {
                chkGeneraPdf.Visible = false;
                chkGeneraPdf.Checked = false;
                txtLogo.Visible = false;
                button7.Visible = false;
            }
            Logger.Info("Iniciando");
            //cbTipo.SelectedIndex = 1;
            //cbTipo.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtCer.Text = GetFileDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtKey.Text = GetFileDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtEntrada.Text = GetFolderDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtSalida.Text = GetFolderDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtRespaldo.Text = GetFolderDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtError.Text = GetFolderDialog();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtLogo.Text = GetFileDialog();
        }
        //-
        private void button8_Click(object sender, EventArgs e)
        {
            txtEntradaC.Text = GetFolderDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtSalidaC.Text = GetFolderDialog();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            txtErrorCancelar.Text = GetFolderDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtSalida2.Text = GetFolderDialog();
        }

        private void cbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }
        
    }
}
