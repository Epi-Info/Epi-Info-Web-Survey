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
    public class AdminUserController : Controller
    {
        //
        // GET: /Organization/
        private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;

        public AdminUserController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
        [HttpGet]
        public ActionResult UserList()
        {
            int OrgId = -1;
            if (Session["CurrentOrgId"] != null)
            {
                OrgId = int.Parse(Session["CurrentOrgId"].ToString());
            }
            UserOrgModel UserOrgModel = GetUserInfoList(OrgId);
            UserOrgModel.UserHighestRole = int.Parse(Session["UserHighestRole"].ToString());
            if (Session["CurrentOrgId"] == null)
            {
                Session["CurrentOrgId"] = UserOrgModel.OrgList[0].OrganizationId;
            }
            
            return View("UserList", UserOrgModel);
        }


        [HttpGet]
        public ActionResult UserInfo(int userid, bool iseditmode, int orgid)
        {
            UserModel UserModel = new UserModel();
            UserRequest Request = new UserRequest();
            orgid = int.Parse(Session["CurrentOrgId"].ToString());
            if (iseditmode)
            {


                Request.Organization = new OrganizationDTO();
                Request.Organization.OrganizationId = orgid;

                Request.User = new UserDTO();
                Request.User.UserId = userid;

                UserResponse Response = _isurveyFacade.GetUserInfo(Request);
                UserModel = Mapper.ToUserModelR(Response.User[0]);
                UserModel.IsEditMode = true;
                return View("UserInfo", UserModel);
            }

            UserModel.IsActive = true;
            return View("UserInfo", UserModel);
        }
        [HttpPost]
        public ActionResult UserInfo(UserModel UserModel)
        {
            UserOrgModel UserOrgModel = new UserOrgModel();
            UserResponse Response = new UserResponse();
            UserRequest Request = new UserRequest();
            int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
            try
            {
                if (ModelState.IsValid)
                {
                    if (UserModel.IsEditMode)
                    {
                        Request.Action = "UpDate";

                        Request.User = Mapper.ToUserDTO(UserModel);

                        Request.CurrentOrg = int.Parse(Session["CurrentOrgId"].ToString());

                        Request.CurrentUser = UserId;
                        Response = _isurveyFacade.SetUserInfo(Request);
                        UserOrgModel = GetUserInfoList(Request.CurrentOrg);
                        UserOrgModel.Message = "User information for " + UserModel.FirstName + " " + UserModel.LastName + " has been updated. ";
                    }
                    else
                    {
                        Request.Action = "";
                        Request.User = Mapper.ToUserDTO(UserModel);

                        Request.CurrentOrg = int.Parse(Session["CurrentOrgId"].ToString());


                        Request.CurrentUser = UserId;
                        Response = _isurveyFacade.SetUserInfo(Request);
                        UserOrgModel = GetUserInfoList(Request.CurrentOrg);
                        UserOrgModel.Message = "User " + UserModel.FirstName + " " + UserModel.LastName + " has been added. ";
                    }


                }
                else
                {
                    return View("UserInfo", UserModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            UserOrgModel.UserHighestRole = int.Parse(Session["UserHighestRole"].ToString());
            return View("UserList", UserOrgModel);
        }
        [HttpGet]
        public ActionResult GetUserList(int orgid)
        {


            OrganizationRequest Request = new OrganizationRequest();
            Request.Organization.OrganizationId = orgid;
            OrganizationResponse OrganizationUsers = _isurveyFacade.GetOrganizationUsers(Request);
            List<UserModel> UserModel = Mapper.ToUserModelList(OrganizationUsers.OrganizationUsersList);
            ViewBag.SelectedOrg = orgid;
            Session["CurrentOrgId"] = orgid;

            return PartialView("PartialUserList", UserModel);


        }
        private UserOrgModel GetUserInfoList(int OrgId = -1)
        {
            int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
            UserOrgModel UserOrgModel = new UserOrgModel();
            try
            {


                OrganizationRequest Request = new OrganizationRequest();
                Request.UserId = UserId;
                Request.UserRole = Convert.ToInt16(Session["UserHighestRole"].ToString());
                OrganizationResponse Organizations = _isurveyFacade.GetAdminOrganizations(Request);
                List<OrganizationModel> OrgListModel = Mapper.ToOrganizationModelList(Organizations.OrganizationList);
                UserOrgModel.OrgList = OrgListModel;
                if (OrgId != -1)
                {
                    Request.Organization.OrganizationId = OrgId;
                    ViewBag.SelectedOrg = OrgId;
                }
                else
                {
                    Request.Organization.OrganizationId = Organizations.OrganizationList[0].OrganizationId;
                    ViewBag.SelectedOrg = Organizations.OrganizationList[0].OrganizationId;
                }
                OrganizationResponse OrganizationUsers = _isurveyFacade.GetOrganizationUsers(Request);
                List<UserModel> UserModel = Mapper.ToUserModelList(OrganizationUsers.OrganizationUsersList);

                UserOrgModel.UserList = UserModel;

            }
            catch (Exception Ex)
            {
                throw Ex;

            }
            return UserOrgModel;
        }
    }
}
