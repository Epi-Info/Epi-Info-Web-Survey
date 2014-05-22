﻿using System;
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

        void InsertAdmin(AdminBO Admin);

         
        void UpdateAdmin(AdminBO Admin);

         
        void DeleteAdmin(AdminBO Admin);

        }
    }
