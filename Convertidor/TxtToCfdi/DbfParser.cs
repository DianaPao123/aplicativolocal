using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using GeneradorCfdi;
using log4net;

namespace TxtToCfdi
{
    public class DbfParser : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserNtLink));

        public string Foldername { get; set; }
        public DbfDatosVenta DbfDatosVenta { get; set; }
        public DbfDatosCliente DbfDatosCliente { get; set; }
        public string NombreVendedor { get; set; }

        public DbfParser()
        {
            Foldername = @"C:\Users\chipx0r\Desktop\DbfFiles\";
        }

        public DbfParser(string folder)
        {
            Foldername = folder;
        }

        public void GetNotaFromDb(string noFolio)
        {
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Foldername + ";Extended Properties=dBASE IV;User ID=Admin;Password=;";
            using (OleDbConnection con = new OleDbConnection(constr))
            {
                string fileName = "M_DEVO.dbf";
                var sql = "select * from " + fileName;
                OleDbCommand cmd = new OleDbCommand(sql, con);
                con.Open();
                DataSet ds = new DataSet(); ;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                DataTable dataTable = ds.Tables[0];
                var query = dataTable.AsEnumerable().FirstOrDefault(i => i.Field<string>("FOLIO") == noFolio);
                if (query != null)
                {
                    DbfDatosVenta = new DbfDatosVenta();
                    DbfDatosVenta.CLIENTE = query["CLIENTE"] as string;
                    DbfDatosVenta.VENDEDOR = query["VENDEDOR"] as string;
                    //Console.WriteLine(DbfDatosVenta);
                    Logger.Info(DbfDatosVenta);
                }
            }
        }


        public void GetVentaFromDb(string noFactura)
        {
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Foldername + ";Extended Properties=dBASE IV;User ID=Admin;Password=;";
            using (OleDbConnection con = new OleDbConnection(constr))
            {
                string fileName = "M_VENT.dbf";
                var sql = "select * from " + fileName;
                OleDbCommand cmd = new OleDbCommand(sql, con);
                con.Open();
                DataSet ds = new DataSet(); ;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                DataTable dataTable = ds.Tables[0];
                var query = dataTable.AsEnumerable().FirstOrDefault(i => i.Field<string>("FACTURA") == noFactura);
                if (query != null)
                {
                    DbfDatosVenta = new DbfDatosVenta();
                    DbfDatosVenta.VENTA = query["VENTA"] as string;
                    DbfDatosVenta.CLIENTE = query["CLIENTE"] as string;
                    DbfDatosVenta.VENDEDOR = query["VENDEDOR"] as string;
                    DbfDatosVenta.ORDCOMP = query["ORDCOMP"] as string;
                    // Entrega
                    DbfDatosVenta.DIRECCION2 = query["DIRECCION2"] as string;
                    DbfDatosVenta.COLONIA2 = query["COLONIA2"] as string;
                    DbfDatosVenta.CIUDAD2 = query["CIUDAD2"] as string;
                    DbfDatosVenta.ESTADO2 = query["ESTADO2"] as string;
                    DbfDatosVenta.TEL3 = query["TEL3"] as string;
                    DbfDatosVenta.CONTACTO3 = query["CONTACTO3"] as string;
                    DbfDatosVenta.HORARIO = query["HORARIO"] as string;
                    DbfDatosVenta.ENTRE1 = query["ENTRE1"] as string;
                    DbfDatosVenta.ENTRE2 = query["ENTRE2"] as string;
                    DbfDatosVenta.CONT = query["CONT"] as string;
                    // Fin entrega

                    //Console.WriteLine(DbfDatosVenta);
                    Logger.Info(DbfDatosVenta);
                }
            }
        }

        public void GetClienteFromDb()
        {
            if(DbfDatosVenta != null)
                GetClienteFromDb(DbfDatosVenta.CLIENTE);
        }

        public void GetClienteFromDb(string numeroCliente)
        {
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Foldername + ";Extended Properties=dBASE IV;User ID=Admin;Password=;";
            using (OleDbConnection con = new OleDbConnection(constr))
            {
                string fileName = "M_CLIE.dbf";
                var sql = "select * from " + fileName;
                OleDbCommand cmd = new OleDbCommand(sql, con);
                con.Open();
                DataSet ds = new DataSet(); ;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                DataTable dataTable = ds.Tables[0];
                var values = dataTable.AsEnumerable().FirstOrDefault(i => i.Field<string>("CLIENTE") == numeroCliente);
                if (values != null)
                {
                    DbfDatosCliente = new DbfDatosCliente();
                    DbfDatosCliente.Nombre = values[1] as string;
                    DbfDatosCliente.Nombre2 = values[10] as string;
                    DbfDatosCliente.Nombre3 = values[11] as string;
                    DbfDatosCliente.CALLE = values[18] as string;
                    DbfDatosCliente.NUMERO = values[93] as string;
                    DbfDatosCliente.COLONIA = values[19] as string;
                    DbfDatosCliente.ESTADO = values[20] as string;
                    DbfDatosCliente.CIUDAD = values[21] as string;
                    DbfDatosCliente.CP = values[22] as string;
                    DbfDatosCliente.RFC = values[24] as string;

                    DbfDatosCliente.CONTACTO = values[27] as string;
                    DbfDatosCliente.CONTACTO2 = values[28] as string;
                    DbfDatosCliente.EMAIL = values[29] as string;
                    DbfDatosCliente.DIAS = values[41] is double ? (double) values[41] : 0d;
                    DbfDatosCliente.REVISION = values[42] as string;
                    DbfDatosCliente.PAGOS = values[43] as string;
                    DbfDatosCliente.FEMETOPA = values[99] as string;
                    DbfDatosCliente.FENUMCTA = values[100] as string;
                    //Console.WriteLine(DbfDatosCliente);
                    Logger.Info(DbfDatosCliente);
                }
            }
        }

        public void GetVendedorFromDb()
        {
            if(DbfDatosVenta != null)
                GetVendedorFromDb(DbfDatosVenta.VENDEDOR);
        }

        public void GetVendedorFromDb(string idVendedor)
        {

            string fileName = "M_VEND.dbf";
            string constr = "Provider=VFPOLEDB;Data Source=" + Foldername + ";";
            using (OleDbConnection con = new OleDbConnection(constr))
            {
                var sql = "select * from " + fileName;
                OleDbCommand cmd = new OleDbCommand(sql, con);
                con.Open();
                DataSet ds = new DataSet(); ;
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                DataTable dataTable = ds.Tables[0];
                var query = dataTable.AsEnumerable().FirstOrDefault(i => i.Field<string>("VENDEDOR") == idVendedor);
                if (query != null)
                {
                    //Console.WriteLine(query["NOMBRE"]);
                    NombreVendedor = ((string) query["NOMBRE"]).Trim();
                    Logger.Info(NombreVendedor);
                }
            }            
        }

        public List<Comprobante> Parse(string fileName)
        {
            Cfd22Parser cfd22Parser = new Cfd22Parser();

            Logger.Info(fileName);
            List<Comprobante> c = cfd22Parser.Parse(fileName);
            if(c.Count > 0)
            {
                foreach (var comprobante in c)
                {
                    //comprobante.folio
#if DEBUG
                    comprobante.Fecha = DateTime.Now.ToString();
#endif
                    String noFactura = null;
                    if(comprobante.TipoDeComprobante == "I")
                    {
                        //Factura
                        noFactura = comprobante.Serie + "0" + comprobante.Folio;
                        Logger.Info(noFactura);
                        GetVentaFromDb(noFactura);
                    }
                    else
                    {
                        //Nota Credito
                        noFactura = comprobante.Serie + "00" + comprobante.Folio;
                        Logger.Info(noFactura);
                        GetNotaFromDb(noFactura);
                    }
                    
                    GetClienteFromDb();
                    GetVendedorFromDb();
                    if (DbfDatosVenta != null)
                    {
                        comprobante.DbfDatosVenta = DbfDatosVenta;
                        comprobante.DbfDatosCliente = DbfDatosCliente;
                        comprobante.DbfNombreVendedor = NombreVendedor;
                    }
                }
                return c;
            }
            return null;
        }
    }
}
