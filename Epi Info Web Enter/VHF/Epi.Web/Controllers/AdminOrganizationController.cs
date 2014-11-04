using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Epi.Web.Enter.Common.Message;
using Epi.Web.MVC.Utility;
using Epi.Web.Enter.Common.DTO;
using System.Web.Configuration;
using Epi.Web.MVC.Models;
using System.Text.RegularExpressions;
namespace Epi.Web.MVC.Controllers
{
    public class AdminOrganizationController : Controller
    {

        private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;

        public AdminOrganizationController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
        [HttpGet]
        public ActionResult OrgList()
        {
            int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
            int UserHighestRole = int.Parse(Session["UserHighestRole"].ToString());
            OrganizationRequest Request = new OrganizationRequest();
            Request.UserId = UserId;
            Request.UserRole = UserHighestRole;
            OrganizationResponse Organizations = _isurveyFacade.GetUserOrganizations(Request);

            List<OrganizationModel> Model = Mapper.ToOrganizationModelList(Organizations.OrganizationList);
            OrgListModel OrgListModel = new OrgListModel();
            OrgListModel.OrganizationList = Model;

            if (UserHighestRole == 3)
            {
                return View("OrgList", OrgListModel);
            }
            else
            {

                return RedirectToAction("UserList", "AdminUser");
            }

        }


