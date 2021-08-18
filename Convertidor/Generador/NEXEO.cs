// NEXEO
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[Serializable]
[DesignerCategory("code")]
[GeneratedCode("xsd", "4.0.30319.1")]
[DebuggerStepThrough]
[XmlType(AnonymousType = true, Namespace = "http://www.sunchemical.com/")]
[XmlRoot(Namespace = "http://www.sunchemical.com/", IsNullable = false)]
public class NEXEO
{
    private string pO_NUMBERField;

    [XmlElement(DataType = "integer")]
    public string PO_NUMBER
    {
        get
        {
            return pO_NUMBERField;
        }
        set
        {
            pO_NUMBERField = value;
        }
    }
}
