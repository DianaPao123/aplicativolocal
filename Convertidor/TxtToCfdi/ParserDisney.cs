using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using GeneradorCfdi;
using log4net;
using System.Xml.Linq;

namespace TxtToCfdi
{
    public class ParserDisney : IParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParserDisney));

        

        public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);

            if (comp != null)
            {
                var adenda = this.GetDisneyAdenda(data);
                if (adenda != null)
                {
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(adenda, typeof(AddendaDisney), null, null);
                }
                res.Add(comp);
            }
            return res;
        }

        private AddendaDisney GetDisneyAdenda(string[][] data)
        {
            #region GetData
            var disneySupplierInfo = data.FirstOrDefault(j => j[0] == "DISNEY");
            var disneyTransaction = data.FirstOrDefault(j => j[0] == "DISNEYT");
            #endregion
            if (disneySupplierInfo != null && disneyTransaction != null)
            {
                #region Parse adenda
                AddendaDisney aDisney = new AddendaDisney();
                aDisney.SupplierInformation = new DisneySupplierInformation()
                {
                    ContactName = ParserNtLink.GetValue(disneySupplierInfo[1]),
                    PhoneNumber = ParserNtLink.GetValue(disneySupplierInfo[2]),
                    Email = ParserNtLink.GetValue(disneySupplierInfo[3]),
                    Number = ParserNtLink.GetValue(disneySupplierInfo[4])
                };

                aDisney.Transaction = new DisneyTransaction()
                {
                    PurchaseOrder = ParserNtLink.GetValue(disneyTransaction[1]),
                    GoodReceipt = ParserNtLink.GetValue(disneyTransaction[2]),
                    CasualBuyerEmail = ParserNtLink.GetValue(disneyTransaction[3])
                };
                #endregion
                return aDisney;
            }
            else
            {
                return null;
            }
        }

    }
}
