using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using GeneradorCfdi;

namespace TxtToCfdi
{
    class Program
    {
        public static void Main(string[] args)
        {

            string archivo = "C:\\convertidor\\entrada\\01-NTLINK_con IVA.xlsx";
            MyExcel excel = new MyExcel();
            excel.read_file(archivo);

            // Lets test the Addenda modules for the Neto Layout

          //  var neto = new ParserSorianaExtemporanea();
          //  var neto = new ParserLiverpool();
            //var neto = new ParserBic();
           // var neto = new ParserLowes();
           // var neto = new ParserVallen();
           // var neto = new Parser1888();
            var neto = new ParserNtLink();

           /* var neto2 = new ParserRetenciones();
            string path2 = Directory.GetCurrentDirectory();
            path2 = path2.Replace("bin\\Debug", "") + "Ejemplos\\retenciones.txt";
            List<Retenciones> comprobantes2 = neto2.Parse(@path2);
            Generador gen2 = new Generador();
            gen2.GenerarCfdRetenciones(comprobantes2[0], new X509Certificate2("C:\\convertidor\\NLC091211KC6\\csd.cer"), "C:\\convertidor\\NLC091211KC6\\csd.key", "ntlinksat1");
           */

           // var neto = new ParserleyendasFisc();

            string path = Directory.GetCurrentDirectory();
            path = path.Replace("bin\\Debug", "") + "Ejemplos\\CartaPorte_Federal_R.txt";
          //  path = path.Replace("bin\\Debug", "") + "Ejemplos\\impuestoslocales.txt";
          
           List< Comprobante> comprobantes =neto.Parse(@path);
            Generador gen=new Generador();
            gen.GenerarCfd(comprobantes[0], new X509Certificate2("C:\\convertidor\\NLC091211KC6\\csd.cer"), "C:\\convertidor\\NLC091211KC6\\csd.key", "NtcSd2.2020", "ricardo.gomez@ntlink.com.mx", "Ricardin123#");

           var pdf = gen.GetPdfFromComprobante(comprobantes[0], "");
           var ruta = Path.Combine("C:\\convertidor\\", "pdf");
            string pdfFile = ruta + ".pdf";
            File.WriteAllBytes(pdfFile, pdf);
         

            //var comprobantes = neto.Parse(@"C:\Users\Elias\Source\Workspaces\NtLink\Convertidor\Convertidor\TxtToCfdi\Ejemplos\EjemploLiverpool.txt");
        }
    }
}
