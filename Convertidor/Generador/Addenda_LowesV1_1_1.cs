
using System.Xml.Serialization;


/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="www.colabora.com")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="www.colabora.com", IsNullable=false)]
public partial class Lowes{
    
    private LowesProveedor proveedorField;
    
    private LowesOrden ordenField;
    
    private LowesComprobante comprobanteField;
    
    private LowesArticulo[] articulosField;
    
    /// <comentarios/>
    public LowesProveedor Proveedor {
        get {
            return this.proveedorField;
        }
        set {
            this.proveedorField = value;
        }
    }
    
    /// <comentarios/>
    public LowesOrden Orden {
        get {
            return this.ordenField;
        }
        set {
            this.ordenField = value;
        }
    }
    
    /// <comentarios/>
    public LowesComprobante Comprobante {
        get {
            return this.comprobanteField;
        }
        set {
            this.comprobanteField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Articulo", IsNullable=false)]
    public LowesArticulo[] Articulos {
        get {
            return this.articulosField;
        }
        set {
            this.articulosField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="www.colabora.com")]
public partial class LowesProveedor {
    
    private string idField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="www.colabora.com")]
public partial class LowesOrden {
    
    private string idField;
    
    private decimal articulosField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal articulos {
        get {
            return this.articulosField;
        }
        set {
            this.articulosField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="www.colabora.com")]
public partial class LowesComprobante {
    
    private LowesComprobanteMoneda monedaField;
    
    private decimal subtotalField;
    
    private string serieField;
    
    private string folioField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public LowesComprobanteMoneda moneda {
        get {
            return this.monedaField;
        }
        set {
            this.monedaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal subtotal {
        get {
            return this.subtotalField;
        }
        set {
            this.subtotalField = value;
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
    public string folio {
        get {
            return this.folioField;
        }
        set {
            this.folioField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="www.colabora.com")]
public enum LowesComprobanteMoneda {
    
    /// <comentarios/>
    MXN,
    
    /// <comentarios/>
    USD,
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="www.colabora.com")]
public partial class LowesArticulo {
    
    private string idField;
    
    private string upcField;
    
    private decimal cantidadField;
    
    private LowesArticuloUom uomField;
    
    private decimal valorUnitarioField;
    
    private decimal importeField;
    
    private decimal ivaField;
    
    private decimal iepsField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string id {
        get {
            return this.idField;
        }
        set {
            this.idField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string upc {
        get {
            return this.upcField;
        }
        set {
            this.upcField = value;
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
    public LowesArticuloUom uom {
        get {
            return this.uomField;
        }
        set {
            this.uomField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal valorUnitario {
        get {
            return this.valorUnitarioField;
        }
        set {
            this.valorUnitarioField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal importe {
        get {
            return this.importeField;
        }
        set {
            this.importeField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal iva {
        get {
            return this.ivaField;
        }
        set {
            this.ivaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal ieps {
        get {
            return this.iepsField;
        }
        set {
            this.iepsField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="www.colabora.com")]
public enum LowesArticuloUom {
    
    /// <comentarios/>
    UOM,
    
    /// <comentarios/>
    LB,
    
    /// <comentarios/>
    CM,
    
    /// <comentarios/>
    EA,
    
    /// <comentarios/>
    DEG,
    
    /// <comentarios/>
    PFL,
    
    /// <comentarios/>
    K,
    
    /// <comentarios/>
    KWH,
    
    /// <comentarios/>
    X,
    
    /// <comentarios/>
    GBQ,
    
    /// <comentarios/>
    NO,
    
    /// <comentarios/>
    BBL,
    
    /// <comentarios/>
    CAR,
    
    /// <comentarios/>
    CC,
    
    /// <comentarios/>
    CG,
    
    /// <comentarios/>
    CGM,
    
    /// <comentarios/>
    CKG,
    
    /// <comentarios/>
    CM2,
    
    /// <comentarios/>
    CM3,
    
    /// <comentarios/>
    CTN,
    
    /// <comentarios/>
    CU,
    
    /// <comentarios/>
    DOZ,
    
    /// <comentarios/>
    DPC,
    
    /// <comentarios/>
    DPR,
    
    /// <comentarios/>
    G,
    
    /// <comentarios/>
    HUN,
    
    /// <comentarios/>
    KG,
    
    /// <comentarios/>
    M,
    
    /// <comentarios/>
    MM,
    
    /// <comentarios/>
    M2,
    
    /// <comentarios/>
    M3,
    
    /// <comentarios/>
    PCS,
    
    /// <comentarios/>
    PRS,
    
    /// <comentarios/>
    T,
    
    /// <comentarios/>
    IN,
    
    /// <comentarios/>
    IN2,
    
    /// <comentarios/>
    IN3,
    
    /// <comentarios/>
    FT,
    
    /// <comentarios/>
    FT2,
    
    /// <comentarios/>
    FT3,
    
    /// <comentarios/>
    LBS,
    
    /// <comentarios/>
    OZ,
    
    /// <comentarios/>
    PACK,
    
    /// <comentarios/>
    MBQ,
    
    /// <comentarios/>
    ODE,
    
    /// <comentarios/>
    GR,
    
    /// <comentarios/>
    SQ,
    
    /// <comentarios/>
    CYK,
    
    /// <comentarios/>
    LNM,
    
    /// <comentarios/>
    FBM,
    
    /// <comentarios/>
    JWL,
    
    /// <comentarios/>
    PK,
    
    /// <comentarios/>
    GRL,
    
    /// <comentarios/>
    BA,
    
    /// <comentarios/>
    BE,
    
    /// <comentarios/>
    BG,
    
    /// <comentarios/>
    BI,
    
    /// <comentarios/>
    BJ,
    
    /// <comentarios/>
    BK,
    
    /// <comentarios/>
    BX,
    
    /// <comentarios/>
    C,
    
    /// <comentarios/>
    CA,
    
    /// <comentarios/>
    CBM,
    
    /// <comentarios/>
    CFT,
    
    /// <comentarios/>
    CON,
    
    /// <comentarios/>
    CR,
    
    /// <comentarios/>
    CS,
    
    /// <comentarios/>
    CT,
    
    /// <comentarios/>
    CY,
    
    /// <comentarios/>
    CYG,
    
    /// <comentarios/>
    FIB,
    
    /// <comentarios/>
    HZ,
    
    /// <comentarios/>
    JR,
    
    /// <comentarios/>
    KHZ,
    
    /// <comentarios/>
    KN,
    
    /// <comentarios/>
    KPA,
    
    /// <comentarios/>
    KSB,
    
    /// <comentarios/>
    KW,
    
    /// <comentarios/>
    MPA,
    
    /// <comentarios/>
    PAL,
    
    /// <comentarios/>
    PC,
    
    /// <comentarios/>
    PO,
    
    /// <comentarios/>
    SF,
    
    /// <comentarios/>
    SBE,
    
    /// <comentarios/>
    YD,
    
    /// <comentarios/>
    MG,
    
    /// <comentarios/>
    YD3,
    
    /// <comentarios/>
    YD2,
    
    /// <comentarios/>
    MM2,
    
    /// <comentarios/>
    MM3,
    
    /// <comentarios/>
    TS,
    
    /// <comentarios/>
    KM,
    
    /// <comentarios/>
    KM2,
    
    /// <comentarios/>
    KM3,
    
    /// <comentarios/>
    FZ,
    
    /// <comentarios/>
    ML,
    
    /// <comentarios/>
    L,
    
    /// <comentarios/>
    PT,
    
    /// <comentarios/>
    QT,
    
    /// <comentarios/>
    GL,
    
    /// <comentarios/>
    DS,
    
    /// <comentarios/>
    AGG,
    
    /// <comentarios/>
    AUG,
    
    /// <comentarios/>
    PTG,
    
    /// <comentarios/>
    PDG,
    
    /// <comentarios/>
    RHG,
    
    /// <comentarios/>
    IRG,
    
    /// <comentarios/>
    OSG,
    
    /// <comentarios/>
    RUG,
    
    /// <comentarios/>
    CUR,
    
    /// <comentarios/>
    D,
    
    /// <comentarios/>
    GCN,
    
    /// <comentarios/>
    GKG,
    
    /// <comentarios/>
    GM,
    
    /// <comentarios/>
    GRS,
    
    /// <comentarios/>
    GVW,
    
    /// <comentarios/>
    HND,
    
    /// <comentarios/>
    IRC,
    
    /// <comentarios/>
    KCAL,
    
    /// <comentarios/>
    KTS,
    
    /// <comentarios/>
    KVA,
    
    /// <comentarios/>
    KVAR,
    
    /// <comentarios/>
    LIN,
    
    /// <comentarios/>
    MC,
    
    /// <comentarios/>
    MHZ,
    
    /// <comentarios/>
    MWH,
    
    /// <comentarios/>
    NA,
    
    /// <comentarios/>
    PF,
    
    /// <comentarios/>
    RBA,
    
    /// <comentarios/>
    RPM,
    
    /// <comentarios/>
    SME,
    
    /// <comentarios/>
    SQM,
    
    /// <comentarios/>
    THM,
    
    /// <comentarios/>
    THS,
    
    /// <comentarios/>
    TNV,
    
    /// <comentarios/>
    TON,
    
    /// <comentarios/>
    V,
    
    /// <comentarios/>
    W,
    
    /// <comentarios/>
    WTS,
}
