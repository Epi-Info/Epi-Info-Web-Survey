using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
//using BusinessObjects;
//using DataObjects.EntityFramework.ModelMapper;
//using System.Linq.Dynamic;
using Epi.Web.Enter.Interfaces.DataInterfaces;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Criteria;

namespace Epi.Web.EF
{
    /// <summary>
    /// Entity Framework implementation of the IOrganizationDao interface.
    /// </summary>
    public class EntityOrganizationDao : IOrganizationDao
    {
        /// <summary>
        /// Gets a specific Organization.
        /// </summary>
        /// <param name="OrganizationId">Unique Organization identifier.</param>
        /// <returns>Organization.</returns>
        public List<OrganizationBO> GetOrganizationKeys(string OrganizationName)
        {

            List<OrganizationBO> OrganizationBO = new List<OrganizationBO>();
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from response in Context.Organizations
                                where response.Organization1 == OrganizationName
                                select response;

                    var DataRow = Query;
                    foreach (var Row in DataRow)
                    {

                        OrganizationBO.Add(Mapper.Map(Row));

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return OrganizationBO;
        }


        public List<OrganizationBO> GetOrganizationInfo()
        {

            List<OrganizationBO> OrganizationBO = new List<OrganizationBO>();
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = (from response in Context.Organizations

                                 select response);


                    var DataRow = Query.Distinct();
                    foreach (var Row in DataRow)
                    {

                        OrganizationBO.Add(Mapper.Map(Row));

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return OrganizationBO;
        }


        public OrganizationBO GetOrganizationInfoByKey(string key)
        {

            OrganizationBO OrganizationBO = new OrganizationBO();
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = (from response in Context.Organizations
                                 where response.OrganizationKey == key
                                 select response);
                    if (Query.Count() > 0)
                    {
                        OrganizationBO = Mapper.Map(Query.SingleOrDefault());

                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return OrganizationBO;
        }
        public List<OrganizationBO> GetOrganizationNames()
        {

            List<OrganizationBO> OrganizationBO = new List<OrganizationBO>();
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = (from response in Context.Organizations

                                 select response);//{response.Organization1 }).Distinct();


                    var DataRow = Query.Distinct();
                    foreach (var Row in DataRow)
                    {

                        OrganizationBO.Add(Mapper.Map(Row.Organization1));

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return OrganizationBO;
        }


        public List<OrganizationBO> GetOrganizationInfoByOrgKey(string gOrgKeyEncrypted)
        {

            List<OrganizationBO> OrganizationBO = new List<OrganizationBO>();
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = (from response in Context.Organizations
                                 where response.OrganizationKey == gOrgKeyEncrypted && response.IsEnabled == true

                                 select response);


                    var DataRow = Query;
                    foreach (var Row in DataRow)
                    {

                        OrganizationBO.Add(Mapper.Map(Row));

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return OrganizationBO;
        }

        /// <summary>
        /// Inserts a new Organization. 
        /// </summary>
        /// <remarks>
        /// Following insert, Organization object will contain the new identifier.
        /// </remarks>  
        /// <param name="Organization">Organization.</param>
        public void InsertOrganization(OrganizationBO Organization)
        {
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    Organization OrganizationEntity = Mapper.ToEF(Organization);
                    Context.AddToOrganizations(OrganizationEntity);

                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        /// <summary>
        /// Inserts a new Organization. 
        /// </summary>
        /// <remarks>
        /// Following insert, Organization object will contain the new identifier.
        /// </remarks>  
        /// <param name="Organization">Organization.</param>
        public bool InsertOrganization(OrganizationBO Organization, UserBO User)
        {
            try
            {

                UserOrganization UserOrganizationEntity = Mapper.ToUserOrganizationEntity(Organization.IsEnabled, User, Organization);
                using (var Context = DataObjectFactory.CreateContext())
                {

                    Context.AddToUserOrganizations(UserOrganizationEntity);
                    //Context.AddToUsers(User);

                    Context.SaveChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        public bool InsertOrganization(OrganizationBO Organization, int UserId, int RoleId)
        {
            try
            {


                UserOrganization UserOrganizationEntity = Mapper.ToUserOrganizationEntity(Organization.IsEnabled, UserId, RoleId, Organization);
                using (var Context = DataObjectFactory.CreateContext())
                {

                    Context.AddToUserOrganizations(UserOrganizationEntity);

                    Context.SaveChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
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
        /// <summary>
        /// Updates a Organization.
        /// </summary>
        /// <param name="Organization">Organization.</param>
        public bool UpdateOrganization(OrganizationBO Organization)
        {

            ////Update Survey
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from org in Context.Organizations
                                where org.OrganizationKey == Organization.OrganizationKey
                                select org;

                    var SuperAdminChkQry = from usrorg in Context.UserOrganizations
                                            join org in Context.Organizations
                                            on usrorg.OrganizationID equals org.OrganizationId
                                           where org.OrganizationKey == Organization.OrganizationKey && usrorg.RoleId > 2
                                           select usrorg;

                    var DataRow = Query.Single();

                    var SuperAdminRow = SuperAdminChkQry.FirstOrDefault();

                    DataRow.Organization1 = Organization.Organization;

                    if (SuperAdminRow != null && DataRow.IsEnabled != Organization.IsEnabled)
                    {
                        return false;
                    }

                    DataRow.IsEnabled = Organization.IsEnabled;
                    Context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //public UserAuthenticationResponseBO GetAuthenticationResponse(UserAuthenticationRequestBO UserAuthenticationRequestBO)
        //{

        //    UserAuthenticationResponseBO UserAuthenticationResponseBO = Mapper.ToAuthenticationResponseBO(UserAuthenticationRequestBO);
        //    try
        //    {

        //        Guid Id = new Guid(UserAuthenticationRequestBO.ResponseId);


        //        using (var Context = DataObjectFactory.CreateContext())
        //        {
        //            Organization surveyResponse = Context.Organizations.First(x => x.ResponseId == Id);
        //            if (surveyResponse != null)
        //            {
        //                UserAuthenticationResponseBO.PassCode = surveyResponse.ResponsePasscode;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw (ex);
        //    }
        //    return UserAuthenticationResponseBO;

        //}

        /// <summary>
        /// Reads a Organization
        /// </summary>
        /// <param name="Organization">Organization.</param>
        public List<OrganizationBO> GetOrganizationInfoByUserId(int UserId, int UserRole)
        {
            List<OrganizationBO> result = new List<OrganizationBO>();


            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {

                    if (UserRole == 3)
                    {
                        var Query = from OrganizationTable in Context.Organizations

                                    select OrganizationTable;
                        var DataRow = Query.Distinct();
                        foreach (var Row in DataRow)
                        {

                            result.Add(Mapper.Map(Row));

                        }

                        return result;
                    }
                    else
                    {
                        var Query = from OrganizationTable in Context.Organizations
                                    from UserOrganizationTable in Context.UserOrganizations

                                    where UserOrganizationTable.OrganizationID == OrganizationTable.OrganizationId && UserOrganizationTable.UserID == UserId
                                    select OrganizationTable;

                        var DataRow = Query;
                        foreach (var Row in DataRow)
                        {

                            result.Add(Mapper.Map(Row));

                        }

                        return result;

                    }


                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }


        }

        public List<OrganizationBO> GetOrganizationInfoForAdmin(int UserId, int UserRole)
        {
            List<OrganizationBO> result = new List<OrganizationBO>();


            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {

                    if (UserRole == 3)
                    {
                        var Query = from OrganizationTable in Context.Organizations
                                    from UserOrganizationTable in Context.UserOrganizations

                                    where UserOrganizationTable.OrganizationID == OrganizationTable.OrganizationId &&
                                    UserOrganizationTable.UserID == UserId &&
                                    UserOrganizationTable.RoleId >= 2 &&
                                    UserOrganizationTable.Active == true &&
                                    OrganizationTable.IsEnabled == true
                                    select OrganizationTable;

                        var DataRow = Query.Distinct();
                        foreach (var Row in DataRow)
                        {

                            result.Add(Mapper.Map(Row));

                        }

                        return result;
                    }
                    else
                    {
                        var Query = from OrganizationTable in Context.Organizations
                                    from UserOrganizationTable in Context.UserOrganizations

                                    where UserOrganizationTable.OrganizationID == OrganizationTable.OrganizationId &&
                                    UserOrganizationTable.UserID == UserId &&
                                    UserOrganizationTable.RoleId == 2 &&
                                    UserOrganizationTable.Active == true &&
                                    OrganizationTable.IsEnabled == true
                                    select OrganizationTable;

                        var DataRow = Query.Distinct();
                        foreach (var Row in DataRow)
                        {

                            result.Add(Mapper.Map(Row));

                        }

                        return result;

                    }


                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }


        }

        public List<OrganizationBO> GetOrganizationsByUserId(int UserId)
        {
            List<OrganizationBO> result = new List<OrganizationBO>();


            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {


                    var Query = from OrganizationTable in Context.Organizations
                                from UserOrganizationTable in Context.UserOrganizations

                                where UserOrganizationTable.OrganizationID == OrganizationTable.OrganizationId &&
                                UserOrganizationTable.UserID == UserId &&
                                UserOrganizationTable.Active == true &&
                                OrganizationTable.IsEnabled == true
                                select OrganizationTable;

                    var DataRow = Query;
                    foreach (var Row in DataRow)
                    {

                        result.Add(Mapper.Map(Row));

                    }

                    return result;




                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }


        }


        public void DeleteOrganization(OrganizationBO Organization)
        {

            //Delete Survey

        }
        public OrganizationBO GetOrganizationByOrgId(int OrganizationId)
        {
            OrganizationBO OrganizationBO = new OrganizationBO();
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from response in Context.Organizations
                                where response.OrganizationId == OrganizationId
                                select response;

                    if (Query.Count() > 0)
                    {
                        OrganizationBO = Mapper.Map(Query.SingleOrDefault());

                    }
                    else
                    {
                        return null;
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return OrganizationBO;

        }
    }
}
