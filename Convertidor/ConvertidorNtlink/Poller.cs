using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using log4net;
using log4net.Config;

namespace ConvertidorCfdi
{
    public class Poller
    {
        private Thread _worker;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Poller));
        private bool _activo;
        private readonly string _rutaArchivos;
        private Configuracion _conf;
        private static int rgv=0;
        //-
        
        public Action<List<string>,Configuracion > Trabajador { get; set; }
        //-

        public int Segundos { get; set; }

        public bool Activo
        {
            get { return _activo; }
        }


        public Poller(string rutaEntrada,Configuracion conf)
        {
            XmlConfigurator.Configure();
            _rutaArchivos = rutaEntrada;
            Logger.Info(conf.EmisorRfc + "->" + _rutaArchivos);
            _conf = conf;
            //Logger.Info(_rutaArchivosValidador);
           
        }


        public void Iniciar()
        {
            Logger.Info("Iniciando...");
            _activo = true;
            if (!Directory.Exists(_rutaArchivos))
                Directory.CreateDirectory(_rutaArchivos);

            //--------solo para archivos excel--------------
            
                //List<string> archivosExcel = Directory.EnumerateFiles(_rutaArchivos).Where(p => p.EndsWith("xlsx", StringComparison.InvariantCultureIgnoreCase)).ToList();
                //if (archivosExcel.Count() > 0)
                //{
                //    rgv = 1;
                //    foreach (var archivo in archivosExcel)
                //    {
                //        MyExcel excel = new MyExcel();
                //        excel.LeerExcelFile(archivo);
                //        // MyExcel.read_file(archivo);
                //        // Thread.Sleep(System.Threading.Timeout.Infinite);
                //        //excel.read_file(archivo);
                //    }
                     
                //}
            
            //---------------------------------------------
            
            _worker = new Thread(Trabajar);
            _worker.Start();
        }




        public void Trabajar()
        {
            try
            {
                while (_activo)
                {
                    try
                    {

                        List<string> archivosExcel = Directory.EnumerateFiles(_rutaArchivos).Where(p => p.EndsWith("xlsx", StringComparison.InvariantCultureIgnoreCase)).ToList();
                        if (archivosExcel.Count() > 0)
                        {
                            foreach (var archivo in archivosExcel)
                            {
                                MyExcel excel = new MyExcel();
                                //excel.read_file(archivo);
                                excel.LeerExcelFile(archivo);

                                // MyExcel.read_file(archivo);
                                // Thread.Sleep(System.Threading.Timeout.Infinite);
                                //excel.read_file(archivo);
                            }

                        }
                        
                        ///EDITAR PARA VALIDAR Y CANCELAR
                        List<string> archivos = Directory.EnumerateFiles(_rutaArchivos).Where(p => p.EndsWith("xml", StringComparison.InvariantCultureIgnoreCase) || p.EndsWith(".txt", StringComparison.InvariantCultureIgnoreCase)).ToList();
                        if (archivos.Count() > 0)
                        {
                            Logger.Info("Se encontraron " + archivos.Count() + " archivos");
                            StringBuilder sb = new StringBuilder();
                            foreach (var archivo in archivos)
                            {
                                Logger.Info(archivo);
                            }
                            if (Trabajador != null)
                            {
                                Trabajador(archivos,_conf);
                            }
                            
                        }
                        else
                        {
                            Logger.Info("No existen archivos, voya esperar " + Segundos + " segundos.");
                            for (int i = 1; i <= Segundos; i++)
                            {
                                if (_activo)
                                    Thread.Sleep(1000);
                                else break;
                            }

                        }
                        
                        
                        
                    }
                    catch (Exception ex)
                    {
                        Logger.Error( ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error( ex);
            }
        }


       
        public void Detener()
        {
            Logger.Info("Detenido, esperando a que termine la ultima tarea");
            _activo = false;
            _worker.Join();
            Logger.Info("Listo");
        }



    }
}
