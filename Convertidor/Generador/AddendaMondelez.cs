//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
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
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class Inicial {
    
    private string ordenCompraField;
    
    private long numeroGrField;
    
    private string notaEntregaField;
    
    private long fechaCreacionOrdenCompraField;
    
    private bool fechaCreacionOrdenCompraFieldSpecified;
    
    private string numeroFacturaOriginalField;
    
    private string nombreContactoClienteField;
    
    private string correoContactoClienteField;
    
    private InicialDireccionEmision direccionEmisionField;
    
    private InicialDireccionEntrega direccionEntregaField;
    
    private InicialDetalle[] detalleField;
    
    /// <comentarios/>
    public string OrdenCompra {
        get {
            return this.ordenCompraField;
        }
        set {
            this.ordenCompraField = value;
        }
    }
    
    /// <comentarios/>
    public long NumeroGr {
        get {
            return this.numeroGrField;
        }
        set {
            this.numeroGrField = value;
        }
    }
    
    /// <comentarios/>
    public string NotaEntrega {
        get {
            return this.notaEntregaField;
        }
        set {
            this.notaEntregaField = value;
        }
    }
    
    /// <comentarios/>
    public long FechaCreacionOrdenCompra {
        get {
            return this.fechaCreacionOrdenCompraField;
        }
        set {
            this.fechaCreacionOrdenCompraField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool FechaCreacionOrdenCompraSpecified {
        get {
            return this.fechaCreacionOrdenCompraFieldSpecified;
        }
        set {
            this.fechaCreacionOrdenCompraFieldSpecified = value;
        }
    }
    
    /// <comentarios/>
    public string NumeroFacturaOriginal {
        get {
            return this.numeroFacturaOriginalField;
        }
        set {
            this.numeroFacturaOriginalField = value;
        }
    }
    
    /// <comentarios/>
    public string NombreContactoCliente {
        get {
            return this.nombreContactoClienteField;
        }
        set {
            this.nombreContactoClienteField = value;
        }
    }
    
    /// <comentarios/>
    public string CorreoContactoCliente {
        get {
            return this.correoContactoClienteField;
        }
        set {
            this.correoContactoClienteField = value;
        }
    }
    
    /// <comentarios/>
    public InicialDireccionEmision DireccionEmision {
        get {
            return this.direccionEmisionField;
        }
        set {
            this.direccionEmisionField = value;
        }
    }
    
    /// <comentarios/>
    public InicialDireccionEntrega DireccionEntrega {
        get {
            return this.direccionEntregaField;
        }
        set {
            this.direccionEntregaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute("Detalle")]
    public InicialDetalle[] Detalle {
        get {
            return this.detalleField;
        }
        set {
            this.detalleField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class InicialDireccionEmision {
    
    private string calleEmisionField;
    
    private string estadoEmisionField;
    
    private string municipioEmisionField;
    
    private string coloniaEmisionField;
    
    private string codigoPostalField;
    
    private string nombreProveedorField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CalleEmision {
        get {
            return this.calleEmisionField;
        }
        set {
            this.calleEmisionField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string EstadoEmision {
        get {
            return this.estadoEmisionField;
        }
        set {
            this.estadoEmisionField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string MunicipioEmision {
        get {
            return this.municipioEmisionField;
        }
        set {
            this.municipioEmisionField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ColoniaEmision {
        get {
            return this.coloniaEmisionField;
        }
        set {
            this.coloniaEmisionField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CodigoPostal {
        get {
            return this.codigoPostalField;
        }
        set {
            this.codigoPostalField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NombreProveedor {
        get {
            return this.nombreProveedorField;
        }
        set {
            this.nombreProveedorField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class InicialDireccionEntrega {
    
    private string calleEntregaField;
    
    private string estadoEntregaField;
    
    private string municipioEntregaField;
    
    private string coloniaEntregaField;
    
    private string codigoPostalField;
    
    private string nombreClienteField;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CalleEntrega {
        get {
            return this.calleEntregaField;
        }
        set {
            this.calleEntregaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string EstadoEntrega {
        get {
            return this.estadoEntregaField;
        }
        set {
            this.estadoEntregaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string MunicipioEntrega {
        get {
            return this.municipioEntregaField;
        }
        set {
            this.municipioEntregaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ColoniaEntrega {
        get {
            return this.coloniaEntregaField;
        }
        set {
            this.coloniaEntregaField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CodigoPostal {
        get {
            return this.codigoPostalField;
        }
        set {
            this.codigoPostalField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NombreCliente {
        get {
            return this.nombreClienteField;
        }
        set {
            this.nombreClienteField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class InicialDetalle {
    
    private int noItemField;
    
    private bool noItemFieldSpecified;
    
    private string codigoProductoClienteField;
    
    private string codigoProductoProveedorField;
    
    /// <comentarios/>
    public int NoItem {
        get {
            return this.noItemField;
        }
        set {
            this.noItemField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool NoItemSpecified {
        get {
            return this.noItemFieldSpecified;
        }
        set {
            this.noItemFieldSpecified = value;
        }
    }
    
    /// <comentarios/>
    public string CodigoProductoCliente {
        get {
            return this.codigoProductoClienteField;
        }
        set {
            this.codigoProductoClienteField = value;
        }
    }
    
    /// <comentarios/>
    public string CodigoProductoProveedor {
        get {
            return this.codigoProductoProveedorField;
        }
        set {
            this.codigoProductoProveedorField = value;
        }
    }
}
