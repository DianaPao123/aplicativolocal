﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.tiendasneto.com/ap")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.tiendasneto.com/ap", IsNullable=false)]
public partial class ap {
    
    private apDetalle[] detalleField;
    
    private apTipoComprobante tipoComprobanteField;
    
    private string plazoPagoField;
    
    private string observacionesField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Detalle")]
    public apDetalle[] Detalle {
        get {
            return this.detalleField;
        }
        set {
            this.detalleField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public apTipoComprobante tipoComprobante {
        get {
            return this.tipoComprobanteField;
        }
        set {
            this.tipoComprobanteField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string plazoPago {
        get {
            return this.plazoPagoField;
        }
        set {
            this.plazoPagoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string observaciones {
        get {
            return this.observacionesField;
        }
        set {
            this.observacionesField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.tiendasneto.com/ap")]
public partial class apDetalle {
    
    private apDetalleProducto[] productoField;
    
    private string folioField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Producto")]
    public apDetalleProducto[] Producto {
        get {
            return this.productoField;
        }
        set {
            this.productoField = value;
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
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.tiendasneto.com/ap")]
public partial class apDetalleProducto {
    
    private apDetalleProductoImpuestos impuestosField;
    
    private string codigoBarrasField;
    
    private decimal cajasEntregadasField;
    
    private decimal precioUnitarioCajaField;
    
    private decimal piezasEntregadasField;
    
    private decimal precioUnitarioPiezaField;
    
    /// <remarks/>
    public apDetalleProductoImpuestos Impuestos {
        get {
            return this.impuestosField;
        }
        set {
            this.impuestosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(DataType="integer")]
    public string codigoBarras {
        get {
            return this.codigoBarrasField;
        }
        set {
            this.codigoBarrasField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal cajasEntregadas {
        get {
            return this.cajasEntregadasField;
        }
        set {
            this.cajasEntregadasField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal precioUnitarioCaja {
        get {
            return this.precioUnitarioCajaField;
        }
        set {
            this.precioUnitarioCajaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal piezasEntregadas {
        get {
            return this.piezasEntregadasField;
        }
        set {
            this.piezasEntregadasField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal precioUnitarioPieza {
        get {
            return this.precioUnitarioPiezaField;
        }
        set {
            this.precioUnitarioPiezaField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.tiendasneto.com/ap")]
public partial class apDetalleProductoImpuestos {
    
    private apDetalleProductoImpuestosTraslado[] trasladosField;
    
    private decimal totalImpuestosTrasladadosField;
    
    private bool totalImpuestosTrasladadosFieldSpecified;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Traslado", IsNullable=false)]
    public apDetalleProductoImpuestosTraslado[] Traslados {
        get {
            return this.trasladosField;
        }
        set {
            this.trasladosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal totalImpuestosTrasladados {
        get {
            return this.totalImpuestosTrasladadosField;
        }
        set {
            this.totalImpuestosTrasladadosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool totalImpuestosTrasladadosSpecified {
        get {
            return this.totalImpuestosTrasladadosFieldSpecified;
        }
        set {
            this.totalImpuestosTrasladadosFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.tiendasneto.com/ap")]
public partial class apDetalleProductoImpuestosTraslado {
    
    private apDetalleProductoImpuestosTrasladoImpuesto impuestoField;
    
    private decimal tasaField;
    
    private decimal importeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public apDetalleProductoImpuestosTrasladoImpuesto impuesto {
        get {
            return this.impuestoField;
        }
        set {
            this.impuestoField = value;
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
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.tiendasneto.com/ap")]
public enum apDetalleProductoImpuestosTrasladoImpuesto {
    
    /// <remarks/>
    IVA,
    
    /// <remarks/>
    IVAEX,
    
    /// <remarks/>
    IEPS,
    
    /// <remarks/>
    IETU,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.tiendasneto.com/ap")]
public enum apTipoComprobante {
    
    /// <remarks/>
    FE,
    
    /// <remarks/>
    NC,
    
    /// <remarks/>
    ND,
}