using System;
using System.Web.Mvc;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Epi.Core.EnterInterpreter;
using System.Web.Security;


namespace Epi.Web.MVC.Controllers
{
    public class LoginController : Controller
    {
        //declare SurveyTransactionObject object
        private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;
        /// <summary>
        /// Injectinting SurveyTransactionObject through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        
        public LoginController(   Epi.Web.MVC.Facade.ISurveyFacade  isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
        
        // GET: /Login/
       [HttpGet]
        public ActionResult Index(string responseId, string ReturnUrl)
        {
            
           
          
          
            return View("Index");
        }
       [HttpPost]
       public ActionResult Index(PassCodeModel Model, string responseId, string ReturnUrl)
       {


           string[] expressions = ReturnUrl.Split('/');

           foreach (var expression in expressions)
           {
               if (Epi.Web.MVC.Utility.SurveyHelper.IsGuid(expression))
               {

                       responseId = expression;
                       break;
               }
           
           }
           

           Epi.Web.Common.Message.UserAuthenticationResponse result = _isurveyFacade.ValidateUser(responseId, Model.PassCode);

           if (result.UserIsValid)
           {
               FormsAuthentication.SetAuthCookie(Model.PassCode, false);
              // return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseid = responseId });

               return Redirect(ReturnUrl ?? Url.Action("Index", "Admin", responseId));
           }
           else
           {
               ModelState.AddModelError("", "Pass code is incorrect.");
               return View();
           }
       }

       
      
    }
}
