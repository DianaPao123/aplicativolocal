

using System.Xml.Serialization;


// 
namespace PPY
{


/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class factura {
    
    private facturaCancelaciones[] cancelacionesField;
    
    private facturaMoneda monedaField;
    
    private Locacion proveedorField;
    
    private Locacion origenField;
    
    private Locacion destinoField;
    
    private Locacion receivingField;
    
    private facturaProyecto proyectoField;
    
    private string[] notaField;
    
    private facturaCargosCreditos[] cargosCreditosField;
    
    private facturaOtrosCargos[] otrosCargosField;
    
    private facturaPart[] partesField;
    
    private facturaTipoDocumento tipoDocumentoField;
    
    private string tipoDocumentoFiscalField;
    
    private string versionField;
    
    private string serieField;
    
    private string folioFiscalField;
    
    private System.DateTime fechaField;
    
    private decimal montoTotalField;
    
    private string referenciaProveedorField;
    
    public factura() {
        this.versionField = "1.0";
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute("Cancelaciones", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public facturaCancelaciones[] Cancelaciones {
        get {
            return this.cancelacionesField;
        }
        set {
            this.cancelacionesField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public facturaMoneda moneda {
        get {
            return this.monedaField;
        }
        set {
            this.monedaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Locacion proveedor {
        get {
            return this.proveedorField;
        }
        set {
            this.proveedorField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Locacion origen {
        get {
            return this.origenField;
        }
        set {
            this.origenField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Locacion destino {
        get {
            return this.destinoField;
        }
        set {
            this.destinoField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public Locacion receiving {
        get {
            return this.receivingField;
        }
        set {
            this.receivingField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public facturaProyecto proyecto {
        get {
            return this.proyectoField;
        }
        set {
            this.proyectoField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute("nota", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string[] nota {
        get {
            return this.notaField;
        }
        set {
            this.notaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute("cargosCreditos", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public facturaCargosCreditos[] cargosCreditos {
        get {
            return this.cargosCreditosField;
        }
        set {
            this.cargosCreditosField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute("otrosCargos", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public facturaOtrosCargos[] otrosCargos {
        get {
            return this.otrosCargosField;
        }
        set {
            this.otrosCargosField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    [System.Xml.Serialization.XmlArrayItemAttribute("part", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
    public facturaPart[] partes {
        get {
            return this.partesField;
        }
        set {
            this.partesField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public facturaTipoDocumento tipoDocumento {
        get {
            return this.tipoDocumentoField;
        }
        set {
            this.tipoDocumentoField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string TipoDocumentoFiscal {
        get {
            return this.tipoDocumentoFiscalField;
        }
        set {
            this.tipoDocumentoFiscalField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string version {
        get {
            return this.versionField;
        }
        set {
            this.versionField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string serie {
        get {
            return this.serieField;
        }
        set {
            this.serieField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string folioFiscal {
        get {
            return this.folioFiscalField;
        }
        set {
            this.folioFiscalField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
    public System.DateTime fecha {
        get {
            return this.fechaField;
        }
        set {
            this.fechaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal montoTotal {
        get {
            return this.montoTotalField;
        }
        set {
            this.montoTotalField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string referenciaProveedor {
        get {
            return this.referenciaProveedorField;
        }
        set {
            this.referenciaProveedorField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class facturaCancelaciones {
    
    private string cancelaSustituyeField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CancelaSustituye {
        get {
            return this.cancelaSustituyeField;
        }
        set {
            this.cancelaSustituyeField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class Locacion {
    
    private string codigoField;
    
    private string nombreField;
    
    private string sufijoField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string codigo {
        get {
            return this.codigoField;
        }
        set {
            this.codigoField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string nombre {
        get {
            return this.nombreField;
        }
        set {
            this.nombreField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string sufijo {
        get {
            return this.sufijoField;
        }
        set {
            this.sufijoField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class facturaMoneda {
    
    private string tipoMonedaField;
    
    private decimal tipoCambioField;
    
    private bool tipoCambioFieldSpecified;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string tipoMoneda {
        get {
            return this.tipoMonedaField;
        }
        set {
            this.tipoMonedaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal tipoCambio {
        get {
            return this.tipoCambioField;
        }
        set {
            this.tipoCambioField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool tipoCambioSpecified {
        get {
            return this.tipoCambioFieldSpecified;
        }
        set {
            this.tipoCambioFieldSpecified = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class facturaProyecto {
    
    private string numeroField;
    
    private string numeroTrabajoField;
    
    private string chargeUnitField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string numero {
        get {
            return this.numeroField;
        }
        set {
            this.numeroField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string numeroTrabajo {
        get {
            return this.numeroTrabajoField;
        }
        set {
            this.numeroTrabajoField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string chargeUnit {
        get {
            return this.chargeUnitField;
        }
        set {
            this.chargeUnitField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class facturaCargosCreditos {
    
    private string referenciaChryslerField;
    
    private string consecutivoField;
    
    private decimal montoLineaField;
    
    private string facturaField;
    
    private string archivoField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string referenciaChrysler {
        get {
            return this.referenciaChryslerField;
        }
        set {
            this.referenciaChryslerField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string consecutivo {
        get {
            return this.consecutivoField;
        }
        set {
            this.consecutivoField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal montoLinea {
        get {
            return this.montoLineaField;
        }
        set {
            this.montoLineaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string factura {
        get {
            return this.facturaField;
        }
        set {
            this.facturaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string archivo {
        get {
            return this.archivoField;
        }
        set {
            this.archivoField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class facturaOtrosCargos {
    
    private facturaOtrosCargosCodigo codigoField;
    
    private decimal montoField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public facturaOtrosCargosCodigo codigo {
        get {
            return this.codigoField;
        }
        set {
            this.codigoField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal monto {
        get {
            return this.montoField;
        }
        set {
            this.montoField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public enum facturaOtrosCargosCodigo {
    
    /// <comentarios/>
    V0,
    
    /// <comentarios/>
    V4,
    
    /// <comentarios/>
    V6,
    
    /// <comentarios/>
    V1,
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class facturaPart {
    
    private facturaPartReferences referencesField;
    
    private string[] notaField;
    
    private string numeroField;
    
    private decimal cantidadField;
    
    private string unidadDeMedidaField;
    
    private decimal precioUnitarioField;
    
    private decimal montoDeLineaField;
    
    private System.DateTime fechaReciboField;
    
    private bool fechaReciboFieldSpecified;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public facturaPartReferences references {
        get {
            return this.referencesField;
        }
        set {
            this.referencesField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute("nota", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string[] nota {
        get {
            return this.notaField;
        }
        set {
            this.notaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string numero {
        get {
            return this.numeroField;
        }
        set {
            this.numeroField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal cantidad {
        get {
            return this.cantidadField;
        }
        set {
            this.cantidadField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string unidadDeMedida {
        get {
            return this.unidadDeMedidaField;
        }
        set {
            this.unidadDeMedidaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal precioUnitario {
        get {
            return this.precioUnitarioField;
        }
        set {
            this.precioUnitarioField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal montoDeLinea {
        get {
            return this.montoDeLineaField;
        }
        set {
            this.montoDeLineaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="date")]
    public System.DateTime fechaRecibo {
        get {
            return this.fechaReciboField;
        }
        set {
            this.fechaReciboField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool fechaReciboSpecified {
        get {
            return this.fechaReciboFieldSpecified;
        }
        set {
            this.fechaReciboFieldSpecified = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class facturaPartReferences {
    
    private string ordenCompraField;
    
    private string releaseRequisicionField;
    
    private string ammendmentField;
    
    private string packingListField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ordenCompra {
        get {
            return this.ordenCompraField;
        }
        set {
            this.ordenCompraField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string releaseRequisicion {
        get {
            return this.releaseRequisicionField;
        }
        set {
            this.releaseRequisicionField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ammendment {
        get {
            return this.ammendmentField;
        }
        set {
            this.ammendmentField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string packingList {
        get {
            return this.packingListField;
        }
        set {
            this.packingListField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public enum facturaTipoDocumento {
    
    /// <comentarios/>
    PPY,
}
}
