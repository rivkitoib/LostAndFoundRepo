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
            string from = "rivki.toib@gmail.com";

            string to = "tzippyglantz@gmail.com";

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
//using (StreamReader reader = File.OpenText(htmlFilePath)) // Path to your 
//{                                                         // HTML file
//    MailMessage myMail = new MailMessage();
//myMail.From = "from@microsoft.com";
//    myMail.To = "to@microsoft.com";
//    myMail.Subject = "HTML Message";
//    myMail.BodyFormat = MailFormat.Html;

//    myMail.Body = reader.ReadToEnd();  // Load the content from your file...
//    //...
//}