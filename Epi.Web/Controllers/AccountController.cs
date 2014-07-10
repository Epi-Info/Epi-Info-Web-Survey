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
        OrganizationAccountResponse Response = new OrganizationAccountResponse();
        OrganizationAccountRequest Request = new OrganizationAccountRequest();
        string AccountType = ConfigurationManager.AppSettings["ACCOUNT_TYPE"];
           string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
           ViewBag.Version = version;
           string filepath = Server.MapPath("~\\Content\\Text\\TermOfUse.txt");
           string content = string.Empty;
           AccountInfoModel Model = new AccountInfoModel();
          // Model.AccountType = ConfigurationManager.AppSettings["ACCOUNT_TYPE"];
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

             Response = _isurveyFacade.GetStateList(Request);
            //Model.States.Add(new SelectListItem { Text = "Select a State", Value = "0" });

            foreach (var item in Response.StateList)
                {
                
                Model.States.Add(new SelectListItem { Text =item.StateName, Value =item.StateId.ToString() });
                }
            if (string.IsNullOrEmpty(AccountType))
                {
               return View("AccessDenied");
                
                }
            else
                {

                return View(Model);
                }
         
        }
        [HttpPost]

        public ActionResult Index(Epi.Web.MVC.Models.AccountInfoModel AccountInfo) 
            {
            string filepath = Server.MapPath("~\\Content\\Text\\TermOfUse.txt");
            string content = string.Empty;
            string AccountType = ConfigurationManager.AppSettings["ACCOUNT_TYPE"];
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
                OrganizationAccountResponse StateResponse = new OrganizationAccountResponse();
                OrganizationAccountRequest Request = new OrganizationAccountRequest();
                AdminDTO AdminDTO = new AdminDTO();
                OrganizationDTO OrganizationDTO = new OrganizationDTO();
                Guid OrgKey = Guid.NewGuid();
                Guid AdminKey = Guid.NewGuid();

                AdminDTO.AdminEmail = AccountInfo.Email;
                AdminDTO.FirstName = AccountInfo.FirstName;
                AdminDTO.LastName = AccountInfo.LastName;
                AdminDTO.PhoneNumber = AccountInfo.PhoneNumber;
                AdminDTO.AdressLine1 = AccountInfo.AdressLine1;
                AdminDTO.AdressLine2 = AccountInfo.AdressLine2;
                AdminDTO.City = AccountInfo.City;
                AdminDTO.StateId = AccountInfo.SelectedState;
                AdminDTO.Zip = AccountInfo.Zip;
                AdminDTO.IsActive = true;
                OrganizationDTO.IsEnabled = true;


                OrganizationDTO.Organization = AccountInfo.OrgName;

                OrganizationDTO.OrganizationKey = OrgKey.ToString();
                Request.AccountType = AccountType;
                Request.Organization = OrganizationDTO;
                Request.Admin = AdminDTO;
                if (AccountType.ToUpper() !="USER")
                    {
                    this.ModelState.Remove("LastName");
                    this.ModelState.Remove("FirstName");
                    this.ModelState.Remove("PhoneNumber");

                    this.ModelState.Remove("AdressLine1");
                    
                    this.ModelState.Remove("City");
                    this.ModelState.Remove("State");
                    this.ModelState.Remove("Zip");
                    }
                StateResponse = _isurveyFacade.GetStateList(Request);
                
                foreach (var item in StateResponse.StateList)
                    {

                    AccountInfo.States.Add(new SelectListItem { Text = item.StateName, Value = item.StateId.ToString() });
                    }
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
                    if (AccountType.ToUpper() == "USER")
                        {
                        ModelState.AddModelError("Error", "This organization name and/or email address already exists.");
                        }
                    else
                        {
                        ModelState.AddModelError("Error", "This organization name already exists.");
                        }
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
