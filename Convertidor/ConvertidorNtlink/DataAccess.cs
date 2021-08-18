using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using GeneradorCfdi;
using log4net;
using log4net.Config;

namespace ConvertidorNtlink
{
    public class DataAccess
    {
        private static ILog Logger = LogManager.GetLogger(typeof(ClienteTimbradoNtlink));
        public DataAccess()
        {
            XmlConfigurator.Configure();
        }


        public bool ActualizarEstatusTimbrado(string folio, string serie, int status, string cnnStr)
        {
            try
            {
                //System.Diagnostics.Debugger.Launch();
                using (var con = new SqlConnection(cnnStr))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("update JCRG_CFDI_ORIGINAL set  " +
                         " [status_timbrado] = @status " +
                         " WHERE [serie] = @serie " +
                         " AND [folio] = @folio"
                        , con);
                    command.Parameters.AddWithValue("status", status);
                    command.Parameters.AddWithValue("serie", serie);
                    command.Parameters.AddWithValue("folio", folio);
                    foreach (IDataParameter param in command.Parameters)
                    {
                        if (param.Value == null) param.Value = DBNull.Value;
                    }
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                if (ee.InnerException != null)
                    Logger.Error(ee.InnerException);
                return false;
            }
        }


        public bool ActualizarEstatusCancelacion(string uuid, int status,string cnnStr)
        {
            try
            {
                //System.Diagnostics.Debugger.Launch();
                using (var con = new SqlConnection(cnnStr))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("update JCRG_CFDI_ORIGINAL set  " +
                         " [status_cancelacion] = @status" +
                         " WHERE [uuid] = @uuid"
                        , con);
                    command.Parameters.AddWithValue("uuid", uuid);
                    command.Parameters.AddWithValue("status", status);
                    foreach (IDataParameter param in command.Parameters)
                    {
                        if (param.Value == null) param.Value = DBNull.Value;
                    }
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                if (ee.InnerException != null)
                    Logger.Error(ee.InnerException);
                return false;
            }
        }


        public bool ActualizarCancelacion(string uuid, string sello, DateTime fechaCancelacion, string cnnStr)
        {
            try
            {
                using (var con = new SqlConnection(cnnStr))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("update JCRG_CFDI_ORIGINAL set  " +
                         " [fecha_cancelacion_sat] = @fecha," +
                         " [sello_cancelacion_sat] = @sello" +
                         " WHERE [uuid] = @uuid"
                        , con);
                    command.Parameters.AddWithValue("uuid", uuid);
                    command.Parameters.AddWithValue("sello", sello);
                    command.Parameters.AddWithValue("fecha", fechaCancelacion);
                    foreach (IDataParameter param in command.Parameters)
                    {
                        if (param.Value == null) param.Value = DBNull.Value;
                    }
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                if (ee.InnerException != null)
                    Logger.Error(ee.InnerException);
                return false;
            }
        }



        public bool ActualizarCfdi(string uuid, string xml, string serie, string folio, string cnnStr)
        {
            
            try
            {
                //System.Diagnostics.Debugger.Launch();
                using (var con = new SqlConnection(cnnStr))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("update JCRG_CFDI_ORIGINAL set  " +
                         " [uuid] = @uuid, " +
                         " [xml] = @xml "+ 
                         " WHERE [serie] = @serie " +
                         " AND [folio] = @folio"
                        , con);
                    command.Parameters.AddWithValue("uuid", uuid);
                    command.Parameters.AddWithValue("xml", xml);
                    command.Parameters.AddWithValue("serie", serie);
                    command.Parameters.AddWithValue("folio", folio);
                    foreach (IDataParameter param in command.Parameters)
                    {
                        if (param.Value == null) param.Value = DBNull.Value;
                    }
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                if (ee.InnerException != null)
                    Logger.Error(ee.InnerException);
                return false;
            }
        }

        public bool GuardarValidacion(string uuid, bool valido, string mensaje, string cnnStr)
        {
            
            try
            {
                using (var con = new SqlConnection(cnnStr))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO [dbo].[TimbreWs] " +
                           "([Uuid],[Valido],[Errores]) " +
                           "VALUES " +
                                   "(@uuid,@valido,@errores)", con);
                    command.Parameters.AddWithValue("uuid", uuid);
                    command.Parameters.AddWithValue("valido", valido);
                    command.Parameters.AddWithValue("errores", mensaje);
                    foreach (IDataParameter param in command.Parameters)
                    {
                        if (param.Value == null) param.Value = DBNull.Value;
                    }
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                if (ee.InnerException != null)
                    Logger.Error(ee.InnerException);
                return false;
            }
        }
    }
}
