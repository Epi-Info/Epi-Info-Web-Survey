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
                Epi.Web.Common.Email.Email EmailObj = new Common.Email.Email();
                EmailObj.Body = redirectUrl + " and Pass Code is: " + passCode;
                EmailObj.From = ConfigurationManager.AppSettings["EMAIL_FROM"].ToString();
                EmailObj.Subject = Uri.UnescapeDataString(emailSubject);

                List<string> tempList = new List<string>();
                tempList.Add(emailAddress);
                EmailObj.To = tempList;

                if (Epi.Web.Common.Email.EmailHandler.SendMessage(EmailObj))
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
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
