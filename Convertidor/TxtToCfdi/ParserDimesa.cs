using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneradorCfdi;
using log4net;

namespace TxtToCfdi
{
    public class ParserDimesa: IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserPemex));
        public List<Comprobante> Parse(string fileName)
        {
            try
            {
                var data = ParserNtLink.GetFileData(fileName);
                var comp = new ParserNtLink().ParseData(data);
                if (comp != null)
                {
                    var axosFER = data.FirstOrDefault(p => p[0] == "axosFER");
                    var ordenDeCompra = data.FirstOrDefault(p => p[0] == "OrdenDeCompra[]");
                    var posicion = data.Where(p => p[0] == "Posicion[]");
                    if (axosFER != null && axosFER.Length > 0)
                    {
                        ECFD ap = new ECFD();
                        ap.version = axosFER[1];
                        ap.noProveedor = axosFER[2];
                        var oc = new ECFDOrdenDeCompra();
                        oc.numero = ParserNtLink.GetValue(ordenDeCompra[1]);
                        oc.moneda = ParserNtLink.GetValue(ordenDeCompra[2]);
                        oc.tipoCambio = ParserNtLink.GetValue(ordenDeCompra[3]);
                        var posiciones = new List<ECFDOrdenDeCompraPosicion>();
                        foreach (string[] pos in posicion)
                        {
                            ECFDOrdenDeCompraPosicion posi = new ECFDOrdenDeCompraPosicion();
                            posi.numero = ParserNtLink.GetValue(pos[1]);
                            posi.material = ParserNtLink.GetValue(pos[2]);
                            posi.cantidad = Decimal.Parse(pos[3]);
                            posi.unidad = ParserNtLink.GetValue2(pos[4]);
                            posi.precio = Decimal.Parse(pos[5]);
                            posi.total = Decimal.Parse(pos[6]);
                            posi.deliveryNote = ParserNtLink.GetValue(pos[7]);
                            posiciones.Add(posi);
                        }
                        oc.Posicion = posiciones.ToArray();
                        ap.OrdenDeCompra = new[] { oc };
                        comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(ap, typeof(ECFD), "axosFER",
                            "http://www.axosnet.com");
                        comp.XmlAdenda =
                            comp.XmlAdenda.Replace(
                                "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ",
                                "");
                        //comp.XmlAdenda = comp.XmlAdenda.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ", "");
                        
                    }
                    List<Comprobante> lista = new List<Comprobante>();
                    lista.Add(comp);
                    return lista;
                }
                else return null;
                // parsear parte de la addenda

            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return null;
            }


        }
    }
}
