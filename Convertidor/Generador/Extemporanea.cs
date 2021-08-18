﻿

using System.Xml.Serialization;

// 
namespace SorianaAdendas
{

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/DSCargaRemisionProv.xsd")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/DSCargaRemisionProv.xsd", IsNullable=false)]
public partial class DSCargaRemisionProv {
    /*
    private object[] itemsField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Articulos", typeof(DSCargaRemisionProvArticulos))]
    [System.Xml.Serialization.XmlElementAttribute("Pedidos", typeof(DSCargaRemisionProvPedidos))]
    [System.Xml.Serialization.XmlElementAttribute("Pedimento", typeof(DSCargaRemisionProvPedimento))]
    [System.Xml.Serialization.XmlElementAttribute("Remision", typeof(DSCargaRemisionProvRemision))]
    public object[] Items {
        get {
            return this.itemsField;
        }
        set {
            this.itemsField = value;
        }
    }
     * */
    private DSCargaRemisionProvPedimento[] PedimentoField;
      [System.Xml.Serialization.XmlElementAttribute("Pedimento")]
     public DSCargaRemisionProvPedimento[] Pedimento
    {
        get
        {
            return this.PedimentoField;
        }
        set
        {
            this.PedimentoField = value;
        }
    }
    private DSCargaRemisionProvPedidos[] PedidosField;

     [System.Xml.Serialization.XmlElementAttribute("Pedidos")]
    public DSCargaRemisionProvPedidos[] Pedidos
    {
        get
        {
            return this.PedidosField;
        }
        set
        {
            this.PedidosField = value;
        }
    }

    private DSCargaRemisionProvArticulos[] ArticulosField;
         [System.Xml.Serialization.XmlElementAttribute("Articulos")]
     public DSCargaRemisionProvArticulos[] Articulos
    {
        get
        {
            return this.ArticulosField;
        }
        set
        {
            this.ArticulosField = value;
        }
    }
    private DSCargaRemisionProvRemision[] RemisionField;
          [System.Xml.Serialization.XmlElementAttribute("Remision")]
   public DSCargaRemisionProvRemision[] Remision
    {
        get
        {
            return this.RemisionField;
        }
        set
        {
            this.RemisionField = value;
        }
    }


}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/DSCargaRemisionProv.xsd")]
public partial class DSCargaRemisionProvArticulos {
    
    private int proveedorField;
    
    private string remisionField;
    
    private int folioPedidoField;
    
    private short tiendaField;
    
    private decimal codigoField;
    
    private decimal cantidadUnidadCompraField;
    
    private decimal costoNetoUnidadCompraField;
    
    private decimal porcentajeIEPSField;
    
    private decimal porcentajeIVAField;
    
    /// <remarks/>
    public int Proveedor {
        get {
            return this.proveedorField;
        }
        set {
            this.proveedorField = value;
        }
    }
    
    /// <remarks/>
    public string Remision {
        get {
            return this.remisionField;
        }
        set {
            this.remisionField = value;
        }
    }
    
    /// <remarks/>
    public int FolioPedido {
        get {
            return this.folioPedidoField;
        }
        set {
            this.folioPedidoField = value;
        }
    }
    
    /// <remarks/>
    public short Tienda {
        get {
            return this.tiendaField;
        }
        set {
            this.tiendaField = value;
        }
    }
    
    /// <remarks/>
    public decimal Codigo {
        get {
            return this.codigoField;
        }
        set {
            this.codigoField = value;
        }
    }
    
    /// <remarks/>
    public decimal CantidadUnidadCompra {
        get {
            return this.cantidadUnidadCompraField;
        }
        set {
            this.cantidadUnidadCompraField = value;
        }
    }
    
    /// <remarks/>
    public decimal CostoNetoUnidadCompra {
        get {
            return this.costoNetoUnidadCompraField;
        }
        set {
            this.costoNetoUnidadCompraField = value;
        }
    }
    
    /// <remarks/>
    public decimal PorcentajeIEPS {
        get {
            return this.porcentajeIEPSField;
        }
        set {
            this.porcentajeIEPSField = value;
        }
    }
    
