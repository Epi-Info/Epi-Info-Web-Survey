using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
//using BusinessObjects;
//using DataObjects.EntityFramework.ModelMapper;
//using System.Linq.Dynamic;
using Epi.Web.Interfaces.DataInterfaces;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
namespace Epi.Web.EF
    {
    public class EntityAdminDao : IAdminDao
        {
        public List<AdminBO> GetAdminInfoByOrgKey(string gOrgKeyEncrypted) 
            {
            List<AdminBO> AdminList = new List<AdminBO>();
      
            int OrgId = 0;
            int AdminOrgId = 0;
            try
                {
                using (var Context = DataObjectFactory.CreateContext())
                    {
                    var OrgQuery = (from response in Context.Organizations
                                 where response.OrganizationKey == gOrgKeyEncrypted
                                    select new { response.OrganizationId }).First();

                    OrgId = OrgQuery.OrganizationId;

                  

                    var AdminOrgQuery = (from response in Context.Organizations
                                         where response.IsHostOrganization == true
                                         select new { response.OrganizationId }).First();
                    AdminOrgId = AdminOrgQuery.OrganizationId;
                    

                    var AdminQuery = (from response in Context.Admins
                                      where response.OrganizationId == OrgId  && response.IsActive == true && response.Notify == true 
                                 select new { response });
                    var AdminQuery1 = (from response in Context.Admins
                                      where  response.OrganizationId == AdminOrgId && response.IsActive == true && response.Notify == true
                                      select new { response });

                    foreach (var row in AdminQuery)
                        {
                        AdminBO AdminBO = new Common.BusinessObject.AdminBO();
                        AdminBO.AdminEmail = row.response.AdminEmail;
                        AdminBO.IsActive = row.response.IsActive;
                        
                        AdminList.Add(AdminBO);
                        
                        }
                    foreach (var row in AdminQuery1)
                        {
                        AdminBO AdminBO = new Common.BusinessObject.AdminBO();
                        AdminBO.AdminEmail = row.response.AdminEmail;
                        AdminBO.IsActive = row.response.IsActive;

                        AdminList.Add(AdminBO);

                        }
                    }
                }
            catch (Exception ex)
                {
                throw (ex);
                }





            return AdminList;
            }

        public List<AdminBO> GetAdminInfoByOrgId(int OrgId)
            {

            List<AdminBO> AdminList = new List<AdminBO>();
             return AdminList;
            }
        public void InsertAdmin(AdminBO Admin) {



        try
            {
            using (var Context = DataObjectFactory.CreateContext())
                {
                Admin AdminEntity = Mapper.ToEF(Admin);



                Context.Admins.Add(AdminEntity);

                Context.SaveChanges();



                }

            }
        catch (Exception ex)
            {
            throw (ex);
            }



      
            // Get Admin Id 
        try
            {
            


            using (var Context = DataObjectFactory.CreateContext())
                {
                var Query = (from _Admin in Context.Admins
                             where _Admin.AdminEmail == Admin.AdminEmail && _Admin.FirstName == Admin.FirstName && Admin.LastName == _Admin.LastName
                             select new { _Admin.AdminId }).Distinct();


                var DataRow = Query.Distinct();
                foreach (var Row in DataRow)
                    {

                    Admin.AdminId = Row.AdminId;
                    break;
                    }

                }

            }
        catch (Exception ex)
            {
            throw (ex);
            }

        // Insert Address 
        using (var Context = DataObjectFactory.CreateContext())
            {
            Address AddressEntity = Mapper.ToAddressEF(Admin);



            Context.Addresses.Add(AddressEntity);

            Context.SaveChanges();



            }
            
            }

        public void InsertAdminInfo(AdminBO Admin) {

        try
            {
            using (var Context = DataObjectFactory.CreateContext())
                {
                Admin AdminEntity = Mapper.ToEF(Admin);



                Context.Admins.Add(AdminEntity);

                Context.SaveChanges();



                }

            }
        catch (Exception ex)
            {
            throw (ex);
            }
            
            
            }
       public  void UpdateAdmin(AdminBO Admin) { }


        public void DeleteAdmin(AdminBO Admin) { }
        public List<AdminBO> GetAdminEmails()
            {

            List<AdminBO> AdminBO = new List<AdminBO>();
            try
                {
                using (var Context = DataObjectFactory.CreateContext())
                    {
                    var Query = (from response in Context.Admins

                                 select new { response.AdminEmail }).Distinct();


                    var DataRow = Query.Distinct();
                    foreach (var Row in DataRow)
                        {

                        AdminBO.Add(Mapper.MapAdminEmail(Row.AdminEmail));

                        }
                    }
                }
            catch (Exception ex)
                {
                throw (ex);
                }
            return AdminBO;
            }

        public AdminBO GetAdminEmailByAdminId(string AdminEmail)
            {

             AdminBO AdminBO = new AdminBO();
            try
                {
                using (var Context = DataObjectFactory.CreateContext())
                    {
                    var Query = (from response in Context.Admins
                                 where response.AdminEmail == AdminEmail
                                 select new { response.AdminEmail }).Distinct();


                    var DataRow = Query.Distinct();
                    foreach (var Row in DataRow)
                        {

                        AdminBO  = Mapper.MapAdminEmail(Row.AdminEmail) ;

                        }
                    }
                }
            catch (Exception ex)
                {
                throw (ex);
                }
            return AdminBO;
            }

        public string GetAdminOrgKeyByEmail(string UserEmail) {
            var OrgKey = "";
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = (from response in Context.Admins
                                 where response.AdminEmail == UserEmail
                                 select new { response.Organization.OrganizationKey }).Distinct();

                    foreach (var item in Query)
                     {
                           OrgKey = item.OrganizationKey.ToString();
                         break;
                     }
                   
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return OrgKey;
        }
        }
    }
