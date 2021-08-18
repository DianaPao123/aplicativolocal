using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GeneradorCfdi;

namespace TxtToCfdi
{
    public class ParserTxtAndamios : IParser
    {
        private string nombre { get; set; }
        public bool esFactura { get; set; }

        public List<Comprobante> Parse(string fileName)
        {
            if (!File.Exists(fileName))
                throw new ApplicationException("El archivo no existe");
            //string[][] contenido = File.ReadAllLines(fileName, Encoding.Default).ToArray().Select(p => p.Split('|')).ToArray();
            IEnumerable<AndamiosRow> datos = null;
           
            try
            {
                // Factura - Ingreso
                if (esFactura)
                {
                    datos = from line in File.ReadAllLines(fileName, Encoding.Default)
                            where line.StartsWith("|")
                            let parts = line.Split('|')
                            where !parts[1].Contains("Id_Contrato")
                            select new AndamiosRow
                                       {
                                           IdContrato = parts[1].Trim(),
                                           MetodoPago=parts[2].Trim(),
                                           NumeroCuenta = parts[3].Trim(),
                                           EstaFactura = parts[4].Trim(),
                                           PeriodoFacturado = parts[5].Trim(),
                                           //FechaSiguienteFactura = parts[4].Trim(),
                                           FacturaNumero = parts[6].Trim(),
                                           ClaveControl = parts[7].Trim(),
                                           Descripcion = parts[8].Trim(),
                                           ExistenciaEnContrato = parts[9].Trim(),
                                           PrecioUnitarioRentaDiario = parts[10].Trim(),
                                           RazonSocial = parts[11].Trim(),
                                           Calle = parts[12].Trim(),
                                           Numero = parts[13].Trim(),
                                           Colonia = parts[14].Trim(),
                                           Delegacion = parts[15].Trim(),
                                           CodigoPostal = parts[16].Trim(),
                                           Ciudad = parts[17].Trim(),
                                           Estado = parts[18].Trim(),
                                           Pais = parts[19].Trim(),
                                           RFC = parts[20].Trim().Replace("-", "").Replace(" ", ""),
                                           Dias = parts[21].Trim(),
                                           IdCliente = parts[22].Trim(),
                                           ParcialDiario = parts[23].Trim(),
                                           DiasRenta = parts[24].Trim(),
                                           ParcialPeriodo = parts[25].Trim(),
                                           IVA = parts[26].Trim(),
                                           Fletes = parts[27].Trim(),
                                           IvaFletes = parts[28].Trim(),
                                           CalleObra = parts[29].Trim(),
                                           NumeroObra = parts[30].Trim(),
                                           ColoniaObra = parts[31].Trim(),
                                           DelegacionObra = parts[32].Trim(),
                                           CiudadObra = parts[33].Trim(),
                                           EstadoObra = parts[34].Trim(),
                                           CodigoPostalObra = parts[35].Trim(),
                                           PaisObra = parts[36].Trim(),
                                           FechaContrato = parts[37].Trim(),
                                           Telefono = parts[38].Trim()
                                       };
                } else
                {
                    // Nota Credito - Egreso
                    datos = from line in File.ReadAllLines(fileName, Encoding.Default)
                            where line.StartsWith("|")
                            let parts = line.Split('|')
                            where !parts[1].Contains("Id_Contrato")
                            select new AndamiosRow
                            {
                                IdContrato = parts[1].Trim(),
                                MetodoPago = parts[2].Trim(),
                                NumeroCuenta = parts[3].Trim(),
                                EstaFactura = parts[4].Trim(),
                                PeriodoFacturado = parts[5].Trim(),
                                FacturaNumero = parts[6].Trim(),
                                ClaveControl = parts[7].Trim(),
                                Descripcion = parts[8].Trim(),
                                ExistenciaEnContrato = parts[9].Trim(),
                                PrecioUnitarioRentaDiario = parts[10].Trim(),
                                RazonSocial = parts[11].Trim(),
                                Calle = parts[12].Trim(),
                                Numero = parts[13].Trim(),
                                Colonia = parts[14].Trim(),
                                Delegacion = parts[15].Trim(),
                                CodigoPostal = parts[16].Trim(),
                                Ciudad = parts[17].Trim(),
                                Estado = parts[18].Trim(),
                                Pais = parts[19].Trim(),
                                RFC = parts[20].Trim().Replace("-", "").Replace(" ", ""),
                                Dias = parts[21].Trim(),
                                IdCliente = parts[22].Trim(),
                                ParcialDiario = parts[23].Trim(),
                                DiasRenta = parts[24].Trim(),
                                ParcialPeriodo = parts[25].Trim(),
                                IVA = parts[26].Trim(),
                                CalleObra = parts[27].Trim(),
                                NumeroObra = parts[28].Trim(),
                                ColoniaObra = parts[29].Trim(),
                                DelegacionObra = parts[30].Trim(),
                                CiudadObra = parts[31].Trim(),
                                EstadoObra = parts[32].Trim(),
                                CodigoPostalObra = parts[33].Trim(),
                                PaisObra = parts[34].Trim(),
                                FechaContrato = parts[35].Trim(),
                                Telefono = parts[36].Trim(),
                                Fletes = "$0.00",
                                IvaFletes = "$0.00"
                            };
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Archivo mal formado", ex);
            }
            if (datos.Count() == 0)
            {
                throw new ApplicationException("El archivo esta vacío");
            }

            var facturasdistintas =
                datos.GroupBy(p => p.FacturaNumero).Select(
                    p => new AndamiosAgrupado
                             {
                                FacturaNumero = p.Key,
                                Rows = p.ToList(),
                                SubTotal = p.Sum(t => decimal.Parse(t.ParcialPeriodo, NumberStyles.Currency)),
                                Iva = p.Sum(t => decimal.Parse(t.IVA, NumberStyles.Currency)),
                                Maniobra = decimal.Parse(p.First().Fletes, NumberStyles.Currency),
                                IvaManiobra = decimal.Parse(p.First().IvaFletes, NumberStyles.Currency),
                                NumeroContrato = p.First().IdContrato,
                                Periodo = p.First().PeriodoFacturado,
                                FechaContrato = p.First().FechaContrato,
                                IdCliente = p.First().IdCliente,
                                CalleObra = p.First().CalleObra,
                                NumeroObra = p.First().NumeroObra,
                                ColoniaObra = p.First().ColoniaObra,
                                DelegacionObra = p.First().DelegacionObra,
                                CodigoPostalObra = p.First().CodigoPostalObra,
                                CiudadObra = p.First().CiudadObra,
                                EstadoObra = p.First().EstadoObra,
                                PaisObra = p.First().PaisObra
                    });
            
            List<Comprobante> listaComp = new List<Comprobante>();
            Console.WriteLine(facturasdistintas.Count());
            Console.WriteLine(datos.Count());
            foreach (var andamiosAgrupado in facturasdistintas)
            {
                Comprobante co = GetComprobante(andamiosAgrupado);
                listaComp.Add(co);
            }
           
            return listaComp;
        }
        public Comprobante GetComprobante(AndamiosAgrupado entrada)
        {
            Comprobante comprobante = new Comprobante();
            comprobante.Emisor = new GeneradorCfdi.ComprobanteEmisor();
            
            // Sin Datos Emisor - Se obtienen en ConvertidorAndamios.MainForm
            /*
            comprobante.Emisor.RegimenFiscal = new[]
                                                    {
                                                        new GeneradorCfdi.ComprobanteEmisorRegimenFiscal
                                                            {Regimen = entrada.Emisor.RegimenFiscal.First().Regimen}
                                                    };
            comprobante.Emisor.nombre = entrada.Emisor.nombre;
            comprobante.Emisor.rfc = entrada.Emisor.rfc;
            comprobante.Emisor.DomicilioFiscal = new GeneradorCfdi.t_UbicacionFiscal();
            comprobante.Emisor.DomicilioFiscal.calle = entrada.Emisor.DomicilioFiscal.calle;
            comprobante.Emisor.DomicilioFiscal.colonia = entrada.Emisor.DomicilioFiscal.colonia;
            comprobante.Emisor.DomicilioFiscal.codigoPostal = entrada.Emisor.DomicilioFiscal.codigoPostal;
            comprobante.Emisor.DomicilioFiscal.municipio = entrada.Emisor.DomicilioFiscal.municipio;
            comprobante.Emisor.DomicilioFiscal.pais = entrada.Emisor.DomicilioFiscal.pais;
            comprobante.Emisor.DomicilioFiscal.estado = entrada.Emisor.DomicilioFiscal.estado;
            */

            // Define Titulo en Base al Tipo de Comprobante
            if (esFactura)
            {
                comprobante.Titulo = "Factura";
                comprobante.TipoDeComprobante = "I";
            } else
            {
                comprobante.Titulo = "Nota de Crédito";
                comprobante.TipoDeComprobante = "E";
            }
            
            
            // Datos Receptor
            comprobante.Receptor = new GeneradorCfdi.ComprobanteReceptor();
            comprobante.Receptor.Nombre = entrada.Rows[0].RazonSocial;
            comprobante.Receptor.Rfc = entrada.Rows[0].RFC;
        /*    comprobante.Receptor.Domicilio = new GeneradorCfdi.t_Ubicacion();
            comprobante.Receptor.Domicilio.pais = "México";
            comprobante.Receptor.Domicilio.calle = entrada.Rows[0].Calle;
            comprobante.Receptor.Domicilio.noExterior = entrada.Rows[0].Numero;
            comprobante.Receptor.Domicilio.municipio = entrada.Rows[0].Delegacion;
            comprobante.Receptor.Domicilio.estado = entrada.Rows[0].Estado;
            comprobante.Receptor.Domicilio.colonia = entrada.Rows[0].Colonia;
            comprobante.Receptor.Domicilio.codigoPostal = entrada.Rows[0].CodigoPostal;
          */
            comprobante.LugarExpedicion = entrada.Rows[0].CodigoPostal;
            //comprobante.fecha = DateTime.Parse(entrada.Rows[0].EstaFactura);
            comprobante.Fecha = DateTime.Now.ToString("s"); //DateTime.Now;
            // Calculo en Absoluto
            comprobante.Total = Math.Abs(entrada.SubTotal) + Math.Abs(entrada.Iva) + Math.Abs(entrada.Maniobra) + Math.Abs(entrada.IvaManiobra);
            comprobante.Folio = entrada.Rows[0].FacturaNumero;
            
            //TODO Inicio hardcodeado
           // comprobante.TelefonoReceptor = entrada.Rows[0].Telefono;
            //comprobante.metodoDePago = "No identificado";  //modificado por rgv
            //comprobante.NumCtaPago = "0000";  //modificado por rgv
            comprobante.MetodoPago = entrada.Rows[0].MetodoPago;
            //comprobante.NumCtaPago = entrada.Rows[0].NumeroCuenta;
            comprobante.Moneda = "Pesos";
            //comprobante.Regimen = "Personas Morales del Régimen General";
            // Sumatoria de campo Fletes con Subtotal
            comprobante.SubTotal = Math.Abs(entrada.SubTotal) + Math.Abs(entrada.Maniobra);
            //comprobante.serie;
            comprobante.FormaPago = "En una sola exhibición";
            comprobante.LeyendaRentaAndamios = "RENTA, de acuerdo a lo estipulado en el CONTRATO " + entrada.NumeroContrato + " de fecha " +
                                               entrada.FechaContrato + ", correspondiente al " + entrada.Periodo + ".";
            comprobante.DomicilioObraAndamios = entrada.CalleObra + " " + entrada.NumeroObra + " COL. " +
                                                entrada.ColoniaObra + ", DELEGACION. " + entrada.DelegacionObra + ", " +
                                                entrada.EstadoObra + ", " + entrada.CiudadObra + ", C.P. " +
                                                entrada.CodigoPostalObra + ".";
            comprobante.IdClienteAndamios = entrada.IdCliente + "      " + entrada.NumeroContrato;
            //TODO Fin hardcodeado

            List<GeneradorCfdi.ConceptoInformativo> listaConceptoInformativo = new List<GeneradorCfdi.ConceptoInformativo>();
            foreach (var detalle in entrada.Rows)
            {
                GeneradorCfdi.ConceptoInformativo conceptoInformativo = new GeneradorCfdi.ConceptoInformativo();
                if (!string.IsNullOrEmpty(detalle.ClaveControl))
                    conceptoInformativo.noIdentificacionField = detalle.ClaveControl;                
                conceptoInformativo.descripcionField = detalle.Descripcion;
                conceptoInformativo.cantidadField = decimal.Parse(detalle.ExistenciaEnContrato);                
                listaConceptoInformativo.Add(conceptoInformativo);
            }

            if(entrada.Maniobra > 0)
            {
                GeneradorCfdi.ConceptoInformativo conManiobra = new GeneradorCfdi.ConceptoInformativo();
                conManiobra.noIdentificacionField = "MAN1";
                conManiobra.cantidadField = 1;
                conManiobra.descripcionField = "Maniobras";
                conManiobra.unidadField = "Pieza";
                conManiobra.valorUnitarioField = entrada.Maniobra;
                conManiobra.importeField = entrada.Maniobra;
                listaConceptoInformativo.Add(conManiobra);
            }

            List<GeneradorCfdi.ComprobanteConcepto> conceptos = new List<GeneradorCfdi.ComprobanteConcepto>();
            if(listaConceptoInformativo.Count > 0)
            {
                GeneradorCfdi.ComprobanteConcepto con = new GeneradorCfdi.ComprobanteConcepto();
                con.Descripcion = "Andamios en arrendamiento " + entrada.Periodo;
                // Siempre 1 Renta
                con.Cantidad = 1;
                con.Unidad = "No Aplica";
                // Valor Unitario
                con.ValorUnitario = Math.Abs(entrada.SubTotal) + Math.Abs(entrada.Maniobra).ToString();
                con.Importe = Math.Abs(entrada.SubTotal) + Math.Abs(entrada.Maniobra).ToString();
                conceptos.Add(con);
                comprobante.Conceptos = conceptos.ToArray();
                comprobante.ListaConceptoInformativo = listaConceptoInformativo;
            }

            GeneradorCfdi.ComprobanteImpuestos impuestos = new GeneradorCfdi.ComprobanteImpuestos();
            impuestos.TotalImpuestosTrasladados = Math.Abs(entrada.Iva) + Math.Abs(entrada.IvaManiobra).ToString();
            impuestos.TotalImpuestosTrasladadosSpecified = true;
            comprobante.Impuestos = impuestos;

            comprobante.CantidadLetra = CantidadLetra.Enletras(comprobante.Total.ToString(), comprobante.Moneda, comprobante.Emisor.Rfc);

            return comprobante;
        }



    }
}
