using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GeneradorCfdi.ComplementoCartaPorte
{

    [Serializable()]
    public class ComplementoCP
    {
        [DataMemberAttribute]
        public string transpInternac{ get; set; }
        [DataMemberAttribute]
        public string entradaSalidaMerc{ get; set; }
        [DataMemberAttribute]
        public string viaEntradaSalida{ get; set; }
        [DataMemberAttribute]
        public string totalDistRec{ get; set; }
         
    }

    [Serializable()]
    public class UbicacionCP
    {
          [DataMemberAttribute]
          public string tipoEstacion{ get; set; }
          [DataMemberAttribute]
          public string distanciaRecorrida{ get; set; }
        //-------------------------------------
          [DataMemberAttribute]
          public string iDOrigen{ get; set; }
          [DataMemberAttribute]
          public string rFCRemitente{ get; set; }
         // [DataMemberAttribute]
         // public string nombreRemitente{ get; set; }
         // [DataMemberAttribute]
         // public string numRegIdTribOrigen{ get; set; }
         // [DataMemberAttribute]
         // public string residenciaFiscalOrigen{ get; set; }
          [DataMemberAttribute]
          public string numEstacionOrigen{ get; set; }
         //  [DataMemberAttribute]
         //  public string nombreEstacionOrigen{ get; set; }
         //  [DataMemberAttribute]
         //  public string navegacionTraficoOrigen{ get; set; }
           [DataMemberAttribute]
           public string fechaHoraSalida{ get; set; }
         //-----------------------------------
           [DataMemberAttribute]
           public string iDDestino{ get; set; }
           [DataMemberAttribute]
           public string rFCDestinatario{ get; set; }
          // [DataMemberAttribute]
           //public string nombreDestinatario{ get; set; }
          // [DataMemberAttribute]
          // public string numRegIdTribDestino{ get; set; }
         //  [DataMemberAttribute]
          // public string residenciaFiscalDestino{ get; set; }
           [DataMemberAttribute]
           public string numEstacionDestino{ get; set; }
         //  [DataMemberAttribute]
         //  public string nombreEstacionDestino{ get; set; }
         //  [DataMemberAttribute]
         //  public string navegacionTraficoDestino{ get; set; }
           [DataMemberAttribute]
           public string fechaHoraProgLlegada{ get; set; }
        //-------------------------------------------
          /* [DataMemberAttribute]
           public string calle{ get; set; }
           [DataMemberAttribute]
           public string numeroExterior{ get; set; }
           [DataMemberAttribute]
           public string numeroInterior{ get; set; }
           [DataMemberAttribute]
           public string colonia{ get; set; };
           [DataMemberAttribute]
           public string localidad{ get; set; }
           [DataMemberAttribute]
           public string referencia{ get; set; }
           [DataMemberAttribute]
           public string municipio{ get; set; }
          */ [DataMemberAttribute]
           public string estado{ get; set; }
           [DataMemberAttribute]
           public string pais{ get; set; }
           [DataMemberAttribute]
           public string codigoPostal{ get; set; }
        //----------------------------------------

    }
    [Serializable()]
    public class MercanciaCP
    {
        [DataMemberAttribute]
        public string pesoBrutoTotal { get; set; }
        [DataMemberAttribute]
        public string unidadPeso { get; set; }
        [DataMemberAttribute]
        public string pesoNetoTotal { get; set; }
        [DataMemberAttribute]
        public string numTotalMercancias { get; set; }
     //   [DataMemberAttribute]
    //    public string cargoPorTasacion { get; set; }
        //---------
        [DataMemberAttribute]
        public string bienesTransp { get; set; }
        [DataMemberAttribute]
        public string claveSTCC { get; set; }
        //[DataMemberAttribute]
        //public string descripcion { get; set; }
        //[DataMemberAttribute]
        //public string cantidad { get; set; }
        [DataMemberAttribute]
        public string claveUnidad { get; set; }
       // [DataMemberAttribute]
       // public string unidad { get; set; }
       // [DataMemberAttribute]
       // public string dimensiones { get; set; }
        [DataMemberAttribute]
        public string materialPeligroso { get; set; }
       // [DataMemberAttribute]
       // public string cveMaterialPeligroso { get; set; }
       // [DataMemberAttribute]
       // public string embalaje { get; set; }
       // [DataMemberAttribute]
      //  public string descripEmbalaje { get; set; }
        [DataMemberAttribute]
        public string pesoEnKg { get; set; }
        [DataMemberAttribute]
        public string valorMercancia { get; set; }
        [DataMemberAttribute]
        public string moneda { get; set; }
        [DataMemberAttribute]
        public string fraccionArancelaria { get; set; }
        [DataMemberAttribute]
        public string uUIDComercioExt { get; set; }
    }
    [Serializable()]
    public class AutotransporteFederalCP
    {
        [DataMemberAttribute]
        public string permSCT { get; set; }
        [DataMemberAttribute]
        public string numPermisoSCT { get; set; }
        [DataMemberAttribute]
        public string nombreAseg { get; set; }
        [DataMemberAttribute]
        public string numPolizaSeguro { get; set; }
        //--------
        [DataMemberAttribute]
        public string configVehicular { get; set; }
        [DataMemberAttribute]
        public string placaVM { get; set; }
        [DataMemberAttribute]
        public string anioModeloVM { get; set; }
    }


    [Serializable()]
    public class Operador
    {
        [DataMemberAttribute]
        public string rFCOperador { get; set; }
        [DataMemberAttribute]
        public string numLicencia { get; set; }
        [DataMemberAttribute]
        public string nombreOperador { get; set; }
        [DataMemberAttribute]
        public string numRegIdTribOperador { get; set; }
        [DataMemberAttribute]
        public string residenciaFiscalOperador { get; set; }
        //--
        [DataMemberAttribute]
        public string estado { get; set; }
        [DataMemberAttribute]
        public string pais { get; set; }
        [DataMemberAttribute]
        public string codigoPostal { get; set; }
    }

    [Serializable()]
    public class Propietario
    {
        [DataMemberAttribute]
        public string rFCPropietario { get; set; }
        [DataMemberAttribute]
        public string nombrePropietario { get; set; }
        [DataMemberAttribute]
        public string numRegIdTribPropietario { get; set; }
        [DataMemberAttribute]
        public string residenciaFiscalPropietario { get; set; }
        [DataMemberAttribute]
        //--
         public string estado { get; set; }
        [DataMemberAttribute]
        public string pais { get; set; }
        [DataMemberAttribute]
        public string codigoPostal { get; set; }


    }

    [Serializable()]
    public class Arrendatario
    {
        [DataMemberAttribute]
        public string rFCArrendatario { get; set; }
        [DataMemberAttribute]
        public string nombreArrendatario { get; set; }
        [DataMemberAttribute]
        public string numRegIdTribArrendatario { get; set; }
        [DataMemberAttribute]
        public string residenciaFiscalArrendatario { get; set; }
        //--
        [DataMemberAttribute]
        public string estado { get; set; }
        [DataMemberAttribute]
        public string pais { get; set; }
        [DataMemberAttribute]
        public string codigoPostal { get; set; }
    }
    [Serializable()]
    public class Notificado
    {
        [DataMemberAttribute]
        public string rFCNotificado { get; set; }
        [DataMemberAttribute]
        public string nombreNotificado { get; set; }
        [DataMemberAttribute]
        public string numRegIdTribNotificado { get; set; }
        [DataMemberAttribute]
        public string residenciaFiscalNotificado { get; set; }
        //--
        [DataMemberAttribute]
        public string estado { get; set; }
        [DataMemberAttribute]
        public string pais { get; set; }
        [DataMemberAttribute]
        public string codigoPostal { get; set; }
    }

}
