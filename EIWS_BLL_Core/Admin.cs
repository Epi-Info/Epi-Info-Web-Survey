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
        }
    }
