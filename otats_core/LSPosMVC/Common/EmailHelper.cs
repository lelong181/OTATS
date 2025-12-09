using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Mvc;

namespace LSPosMVC.Common
{
    public class EmailHelper
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(EmailHelper));

        public void SendEmail(string htmlString, string Email, string[] To, string subject)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailAccount"]);
                message.To.Add(new MailAddress(Email));
                if (To != null)
                {
                    foreach (string m in To)
                    {
                        message.To.Add(new MailAddress(m));
                    }
                }
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["EmailAccount"], System.Configuration.ConfigurationManager.AppSettings["EmailPassword"]);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}