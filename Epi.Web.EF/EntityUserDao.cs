using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.Constants;
using Epi.Web.Interfaces.DataInterfaces;

namespace Epi.Web.EF
{
    public class EntityUserDao : IUserDao
    {
        //public UserBO GetUser(UserBO User)
        //{
        //    var Context = DataObjectFactory.CreateContext();
        //    var UserQuery = from user in Context.Users
        //                    where user.UserName == User.UserName && user.PasswordHash == User.PasswordHash
        //                    select user;
        //    UserBO Result = new UserBO();

        //    foreach (var user in UserQuery)
        //    {
        //        Result = Mapper.MapToUserBO(user);
        //        return Result;
        //    }

        //    return null;
        //}

        public bool GetExistingUser(UserBO User)
        {
            var Context = DataObjectFactory.CreateContext();
            var UserQuery = from user in Context.Users
                            where user.UserName == User.UserName && user.LastName == User.LastName && user.EmailAddress == User.EmailAddress
                            select user;
            bool Result = false;

            foreach (var user in UserQuery)
            {
                Result = true;
            }

            return Result;
        }
        public bool IsUserActive(int UserID, int OrganizationId)
        {
          
            using (var Context = DataObjectFactory.CreateContext())
            {
                var _User = Context.UserOrganizations.Where(x => x.UserID == UserID && x.OrganizationID == OrganizationId).Single();

                return _User.Active;
            }
           
        
         }
        //public bool UpdateUser(UserBO User)
        //{
        //var Context = DataObjectFactory.CreateContext();
        //switch (User.Operation)
        //{
        //    case Constant.OperationMode.UpdatePassword:
        //        var user = Context.Users.Single(a => a.UserName == User.UserName);
        //        user.PasswordHash = User.PasswordHash;
        //        Context.SaveChanges();
        //        return true;
        //    case Constant.OperationMode.UpdateUserInfo:
        //        break;
        //    case Constant.OperationMode.UpdateUser:
        //        break;
        //    default:
        //        break;
        //}
        //return false;
        //    throw new NotImplementedException();
        //}

        //public bool DeleteUser(UserBO User)
        //{
        //    throw new NotImplementedException();
        //}

