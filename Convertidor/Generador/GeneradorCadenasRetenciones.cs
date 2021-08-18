using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using log4net;

namespace GeneradorCfdi
{
    class GeneradorCadenasRetenciones
    {
        private XmlTextReader xsltReader;
        private StringReader xsltInput;
        private XslCompiledTransform xsltTransform = new XslCompiledTransform();
        private static readonly ILog Log = LogManager.GetLogger(typeof(GeneradorCadenasRetenciones));

        class LocalFileResolver : XmlUrlResolver
        {
            public override Uri ResolveUri(Uri baseUri, string relativeUri)
            {

                return base.ResolveUri(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\\Resources\\"), relativeUri);
            }
        }

        public GeneradorCadenasRetenciones()
        {

            try
            {
                LocalFileResolver resolver = new LocalFileResolver();
                var xsl = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory ,@"Resources\retenciones.xslt"));
                xsltInput = new StringReader(xsl);
                xsltReader = new XmlTextReader(xsltInput);
                xsltTransform.Load(xsltReader, new XsltSettings(false, true), resolver);
            }
            catch (Exception exception)
            {
                Log.Error("Error(GeneradorCadenas):" + exception);
            }

        }

        public GeneradorCadenasRetenciones(string path)
        {
            var cwd = Environment.CurrentDirectory;
            try
            {
                var xsl = File.ReadAllText(path + "\\retenciones.xslt");
                Environment.CurrentDirectory = path;
                xsltInput = new StringReader(xsl);
                xsltReader = new XmlTextReader(xsltInput);
                xsltTransform.Load(xsltReader);
            }
            catch (Exception exception)
            {
                Log.Error("Error(GeneradorCadenas):" + exception);
            }
            finally
            {
                Environment.CurrentDirectory = cwd;
            }
        }

        public string CadenaOriginal(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("Archivo XML Inválido", "xml");
            }
            StringReader xmlInput = new StringReader(xml);

            XmlTextReader xmlReader = new XmlTextReader(xmlInput);
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter transformedXml = new XmlTextWriter(stringWriter);
            try
            {
                xsltTransform.Transform(xmlReader, transformedXml);
            }
            catch (Exception ex)
            {
                Log.Error("Error(CadenaOriginal)" + ex);
                throw;
            }
            return HttpUtility.HtmlDecode(stringWriter.ToString());
        }

    }

}
