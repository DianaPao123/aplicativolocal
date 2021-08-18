// TxtToCfdi.ParserDetallista
using AddendaDetallistaCorta;
using GeneradorCfdi;
using System.Collections.Generic;
using System.Linq;
using TxtToCfdi;

public class ParserDetallista : IParser
{
    public List<Comprobante> Parse(string fileName)
    {
        List<Comprobante> list = new List<Comprobante>();
        string[][] fileData = ParserNtLink.GetFileData(fileName);
        Comprobante comprobante = new ParserNtLink().ParseData(fileData);
        if (comprobante != null)
        {
            detallista detallistaAdenda = GetDetallistaAdenda(fileData);
            if (detallistaAdenda != null)
            {
                string xmlStringFromAddendaObject = AddendaSerializer.GetXmlStringFromAddendaObject(detallistaAdenda, typeof(detallista), "detallista", "http://www.sat.gob.mx/detallista");
                xmlStringFromAddendaObject = (comprobante.XmlAdenda = xmlStringFromAddendaObject.Replace("detallista:detallista xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "detallista:detallista xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.sat.gob.mx/detallista http://www.sat.gob.mx/sitio_internet/cfd/detallista/detallista.xsd\""));
            }
            list.Add(comprobante);
        }
        return list;
    }

    private detallista GetDetallistaAdenda(string[][] data)
    {
        string[] array = data.FirstOrDefault((string[] j) => j[0] == "ADETALLISTA");
        if (array != null)
        {
            detallista detallista = new detallista();
            detallista.contentVersion = array[1];
            detallista.documentStatus = array[2];
            detallista.documentStructureVersion = array[3];
            detallistaOrderIdentification detallistaOrderIdentification = new detallistaOrderIdentification();
            detallistaOrderIdentificationReferenceIdentification detallistaOrderIdentificationReferenceIdentification = new detallistaOrderIdentificationReferenceIdentification();
            detallistaOrderIdentificationReferenceIdentification.type = array[4];
            detallistaOrderIdentificationReferenceIdentification.Value = array[5];
            detallistaOrderIdentification.referenceIdentification = detallistaOrderIdentificationReferenceIdentification;
            detallista.orderIdentification = detallistaOrderIdentification;
            return detallista;
        }
        return null;
    }
}
