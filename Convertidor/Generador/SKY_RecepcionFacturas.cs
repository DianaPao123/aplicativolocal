﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Schema;
using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://repository.edicomnet.com/schemas/mx/cfd/addenda")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://repository.edicomnet.com/schemas/mx/cfd/addenda", IsNullable=false)]
public partial class SKY_RecepcionFacturas {
    
	 [XmlAttribute("schemaLocation", Namespace = XmlSchema.InstanceNamespace)] 
        public string xsiSchemaLocation = "http://repository.edicomnet.com/schemas/mx/cfd/addenda http://repository.edicomnet.com/schemas/mx/cfd/addenda/SKY_RecepcionFacturas.xsd";

	
	
    private string numAcreedorField;
    
    private SKY_RecepcionFacturasTipoFacturaProveedor tipoFacturaProveedorField;
    
    private string codigoFacturacionField;
    
    private string numOrdenComprasField;
    
    private SKY_RecepcionFacturasSistema sistemaField;
    
    private bool sistemaFieldSpecified;
    
    private string personaContactoField;
    
    private string numRefSisField;
    
    private string numPedimentoField;
    
    /// <remarks/>
    public string NumAcreedor {
        get {
            return this.numAcreedorField;
        }
        set {
            this.numAcreedorField = value;
        }
    }
    
    /// <remarks/>
    public SKY_RecepcionFacturasTipoFacturaProveedor TipoFacturaProveedor {
        get {
            return this.tipoFacturaProveedorField;
        }
        set {
            this.tipoFacturaProveedorField = value;
        }
    }
    
    /// <remarks/>
    public string CodigoFacturacion {
        get {
            return this.codigoFacturacionField;
        }
        set {
            this.codigoFacturacionField = value;
        }
    }
    
    /// <remarks/>
    public string NumOrdenCompras {
        get {
            return this.numOrdenComprasField;
        }
        set {
            this.numOrdenComprasField = value;
        }
    }
    
    /// <remarks/>
    public SKY_RecepcionFacturasSistema Sistema {
        get {
            return this.sistemaField;
        }
        set {
            this.sistemaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool SistemaSpecified {
        get {
            return this.sistemaFieldSpecified;
        }
        set {
            this.sistemaFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    public string PersonaContacto {
        get {
            return this.personaContactoField;
        }
        set {
            this.personaContactoField = value;
        }
    }
    
    /// <remarks/>
    public string NumRefSis {
        get {
            return this.numRefSisField;
        }
        set {
            this.numRefSisField = value;
        }
    }
    
    /// <remarks/>
    public string NumPedimento {
        get {
            return this.numPedimentoField;
        }
        set {
            this.numPedimentoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://repository.edicomnet.com/schemas/mx/cfd/addenda")]
public enum SKY_RecepcionFacturasTipoFacturaProveedor {
    
    /// <remarks/>
    master,
    
    /// <remarks/>
    distribuidor,
    
    /// <remarks/>
    reparador,
    
    /// <remarks/>
    recuperador,
    
    /// <remarks/>
    planc,
    
    /// <remarks/>
    planhotelero,
    
    /// <remarks/>
    programadores,
    
    /// <remarks/>
    proveedores,
    
    /// <remarks/>
    arrendadores,
    
    /// <remarks/>
    aduanal,
    
    /// <remarks/>
    comisionistas,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://repository.edicomnet.com/schemas/mx/cfd/addenda")]
public enum SKY_RecepcionFacturasCodigoFacturacion {
    
    /// <remarks/>
    CM00001495,
    
    /// <remarks/>
    CM00000778,
    
    /// <remarks/>
    CM00000406,
    
    /// <remarks/>
    CM00000851,
    
    /// <remarks/>
    CM00000620,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("CM00000620")]
    CM000006201,
    
    /// <remarks/>
    CM00000323,
    
    /// <remarks/>
    CM00000745,
    
    /// <remarks/>
    CM00000711,
    
    /// <remarks/>
    CM00000547,
    
    /// <remarks/>
    CM00000554,
    
    /// <remarks/>
    CM00000729,
    
    /// <remarks/>
    CM00000570,
    
    /// <remarks/>
    CM00000604,
    
    /// <remarks/>
    CM00000786,
    
    /// <remarks/>
    CM00000588,
    
    /// <remarks/>
    CM00000679,
    
    /// <remarks/>
    CM00000752,
    
    /// <remarks/>
    CM00000232,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://repository.edicomnet.com/schemas/mx/cfd/addenda")]
public enum SKY_RecepcionFacturasSistema {
    
    /// <remarks/>
    SAP,
    
    /// <remarks/>
    HEAT,
    
    /// <remarks/>
    SICO,
}
