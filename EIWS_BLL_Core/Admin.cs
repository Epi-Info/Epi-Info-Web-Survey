using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using System.Configuration;
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
        }
    }
