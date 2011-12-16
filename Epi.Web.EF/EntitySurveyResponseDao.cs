using System;
using System.Linq;
using System.Collections.Generic;

//using BusinessObjects;
//using DataObjects.EntityFramework.ModelMapper;
//using System.Linq.Dynamic;
using Epi.Web.Interfaces.DataInterfaces;
using Epi.Web.Common.BusinessObject;

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
        public SurveyResponseBO GetSurveyResponse(string SurveyResponseId)
        {

            SurveyResponseBO result = new SurveyResponseBO();
            Guid Id = new Guid(SurveyResponseId);

            using (var Context = DataObjectFactory.CreateContext())
            {

                result = Mapper.Map(Context.SurveyResponses.FirstOrDefault(x => x.ResponseId == Id));
            }

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
                DataRow.StatusId = 2;
                DataRow.DateLastUpdated = DateTime.Now;
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
