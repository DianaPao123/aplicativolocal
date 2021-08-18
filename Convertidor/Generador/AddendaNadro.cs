
using System.Xml.Serialization;

namespace AddendaNadro
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Adenda
    {

        private AdendaDatosNadro[] datosNadroField;

        /// <remarks/>
         [System.Xml.Serialization.XmlElementAttribute("DatosNadro", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AdendaDatosNadro[] DatosNadro
        {
            get
            {
                return this.datosNadroField;
            }
            set
            {
                this.datosNadroField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AdendaDatosNadro
    {

        private string ordenField;

        private string plazoField;

        private string entregaEntranteField;

        private string posicionOCField;

        private string totalOCField;

        private string codEANField;

        /// <remarks/>
        public string Orden
        {
            get
            {
                return this.ordenField;
            }
            set
            {
                this.ordenField = value;
            }
        }

        /// <remarks/>
        public string Plazo
        {
            get
            {
                return this.plazoField;
            }
            set
            {
                this.plazoField = value;
            }
        }

        /// <remarks/>
        public string EntregaEntrante
        {
            get
            {
                return this.entregaEntranteField;
            }
            set
            {
                this.entregaEntranteField = value;
            }
        }

        /// <remarks/>
        public string PosicionOC
        {
            get
            {
                return this.posicionOCField;
            }
            set
            {
                this.posicionOCField = value;
            }
        }

        /// <remarks/>
        public string TotalOC
        {
            get
            {
                return this.totalOCField;
            }
            set
            {
                this.totalOCField = value;
            }
        }

        /// <remarks/>
        public string CodEAN
        {
            get
            {
                return this.codEANField;
            }
            set
            {
                this.codEANField = value;
            }
        }
    }
}
