using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using GeneradorCfdi;
using log4net;

namespace TxtToCfdi
{
    public class ParserSorianaExtemporanea : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserSorianaExtemporanea));



        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetExtemporaneaAdenda(data);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(SorianaAdendas.DSCargaRemisionProv), "", "");
                    //  xmlAdenda = xmlAdenda.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private SorianaAdendas.DSCargaRemisionProv GetExtemporaneaAdenda(string[][] data)
        {

            var SExtemporanea = data.FirstOrDefault(j => j[0] == "SExtemporanea");
            var SRemision = data.Where(j => j[0] == "SRemision");
            var SArticulos = data.Where(j => j[0] == "SArticulos");
            var SPedimento = data.Where(j => j[0] == "SPedimento");
            var SPedidos = data.Where(j => j[0] == "SPedidos");
            if (SExtemporanea != null)
            {
                SorianaAdendas.DSCargaRemisionProv H = new SorianaAdendas.DSCargaRemisionProv();
                if (SRemision.Any())
                {
                    
                  List<SorianaAdendas.DSCargaRemisionProvRemision> listaRemision = new List<SorianaAdendas.DSCargaRemisionProvRemision>();
            
                   foreach (var remision in SRemision)
                  {
                    SorianaAdendas.DSCargaRemisionProvRemision R=new SorianaAdendas.DSCargaRemisionProvRemision();
                      
                       R.Proveedor=int.Parse(ParserNtLink.GetValue(remision[1]));
                       R.Remision=ParserNtLink.GetValue(remision[2]);
                       R.Consecutivo=short.Parse(ParserNtLink.GetValue(remision[3]));
                       R.FechaRemision=DateTime.Parse(ParserNtLink.GetValue(remision[4]));
                       R.Tienda=short.Parse(ParserNtLink.GetValue(remision[5]));
                       R.TipoMoneda=short.Parse(ParserNtLink.GetValue(remision[6]));
                       R.TipoBulto=short.Parse(ParserNtLink.GetValue(remision[7]));
                       R.EntregaMercancia=short.Parse(ParserNtLink.GetValue(remision[8]));
                       R.CumpleReqFiscales=bool.Parse(ParserNtLink.GetValue(remision[9]));
                       R.CantidadBultos=decimal.Parse(ParserNtLink.GetValue(remision[10]));
                       R.Subtotal=decimal.Parse(ParserNtLink.GetValue(remision[11]));
                       R.IEPS=decimal.Parse(ParserNtLink.GetValue(remision[12]));
                       R.IVA=decimal.Parse(ParserNtLink.GetValue(remision[13]));
                       R.OtrosImpuestos=decimal.Parse(ParserNtLink.GetValue(remision[14]));
                       R.Total=decimal.Parse(ParserNtLink.GetValue(remision[15]));
                       R.CantidadPedidos=int.Parse(ParserNtLink.GetValue(remision[16]));
                       R.FechaEntregaMercancia=DateTime.Parse(ParserNtLink.GetValue(remision[17]));
                       if(!string.IsNullOrEmpty( remision[18]))
                       {
                       R.Cita=int.Parse(ParserNtLink.GetValue(remision[18]));
                           R.CitaSpecified=true;
                       }
                       else
                           R.CitaSpecified=false;
                       if (!string.IsNullOrEmpty(remision[19]))
                       {
                           R.FolioNotaEntrada = int.Parse(ParserNtLink.GetValue(remision[19]));
                           R.FolioNotaEntradaSpecified = true;
                       }
                       else
                           R.FolioNotaEntradaSpecified = false;

                       listaRemision.Add(R);

                   }
                    H.Remision=listaRemision.ToArray();

                }
                if (SPedimento.Any())
                {

                    List<SorianaAdendas.DSCargaRemisionProvPedimento> listaPedimento = new List<SorianaAdendas.DSCargaRemisionProvPedimento>();
                    foreach (var pedimento in SPedimento)
                    {

                        SorianaAdendas.DSCargaRemisionProvPedimento P = new SorianaAdendas.DSCargaRemisionProvPedimento();
                       
                        P.Proveedor = int.Parse(ParserNtLink.GetValue(pedimento[1]));
                        P.Remision = ParserNtLink.GetValue(pedimento[2]);
                        P.Pedimento = int.Parse(ParserNtLink.GetValue(pedimento[3]));
                        P.Aduana = short.Parse(ParserNtLink.GetValue(pedimento[4]));
                        P.AgenteAduanal = short.Parse(ParserNtLink.GetValue(pedimento[5]));
                        P.TipoPedimento = ParserNtLink.GetValue(pedimento[6]);
                        P.FechaPedimento = DateTime.Parse(ParserNtLink.GetValue(pedimento[7]));
                        P.FechaReciboLaredo = DateTime.Parse(ParserNtLink.GetValue(pedimento[8]));
                        P.FechaBillOfLading = DateTime.Parse(ParserNtLink.GetValue(pedimento[9]));
                        listaPedimento.Add(P);
                    }
                    H.Pedimento = listaPedimento.ToArray();
                }

                if (SPedidos.Any())
                {

                    List<SorianaAdendas.DSCargaRemisionProvPedidos> listaPedido = new List<SorianaAdendas.DSCargaRemisionProvPedidos>();
                    foreach (var pedido in SPedidos)
                    {

                        SorianaAdendas.DSCargaRemisionProvPedidos D = new SorianaAdendas.DSCargaRemisionProvPedidos();

                        D.Proveedor = int.Parse(ParserNtLink.GetValue(pedido[1]));
                        D.Remision = ParserNtLink.GetValue(pedido[2]);
                        D.FolioPedido = int.Parse(ParserNtLink.GetValue(pedido[3]));
                        D.Tienda = short.Parse(ParserNtLink.GetValue(pedido[4]));
                        D.CantidadArticulos = short.Parse(ParserNtLink.GetValue(pedido[5]));
                        if (!string.IsNullOrEmpty(pedido[6]))
                        {
                            if (pedido[6]=="SI")
                            D.PedidoEmitidoProveedor = SorianaAdendas.DSCargaRemisionProvPedidosPedidoEmitidoProveedor.SI;
                            if (pedido[6] == "NO")
                                D.PedidoEmitidoProveedor = SorianaAdendas.DSCargaRemisionProvPedidosPedidoEmitidoProveedor.NO;
                       
                            D.PedidoEmitidoProveedorSpecified = true;
                        }
                        else
                            D.PedidoEmitidoProveedorSpecified = false;
                       
                        listaPedido.Add(D);
                    }
                    H.Pedidos = listaPedido.ToArray();
                }
               
                if (SArticulos.Any())
                {

                    List<SorianaAdendas.DSCargaRemisionProvArticulos> listaArticulos = new List<SorianaAdendas.DSCargaRemisionProvArticulos>();
                    foreach (var articulos in SArticulos)
                    {
                        
                        SorianaAdendas.DSCargaRemisionProvArticulos A = new SorianaAdendas.DSCargaRemisionProvArticulos();
                        A.Proveedor =int.Parse(ParserNtLink.GetValue( articulos[1]));
                        A.Remision = ParserNtLink.GetValue(articulos[2]);
                        A.FolioPedido =int.Parse(ParserNtLink.GetValue( articulos[3]));
                        A.Tienda = short.Parse(ParserNtLink.GetValue( articulos[4]));
                        A.Codigo = decimal.Parse(ParserNtLink.GetValue( articulos[5]));
                        A.CantidadUnidadCompra = decimal.Parse(ParserNtLink.GetValue( articulos[6]));
                        A.CostoNetoUnidadCompra = decimal.Parse(ParserNtLink.GetValue( articulos[7]));
                        A.PorcentajeIEPS = decimal.Parse(ParserNtLink.GetValue( articulos[8]));
                        A.PorcentajeIVA = decimal.Parse(ParserNtLink.GetValue(articulos[9]));
                        listaArticulos.Add(A);
                    }
                    H.Articulos = listaArticulos.ToArray();
                }
                
               

                return H;


            }
            return null;

        }
    }
}
