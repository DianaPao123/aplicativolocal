  using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AddendaBic;
using GeneradorCfdi;

namespace TxtToCfdi
{
   public class ParserJUMEX:IParser
   {
       public List<Comprobante> Parse(string fileName)
        {
            var res = new List<Comprobante>();
            var data = ParserNtLink.GetFileData(fileName);
            var comp = new ParserNtLink().ParseData(data);
            
            if (comp != null)
            {
                var addenda = GetJUMEXAddendaInfo(data, comp);

                if (addenda != null)
                {
                    //comp.AddendaSoriana = adenda;
                    comp.XmlAdenda = AddendaSerializer.GetXmlStringFromAddendaObject(addenda, typeof(AddendaJumex), null, null);
                     comp.XmlAdenda = comp.XmlAdenda.Replace("<AddendaJumex xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.sat.gob.mx/cfd/2\">", "");
                     comp.XmlAdenda = comp.XmlAdenda.Replace("</AddendaJumex>","");
                     comp.XmlAdenda = comp.XmlAdenda.Replace("<Addenda>", "<AddendaJumex>");
                     comp.XmlAdenda = comp.XmlAdenda.Replace("</Addenda>", "</AddendaJumex>");
                }
                res.Add(comp);
            }
            return res;
        }

       private AddendaJumex GetJUMEXAddendaInfo(string[][] data, Comprobante comprobante)
       {


           #region GetData
           var rfp = data.FirstOrDefault(j => j[0] == "RFP");
           var rfpId = data.FirstOrDefault(j => j[0] == "RFPID");
           var spIntr = data.Where(j => j[0] == "SPINTR");
          // var spIntrString = data.Where(j => j[0] == "SPINTRTEXTO");
           var oid = data.FirstOrDefault(j => j[0] == "OID");
           var rid = data.Where(j => j[0] == "RID");
           var ai = data.Where(j => j[0] == "AI");
           var dNote = data.FirstOrDefault(j => j[0] == "DNOTE");
           var dNoteId = data.Where(j => j[0] == "DNOTEID");
           var buyer = data.FirstOrDefault(j => j[0] == "BUYER");
           var seller = data.FirstOrDefault(j => j[0] == "SELLER");
           var shipto = data.FirstOrDefault(j => j[0] == "SHIPTO");
           var shiptoAddress = data.Where(j => j[0] == "SHIPTOADDRESS");
           var Customs = data.Where(j => j[0] == "CUSTOMS");
           var currency = data.Where(j => j[0] == "CURRENCY");
           var currencyf = data.Where(j => j[0] == "CURRENCYF");
           var pTerms = data.FirstOrDefault(j => j[0] == "PTERMS");
          // var sDetail = data.FirstOrDefault(j => j[0] == "SDETAIL");//????
           var aCharge = data.Where(j => j[0] == "ACHARGE");
           var lineItem = data.Where(j => j[0] == "LINEITEM");
           var lineItemI = data.Where(j => j[0] == "LINEITEMI");
           var lineItemIQ = data.Where(j => j[0] == "LINEITEMIQ");
           var lineItemCustoms = data.Where(j => j[0] == "LINEITEMCUSTOMS");
           var lineItemAttributes = data.Where(j => j[0] == "LINEITEMATTRIBUTES");
           var lineItemcharge= data.Where(j => j[0] == "LINEITEMCHARGE");
           var lineItemInformation = data.Where(j => j[0] == "LINEITEMINFORMATION");
           var totalAmount = data.FirstOrDefault(j => j[0] == "TOTALAMOUNT");
           var TotalAllowanceCharge = data.Where(j => j[0] == "TOTALALLOWANCECHARGE");
           var tax = data.Where(j => j[0] == "TAX");


           #endregion
               if (rfp != null)
               {

                   AddendaJumex.ComprobanteAddenda request = new AddendaJumex.ComprobanteAddenda();
         

                   request.requestForPayment = new AddendaJumex.ComprobanteAddendaRequestForPayment();
                   if (!string.IsNullOrEmpty(rfp[1]))//opcional
                   {
                       request.requestForPayment.DeliveryDateSpecified = true;
                       request.requestForPayment.DeliveryDate = DateTime.ParseExact(rfp[1], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                   }
                   else
                       request.requestForPayment.DeliveryDateSpecified = false;
                   if (rfp[2] == "COPY")//requerido
                       request.requestForPayment.documentStatus = AddendaJumex.ComprobanteAddendaRequestForPaymentDocumentStatus.COPY;
                   if (rfp[2] == "DELETE")
                       request.requestForPayment.documentStatus = AddendaJumex.ComprobanteAddendaRequestForPaymentDocumentStatus.DELETE;
                   if (rfp[2] == "ORIGINAL")
                       request.requestForPayment.documentStatus = AddendaJumex.ComprobanteAddendaRequestForPaymentDocumentStatus.ORIGINAL;
                   if (rfp[2] == "REEMPLAZA")
                       request.requestForPayment.documentStatus = AddendaJumex.ComprobanteAddendaRequestForPaymentDocumentStatus.REEMPLAZA;

                   request.requestForPayment.documentStructureVersion = rfp[3];//requerido
                   if (!string.IsNullOrEmpty(rfp[4]))
                         request.requestForPayment.contentVersion = rfp[4];//opcional
                   if (!string.IsNullOrEmpty(rfp[5]))
                       request.requestForPayment.type = rfp[5];//opcional
                   if (!string.IsNullOrEmpty(rfp[6]))
                   {
                       request.requestForPayment.totalAmount = new AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAmount();
                       request.requestForPayment.totalAmount.Amount = Convert.ToDecimal(rfp[6]);
                   }
                   if (!string.IsNullOrEmpty(rfp[7]))
                   {
                   
                   request.requestForPayment.baseAmount = new AddendaJumex.ComprobanteAddendaRequestForPaymentBaseAmount();
                   request.requestForPayment.baseAmount.Amount = Convert.ToDecimal(rfp[7]);
                    }
                   if (!string.IsNullOrEmpty(rfp[8]))
                   {

                       request.requestForPayment.payableAmount = new AddendaJumex.ComprobanteAddendaRequestForPaymentPayableAmount();
                       request.requestForPayment.payableAmount.Amount = Convert.ToDecimal(rfp[8]);
                   }
                   //requerido
                   request.requestForPayment.requestForPaymentIdentification = new AddendaJumex.ComprobanteAddendaRequestForPaymentRequestForPaymentIdentification();
                   if (rfpId[2] == "AUTO_INVOICE")
                       request.requestForPayment.requestForPaymentIdentification.entityType = AddendaJumex.ComprobanteAddendaRequestForPaymentRequestForPaymentIdentificationEntityType.AUTO_INVOICE;
                   if (rfpId[2] == "CREDIT_NOTE")
                       request.requestForPayment.requestForPaymentIdentification.entityType = AddendaJumex.ComprobanteAddendaRequestForPaymentRequestForPaymentIdentificationEntityType.CREDIT_NOTE;
                   if (rfpId[2] == "DEBIT_NOTE")
                       request.requestForPayment.requestForPaymentIdentification.entityType = AddendaJumex.ComprobanteAddendaRequestForPaymentRequestForPaymentIdentificationEntityType.DEBIT_NOTE;
                   if (rfpId[2] == "HONORARY_RECEIPT")
                       request.requestForPayment.requestForPaymentIdentification.entityType = AddendaJumex.ComprobanteAddendaRequestForPaymentRequestForPaymentIdentificationEntityType.HONORARY_RECEIPT;
                   if (rfpId[2] == "INVOICE")
                       request.requestForPayment.requestForPaymentIdentification.entityType = AddendaJumex.ComprobanteAddendaRequestForPaymentRequestForPaymentIdentificationEntityType.INVOICE;
                   if (rfpId[2] == "LEASE_RECEIPT")
                       request.requestForPayment.requestForPaymentIdentification.entityType = AddendaJumex.ComprobanteAddendaRequestForPaymentRequestForPaymentIdentificationEntityType.LEASE_RECEIPT;
                   if (rfpId[2] == "PARTIAL_INVOICE")
                       request.requestForPayment.requestForPaymentIdentification.entityType = AddendaJumex.ComprobanteAddendaRequestForPaymentRequestForPaymentIdentificationEntityType.PARTIAL_INVOICE;
                   if (rfpId[2] == "TRANSPORT_DOCUMENT")
                       request.requestForPayment.requestForPaymentIdentification.entityType = AddendaJumex.ComprobanteAddendaRequestForPaymentRequestForPaymentIdentificationEntityType.TRANSPORT_DOCUMENT;

                   request.requestForPayment.requestForPaymentIdentification.uniqueCreatorIdentification = rfpId[1];//requerido

                   if (spIntr.Any())//-----opcional------------
                   {

                       List<AddendaJumex.ComprobanteAddendaRequestForPaymentSpecialInstruction> SPI = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentSpecialInstruction>();

                       foreach (var sp in spIntr)
                       {
                           AddendaJumex.ComprobanteAddendaRequestForPaymentSpecialInstruction Spi = new AddendaJumex.ComprobanteAddendaRequestForPaymentSpecialInstruction();

                           if (sp[1] == "AAB")
                               Spi.code = AddendaJumex.ComprobanteAddendaRequestForPaymentSpecialInstructionCode.AAB;
                           if (sp[1] == "DUT")
                               Spi.code = AddendaJumex.ComprobanteAddendaRequestForPaymentSpecialInstructionCode.DUT;
                           if (sp[1] == "PUR")
                               Spi.code = AddendaJumex.ComprobanteAddendaRequestForPaymentSpecialInstructionCode.PUR;
                           if (sp[1] == "ZZZ")
                               Spi.code = AddendaJumex.ComprobanteAddendaRequestForPaymentSpecialInstructionCode.ZZZ;

                           Spi.text = new string[1];
                           Spi.text[0] = sp[2];

                           /*
                           string[][] texto = spIntrString.Where(j => j[1] == sp[1]).ToArray();
                           List<string> t=new List<string>();
                           foreach (var cad in texto)
                           {
                               t.Add(cad[2]);
                           }
                           Spi.text = t.ToArray();
                           */
                           SPI.Add(Spi);
                           
                       }
                       request.requestForPayment.specialInstruction = SPI.ToArray();

                   }//opcional------------------------------

                   request.requestForPayment.orderIdentification = new AddendaJumex.ComprobanteAddendaRequestForPaymentOrderIdentification();

                   if (!string.IsNullOrEmpty(oid[1]))
                   {
                       request.requestForPayment.orderIdentification.ReferenceDate = DateTime.ParseExact(oid[1], "yyyy-MM-dd", CultureInfo.InvariantCulture); 
                       request.requestForPayment.orderIdentification.ReferenceDateSpecified = true;
                   }
                   else
                       request.requestForPayment.orderIdentification.ReferenceDateSpecified = false;
                   //requerido------------------------------
                   List<AddendaJumex.ComprobanteAddendaRequestForPaymentOrderIdentificationReferenceIdentification> I = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentOrderIdentificationReferenceIdentification>();
                   foreach (var ri in rid)
                   {

                       AddendaJumex.ComprobanteAddendaRequestForPaymentOrderIdentificationReferenceIdentification i = new AddendaJumex.ComprobanteAddendaRequestForPaymentOrderIdentificationReferenceIdentification();
                       if (ri[1] == "ON")
                       i.type = AddendaJumex.ComprobanteAddendaRequestForPaymentOrderIdentificationReferenceIdentificationType.ON ;
                       i.Value = ri[2];
                       I.Add(i);
                   }
                   request.requestForPayment.orderIdentification.referenceIdentification = I.ToArray();
                   //--------------------------------------
                     if (ai.Any())
                     {
                    List<AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentification> R = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentification>();
                   foreach (var a in ai)
                   {
                       AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentification r = new AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentification();
                       if (a[1] == "AAE")
                       r.type = AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentificationType.AAE;
                       if (a[1] == "ACE")
                       r.type = AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentificationType.ACE;
                       if (a[1] == "ATZ")
                       r.type = AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentificationType.ATZ;
                       if (a[1] == "AWR")
                       r.type = AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentificationType.AWR;
                       if (a[1] == "CK")
                       r.type = AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentificationType.CK;
                       if (a[1] == "DQ")
                       r.type = AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentificationType.DQ;
                       if (a[1] == "IV")
                       r.type = AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentificationType.IV;
                       if (a[1] == "ON")
                       r.type = AddendaJumex.ComprobanteAddendaRequestForPaymentReferenceIdentificationType.ON;
                       r.Value=a[2];
                       R.Add(r);
                   }
                   request.requestForPayment.AdditionalInformation = R.ToArray();
                     }
                   //-------------------------------
                     if (dNote.Any())
                   {
                       request.requestForPayment.DeliveryNote = new AddendaJumex.ComprobanteAddendaRequestForPaymentDeliveryNote();
                       if (!string.IsNullOrEmpty(dNote[1]))
                       {
                           request.requestForPayment.DeliveryNote.ReferenceDate = DateTime.ParseExact(dNote[1], "yyyy-MM-dd", CultureInfo.InvariantCulture); ;
                           request.requestForPayment.DeliveryNote.ReferenceDateSpecified = true;
                       }
                       else
                           request.requestForPayment.DeliveryNote.ReferenceDateSpecified = false;
                         
                          if (dNoteId.Any())//-----requerido------------
                          {
                              List<string> t = new List<string>();
                              foreach (var cad in dNoteId)
                              {
                                  t.Add(cad[1]);
                              }
                              request.requestForPayment.DeliveryNote.referenceIdentification = t.ToArray();
                       
                          }
                       
                   }
                   //----------------------requerido

                     request.requestForPayment.buyer = new AddendaJumex.ComprobanteAddendaRequestForPaymentBuyer();
                     request.requestForPayment.buyer.gln = buyer[1];
                     if (!string.IsNullOrEmpty(buyer[2]))
                     {
                         request.requestForPayment.buyer.contactInformation = new AddendaJumex.ComprobanteAddendaRequestForPaymentBuyerContactInformation();
                         request.requestForPayment.buyer.contactInformation.personOrDepartmentName = new AddendaJumex.ComprobanteAddendaRequestForPaymentBuyerContactInformationPersonOrDepartmentName();
                         request.requestForPayment.buyer.contactInformation.personOrDepartmentName.text = buyer[2];
                     }

                   //-----------------------------------
                     if (seller != null)
                     {
                         request.requestForPayment.seller = new AddendaJumex.ComprobanteAddendaRequestForPaymentSeller();
                         request.requestForPayment.seller.gln = seller[1];
                         request.requestForPayment.seller.alternatePartyIdentification = new AddendaJumex.ComprobanteAddendaRequestForPaymentSellerAlternatePartyIdentification();
                         request.requestForPayment.seller.alternatePartyIdentification.type = new AddendaJumex.ComprobanteAddendaRequestForPaymentSellerAlternatePartyIdentificationType();
                         if (seller[2] == "IEPS_REFERENCE")
                         request.requestForPayment.seller.alternatePartyIdentification.type = AddendaJumex.ComprobanteAddendaRequestForPaymentSellerAlternatePartyIdentificationType.IEPS_REFERENCE;
                         if (seller[2] == "SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY")
                          request.requestForPayment.seller.alternatePartyIdentification.type = AddendaJumex.ComprobanteAddendaRequestForPaymentSellerAlternatePartyIdentificationType.SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY;
                         request.requestForPayment.seller.alternatePartyIdentification.Value = seller[3];

                     }
                     //------------------------------------------     
                   if(shipto!=null){  
                   request.requestForPayment.shipTo = new AddendaJumex.ComprobanteAddendaRequestForPaymentShipTo();
                       if(!string.IsNullOrEmpty(shipto[1]))
                   request.requestForPayment.shipTo.gln = shipto[1];

                       if (shiptoAddress.Any())
                       {
                           request.requestForPayment.shipTo.nameAndAddress = new AddendaJumex.ComprobanteAddendaRequestForPaymentShipToNameAndAddress();
                           List<string> name = new List<string>();
                           List<string> postalCode = new List<string>();
                           List<string> city = new List<string>();
                           List<string> streetAddressOne = new List<string>();
                           
                           foreach (var ship in shiptoAddress)
                           {
                               if (!string.IsNullOrEmpty(ship[1]))
                                   name.Add(ship[1]);
                               if (!string.IsNullOrEmpty(ship[2]))
                                   postalCode.Add(ship[2]);
                               if (!string.IsNullOrEmpty(ship[3]))
                                   city.Add(ship[3]);
                               if (!string.IsNullOrEmpty(ship[4]))
                                   streetAddressOne.Add(ship[4]);
                           }
                           if(name.Count>0)
                           request.requestForPayment.shipTo.nameAndAddress.name = name.ToArray();
                           if (postalCode.Count > 0)
                           request.requestForPayment.shipTo.nameAndAddress.postalCode = postalCode.ToArray();
                           if (city.Count > 0)
                           request.requestForPayment.shipTo.nameAndAddress.city = city.ToArray();
                           if (streetAddressOne.Count > 0)
                           request.requestForPayment.shipTo.nameAndAddress.streetAddressOne = streetAddressOne.ToArray();
                       }

                   }
                   //------------------------------------------------------
                       ///opcional
                   if(Customs.Any())       
                   {
                     // request.requestForPayment.Customs = new AddendaJumex.ComprobanteAddendaRequestForPaymentCustoms();
                       List<AddendaJumex.ComprobanteAddendaRequestForPaymentCustoms> CUS = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentCustoms>();

                       foreach (var c in Customs)
                       {
                           AddendaJumex.ComprobanteAddendaRequestForPaymentCustoms cus = new AddendaJumex.ComprobanteAddendaRequestForPaymentCustoms();
                          if(!string.IsNullOrEmpty(c[1]) )
                           cus.gln = c[1];
                           cus.ReferenceDate = DateTime.ParseExact(c[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                           cus.alternatePartyIdentification = new AddendaJumex.ComprobanteAddendaRequestForPaymentCustomsAlternatePartyIdentification();
                           cus.alternatePartyIdentification.type = new AddendaJumex.ComprobanteAddendaRequestForPaymentCustomsAlternatePartyIdentificationType();
                           cus.alternatePartyIdentification.type = AddendaJumex.ComprobanteAddendaRequestForPaymentCustomsAlternatePartyIdentificationType.TN;
                           if (!string.IsNullOrEmpty(c[3]) || !string.IsNullOrEmpty(c[4]))
                           {
                               cus.nameAndAddress = new AddendaJumex.ComprobanteAddendaRequestForPaymentCustomsNameAndAddress();
                               cus.nameAndAddress.city = c[3];
                               cus.nameAndAddress.name = c[4];
                           }
                           CUS.Add(cus);
                       }
                       request.requestForPayment.Customs = CUS.ToArray();

                   }
                   //-------------------------------------------------------------
                   if (currency.Any())
                   {
                       // request.requestForPayment.Customs = new AddendaJumex.ComprobanteAddendaRequestForPaymentCustoms();
                       List < AddendaJumex.ComprobanteAddendaRequestForPaymentCurrency> CURR = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentCurrency>();

                       foreach (var curre in currency)
                       {
                           AddendaJumex.ComprobanteAddendaRequestForPaymentCurrency curr = new AddendaJumex.ComprobanteAddendaRequestForPaymentCurrency();
                           if (!string.IsNullOrEmpty(curre[1]))
                           { curr.rateOfChangeSpecified = true;
                               curr.rateOfChange = Convert.ToDecimal(curre[1]);
                           }else
                               curr.rateOfChangeSpecified = false;
                           curr.currencyISOCode = new AddendaJumex.ComprobanteAddendaRequestForPaymentCurrencyCurrencyISOCode();
                           if(curre[2]=="MXN")
                           curr.currencyISOCode = AddendaJumex.ComprobanteAddendaRequestForPaymentCurrencyCurrencyISOCode.MXN;
                           if(curre[2]=="USD")    
                           curr.currencyISOCode = AddendaJumex.ComprobanteAddendaRequestForPaymentCurrencyCurrencyISOCode.USD;
                           if (curre[2] == "XEU")
                           curr.currencyISOCode = AddendaJumex.ComprobanteAddendaRequestForPaymentCurrencyCurrencyISOCode.XEU;
                           
                           List<AddendaJumex.ComprobanteAddendaRequestForPaymentCurrencyCurrencyFunction> CURRF = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentCurrencyCurrencyFunction>();
                           foreach (var curref in currencyf)
                           {
                               AddendaJumex.ComprobanteAddendaRequestForPaymentCurrencyCurrencyFunction curf = new AddendaJumex.ComprobanteAddendaRequestForPaymentCurrencyCurrencyFunction();
                               if (curref[1] == "BILLING_CURRENCY")
                               curf = AddendaJumex.ComprobanteAddendaRequestForPaymentCurrencyCurrencyFunction.BILLING_CURRENCY;
                               if (curref[1] == "PAYMENT_CURRENCY") 
                               curf = AddendaJumex.ComprobanteAddendaRequestForPaymentCurrencyCurrencyFunction.PAYMENT_CURRENCY;
                               if (curref[1] == "PRICE_CURRENCY") 
                               curf = AddendaJumex.ComprobanteAddendaRequestForPaymentCurrencyCurrencyFunction.PRICE_CURRENCY;
                               CURRF.Add(curf);
            
                           }
                           curr.currencyFunction = CURRF.ToArray(); 
                           CURR.Add(curr);
                       }
                       request.requestForPayment.currency = CURR.ToArray(); 

                   }
                   //----------------------------
                   if (pTerms.Any())
                   {
                       request.requestForPayment.paymentTerms = new AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTerms();

                       if (!string.IsNullOrEmpty(pTerms[1]) || !string.IsNullOrEmpty(pTerms[2]))
                       {
                           request.requestForPayment.paymentTerms.discountPayment = new AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsDiscountPayment();
                           request.requestForPayment.paymentTerms.discountPayment.discountType = new AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsDiscountPaymentDiscountType();
                           if (pTerms[1] == "ALLOWANCE_BY_PAYMENT_ON_TIME")
                               request.requestForPayment.paymentTerms.discountPayment.discountType = AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsDiscountPaymentDiscountType.ALLOWANCE_BY_PAYMENT_ON_TIME;
                           if (pTerms[1] == "SANCTION")
                               request.requestForPayment.paymentTerms.discountPayment.discountType = AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsDiscountPaymentDiscountType.SANCTION;
                           request.requestForPayment.paymentTerms.discountPayment.percentage = pTerms[2]; 
                         
                       }
                       if (!string.IsNullOrEmpty(pTerms[3]) || !string.IsNullOrEmpty(pTerms[4]))
                       {
                           request.requestForPayment.paymentTerms.netPayment = new AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsNetPayment();
                           request.requestForPayment.paymentTerms.netPayment.netPaymentTermsType = new AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsNetPaymentNetPaymentTermsType();
                           if (pTerms[3] == "BASIC_DISCOUNT_OFFERED")
                               request.requestForPayment.paymentTerms.netPayment.netPaymentTermsType = AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsNetPaymentNetPaymentTermsType.BASIC_DISCOUNT_OFFERED;
                           if (pTerms[3] == "BASIC_NET")
                               request.requestForPayment.paymentTerms.netPayment.netPaymentTermsType = AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsNetPaymentNetPaymentTermsType.BASIC_NET;
                           if (pTerms[3] == "END_OF_MONTH")
                               request.requestForPayment.paymentTerms.netPayment.netPaymentTermsType = AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsNetPaymentNetPaymentTermsType.END_OF_MONTH;
                           if ( !string.IsNullOrEmpty(pTerms[4]))
                           {
                               request.requestForPayment.paymentTerms.netPayment.paymentTimePeriod = new AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsNetPaymentPaymentTimePeriod();
                               request.requestForPayment.paymentTerms.netPayment.paymentTimePeriod.timePeriodDue = new AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsNetPaymentPaymentTimePeriodTimePeriodDue();
                               request.requestForPayment.paymentTerms.netPayment.paymentTimePeriod.timePeriodDue.timePeriod = new AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsNetPaymentPaymentTimePeriodTimePeriodDueTimePeriod();
                               request.requestForPayment.paymentTerms.netPayment.paymentTimePeriod.timePeriodDue.timePeriod = AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsNetPaymentPaymentTimePeriodTimePeriodDueTimePeriod.DAYS;
                               request.requestForPayment.paymentTerms.netPayment.paymentTimePeriod.timePeriodDue.value = pTerms[4];
                           }
                       }
                       if (!string.IsNullOrEmpty(pTerms[5]))
                       {
                           request.requestForPayment.paymentTerms.paymentTermsEventSpecified = true;
                           request.requestForPayment.paymentTerms.paymentTermsEvent = new AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsPaymentTermsEvent();
                           if (pTerms[5] == "DATE_OF_INVOICE")
                               request.requestForPayment.paymentTerms.paymentTermsEvent = AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsPaymentTermsEvent.DATE_OF_INVOICE;
                           if (pTerms[5] == "EFFECTIVE_DATE")
                               request.requestForPayment.paymentTerms.paymentTermsEvent = AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsPaymentTermsEvent.EFFECTIVE_DATE;
                           
                       }else
                         request.requestForPayment.paymentTerms.paymentTermsEventSpecified = false;
                       if (!string.IsNullOrEmpty(pTerms[6]))
                       {
                           request.requestForPayment.paymentTerms.PaymentTermsRelationTimeSpecified = true;
                           request.requestForPayment.paymentTerms.PaymentTermsRelationTime = new AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsPaymentTermsRelationTime();
                           if (pTerms[6] == "REFERENCE_AFTER")
                           request.requestForPayment.paymentTerms.PaymentTermsRelationTime = AddendaJumex.ComprobanteAddendaRequestForPaymentPaymentTermsPaymentTermsRelationTime.REFERENCE_AFTER;
                       }
                       else
                       request.requestForPayment.paymentTerms.PaymentTermsRelationTimeSpecified = false;
                     
                   
                   }
                   //--------------------------------------------------
                   /*if (sDetail.Any())//?
                   {
                       //request.requestForPayment.shipmentDetail
                   }*/
                   //----------------------------------------------------------------
                   if (aCharge.Any())
                   {
                       //request.requestForPayment.allowanceCharge = new AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceCharge(); 
                        List<AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceCharge> ACH = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceCharge>();
                        foreach (var ac in aCharge)
                        {
                            AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceCharge ach = new AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceCharge();
                            ach.allowanceChargeType = new AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeAllowanceChargeType();
                            if (ac[1] == "ALLOWANCE_GLOBAL")
                                ach.allowanceChargeType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeAllowanceChargeType.ALLOWANCE_GLOBAL;
                            if (ac[1] == "CHARGE_GLOBAL")
                                ach.allowanceChargeType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeAllowanceChargeType.CHARGE_GLOBAL;


                             if (!string.IsNullOrEmpty(ac[2]))
                              {
                               ach.monetaryAmountOrPercentage = new AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeMonetaryAmountOrPercentage();
                               ach.monetaryAmountOrPercentage.rate = new AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeMonetaryAmountOrPercentageRate();
                               ach.monetaryAmountOrPercentage.rate.@base = new AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeMonetaryAmountOrPercentageRateBase();
                                ach.monetaryAmountOrPercentage.rate.@base = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeMonetaryAmountOrPercentageRateBase.INVOICE_VALUE;
                                ach.monetaryAmountOrPercentage.rate.percentage = Convert.ToDecimal(ac[2]);
                            }
                             if (!string.IsNullOrEmpty(ac[3]))
                             ach.sequenceNumber = ac[3];
                             ach.settlementType = new AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSettlementType();
                             if (ac[4] == "BILL_BACK") 
                                 ach.settlementType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSettlementType.BILL_BACK;
                             if (ac[4] == "OFF_INVOICE")
                                 ach.settlementType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSettlementType.OFF_INVOICE;
                             if (!string.IsNullOrEmpty(ac[5]))
                             {
                                 ach.specialServicesTypeSpecified = true;
                                 ach.specialServicesType = new AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType();
                                 if (ac[5] == "AA") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.AA;
                                 if (ac[5] == "ABZ") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.ABZ;
                                 if (ac[5] == "ADO") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.ADO;
                                 if (ac[5] == "ADS") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.ADS;
                                 if (ac[5] == "ADT") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.ADT;
                                 if (ac[5] == "AJ") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.AJ;
                                 if (ac[5] == "CAC") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.CAC;
                                 if (ac[5] == "COD") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.COD;
                                 if (ac[5] == "DA") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.DA;
                                 if (ac[5] == "DI") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.DI;
                                 if (ac[5] == "EAA") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.EAA;
                                 if (ac[5] == "EAB") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.EAB;
                                 if (ac[5] == "EAB1") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.EAB1;
                                 if (ac[5] == "FA") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.FA;
                                 if (ac[5] == "FC") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.FC;
                                 if (ac[5] == "FG") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.FG;
                                 if (ac[5] == "FI") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.FI;
                                 if (ac[5] == "HD") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.HD;
                                 if (ac[5] == "PAD") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.PAD;
                                 if (ac[5] == "PI") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.PI;
                                 if (ac[5] == "QD") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.QD;
                                 if (ac[5] == "RAA") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.RAA;
                                 if (ac[5] == "SAB") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.SAB;
                                 if (ac[5] == "TAE") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.TAE;
                                 if (ac[5] == "TD") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.TD;
                                 if (ac[5] == "TS") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.TS;
                                 if (ac[5] == "TX") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.TX;
                                 if (ac[5] == "TZ") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.TZ;
                                 if (ac[5] == "UM") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.UM;
                                 if (ac[5] == "VAB") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.VAB;
                                 if (ac[5] == "ZZZ") ach.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentAllowanceChargeSpecialServicesType.ZZZ;
                 
                             }else
                             ach.specialServicesTypeSpecified = false;
                            //--------------------------------------------------------



                            ACH.Add(ach);
                        }
                        request.requestForPayment.allowanceCharge = ACH.ToArray();
                      
                   }
                   //--------------------------------------------------------------
                   if (lineItem.Any())
                   {
                      // request.requestForPayment.lineItem = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItem(); 
                        List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItem> LI = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItem>();
                        foreach (var l in lineItem)
                        {
                            AddendaJumex.ComprobanteAddendaRequestForPaymentLineItem li = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItem();
                            li.tradeItemIdentification = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemIdentification();
                            li.tradeItemIdentification.gtin = l[2];
                            if (!string.IsNullOrEmpty(l[3]) || !string.IsNullOrEmpty(l[4]))
                            {
                                li.tradeItemDescriptionInformation = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemDescriptionInformation();
                                if (!string.IsNullOrEmpty(l[3]))
                                {
                                    li.tradeItemDescriptionInformation.language = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemDescriptionInformationLanguage();
                                    if (l[3] == "EN") li.tradeItemDescriptionInformation.language = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemDescriptionInformationLanguage.EN;
                                    if (l[3] == "ES") li.tradeItemDescriptionInformation.language = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemDescriptionInformationLanguage.ES;
                                    li.tradeItemDescriptionInformation.languageSpecified = true;
                                }
                                else
                                    li.tradeItemDescriptionInformation.languageSpecified = false;
                                li.tradeItemDescriptionInformation.longText = l[4];
                            }
                            li.invoicedQuantity = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemInvoicedQuantity();
                            li.invoicedQuantity.unitOfMeasure = l[5];
                            li.invoicedQuantity.Text = new string[1];
                            li.invoicedQuantity.Text[0] = l[6];
                            if (!string.IsNullOrEmpty(l[7]))
                            {
                                li.grossPrice = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemGrossPrice();
                                li.grossPrice.Amount = Convert.ToDecimal(l[7]);
                            }
                            if (!string.IsNullOrEmpty(l[8]))
                            {
                                li.netPrice = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemNetPrice(); 
                                li.netPrice.Amount  = Convert.ToDecimal(l[8]);
                            }
                            if (!string.IsNullOrEmpty(l[9]))
                            {
                                li.AdditionalInformation = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAdditionalInformation();
                                li.AdditionalInformation.referenceIdentification = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAdditionalInformationReferenceIdentification();
                                li.AdditionalInformation.referenceIdentification.type = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAdditionalInformationReferenceIdentificationType();
                                li.AdditionalInformation.referenceIdentification.type = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAdditionalInformationReferenceIdentificationType.DQ;
                                li.AdditionalInformation.referenceIdentification.Value = l[9];
                            }
                            if (!string.IsNullOrEmpty(l[10]) || !string.IsNullOrEmpty(l[11]))
                            {
                                li.LogisticUnits = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemLogisticUnits();
                                li.LogisticUnits.serialShippingContainerCode = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemLogisticUnitsSerialShippingContainerCode();
                                 li.LogisticUnits.serialShippingContainerCode.type=new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemLogisticUnitsSerialShippingContainerCodeType();
                            if (l[10] == "BJ") 
                                 li.LogisticUnits.serialShippingContainerCode.type= AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemLogisticUnitsSerialShippingContainerCodeType.BJ; 
                              if (l[10] == "SRV") 
                                  li.LogisticUnits.serialShippingContainerCode.type= AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemLogisticUnitsSerialShippingContainerCodeType.SRV;
                              li.LogisticUnits.serialShippingContainerCode.Value = l[11];


                            }
                            if (!string.IsNullOrEmpty(l[12])||!string.IsNullOrEmpty(l[13])||!string.IsNullOrEmpty(l[14])||!string.IsNullOrEmpty(l[15]))
                            { li.palletInformation=new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformation();
                               li.palletInformation.description=new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationDescription();
                                li.palletInformation.description.Text=new string[1];
                                li.palletInformation.description.Text[0]=l[12];
                                li.palletInformation.description.type=new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationDescriptionType();
                                li.palletInformation.description.type= AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationDescriptionType.BOX;
                            
                                if (l[13] == "BOX") 
                                li.palletInformation.description.type= AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationDescriptionType.BOX;
                                if (l[13] == "CASE")
                                li.palletInformation.description.type= AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationDescriptionType.CASE;
                                if (l[13] == "EXCHANGE_PALLETS") 
                                li.palletInformation.description.type= AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationDescriptionType.EXCHANGE_PALLETS;
                                if (l[13] == "PALLET_80x100") 
                                li.palletInformation.description.type= AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationDescriptionType.PALLET_80x100;
                                if (l[13] == "RETURN_PALLETS") 
                                li.palletInformation.description.type= AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationDescriptionType.RETURN_PALLETS;
                            
                                li.palletInformation.palletQuantity=l[14];
                                li.palletInformation.transport=new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationTransport();
                                li.palletInformation.transport.methodOfPayment=new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationTransportMethodOfPayment();
                                if (l[15] == "PAID_BY_BUYER") 
                                li.palletInformation.transport.methodOfPayment= AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationTransportMethodOfPayment.PAID_BY_BUYER;
                                if (l[15] == "PREPAID_BY_SELLER") 
                                li.palletInformation.transport.methodOfPayment= AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemPalletInformationTransportMethodOfPayment.PREPAID_BY_SELLER;
                             
                            
                            }

                            if (!string.IsNullOrEmpty(l[16])||!string.IsNullOrEmpty(l[17]))
                            {
                                li.totalLineAmount = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTotalLineAmount();
                                li.totalLineAmount.grossAmount = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTotalLineAmountGrossAmount();
                                li.totalLineAmount.grossAmount.Amount = Convert.ToDecimal(l[16]);
                                li.totalLineAmount.netAmount = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTotalLineAmountNetAmount();
                                li.totalLineAmount.netAmount.Amount = Convert.ToDecimal(l[17]);
                            }
                            if (!string.IsNullOrEmpty(l[18]))
                            {
                                li.type = l[18];
                            }
                            if (!string.IsNullOrEmpty(l[19]))
                            {
                                li.number = l[19];
                            }

                            //li.alternateTradeItemIdentification = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAlternateTradeItemIdentification();
                            List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAlternateTradeItemIdentification> ATI = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAlternateTradeItemIdentification>();

                            var lineItemIA = lineItemI.Where(j => (j[1] == l[1]));
                            if (lineItemIA.Count()>0 ) //opciional
                            {
                                foreach (var lI in lineItemIA)
                                {
                                    AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAlternateTradeItemIdentification ati = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAlternateTradeItemIdentification();
                                    ati.type = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAlternateTradeItemIdentificationType();
                                    if (lI[2] == "BUYER_ASSIGNED") ati.type = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAlternateTradeItemIdentificationType.BUYER_ASSIGNED;
                                    if (lI[2] == "GLOBAL_TRADE_ITEM_IDENTIFICATION") ati.type = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAlternateTradeItemIdentificationType.GLOBAL_TRADE_ITEM_IDENTIFICATION;
                                    if (lI[2] == "SERIAL_NUMBER") ati.type = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAlternateTradeItemIdentificationType.SERIAL_NUMBER;
                                    if (lI[2] == "SUPPLIER_ASSIGNED") ati.type = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAlternateTradeItemIdentificationType.SUPPLIER_ASSIGNED;
                                    ati.Text = new string[1]; 
                                    ati.Text[0] = lI[3];
                                    ATI.Add(ati);
                                }
                                li.alternateTradeItemIdentification = ATI.ToArray();

                            }
                            //----------
                            if (lineItemIQ.Any())
                            {
                                var lineItemIQ2 = lineItemIQ.Where(j => (j[1] == l[1]));
                                if (lineItemIQ2.Count() > 0) //opciional
                                {
                                    List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAditionalQuantity> IQT2 = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAditionalQuantity>();
                                    foreach (var iq in lineItemIQ2)
                                    {
                                        AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAditionalQuantity IQT = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAditionalQuantity();
                                        IQT.QuantityType = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAditionalQuantityQuantityType();
                                        if (iq[2] == "FREE_GOODS") 
                                        IQT.QuantityType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAditionalQuantityQuantityType.FREE_GOODS;
                                        if (iq[2] == "NUM_CONSUMER_UNITS") 
                                        IQT.QuantityType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAditionalQuantityQuantityType.NUM_CONSUMER_UNITS;
                                        IQT.Text=new string[1];
                                        IQT.Text[0] = iq[3];

                                        IQT2.Add(IQT);
                                    }
                                    li.aditionalQuantity = IQT2.ToArray();

                                 }
                            }
                            //-------------------------------------------
                            if (lineItemCustoms.Any())//opcional
                            {

                                var lineItemCustomsx = lineItemCustoms.Where(j => (j[1] == l[1]));
                                if (lineItemCustomsx.Count() > 0) //opciional
                            {

                                List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemCustoms> ICUS = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemCustoms>();
                                foreach (var icu in lineItemCustomsx)
                                {
                                    AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemCustoms icus = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemCustoms();
                                    icus.alternatePartyIdentification = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemCustomsAlternatePartyIdentification();
                                    icus.alternatePartyIdentification.type = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemCustomsAlternatePartyIdentificationType();
                                    icus.alternatePartyIdentification.type = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemCustomsAlternatePartyIdentificationType.TN;
                                    icus.alternatePartyIdentification.Value = icu[2];
                                    if (!string.IsNullOrEmpty(icu[3]))
                                        icus.gln = icu[3];
                                    icus.nameAndAddress = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemCustomsNameAndAddress();
                                    icus.nameAndAddress.name = icu[4];
                                    icus.ReferenceDate = DateTime.ParseExact(icu[5], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                    ICUS.Add(icus);
                                }
                                li.Customs = ICUS.ToArray();
                            }
                            }
                            //--------------------------------------
                            if (lineItemAttributes.Any())//opcional
                            {

                                var lineItemAttributesx = lineItemAttributes.Where(j => (j[1] == l[1]));
                                if (lineItemAttributesx.Count() > 0) //opciional
                                {
                                    List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemLotNumber> NUM =new List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemLotNumber>();
                                  foreach (var iatr in lineItemAttributesx)
                                 { 
                                      AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemLotNumber num=new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemLotNumber();
                                      if(!string.IsNullOrEmpty(iatr[2]))
                                      {num.productionDateSpecified=true;
                                      num.productionDate=DateTime.ParseExact(iatr[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                      }else
                                          num.productionDateSpecified=false; 
                                      num.Value=iatr[3];
                                      NUM.Add(num);

                                  }
                                  li.extendedAttributes = NUM.ToArray();
                                }
                            }
                            //--------------------------------------

                            if (lineItemcharge.Any())//opcional
                            {

                                var lineItemchargex = lineItemcharge.Where(j => (j[1] == l[1]));
                                if (lineItemchargex.Count() > 0) //opciional
                                {
                                    List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceCharge> IAC = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceCharge>();
                                    foreach (var iatr in lineItemchargex)
                                    {
                                        AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceCharge iac = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceCharge();
                                        iac.allowanceChargeType = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeAllowanceChargeType();
                                        if (iatr[2] == "ALLOWANCE_GLOBAL")
                                             iac.allowanceChargeType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeAllowanceChargeType.ALLOWANCE_GLOBAL;
                                        if (iatr[2] == "CHARGE_GLOBAL") 
                                        iac.allowanceChargeType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeAllowanceChargeType.CHARGE_GLOBAL;

                                        iac.monetaryAmountOrPercentage = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeMonetaryAmountOrPercentage();
                                        iac.monetaryAmountOrPercentage.percentagePerUnit = iatr[3];
                                        iac.monetaryAmountOrPercentage.ratePerUnit = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeMonetaryAmountOrPercentageRatePerUnit();
                                        iac.monetaryAmountOrPercentage.ratePerUnit.amountPerUnit = iatr[4];
                                        if (!string.IsNullOrEmpty(iatr[5]))
                                        iac.sequenceNumber = iatr[5];

                                        if (!string.IsNullOrEmpty(iatr[6]))
                                        {

                                            iac.settlementType = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSettlementType();
                                            if (iatr[6] == "CHARGE_TO_BE_PAID_BY_CUSTOMER")
                                                iac.settlementType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSettlementType.CHARGE_TO_BE_PAID_BY_CUSTOMER;
                                            if (iatr[6] == "CHARGE_TO_BE_PAID_BY_VENDOR")
                                                iac.settlementType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSettlementType.CHARGE_TO_BE_PAID_BY_VENDOR;
                                            if (iatr[6] == "OFF_INVOICE")
                                                iac.settlementType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSettlementType.OFF_INVOICE;
                                            iac.settlementTypeSpecified = true;
                                        }
                                        else
                                            iac.settlementTypeSpecified = false;
                                        if (!string.IsNullOrEmpty(iatr[7]))
                                        {
                                            iac.specialServicesType = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType();
                                            if (iatr[7] == "AA")
                                                iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.AA;
                                            if (iatr[7] == "ABZ") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.ABZ;
                                            if (iatr[7] == "ADO") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.ADO;
                                            if (iatr[7] == "ADS") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.ADS;
                                            if (iatr[7] == "ADT") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.ADT;
                                            if (iatr[7] == "AJ") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.AJ;
                                            if (iatr[7] == "CAC") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.CAC;
                                            if (iatr[7] == "COD") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.COD;
                                            if (iatr[7] == "DA") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.DA;
                                            if (iatr[7] == "DI") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.DI;
                                            if (iatr[7] == "EAA") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.EAA;
                                            if (iatr[7] == "EAB") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.EAB;
                                            if (iatr[7] == "FA") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.FA;
                                            if (iatr[7] == "FC") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.FC;
                                            if (iatr[7] == "FG") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.FG;
                                            if (iatr[7] == "FI") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.FI;
                                            if (iatr[7] == "HD") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.HD;
                                            if (iatr[7] == "PAD") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.PAD;
                                            if (iatr[7] == "PI") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.PI;
                                            if (iatr[7] == "QD") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.QD;
                                            if (iatr[7] == "RAA") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.RAA;
                                            if (iatr[7] == "SAB") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.SAB;
                                            if (iatr[7] == "TAE") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.TAE;
                                            if (iatr[7] == "TD") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.TD;
                                            if (iatr[7] == "TS") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.TS;
                                            if (iatr[7] == "TX") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.TX;
                                            if (iatr[7] == "TZ") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.TZ;
                                            if (iatr[7] == "UM") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.UM;
                                            if (iatr[7] == "VAB") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.VAB;
                                            if (iatr[7] == "ZZZ") iac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemAllowanceChargeSpecialServicesType.ZZZ;
                                            iac.specialServicesTypeSpecified = true;
                                        }else
                                        iac.specialServicesTypeSpecified = false;
                                        IAC.Add(iac);
                                     }
                                    li.allowanceCharge = IAC.ToArray();
                                }
                            }
                            //---------------------------------------------     
                            
                            if (lineItemInformation.Any())//opcional
                            {

                                var lineItemInformationx = lineItemInformation.Where(j => (j[1] == l[1]));
                                if (lineItemInformationx.Count() > 0) //opciional
                                {
                                    List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformation> ITT = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformation>();
                                    foreach (var ittr in lineItemInformationx)
                                    {
                                        AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformation  itt= new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformation();
                                        if (!string.IsNullOrEmpty(ittr[2]))
                                        itt.referenceNumber = ittr[2];
                                        if (!string.IsNullOrEmpty(ittr[3]))
                                        {
                                            itt.taxCategory = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxCategory();
                                            if (ittr[3] == "RETENIDO")
                                                itt.taxCategory = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxCategory.RETENIDO;
                                            if (ittr[3] == "TRANSFERIDO")
                                                itt.taxCategory = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxCategory.TRANSFERIDO;
                                            itt.taxCategorySpecified = true;
                                        }else itt.taxCategorySpecified = false;
                                         itt.taxTypeDescription = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxTypeDescription();
                                        if (ittr[4] == "AAA") itt.taxTypeDescription = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxTypeDescription.AAA;
                                        if (ittr[4] == "ADD") itt.taxTypeDescription = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxTypeDescription.ADD;
                                        if (ittr[4] == "FRE") itt.taxTypeDescription = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxTypeDescription.FRE;
                                        if (ittr[4] == "GST") itt.taxTypeDescription = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxTypeDescription.GST;
                                        if (ittr[4] == "LAC") itt.taxTypeDescription = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxTypeDescription.LAC;
                                        if (ittr[4] == "LOC") itt.taxTypeDescription = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxTypeDescription.LOC;
                                        if (ittr[4] == "OTH") itt.taxTypeDescription = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxTypeDescription.OTH;
                                        if (ittr[4] == "STT") itt.taxTypeDescription = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxTypeDescription.STT;
                                        if (ittr[4] == "VAT") itt.taxTypeDescription = AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTaxTypeDescription.VAT;
                                        if (!string.IsNullOrEmpty(ittr[5]) || !string.IsNullOrEmpty(ittr[6]))
                                        {
                                            itt.tradeItemTaxAmount = new AddendaJumex.ComprobanteAddendaRequestForPaymentLineItemTradeItemTaxInformationTradeItemTaxAmount();
                                            itt.tradeItemTaxAmount.taxAmount = Convert.ToDecimal(ittr[5]);
                                            itt.tradeItemTaxAmount.taxPercentage = Convert.ToDecimal(ittr[6]);
                                        }
                                        ITT.Add(itt);
                                    }
                                    li.tradeItemTaxInformation = ITT.ToArray();
                                }
                            }
                           

                            //------------------------------------------
                            LI.Add(li);
                        }
                        request.requestForPayment.lineItem = LI.ToArray();
                   }
                   //-------------------------------------------------

                   if (TotalAllowanceCharge.Any())
                   {
                          List<AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceCharge> TAC = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceCharge>();
                         foreach (var ta in TotalAllowanceCharge)
                         {
                             AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceCharge tac = new AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceCharge();
                             tac.allowanceOrChargeType = new AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeAllowanceOrChargeType();
                             if (ta[1] == "ALLOWANCE")
                             tac.allowanceOrChargeType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeAllowanceOrChargeType.ALLOWANCE;
                             if (ta[1] == "CHARGE")
                             tac.allowanceOrChargeType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeAllowanceOrChargeType.CHARGE;
                             if (!string.IsNullOrEmpty(ta[2]))
                             {
                                 tac.AmountSpecified = true;
                                 tac.Amount = Convert.ToDecimal(ta[2]);
                             }else
                             tac.AmountSpecified = false;
                             if (!string.IsNullOrEmpty(ta[3]))
                             {
                             tac.specialServicesType = new AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType();
                            if (ta[3] == "AA") tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.AA;
                            if (ta[3] == "ABZ") tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.ABZ;
                            if (ta[3] == "ABO") tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.ADO;
                            if (ta[3] == "ADS") tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.ADS;
                             if (ta[3] == "ADT") tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.ADT;
                           if (ta[3] == "CAC")   tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.CAC;
                             if (ta[3] == "COD")tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.COD;
                              if (ta[3] == "DA")tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.DA;
                            if (ta[3] == "DI")  tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.DI;
                              if (ta[3] == "EAA")  tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.EAA;
                               if (ta[3] == "EAB")  tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.EAB;
                                if (ta[3] == "FA") tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.FA;
                               if (ta[3] == "FC") tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.FC;
                             if (ta[3] == "FG")  tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.FG;
                           if (ta[3] == "FI")   tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.FI;
                            if (ta[3] == "HD")  tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.HD;
                            if (ta[3] == "PAD")  tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.PAD;
                             if (ta[3] == "PI") tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.PI;
                              if (ta[3] == "QD")tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.QD;
                              if (ta[3] == "RAA")tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.RAA;
                             if (ta[3] == "SAB") tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.SAB;
                                if (ta[3] == "TAE")tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.TAE;
                                if (ta[3] == "TD")tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.TD;
                                if (ta[3] == "TS")tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.TS;
                              if (ta[3] == "TX")tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.TX;
                              if (ta[3] == "TZ")tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.TZ;
                              if (ta[3] == "UM") tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.UM;
                              if (ta[3] == "VAB") tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.VAB;
                             if (ta[3] == "ZZZ")  tac.specialServicesType = AddendaJumex.ComprobanteAddendaRequestForPaymentTotalAllowanceChargeSpecialServicesType.ZZZ;
                             tac.specialServicesTypeSpecified = true;
                             }else
                             tac.specialServicesTypeSpecified = false;
                             TAC.Add(tac);
                         }
                         request.requestForPayment.TotalAllowanceCharge = TAC.ToArray();
                   }
                   //----------------------------------------------------------
                   if (tax.Any())
                   {
                       List<AddendaJumex.ComprobanteAddendaRequestForPaymentTax> TAX = new List<AddendaJumex.ComprobanteAddendaRequestForPaymentTax>();
                       foreach (var tax1 in tax)
                       {
                           AddendaJumex.ComprobanteAddendaRequestForPaymentTax taxx = new AddendaJumex.ComprobanteAddendaRequestForPaymentTax();
                           if (!string.IsNullOrEmpty(tax1[1]))
                           {
                               taxx.taxAmountSpecified = true;
                               taxx.taxAmount = Convert.ToDecimal(tax1[1]);
                           }else
                           taxx.taxAmountSpecified = false;
                           if (!string.IsNullOrEmpty(tax1[2]))
                           {

                               taxx.taxCategory = new AddendaJumex.ComprobanteAddendaRequestForPaymentTaxTaxCategory();
                               if (tax1[2] == "RETENIDO")
                                   taxx.taxCategory = AddendaJumex.ComprobanteAddendaRequestForPaymentTaxTaxCategory.RETENIDO;
                               if (tax1[2] == "TRANSFERIDO")
                                   taxx.taxCategory = AddendaJumex.ComprobanteAddendaRequestForPaymentTaxTaxCategory.TRANSFERIDO;
                               taxx.taxCategorySpecified = true;
                           }
                           else
                               taxx.taxCategorySpecified = false;
                           if (!string.IsNullOrEmpty(tax1[3]))
                           {
                               taxx.taxPercentageSpecified = true;
                               taxx.taxPercentage = Convert.ToDecimal(tax1[3]);
                           }else
                           taxx.taxPercentageSpecified = false;
                           if (!string.IsNullOrEmpty(tax1[4]))
                           {
                               taxx.typeSpecified = true;
                           taxx.type = new AddendaJumex.ComprobanteAddendaRequestForPaymentTaxType();
                           if (tax1[4] == "GST")
                           taxx.type = AddendaJumex.ComprobanteAddendaRequestForPaymentTaxType.GST;
                           if (tax1[4] == "LAC")
                           taxx.type = AddendaJumex.ComprobanteAddendaRequestForPaymentTaxType.LAC;
                           if (tax1[4] == "VAT")
                           taxx.type = AddendaJumex.ComprobanteAddendaRequestForPaymentTaxType.VAT;
                            }else
                           taxx.typeSpecified=false;
                           TAX.Add(taxx);
                       }
                       request.requestForPayment.tax = TAX.ToArray();
                   }

                   
                 //------------------------------------------------------
                          AddendaJumex Add = new AddendaJumex();
                    Add.Addenda=new AddendaJumex.ComprobanteAddenda();
                    Add.Addenda = request;
                    return Add;
               }//si tiene addenda

               return null;
              
       }//---fin metodo
       
    }
}
