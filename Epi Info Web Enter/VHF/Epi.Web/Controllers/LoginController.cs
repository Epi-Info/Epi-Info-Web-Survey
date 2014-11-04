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
using Epi.Web.Enter.Common.Constants;
using System.Linq;
using System.Configuration;

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

        public LoginController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }

        // GET: /Login/
        [HttpGet]
        public ActionResult Index(string responseId, string ReturnUrl)
        {
            //string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //ViewBag.Version = version;

            //   //get the responseId
            //    responseId = GetResponseId(ReturnUrl);
            //    //get the surveyId
            //     string SurveyId = _isurveyFacade.GetSurveyAnswerResponse(responseId).SurveyResponseList[0].SurveyId;
            //     //put surveyId in viewbag so can be retrieved in Login/Index.cshtml
            //     ViewBag.SurveyId = SurveyId;

            return View("Index");
        }
        [HttpPost]

        public ActionResult Index(UserLoginModel Model, string Action, string ReturnUrl)
        {

            return ValidateUser(Model.UserName, Model.Password, ReturnUrl);

            //if (ReturnUrl == null || !ReturnUrl.Contains("/"))
            //{
            //    ReturnUrl = "/Home/Index";
            //}


            //Epi.Web.Enter.Common.Message.UserAuthenticationResponse result = _isurveyFacade.ValidateUser(Model.UserName, Model.Password);

            //if (result.UserIsValid)
            //{
            //    if (result.User.ResetPassword)
            //    {
            //        return ResetPassword(Model.UserName);
            //    }
            //    else
            //    {

            //        FormsAuthentication.SetAuthCookie(Model.UserName, false);
            //        string UserId = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(result.User.UserId.ToString());
            //        Session["UserId"] = UserId;
            //        return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, "Home", new { surveyid = "" });
            //    }
            //}
            //else
            //{
            //    ModelState.AddModelError("", "The email or password you entered is incorrect.");
            //    return View();
            //}
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


        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        //[HttpGet]
        public ActionResult ResetPassword(UserResetPasswordModel model)
        {
            return View("ResetPassword", model);
        }

        [HttpPost]
        public ActionResult ForgotPassword(UserForgotPasswordModel Model, string Action, string ReturnUrl)
        {
            switch (Action.ToUpper())
            {
                case "CANCEL":
                    return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, "Login");
                default:
                    break;
            }

            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                List<string> errorMessages = new List<string>();

                string msg = ModelState.First().Value.Errors.First().ErrorMessage.ToString();

                ModelState.AddModelError("", msg);


                return View("ForgotPassword", Model);
            }

            bool success = _isurveyFacade.UpdateUser(new Enter.Common.DTO.UserDTO() { UserName = Model.UserName, Operation = Constant.OperationMode.UpdatePassword });
            if (success)
            {
                return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, "Login");
            }
            else
            {
                ModelState.AddModelError("", "Error sending email.");
                return View("ForgotPassword", Model);
            }

        }

        [HttpPost]
        public ActionResult ResetPassword(UserResetPasswordModel Model, string Action, string ReturnUrl)
        {

            switch (Action.ToUpper())
            {
                case "CANCEL":
                    return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, "Login");
                default:
                    break;
            }

            if (!ModelState.IsValid)
            {
                UserResetPasswordModel model = new UserResetPasswordModel();
                model.UserName = Model.UserName;
                ModelState.AddModelError("", "Passwords do not match. Please try again.");
                return View("ResetPassword", Model);
            }

            if (!ValidatePassword(Model))
            {
                ModelState.AddModelError("", "Password is not strong enough. Please try again.");
                return View("ResetPassword", Model);
            }

            _isurveyFacade.UpdateUser(new Enter.Common.DTO.UserDTO() { UserName = Model.UserName, PasswordHash = Model.Password, Operation = Constant.OperationMode.UpdatePassword, ResetPassword = true });

            return ValidateUser(Model.UserName, Model.Password, ReturnUrl);

        }

        private ActionResult ValidateUser(string UserName, string Password, string ReturnUrl)
        {
            string formId = "", pageNumber;
            if (ReturnUrl == null || !ReturnUrl.Contains("/"))
            {
                ReturnUrl = "/Home/Index";
            }
            else
            {
                formId = ReturnUrl.Substring(0, ReturnUrl.IndexOf('/'));
                pageNumber = ReturnUrl.Substring(ReturnUrl.LastIndexOf('/') + 1);
            }

            try
            {
                Epi.Web.Enter.Common.Message.UserAuthenticationResponse result = _isurveyFacade.ValidateUser(UserName, Password);
                if (result.UserIsValid)
                {
                    if (result.User.ResetPassword)
                    {
                        UserResetPasswordModel model = new UserResetPasswordModel();
                        model.UserName = UserName;
                        model.FirstName = result.User.FirstName;
                        model.LastName = result.User.LastName;
                        ReadPasswordPolicy(model);
                        return ResetPassword(model);
                    }
                    else
                    {

                        FormsAuthentication.SetAuthCookie(UserName, false);
                        string UserId = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(result.User.UserId.ToString());
                        Session["UserId"] = UserId;
                        Session["UserHighestRole"] = result.User.Role;
                        return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, "Home", new { surveyid = formId });
                        //return Redirect(ReturnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The email or password you entered is incorrect.");
                    return View();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "The email or password you entered is incorrect.");
                return View();
                throw;
            }


            
        }

        private bool ValidatePassword(UserResetPasswordModel Model)
        {
            //int minLength = Convert.ToInt16(ConfigurationManager.AppSettings["PasswordMinimumLength"]);
            //int maxLength = Convert.ToInt16(ConfigurationManager.AppSettings["PasswordMaximumLength"]);
            //bool useSymbols = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSymbols"]); //= false;
            //bool useNumeric = Convert.ToBoolean(ConfigurationManager.AppSettings["UseNumbers"]); //= false;
            //bool useLowerCase = Convert.ToBoolean(ConfigurationManager.AppSettings["UseLowerCase"]);
            //bool useUpperCase = Convert.ToBoolean(ConfigurationManager.AppSettings["UseUpperCase"]);
            //bool useUserIdInPassword = Convert.ToBoolean(ConfigurationManager.AppSettings["UseUserIdInPassword"]);
            //bool useUserNameInPassword = Convert.ToBoolean(ConfigurationManager.AppSettings["UseUserNameInPassword"]);
            //int numberOfTypesRequiredInPassword = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfTypesRequiredInPassword"]);

            ReadPasswordPolicy(Model);

            int successCounter = 0;

            if (Model.UseSymbols && HasSymbol(Model.Password))
            {
                successCounter++;
            }

            if (Model.UseUpperCase && HasUpperCase(Model.Password))
            {
                successCounter++;
            }
            if (Model.UseLowerCase && HasLowerCase(Model.Password))
            {
                successCounter++;
            }
            if (Model.UseNumeric && HasNumber(Model.Password))
            {
                successCounter++;
            }

            if (Model.UseUserIdInPassword)
            {
                if (Model.Password.ToString().Contains(Model.UserName.Split('@')[0].ToString()))
                {
                    successCounter = 0;
                }

            }

            if (Model.UseUserNameInPassword)
            {
                if (Model.Password.ToString().Contains(Model.FirstName) || Model.Password.ToString().Contains(Model.LastName))
                {
                    successCounter = 0;
                }
            }

            if (Model.Password.Length < Model.MinimumLength || Model.Password.Length > Model.MaximumLength)
            {
                return false;
            }

            if (Model.NumberOfTypesRequiredInPassword <= successCounter && successCounter != 0)
            {
                return true;
            }

            return false;
        }

        private bool HasNumber(string password)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(password, @"\d");
        }

        private bool HasUpperCase(string password)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]");
        }

        private bool HasLowerCase(string password)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]");
        }

        private bool HasSymbol(string password)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(password, @"[" + ConfigurationManager.AppSettings["Symbols"].Replace(" ", "") + "]");
        }

        private void ReadPasswordPolicy(UserResetPasswordModel Model)
        {
            Model.MinimumLength = Convert.ToInt16(ConfigurationManager.AppSettings["PasswordMinimumLength"]);
            Model.MaximumLength = Convert.ToInt16(ConfigurationManager.AppSettings["PasswordMaximumLength"]);
            Model.UseSymbols = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSymbols"]); //= false;
            Model.UseNumeric = Convert.ToBoolean(ConfigurationManager.AppSettings["UseNumbers"]); //= false;
            Model.UseLowerCase = Convert.ToBoolean(ConfigurationManager.AppSettings["UseLowerCase"]);
            Model.UseUpperCase = Convert.ToBoolean(ConfigurationManager.AppSettings["UseUpperCase"]);
            Model.UseUserIdInPassword = Convert.ToBoolean(ConfigurationManager.AppSettings["UseUserIdInPassword"]);
            Model.UseUserNameInPassword = Convert.ToBoolean(ConfigurationManager.AppSettings["UseUserNameInPassword"]);
            Model.NumberOfTypesRequiredInPassword = Convert.ToInt16(ConfigurationManager.AppSettings["NumberOfTypesRequiredInPassword"]);
            Model.Symbols = ConfigurationManager.AppSettings["Symbols"];
        }
    }
}
