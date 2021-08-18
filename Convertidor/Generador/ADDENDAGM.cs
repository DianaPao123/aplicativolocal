using System.Xml.Serialization;


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
public partial class ADDENDAGM {
    
    private ADDENDAGMHEADER hEADERField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ADDENDAGMHEADER HEADER {
        get {
            return this.hEADERField;
        }
        set {
            this.hEADERField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class ADDENDAGMHEADER {
    
    private string nUMEROREMISIONField;
    
    private string fECHARECIBOField;
    
    private string fOLIOINTERNOField;
    
    private ADDENDAGMHEADERMONEDA mONEDAField;
    
    private ADDENDAGMHEADERITEM[] iTEMField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string NUMEROREMISION {
        get {
            return this.nUMEROREMISIONField;
        }
        set {
            this.nUMEROREMISIONField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string FECHARECIBO {
        get {
            return this.fECHARECIBOField;
        }
        set {
            this.fECHARECIBOField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string FOLIOINTERNO {
        get {
            return this.fOLIOINTERNOField;
        }
        set {
            this.fOLIOINTERNOField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ADDENDAGMHEADERMONEDA MONEDA {
        get {
            return this.mONEDAField;
        }
        set {
            this.mONEDAField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("ITEM", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ADDENDAGMHEADERITEM[] ITEM {
        get {
            return this.iTEMField;
        }
        set {
            this.iTEMField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public enum ADDENDAGMHEADERMONEDA {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("1")]
    Item1,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("2")]
    Item2,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("3")]
    Item3,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("4")]
    Item4,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("5")]
    Item5,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class ADDENDAGMHEADERITEM {
    
    private string oRDENCOMPRAField;
    
    private string nUMEROPARTEField;
    
    private ADDENDAGMHEADERITEMMATERIAL mATERIALField;
    
    private decimal cANTIDADField;
    
    private decimal pRECIOUNITARIOField;
    
    private string dESCRIPCIONField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string ORDENCOMPRA {
        get {
            return this.oRDENCOMPRAField;
        }
        set {
            this.oRDENCOMPRAField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string NUMEROPARTE {
        get {
            return this.nUMEROPARTEField;
        }
        set {
            this.nUMEROPARTEField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public ADDENDAGMHEADERITEMMATERIAL MATERIAL {
        get {
            return this.mATERIALField;
        }
        set {
            this.mATERIALField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public decimal CANTIDAD {
        get {
            return this.cANTIDADField;
        }
        set {
            this.cANTIDADField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public decimal PRECIOUNITARIO {
        get {
            return this.pRECIOUNITARIOField;
        }
        set {
            this.pRECIOUNITARIOField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string DESCRIPCION {
        get {
            return this.dESCRIPCIONField;
        }
        set {
            this.dESCRIPCIONField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public enum ADDENDAGMHEADERITEMMATERIAL {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("1")]
    Item1,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("2")]
    Item2,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("3")]
    Item3,
}
