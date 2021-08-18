using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneradorCfdi;

namespace TxtToCfdi
{
    public class ParserRetenciones : IParserR
    {
       public List<Retenciones> Parse(string fileName)
       {
           var res = new List<Retenciones>();
           var data = ParserRetencionesNtlink.GetFileData(fileName);
           var comp = new ParserRetencionesNtlink().ParseData(data);

           if (comp != null)
           {
              
               res.Add(comp);
           }
           return res;
       }
    }
}
