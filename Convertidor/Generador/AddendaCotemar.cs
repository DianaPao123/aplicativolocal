
using System.Xml.Serialization;


/*
/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar", IsNullable = false)]
public partial class AddendaCotemar {
    private Cotemar cotemarField;

    public Cotemar cotemar
    {
        get
        {
            return this.cotemarField;
        }
        set
        {
            this.cotemarField = value;
        }
    }
}
*/

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar", IsNullable = false)]

public partial class Cotemar {
    
    private double numProveedorField;
    
    private double numPedidoField;
    
    private string numEntMercanciaField;
    
    private string contactoCompraField;
    
    private CotemarMoneda monedaField;
    
    /// <comentarios/>
    public double NumProveedor {
        get {
            return this.numProveedorField;
        }
        set {
            this.numProveedorField = value;
        }
    }
    
    /// <comentarios/>
    public double NumPedido {
        get {
            return this.numPedidoField;
        }
        set {
            this.numPedidoField = value;
        }
    }
    
    /// <comentarios/>
    public string NumEntMercancia {
        get {
            return this.numEntMercanciaField;
        }
        set {
            this.numEntMercanciaField = value;
        }
    }
    
    /// <comentarios/>
    public string ContactoCompra {
        get {
            return this.contactoCompraField;
        }
        set {
            this.contactoCompraField = value;
        }
    }
    
    /// <comentarios/>
    public CotemarMoneda Moneda {
        get {
            return this.monedaField;
        }
        set {
            this.monedaField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="https://portals.cotemar.com.mx/Finanzas/xmladdendas/Cotemar")]
public enum CotemarMoneda {
    
    /// <comentarios/>
    MXN,
    
    /// <comentarios/>
    USD,
    
    /// <comentarios/>
    EUR,
}
