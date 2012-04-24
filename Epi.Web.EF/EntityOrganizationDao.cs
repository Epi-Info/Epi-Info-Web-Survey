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
        public List<OrganizationBO> GetOrganization(List<string> OrganizationIdList, Guid UserPublishKey)
        {

            List<OrganizationBO> result = new List<OrganizationBO>();

            if (OrganizationIdList.Count > 0)
            {
                foreach (string surveyResponseId in OrganizationIdList.Distinct())
                {
                    Guid Id = new Guid(surveyResponseId);

                    using (var Context = DataObjectFactory.CreateContext())
                    {

                        result.Add(Mapper.Map(Context.Organizations.FirstOrDefault(x => x.ResponseId == Id )));
                    }
                }
            }
            else
            {
                using (var Context = DataObjectFactory.CreateContext())
                {

                    result = Mapper.Map(Context.Organizations.ToList());
                }
            }

            return result;
        }


        /// <summary>
        /// Gets Organizations per a SurveyId.
        /// </summary>
        /// <param name="OrganizationId">Unique Organization identifier.</param>
        /// <returns>Organization.</returns>
        public List<OrganizationBO> GetOrganizationBySurveyId(List<string> SurveyIdList, Guid UserPublishKey)
        {

            List<OrganizationBO> result = new List<OrganizationBO>();

            foreach (string surveyResponseId in SurveyIdList.Distinct())
            {
                Guid Id = new Guid(surveyResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {

                    result.Add(Mapper.Map(Context.Organizations.FirstOrDefault(x => x.SurveyId == Id )));
                }
            }

            return result;
        }


        /// <summary>
        /// Gets Organizations depending on criteria.
        /// </summary>
        /// <param name="OrganizationId">Unique Organization identifier.</param>
        /// <returns>Organization.</returns>
        public List<OrganizationBO> GetOrganization(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId = -1 )
        {
            List<OrganizationBO> result = new List<OrganizationBO>();
            List<Organization> responseList = new List<Organization>();

            if (SurveyAnswerIdList.Count > 0)
            {
                foreach (string surveyResponseId in SurveyAnswerIdList.Distinct())
                {
                    try
                    {
                        Guid Id = new Guid(surveyResponseId);
                       

                        using (var Context = DataObjectFactory.CreateContext())
                        {
                            Organization surveyResponse = Context.Organizations.First(x => x.ResponseId == Id );
                            if (surveyResponse != null)
                            {
                                responseList.Add(surveyResponse);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // do nothing for now
                    }
                }
            }
            else
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    responseList = Context.Organizations.ToList();
                }
            }


            if(! string.IsNullOrEmpty(pSurveyId))
            {
                Guid Id = new Guid(pSurveyId);
                List<Organization> surveyList = new List<Organization>();
                surveyList.AddRange(responseList.Where(x => x.SurveyId == Id));
                responseList = surveyList;
            }

            if (pStatusId > -1)
            {
                List<Organization> statusList = new List<Organization>();
                statusList.AddRange(responseList.Where(x => x.StatusId == pStatusId));
                responseList = statusList;
            }

            if (pDateCompleted > DateTime.MinValue)
            {
                List<Organization> dateList = new List<Organization>();

                dateList.AddRange(responseList.Where(x => x.DateCompleted.Value.Month ==  pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
                responseList = dateList;
            }

            result = Mapper.Map(responseList);
            return result;
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
            using (var Context = DataObjectFactory.CreateContext() ) 
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
            Guid Id = new Guid(Organization.ResponseId);

        //Update Survey
            using (var Context = DataObjectFactory.CreateContext())
            {
                var Query = from response in Context.Organizations
                            where response.ResponseId == Id 
                            select response;

                var DataRow = Query.Single();

              

                DataRow.ResponseXML = Organization.XML;
                DataRow.DateCompleted = DateTime.Now;
                DataRow.StatusId = Organization.Status;
                DataRow.DateLastUpdated = DateTime.Now;
             //   DataRow.ResponsePasscode = Organization.ResponsePassCode;
                DataRow.ResponseXMLSize = RemoveWhitespace(Organization.XML).Length; 
                Context.SaveChanges();
            }
        }
        public static string RemoveWhitespace(string xml)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@">\s*<");
            xml = regex.Replace(xml, "><");

            return xml.Trim();
        }
        public void UpdatePassCode(UserAuthenticationRequestBO passcodeBO) {


            Guid Id = new Guid(passcodeBO.ResponseId);

            //Update Survey
            using (var Context = DataObjectFactory.CreateContext())
            {
                var Query = from response in Context.Organizations
                            where response.ResponseId == Id
                            select response;

                var DataRow = Query.Single();
                
                DataRow.ResponsePasscode = passcodeBO.PassCode;
                Context.SaveChanges();
            }
        }
        public UserAuthenticationResponseBO GetAuthenticationResponse(UserAuthenticationRequestBO UserAuthenticationRequestBO)
        {

            UserAuthenticationResponseBO UserAuthenticationResponseBO = Mapper.ToAuthenticationResponseBO(UserAuthenticationRequestBO);
            try
            {

                Guid Id = new Guid(UserAuthenticationRequestBO.ResponseId);


                using (var Context = DataObjectFactory.CreateContext())
                {
                    Organization surveyResponse = Context.Organizations.First(x => x.ResponseId == Id);
                    if (surveyResponse != null)
                    {
                        UserAuthenticationResponseBO.PassCode = surveyResponse.ResponsePasscode;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return UserAuthenticationResponseBO;

        }

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
