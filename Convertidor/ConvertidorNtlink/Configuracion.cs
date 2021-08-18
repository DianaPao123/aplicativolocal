using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConvertidorCfdi
{
    [Serializable]
    public class Configuracion
    {
        

        public string Editar
        {
            get { return "Editar"; }
        }

        public string Eliminar
        {
            get { return "Eliminar"; }
        }

        public string CadenaConexion { get; set; }

        public string EmisorNombre { get; set; }
        public string SalidaAdicional { get; set; }
        
        public string EmisorRfc { get; set; }
        public string EmisorRazonSocial { get; set; }
        public string EmisorCalle { get; set; }
        public string EmisorNumeroInt { get; set; }
        public string EmisorNumeroExt { get; set; }
        public string EmisorColonia { get; set; }
        public string EmisorReferencia { get; set; }
        public string EmisorMunicipio { get; set; }
        public string EmisorPais { get; set; }
        public string EmisorEstado { get; set; }
        public string EmisorCp { get; set; }
        public string EmisorUsuario { get; set; }
        public string EmisorContraseña { get; set; }

        public string RutaKey { get; set; }
        public string RutaCer { get; set; }
        public string PasswordKey { get; set; }

        public string RutaEntrada { get; set; }
        public string TipoEntrada { get; set; }
        public string RutaSalida { get; set; }
        public string RutaError { get; set; }
        public string RutaRespaldo { get; set; }
        public string RutaLogo { get; set; }
        public bool GenerarPdf { get; set; }

        public int Segundos { get; set; }

        public bool EnviarCorreo { get; set; }

        public string EmailServer { get; set; }
        public string EmailUserName { get; set; }
        public string EmailPassword { get; set; }
        public string EmailPort { get; set; }
        public string Remitente { get; set; }

        public override string ToString()
        {
            return this.EmisorRfc;
        }
        //-
        public string RutaEntradaCancelar { get; set; }
        public string RutaErrorCancelar { get; set; }
        public string RutaSalidaCancelar { get; set; }
        public string RutaDbfs { get; set; }


    }
}
