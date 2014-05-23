﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using System.Configuration;
using Epi.Web.Common.Exception;
namespace Epi.Web.BLL
    {
    public class Account
        {
          private Epi.Web.Interfaces.DataInterfaces.IAdminDao AdminDao;
          private Epi.Web.Interfaces.DataInterfaces.IOrganizationDao OrganizationDao;

          public Account(Epi.Web.Interfaces.DataInterfaces.IAdminDao pAdminDao,Epi.Web.Interfaces.DataInterfaces.IOrganizationDao pOrganizationDao)
            {
               this.AdminDao = pAdminDao;
               this.OrganizationDao = pOrganizationDao;
            }
          private bool AdminEmailExists(string AdminEmail)
              {

              bool AdminExists = false;

            
              AdminBO AdminBOList = this.AdminDao.GetAdminEmailByAdminId(AdminEmail);
              if (!string.IsNullOrEmpty(AdminBOList.AdminEmail))
                  {
                  if (AdminBOList.AdminEmail.ToLower() == AdminEmail.ToLower())
                      {
                      AdminExists = true;
                      }
                  }
             

              return AdminExists;
              }
          private bool OrganizationNameExists(string organizationName, string key, string operation)
              {

              bool orgExists = false;
              key = Epi.Web.Common.Security.Cryptography.Encrypt(key);
              List<OrganizationBO> orgBOList = GetOrganizationNames();
              //first find if the whether the organization name exists in the database
              foreach (OrganizationBO oBo in orgBOList)
                  {
                  if (oBo.Organization.ToLower() == organizationName.ToLower())
                      {
                      orgExists = true;
                      }
                  }

              if (operation == "Update")
                  {
                  //for update if we are updating the organization name to the same value, we should let it pass
                  //so turning the value to false
                  OrganizationBO result = this.OrganizationDao.GetOrganizationInfoByKey(key);
                  if (organizationName.ToLower() == result.Organization.ToLower())
                      {
                      orgExists = false;
                      }
                  }



              return orgExists;
              }
          private List<OrganizationBO> GetOrganizationNames()
              {

              List<OrganizationBO> result = this.OrganizationDao.GetOrganizationNames();
              return result;
              }
          private void InsertOrganizationInfo(OrganizationBO OrganizationBO)
              {
              OrganizationBO.OrganizationKey = Epi.Web.Common.Security.Cryptography.Encrypt(OrganizationBO.OrganizationKey);

              this.OrganizationDao.InsertOrganization(OrganizationBO);

              }
          private OrganizationBO GetOrganizationByKey(string OrganizationKey)
              {
              OrganizationBO result = GetOrganizationObjByKey(OrganizationKey);
              return result;
              }
          private OrganizationBO GetOrganizationObjByKey(string OrganizationKey)
              {
              OrganizationKey = Epi.Web.Common.Security.Cryptography.Encrypt(OrganizationKey);
              OrganizationBO result = this.OrganizationDao.GetOrganizationInfoByKey(OrganizationKey);
              return result;
              }
          private void NotifyAdminAccountCreation(AdminBO AdminBO, OrganizationBO Organization)
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
          private void InsertAdminInfo(AdminBO Admin, OrganizationBO Organization)
              {

              this.AdminDao.InsertAdmin(Admin);
              NotifyAdminAccountCreation(Admin, Organization);
              EmailApplicant(Admin, Organization);
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
          public string CreateAccount(string AccountType, AdminBO AdminBO, OrganizationBO OrganizationBO) 
              {
              bool OrgExists = false;
              bool AdminExists = false;
              string Message = "";
              OrgExists = this.OrganizationNameExists(OrganizationBO.Organization, OrganizationBO.OrganizationKey, "");
              AdminExists = this.AdminEmailExists(AdminBO.AdminEmail.ToString());
              if (AdminExists)
                  {
                  var AdminOrganization = "";
                  
                  }
              var Organization = OrganizationBO;
              var Admin = AdminBO;
                  switch (AccountType.ToUpper())
                        {
                        case "USER":   //The combination of Admin email and organization name should  be unique 
                            try
                                {

                                if (!OrgExists && !AdminExists)
                                    {
                                    InsertOrganizationInfo(Organization);
                                    var OrganizationKey = Epi.Web.Common.Security.Cryptography.Decrypt(Organization.OrganizationKey);
                                    OrganizationBO OrgBO = GetOrganizationByKey(OrganizationKey);

                                    Admin.OrganizationId = OrgBO.OrganizationId;
                                    InsertAdminInfo(Admin, Organization);
                                    Message = "Success";

                                    }
                                else 
                                    {
                                    Message = "Error";
                                    
                                    
                                    }

                               
                                }
                            catch (Exception ex)
                            {
                              Message = "Error";
                               throw ex;
                              }
                            break;
                        case "ORGANIZATION"://Only the organization name should be unique
                            try {
                            if (!OrgExists)
                                {
                                    InsertOrganizationInfo(Organization);
                                    var OrganizationKey = Epi.Web.Common.Security.Cryptography.Decrypt(Organization.OrganizationKey);
                                    OrganizationBO OrgBO = GetOrganizationByKey(OrganizationKey);

                                    Admin.OrganizationId = OrgBO.OrganizationId;
                                    InsertAdminInfo(Admin, Organization);
                                    Message = "Success";

                                }
                            else
                                {
                                Message = "Error";


                                }
                                }
                            catch (Exception ex)
                            {
                            throw ex;
                              }
                            break;
                        }
                  return Message;
              }
        }
    }
