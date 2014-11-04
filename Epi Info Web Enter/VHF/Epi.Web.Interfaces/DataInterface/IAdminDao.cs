using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
namespace Epi.Web.Enter.Interfaces.DataInterfaces
    {
    public interface IAdminDao
        {

      
        List<AdminBO> GetAdminInfoByOrgKey(string gOrgKeyEncrypted);
        List<AdminBO> GetAdminInfoByOrgId(int OrgId);

        void InsertAdmin(AdminBO Admin);

         
        void UpdateAdmin(AdminBO Admin);

         
        void DeleteAdmin(AdminBO Admin);

        }
    }
