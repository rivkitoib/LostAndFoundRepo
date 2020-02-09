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
        //public static void send()
        //{
        //    try
        //    {
        //        SmtpMail oMail = new SmtpMail("TryIt");

        //        // Set sender email address, please change it to yours
        //        oMail.From = "rivki.toib@gmail.com";

        //        // Set recipient email address, please change it to yours
        //        oMail.To = "estie.toib@gmail.com";

        //        // Set email subject
        //        oMail.Subject = "test email from c# project";

        //        // Set email body
        //        oMail.TextBody = "this is a test email sent from c# project, do not reply";

        //        // SMTP server address
        //        SmtpServer oServer = new SmtpServer("smtp.gmail.com");

        //        // User and password for ESMTP authentication
        //        oServer.User = "rivki.toib@gmail.com";
        //        oServer.Password = "207456054";

        //        // Most mordern SMTP servers require SSL/TLS connection now.
        //        // ConnectTryTLS means if server supports SSL/TLS, SSL/TLS will be used automatically.
        //        oServer.ConnectType = SmtpConnectType.ConnectTryTLS;

        //        // If your SMTP server uses 587 port
        //         oServer.Port = 587;

        //        // If your SMTP server requires SSL/TLS connection on 25/587/465 port
        //     //   oServer.Port = 445; // 25 or 587 or 465
        //    //    oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
        //        oServer.UseDefaultCredentials = true;
        //        Console.WriteLine("start to send email ...");

        //        SmtpClient oSmtp = new SmtpClient();
        //        oSmtp.SendMail(oServer, oMail);

        //    }
        //    catch (Exception ep)
        //    {
        //        Console.WriteLine("failed to send email with the following error:");
        //        Console.WriteLine(ep.Message);
        //    }
        //}
        //https://www.codeproject.com/Articles/26971/Sending-E-mail-using-ASP-net-through-Gmail-account
        //  public static void send2()
        //  {
        //      try
        //      {
        //          MailMessage mailMessage = new MailMessage();
        //          mailMessage.To.Add("rivki.toib@gmail.com");
        //          mailMessage.From = new MailAddress("rivki.toib@gmail.com");
        //          mailMessage.Subject = "ASP.NET e-mail test";
        //          mailMessage.Body = "Hello world,\n\nThis is an ASP.NET test e-mail!";
        //          SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
        //          SmtpClient client = new SmtpClient();
        //          smtpClient.Credentials = new System.Net.NetworkCredential("rivki.toib@gmail.com", "207456054");
        //          client.Host = "smtpout.secureserver.net";
        //          smtpClient.Port = 587;
        //          smtpClient.Send(mailMessage);
        //          Console.Write("E-mail sent!");
        //      }
        //      catch (Exception ex)
        //      {
        //          Console.Write("Could not send the e-mail - error: " + ex.Message);
        //      }
        //  }

        public static void send3()
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