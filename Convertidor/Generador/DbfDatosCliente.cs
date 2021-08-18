using System;

namespace GeneradorCfdi
{
    public class DbfDatosCliente
    {
        
        public String Nombre { get; set; }
        public String Nombre2 { get; set; }
        public String Nombre3 { get; set; }
        public String CALLE { get; set; }
        public String NUMERO { get; set; }
        public String COLONIA { get; set; }
        public String ESTADO { get; set; }
        public String CIUDAD { get; set; }
        public String CP { get; set; }
        public String RFC { get; set; }
        public String CONTACTO { get; set; }
        public String CONTACTO2 { get; set; }
        public Double DIAS { get; set; }
        public String REVISION { get; set; }
        public String PAGOS { get; set; }
        public String FEMETOPA { get; set; }
        public String FENUMCTA { get; set; }
        public String EMAIL { get; set; }


        public override string ToString()
        {
            return Nombre + " " + Nombre2 + " " + Nombre3 + " " + CALLE + " " + NUMERO + "\r\n" + COLONIA + " " + ESTADO + " " + CIUDAD + " " + CP + "\r\n" + RFC + "\r\n" + CONTACTO + CONTACTO2 + "\r\n" + DIAS + " " + REVISION + " " + PAGOS + "\r\n" + FEMETOPA + " " + FENUMCTA;
        }
    }
}
