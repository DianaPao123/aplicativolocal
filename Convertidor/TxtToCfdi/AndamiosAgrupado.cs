using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TxtToCfdi
{
    public class AndamiosAgrupado
    {
        public string FacturaNumero { get; set; }
        public decimal Maniobra { get; set; }
        public decimal IvaManiobra { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Iva { get; set; }
        
        public string NumeroContrato { get; set; }
        public string Periodo { get; set; }
        public string FechaContrato { get; set; }

        public string IdCliente { get; set; }
        public string CalleObra { get; set; }
        public string NumeroObra { get; set; }
        public string ColoniaObra { get; set; }
        public string DelegacionObra { get; set; }
        public string CodigoPostalObra { get; set; }
        public string CiudadObra { get; set; }
        public string EstadoObra { get; set; }
        public string PaisObra { get; set; }
        public List<AndamiosRow> Rows { get; set; }

    }
}
