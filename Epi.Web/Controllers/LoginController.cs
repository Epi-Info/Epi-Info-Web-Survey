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
using System.Reflection;
using System.Diagnostics;

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
        string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        ViewBag.Version = version;
               
           //get the responseId
            responseId = GetResponseId(ReturnUrl);
            //get the surveyId
             string SurveyId = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0].SurveyId;
             //put surveyId in viewbag so can be retrieved in Login/Index.cshtml
             ViewBag.SurveyId = SurveyId;
             if (string.IsNullOrEmpty(GetPassCode(ReturnUrl)))
             {
                 return View("Index");
             }
             else {

                 Epi.Web.Common.Message.UserAuthenticationResponse result = _isurveyFacade.ValidateUser(responseId, GetPassCode(ReturnUrl));

                 if (result.UserIsValid)
                 {
                     FormsAuthentication.SetAuthCookie(GetPassCode(ReturnUrl), false);

                     ReturnUrl = GetRedirectUrl(ReturnUrl);

                     return Redirect(ReturnUrl);
                 }
                 else
                 {
                    
                     return View();
                 }
             }
           
        }

       private string GetRedirectUrl(string ReturnUrl)
       {
           string[] expressions = ReturnUrl.Split('/');

           var Value = expressions[expressions.Length - 1];
           Guid Result;
           if (Guid.TryParse(Value, out Result))
           {
               return "";
           }
           else if (Value.Length == 4)
           {

               return ReturnUrl.Replace("/"+Value, "/1");

           }
           else
           {

               return "";
           }
       }
       [HttpPost]
      
       public ActionResult Index(PassCodeModel Model, string responseId, string ReturnUrl)
       {

          

           //parse and get the responseId
           responseId = GetResponseId(ReturnUrl);

           Common.DTO.SurveyAnswerDTO  R = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0];
            Session["RootFormId"] = R.SurveyId.ToString();
            Session["RootResponseId"] = responseId;
           // Get Last Page visited else send to page 1 - Begin

           XDocument Xdoc = XDocument.Parse(R.XML);
           int PageNumber = 0;
           if (Xdoc.Root.Attribute("LastPageVisited") != null)
           {
               if (!int.TryParse(Xdoc.Root.Attribute("LastPageVisited").Value, out PageNumber))
               {
                   PageNumber = 1;
               }
           }
           else
           {
               PageNumber = 1;
           }

           //if (ReturnUrl.EndsWith("/"))
           //{
           //    ReturnUrl = ReturnUrl + PageNumber.ToString();
           //}
           //else
           //{
           //    ReturnUrl = ReturnUrl + "/" + PageNumber.ToString();
           //}


           // Get Last Page visited else send to page 1 - End



           //get the surveyId
           string SurveyId = R.SurveyId;
           //put surveyId in viewbag so can be retrieved in Login/Index.cshtml
           ViewBag.SurveyId = SurveyId;


           Epi.Web.Common.Message.UserAuthenticationResponse result = _isurveyFacade.ValidateUser(responseId, Model.PassCode);

           if (result.UserIsValid)
           {
               FormsAuthentication.SetAuthCookie(Model.PassCode, false);
              // return RedirectToRoute(new { Controller = "Survey", Action = "Index", responseid = responseId });
               
              
               return Redirect(ReturnUrl);
           }
           else
           {
               ModelState.AddModelError("", "Pass code is incorrect.");
               return View();
           }
       }

      
      /// <summary>
      /// parse and return the responseId from response Url 
      /// </summary>
      /// <param name="returnUrl"></param>
      /// <returns></returns>
        private string GetResponseId(string returnUrl)
       {
           string responseId = string.Empty;
           string[] expressions = returnUrl.Split('/');

           foreach (var expression in expressions)
           {
               if (Epi.Web.MVC.Utility.SurveyHelper.IsGuid(expression))
               {

                   responseId = expression;
                   break;
               }

           }
           return responseId;
       }
        private  string  GetPassCode(string returnUrl)
        {
           
            string[] expressions = returnUrl.Split('/');

            var Value = expressions[expressions.Length - 1];
            Guid Result ;
            if (Guid.TryParse(Value, out Result))
             {
                 return "";
             }
            else if (Value.Length == 4)
            {
                return Value;

            }
            else {

                return "";
            }
        }
    }
}
