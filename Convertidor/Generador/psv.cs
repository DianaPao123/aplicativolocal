using System.Xml.Serialization;

namespace PSV
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV", IsNullable = false)]
    public partial class Factura
    {

        private FacturaCancelaciones cancelacionesField;

        private FacturaMoneda monedaField;

        private FacturaProveedor proveedorField;

        private Locacion origenField;

        private FacturaDestino destinoField;

        private FacturaMedidas medidasField;

        private FacturaReferencias referenciasField;

        private FacturaSolicitante solicitanteField;

        private string[] notaField;

        private FacturaArchivo[] archivoField;

        private FacturaParte[] partesField;

       // private FacturaVersion versionField;
        private string versionField;

        private FacturaTipoDocumentoFiscal tipoDocumentoFiscalField;

        private FacturaTipoDocumentoVWM tipoDocumentoVWMField;

        private FacturaDivision divisionField;

        /// <remarks/>
        public FacturaCancelaciones Cancelaciones
        {
            get
            {
                return this.cancelacionesField;
            }
            set
            {
                this.cancelacionesField = value;
            }
        }

        /// <remarks/>
        public FacturaMoneda Moneda
        {
            get
            {
                return this.monedaField;
            }
            set
            {
                this.monedaField = value;
            }
        }

        /// <remarks/>
        public FacturaProveedor Proveedor
        {
            get
            {
                return this.proveedorField;
            }
            set
            {
                this.proveedorField = value;
            }
        }

        /// <remarks/>
        public Locacion Origen
        {
            get
            {
                return this.origenField;
            }
            set
            {
                this.origenField = value;
            }
        }

        /// <remarks/>
        public FacturaDestino Destino
        {
            get
            {
                return this.destinoField;
            }
            set
            {
                this.destinoField = value;
            }
        }

        /// <remarks/>
        public FacturaMedidas Medidas
        {
            get
            {
                return this.medidasField;
            }
            set
            {
                this.medidasField = value;
            }
        }

        /// <remarks/>
        public FacturaReferencias Referencias
        {
            get
            {
                return this.referenciasField;
            }
            set
            {
                this.referenciasField = value;
            }
        }

        /// <remarks/>
        public FacturaSolicitante Solicitante
        {
            get
            {
                return this.solicitanteField;
            }
            set
            {
                this.solicitanteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Nota")]
        public string[] Nota
        {
            get
            {
                return this.notaField;
            }
            set
            {
                this.notaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Archivo")]
        public FacturaArchivo[] Archivo
        {
            get
            {
                return this.archivoField;
            }
            set
            {
                this.archivoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Parte", IsNullable = false)]
        public FacturaParte[] Partes
        {
            get
            {
                return this.partesField;
            }
            set
            {
                this.partesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        //public FacturaVersion version
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public FacturaTipoDocumentoFiscal tipoDocumentoFiscal
        {
            get
            {
                return this.tipoDocumentoFiscalField;
            }
            set
            {
                this.tipoDocumentoFiscalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public FacturaTipoDocumentoVWM tipoDocumentoVWM
        {
            get
            {
                return this.tipoDocumentoVWMField;
            }
            set
            {
                this.tipoDocumentoVWMField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public FacturaDivision division
        {
            get
            {
                return this.divisionField;
            }
            set
            {
                this.divisionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public partial class FacturaCancelaciones
    {

        private string cancelaSustituyeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cancelaSustituye
        {
            get
            {
                return this.cancelaSustituyeField;
            }
            set
            {
                this.cancelaSustituyeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public partial class Locacion
    {

        private string codigoField;

        private string nombreField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codigo
        {
            get
            {
                return this.codigoField;
            }
            set
            {
                this.codigoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombre
        {
            get
            {
                return this.nombreField;
            }
            set
            {
                this.nombreField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public partial class FacturaMoneda
    {

        private string tipoMonedaField;

        private decimal tipoCambioField;

        private bool tipoCambioFieldSpecified;

        private string codigoImpuestoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tipoMoneda
        {
            get
            {
                return this.tipoMonedaField;
            }
            set
            {
                this.tipoMonedaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal tipoCambio
        {
            get
            {
                return this.tipoCambioField;
            }
            set
            {
                this.tipoCambioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool tipoCambioSpecified
        {
            get
            {
                return this.tipoCambioFieldSpecified;
            }
            set
            {
                this.tipoCambioFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codigoImpuesto
        {
            get
            {
                return this.codigoImpuestoField;
            }
            set
            {
                this.codigoImpuestoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public partial class FacturaProveedor
    {

        private string codigoField;

        private string nombreField;

        private string correoContactoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codigo
        {
            get
            {
                return this.codigoField;
            }
            set
            {
                this.codigoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombre
        {
            get
            {
                return this.nombreField;
            }
            set
            {
                this.nombreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string correoContacto
        {
            get
            {
                return this.correoContactoField;
            }
            set
            {
                this.correoContactoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public partial class FacturaDestino
    {

        private string codigoField;

        private string naveReciboMaterialField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codigo
        {
            get
            {
                return this.codigoField;
            }
            set
            {
                this.codigoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string naveReciboMaterial
        {
            get
            {
                return this.naveReciboMaterialField;
            }
            set
            {
                this.naveReciboMaterialField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public partial class FacturaMedidas
    {

        private decimal pesoBrutoField;

        private bool pesoBrutoFieldSpecified;

        private decimal pesoNetoField;

        private bool pesoNetoFieldSpecified;

        private decimal volumenField;

        private bool volumenFieldSpecified;

        private decimal numeroPiezasField;

        private bool numeroPiezasFieldSpecified;

        private string descripcionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal pesoBruto
        {
            get
            {
                return this.pesoBrutoField;
            }
            set
            {
                this.pesoBrutoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pesoBrutoSpecified
        {
            get
            {
                return this.pesoBrutoFieldSpecified;
            }
            set
            {
                this.pesoBrutoFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal pesoNeto
        {
            get
            {
                return this.pesoNetoField;
            }
            set
            {
                this.pesoNetoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pesoNetoSpecified
        {
            get
            {
                return this.pesoNetoFieldSpecified;
            }
            set
            {
                this.pesoNetoFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal volumen
        {
            get
            {
                return this.volumenField;
            }
            set
            {
                this.volumenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool volumenSpecified
        {
            get
            {
                return this.volumenFieldSpecified;
            }
            set
            {
                this.volumenFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal numeroPiezas
        {
            get
            {
                return this.numeroPiezasField;
            }
            set
            {
                this.numeroPiezasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool numeroPiezasSpecified
        {
            get
            {
                return this.numeroPiezasFieldSpecified;
            }
            set
            {
                this.numeroPiezasFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string descripcion
        {
            get
            {
                return this.descripcionField;
            }
            set
            {
                this.descripcionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public partial class FacturaReferencias
    {

        private string referenciaProveedorField;

        private string remisionField;

        private string numeroASNField;

        private string unidadNegociosField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string referenciaProveedor
        {
            get
            {
                return this.referenciaProveedorField;
            }
            set
            {
                this.referenciaProveedorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string remision
        {
            get
            {
                return this.remisionField;
            }
            set
            {
                this.remisionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string numeroASN
        {
            get
            {
                return this.numeroASNField;
            }
            set
            {
                this.numeroASNField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string unidadNegocios
        {
            get
            {
                return this.unidadNegociosField;
            }
            set
            {
                this.unidadNegociosField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public partial class FacturaSolicitante
    {

        private string nombreField;

        private string correoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nombre
        {
            get
            {
                return this.nombreField;
            }
            set
            {
                this.nombreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string correo
        {
            get
            {
                return this.correoField;
            }
            set
            {
                this.correoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public partial class FacturaArchivo
    {

        private string datosField;

        private FacturaArchivoTipo tipoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string datos
        {
            get
            {
                return this.datosField;
            }
            set
            {
                this.datosField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public FacturaArchivoTipo tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public enum FacturaArchivoTipo
    {

        /// <remarks/>
        XLS,

        /// <remarks/>
        PDF,

        /// <remarks/>
        ZIP,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public partial class FacturaParte
    {

        private FacturaParteReferencias referenciasField;

        private string[] notaField;

        private string posicionField;

        private string numeroMaterialField;

        private string descripcionMaterialField;

        private decimal cantidadMaterialField;

        private string unidadMedidaField;

        private decimal precioUnitarioField;

        private decimal montoLineaField;

        private decimal pesoBrutoField;

        private bool pesoBrutoFieldSpecified;

        private decimal pesoNetoField;

        private bool pesoNetoFieldSpecified;

        private string codigoImpuestoField;

        /// <remarks/>
        public FacturaParteReferencias Referencias
        {
            get
            {
                return this.referenciasField;
            }
            set
            {
                this.referenciasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Nota")]
        public string[] Nota
        {
            get
            {
                return this.notaField;
            }
            set
            {
                this.notaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string posicion
        {
            get
            {
                return this.posicionField;
            }
            set
            {
                this.posicionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string numeroMaterial
        {
            get
            {
                return this.numeroMaterialField;
            }
            set
            {
                this.numeroMaterialField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string descripcionMaterial
        {
            get
            {
                return this.descripcionMaterialField;
            }
            set
            {
                this.descripcionMaterialField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal cantidadMaterial
        {
            get
            {
                return this.cantidadMaterialField;
            }
            set
            {
                this.cantidadMaterialField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string unidadMedida
        {
            get
            {
                return this.unidadMedidaField;
            }
            set
            {
                this.unidadMedidaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal precioUnitario
        {
            get
            {
                return this.precioUnitarioField;
            }
            set
            {
                this.precioUnitarioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal montoLinea
        {
            get
            {
                return this.montoLineaField;
            }
            set
            {
                this.montoLineaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal pesoBruto
        {
            get
            {
                return this.pesoBrutoField;
            }
            set
            {
                this.pesoBrutoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pesoBrutoSpecified
        {
            get
            {
                return this.pesoBrutoFieldSpecified;
            }
            set
            {
                this.pesoBrutoFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal pesoNeto
        {
            get
            {
                return this.pesoNetoField;
            }
            set
            {
                this.pesoNetoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pesoNetoSpecified
        {
            get
            {
                return this.pesoNetoFieldSpecified;
            }
            set
            {
                this.pesoNetoFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codigoImpuesto
        {
            get
            {
                return this.codigoImpuestoField;
            }
            set
            {
                this.codigoImpuestoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public partial class FacturaParteReferencias
    {

        private string ordenCompraField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ordenCompra
        {
            get
            {
                return this.ordenCompraField;
            }
            set
            {
                this.ordenCompraField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public enum FacturaVersion
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1.0")]
        Item10,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public enum FacturaTipoDocumentoFiscal
    {

        /// <remarks/>
        FA,

        /// <remarks/>
        CA,

        /// <remarks/>
        CR,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public enum FacturaTipoDocumentoVWM
    {

        /// <remarks/>
        PSV,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.vwnovedades.com/volkswagen/kanseilab/shcp/2009/Addenda/PSV")]
    public enum FacturaDivision
    {

        /// <remarks/>
        VW,

        /// <remarks/>
        INFODE,

        /// <remarks/>
        VWSP,
    }

}