using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Criteria;
using System.Configuration;
using Epi.Web.Enter.Common.Constants;
using Epi.Web.Enter.Common.Email;
using Epi.Web.Enter.Common.Security;

namespace Epi.Web.BLL
{

    public class Organization
    {
        private Epi.Web.Enter.Interfaces.DataInterfaces.IOrganizationDao OrganizationDao;

        public Organization(Epi.Web.Enter.Interfaces.DataInterfaces.IOrganizationDao pOrganizationDao)
        {
            this.OrganizationDao = pOrganizationDao;
        }

        public OrganizationBO GetOrganizationByKey(string OrganizationKey)
        {
            OrganizationBO result = GetOrganizationObjByKey(OrganizationKey);
            return result;
        }
        public OrganizationBO GetOrganizationByOrgId(int OrganizationId)
        {
            OrganizationBO result = this.OrganizationDao.GetOrganizationByOrgId(OrganizationId); ;
            return result;
        }


        public List<OrganizationBO> GetOrganizationKey(string OrganizationName)
        {

            List<OrganizationBO> result = this.OrganizationDao.GetOrganizationKeys(OrganizationName);
            foreach (OrganizationBO _result in result)
            {

                _result.OrganizationKey = Epi.Web.Enter.Common.Security.Cryptography.Decrypt(_result.OrganizationKey);

            }

            return result;
        }
        public List<OrganizationBO> GetOrganizationInfo()
        {

            List<OrganizationBO> result = this.OrganizationDao.GetOrganizationInfo();
            return result;
        }
        public List<OrganizationBO> GetOrganizationNames()
        {

            List<OrganizationBO> result = this.OrganizationDao.GetOrganizationNames();
            return result;
        }
        public void InsertOrganizationInfo(OrganizationBO OrganizationBO)
        {
            OrganizationBO.OrganizationKey = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(OrganizationBO.OrganizationKey);

            this.OrganizationDao.InsertOrganization(OrganizationBO);

        }

        enum InsertCombination
        {
            None = 0,
            NewUserNewOrg = 1,
            ExistingUserNewOrg = 2
        }
        public void InsertOrganizationInfo(OrganizationBO OrganizationBO, UserBO UserBO)
        {
            bool success;
            OrganizationBO.OrganizationKey = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(OrganizationBO.OrganizationKey);
            InsertCombination InsertStatus = new InsertCombination();
            // Check if the user Exists
            var User = this.OrganizationDao.GetUserByEmail(UserBO);
            string tempPassword = string.Empty;
            if (User != null)
            {
                if (string.IsNullOrEmpty(User.EmailAddress))
                {
                    UserBO.ResetPassword = true;
                    success = this.OrganizationDao.InsertOrganization(OrganizationBO, UserBO);
                }

                else
                {
                    success = this.OrganizationDao.InsertOrganization(OrganizationBO, User.UserId, UserBO.Role);

                }
                if (success)
                {
                    InsertStatus = InsertCombination.ExistingUserNewOrg;
                }
            }
            else
            {
                string KeyForUserPasswordSalt = ConfigurationManager.AppSettings["KeyForUserPasswordSalt"];
                PasswordHasher PasswordHasher = new Web.Enter.Common.Security.PasswordHasher(KeyForUserPasswordSalt);
                string salt = PasswordHasher.CreateSalt(UserBO.EmailAddress);
                UserBO.ResetPassword = true;
                PasswordGenerator PassGen = new PasswordGenerator();
                tempPassword = PassGen.Generate();
                UserBO.PasswordHash = PasswordHasher.HashPassword(salt, tempPassword);// "PassWord1");

                success = this.OrganizationDao.InsertOrganization(OrganizationBO, UserBO);
                if (success)
                {
                    InsertStatus = InsertCombination.NewUserNewOrg;
                }

            }
            var OrgKey = Epi.Web.Enter.Common.Security.Cryptography.Decrypt(OrganizationBO.OrganizationKey);
            if (success && InsertStatus != InsertCombination.None)
            {
                Email email = new Email();

                StringBuilder Body = new StringBuilder();
                if (InsertStatus == InsertCombination.ExistingUserNewOrg)
                {
                    Body.Append("Your account has now been created for organization - " + OrganizationBO.Organization + ".\n");
                    Body.Append("\nOrganization Key: " + OrgKey);
                    Body.Append("\n\nPlease click the link below to launch Epi Info™ Web Enter. \n" + ConfigurationManager.AppSettings["BaseURL"] + "\n\nThank you.");
                }
                else
                {
                    Body.Append("Welcome to Epi Info™ Web Enter. \nYour account has now been created for oganization - " + OrganizationBO.Organization + ".");
                    Body.Append("\n\nEmail: " + UserBO.EmailAddress + "\nPassword: " + tempPassword);
                    Body.Append("\nOrganization Key: " + OrgKey);
                    Body.Append("\n\nPlease click the link below to launch the Epi Info™ Web Enter and log in with your email and temporary password. You will then be asked to create a new password. \n" + ConfigurationManager.AppSettings["BaseURL"]);
                    //Add email and temporary password for new user. 
                }

                //Body.Append("\n" + ConfigurationManager.AppSettings["BaseURL"]);

                if (InsertStatus == InsertCombination.NewUserNewOrg)
                {
                    Body.Append("\n\nPlease follow the steps below in order to start publishing forms to the web using Epi Info™ 7.");
                    Body.Append("\n\tStep 1: Download and install the latest version of Epi Info™ 7 from:" + ConfigurationManager.AppSettings["EPI_INFO_DOWNLOAD_URL"]);
                    Body.Append("\n\tStep 2: On the Main Menu, click on “Tools” and select “Options”");
                    Body.Append("\n\tStep 3: On the Options dialog, click on the “Web Enter” Tab.");
                    Body.Append("\n\tStep 4: On the Web Enter tab, enter the following information.");

                    Body.Append("\n\t\t-Endpoint Address:" + ConfigurationManager.AppSettings["ENDPOINT_ADDRESS"] + "\n\t\t-Connect using Windows Authentication:  " + ConfigurationManager.AppSettings["WINDOW_AUTHENTICATION"]);
                    Body.Append("\n\t\t-Binding Protocol:" + ConfigurationManager.AppSettings["BINDING_PROTOCOL"]);

                    Body.Append("\n\tStep 5:Click “OK’ button.");
                    Body.Append("\nOrganization key provided here is to be used in Epi Info™ 7 during publish process.");
                    Body.Append("\n\nPlease contact the system administrator for any questions.");
                }


                email.Body = Body.ToString();
                email.To = new List<string>();
                email.To.Add(UserBO.EmailAddress);

                success = SendEmail(email, Constant.EmailCombinationEnum.InsertOrganization);


            }
        }
        public bool UpdateOrganizationInfo(OrganizationBO OrganizationBO)
        {
            OrganizationBO.OrganizationKey = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(OrganizationBO.OrganizationKey);
            return this.OrganizationDao.UpdateOrganization(OrganizationBO);

        }



