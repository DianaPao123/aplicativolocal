
using System.Xml.Serialization;

// 
// Este código fuente fue generado automáticamente por xsd, Versión=4.0.30319.1.
// 


/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos", IsNullable=false)]
public partial class Dividendos {
    
    private DividendosDividOUtil dividOUtilField;
    
    private DividendosRemanente remanenteField;
    
    private string versionField;
    
    public Dividendos() {
        this.versionField = "1.0";
    }
    
    /// <comentarios/>
    public DividendosDividOUtil DividOUtil {
        get {
            return this.dividOUtilField;
        }
        set {
            this.dividOUtilField = value;
        }
    }
    
    /// <comentarios/>
    public DividendosRemanente Remanente {
        get {
            return this.remanenteField;
        }
        set {
            this.remanenteField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Version {
        get {
            return this.versionField;
        }
        set {
            this.versionField = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos")]
public partial class DividendosDividOUtil {
    
    private string cveTipDivOUtilField;
    
    private decimal montISRAcredRetMexicoField;
    
    private decimal montISRAcredRetExtranjeroField;
    
    private decimal montRetExtDivExtField;
    
    private bool montRetExtDivExtFieldSpecified;
    
 //   private DividendosDividOUtilTipoSocDistrDiv tipoSocDistrDivField;
    private string tipoSocDistrDivField;
    
    private decimal montISRAcredNalField;
    
    private bool montISRAcredNalFieldSpecified;
    
    private decimal montDivAcumNalField;
    
    private bool montDivAcumNalFieldSpecified;
    
    private decimal montDivAcumExtField;
    
    private bool montDivAcumExtFieldSpecified;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CveTipDivOUtil {
        get {
            return this.cveTipDivOUtilField;
        }
        set {
            this.cveTipDivOUtilField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal MontISRAcredRetMexico {
        get {
            return this.montISRAcredRetMexicoField;
        }
        set {
            this.montISRAcredRetMexicoField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal MontISRAcredRetExtranjero {
        get {
            return this.montISRAcredRetExtranjeroField;
        }
        set {
            this.montISRAcredRetExtranjeroField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal MontRetExtDivExt {
        get {
            return this.montRetExtDivExtField;
        }
        set {
            this.montRetExtDivExtField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool MontRetExtDivExtSpecified {
        get {
            return this.montRetExtDivExtFieldSpecified;
        }
        set {
            this.montRetExtDivExtFieldSpecified = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string TipoSocDistrDiv {
        get {
            return this.tipoSocDistrDivField;
        }
        set {
            this.tipoSocDistrDivField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal MontISRAcredNal {
        get {
            return this.montISRAcredNalField;
        }
        set {
            this.montISRAcredNalField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool MontISRAcredNalSpecified {
        get {
            return this.montISRAcredNalFieldSpecified;
        }
        set {
            this.montISRAcredNalFieldSpecified = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal MontDivAcumNal {
        get {
            return this.montDivAcumNalField;
        }
        set {
            this.montDivAcumNalField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool MontDivAcumNalSpecified {
        get {
            return this.montDivAcumNalFieldSpecified;
        }
        set {
            this.montDivAcumNalFieldSpecified = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal MontDivAcumExt {
        get {
            return this.montDivAcumExtField;
        }
        set {
            this.montDivAcumExtField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool MontDivAcumExtSpecified {
        get {
            return this.montDivAcumExtFieldSpecified;
        }
        set {
            this.montDivAcumExtFieldSpecified = value;
        }
    }
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos")]
public enum DividendosDividOUtilCveTipDivOUtil {
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlEnumAttribute("Sociedad Nacional")]
    SociedadNacional,
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlEnumAttribute("Sociedad Extranjera")]
    SociedadExtranjera,
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos")]
public enum DividendosDividOUtilTipoSocDistrDiv {
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlEnumAttribute("Sociedad Nacional")]
    SociedadNacional,
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlEnumAttribute("Sociedad Extranjera")]
    SociedadExtranjera,
}

/// <comentarios/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos")]
public partial class DividendosRemanente {
    
    private decimal proporcionRemField;
    
    private bool proporcionRemFieldSpecified;
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal ProporcionRem {
        get {
            return this.proporcionRemField;
        }
        set {
            this.proporcionRemField = value;
        }
    }
    
    /// <comentarios/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ProporcionRemSpecified {
        get {
            return this.proporcionRemFieldSpecified;
        }
        set {
            this.proporcionRemFieldSpecified = value;
        }
    }
}
