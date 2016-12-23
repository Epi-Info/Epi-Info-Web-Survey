using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
namespace Epi.Web.Interfaces.DataInterfaces
    {
    public interface IAdminDao
        {

      
        List<AdminBO> GetAdminInfoByOrgKey(string gOrgKeyEncrypted);
        List<AdminBO> GetAdminInfoByOrgId(int OrgId);
        List<AdminBO> GetAdminEmails();
        void InsertAdmin(AdminBO Admin);

         
        void UpdateAdmin(AdminBO Admin);

         
        void DeleteAdmin(AdminBO Admin);

        AdminBO  GetAdminEmailByAdminId(string AdminEmail);

        void InsertAdminInfo(AdminBO Admin);

        string GetAdminOrgKeyByEmail(string UserEmail);
        }
    }
