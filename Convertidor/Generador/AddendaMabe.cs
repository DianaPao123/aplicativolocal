
using System.Xml.Serialization;
using System.Xml.Schema;



/// <remarks/>
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1", IsNullable=false, ElementName = "Factura")]
public partial class Factura {

    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Namespaces ;

    [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
    public string xsiSchemaLocation = "https://recepcionfe.mabempresa.com/cfd/addenda/v1 https://recepcionfe.mabempresa.com/cfd/addenda/v1/mabev1.xsd";
        
    private FacturaMoneda monedaField;
    
    private FacturaProveedor proveedorField;
    
    private FacturaEntrega entregaField;
    
    private FacturaDetalle[] detallesField;
    
    private FacturaDescuentos descuentosField;
    
    private FacturaSubtotal subtotalField;
    
    private FacturaTraslado[] trasladosField;
    
    private FacturaRetencion[] retencionesField;
    
    private FacturaTotal totalField;
    
    private decimal versionField;
    
    private FacturaTipoDocumento tipoDocumentoField;
    
    private string folioField;
    
    private System.DateTime fechaField;
    
    private string ordenCompraField;
    
    private string referencia1Field;
    
    private string referencia2Field;

    public Factura()
    { 
        //Namespaces = new XmlSerializerNamespaces();
        //Namespaces.Add("mabe", "http://recepcionfe.mabempresa.com/cfd/addenda/v1");
        this.versionField = ((decimal)(1.0m));
    }
    
    /// <remarks/>
    public FacturaMoneda Moneda {
        get {
            return this.monedaField;
        }
        set {
            this.monedaField = value;
        }
    }
    
    /// <remarks/>
    public FacturaProveedor Proveedor {
        get {
            return this.proveedorField;
        }
        set {
            this.proveedorField = value;
        }
    }
    
    /// <remarks/>
    public FacturaEntrega Entrega {
        get {
            return this.entregaField;
        }
        set {
            this.entregaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Detalle", IsNullable=false)]
    public FacturaDetalle[] Detalles {
        get {
            return this.detallesField;
        }
        set {
            this.detallesField = value;
        }
    }
    
    /// <remarks/>
    public FacturaDescuentos Descuentos {
        get {
            return this.descuentosField;
        }
        set {
            this.descuentosField = value;
        }
    }
    
    /// <remarks/>
    public FacturaSubtotal Subtotal {
        get {
            return this.subtotalField;
        }
        set {
            this.subtotalField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Traslado", IsNullable=false)]
    public FacturaTraslado[] Traslados {
        get {
            return this.trasladosField;
        }
        set {
            this.trasladosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Retencion", IsNullable=false)]
    public FacturaRetencion[] Retenciones {
        get {
            return this.retencionesField;
        }
        set {
            this.retencionesField = value;
        }
    }
    
    /// <remarks/>
    public FacturaTotal Total {
        get {
            return this.totalField;
        }
        set {
            this.totalField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal version {
        get {
            return this.versionField;
        }
        set {
            this.versionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public FacturaTipoDocumento tipoDocumento {
        get {
            return this.tipoDocumentoField;
        }
        set {
            this.tipoDocumentoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string folio {
        get {
            return this.folioField;
        }
        set {
            this.folioField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
    public System.DateTime fecha {
        get {
            return this.fechaField;
        }
        set {
            this.fechaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ordenCompra {
        get {
            return this.ordenCompraField;
        }
        set {
            this.ordenCompraField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string referencia1 {
        get {
            return this.referencia1Field;
        }
        set {
            this.referencia1Field = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string referencia2 {
        get {
            return this.referencia2Field;
        }
        set {
            this.referencia2Field = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://recepcionfe.mabempresa.com/cfd/addenda/v1" )]

public partial class FacturaMoneda {
    
    private FacturaMonedaTipoMoneda tipoMonedaField;
    
    private decimal tipoCambioField;
    
    private bool tipoCambioFieldSpecified;
    
    private string importeConLetraField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public FacturaMonedaTipoMoneda tipoMoneda {
        get {
            return this.tipoMonedaField;
        }
        set {
            this.tipoMonedaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal tipoCambio {
        get {
            return this.tipoCambioField;
        }
        set {
            this.tipoCambioField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool tipoCambioSpecified {
        get {
            return this.tipoCambioFieldSpecified;
        }
        set {
            this.tipoCambioFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string importeConLetra {
        get {
            return this.importeConLetraField;
        }
        set {
            this.importeConLetraField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
public enum FacturaMonedaTipoMoneda {
    
    /// <remarks/>
    MXN,
    
    /// <remarks/>
    USD,
    
    /// <remarks/>
    YEN,
    
    /// <remarks/>
    VEF,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
public partial class FacturaProveedor {
    
    private string codigoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string codigo {
        get {
            return this.codigoField;
        }
        set {
            this.codigoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
public partial class FacturaEntrega {
    
    private string plantaEntregaField;
    
    private string calleField;
    
    private string noExteriorField;
    
    private string noInteriorField;
    
    private string codigoPostalField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string plantaEntrega {
        get {
            return this.plantaEntregaField;
        }
        set {
            this.plantaEntregaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string calle {
        get {
            return this.calleField;
        }
        set {
            this.calleField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string noExterior {
        get {
            return this.noExteriorField;
        }
        set {
            this.noExteriorField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string noInterior {
        get {
            return this.noInteriorField;
        }
        set {
            this.noInteriorField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string codigoPostal {
        get {
            return this.codigoPostalField;
        }
        set {
            this.codigoPostalField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
public partial class FacturaDetalle {
    
    private string noLineaArticuloField;
    
    private string codigoArticuloField;
    
    private string descripcionField;
    
    private string unidadField;
    
    private decimal cantidadField;
    
    private decimal precioSinIvaField;
    
    private decimal precioConIvaField;
    
    private bool precioConIvaFieldSpecified;
    
    private decimal importeSinIvaField;
    
    private decimal importeConIvaField;
    
    private bool importeConIvaFieldSpecified;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
    public string noLineaArticulo {
        get {
            return this.noLineaArticuloField;
        }
        set {
            this.noLineaArticuloField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string codigoArticulo {
        get {
            return this.codigoArticuloField;
        }
        set {
            this.codigoArticuloField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string descripcion {
        get {
            return this.descripcionField;
        }
        set {
            this.descripcionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string unidad {
        get {
            return this.unidadField;
        }
        set {
            this.unidadField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal cantidad {
        get {
            return this.cantidadField;
        }
        set {
            this.cantidadField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal precioSinIva {
        get {
            return this.precioSinIvaField;
        }
        set {
            this.precioSinIvaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal precioConIva {
        get {
            return this.precioConIvaField;
        }
        set {
            this.precioConIvaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool precioConIvaSpecified {
        get {
            return this.precioConIvaFieldSpecified;
        }
        set {
            this.precioConIvaFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal importeSinIva {
        get {
            return this.importeSinIvaField;
        }
        set {
            this.importeSinIvaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal importeConIva {
        get {
            return this.importeConIvaField;
        }
        set {
            this.importeConIvaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool importeConIvaSpecified {
        get {
            return this.importeConIvaFieldSpecified;
        }
        set {
            this.importeConIvaFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
public partial class FacturaDescuentos {
    
    private FacturaDescuentosTipo tipoField;
    
    private string descripcionField;
    
    private decimal importeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public FacturaDescuentosTipo tipo {
        get {
            return this.tipoField;
        }
        set {
            this.tipoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string descripcion {
        get {
            return this.descripcionField;
        }
        set {
            this.descripcionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal importe {
        get {
            return this.importeField;
        }
        set {
            this.importeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
public enum FacturaDescuentosTipo {
    
    /// <remarks/>
    NA,
    
    /// <remarks/>
    CARGO,
    
    /// <remarks/>
    DESCUENTO,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
public partial class FacturaSubtotal {
    
    private decimal importeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal importe {
        get {
            return this.importeField;
        }
        set {
            this.importeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
public partial class FacturaTraslado {
    
    private string tipoField;
    
    private decimal tasaField;
    
    private decimal importeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string tipo {
        get {
            return this.tipoField;
        }
        set {
            this.tipoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal tasa {
        get {
            return this.tasaField;
        }
        set {
            this.tasaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal importe {
        get {
            return this.importeField;
        }
        set {
            this.importeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
public partial class FacturaRetencion {
    
    private string tipoField;
    
    private decimal tasaField;
    
    private decimal importeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string tipo {
        get {
            return this.tipoField;
        }
        set {
            this.tipoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal tasa {
        get {
            return this.tasaField;
        }
        set {
            this.tasaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal importe {
        get {
            return this.importeField;
        }
        set {
            this.importeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
public partial class FacturaTotal {
    
    private decimal importeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal importe {
        get {
            return this.importeField;
        }
        set {
            this.importeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://recepcionfe.mabempresa.com/cfd/addenda/v1")]
public enum FacturaTipoDocumento {
    
    /// <remarks/>
    FACTURA,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("NOTA CREDITO")]
    NOTACREDITO,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("NOTA CARGO")]
    NOTACARGO,
}