    /// <remarks/>
    public decimal PorcentajeIVA {
        get {
            return this.porcentajeIVAField;
        }
        set {
            this.porcentajeIVAField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/DSCargaRemisionProv.xsd")]
public partial class DSCargaRemisionProvPedidos {
    
    private int proveedorField;
    
    private string remisionField;
    
    private int folioPedidoField;
    
    private short tiendaField;
    
    private int cantidadArticulosField;
    
    private DSCargaRemisionProvPedidosPedidoEmitidoProveedor pedidoEmitidoProveedorField;
    
    private bool pedidoEmitidoProveedorFieldSpecified;
    
    /// <remarks/>
    public int Proveedor {
        get {
            return this.proveedorField;
        }
        set {
            this.proveedorField = value;
        }
    }
    
    /// <remarks/>
    public string Remision {
        get {
            return this.remisionField;
        }
        set {
            this.remisionField = value;
        }
    }
    
    /// <remarks/>
    public int FolioPedido {
        get {
            return this.folioPedidoField;
        }
        set {
            this.folioPedidoField = value;
        }
    }
    
    /// <remarks/>
    public short Tienda {
        get {
            return this.tiendaField;
        }
        set {
            this.tiendaField = value;
        }
    }
    
    /// <remarks/>
    public int CantidadArticulos {
        get {
            return this.cantidadArticulosField;
        }
        set {
            this.cantidadArticulosField = value;
        }
    }
    
    /// <remarks/>
    public DSCargaRemisionProvPedidosPedidoEmitidoProveedor PedidoEmitidoProveedor {
        get {
            return this.pedidoEmitidoProveedorField;
        }
        set {
            this.pedidoEmitidoProveedorField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool PedidoEmitidoProveedorSpecified {
        get {
            return this.pedidoEmitidoProveedorFieldSpecified;
        }
        set {
            this.pedidoEmitidoProveedorFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/DSCargaRemisionProv.xsd")]
public enum DSCargaRemisionProvPedidosPedidoEmitidoProveedor {
    
    /// <remarks/>
    SI,
    
    /// <remarks/>
    NO,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/DSCargaRemisionProv.xsd")]
public partial class DSCargaRemisionProvPedimento {
    
    private int proveedorField;
    
    private string remisionField;
    
    private int pedimentoField;
    
    private short aduanaField;
    
    private short agenteAduanalField;
    
    private string tipoPedimentoField;
    
    private System.DateTime fechaPedimentoField;
    
    private System.DateTime fechaReciboLaredoField;
    
    private System.DateTime fechaBillOfLadingField;
    
    /// <remarks/>
    public int Proveedor {
        get {
            return this.proveedorField;
        }
        set {
            this.proveedorField = value;
        }
    }
    
    /// <remarks/>
    public string Remision {
        get {
            return this.remisionField;
        }
        set {
            this.remisionField = value;
        }
    }
    
    /// <remarks/>
    public int Pedimento {
        get {
            return this.pedimentoField;
        }
        set {
            this.pedimentoField = value;
        }
    }
    
    /// <remarks/>
    public short Aduana {
        get {
            return this.aduanaField;
        }
        set {
            this.aduanaField = value;
        }
    }
    
    /// <remarks/>
    public short AgenteAduanal {
        get {
            return this.agenteAduanalField;
        }
        set {
            this.agenteAduanalField = value;
        }
    }
    
    /// <remarks/>
    public string TipoPedimento {
        get {
            return this.tipoPedimentoField;
        }
        set {
            this.tipoPedimentoField = value;
        }
    }
    
    /// <remarks/>
    public System.DateTime FechaPedimento {
        get {
            return this.fechaPedimentoField;
        }
        set {
            this.fechaPedimentoField = value;
        }
    }
    
    /// <remarks/>
    public System.DateTime FechaReciboLaredo {
        get {
            return this.fechaReciboLaredoField;
        }
        set {
            this.fechaReciboLaredoField = value;
        }
    }
    
    /// <remarks/>
    public System.DateTime FechaBillOfLading {
        get {
            return this.fechaBillOfLadingField;
        }
        set {
            this.fechaBillOfLadingField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://tempuri.org/DSCargaRemisionProv.xsd")]
public partial class DSCargaRemisionProvRemision
{

    private int proveedorField;

    private string remisionField;

    private short consecutivoField;

    private System.DateTime fechaRemisionField;

    private short tiendaField;

    private short tipoMonedaField;

    private short tipoBultoField;

    private short entregaMercanciaField;

    private bool cumpleReqFiscalesField;

    private decimal cantidadBultosField;

    private decimal subtotalField;

    private decimal iEPSField;

    private decimal iVAField;

    private decimal otrosImpuestosField;

    private decimal totalField;

    private int cantidadPedidosField;

    private System.DateTime fechaEntregaMercanciaField;

    private int citaField;

    private bool citaFieldSpecified;

    private int folioNotaEntradaField;

    private bool folioNotaEntradaFieldSpecified;

    /// <remarks/>
    public int Proveedor
    {
        get
        {
            return this.proveedorField;
        }
        set
        {
            this.proveedorField = value;
        }
    }

    /// <remarks/>
    public string Remision
    {
        get
        {
            return this.remisionField;
        }
        set
        {
            this.remisionField = value;
        }
    }

    /// <remarks/>
    public short Consecutivo
    {
        get
        {
            return this.consecutivoField;
        }
        set
        {
            this.consecutivoField = value;
        }
    }

    /// <remarks/>
    public System.DateTime FechaRemision
    {
        get
        {
            return this.fechaRemisionField;
        }
        set
        {
            this.fechaRemisionField = value;
        }
    }

    /// <remarks/>
    public short Tienda
    {
        get
        {
            return this.tiendaField;
        }
        set
        {
            this.tiendaField = value;
        }
    }

    /// <remarks/>
    public short TipoMoneda
    {
        get
        {
            return this.tipoMonedaField;
        }
        set
        {
            this.tipoMonedaField = value;
        }
    }

    /// <remarks/>
    public short TipoBulto
    {
        get
        {
            return this.tipoBultoField;
        }
        set
        {
            this.tipoBultoField = value;
        }
    }

    /// <remarks/>
    public short EntregaMercancia
    {
        get
        {
            return this.entregaMercanciaField;
        }
        set
        {
            this.entregaMercanciaField = value;
        }
    }

    /// <remarks/>
    public bool CumpleReqFiscales
    {
        get
        {
            return this.cumpleReqFiscalesField;
        }
        set
        {
            this.cumpleReqFiscalesField = value;
        }
    }

    /// <remarks/>
    public decimal CantidadBultos
    {
        get
        {
            return this.cantidadBultosField;
        }
        set
        {
            this.cantidadBultosField = value;
        }
    }

    /// <remarks/>
    public decimal Subtotal
    {
        get
        {
            return this.subtotalField;
        }
        set
        {
            this.subtotalField = value;
        }
    }

    /// <remarks/>
    public decimal IEPS
    {
        get
        {
            return this.iEPSField;
        }
        set
        {
            this.iEPSField = value;
        }
    }

    /// <remarks/>
    public decimal IVA
    {
        get
        {
            return this.iVAField;
        }
        set
        {
            this.iVAField = value;
        }
    }

    /// <remarks/>
    public decimal OtrosImpuestos
    {
        get
        {
            return this.otrosImpuestosField;
        }
        set
        {
            this.otrosImpuestosField = value;
        }
    }

    /// <remarks/>
    public decimal Total
    {
        get
        {
            return this.totalField;
        }
        set
        {
            this.totalField = value;
        }
    }

    /// <remarks/>
    public int CantidadPedidos
    {
        get
        {
            return this.cantidadPedidosField;
        }
        set
        {
            this.cantidadPedidosField = value;
        }
    }

    /// <remarks/>
    public System.DateTime FechaEntregaMercancia
    {
        get
        {
            return this.fechaEntregaMercanciaField;
        }
        set
        {
            this.fechaEntregaMercanciaField = value;
        }
    }

    /// <remarks/>
    public int Cita
    {
        get
        {
            return this.citaField;
        }
        set
        {
            this.citaField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool CitaSpecified
    {
        get
        {
            return this.citaFieldSpecified;
        }
        set
        {
            this.citaFieldSpecified = value;
        }
    }

    /// <remarks/>
    public int FolioNotaEntrada
    {
        get
        {
            return this.folioNotaEntradaField;
        }
        set
        {
            this.folioNotaEntradaField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool FolioNotaEntradaSpecified
    {
        get
        {
            return this.folioNotaEntradaFieldSpecified;
        }
        set
        {
            this.folioNotaEntradaFieldSpecified = value;
        }
    }
}
}
