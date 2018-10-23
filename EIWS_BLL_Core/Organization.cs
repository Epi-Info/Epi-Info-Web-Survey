using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using System.Configuration;
using Epi.Web.Common.Security;

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
             OrganizationBO result =  GetOrganizationObjByKey(OrganizationKey);
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



        //Validate Organization
        public bool ValidateOrganization(string Key)
        {
            //string EncryptedKey = Epi.Web.Common.Security.Cryptography.Encrypt(Key);
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

        private OrganizationBO GetOrganizationObjByKey(string OrganizationKey)
            {
            OrganizationKey = Epi.Web.Common.Security.Cryptography.Encrypt(OrganizationKey);
            OrganizationBO result = this.OrganizationDao.GetOrganizationInfoByKey(OrganizationKey);
            return result;
            }

        public OrganizationBO GetOrganizationById(int OrganizationId)
        {
             OrganizationBO result = this.OrganizationDao.GetOrganizationInfoById(OrganizationId);
             string orgKey = Cryptography.Decrypt(result.OrganizationKey);
             result.OrganizationKey = orgKey;
             return result;
            //return null;
        }


    }
}
