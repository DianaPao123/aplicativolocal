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
    public class ParserLowes : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserLowes));



        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetLowerAdenda(data,comp.Moneda);
                if (adenda != null)
                {
                    var xmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(Lowes), "", "www.colabora.com/Facturacion/addenda/AddendaLowesv1.0.xsd");

                      xmlAdenda = xmlAdenda.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "xsi:schemaLocation=\"www.colabora.com/Facturacion/addenda/AddendaLowesv1.0.xsd\"");


                    comp.XmlAdenda = xmlAdenda;
                }
                res.Add(comp);
            }
            return res;
        }

        private Lowes GetLowerAdenda(string[][] data, string moneda)
        {

            var Proveedor = data.FirstOrDefault(j => j[0] == "LowesProveedor");
            var Orden = data.FirstOrDefault(j => j[0] == "LowesOrden");
            var Comprob = data.FirstOrDefault(j => j[0] == "LowesComprobante");

            var Articulos = data.Where(j => j[0] == "LowesArticulo");

            Lowes L = new Lowes();
            L = null;
            //--------------------------------------------------
            if (Proveedor != null)
            {
                if (L == null)
                    L = new Lowes();
                LowesProveedor p = new LowesProveedor();
                p.id = ParserNtLink.GetValue(Proveedor[1]);
                // L.Proveedor=new LowesProveedor();
                L.Proveedor = new LowesProveedor();
                L.Proveedor = p;

            }
            //-----------------------------
            if (Orden != null)
            {
                if (L == null)
                    L = new Lowes();

                LowesOrden O = new LowesOrden();
                O.id = ParserNtLink.GetValue(Orden[1]);
                O.articulos = decimal.Parse(ParserNtLink.GetValue(Orden[2]));
                L.Orden = new LowesOrden();
                L.Orden = O;
            }
            //-----------------------------

            if (Comprob != null)
            {
                if (L == null)
                    L = new Lowes();
                LowesComprobante C = new LowesComprobante();
                if (moneda == "MXN")
                    C.moneda = LowesComprobanteMoneda.MXN;
                if (moneda == "USD")
                    C.moneda = LowesComprobanteMoneda.USD;
                C.subtotal = decimal.Parse(ParserNtLink.GetValue(Comprob[1]));
                C.serie = ParserNtLink.GetValue(Comprob[2]);
                C.folio = ParserNtLink.GetValue(Comprob[3]);

                L.Comprobante = new LowesComprobante();
                L.Comprobante = C;
            }
            //-----------------------------

            if (Articulos.Any())
            {
                if (L == null)
                    L = new Lowes();
                List<LowesArticulo> listaHondaConceptos = new List<LowesArticulo>();

                foreach (var concepto in Articulos)
                {
                    LowesArticulo A = new LowesArticulo();
                    A.id = ParserNtLink.GetValue(concepto[1]);
                    if (!string.IsNullOrEmpty(concepto[2]))
                        A.upc = ParserNtLink.GetValue(concepto[2]);
                    A.cantidad = decimal.Parse(ParserNtLink.GetValue(concepto[3]));

                    var X = obtenerUOM(ParserNtLink.GetValue(concepto[4]));

                    if (X != null)
                        A.uom = (LowesArticuloUom)X;

                    A.valorUnitario = decimal.Parse(ParserNtLink.GetValue(concepto[5]));
                    A.importe = decimal.Parse(ParserNtLink.GetValue(concepto[6]));
                    A.iva = decimal.Parse(ParserNtLink.GetValue(concepto[7]));
                    A.ieps = decimal.Parse(ParserNtLink.GetValue(concepto[8]));

                    listaHondaConceptos.Add(A);

                }
                L.Articulos = listaHondaConceptos.ToArray();

            }

            return L;

        }
        //---------------------------------------------------------------
        private LowesArticuloUom? obtenerUOM(string uom)
        {
            switch (uom)
            {
                case "AGG": return LowesArticuloUom.AGG;
                case "AUG": return LowesArticuloUom.AUG;
                case "BA": return LowesArticuloUom.BA;
                case "BBL": return LowesArticuloUom.BBL;
                case "BE": return LowesArticuloUom.BE;
                case "BG": return LowesArticuloUom.BG;
                case "BI": return LowesArticuloUom.BI;
                case "BJ": return LowesArticuloUom.BJ;
                case "BK": return LowesArticuloUom.BK;
                case "BX": return LowesArticuloUom.BX;
                case "C": return LowesArticuloUom.C;
                case "CA": return LowesArticuloUom.CA;
                case "CAR": return LowesArticuloUom.CAR;
                case "CBM": return LowesArticuloUom.CBM;
                case "CC": return LowesArticuloUom.CC;
                case "CFT": return LowesArticuloUom.CFT;
                case "CG": return LowesArticuloUom.CG;
                case "CGM": return LowesArticuloUom.CGM;
                case "CKG": return LowesArticuloUom.CKG;
                case "CM": return LowesArticuloUom.CM;
                case "CM2": return LowesArticuloUom.CM2;
                case "CM3": return LowesArticuloUom.CM3;
                case "CON": return LowesArticuloUom.CON;
                case "CR": return LowesArticuloUom.CR;
                case "CS": return LowesArticuloUom.CS;
                case "CT": return LowesArticuloUom.CT;
                case "CTN": return LowesArticuloUom.CTN;
                case "CU": return LowesArticuloUom.CU;
                case "CUR": return LowesArticuloUom.CUR;
                case "CY": return LowesArticuloUom.CY;
                case "CYG": return LowesArticuloUom.CYG;
                case "CYK": return LowesArticuloUom.CYK;
                case "D": return LowesArticuloUom.D;
                case "DEG": return LowesArticuloUom.DEG;
                case "DOZ": return LowesArticuloUom.DOZ;
                case "DPC": return LowesArticuloUom.DPC;
                case "DPR": return LowesArticuloUom.DPR;
                case "DS": return LowesArticuloUom.DS;
                case "EA": return LowesArticuloUom.EA;
                case "FBM": return LowesArticuloUom.FBM;
                case "FIB": return LowesArticuloUom.FIB;
                case "FT": return LowesArticuloUom.FT;
                case "FT2": return LowesArticuloUom.FT2;
                case "FT3": return LowesArticuloUom.FT3;
                case "FZ": return LowesArticuloUom.FZ;
                case "G": return LowesArticuloUom.G;
                case "GBQ": return LowesArticuloUom.GBQ;
                case "GCN": return LowesArticuloUom.GCN;
                case "GKG": return LowesArticuloUom.GKG;
                case "GL": return LowesArticuloUom.GL;
                case "GM": return LowesArticuloUom.GM;
                case "GR": return LowesArticuloUom.GR;
                case "GRL": return LowesArticuloUom.GRL;
                case "GRS": return LowesArticuloUom.GRS;
                case "GVW": return LowesArticuloUom.GVW;
                case "HND": return LowesArticuloUom.HND;
                case "HUN": return LowesArticuloUom.HUN;
                case "HZ": return LowesArticuloUom.HZ;
                case "IN": return LowesArticuloUom.IN;
                case "IN2": return LowesArticuloUom.IN2;
                case "IN3": return LowesArticuloUom.IN3;
                case "IRC": return LowesArticuloUom.IRC;
                case "IRG": return LowesArticuloUom.IRG;
                case "JR": return LowesArticuloUom.JR;
                case "JWL": return LowesArticuloUom.JWL;
                case "K": return LowesArticuloUom.K;
                case "KCAL": return LowesArticuloUom.KCAL;
                case "KG": return LowesArticuloUom.KG;
                case "KHZ": return LowesArticuloUom.KHZ;
                case "KM": return LowesArticuloUom.KM;
                case "KM2": return LowesArticuloUom.KM2;
                case "KM3": return LowesArticuloUom.KM3;
                case "KN": return LowesArticuloUom.KN;
                case "KPA": return LowesArticuloUom.KPA;
                case "KSB": return LowesArticuloUom.KSB;
                case "KTS": return LowesArticuloUom.KTS;
                case "KVA": return LowesArticuloUom.KVA;
                case "KVAR": return LowesArticuloUom.KVAR;
                case "KW": return LowesArticuloUom.KW;
                case "KWH": return LowesArticuloUom.KWH;
                case "L": return LowesArticuloUom.L;
                case "LB": return LowesArticuloUom.LB;
                case "LBS": return LowesArticuloUom.LBS;
                case "LIN": return LowesArticuloUom.LIN;
                case "LNM": return LowesArticuloUom.LNM;
                case "M": return LowesArticuloUom.M;
                case "M2": return LowesArticuloUom.M2;
                case "M3": return LowesArticuloUom.M3;
                case "MBQ": return LowesArticuloUom.MBQ;
                case "MC": return LowesArticuloUom.MC;
                case "MG": return LowesArticuloUom.MG;
                case "MHZ": return LowesArticuloUom.MHZ;
                case "ML": return LowesArticuloUom.ML;
                case "MM": return LowesArticuloUom.MM;
                case "MM2": return LowesArticuloUom.MM2;
                case "MM3": return LowesArticuloUom.MM3;
                case "MPA": return LowesArticuloUom.MPA;
                case "MWH": return LowesArticuloUom.MWH;
                case "NA": return LowesArticuloUom.NA;
                case "NO": return LowesArticuloUom.NO;
                case "ODE": return LowesArticuloUom.ODE;
                case "OSG": return LowesArticuloUom.OSG;
                case "OZ": return LowesArticuloUom.OZ;
                case "PACK": return LowesArticuloUom.PACK;
                case "PAL": return LowesArticuloUom.PAL;
                case "PC": return LowesArticuloUom.PC;
                case "PCS": return LowesArticuloUom.PCS;
                case "PDG": return LowesArticuloUom.PDG;
                case "PF": return LowesArticuloUom.PF;
                case "PFL": return LowesArticuloUom.PFL;
                case "PK": return LowesArticuloUom.PK;
                case "PO": return LowesArticuloUom.PO;
                case "PRS": return LowesArticuloUom.PRS;
                case "PT": return LowesArticuloUom.PT;
                case "PTG": return LowesArticuloUom.PTG;
                case "QT": return LowesArticuloUom.QT;
                case "RBA": return LowesArticuloUom.RBA;
                case "RHG": return LowesArticuloUom.RHG;
                case "RPM": return LowesArticuloUom.RPM;
                case "RUG": return LowesArticuloUom.RUG;
                case "SBE": return LowesArticuloUom.SBE;
                case "SF": return LowesArticuloUom.SF;
                case "SME": return LowesArticuloUom.SME;
                case "SQ": return LowesArticuloUom.SQ;
                case "SQM": return LowesArticuloUom.SQM;
                case "T": return LowesArticuloUom.T;
                case "THM": return LowesArticuloUom.THM;
                case "THS": return LowesArticuloUom.THS;
                case "TNV": return LowesArticuloUom.TNV;
                case "TON": return LowesArticuloUom.TON;
                case "TS": return LowesArticuloUom.TS;
                case "UOM": return LowesArticuloUom.UOM;
                case "V": return LowesArticuloUom.V;
                case "W": return LowesArticuloUom.W;
                case "WTS": return LowesArticuloUom.WTS;
                case "X": return LowesArticuloUom.X;
                case "YD": return LowesArticuloUom.YD;
                case "YD2": return LowesArticuloUom.YD2;
                case "YD3": return LowesArticuloUom.YD3;
             }

            return null;
        }
    }
}
