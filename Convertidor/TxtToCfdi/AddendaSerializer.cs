using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using log4net;
using System.Xml;
using System.Xml.Linq;

namespace TxtToCfdi
{
    public class AddendaSerializer
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserNtLink));

        public static string GetXmlStringFromAddendaObject(object addenda, Type tipoAddenda, string prefijo, string ns)
        {
            XmlSerializer ser;
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            if (string.IsNullOrEmpty(prefijo))
            {
                ser = new XmlSerializer(tipoAddenda, ns);
            }
            else if (!string.IsNullOrEmpty(ns))
            {
                ser = new XmlSerializer(tipoAddenda);

                namespaces.Add(prefijo, ns);
                namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            }
            else
            {
                ser = new XmlSerializer(tipoAddenda);
            }

            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    var sw = new StreamWriter(memStream, Encoding.UTF8);
                    using (
                        XmlWriter xmlWriter = XmlWriter.Create(sw,
                                                               new XmlWriterSettings() { Indent = false, Encoding = Encoding.UTF8 }))
                    {
                        if (namespaces.Count > 0)
                            ser.Serialize(xmlWriter, addenda, namespaces);
                        else
                        {
                            ser.Serialize(xmlWriter, addenda);
                        }
                        string xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                        xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                        xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
                        return xml;
                    }
                }
            }
            catch (Exception ee)
            {

                Logger.Error(ee);
                return null;
            }
        }

        public static string GetXmlStringFromXElement(XElement xml)
        {
            return null;
        }
    }
}
