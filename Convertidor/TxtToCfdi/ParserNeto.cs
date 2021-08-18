using System.Collections.Generic;
using System.Linq;
using GeneradorCfdi;

namespace TxtToCfdi
{
    public class ParserNeto : IParser
    {
        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);
            

            if (comp != null)
            {
                var addenda = this.GetNetoAddendaInfo(data);
                if (addenda != null)
                {
                    //comp.AddendaSoriana = adenda;
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(addenda, typeof(ap), "ap", "http://www.tiendasneto.com/ap");
                }
                res.Add(comp);
            }
            return res;
        }

        private ap GetNetoAddendaInfo(string[][] data)
        {
            //var adenda = this.GetSorianaAdenda(data);
            var ap = data.FirstOrDefault(p => p[0] == "ap");
            var detalle = data.FirstOrDefault(p => p[0] == "Detalle");
            var productos = data.Where(p => p[0] == "Producto").ToList();

            if (ap == null && detalle == null && productos.Count == 0)
            {
                return null;
            }

            ap addenda = new ap();
            addenda.tipoComprobante = apTipoComprobante.FE;
            addenda.plazoPago = ap[2];
            addenda.observaciones = ap[3];
            List<apDetalleProducto> prods = new List<apDetalleProducto>();
            var det = new apDetalle();
            det.folio = detalle[1];
            foreach (var prod in productos)
            {
                var producto = new apDetalleProducto();
                producto.codigoBarras = prod[1];
                producto.cajasEntregadas = decimal.Parse(prod[2]);
                producto.precioUnitarioCaja = decimal.Parse(prod[3]);
                producto.piezasEntregadas = decimal.Parse(prod[4]);
                producto.precioUnitarioPieza = decimal.Parse(prod[5]);
                producto.Impuestos = new apDetalleProductoImpuestos();
                producto.Impuestos.Traslados = new[] { 
                        new apDetalleProductoImpuestosTraslado(){
                             impuesto = apDetalleProductoImpuestosTrasladoImpuesto.IVA,
                             tasa = decimal.Parse(prod[6]),
                             importe = decimal.Parse(prod[7])
                        },
                        new apDetalleProductoImpuestosTraslado(){
                             impuesto = apDetalleProductoImpuestosTrasladoImpuesto.IEPS,
                             tasa = decimal.Parse(prod[8]),
                             importe = decimal.Parse(prod[9])
                        }
                       
                    };
                producto.Impuestos.totalImpuestosTrasladados = decimal.Parse(prod[10]);
                producto.Impuestos.totalImpuestosTrasladadosSpecified = true;
                prods.Add(producto);

            }
            det.Producto = prods.ToArray();
            addenda.Detalle = new[] { det };
            return addenda;
        }
    }
}
