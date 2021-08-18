using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.ServiceModel;
using System.Text;
using log4net;

namespace GeneradorCfdi
{
    public class Mailer 
    {
        readonly string _host;
        readonly string _port;
        readonly string _username;
        readonly string _password;
        //public string Bcc { get; set; }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(Mailer));

        public Mailer(string host, string port, string userName, string password)
        {
            _host = host;
            _port = port;
            _username = userName;
            _password = password;
        }

        public void Send(List<string> recipients, List<EmailAttachment> attachments, string message, string subject, string fromEmail, string fromDescription, string bcc)
        {
            try
            {
                Logger.Info("Enviando a " + recipients.Count + " emails");
                var client = new SmtpClient
                {
                    Host = this._host,
                    Port = int.Parse(this._port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(this._username, this._password)
                };
                if (_host.Contains("gmail") || _port.Contains("465") || ConfigurationManager.AppSettings["sslMail"] == "1")
                {
                    client.EnableSsl = true;
                }
                Logger.Info("Creando MailMessage");
                var mailMsg = new MailMessage
                {
                    Sender = new MailAddress(_username),
                    From = new MailAddress(fromEmail, fromDescription),
                    Subject = subject,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess,
                    Body = message,
                    BodyEncoding = Encoding.UTF8
                };

                Logger.Info("Cargando Bcc: " + bcc);
                if (!string.IsNullOrEmpty(bcc))
                {
                    var bccs = bcc.Split(';');
                    foreach (var s in bccs)
                    {
                        mailMsg.Bcc.Add(s); 
                    }
                }
                mailMsg.Headers.Add("Disposition-Notification-To", _username);
                mailMsg.IsBodyHtml = true;
                Logger.Info("Agregando attach");

                foreach (EmailAttachment attachment in attachments)
                {
                    MemoryStream mStream = new MemoryStream();

                    mStream.Write(attachment.Attachment, 0, attachment.Attachment.Length);
                    mStream.Position = 0;
                    mailMsg.Attachments.Add(new Attachment(mStream,attachment.Name));
                }
                foreach (var recipient in recipients)
                {
                    Logger.Info("Enviando a la direccion: " + recipient);
                    if (!string.IsNullOrEmpty(recipient))
                        mailMsg.To.Add(new MailAddress(recipient));
                }
                client.Send(mailMsg);
                Logger.Debug("Enviado correctamente");
            }
            catch (Exception ex)
            {
                throw;
            }

        }



        public void Send(List<string> recipients, List<string> attachments, string message, string subject,string fromEmail, string fromDescription)
        {
            try
            {
                var client = new SmtpClient
                {
                    Host = this._host,
                    Port = int.Parse(this._port),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(this._username, this._password)
                };

                var mailMsg = new MailMessage
                {
                    Sender = new MailAddress(_username),
                    From = new MailAddress(fromEmail, fromDescription),
                    Subject = subject,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess,
                    Body = message,
                    BodyEncoding = Encoding.UTF8
                };
                mailMsg.Headers.Add("Disposition-Notification-To", _username);
                mailMsg.IsBodyHtml = true;
                foreach (string attachment in attachments)
                {
                    mailMsg.Attachments.Add(new Attachment(attachment));
                }
                foreach (var recipient in recipients)
                {
                    mailMsg.To.Add(new MailAddress(recipient));
                }
                client.Send(mailMsg);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new FaultException("Ocurrio un error al enviar el correo");
            }
            
        }
    }
}
