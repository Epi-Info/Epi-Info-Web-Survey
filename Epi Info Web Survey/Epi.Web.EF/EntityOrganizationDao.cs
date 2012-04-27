﻿using System;
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

           List<OrganizationBO> OrganizationBO = new  List<OrganizationBO>();

           using (var Context = DataObjectFactory.CreateContext())
           {
               var Query = from response in Context.Organizations
                           where response.Organization1 == OrganizationName
                           select response;

               var DataRow = Query;
             foreach(var Row in DataRow)
               {

                 OrganizationBO.Add(Mapper.Map(Row));
               
               }
           }
            return OrganizationBO;
        }


        public List<OrganizationBO> GetOrganizationInfo()
        {

            List<OrganizationBO> OrganizationBO = new List<OrganizationBO>();

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
            return OrganizationBO;
        }


        public  OrganizationBO  GetOrganizationInfoByKey(string key)
        {

             OrganizationBO OrganizationBO = new  OrganizationBO ();

            using (var Context = DataObjectFactory.CreateContext())
            {
                var Query = (from response in Context.Organizations
                             where response.OrganizationKey == key
                             select response);

                OrganizationBO = Mapper.Map(Query.Single());

                 
            }
            return OrganizationBO;
        }
        public List<OrganizationBO> GetOrganizationNames()
        {

            List<OrganizationBO> OrganizationBO = new List<OrganizationBO>();

            using (var Context = DataObjectFactory.CreateContext())
            {
                var Query = (from response in Context.Organizations

                             select new {response.Organization1 }).Distinct();


                var DataRow = Query.Distinct() ;
                foreach (var Row in DataRow)
                {

                    OrganizationBO.Add(Mapper.Map(Row.Organization1));

                }
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
        public  void InsertOrganization(OrganizationBO Organization)
        {
            using (var Context = DataObjectFactory.CreateContext())
            {
                Organization OrganizationEntity = Mapper.ToEF(Organization);
                Context.AddToOrganizations(OrganizationEntity);

                Context.SaveChanges();
            }

             
        }

        /// <summary>
        /// Updates a Organization.
        /// </summary>
        /// <param name="Organization">Organization.</param>
        public void UpdateOrganization(OrganizationBO Organization)
        {
       
        ////Update Survey
            using (var Context = DataObjectFactory.CreateContext())
            {
                var Query = from response in Context.Organizations
                            where response.OrganizationKey == Organization.OrganizationKey
                            select response;

                var DataRow = Query.Single();
                DataRow.Organization1 = Organization.Organization;
              
                DataRow.IsEnabled =  Organization.IsEnabled ;
                Context.SaveChanges();
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
        /// Deletes a Organization
        /// </summary>
        /// <param name="Organization">Organization.</param>
        public void DeleteOrganization(OrganizationBO Organization)
        {

           //Delete Survey
       
       }
       
    }
}
