using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epi.Web.Utility;
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
            
            if (EmailMessage.SendMessage(emailAddress,redirectUrl,surveyName,passCode))
            {
                return Json(true);
            }
            else 
            {
                return Json(false);
            }
        }
      
    }
}
