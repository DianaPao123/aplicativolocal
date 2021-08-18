using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using log4net;
using log4net.Config;

namespace GeneradorCfdi
{
    class GeneradorCadenas 
    {
        private static XmlTextReader xsltReader;
        private static string xsl;
        private static StringReader xsltInput;
        private static XslCompiledTransform xsltTransform = new XslCompiledTransform();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GeneradorCadenas));

        public GeneradorCadenas()
        {
            XmlConfigurator.Configure();
            if (xsl == null)
            {
                var cwd = Environment.CurrentDirectory;
                try
                {
                    xsl = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory ,@"Resources\cadenaoriginal_3_3.xslt"));
                    Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Resources\";
                    xsltInput = new StringReader(xsl);
                    xsltReader = new XmlTextReader(xsltInput);
                    xsltTransform.Load(xsltReader);
                }
                catch (Exception exception)
                {
                    Logger.Error((exception.Message));
                }
                finally
                {
                    Environment.CurrentDirectory = cwd;
                }
            }
        }

        public string CadenaOriginal(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ArgumentException("Error", "xml");
            }
            StringReader xmlInput = new StringReader(xml);
            XmlTextReader xmlReader = new XmlTextReader(xmlInput);
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter transformedXml = new XmlTextWriter(stringWriter);
            try
            {
                xsltTransform.Transform(xmlReader, transformedXml);
            }
            catch (XmlException xmlEx)
            {
                Logger.Error(xmlEx.Message);
                throw;
            }
            catch (XsltException xsltEx)
            {
                Logger.Error(xsltEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw;
            }

            string res = HttpUtility.HtmlDecode(stringWriter.ToString());

            return res;
        }

    }
}

