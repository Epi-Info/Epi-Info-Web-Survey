using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epi.Web.Utility;
using System.Web.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;
namespace Epi.Web.Controllers
{
    public class PostController : Controller
    {
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public JsonResult Notify(string emailAddress, string redirectUrl, string surveyName, string passCode, string emailSubject)
        {
            try
            {
                if (redirectUrl.Contains("&IsSaved=True"))
                {
                    redirectUrl = redirectUrl.Replace("&IsSaved=True","");
                
                }
                if (redirectUrl.Contains("&IsSaved=False"))
                {
                    redirectUrl = redirectUrl.Replace("&IsSaved=False", "");

                }

                Epi.Web.Common.Email.Email surveyLinkEmail = new Common.Email.Email();
                surveyLinkEmail.Body = 
                    redirectUrl + 
                    "<br/><br/>" +
                    "For enhanced security, the information needed to access your response securely is provided in two emails.";
                surveyLinkEmail.From = ConfigurationManager.AppSettings["EMAIL_FROM"].ToString();
                surveyLinkEmail.Subject = Uri.UnescapeDataString("Link for survey: " + surveyName);

                List<string> toList = new List<string>();
                toList.Add(emailAddress);
                surveyLinkEmail.To = toList;

                if (Epi.Web.Common.Email.EmailHandler.SendMessage(surveyLinkEmail))
                {
                    Epi.Web.Common.Email.Email passcodeEmail = new Common.Email.Email();
                    passcodeEmail.Body = passCode.Trim() + "<br/><br/>";
                    passcodeEmail.From = ConfigurationManager.AppSettings["EMAIL_FROM"].ToString();
                    passcodeEmail.Subject = Uri.UnescapeDataString("Information for survey: " + surveyName);
                    passcodeEmail.To = toList;

                    if (Epi.Web.Common.Email.EmailHandler.SendMessage(passcodeEmail))
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }

            }
            catch 
            {
                return Json(false);
            }
        }

        [HttpPost]
        public JsonResult SignOut()
        {
            try
            {
                FormsAuthentication.SignOut();
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }
    }
}
