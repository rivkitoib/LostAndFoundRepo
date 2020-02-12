//using EASendMail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace LostAndFound.Controllers
{
    public class SendMail
    {
        public static void send()
        {
            string from = "rivki.toib@gmail.com"; //Replace this with your own correct Gmail Address

            string to = "tzippyglantz@gmail.com"; //Replace this with the Email Address to whom you want to send the mail

        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(from, "והשבות", System.Text.Encoding.UTF32);
            mail.Subject = "This is a test mail";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Hi Tzippy! \\nSay hello!";
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            SmtpClient client = new SmtpClient();
            //Add the Creddentials- use your own email id and password

            client.Credentials = new System.Net.NetworkCredential(from, "207456054");
            client.Port = 587; // Gmail works on this port<o:p />
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true; //Gmail works on Server Secured Layer
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                HttpContext.Current.Response.Write(errorMessage);
            } // end try 

        }
    }
}