        public bool InsertUser(UserBO User, OrganizationBO OrgBO)
        {
            try
            {
                var _user = GetUserByEmail(User);
              
               
                using (var Context = DataObjectFactory.CreateContext())
                {
                     
                        var Org = Context.Organizations.Where(x => x.OrganizationId == OrgBO.OrganizationId).Single();

                        Context.Organizations.Attach(Org);
                    if (_user == null)
                    {
                        User.UGuid = Guid.NewGuid();
                        Context.Users.Add(Mapper.ToUserEntity(User));
                        
                        UserOrganization UserOrganizationEntity = Mapper.ToUserOrganizationEntity(User, OrgBO);
                        Context.UserOrganizations.Add(UserOrganizationEntity);
                    }
                    else
                    {
                        _user.Role = User.Role;
                        UserOrganization UserOrganizationEntity = Mapper.ToUserOrganizationEntity(_user, OrgBO);
                        //UserOrganizationEntity.Role = 1;
                       
                        Context.UserOrganizations.Add(UserOrganizationEntity);
                    }


                        Context.SaveChanges();
                 
                }
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public UserBO GetUserByUserId(UserBO User)
        {
            var Context = DataObjectFactory.CreateContext();
            var UserQuery = from Users in Context.Users
                            where Users.UserID == User.UserId
                            select Users;
            UserBO Result = new UserBO();

            foreach (var user in UserQuery)
            {
             
                Result = Mapper.MapToUserBO(user);
                
            }

            return Result;
        }
        //public UserBO GetUserByUserIdAndOrgId(UserBO User, OrganizationBO OrgBO)
        //{

        //    var Context = DataObjectFactory.CreateContext();
        //    var UserQuery = from Users in Context.Users
        //                    where Users.UserID == User.UserId
        //                    select Users;
        //    UserBO Result = new UserBO();

        //    foreach (var user in UserQuery)
        //    {
        //        Result = Mapper.MapToUserBO(user);

        //        var _User = Context.UserOrganizations.Where(x => x.UserID == user.UserID && x.OrganizationID == OrgBO.OrganizationId).Single();
        //        Result.IsActive = _User.Active;
        //        Result.Role = _User.RoleId;

        //    }
        //    return Result;


        //}
        //public UserBO GetCurrentUser(int UserId)
        //{
        //    UserBO Result = new UserBO();
        //    using (var Context = DataObjectFactory.CreateContext())
        //    {

        //        Result = Mapper.MapToUserBO(Context.Users.Single(x => x.UserID == UserId));


        //    }



        //    return Result;
        //}

        //public bool UpdateUserPassword(UserBO User)
        //{
        //    try
        //    {
        //        var Context = DataObjectFactory.CreateContext();
        //        var user = Context.Users.Single(a => a.UserName == User.UserName);
        //        user.PasswordHash = User.PasswordHash;
        //        user.ResetPassword = User.ResetPassword;
        //        Context.SaveChanges();
        //        return true;

        //    }
        //    catch (Exception ex )
        //    {

        //        return false;
        //    }
        //}

        public bool UpdateUserInfo(UserBO User, OrganizationBO OrgBO)
        {

            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {


                    User user = Context.Users.First(x => x.UserID == User.UserId);
                    //user.UserName = User.UserName;
                    user.EmailAddress = User.EmailAddress;
                    user.FirstName = User.FirstName;
                    user.LastName = User.LastName;
                    user.PhoneNumber = User.PhoneNumber;
                    user.UserName = User.EmailAddress.Split('@')[0];
                    UserOrganization UserOrganization = Context.UserOrganizations.First(x => x.OrganizationID == OrgBO.OrganizationId && x.UserID == User.UserId);
                    //UserOrganization.RoleId = User.Role;
                    UserOrganization.Active = User.IsActive;

                    Context.SaveChanges();



                }
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        //public List<UserBO> GetUserByFormId(string FormId)
        //{
        //    Guid id = new Guid(FormId);
        //    List<UserBO> UserList = new List<UserBO>();
        //    UserBO UserBO = new UserBO();
        //    using (var Context = DataObjectFactory.CreateContext())
        //    {
        //        SurveyMetaData SelectedUserQuery = Context.SurveyMetaDatas.First(x => x.SurveyId == id);

        //        IQueryable<User> Users = SelectedUserQuery.Users.AsQueryable();
        //        foreach (User user in Users)
        //        {

        //            UserList.Add(Mapper.MapToUserBO(user));
        //        }
        //    }
        //    return UserList;

        //}

        public UserBO GetUserByEmail(UserBO User)
        {
            var Context = DataObjectFactory.CreateContext();
            var UserQuery = from Users in Context.Users
                            where Users.EmailAddress == User.EmailAddress
                            select Users;
            UserBO Result = new UserBO();

            foreach (var user in UserQuery)
            {
                Result = Mapper.MapToUserBO(user);
                return Result;
            }

            return null;
        }

        public List<UserBO> GetUserByOrgId(int OrgId)
        {

            List<UserBO> UserList = new List<UserBO>();
            using (var Context = DataObjectFactory.CreateContext())
            {
                var SelectedUserQuery = from UserOrg in Context.UserOrganizations
                                        join user in Context.Users on UserOrg.UserID equals user.UserID
                                        where UserOrg.OrganizationID == OrgId
                                        select user;
                foreach (var user in SelectedUserQuery)
                {
                    var UserBO = Mapper.MapToUserBO(user);
                    var User = Context.UserOrganizations.Where(x => x.UserID == user.UserID && x.OrganizationID == OrgId).Single();
                    UserBO.IsActive = User.Active;
                    UserBO.Role = User.RoleId;
                    UserList.Add(UserBO);
                }
            }



            return UserList;

        }

        //public int GetUserHighestRole(int UserId)
        //{
        //    int HighestRole = 0;

        //    try
        //    {
        //        using (var Context = DataObjectFactory.CreateContext())
        //        {


        //            var Rows = Context.UserOrganizations.Where(x => x.UserID == UserId && x.Active == true && x.Organization.IsEnabled == true);
        //            HighestRole = Rows.Max(x => x.RoleId);


        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }


        //    return HighestRole;


        //}




        //public bool UpdateUserOrganization(UserBO User, OrganizationBO OrgBO)
        //{
        //    UserOrganization UserOrganizationEntity = Mapper.ToUserOrganizationEntity(User, OrgBO);
        //    using (var Context = DataObjectFactory.CreateContext())
        //    {
        //        UserOrganizationEntity.UserID = User.UserId;
        //        Context.AddToUserOrganizations(UserOrganizationEntity);
        //        Context.SaveChanges();

        //        return true;
        //    }
        //}


        //public bool IsUserExistsInOrganizaion(UserBO User, OrganizationBO OrgBO)
        //{
        //    var Context = DataObjectFactory.CreateContext();
        //    var UserQuery = from userorganization in Context.UserOrganizations
        //                    where userorganization.UserID == User.UserId && userorganization.OrganizationID == OrgBO.OrganizationId
        //                    select userorganization;
        //    bool Result = false;

        //    foreach (var user in UserQuery)
        //    {
        //        Result = true;
        //    }

        //    return Result;
        //}

        //public List<UserBO> GetAdminsBySelectedOrgs(FormSettingBO FormSettingBO, string formId)
        //   {

        //      List<UserBO> AdminList = new List<UserBO>();
        //      using (var Context = DataObjectFactory.CreateContext())
        //      {
        //          foreach (var item in FormSettingBO.SelectedOrgList)
        //          {
        //              int OrgId = int.Parse(item.Value);

        //              var users = Context.UserOrganizations.Where(x => x.RoleId == 2 && x.OrganizationID == OrgId && x.Active == true);

        //              foreach (var user in users)
        //              {
        //                  AdminList.Add(Mapper.MapToUserBO(user.User, user.RoleId));
        //              }

        //          }
        //      }
        //          return AdminList;
        //     }

    }
}
