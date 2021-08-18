using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using GeneradorCfdi;
using System.Globalization;
using Elektra;
namespace TxtToCfdi
{
    public class ParserElektra: IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserNtLink));

        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);
            if (comp != null)
            {
                var FacturaInterfactura = data.FirstOrDefault(p => p[0] == "FacturaInterfactura");
                var Emisor = data.FirstOrDefault(p => p[0] == "Emisor");
                var Receptor = data.FirstOrDefault(p => p[0] == "Receptor");
                var Cuerpo = data.Where(p => p[0] == "Cuerpo[]").ToList();
                var encabezado = data.FirstOrDefault(p => p[0] == "Encabezado");
                if (FacturaInterfactura != null && FacturaInterfactura.Length > 0)
                {
                    FacturaInterfactura fIFact = new FacturaInterfactura();

                    try
                    {
                        fIFact.TipoDocumento = new t_TipoDocumento();
                        fIFact.TipoDocumento =
                            (t_TipoDocumento)Enum.Parse(typeof(t_TipoDocumento), FacturaInterfactura[1]);

                        fIFact.Emisor = new FacturaInterfacturaEmisor();
                        fIFact.Emisor.RI = Emisor[1];

                        fIFact.Receptor = new FacturaInterfacturaReceptor();
                        fIFact.Receptor.RI = Receptor[1];

                        fIFact.Encabezado = new FacturaInterfacturaEncabezado();
                        fIFact.Encabezado.TipoProveedorEKT = ParserNtLink.GetValue(encabezado[1]);
                        fIFact.Encabezado.Fecha = DateTime.ParseExact(encabezado[2], "yyyy-MM-ddThh:mm:ss",
                            CultureInfo.InvariantCulture);
                        fIFact.Encabezado.MonedaDoc =
                            (FacturaInterfacturaEncabezadoMonedaDoc)
                                Enum.Parse(typeof(FacturaInterfacturaEncabezadoMonedaDoc), encabezado[3]);
                        fIFact.Encabezado.SubTotal = Decimal.Parse(encabezado[4]);
                        fIFact.Encabezado.IVAPCT = ParserNtLink.GetValue(encabezado[5]);
                        fIFact.Encabezado.Iva = Decimal.Parse(encabezado[6]);
                        fIFact.Encabezado.SubTotalIva = Decimal.Parse(encabezado[7]);
                        if (!string.IsNullOrEmpty(encabezado[8]))
                        {
                            fIFact.Encabezado.ISRPCT = ParserNtLink.GetValue(encabezado[8]);
                            fIFact.Encabezado.ISRPCTSpecified = true;
                        }
                        if (!string.IsNullOrEmpty(encabezado[10]) && encabezado[10] != "0")
                        {
                            fIFact.Encabezado.ISR = decimal.Parse(encabezado[10]);
                            fIFact.Encabezado.ISRSpecified = true;
                        }
                        if (!string.IsNullOrEmpty(encabezado[12]) && encabezado[12] != "0")
                        {
                            fIFact.Encabezado.IvaRetPCT = Decimal.Parse(encabezado[12]);
                            fIFact.Encabezado.IvaRetPCTSpecified = true;
                        }
                        if (!string.IsNullOrEmpty(encabezado[14]) && encabezado[14] != "0")
                        {
                            fIFact.Encabezado.IvaRet = Decimal.Parse(encabezado[14]);
                            fIFact.Encabezado.IvaRetSpecified = true;
                        }
                        fIFact.Encabezado.Total = Decimal.Parse(encabezado[16]);
                        fIFact.Encabezado.NumProveedor = ParserNtLink.GetValue(encabezado[17]);
                        fIFact.Encabezado.FolioOrdenCompra = ParserNtLink.GetValue(encabezado[18]);
                        fIFact.Encabezado.Observaciones = ParserNtLink.GetValue(encabezado[19]);
                        var ris = new List<FacturaInterfacturaEncabezadoCuerpo>();
                        foreach (var ri in Cuerpo)
                        {
                            var cuerpo = new FacturaInterfacturaEncabezadoCuerpo();
                            cuerpo.Renglon = ParserNtLink.GetValue(ri[1]);
                            cuerpo.Cantidad = Decimal.Parse(ri[2]);
                            cuerpo.Concepto = ParserNtLink.GetValue(ri[3]);
                            cuerpo.PUnitario = Decimal.Parse(ri[4]);
                            cuerpo.Importe = Decimal.Parse(ri[5]);

                            ris.Add(cuerpo);
                        }
                        fIFact.Encabezado.Cuerpo = ris.ToArray();
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e.Message);
                    }
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(fIFact, typeof(FacturaInterfactura),
                        "if", "https://www.interfactura.com/Schemas/Documentos");
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
