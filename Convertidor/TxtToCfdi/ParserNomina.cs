using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneradorCfdi;

namespace TxtToCfdi
{
    public class ParserNomina : IParser
    {
       public List<Comprobante> Parse(string fileName)
       {
           var res = new List<Comprobante>();
           var data = ParserNtLink.GetFileData(fileName);
           var comp = new ParserNtLink().ParseNominaData(data);

           if (comp != null)
           {
              
               res.Add(comp);
           }
           return res;
       }
    }
}
