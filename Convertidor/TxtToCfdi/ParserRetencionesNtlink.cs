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
//using ServicioLocal.Business;
using ConvertidorCfdi;

namespace TxtToCfdi
{
    public class ParserRetencionesNtlink : IParserR
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserRetencionesNtlink));

        public static string GetValue(string input)
        {
            if (input == "")
                return null;
            return input;
        }

        public static string GetValue2(string input)
        {
            if (input == "")
                return null;
            return input.Replace("\\","\r\n");
        }

        public static string[][] GetFileData(string fileName)
        {
            if (!File.Exists(fileName))
                throw new ApplicationException("El archivo no existe");
            var datos = File.ReadAllLines(fileName, Encoding.Default).Select(p => p.Split('|')).ToArray();

            if (datos.Length == 0)
            {
                throw new Exception("El archivo " + fileName + " está vacío");
            }
            return datos;
        }

        public Retenciones ParseData(string[][] datos)
        {
            #region Obtención de datos
            var comprob = datos.FirstOrDefault(p => p[0] == "RETENCION");
            var emisor = datos.FirstOrDefault(p => p[0] == "E");
            var receptor = datos.FirstOrDefault(p => p[0] == "R");
            var periodo = datos.FirstOrDefault(p => p[0] == "P");
            var Totales = datos.FirstOrDefault(p => p[0] == "Totales");//nuevo
            var ImpRetenidos = datos.Where(p => p[0] == "ImpRetenidos");
            var dividendos = datos.FirstOrDefault(p => p[0] == "DIV");
            var DividOUtil = datos.FirstOrDefault(p => p[0] == "DividOUtil");
                    
          
            #endregion
            #region Emisor

            Retenciones comprobante = new Retenciones();

            comprobante.Emisor = new RetencionesEmisor();
            comprobante.Emisor.NomDenRazSocE = emisor[1];
            comprobante.Emisor.RFCEmisor = emisor[2]; 
            
            if (!string.IsNullOrEmpty(emisor[3] ))
                comprobante.Emisor.CURPE = emisor[3];
            
            #endregion

            comprobante.Receptor = new RetencionesReceptor();
            //comprobante.IdReceptor = cliente.idCliente;
            comprobante.Receptor.Emails = receptor[6];
            comprobante.Receptor.Bcc = receptor[7];
          //  comprobante.Receptor.direccion = receptor[8];

            //-----------------------------------------------------------------------------
            if (receptor[1] == "Nacional")
            {
                comprobante.Receptor.Nacionalidad = RetencionesReceptorNacionalidad.Nacional;
                RetencionesReceptorNacional receptorNacional = new RetencionesReceptorNacional();
                receptorNacional.NomDenRazSocR = receptor[2];
                receptorNacional.RFCRecep = receptor[3];
                if (!string.IsNullOrEmpty(receptor[4]))
                    receptorNacional.CURPR = receptor[4];
                comprobante.Receptor.Item = receptorNacional;
            }
            else
            {
                comprobante.Receptor.Nacionalidad = RetencionesReceptorNacionalidad.Extranjero;
                RetencionesReceptorExtranjero receptorExtranjero = new RetencionesReceptorExtranjero();
                receptorExtranjero.NomDenRazSocR = receptor[2];
                if (!string.IsNullOrEmpty(receptor[5]))
                    receptorExtranjero.NumRegIdTrib = receptor[5];
                comprobante.Receptor.Item = receptorExtranjero;
            }
            //_---------------------------------------------------------------------

            comprobante.Periodo = new RetencionesPeriodo();
            comprobante.Periodo.MesIni =Convert.ToInt16( periodo[1]);
            comprobante.Periodo.MesFin =Convert.ToInt16( periodo[2]);
            comprobante.Periodo.Ejerc =Convert.ToInt16( periodo[3]);

            comprobante.CveRetenc = comprob[1];
            if(!string.IsNullOrEmpty(comprob[2]))
            comprobante.DescRetenc = comprob[2];
            
            comprobante.FolioInt = comprob[3];
            comprobante.Version = "1.0";
            if (comprob.Count() > 3)
                comprobante.Observacion = comprob[4];

            comprobante.Totales = new RetencionesTotales();
            comprobante.Totales.montoTotOperacion =Convert.ToDecimal( Totales[1]);
            comprobante.Totales.montoTotGrav =Convert.ToDecimal( Totales[2]);
            comprobante.Totales.montoTotExent =Convert.ToDecimal( Totales[3]);
            comprobante.Totales.montoTotRet =Convert.ToDecimal( Totales[4]);


            if (ImpRetenidos.Any())
            {
                List<RetencionesTotalesImpRetenidos> impuestosRetenidos = new List<RetencionesTotalesImpRetenidos>();
                foreach (var item in ImpRetenidos)
                {
                    RetencionesTotalesImpRetenidos rt = new RetencionesTotalesImpRetenidos();
                       if(!string.IsNullOrEmpty( item[1]))
                       {
                        rt.BaseRet =Convert.ToDecimal( item[1]);
                           rt.BaseRetSpecified = true;
                       }
                       else
                               rt.BaseRetSpecified = false;
                     if(!string.IsNullOrEmpty( item[2]))
                      {
                        rt.Impuesto = item[2];
                        rt.ImpuestoSpecified = true;

                      }
                      else
                        rt.ImpuestoSpecified = false;

                        rt.TipoPagoRet = item[3];
                        rt.montoRet =Convert.ToDecimal( item[4]);

                        impuestosRetenidos.Add(rt);
                }
                comprobante.Totales.ImpRetenidos = impuestosRetenidos.ToArray();
            }
            //-----------agragando las retenciones---------------------------------------------------
            if (dividendos != null)
            {
                if (comprobante.Complemento == null)
                    comprobante.Complemento = new RetencionesComplemento();

                comprobante.Complemento.dividendos = new Dividendos();
                comprobante.Version = "1.0";
                if (DividOUtil != null)
                {
                    comprobante.Complemento.dividendos.DividOUtil = new DividendosDividOUtil();
                    comprobante.Complemento.dividendos.DividOUtil.CveTipDivOUtil = DividOUtil[1];
                    if (!string.IsNullOrEmpty(DividOUtil[2]))
                    {
                        comprobante.Complemento.dividendos.DividOUtil.MontDivAcumExtSpecified = true;
                        comprobante.Complemento.dividendos.DividOUtil.MontDivAcumExt = Convert.ToDecimal(DividOUtil[2]);
                    }
                    else
                        comprobante.Complemento.dividendos.DividOUtil.MontDivAcumExtSpecified = false;
                    if (!string.IsNullOrEmpty(DividOUtil[3]))
                    {
                        comprobante.Complemento.dividendos.DividOUtil.MontDivAcumNalSpecified = true;
                        comprobante.Complemento.dividendos.DividOUtil.MontDivAcumNal = Convert.ToDecimal(DividOUtil[3]);
                    }
                    else
                        comprobante.Complemento.dividendos.DividOUtil.MontDivAcumNalSpecified = false;

                    comprobante.Complemento.dividendos.DividOUtil.MontISRAcredRetExtranjero = Convert.ToDecimal(DividOUtil[4]);
                    if (DividOUtil[5] == "SociedadExtranjera")
                        comprobante.Complemento.dividendos.DividOUtil.TipoSocDistrDiv = "Sociedad Extranjera";//DividendosDividOUtilTipoSocDistrDiv.SociedadExtranjera.ToString();
                    else
                        comprobante.Complemento.dividendos.DividOUtil.TipoSocDistrDiv = "Sociedad Nacional";//DividendosDividOUtilTipoSocDistrDiv.SociedadNacional.ToString();

                    if (!string.IsNullOrEmpty(DividOUtil[6]))
                    {
                        comprobante.Complemento.dividendos.DividOUtil.MontRetExtDivExtSpecified = true;
                        comprobante.Complemento.dividendos.DividOUtil.MontRetExtDivExt = Convert.ToDecimal(DividOUtil[6]);
                    }
                    else
                        comprobante.Complemento.dividendos.DividOUtil.MontRetExtDivExtSpecified = false;

                    comprobante.Complemento.dividendos.DividOUtil.MontISRAcredRetMexico = Convert.ToDecimal(DividOUtil[7]);

                    if (!string.IsNullOrEmpty(DividOUtil[8]))
                    {
                        comprobante.Complemento.dividendos.DividOUtil.MontISRAcredNalSpecified = true;
                        comprobante.Complemento.dividendos.DividOUtil.MontISRAcredNal = Convert.ToDecimal(DividOUtil[8]);
                    }
                    else
                        comprobante.Complemento.dividendos.DividOUtil.MontISRAcredNalSpecified = false;
                }
                //.....
                comprobante.Complemento.dividendos.Remanente = new DividendosRemanente();
                
                if (!string.IsNullOrEmpty(dividendos[1]))
                {
                    comprobante.Complemento.dividendos.Remanente.ProporcionRemSpecified = true;
                    comprobante.Complemento.dividendos.Remanente.ProporcionRem = Convert.ToDecimal(dividendos[1]);
                }
                else
                    comprobante.Complemento.dividendos.Remanente.ProporcionRemSpecified = false;
            }
            //-------------------------fin complementos---------------------------------
            comprobante.CantidadLetra = CantidadLetra.Enletras(comprobante.Totales.montoTotRet.ToString(), "MXN", comprobante.Emisor.RFCEmisor);

            return comprobante;

        }

       
        public List<Retenciones> Parse(string fileName)
        {
            var datos = GetFileData(fileName);
            var comp =  ParseData(datos);
            return new List<Retenciones>(){comp};
            
        }

    }
}
