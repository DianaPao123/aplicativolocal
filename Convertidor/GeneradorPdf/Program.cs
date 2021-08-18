using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;
using GeneradorCfdi;
using TxtToCfdi;
using log4net;
using log4net.Config;
//using ComprobanteTipoDeComprobante = GeneradorCfdi.ComprobanteTipoDeComprobante;

namespace GeneradorPdf
{

    public class Opciones
    {
        [Option('x', "xml", Required = true, HelpText = "Archivo Xml")]
        public string Xml { get; set; }

        [Option('t', "txtLayout", Required = true, HelpText = "Archivo de layout .txt")]
        public string Txt { get; set; }

        [Option('l', "logo", DefaultValue = "", Required = false, HelpText = "Archivo de logo .png")]
        public string Logo { get; set; }

        [Option('s', "salida", Required = true, HelpText = "Nombre del archivo de salida .pdf")]
        public string Salida { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var options = new Opciones();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                List<Comprobante> x;
                try
                {
                    var parser = new ParserAmece71();
                    var comprobantes = parser.Parse(options.Txt);
                    if (comprobantes.Count > 0)
                    {
                        Comprobante comprobante = Generador.GetTimbre(File.ReadAllText(options.Xml),
                            comprobantes.First());
                        var gen = new GeneradorCfdi.Generador();
                        if (comprobante.TipoDeComprobante == "E")
                            comprobante.Titulo = "Nota de Crédito";
                        else comprobante.Titulo = "Factura";
                        byte[] pdf = null;
                        try
                        {
                            pdf = gen.GetPdfFromComprobante(comprobante, options.Logo);
                            // var ruta = Path.Combine(conf.RutaSalida, Path.GetFileNameWithoutExtension(archivo));
                            string pdfFile = options.Salida;
                            File.WriteAllBytes(pdfFile, pdf);
                        }
                        catch (Exception ee)
                        {
                            Logger.Error(ee);
                            Environment.Exit(1);
                        }
                    }
                }
                catch (Exception ee)
                {
                    Logger.Error(ee);
                }

            }
        }
    }
}
