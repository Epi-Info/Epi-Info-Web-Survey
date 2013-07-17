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
         
            try
                {
                using (var Context = DataObjectFactory.CreateContext())
                    {
                    var OrgQuery = (from response in Context.Organizations
                                 where response.OrganizationKey == gOrgKeyEncrypted
                                 select new { response.OrganizationId }).Distinct();


                    var DataRow = OrgQuery.Distinct();
                    foreach (var Row in DataRow)
                        {

                       
                        OrgId = Row.OrganizationId;
                        
                        }

                    var AdminQuery = (from response in Context.Admins
                                      where response.OrganizationId == OrgId
                                 select new { response });

                    foreach (var row in AdminQuery)
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
        public void InsertAdmin(AdminBO Admin) { }


       public  void UpdateAdmin(AdminBO Admin) { }


        public void DeleteAdmin(AdminBO Admin) { }

        }
    }
