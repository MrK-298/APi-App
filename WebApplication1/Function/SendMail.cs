using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Configuration;

namespace WebApplication1.Function
{
    public class SendMail
    {
        private static string Email = "hutechdriver@gmail.com";
        private static string password = "dlpywleqrnjkjdmm";
        public static void SendEmail(string to, string subject, string body, string attachFile)
        {

            try
            {
                MailMessage msg = new MailMessage("hutechdriver@gmail.com", to, subject, body);
                msg.IsBodyHtml = true;
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    if (!string.IsNullOrEmpty(attachFile))
                    {
                        Attachment attachment = new Attachment(attachFile);
                        msg.Attachments.Add(attachment);
                    }
                    NetworkCredential credential = new NetworkCredential(Email, password);
                    client.UseDefaultCredentials = false;
                    client.Credentials = credential;
                    client.Send(msg);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
