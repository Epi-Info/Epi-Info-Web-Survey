using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using System.Configuration;
using Epi.Web.Common.Exception;
namespace Epi.Web.BLL
    {
   public class Admin
        {


       public Epi.Web.Interfaces.DataInterfaces.IAdminDao AdminDao;

           public Admin(Epi.Web.Interfaces.DataInterfaces.IAdminDao pAdminDao)
            {
               this.AdminDao = pAdminDao;
            }

            

           public  List<AdminBO> GetAdminInfoByOrgKey(string gOrgKeyEncrypted)
               {
               string OrganizationKey = Epi.Web.Common.Security.Cryptography.Encrypt(gOrgKeyEncrypted);
               List<AdminBO> result = this.AdminDao.GetAdminInfoByOrgKey(OrganizationKey);
               return result;
               }
           public List<AdminBO> GetAdminInfoByOrgId(int OrgId)
               {
               List<AdminBO> result = this.AdminDao.GetAdminInfoByOrgId(OrgId);
               return result;
               }
          
           private List<AdminBO> GetOrganizationAdmins(SurveyInfoBO request)
               {
              
              
               List<AdminBO> AdminBOList = new List<AdminBO>(); 
               try
                   {

                   Epi.Web.Interfaces.DataInterfaces.IAdminDao AdminDao = new EF.EntityAdminDao();
                   Epi.Web.BLL.Admin Implementation = new Epi.Web.BLL.Admin(AdminDao);
                   AdminBOList = Implementation.GetAdminInfoByOrgKey(request.OrganizationKey.ToString());
                  
                 
                }
               catch (Exception ex)
                   {
                   CustomFaultException customFaultException = new CustomFaultException();
                   customFaultException.CustomMessage = ex.Message;
                   customFaultException.Source = ex.Source;
                   customFaultException.StackTrace = ex.StackTrace;
                   customFaultException.HelpLink = ex.HelpLink;
                    
                 }
               return AdminBOList;
               }

           public void SendEmailToAdmins(SurveyInfoBO SurveyInfo)
               {

                List<AdminBO> AdminBOList = new  List<AdminBO>();
               List<string> AdminList = new List<string>();
              
               AdminBOList = GetOrganizationAdmins(SurveyInfo);
               foreach (var item in AdminBOList)
                   {
                   AdminList.Add(item.AdminEmail);
                   }

               Epi.Web.Common.Email.Email Email = new Web.Common.Email.Email();
               Email.Body = "The following survey has been promoted to FINAL mode:\n Title:" + SurveyInfo.SurveyName + " \n Survey ID:" + SurveyInfo.SurveyId + " \nOrganization:" + SurveyInfo.OrganizationName + "\n Start Date & Time:" + SurveyInfo.StartDate + "\n Closing Date & Time:" + SurveyInfo.ClosingDate + " \n \n \n  Thank you.";
               Email.From = ConfigurationManager.AppSettings["EMAIL_FROM"];
               Email.To = AdminList;
               Email.Subject = "Survey -" + SurveyInfo.SurveyName + " has been promoted to FINAL";
               bool success = Epi.Web.Common.Email.EmailHandler.SendMessage(Email);

               }

           public void InsertAdminInfo(AdminBO Admin, OrganizationBO Organization)
               {
               
                 this.AdminDao.InsertAdmin(Admin);
                 NotifyAdminAccountCreation(Admin, Organization);
                 EmailApplicant(Admin, Organization);
               }
           private void NotifyAdminAccountCreation(AdminBO AdminBO , OrganizationBO Organization)
               {
               string NotifyAdmin = ConfigurationManager.AppSettings["NOTIFY_ADMIN_IS_ENABLED"];
               string AdminEmail = ConfigurationManager.AppSettings["LOGGING_ADMIN_EMAIL_ADDRESS"];

               string strOrgKeyDecrypted = Epi.Web.Common.Security.Cryptography.Decrypt(Organization.OrganizationKey.ToString());
              // List<AdminBO> AdminBOList = new List<AdminBO>();
               List<string> AdminList = new List<string>();
               if (NotifyAdmin.ToUpper() == "TRUE")
                   {
                   AdminList.Add(AdminEmail);
                   }
             
               //AdminBOList = GetOrganizationAdmins(SurveyInfo);
               //foreach (var item in AdminBOList)
               //    {
               //  AdminList.Add(item.AdminEmail);
               //    }

               Epi.Web.Common.Email.Email Email = new Web.Common.Email.Email();
               Email.Body = "Organization Name:" + Organization.Organization + "\nOrganization Key: " + strOrgKeyDecrypted + "\nAdmin Email: " + AdminBO.AdminEmail + "\n\nThank you.";
               Email.From = ConfigurationManager.AppSettings["EMAIL_FROM"];
               Email.To = AdminList;
               Email.Subject = "An organization account has been created.";
               if (AdminList.Count() > 0)
                   {
                     bool success = Epi.Web.Common.Email.EmailHandler.SendMessage(Email);
                   }
               }
           private void EmailApplicant(AdminBO AdminBO, OrganizationBO Organization)
               {
              
               string AdminEmail = ConfigurationManager.AppSettings["LOGGING_ADMIN_EMAIL_ADDRESS"];
               string ApplicantValidation = ConfigurationManager.AppSettings["APPLICANT_VALIDATION_IS_ENABLED"];
               List<string> AdminList = new List<string>();
               string strOrgKeyDecrypted = Epi.Web.Common.Security.Cryptography.Decrypt(Organization.OrganizationKey.ToString());
               if (ApplicantValidation.ToUpper() == "FALSE")
                   {
                   AdminList.Add(AdminBO.AdminEmail);
                   }

               StringBuilder Body = new StringBuilder();
               Body.Append("Organization Name:" + Organization.Organization + "\nOrganization Key: " + strOrgKeyDecrypted);
               Body.Append("\n\nPlease follow the steps below in order to start publishing  forms to the web.");
               Body.Append("\n\tStep1:Open Epi Info 7");
               Body.Append("\n\tStep2:On the Main Menu, click on “Tools” and select “Options”");
               Body.Append("\n\tStep3:On the Options dialog, click on the “Web Survey” Tab.");
               Body.Append("\n\tStep4:On the Web Survey tab, enter the following information.");

               Body.Append("\n\t\t-Endpoint Address:" + ConfigurationManager.AppSettings["ENDPOINT_ADDRESS"] + "\n\t\t-Connect using Windows Authentication:  " + ConfigurationManager.AppSettings["WINDOW_AUTHENTICATION"]);
               Body.Append("\n\t\t-Binding Protocol:" + ConfigurationManager.AppSettings["BINDING_PROTOCOL"]);

               Body.Append("\n\tStep5:Click “OK’ button.");
               Body.Append("\n\nPlease contact your system administrator " + AdminEmail + " in case of any questions.");
               Body.Append("\n\nThank you.");
               
               Epi.Web.Common.Email.Email Email = new Web.Common.Email.Email();
               Email.Body = Body.ToString();
               Email.From = ConfigurationManager.AppSettings["EMAIL_FROM"];
               Email.To = AdminList;
               Email.Subject = "An account for your organization has been created. Please refer to details below.";

               if (AdminList.Count() > 0)
                   {
                   bool success = Epi.Web.Common.Email.EmailHandler.SendMessage(Email);
                   }
               }
           public List<AdminBO> GetAdminEmails()
               {
               
               List<AdminBO> result = this.AdminDao.GetAdminEmails();
               return result;
               }
           public bool AdminEmailExists(string AdminEmail)
               {

               bool AdminExists = false;

               List<AdminBO> AdminBOList = GetAdminEmails(); 
               //first find if the whether the Admin name exists in the database
               foreach (AdminBO Admin in AdminBOList)
                   {
                   if (Admin.AdminEmail.ToLower() == AdminEmail.ToLower())
                       {
                       AdminExists = true;
                       }
                   }
 
               return AdminExists;
               }
        }
    }
