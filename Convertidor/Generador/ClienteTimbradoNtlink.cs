using System;
using GeneradorCfdi.ServiceReferenceNtLink;
using log4net.Config;
using log4net;

namespace GeneradorCfdi
{
    public class ClienteTimbradoNtlink
    {
        private static ILog Logger = LogManager.GetLogger(typeof(ClienteTimbradoNtlink));


        /// <summary>
        /// Constructor por default
        /// </summary>
        public ClienteTimbradoNtlink()
        {
            XmlConfigurator.Configure();
        }


        /// <summary>
        /// Método para timbrar CFDi
        /// </summary>
        /// <param name="comprobante">el string en UTF-8 del cfdi</param>
        /// <returns>el string en UTF-8 con el comprobante timbrado</returns>
        public string TimbraCfdi(string comprobante,string usuario,string contraseña,string llave, string aplicacion)
        {
            Logger.Info(comprobante);
            CertificadorClient cliente = new CertificadorClient();
            try
            {
                
                return cliente.TimbraCfdi(comprobante,usuario,contraseña,llave,aplicacion);
            }
            catch (Exception ee)
            {
                Logger.Error(ee.Message);
                return null;
            }
           
        }

        public string Activar(string llave, string RFC)
        {
            CertificadorClient cliente = new CertificadorClient();
            try
            {
                return cliente.Activar(llave,RFC);
            }
            catch (Exception ee)
            {
                Logger.Error(ee.Message);
                return null;
            }
        

        }
        public string TimbraRetencion(string comprobante,string usuario ,string contraseña,string llave, string aplicacion)
        {
            Logger.Info(comprobante);
            CertificadorClient cliente = new CertificadorClient();
            try
            {

                return cliente.TimbraRetencion(comprobante,usuario,contraseña,llave,aplicacion);
            }
            catch (Exception ee)
            {
                Logger.Error(ee.Message);
                return null;
            }

        }
        /// <summary>
        /// Método
        /// </summary>
        /// <param name="uuid">UUID del comprobante que se va a cancelar</param>
        /// <param name="rfc">RFC del emisor</param>
        /// <returns>Regresa el acuse de cancelación</returns>
        public string CancelaCfdi(string uuid, string rfcEmisor,string expresion,string rfcReceptor)
        {
            CertificadorClient cliente = new CertificadorClient();
            try
            {
                return cliente.CancelaCfdi(uuid, rfcEmisor, expresion, rfcReceptor);
            }
            catch (Exception ee)
            {
                Logger.Error(ee.Message);
                return null;
            }

        }

    }
}
