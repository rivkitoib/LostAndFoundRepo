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

namespace LostAndFound.eMail
{
    public class SendMail
    {
       static DbHandle DB = new DbHandle();
        public static void send()
        {
            string from = "vehashevotaSystem@gmail.com";

            string to = "tzippyglantz@gmail.com";

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(from, ":)", System.Text.Encoding.UTF32);
            mail.Subject = "This is a test mail";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Hi Tzippy! \\nSay hello!";
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;


            ////ContentType c = new ContentType(pathToImage); 
            //Attachment img = new Attachment(pathToImage,);
            //Attachment img = new Attachment(file,"o");
          //  var path = "C:\\Users\\1000270290\\Documents\\MVC\\trial2\\page.htm";
            var path = "email.htm";
            string Body = System.IO.File.ReadAllText(path);
            
            mail.Body = Body;
            var pathToImage = "C:\\Users\\1000270290\\Pictures\\Camera Roll\\1.jpg";
          
            FileStream file = new FileStream(pathToImage, FileMode.Open);  
            Attachment img = new Attachment(file,"Original.jpg");
            mail.Attachments.Add(img);
            file.Close();


            SmtpClient client = new SmtpClient();

            client.Credentials = new System.Net.NetworkCredential(from, "vehashevota1");
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
                //  string from = DB.Settings;
            string from = ConfigurationManager.AppSettings["Email"];
            string EmailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            string to = findInDB.email;

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(to);
            var mailFrom = "והשבות";
            mail.From = new MailAddress(from, mailFrom, System.Text.Encoding.UTF32);
            mail.Subject = "המציאה נקלטה בהצלחה";
                //mail.SubjectEncoding = System.Text.Encoding.UTF8;
                //mail.Body = "Hi Tzippy! \\nSay hello!";
                //mail.BodyEncoding = System.Text.Encoding.UTF8;
               mail.IsBodyHtml = true;
            //mail.Priority = MailPriority.High;
          
            var path = ConfigurationManager.AppSettings["mailContent"];

            string Body = System.IO.File.ReadAllText(path);
          Body= Body.Replace("@#name", findInDB.finderName);
          Body= Body.Replace("@#description", findInDB.description);
          Body= Body.Replace("@#location", findInDB.location.PlaceOrEvent);
            mail.Body = Body;

            string pathToImage = findInDB.picture;
            FileStream file = new FileStream(pathToImage, FileMode.Open);
            Attachment img = new Attachment(file, "Visible.jpg");
            mail.Attachments.Add(img);

            //TODO
            //pathToImage = pathToImage.Replace("DB_", "");
            //pathToImage = pathToImage.Replace("ForDB", "Original");
            //FileStream file2 = new FileStream(pathToImage, FileMode.Open);
            //img = new Attachment(file2, "Original.jpg");
            //mail.Attachments.Add(img);
            //file2.Close();

            SmtpClient host = new SmtpClient();

            host.Credentials = new System.Net.NetworkCredential(from, EmailPassword);
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
   
            file.Close();
        }

    }
}
