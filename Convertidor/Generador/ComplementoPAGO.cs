using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GeneradorCfdi
{
    [Serializable()]
  public  class ComplementoPAGO
    {
        [DataMemberAttribute]
        public string fechaPago { get; set; }
        [DataMemberAttribute]
        public string formaDePagoP { get; set; }
         [DataMemberAttribute]
              
        public string monedaP { get; set; }
         [DataMemberAttribute]
       
        public decimal tipoCambioP { get; set; }
        [DataMemberAttribute]
         public decimal monto { get; set; }
         [DataMemberAttribute]
       
        public string numOperacion { get; set; }
         [DataMemberAttribute]
         public string rfcEmisorCtaOrd { get; set; }
         [DataMemberAttribute]
       
        public string nomBancoOrd { get; set; }
         [DataMemberAttribute]
         public string ctaOrdenante { get; set; }
         [DataMemberAttribute]
       
        public string rfcEmisorCtaBen { get; set; }
         [DataMemberAttribute]
       
        public string ctaBeneficiario { get; set; }
         [DataMemberAttribute]
       
        //private c_TipoCadenaPago tipoCadPagoField;
        public string tipoCadPago { get; set; }



    }
}
