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
        /// <param name="emailAddress">email address for sending message (email is NOT saved)</param>
        /// <param name="redirectUrl">url for resuming the saved survey</param>
        /// <param name="surveyName">Name of the survey</param>
        /// <param name="passCode"> Code for accessing an unfinished survey </param>
        /// <returns></returns>
        public static bool SendMessage(string emailAddress, string redirectUrl, string surveyName, string passCode)
        {
            try
            {
                bool isAuthenticated = false;
                bool isUsingSSL = false;
                int SMTPPort = 25;

                // App Config Settings:
                // EMAIL_USE_AUTHENTICATION [ True | False ] default is False
                // EMAIL_USE_SSL [ True | False] default is False
                // SMTP_HOST [ url or ip address of smtp server ]
                // SMTP_PORT [ port number to use ] default is 25
                // EMAIL_FROM [ email address of sender and authenticator ]
                // EMAIL_PASSWORD [ password of sender and authenticator ]


                string s = ConfigurationManager.AppSettings["EMAIL_USE_AUTHENTICATION"];
                if (!String.IsNullOrEmpty(s))
                {
                    if (s.ToUpper() == "TRUE")
                    {
                        isUsingSSL = true;
                    }
                }

                s = ConfigurationManager.AppSettings["EMAIL_USE_SSL"];
                if (!String.IsNullOrEmpty(s))
                {
                    if (s.ToUpper() == "TRUE")
                    {
                        isUsingSSL = true;
                    }
                }

                s = ConfigurationManager.AppSettings["SMTP_PORT"];
                if (!int.TryParse(s, out SMTPPort))
                {
                    SMTPPort = 25;
                }

                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(emailAddress);
                message.Subject = "Link for Survey: " + surveyName; 
                message.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["EMAIL_FROM"].ToString());
                message.Body = redirectUrl + " and Pass Code is: " + passCode;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTP_HOST"].ToString());
                smtp.Port = SMTPPort;

                if (isAuthenticated)
                {
                    smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EMAIL_FROM"].ToString(), ConfigurationManager.AppSettings["EMAIL_PASSWORD"].ToString());
                }

                if (isUsingSSL)
                {
                    smtp.EnableSsl = true;
                }

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