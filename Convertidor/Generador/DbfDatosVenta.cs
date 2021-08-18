using System;

namespace GeneradorCfdi
{
    public class DbfDatosVenta
    {
        public String VENTA { get; set; }
        public String CLIENTE { get; set; }
        public String VENDEDOR { get; set; }
        public String ORDCOMP { get; set; }
        // Entrega
        public String DIRECCION2 { get; set; }
        public String COLONIA2 { get; set; }
        public String CIUDAD2 { get; set; }
        public String ESTADO2 { get; set; }
        public String TEL3 { get; set; }
        public String CONTACTO3 { get; set; }
        public String HORARIO { get; set; }
        public String ENTRE1 { get; set; }
        public String ENTRE2 { get; set; }
        public String CONT { get; set; } 
        // Fin Entrega



        public override string ToString()
        {
            return VENTA + " " + CLIENTE + " " + VENDEDOR + " " + ORDCOMP + "\r\n" + DIRECCION2 + " " + COLONIA2 + " " + CIUDAD2 + " " + ESTADO2 + "\r\n" + TEL3 + " " + CONTACTO3 + " " + HORARIO + "\r\n" + ENTRE1 + " " + ENTRE2 + " " + CONT;
        }
    }
}
