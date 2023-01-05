using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PortlandCredentials;
using PortlandPublishedCalculator.Prices;

namespace PortlandPublishedCalculator.Utility
{
    public class ArgusCheck
    {   
        // Will return true if there is Argus data for the specified date in the database
        public static bool DoesArgusDataForDateExist(DateOnly date)
        {
            using (PricesContext db = new())
            {
                try
                {
                    // Checking if the specified date exists in the database
                    YArgusomrBiofuelsNewer? rowForDate = db.YArgusomrBiofuelsNewers.Where(x => x.PublishedDate == date).FirstOrDefault();
                    if (rowForDate != null) { return true; }

                    else { return false; }
                }
                catch
                {
                    return false;
                }
            }
        }
        public static void SendArgusCheckEmail(string str)
        {
            var creds = Credentials.GetEmailCreds();
            MailMessage message = new();
            SmtpClient smtp = new();
            message.From = new MailAddress(creds.Username);
            message.To.Add(new MailAddress("it@portland-fuel.co.uk"));
            message.To.Add(new MailAddress("analytics@portland-fuel.co.uk"));
            message.Subject = "ERROR: Portland Published Wholesale Calculator - NO ARGUS DATA";
            message.Body = str;
            message.IsBodyHtml = true;

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
