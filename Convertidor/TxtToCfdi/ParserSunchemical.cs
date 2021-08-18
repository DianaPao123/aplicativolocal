// TxtToCfdi.ParserSunchemical
using GeneradorCfdi;
using log4net;
using System.Collections.Generic;
using System.Linq;
using TxtToCfdi;

public class ParserSunchemical : IParser
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserSunchemical));

    public List<Comprobante> Parse(string fileName)
    {
        List<Comprobante> list = new List<Comprobante>();
        string[][] fileData = ParserNtLink.GetFileData(fileName);
        Comprobante comprobante = new ParserNtLink().ParseData(fileData);
        if (comprobante != null)
        {
            NEXEO sunchemicalAddenda = GetSunchemicalAddenda(fileData);
            if (sunchemicalAddenda != null)
            {
                string xmlStringFromAddendaObject = AddendaSerializer.GetXmlStringFromAddendaObject(sunchemicalAddenda, typeof(NEXEO), "", "");
                xmlStringFromAddendaObject = (comprobante.XmlAdenda = xmlStringFromAddendaObject.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.sunchemical.com/\"", ""));
            }
            list.Add(comprobante);
        }
        return list;
    }

    private NEXEO GetSunchemicalAddenda(string[][] data)
    {
        string[] array = data.FirstOrDefault((string[] j) => j[0] == "SUNCHEMICAL");
        if (array != null)
        {
            NEXEO nEXEO = new NEXEO();
            nEXEO.PO_NUMBER = array[1];
            return nEXEO;
        }
        return null;
    }
}
