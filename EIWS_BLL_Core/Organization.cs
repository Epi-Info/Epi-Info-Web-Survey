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

        public OrganizationBO GetOrganizationKey(string OrganizationName)
        {

            OrganizationBO result = this.OrganizationDao.GetOrganizationKey(OrganizationName);
            return result;
        }
        public void InsertOrganizationInfo(OrganizationBO OrganizationBO)
        {

            this.OrganizationDao.InsertOrganization(OrganizationBO);
             
        }
        public void UpdateOrganizationInfo(OrganizationBO OrganizationBO)
        {

            this.OrganizationDao.UpdateOrganization(OrganizationBO);

        }

    }
}
