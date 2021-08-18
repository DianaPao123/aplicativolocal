using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AddendaAmece;
using GeneradorCfdi;
using TxtToCfdi;

namespace Pruebas
{
    

    class Program
    {

        static void ListProperties(Type tipo)
        {
            

            foreach (PropertyInfo pi in tipo.GetProperties())
            {
                if (pi.PropertyType.GetProperties().Count() > 0 &&
                    pi.PropertyType != tipo &&
                    !pi.PropertyType.IsPrimitive &&
                    pi.PropertyType != typeof(string) &&
                    pi.PropertyType != typeof(decimal) &&
                    pi.PropertyType != typeof(bool) &&
                    pi.PropertyType != typeof(DateTime) &&
                    !pi.PropertyType.IsEnum && 
                    !pi.PropertyType.IsArray)
                {
                    ListProperties(pi.PropertyType);
                }
                else if (pi.PropertyType.IsEnum)
                {
                    Console.WriteLine("\tENUM: " + pi.PropertyType.Name);
                    var values = Enum.GetValues(pi.PropertyType);
                    foreach (var value in values)
                    {
                        Console.WriteLine("\t\t\t- " + value);
                    }
                }
                else
                {
                    Console.WriteLine("\t" + pi.Name + "->" + pi.PropertyType.Name);
                }

            }
        }

        static void Main(string[] args)
        {

            var tipos =
                Assembly.GetAssembly(typeof (AddendaAmece.requestForPayment)).GetTypes();
            var types = tipos.Where(p => p.Namespace == "AddendaAmece").ToArray();

            foreach (var type in types)
            {
                Console.WriteLine("----------------------" + type.Name + "-----------------------");
                ListProperties(type);
                Console.WriteLine("----------------------" + type.Name + "-----------------------");
            }
            Console.ReadKey();
            //IParser p = new Cfd22Parser();
            //var x = p.Parse(@"c:\respaldo\B0191727.xml");
            //foreach (var comprobante in x)
            //{
            //    try
            //    {
            //        comprobante.fecha = DateTime.Now;
            //        comprobante.Emisor.rfc = "SID080303VE0";
            //        comprobante.noCertificado = "00001000000104602374";
            //        comprobante.certificado =
            //            "MIIElDCCA3ygAwIBAgIUMDAwMDEwMDAwMDAxMDQ2MDIzNzQwDQYJKoZIhvcNAQEFBQAwggE2MTgwNgYDVQQDDC9BLkMuIGRlbCBTZXJ2aWNpbyBkZSBBZG1pbmlzdHJhY2nDs24gVHJpYnV0YXJpYTEvMC0GA1UECgwmU2VydmljaW8gZGUgQWRtaW5pc3RyYWNpw7NuIFRyaWJ1dGFyaWExHzAdBgkqhkiG9w0BCQEWEGFjb2RzQHNhdC5nb2IubXgxJjAkBgNVBAkMHUF2LiBIaWRhbGdvIDc3LCBDb2wuIEd1ZXJyZXJvMQ4wDAYDVQQRDAUwNjMwMDELMAkGA1UEBhMCTVgxGTAXBgNVBAgMEERpc3RyaXRvIEZlZGVyYWwxEzARBgNVBAcMCkN1YXVodGVtb2MxMzAxBgkqhkiG9w0BCQIMJFJlc3BvbnNhYmxlOiBGZXJuYW5kbyBNYXJ0w61uZXogQ29zczAeFw0xMTEwMjYyMTQ0MTZaFw0xMzEwMjUyMTQ0MTZaMIIBMzFFMEMGA1UEAxM8U0VHVVJJREFEIElORk9STUFUSUNBIFkgREVTQVJST0xMTyBURUNOT0xPR0lDTyBTIERFIFJMIERFIENWMUUwQwYDVQQpEzxTRUdVUklEQUQgSU5GT1JNQVRJQ0EgWSBERVNBUlJPTExPIFRFQ05PTE9HSUNPIFMgREUgUkwgREUgQ1YxRTBDBgNVBAoTPFNFR1VSSURBRCBJTkZPUk1BVElDQSBZIERFU0FSUk9MTE8gVEVDTk9MT0dJQ08gUyBERSBSTCBERSBDVjElMCMGA1UELRMcU0lEMDgwMzAzVkUwIC8gWkFTUzgxMDYxOEtNOTEeMBwGA1UEBRMVIC8gWkFTUzgxMDYxOEhERlZOUjA3MRUwEwYDVQQLEwxUbGFsbmVwYW5sdGEwgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBANy3hQksRj1k7eQGpg2llIi57Y+eU2yjnRQKJnKBFCTPW+04XS/01pIGOowN+/W9oKNocp8c/Sn2wLaX35EIPtLgyTQ1WmfEMSs5iqO5MVld/ctMlJdMQYQUCdrtL3QVhluzsej4oZslHnLIu68rZo2MmssXco6UHkn/+D6r4RovAgMBAAGjHTAbMAwGA1UdEwEB/wQCMAAwCwYDVR0PBAQDAgbAMA0GCSqGSIb3DQEBBQUAA4IBAQDeMIcQiieeHtV1Xzko32NOZKN118g6m566AqFOZ6mT8CfSrSwVpGV6qIj6+yG++OCWKuDkTfLEh448ELELjnqegWsTHYctwpbnHMqzK/dLgr3bH5BNtd0mQexFgn9OYnikH2qWf6PDM3cL78/WCTUme5Dy03it0biA5wlL1DWpZAdXgP2qt3t9aANY6qObzyNgrviDbapErc05McbOp1UZZwFr2/PEF3XtZDqc8iVUVWH0U5J5MOo1KDnckbIIU1rUu5xjBTW+d4X+NENWNcRTOq+Z4NX/icz/kQTkoePpH0zRh0zR/JFOg5QGYiVHQ1NkmiVkoun/2JmfBPFjI0/p";
            //        Console.WriteLine(
            //            "-----------------------------------------------------------------");
            //        Generador gen = new Generador();
            //        gen.GenerarCfd(comprobante,
            //                       new X509Certificate2(
            //                           @"C:\Sidetec\Resources\SID080303VE0\certs\csd.cer"),
            //                       @"C:\Sidetec\Resources\SID080303VE0\certs\csd.key",
            //                       "sidetec11");
            //        Parallel.For(0, 1000, (i) =>
            //                                 {
            //                                     try
            //                                     {

            //                                         comprobante.Complemento.timbreFiscalDigital.UUID = Guid.NewGuid().ToString();
            //                                             byte[] bytes = gen.GetPdfFromComprobante(comprobante,
            //                                                                                      @"C:\Sidetec\Resources\LogoGenerico.png");


            //                                     }
            //                                     catch (Exception eee)
            //                                     {
            //                                         Console.WriteLine(eee);
            //                                     }



            //                                 });


            //    }
            //    catch (FaultException fe)
            //    {
            //        Console.WriteLine(fe.Message);
            //    }

            //}
            //Console.ReadKey();
        }
    }
}
