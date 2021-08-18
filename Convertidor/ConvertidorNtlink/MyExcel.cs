using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;
using log4net;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Globalization;

namespace ConvertidorCfdi
{
    class MyExcel
    {
        private class ComObject<TType> : IDisposable
        {
            public TType Instance { get; set; }

            public ComObject(TType instance)
            {
                this.Instance = instance;
            }

            public void Dispose()
            {
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(this.Instance);
            }
        } 


        private static readonly ILog Logger = LogManager.GetLogger(typeof(MyExcel));
     
        private static void creaArchivo(string dato, string nombre)
        {
            /*
          using (StreamWriter outputFile = new StreamWriter(@""+nombre+".txt")) {
          //foreach (string line in lines)
           // outputFile.WriteLine(line);
              outputFile.Write(dato,false, Encoding.ASCII);
           }
            */
          using (StreamWriter writer = new StreamWriter(@"" + nombre + ".txt", true, Encoding.UTF8))
          {
              writer.Write(dato);
          }
        
        }

        public void read_file(string xlsFilePath)
        {
            try
            {
                xlsFilePath = xlsFilePath.Replace(@"\\", @"\");
                if (!File.Exists(xlsFilePath))
                    return;
                string nombre = Path.GetFileNameWithoutExtension(xlsFilePath);
                string directoryName = Path.GetDirectoryName(xlsFilePath);
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;
                var misValue = Type.Missing;//System.Reflection.Missing.Value;

                // abrir el documento

                xlApp = new Excel.Application();// .ApplicationClass();

                xlWorkBook = xlApp.Workbooks.Open(xlsFilePath, misValue, misValue,
                    misValue, misValue, misValue, misValue, misValue, misValue,
                    misValue, misValue, misValue, misValue, misValue, misValue);

                // seleccion de la hoja de calculo
                // get_item() devuelve object y numera las hojas a partir de 1
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                // seleccion rango activo
                range = xlWorkSheet.UsedRange;


                // leer las celdas
                int rows = range.Rows.Count;
                int cols = range.Columns.Count;
                cols = 30;

                string str_value = "";
                int cont = 1;
                for (int row = 1; row <= rows; row++)
                {
                    var z = (range.Cells[row, 1] as Excel.Range).Value2;
                    if (z == null)//salto
                    {
                        break; //salida fin
                    }
                    for (int col = 1; col <= cols; col++)
                    {
                        // lectura como cadena
                        var x = (range.Cells[row, col] as Excel.Range).Value2;
                        // if (x == null)//salto
                        // {
                        //     str_value = str_value + "\n";
                        //     break;
                        // }

                        if (x != null)
                        {
                            if (x.ToString() == "FILN")
                            {
                                str_value = str_value + "\n";
                                break;
                            }
                            if (x.ToString() == "FIN" || x.ToString() == "FIN|")
                            {
                                str_value = str_value + x;

                                creaArchivo(str_value, directoryName + "\\" + nombre + cont);
                                cont++;
                                str_value = "";
                                break;
                            }
                            else
                                str_value = str_value + x + "|";
                        }
                        else
                            str_value = str_value + x + "|";

                    }



                }


                // cerrar
                xlWorkBook.Close(false, misValue, misValue);
                xlApp.Quit();

                // liberar
                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                int idproc = GetIDProcces("EXCEL");

                if (idproc != -1)
                {
                    Process.GetProcessById(idproc).Kill();
                }
                File.Delete(xlsFilePath);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error en el proceso de generar txt");
                Logger.Info("Error en el proceso de generar txt");

            }
            finally
            {
                GC.Collect();
            }
        }
 
 
       

        public void LeerExcelFile(string fullFilePath)
        {
            fullFilePath = fullFilePath.Replace(@"\\", @"\");
            if (!File.Exists(fullFilePath))
                return;
            string nombre = Path.GetFileNameWithoutExtension(fullFilePath);
            string directoryName = Path.GetDirectoryName(fullFilePath);
            //---------------------problemas con el excel manda que esta bloqueado-----------
            try
            {

                if (Directory.Exists("C:\\Windows\\SysWOW64"))
                {
                    if (!Directory.Exists("C:\\Windows\\SysWOW64\\config\\systemprofile\\Desktop"))
                        Directory.CreateDirectory("C:\\Windows\\SysWOW64\\config\\systemprofile\\Desktop\\");
                }
                else
                {
                    if (!Directory.Exists("C:\\Windows\\System32\\config\\systemprofile\\Desktop"))
                        Directory.CreateDirectory("C:\\Windows\\System32\\config\\systemprofile\\Desktop\\");

                }
            }
            catch (Exception ex) { }
           //----------------------------------------------------
            using (var comApplication = new ComObject<Application>(new Application()))
            {
                var excelInstance = comApplication.Instance;
                excelInstance.Visible = true;
                excelInstance.DisplayAlerts = true;
              
                try
                {
                    using (var workbooks = new ComObject<Workbooks>(excelInstance.Workbooks))
                    {

                       // var misValue = Type.Missing;//System.Reflection.Missing.Value;
                        //var workbook = workbooks.Instance.Open(fullFilePath, misValue, misValue,
                        // misValue, misValue, misValue, misValue, misValue, misValue,
                        // misValue, misValue, misValue, misValue, misValue, misValue);

                        //var workbook = workbooks.Instance.Open(fullFilePath, 0, false, 5, "", "", false, 
                        //Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, false, null, null);

                        var workbook = workbooks.Instance.Open(fullFilePath, 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t", true, false, 0, false, true, true);

                        var worksheet = (Excel.Worksheet)workbook.Worksheets.get_Item(1);
                        Excel.Range range;

                        range = worksheet.UsedRange;

                        // leer las celdas
                        int rows = range.Rows.Count;
                        int cols = range.Columns.Count;
                        cols = 30;
                        string str_value = "";
                        int cont = 1;
                        for (int row = 1; row <= rows; row++)
                        {
                            var z = (range.Cells[row, 1] as Excel.Range).Value2;
                            if (z == null)//salto
                            {
                                break; //salida fin
                            }
                            for (int col = 1; col <= cols; col++)
                            {
                                // lectura como cadena
                                var x = (range.Cells[row, col] as Excel.Range).Value2;
                                if (x != null)
                                {
                                    if (x.ToString() == "FILN")
                                    {
                                        str_value = str_value + "\n";
                                        break;
                                    }
                                    if (x.ToString() == "FIN" || x.ToString() == "FIN|")
                                    {
                                        str_value = str_value + x;

                                        creaArchivo(str_value, directoryName + "\\" + nombre + cont);
                                        cont++;
                                        str_value = "";
                                        break;
                                    }
                                    else
                                        str_value = str_value + x + "|";
                                }
                                else
                                    str_value = str_value + x + "|";

                            }

                        }
                        //-----------
                        File.Delete(fullFilePath);
                        Marshal.ReleaseComObject(range); 
                        Marshal.ReleaseComObject(worksheet);
                        Marshal.ReleaseComObject(workbook);


                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                    //throw;
                }
                finally
                {
                    // Close Excel instance 
                 
                    excelInstance.Quit();
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelInstance);

                    GC.Collect();
                    GC.WaitForPendingFinalizers(); 
                }
            }
        } 


        public static void releaseObject(object obj) 
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
               // Console.WriteLine("Unable to release the object(object:{0})", obj.ToString());
                Logger.Info("Unable to release the object(object:"+ obj.ToString()+")");
          
            }
            finally 
            {
                obj = null;
                GC.Collect();
            }
        }

        private static int GetIDProcces(string nameProcces)
        {

            try
            {
                Process[] asProccess = Process.GetProcessesByName(nameProcces);

                foreach (Process pProccess in asProccess)
                {
                    if (pProccess.MainWindowTitle == "")
                    {
                        return pProccess.Id;
                    }
                }

                return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


    }
}
 