        [HttpGet]
        public ActionResult OrgInfo(string orgkey, bool iseditmode)
        {
            OrgAdminInfoModel OrgInfo = new OrgAdminInfoModel();

            if (iseditmode)
            {
                OrganizationRequest Request = new OrganizationRequest();
                Request.Organization.OrganizationKey = orgkey;

                OrganizationResponse Organizations = _isurveyFacade.GetOrganizationInfo(Request);
                OrgInfo = Mapper.ToOrgAdminInfoModel(Organizations);
                OrgInfo.IsEditMode = iseditmode;
                return View("OrgInfo", OrgInfo);
            }

            OrgInfo.IsEditMode = iseditmode;
            OrgInfo.IsOrgEnabled = true;
            return View("OrgInfo", OrgInfo);
        }
        [HttpPost]
        public ActionResult OrgInfo(OrgAdminInfoModel OrgAdminInfoModel)
        {
            int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
            int UserHighestRole = int.Parse(Session["UserHighestRole"].ToString());
            string url = "";
            if (this.Request.UrlReferrer == null)
            {
                url = this.Request.Url.ToString();
            }
            else
            {
                url = this.Request.UrlReferrer.ToString();
            }
            //Edit Organization
            if (OrgAdminInfoModel.IsEditMode)
            {

                ModelState.Remove("AdminFirstName");
                ModelState.Remove("AdminLastName");
                ModelState.Remove("ConfirmAdminEmail");
                ModelState.Remove("AdminEmail");



                OrganizationRequest Request = new OrganizationRequest();


                UserDTO AdminInfo = new UserDTO();

                AdminInfo.FirstName = "";
                AdminInfo.LastName = "";
                AdminInfo.EmailAddress = "";
                AdminInfo.Role = 0;
                AdminInfo.PhoneNumber = "";
                Request.OrganizationAdminInfo = AdminInfo;


                Request.Organization.Organization = OrgAdminInfoModel.OrgName;
                Request.Organization.IsEnabled = OrgAdminInfoModel.IsOrgEnabled;

                Request.Organization.OrganizationKey = GetOrgKey(url);
                Request.UserId = UserId;
                Request.UserRole = UserHighestRole;
                Request.Action = "UpDate";
                try
                {
                    OrganizationResponse Result = _isurveyFacade.SetOrganization(Request);
                    OrganizationResponse Organizations = _isurveyFacade.GetUserOrganizations(Request);
                    List<OrganizationModel> Model = Mapper.ToOrganizationModelList(Organizations.OrganizationList);
                    OrgListModel OrgListModel = new OrgListModel();
                    OrgListModel.OrganizationList = Model;
                    OrgListModel.Message = "Organization " + OrgAdminInfoModel.OrgName + " has been updated.";
                    if (Result.Message.ToUpper() != "EXISTS" && Result.Message.ToUpper() != "ERROR")
                    {

                        OrgListModel.Message = "Organization " + OrgAdminInfoModel.OrgName + " has been updated.";
                        return View("OrgList", OrgListModel);
                    }
                    else if (Result.Message.ToUpper() == "ERROR")
                    {
                        OrgAdminInfoModel OrgInfo = new OrgAdminInfoModel();
                        Request.Organization.OrganizationKey = GetOrgKey(url); ;

                        Organizations = _isurveyFacade.GetOrganizationInfo(Request);
                        OrgInfo = Mapper.ToOrgAdminInfoModel(Organizations);
                        OrgInfo.IsEditMode = true;
                        ModelState.AddModelError("IsOrgEnabled", "Organization for the super admin cannot be deactivated.");
                        return View("OrgInfo", OrgInfo);
                    }
                    else
                    {
                        OrgAdminInfoModel OrgInfo = new OrgAdminInfoModel();
                        Request.Organization.OrganizationKey = GetOrgKey(url); ;

                        Organizations = _isurveyFacade.GetOrganizationInfo(Request);
                        OrgInfo = Mapper.ToOrgAdminInfoModel(Organizations);
                        OrgInfo.IsEditMode = true;
                        ModelState.AddModelError("OrgName", "The organization name provided already exists.");
                        return View("OrgInfo", OrgInfo);



                    }

                }
                catch (Exception ex)
                {

                    return View(OrgAdminInfoModel);
                }




            }
            else
            {
                // Add new Organization 

                if (ModelState.IsValid)
                {



                    OrganizationRequest Request = new OrganizationRequest();
                    Request.Organization.Organization = OrgAdminInfoModel.OrgName;
                    Request.Organization.IsEnabled = OrgAdminInfoModel.IsOrgEnabled;
                    UserDTO AdminInfo = new UserDTO();

                    AdminInfo.FirstName = OrgAdminInfoModel.AdminFirstName;
                    AdminInfo.LastName = OrgAdminInfoModel.AdminLastName;
                    AdminInfo.EmailAddress = OrgAdminInfoModel.AdminEmail;
                    AdminInfo.Role = 2;
                    AdminInfo.PhoneNumber = "123456789";
                    Request.OrganizationAdminInfo = AdminInfo;

                    Request.UserRole = UserHighestRole;
                    Request.UserId = UserId;
                    Request.Action = "Insert";
                    try
                    {
                        OrganizationResponse Result = _isurveyFacade.SetOrganization(Request);
                        OrgListModel OrgListModel = new OrgListModel();
                        OrganizationResponse Organizations = _isurveyFacade.GetUserOrganizations(Request);
                        List<OrganizationModel> Model = Mapper.ToOrganizationModelList(Organizations.OrganizationList);
                        OrgListModel.OrganizationList = Model;

                        if (Result.Message.ToUpper() != "EXISTS")
                        {

                            OrgListModel.Message = "Organization " + OrgAdminInfoModel.OrgName + " has been created.";
                        }
                        else
                        {
                            // OrgListModel.Message = "The organization name provided already exists.";
                            OrgAdminInfoModel OrgInfo = new OrgAdminInfoModel();
                            //Request.Organization.OrganizationKey = GetOrgKey(url); ;

                            //Organizations = _isurveyFacade.GetOrganizationInfo(Request);
                            OrgInfo = Mapper.ToOrgAdminInfoModel(Organizations);
                            OrgInfo.IsEditMode = false;
                            ModelState.AddModelError("OrgName", "The organization name provided already exists.");
                            return View("OrgInfo", OrgInfo);
                        }
                        return View("OrgList", OrgListModel);
                    }
                    catch (Exception ex)
                    {

                        return View(OrgAdminInfoModel);
                    }


                }
                else
                {

                    return View(OrgAdminInfoModel);
                }


            }




        }

        private string GetOrgKey(string url)
        {
            var Array = url.Split('/');
            string Orgkey = "";
            Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

            foreach (var item in Array)
            {

                if (guidRegEx.IsMatch(item))
                {
                    Orgkey = item;
                    break;
                }

            }
            return Orgkey;
        }
        [HttpPost]

        public ActionResult AutoComplete(string term)
        {

            var result = new[] { "App", "Bbc", "Cool", "Div", "Enter", "False" };



            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}