        //Validate Organization
        public bool ValidateOrganization(string Key)
        {
            //string EncryptedKey = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(Key);
            OrganizationBO OrganizationBO = GetOrganizationObjByKey(Key);
            bool ISValidOrg = false;

            if (OrganizationBO != null)
            {
                ISValidOrg = true;


            }
            else
            {
                ISValidOrg = false;
            }

            return ISValidOrg;
        }

        /// <summary>
        /// Checks whether a particular organization name already exists in database
        /// </summary>
        /// <param name="key"></param>
        /// <param name="organizationName"></param>
        /// <returns></returns>
        public bool OrganizationNameExists(string organizationName, string key, string operation)
        {

            bool orgExists = false;
            key = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(key);
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

        private OrganizationBO GetOrganizationObjByKey(string OrganizationKey)
        {
            OrganizationKey = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(OrganizationKey);
            OrganizationBO result = this.OrganizationDao.GetOrganizationInfoByKey(OrganizationKey);
            return result;
        }



        public List<OrganizationBO> GetOrganizationsByUserId(int UserId)
        {

            List<OrganizationBO> result = this.OrganizationDao.GetOrganizationsByUserId(UserId);
            return result;
        }

        public List<OrganizationBO> GetOrganizationInfoByUserId(int UserId, int UserRole)
        {

            List<OrganizationBO> result = this.OrganizationDao.GetOrganizationInfoByUserId(UserId, UserRole);
            return result;
        }

        public List<OrganizationBO> GetOrganizationInfoForAdmin(int UserId, int UserRole)
        {

            List<OrganizationBO> result = this.OrganizationDao.GetOrganizationInfoForAdmin(UserId, UserRole);
            return result;
        }
        private bool SendEmail(Email email, Constant.EmailCombinationEnum Combination)
        {



            switch (Combination)
            {

                case Constant.EmailCombinationEnum.InsertOrganization:
                    email.Subject = "An Epi Info Web Enter account has been created for your organization.";

                    break;
                default:
                    break;
            }

            email.Body = email.Body.ToString();
            //+" \n \nPlease click the link below to launch Epi Web Enter. \n" + ConfigurationManager.AppSettings["BaseURL"] + "\n\nThank you."; //email.Body.ToString() + " \n \n" + ConfigurationManager.AppSettings["BaseURL"];
            email.From = ConfigurationManager.AppSettings["EMAIL_FROM"];

            return Epi.Web.Enter.Common.Email.EmailHandler.SendMessage(email);

        }
    }
}
