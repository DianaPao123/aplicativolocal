using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneradorCfdi;

namespace TxtToCfdi
{
    public interface IParser
    {
        List<Comprobante> Parse(string fileName);
    }
    public interface IParserR
    {
        List<Retenciones> Parse(string fileName);
    }
}
