﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Serialization;
using GeneradorCfdi;

namespace ConvertidorCfdi
{

    /// <summary>
    /// Generador de los mensajes de cancelación para NT LINK 
    /// </summary>
    public class GeneradorMensajeCancelacion
    {


        private static void SignXmlFile(string fileName, ref string signedContent, RSA key)
        {
            // Check the arguments.  
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (signedContent == null) throw new ArgumentNullException("signedFileName");
            if (key == null) throw new ArgumentNullException("key");

            // Create a new XML document.
            XmlDocument doc = new XmlDocument();
            // Format the document to ignore white spaces.
            doc.PreserveWhitespace = false;
            // Load the passed XML file using it's name.
            //doc.Load(new XmlTextReader(FileName));
            doc.LoadXml(fileName);
            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(doc);
            // Add the key to the SignedXml document. 
            signedXml.SigningKey = key;
            // Get the signature object from the SignedXml object.
            Signature XMLSignature = signedXml.Signature;
            // Create a reference to be signed.  Pass "" 
            // to specify that all of the current XML
            // document should be signed.
            Reference reference = new Reference("");
            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);
            // Add the Reference object to the Signature object.
            XMLSignature.SignedInfo.AddReference(reference);
            // Add an RSAKeyValue KeyInfo (optional; helps recipient find key to validate).
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new RSAKeyValue((RSA)key));
            // Add the KeyInfo object to the Reference object.
            XMLSignature.KeyInfo = keyInfo;
            // Compute the signature.
            signedXml.ComputeSignature();
            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();
            // Append the element to the XML document.
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

            if (doc.FirstChild is XmlDeclaration)
            {
                doc.RemoveChild(doc.FirstChild);
            }

            signedContent = doc.InnerXml;
        }

        /// <summary>
        /// Genera el mensaje de cancelacion
        /// </summary>
        /// <param name="uuids">UUIDs de los comprobantes a cancelar</param>
        /// <param name="rutaCer">Ruta del certificado del emisor (.cer)</param>
        /// <param name="rutaKey">Ruta de la llave privada del emisor (.key)</param>
        /// <param name="passwordLlave">Password de la llave privada</param>
        /// <param name="rfcEmisor">RFC del emisor</param>
        /// <returns></returns>
        public static string GetMensajeCancelacion(List<string> uuids, string rutaCer, string rutaKey,
                                                   string passwordLlave, string rfcEmisor)
        {
            string fecha = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
            var encLMetadata = new Encabezado(rfcEmisor, fecha, uuids);
            string str = "<?xml version=\"1.0\"?>" +
                         "<Cancelacion xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                         "RfcEmisor=\"" + rfcEmisor + "\" Fecha=\"" + fecha +
                         "\" xmlns=\"http://cancelacfd.sat.gob.mx\">";

            str = encLMetadata.LisMListaFolios.Cast<object>().Aggregate(str,
                                                                        (current, elemento) =>
                                                                        current +
                                                                        ("<Folios>" + "<UUID>" + elemento.ToString() +
                                                                         "</UUID>" + "</Folios>"));

            str += "</Cancelacion>";
            string strPXmlFirmado = "";
            //Armamos el xml a firmar//
            SignXmlFile(str, ref strPXmlFirmado,
                        OpensslKey.DecodeEncryptedPrivateKeyInfo(File.ReadAllBytes(rutaKey), passwordLlave));

            var a = new SignatureType();
            var reference = new ReferenceType { URI = "" };
            var env = new XmlDsigEnvelopedSignatureTransform();
            a.SignedInfo = new SignedInfoType { Reference = reference };
            string hex = strPXmlFirmado.Substring(strPXmlFirmado.IndexOf("<DigestValue>") + 13,
                                                  strPXmlFirmado.IndexOf("</DigestValue>") -
                                                  strPXmlFirmado.IndexOf("<DigestValue>") - 13);
            a.SignedInfo.Reference.DigestValue = Convert.FromBase64String(hex);
            a.SignedInfo.Reference.DigestMethod = new DigestMethodType
            {
                Algorithm = "http://www.w3.org/2000/09/xmldsig#sha1"
            };
            a.SignedInfo.Reference.Transforms = new TransformType[] { new TransformType() };
            a.SignedInfo.Reference.Transforms[0].Algorithm = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";
            a.SignedInfo.CanonicalizationMethod = new CanonicalizationMethodType
            {
                Algorithm = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315"
            };
            a.SignedInfo.SignatureMethod = new SignatureMethodType
            {
                Algorithm = "http://www.w3.org/2000/09/xmldsig#rsa-sha1"
            };
            a.SignatureValue =
                Convert.FromBase64String(strPXmlFirmado.Substring(strPXmlFirmado.IndexOf("<SignatureValue>") + 16,
                                                                  strPXmlFirmado.IndexOf("</SignatureValue>") -
                                                                  strPXmlFirmado.IndexOf("<SignatureValue>") - 16));

            var x509 = new X509Certificate2(rutaCer);

            a.KeyInfo = new KeyInfoType();
            a.KeyInfo.X509Data = new X509DataType();
            a.KeyInfo.X509Data.X509IssuerSerial = new X509IssuerSerialType();
            a.KeyInfo.X509Data.X509IssuerSerial.X509IssuerName = x509.IssuerName.Name.ToString();
            // "OID.1.2.840.113549.1.9.2=Responsable: Héctor Ornelas Arciga, OID.2.5.4.45=SAT970701NN3, L=Coyoacán, S=Distrito Federal, C=MX, PostalCode=06300, STREET=\"Av. Hidalgo 77, Col. Guerrero\", E=asisnet@pruebas.sat.gob.mx, OU=Administración de Seguridad de la Información, O=Servicio de Administración Tributaria, CN=A.C. de pruebas";
            a.KeyInfo.X509Data.X509IssuerSerial.X509SerialNumber = x509.SerialNumber;
            // "286524172099382162235533054511188021807345578801";
            a.KeyInfo.X509Data.X509Certificate = File.ReadAllBytes(rutaCer);
            var mensajeCancelacion = new Cancelacion
            {
                RfcEmisor = encLMetadata.RfcEmisor,
                Fecha = Convert.ToDateTime(encLMetadata.Fecha),
                Signature = a
            };

            mensajeCancelacion.Folios = new CancelacionFolios[encLMetadata.LisMListaFolios.Count];

            for (int i = 0; i < encLMetadata.LisMListaFolios.Count; i++)
                mensajeCancelacion.Folios[i] = new CancelacionFolios() { UUID = encLMetadata.LisMListaFolios[i].ToString() };

            if (mensajeCancelacion.Folios.Count() > 0)
            {
                XmlSerializer ser = new XmlSerializer(typeof(Cancelacion), "http://cancelacfd.sat.gob.mx");
                MemoryStream ms = new MemoryStream();
                XmlTextWriter tw = new XmlTextWriter(ms, Encoding.UTF8);
                ser.Serialize(tw, mensajeCancelacion);

                var strCancela = Encoding.UTF8.GetString(ms.GetBuffer());
                strCancela = strCancela.Substring(strCancela.IndexOf(Convert.ToChar(60)));
                strCancela = strCancela.Substring(0, (strCancela.LastIndexOf(Convert.ToChar(62)) + 1));
                return strCancela;

            }
            return null;

        }
    }
}
