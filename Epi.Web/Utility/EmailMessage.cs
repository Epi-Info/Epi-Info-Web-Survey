using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;
namespace Epi.Web.Utility
{
    public class EmailMessage
    {
        /// <summary>
        /// the following method takes email and responseUrl as argument and email redirection url to the user 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public static bool SendMessage(string emailAddress, string redirectUrl,string surveyName,string passCode)
        {
            try
            {
                //System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                //message.To.Add(emailAddress);
                //message.Subject = ConfigurationManager.AppSettings["EMAIL_SUBJECT"].ToString();   //"This is the Subject line";
                //message.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["EMAIL_FROM"].ToString());//sender email address
                //message.Body = redirectUrl;
                //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["PORT"].ToString());//port 
                //smtp.Send(message);
                //return true;

                //for test
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(emailAddress);
                message.Subject = "Link for Survey: " + surveyName; //ConfigurationManager.AppSettings["EMAIL_SUBJECT"].ToString();   //"This is the Subject line";
                message.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["EMAIL_FROM"].ToString());//sender email address
                message.Body = redirectUrl + " and Pass Code is: " + passCode;
                
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTP_HOST"].ToString());//server 
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EMAIL_FROM"].ToString(), ConfigurationManager.AppSettings["EMAIL_PASSWORD"].ToString());
                //smtp.EnableSsl = true;
                smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["SMTP_PORT"].ToString());
                smtp.EnableSsl = true;

                smtp.Send(message);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}