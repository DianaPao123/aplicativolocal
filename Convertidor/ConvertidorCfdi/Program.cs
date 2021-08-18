using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommandLine;
using CommandLine.Text;
using GeneradorCfdi;
using TxtToCfdi;
using log4net;
using log4net.Config;
//using ComprobanteTipoDeComprobante = GeneradorCfdi.ComprobanteTipoDeComprobante;

namespace ConvertidorCfdi
{

    

    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            
        }
    }
}
