using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;


namespace Epi.Web.BLL
{

    public class Organization
    {
        private Epi.Web.Interfaces.DataInterfaces.IOrganizationDao OrganizationDao;

        public Organization(Epi.Web.Interfaces.DataInterfaces.IOrganizationDao pOrganizationDao)
        {
            this.OrganizationDao = pOrganizationDao;
        }

        public OrganizationBO GetOrganizationByKey(string OrganizationKey)
        {
            OrganizationKey = Epi.Web.Common.Security.Cryptography.Encrypt(OrganizationKey);
            OrganizationBO result = this.OrganizationDao.GetOrganizationInfoByKey(OrganizationKey);
            return result;
        }
        public List<OrganizationBO> GetOrganizationKey(string OrganizationName)
        {

            List<OrganizationBO> result = this.OrganizationDao.GetOrganizationKeys(OrganizationName);
            foreach (OrganizationBO _result in result)
            {

                _result.OrganizationKey = Epi.Web.Common.Security.Cryptography.Decrypt(_result.OrganizationKey);
            
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
            OrganizationBO.OrganizationKey = Epi.Web.Common.Security.Cryptography.Encrypt(OrganizationBO.OrganizationKey);

            this.OrganizationDao.InsertOrganization(OrganizationBO);
             
        }
        public void UpdateOrganizationInfo(OrganizationBO OrganizationBO)
        {
            OrganizationBO.OrganizationKey = Epi.Web.Common.Security.Cryptography.Encrypt(OrganizationBO.OrganizationKey);
            this.OrganizationDao.UpdateOrganization(OrganizationBO);

        }
        //Validate Admin
        public bool ValidateAdmin(string AdminKey, OrganizationBO OrganizationBO)
        {
            string EncryptedAdminKey = Epi.Web.Common.Security.Cryptography.Decrypt(AdminKey);
           
            bool ISValidUser = false;

            if (!string.IsNullOrEmpty(EncryptedAdminKey) && !string.IsNullOrEmpty(OrganizationBO.AdminId.ToString()))
            {

                if (EncryptedAdminKey == OrganizationBO.AdminId.ToString())
                {
                    ISValidUser = true;


                }
                else
                {
                    ISValidUser = false;
                }
            }
            return ISValidUser;
        }

        //Validate Organization
        public bool ValidateOrganization(string  Key)
        {
            string EncryptedKey = Epi.Web.Common.Security.Cryptography.Encrypt(Key);
            OrganizationBO OrganizationBO =  GetOrganizationByKey(EncryptedKey);
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


    }
}
