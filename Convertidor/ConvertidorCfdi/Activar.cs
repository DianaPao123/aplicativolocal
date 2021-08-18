using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Net.NetworkInformation;
using System.IO;
using System.Security.Cryptography;
using GeneradorCfdi;

namespace ConvertidorCfdi
{
    public partial class Activar : Form
    {
        private string clave = "rgv123";
        
        public Activar()
        {
            InitializeComponent();
            label2.Text = "";
            label2.ForeColor = System.Drawing.Color.Black;
            Activo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var MAC = getMacAddress();
            String Mac =MAC[0].ToString();
            String Key = textBox2.Text + "-" + textBox3.Text + "-" + textBox4.Text + "-" + textBox5.Text + "-" + textBox6.Text;

            String llave=cifrar(Mac+"|"+Key);
            label2.Text = "Espere por favor...";
            label2.ForeColor = System.Drawing.Color.Black;
           
            ClienteTimbradoNtlink cliente = new ClienteTimbradoNtlink();
            var x=cliente.Activar(llave,textBox1.Text);
            
            if (x == "OK")
            {
              //  textBox1.Text = llave + "...." + descifrar(llave);

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + "KEY.txt"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\" + "KEY.txt");
                }

                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\" + "KEY.txt", Key.ToString());
                label2.Text = "Licencia Activa";
                label2.ForeColor = System.Drawing.Color.Black;
                button1.Enabled = false;
            }
            else
            {
                label2.Text = x;
                label2.ForeColor = System.Drawing.Color.Red;
            
            }

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
        public string descifrar(string cadena)
        {
            byte[] llave;
            byte[] arreglo = Convert.FromBase64String(cadena); // Arreglo donde guardaremos la cadena descovertida.
            // Ciframos utilizando el Algoritmo MD5.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();
            //Ciframos utilizando el Algoritmo 3DES.
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateDecryptor();
            byte[] resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
            tripledes.Clear();
            string cadena_descifrada = UTF8Encoding.UTF8.GetString(resultado); // Obtenemos la cadena
            return cadena_descifrada; // Devolvemos la cadena
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


        public void Activo()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + "KEY.txt"))
            {
                var lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + "KEY.txt");
                if (lines.Length < 1)
                { label2.Text = "Licencia no activa"; label2.ForeColor = System.Drawing.Color.Red; }
                else
                {
                    var datos = lines[0].Split('-');
                    int x = datos.Count();
                    if (x == 5)
                    { label2.Text = "Licencia activa"; label2.ForeColor = System.Drawing.Color.Black;
                       button1.Enabled = false;
                    }
                    else
                    { label2.Text = "Licencia no activa"; label2.ForeColor = System.Drawing.Color.Red; }
                }
            }
            else
            { label2.Text = "Licencia no activa"; label2.ForeColor = System.Drawing.Color.Red; }
           
        }
       


        private void Activar_Load(object sender, EventArgs e)
        {
            label2.Text = "";
            label2.ForeColor = System.Drawing.Color.Black;
            Activo();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if ((textBox3.TextLength == 1 || textBox3.TextLength == 0) && (e.KeyValue == 8 || e.KeyCode == Keys.Delete))
                textBox2.Focus();

            if (textBox3.TextLength == 4 && e.KeyValue != 8)
            {
                textBox4.Focus();
                char pressedCharacter = (char)e.KeyValue;
                textBox4.AppendText(pressedCharacter.ToString());

            }
        }

        
        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if ((textBox4.TextLength == 1 || textBox4.TextLength == 0) && (e.KeyValue == 8 || e.KeyCode == Keys.Delete))
                textBox3.Focus();
            if (textBox4.TextLength == 4 && e.KeyValue != 8)
            {
                textBox5.Focus();
                char pressedCharacter = (char)e.KeyValue;
                textBox5.AppendText(pressedCharacter.ToString());

            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if ((textBox5.TextLength == 1 || textBox5.TextLength == 0) && (e.KeyValue == 8 || e.KeyCode == Keys.Delete))
                textBox4.Focus();
            if (textBox5.TextLength == 4 && e.KeyValue != 8)
            {
                textBox6.Focus();
                char pressedCharacter = (char)e.KeyValue;
                textBox6.AppendText(pressedCharacter.ToString());

            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if ((textBox6.TextLength == 1 || textBox6.TextLength == 0) && (e.KeyValue == 8 || e.KeyCode == Keys.Delete))
                textBox5.Focus();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

       
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (textBox2.TextLength == 4 && e.KeyValue != 8)
            {
                textBox3.Focus();
                char pressedCharacter = (char)e.KeyValue;
                textBox3.AppendText(pressedCharacter.ToString());

            }
        }


       


      

      

      
    }
}
