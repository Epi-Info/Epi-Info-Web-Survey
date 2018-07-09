using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Configuration;
namespace Epi.Web.Utility
{
    public class ExceptionMessage
    { 
        /// <summary>
        /// the following method sends email messages from loggin errors 
        /// </summary>
        /// <param name="emailAddress">email address for sending message (email is NOT saved)</param>
        /// <param name="pSubjectLine">subject text</param>
        /// <param name="pMessage">Message body text</param>
        /// <returns></returns>
        public static bool SendLogMessage(Exception exc, HttpContextBase Context = null)
        {
            try
            {
                string useEmailLogging = ConfigurationManager.AppSettings["LOGGING_SEND_EMAIL_NOTIFICATION"];

                if (String.IsNullOrEmpty(useEmailLogging))
                {
                    return false;
                }
                else 
                {
                    if (useEmailLogging.ToLower() == "false")
                    {
                        return false;
                    }
                }
                
                bool isAuthenticated = false;
                bool isUsingSSL = false;
                int SMTPPort = 25;
                string AdminEmailAddress = "";
                bool IsEmailNOTIFICATION = false;
                // App Config Settings:
                // EMAIL_USE_AUTHENTICATION [ True | False ] default is False
                // EMAIL_USE_SSL [ True | False] default is False
                // SMTP_HOST [ url or ip address of smtp server ]
                // SMTP_PORT [ port number to use ] default is 25
                // EMAIL_FROM [ email address of sender and authenticator ]
                // EMAIL_PASSWORD [ password of sender and authenticator ]
                string pMessage;
                 
                pMessage = "Exception Message:\n" + exc.Message + "\n\n\n";
                    if (Context!=null){
                        pMessage += "Exception Timestamp:\n" + Context.Timestamp + "\n\n\n"
                            + "Request Path:\n " + (Context.Request).Path + "\n\n\n"
                            + "Request Method:\n" + (Context.Request).HttpMethod + "\n\n\n";
                    }
                    pMessage += "Inner Exception :\n" + exc.InnerException + "\n\n\n" +
                                "Exception StackTrace:\n" + exc.StackTrace;
                pMessage += "Source :\n" + exc.Source + "\n\n\n";
                             



                string s = ConfigurationManager.AppSettings["LOGGING_ADMIN_EMAIL_ADDRESS"];
                if (!String.IsNullOrEmpty(s))
                {
                    AdminEmailAddress = s.ToString(); 
                }


                s = ConfigurationManager.AppSettings["LOGGING_SEND_EMAIL_NOTIFICATION"];
                if (!String.IsNullOrEmpty(s))
                {
                    if (s.ToUpper() == "TRUE")
                    {
                    IsEmailNOTIFICATION = true;
                    }
                }


                    

                  s = ConfigurationManager.AppSettings["EMAIL_USE_AUTHENTICATION"];
                if (!String.IsNullOrEmpty(s))
                {
                    if (s.ToUpper() == "TRUE")
                    {
                        isAuthenticated = true;
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
                //message.To.Add(emailAddress);
                message.To.Add(AdminEmailAddress);
                message.Subject = ConfigurationManager.AppSettings["LOGGING_EMAIL_SUBJECT"].ToString(); 
                message.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["EMAIL_FROM"].ToString());
                message.Body = pMessage;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTP_HOST"].ToString());
                smtp.Port = SMTPPort;

                if (isAuthenticated)
                {
                    smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EMAIL_FROM"].ToString(), ConfigurationManager.AppSettings["EMAIL_PASSWORD"].ToString());
                }

                
                smtp.EnableSsl = isUsingSSL;

                if (IsEmailNOTIFICATION)
                {
                    smtp.Send(message);
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

   
}