using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneradorCfdi;
using log4net;

namespace TxtToCfdi
{
    public class ParserPemex : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserPemex));
        public List<Comprobante> Parse(string fileName)
        {
            try
            {
                var data = ParserNtLink.GetFileData(fileName);
                var comp = new ParserNtLink().ParseData(data);
                // parsear parte de la addenda
                var addendaPemex = data.FirstOrDefault(p => p[0] == "AP");
                if (addendaPemex != null)
                {
                    var ap = new AddendaPemex()
                             {
                                 CONTRATO = ParserNtLink.GetValue(addendaPemex[1]),
                                 O_SURTIMIENTO = ParserNtLink.GetValue(addendaPemex[2]),
                                 N_ESTIMACION = ParserNtLink.GetValue(addendaPemex[3]),
                                 P_ESTIMACION = ParserNtLink.GetValue(addendaPemex[4]),
                                 N_ACREEDOR = ParserNtLink.GetValue(addendaPemex[5]),
                                 C_GESTOR = ParserNtLink.GetValue(addendaPemex[6]),
                                 FINIQUITO = ParserNtLink.GetValue(addendaPemex[7]),
                                 POSICIONAP = ParserNtLink.GetValue(addendaPemex[8]),
                                 AUTORIZA = ParserNtLink.GetValue(addendaPemex[9]),
                                 ENTRADA = ParserNtLink.GetValue(addendaPemex[10]),
                                 EJERCICIO = ParserNtLink.GetValue(addendaPemex[11]),
                                 CEJECUTOR = ParserNtLink.GetValue(addendaPemex[12]),
                                 RECEPSAP = ParserNtLink.GetValue(addendaPemex[13]),
                                 NREMISION = ParserNtLink.GetValue(addendaPemex[14]),
                                 PLAZO = ParserNtLink.GetValue(addendaPemex[15]),
                                 RFCPROVEEDOR = ParserNtLink.GetValue(addendaPemex[16]),
                                 REMESA = ParserNtLink.GetValue(addendaPemex[17]),
                                 VUREGION = ParserNtLink.GetValue(addendaPemex[18]),
                                 FICHAE = ParserNtLink.GetValue(addendaPemex[19]),
                                 FICHAF = ParserNtLink.GetValue(addendaPemex[20]),
                                 MONEDA = ParserNtLink.GetValue(addendaPemex[21]),
                                 FONDO = ParserNtLink.GetValue(addendaPemex[22]),
                                 POSICIONF = ParserNtLink.GetValue(addendaPemex[23]),
                                 CLAVE_TRANSP = ParserNtLink.GetValue(addendaPemex[24]),
                                 A_RELACION = ParserNtLink.GetValue(addendaPemex[25]),
                                 ID_ANALITICO = ParserNtLink.GetValue(addendaPemex[26]),
                                 TIPO_PRODUCTO = ParserNtLink.GetValue(addendaPemex[27]),
                                 CEDULA = ParserNtLink.GetValue(addendaPemex[28]),
                                 CONTRATO_SIIC = ParserNtLink.GetValue(addendaPemex[29]),
                                 OCOMERCIAL = ParserNtLink.GetValue(addendaPemex[30]),
                                 SERVICIOG = ParserNtLink.GetValue(addendaPemex[31]),
                                 SERVICIOA = ParserNtLink.GetValue(addendaPemex[32]),
                                 CORREOPMI = ParserNtLink.GetValue(addendaPemex[33]),
                                 ANALITICO = ParserNtLink.GetValue(addendaPemex[34])
                             };
                    comp.xsiSchemaLocation = comp.xsiSchemaLocation + " http://pemex.com/facturaelectronica/addenda/v2 https://pemex.reachcore.com/schemas/addenda-pemex-v2.xsd";
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(ap, typeof(AddendaPemex), "pm", "http://pemex.com/facturaelectronica/addenda/v2");
                    comp.XmlAdenda =
                         comp.XmlAdenda.Replace(
                             "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" ",
                             "");
                }
               
                
                List<Comprobante> lista = new List<Comprobante>();
                lista.Add(comp);
                return lista;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                return null;
            }

           
        }
    }
}
