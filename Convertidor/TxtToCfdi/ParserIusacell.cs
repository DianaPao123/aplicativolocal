using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using GeneradorCfdi;
using System.Globalization;
using Iusacell;
namespace TxtToCfdi
{
    public class ParserIusacell: IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserNtLink));

        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);
            if (comp != null)
            {
                // parsear parte de la addenda
                var FacturaInterfactura = data.Where(p => p[0] == "FacturaInterfactura").FirstOrDefault();
                var Emisor = data.Where(p => p[0] == "Emisor").FirstOrDefault();
                var Receptor = data.Where(p => p[0] == "Receptor").FirstOrDefault();
                var Cuerpo = data.Where(p => p[0] == "Cuerpo[]").ToList();
                var Encabezado = data.Where(p => p[0] == "Encabezado").FirstOrDefault();
                if (FacturaInterfactura != null && FacturaInterfactura.Length > 0)
                {
                    FacturaInterfactura fIFact = new FacturaInterfactura();

                    try
                    {
                        fIFact.TipoDocumento = new t_TipoDocumento();
                        fIFact.TipoDocumento =
                            (t_TipoDocumento) Enum.Parse(typeof (t_TipoDocumento), FacturaInterfactura[1]);

                        fIFact.Emisor = new FacturaInterfacturaEmisor();
                        fIFact.Emisor.RI = Emisor[1];

                        fIFact.Receptor = new FacturaInterfacturaReceptor();
                        fIFact.Receptor.RI = Receptor[1];

                        fIFact.Encabezado = new FacturaInterfacturaEncabezado();
                        fIFact.Encabezado.Fecha = DateTime.ParseExact(Encabezado[1], "yyyy-MM-ddThh:mm:ss",
                            CultureInfo.InvariantCulture);
                        fIFact.Encabezado.MonedaDoc =
                            (FacturaInterfacturaEncabezadoMonedaDoc)
                                Enum.Parse(typeof (FacturaInterfacturaEncabezadoMonedaDoc), Encabezado[2]);
                        if (ParserNtLink.GetValue(Encabezado[3]) == "Item10")
                        {
                            fIFact.Encabezado.IVAPCT = FacturaInterfacturaEncabezadoIVAPCT.Item10;
                        }
                        else
                        {
                            fIFact.Encabezado.IVAPCT = FacturaInterfacturaEncabezadoIVAPCT.Item10;
                        }
                        fIFact.Encabezado.Iva = Decimal.Parse(Encabezado[4]);
                        fIFact.Encabezado.SubTotal = Decimal.Parse(Encabezado[5]);
                        fIFact.Encabezado.Total = Decimal.Parse(Encabezado[6]);
                        fIFact.Encabezado.FechaEntrega = DateTime.ParseExact(Encabezado[7], "yyyy-MM-ddThh:mm:ss",
                            CultureInfo.InvariantCulture);
                        fIFact.Encabezado.LugarEntrega = ParserNtLink.GetValue(Encabezado[8]);
                        fIFact.Encabezado.LugarExpedicion = ParserNtLink.GetValue(Encabezado[9]);
                        fIFact.Encabezado.CondicionPago = ParserNtLink.GetValue(Encabezado[10]);
                        fIFact.Encabezado.NumProveedor = ParserNtLink.GetValue(Encabezado[11]);
                        fIFact.Encabezado.FolioOrdenCompra = ParserNtLink.GetValue(Encabezado[12]);
                        fIFact.Encabezado.Observaciones = ParserNtLink.GetValue(Encabezado[13]);

                        var ris = new List<FacturaInterfacturaEncabezadoCuerpo>();
                        foreach (var ri in Cuerpo)
                        {
                            var cuerpo = new FacturaInterfacturaEncabezadoCuerpo();
                            cuerpo.Renglon = ParserNtLink.GetValue(ri[1]);
                            cuerpo.Cantidad = Decimal.Parse(ri[2]);
                            cuerpo.Concepto = ParserNtLink.GetValue(ri[3]);
                            cuerpo.PUnitario = Decimal.Parse(ri[4]);
                            cuerpo.Importe = Decimal.Parse(ri[5]);
                            cuerpo.Material = ParserNtLink.GetValue(ri[6]);
                            cuerpo.UdeM = ParserNtLink.GetValue(ri[7]);
                            cuerpo.NumSerie = ParserNtLink.GetValue(ri[8]);
                            cuerpo.Observaciones = ParserNtLink.GetValue(ri[9]);

                            ris.Add(cuerpo);
                        }
                        fIFact.Encabezado.Cuerpo = ris.ToArray();
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e.Message);
                    }

                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(fIFact,
                        typeof (FacturaInterfactura), "if", "https://www.interfactura.com/Schemas/Documentos");
                    //comp.XmlAdenda =
                    //    comp.XmlAdenda.Replace(
                    //        "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ",
                    //        "");

                }
                List<Comprobante> lista = new List<Comprobante>();
                lista.Add(comp);
                return lista;
            }
            else return null;


        }
    }
}
