using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using PortlandCredentials;

namespace PortlandPublishedCalculator.Utility
{
    public class Email
    {
        public static void SendEmail(bool ErrorChecker)
        {   
            // Retrieves the built up string that has been created during the program's runtime
            StringBuilder sb = StringBuilderPlusConsole.GetLogString();

            var creds = Credentials.GetEmailCreds();
            MailMessage message = new();
            SmtpClient smtp = new();
            message.From = new MailAddress(creds.Username);
            //message.To.Add(new MailAddress("miles@portland-fuel.co.uk"));
            message.To.Add(new MailAddress("it@portland-fuel.co.uk"));
            message.To.Add(new MailAddress("analytics@portland-fuel.co.uk"));
            if (ErrorChecker == true) { message.Subject = "ERROR: Portland Published Wholesale Calculator"; }
            else { message.Subject = "Portland Published Wholesale Calculator"; }
            message.Body = sb.ToString();
            message.IsBodyHtml= true;

            smtp.Host = "smtp-mail.outlook.com";
            smtp.Port = 587;          
            smtp.EnableSsl= true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(creds.Username, creds.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
        // Sends debug email. Only used in the Portland Diesel CIF NWE calculation so far, as we are debugging the methodology. 
        public static void SendDieselDebugEmail(string str)
        {
            var creds = Credentials.GetEmailCreds();
            MailMessage message = new();
            SmtpClient smtp = new();
            message.From = new MailAddress(creds.Username);
            message.To.Add(new MailAddress("miles@portland-fuel.co.uk"));
            message.To.Add(new MailAddress("mike@portland-fuel.co.uk"));
            message.Subject = "(DEBUG LOG) Diesel CIF NWE Calculations";
            message.Body = str;
            message.IsBodyHtml= true;

            smtp.Host = "smtp-mail.outlook.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(creds.Username, creds.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
    }
}
