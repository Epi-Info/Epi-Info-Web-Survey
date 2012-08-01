using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epi.Web.Utility;
using System.Web.Security;
namespace Epi.Web.Controllers
{
    public class PostController : Controller
    {
        //
        // GET: /Notify/

        [AcceptVerbs(HttpVerbs.Post)]
       [ValidateAntiForgeryToken]
        public JsonResult Notify(string emailAddress, string redirectUrl, string surveyName,string passCode)
        {
            try
            {
                if (EmailMessage.SendMessage(emailAddress, redirectUrl, surveyName, passCode))
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


        /// <summary>
        /// Sign out a Survey Instance
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        
      // [ValidateAntiForgeryToken]
        public JsonResult SignOut()
        {
            try
            {
                FormsAuthentication.SignOut();
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
       

      
    }
}
