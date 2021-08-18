//------------------------------------------------------------------------------


using System.Xml.Serialization;

// 
// Este código fuente fue generado automáticamente por xsd, Versión=4.0.30319.1.
// 


/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.honda.net.mx/GPC")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.honda.net.mx/GPC", IsNullable=false)]
public partial class Honda {
    
    private HondaProveedor proveedorField;
    
    private HondaConcepto[] conceptosField;
    
    private HondaTipoDocumento tipoDocumentoField;
    
    private HondaTipoComprobante tipoComprobanteField;
    
    private string monedaField;
    
    private string fechaField;
    
    private string folioField;
    
    private HondaPlantCode plantCodeField;
    
    private string aSNNumberField;
    
    private string referenceNumberField;
    
    public Honda() {
        this.monedaField = "USD";
    }
    
    /// <comentarios/>
    public HondaProveedor Proveedor {
        get {
            return this.proveedorField;
        }
        set {
            this.proveedorField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Concepto", IsNullable=false)]
    public HondaConcepto[] Conceptos {
        get {
            return this.conceptosField;
        }
        set {
            this.conceptosField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public HondaTipoDocumento tipoDocumento {
        get {
            return this.tipoDocumentoField;
        }
        set {
            this.tipoDocumentoField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public HondaTipoComprobante tipoComprobante
    {
        get {
            return this.tipoComprobanteField;
        }
        set {
            this.tipoComprobanteField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string moneda {
        get {
            return this.monedaField;
        }
        set {
            this.monedaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string fecha {
        get {
            return this.fechaField;
        }
        set {
            this.fechaField = value;
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
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public HondaPlantCode PlantCode {
        get {
            return this.plantCodeField;
        }
        set {
            this.plantCodeField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ASNNumber {
        get {
            return this.aSNNumberField;
        }
        set {
            this.aSNNumberField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ReferenceNumber {
        get {
            return this.referenceNumberField;
        }
        set {
            this.referenceNumberField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.honda.net.mx/GPC")]
public partial class HondaProveedor {
    
    private string numeroProveedorField;
    
    private string rfcField;
    
    private string shipToField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string numeroProveedor {
        get {
            return this.numeroProveedorField;
        }
        set {
            this.numeroProveedorField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string rfc {
        get {
            return this.rfcField;
        }
        set {
            this.rfcField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ShipTo {
        get {
            return this.shipToField;
        }
        set {
            this.shipToField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.honda.net.mx/GPC")]
public partial class HondaConcepto {
    
    private decimal importeField;
    
    private decimal valorUnitarioField;
    
    private string descripcionField;
    
    private string partnumberField;
    
    private long cantidadField;
    
    private int nolineaField;
    
    private string partcolorField;
    
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
    public string descripcion {
        get {
            return this.descripcionField;
        }
        set {
            this.descripcionField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string partnumber {
        get {
            return this.partnumberField;
        }
        set {
            this.partnumberField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public long cantidad {
        get {
            return this.cantidadField;
        }
        set {
            this.cantidadField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public int nolinea {
        get {
            return this.nolineaField;
        }
        set {
            this.nolineaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string partcolor {
        get {
            return this.partcolorField;
        }
        set {
            this.partcolorField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.honda.net.mx/GPC")]
public enum HondaTipoDocumento {
    
    /// <comentarios/>
    GPC,
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.honda.net.mx/GPC")]
public enum HondaTipoComprobante {
    
    /// <comentarios/>
    I,
    
    /// <comentarios/>
    E,
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.honda.net.mx/GPC")]
public enum HondaPlantCode {
    
    /// <comentarios/>
    HCL,
    
    /// <comentarios/>
    MPS,
    
    /// <comentarios/>
    MTP,
    
    /// <comentarios/>
    GDL,
}
