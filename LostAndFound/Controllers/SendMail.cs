//using EASendMail;
using LostAndFound.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace LostAndFound.Controllers
{
    public class SendMail
    {
       static DbHandle DB = new DbHandle();
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
        public static void sendFinderFind(Find findInDB)
        {

            var pathToImage = findInDB.picture.Substring(3);

                //  string from = DB.Settings;
                string from = ConfigurationManager.AppSettings["Email"];
            string EmailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            string to = findInDB.email;

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(from, "והשבות", System.Text.Encoding.UTF32);
            mail.Subject = "המציאה נקלטה בהצלחה";
                //mail.SubjectEncoding = System.Text.Encoding.UTF8;
                //mail.Body = "Hi Tzippy! \\nSay hello!";
                //mail.BodyEncoding = System.Text.Encoding.UTF8;
               mail.IsBodyHtml = true;
            //mail.Priority = MailPriority.High;
            mail.Body = "<div>שלום</div>";


                //  mail.BodyFormat = MailFormat.Html;
                //
                //  mail.Body = reader.ReadToEnd();  
                // Load the content from your file...
                // 
                
                Attachment img = new Attachment(pathToImage, MediaTypeNames.Application.Octet);
                img.Name = "תמונה_מקורית";
                mail.Attachments.Add(img);
                img = new Attachment(findInDB.picture, MediaTypeNames.Application.Octet);
                img.Name = "תמונה_מוסתרת";
                SmtpClient host = new SmtpClient();

            host.Credentials = new System.Net.NetworkCredential(from, "207456054");
            host.Port = 587; // Gmail works on this port<o:p />
            host.Host = "smtp.gmail.com";
            host.EnableSsl = true; //Gmail works on Server Secured Layer
            try
            {
                host.Send(mail);
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
//        using (StreamReader reader = File.OpenText(htmlFilePath)) // Path to your 
//{                                                         // HTML file
//    MailMessage myMail = new MailMessage();
//    myMail.From = "from@microsoft.com";
//    myMail.To = "to@microsoft.com";
//    myMail.Subject = "HTML Message";
//    myMail.BodyFormat = MailFormat.Html;

//    myMail.Body = reader.ReadToEnd();  // Load the content from your file...
//    //...
//}