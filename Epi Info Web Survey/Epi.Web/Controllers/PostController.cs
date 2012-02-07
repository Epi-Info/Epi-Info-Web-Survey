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
       
        public JsonResult Notify(string emailAddress, string redirectUrl)
        {
            
            if (EmailMessage.SendMessage(emailAddress,redirectUrl))
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
