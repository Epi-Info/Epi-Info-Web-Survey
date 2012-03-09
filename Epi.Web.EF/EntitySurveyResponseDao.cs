using System;
using System.Linq;
using System.Collections.Generic;

//using BusinessObjects;
//using DataObjects.EntityFramework.ModelMapper;
//using System.Linq.Dynamic;
using Epi.Web.Interfaces.DataInterfaces;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;

namespace Epi.Web.EF
{
    /// <summary>
    /// Entity Framework implementation of the ISurveyResponseDao interface.
    /// </summary>
    public class EntitySurveyResponseDao : ISurveyResponseDao
    {
        /// <summary>
        /// Gets a specific SurveyResponse.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyResponseIdList, Guid UserPublishKey)
        {

            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            if (SurveyResponseIdList.Count > 0)
            {
                foreach (string surveyResponseId in SurveyResponseIdList.Distinct())
                {
                    Guid Id = new Guid(surveyResponseId);

                    using (var Context = DataObjectFactory.CreateContext())
                    {

                        result.Add(Mapper.Map(Context.SurveyResponses.FirstOrDefault(x => x.ResponseId == Id && x.UserPublishKey == UserPublishKey)));
                    }
                }
            }
            else
            {
                using (var Context = DataObjectFactory.CreateContext())
                {

                    result = Mapper.Map(Context.SurveyResponses.ToList());
                }
            }

            return result;
        }


        /// <summary>
        /// Gets SurveyResponses per a SurveyId.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        public List<SurveyResponseBO> GetSurveyResponseBySurveyId(List<string> SurveyIdList, Guid UserPublishKey)
        {

            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            foreach (string surveyResponseId in SurveyIdList.Distinct())
            {
                Guid Id = new Guid(surveyResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {

                    result.Add(Mapper.Map(Context.SurveyResponses.FirstOrDefault(x => x.SurveyId == Id && x.UserPublishKey == UserPublishKey)));
                }
            }

            return result;
        }


        /// <summary>
        /// Gets SurveyResponses depending on criteria.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId = -1 )
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            List<SurveyResponse> responseList = new List<SurveyResponse>();

            if (SurveyAnswerIdList.Count > 0)
            {
                foreach (string surveyResponseId in SurveyAnswerIdList.Distinct())
                {
                    try
                    {
                        Guid Id = new Guid(surveyResponseId);
                       

                        using (var Context = DataObjectFactory.CreateContext())
                        {
                            SurveyResponse surveyResponse = Context.SurveyResponses.First(x => x.ResponseId == Id );
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
                    responseList = Context.SurveyResponses.ToList();
                }
            }


            if(! string.IsNullOrEmpty(pSurveyId))
            {
                Guid Id = new Guid(pSurveyId);
                List<SurveyResponse> surveyList = new List<SurveyResponse>();
                surveyList.AddRange(responseList.Where(x => x.SurveyId == Id));
                responseList = surveyList;
            }

            if (pStatusId > -1)
            {
                List<SurveyResponse> statusList = new List<SurveyResponse>();
                statusList.AddRange(responseList.Where(x => x.StatusId == pStatusId));
                responseList = statusList;
            }

            if (pDateCompleted > DateTime.MinValue)
            {
                List<SurveyResponse> dateList = new List<SurveyResponse>();

                dateList.AddRange(responseList.Where(x => x.DateCompleted.Value.Month ==  pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
                responseList = dateList;
            }

            result = Mapper.Map(responseList);
            return result;
        }


        /// <summary>
        /// Inserts a new SurveyResponse. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyResponse object will contain the new identifier.
        /// </remarks>  
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public  void InsertSurveyResponse(SurveyResponseBO SurveyResponse)
        {
            using (var Context = DataObjectFactory.CreateContext() ) 
            {
                SurveyResponse SurveyResponseEntity = Mapper.ToEF(SurveyResponse);
                Context.AddToSurveyResponses(SurveyResponseEntity);
               
                Context.SaveChanges();
            }

             
        }

        /// <summary>
        /// Updates a SurveyResponse.
        /// </summary>
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public void UpdateSurveyResponse(SurveyResponseBO SurveyResponse)
        {
            Guid Id = new Guid(SurveyResponse.ResponseId);

        //Update Survey
            using (var Context = DataObjectFactory.CreateContext())
            {
                var Query = from response in Context.SurveyResponses
                            where response.ResponseId == Id 
                            select response;

                var DataRow = Query.Single();
                DataRow.ResponseXML = SurveyResponse.XML;
                DataRow.DateCompleted = DateTime.Now;
                DataRow.StatusId = SurveyResponse.Status;
                DataRow.DateLastUpdated = DateTime.Now;
                DataRow.UserPublishKey = SurveyResponse.UserPublishKey;
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a SurveyResponse
        /// </summary>
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public void DeleteSurveyResponse(SurveyResponseBO SurveyResponse)
        {

           //Delete Survey
       
       }
       
    }
}
