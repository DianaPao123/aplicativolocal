// AddendaDetallistaCorta.detallista
using AddendaDetallistaCorta;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

// 

namespace AddendaDetallistaCorta
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/detallista")]
    [XmlRoot(Namespace = "http://www.sat.gob.mx/detallista", IsNullable = false)]
    [GeneratedCode("xsd", "4.0.30319.1")]
    [DesignerCategory("code")]
    [DebuggerStepThrough]
    public class detallista
    {
        private detallistaOrderIdentification orderIdentificationField;

        private string contentVersionField;

        private string documentStatusField;

        private string documentStructureVersionField;

        public detallistaOrderIdentification orderIdentification
        {
            get
            {
                return orderIdentificationField;
            }
            set
            {
                orderIdentificationField = value;
            }
        }

        [XmlAttribute]
        public string contentVersion
        {
            get
            {
                return contentVersionField;
            }
            set
            {
                contentVersionField = value;
            }
        }

        [XmlAttribute]
        public string documentStatus
        {
            get
            {
                return documentStatusField;
            }
            set
            {
                documentStatusField = value;
            }
        }

        [XmlAttribute]
        public string documentStructureVersion
        {
            get
            {
                return documentStructureVersionField;
            }
            set
            {
                documentStructureVersionField = value;
            }
        }
    }
    [Serializable]
    [GeneratedCode("xsd", "4.0.30319.1")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/detallista")]
    public class detallistaOrderIdentification
    {
        private detallistaOrderIdentificationReferenceIdentification referenceIdentificationField;

        public detallistaOrderIdentificationReferenceIdentification referenceIdentification
        {
            get
            {
                return referenceIdentificationField;
            }
            set
            {
                referenceIdentificationField = value;
            }
        }
    }
    [Serializable]
    [GeneratedCode("xsd", "4.0.30319.1")]
    [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/detallista")]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    public class detallistaOrderIdentificationReferenceIdentification
    {
        private string typeField;

        private string valueField;

        [XmlAttribute]
        public string type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        [XmlText]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

}