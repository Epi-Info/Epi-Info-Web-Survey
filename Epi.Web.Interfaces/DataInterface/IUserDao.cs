using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;

namespace Epi.Web.Interfaces.DataInterfaces
{
    public interface IUserDao
    {
       // UserBO GetUser(UserBO User);
        bool GetExistingUser(UserBO User);
        //bool UpdateUser(UserBO User);
        //bool DeleteUser(UserBO User);
        bool InsertUser(UserBO User, OrganizationBO OrgBO);
        //bool UpdateUserOrganization(UserBO User, OrganizationBO OrgBO);
        UserBO GetUserByUserId(UserBO User);
        //bool UpdateUserPassword(UserBO User);
        bool UpdateUserInfo(UserBO User, OrganizationBO OrgBO);
        //List<UserBO> GetUserByFormId(string FormId);
        //UserBO GetCurrentUser(int userId);
        UserBO GetUserByEmail(UserBO User);
        //bool IsUserExistsInOrganizaion(UserBO User, OrganizationBO OrgBO);
        List<UserBO> GetUserByOrgId(int OrgId);
        bool IsUserActive(int UserId, int OrgId);
        //UserBO GetUserByUserIdAndOrgId(UserBO UserBO, OrganizationBO OrgBO);

        //int GetUserHighestRole(int UserId);

      //  List<UserBO> GetAdminsBySelectedOrgs(FormSettingBO FormSettingBO, string p);
    }
}
