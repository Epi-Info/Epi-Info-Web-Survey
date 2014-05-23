using System;
using System.IO;
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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.Routing;
using System.Web.WebPages;
using System.Web.Caching;
using System.Reflection;
using System.Diagnostics;
using System.Reflection;
using System.Diagnostics;
using Epi.Web.Common.Message;
using Epi.Web.Common.DTO;
namespace Epi.Web.MVC.Controllers
{
  
    public class AccountController : Controller
    {
    private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;

    public AccountController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
        {
        _isurveyFacade = isurveyFacade;
        }

        // GET: /Account/
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult Index()
        {
           string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
           ViewBag.Version = version;
           string filepath = Server.MapPath("~\\Content\\Text\\TermOfUse.txt");
           string content = string.Empty;
           AccountInfo Obj = new AccountInfo();
           try
               {
               using (var stream = new StreamReader(filepath))
                   {
                   content = stream.ReadToEnd();
                   }
               }
           catch (Exception exc)
               {
 
               }
         ViewData["TermOfUse"] = content;
           return View(Obj);
        }
        [HttpPost]

        public ActionResult Index(Epi.Web.MVC.Models.AccountInfo AccountInfo) 
            {
            string filepath = Server.MapPath("\\Content\\Text\\TermOfUse.txt");
            string content = string.Empty;

            string ApplicantValidation = ConfigurationManager.AppSettings["APPLICANT_VALIDATION_IS_ENABLED"];
            try
                {
                using (var stream = new StreamReader(filepath))
                    {
                    content = stream.ReadToEnd();
                    }
                }
            catch (Exception exc)
                {

                }
            ViewData["TermOfUse"] = content;
           
            try
                {
                OrganizationAccountResponse Response = new OrganizationAccountResponse();
                OrganizationAccountRequest Request = new OrganizationAccountRequest();
                AdminDTO AdminDTO = new AdminDTO();
                OrganizationDTO OrganizationDTO = new OrganizationDTO();
                Guid OrgKey = Guid.NewGuid();
                Guid AdminKey = Guid.NewGuid();

                AdminDTO.AdminEmail = AccountInfo.Email;
                AdminDTO.IsActive = true;
                OrganizationDTO.IsEnabled = true;


                OrganizationDTO.Organization = AccountInfo.OrgName;

                OrganizationDTO.OrganizationKey = OrgKey.ToString();

                Request.Organization = OrganizationDTO;
                Request.Admin = AdminDTO;

                if(ModelState.IsValid){

                        Response = _isurveyFacade.CreateAccount(Request);
                    
                    }
                else
                    {
                    
                      return View(AccountInfo);
                    }

                if (Response.Message.ToUpper() == "SUCCESS")
                    {
                    AccountInfo.Status = "SUCCESS";
                    return View(AccountInfo);
                    }
                else
                    {
                    ModelState.AddModelError("Error", "This organization name and/or email address already exists.");
                   
                    return View(AccountInfo);
                    }



                }
            catch (Exception ex)
                {
                ModelState.AddModelError("Error", "An error occurred while trying to create an account. Please try again later.");
                return View(AccountInfo);
                }
           }
    }
}
