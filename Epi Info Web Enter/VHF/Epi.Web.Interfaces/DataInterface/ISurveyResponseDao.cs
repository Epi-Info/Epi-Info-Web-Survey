using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Criteria;

namespace Epi.Web.Enter.Interfaces.DataInterfaces
{
    /// <summary>
    /// Defines methods to access SurveyResponses.
    /// </summary>
    /// <remarks>
    /// This is a database-independent interface. Implementations are database specific.
    /// </remarks>
    public interface ISurveyResponseDao
    {
        /// <summary>
        /// Gets a specific SurveyResponse.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyResponseIdList, Guid UserPublishKey, int PageNumber = -1, int PageSize = -1);
        List<SurveyResponseBO> GetSurveyResponseSize(List<string> SurveyResponseIdList, Guid UserPublishKey, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1);

        /// <summary>
        /// Gets a specific SurveyResponse.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        List<SurveyResponseBO> GetSurveyResponseBySurveyId(List<string> SurveyIdList, Guid UserPublishKey, int PageNumber = -1, int PageSize = -1);

        List<SurveyResponseBO> GetSurveyResponseBySurveyIdSize(List<string> SurveyIdList, Guid UserPublishKey,  int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1);
        
        /// <summary>
        /// Get SurveyResponses based on criteria.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId = -1, int PageNumber = -1, int PageSize = -1);

        List<SurveyResponseBO> GetSurveyResponseSize(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted,  int pStatusId = -1, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1);
        /// <summary>
        /// Gets a sorted list of all SurveyResponses.
        /// </summary>
        /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of SurveyResponses.</returns>
       // List<SurveyResponseBO> GetSurveyResponses(string sortExpression = "SurveyResponseId ASC");
        
        /// <summary>
        /// Gets SurveyResponse given an order.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>SurveyResponse.</returns>
       // SurveyResponseBO GetSurveyResponseByOrder(int orderId);

        /// <summary>
        /// Gets SurveyResponses with order statistics in given sort order.
        /// </summary>
        /// <param name="SurveyResponses">SurveyResponse list.</param>
        /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of SurveyResponses with order statistics.</returns>
     //   List<SurveyResponseBO> GetSurveyResponsesWithOrderStatistics(string sortExpression);

        /// <summary>
        /// Inserts a new SurveyResponse. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyResponse object will contain the new identifier.
        /// </remarks>
        /// <param name="SurveyResponse">SurveyResponse.</param>
        void InsertSurveyResponse(SurveyResponseBO SurveyResponse);
        /// <summary>
        /// Inserts a new SurveyResponse. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyResponse object will contain the new identifier.
        /// </remarks>
        /// <param name="SurveyResponse">SurveyResponse.</param>
        void InsertChildSurveyResponse(SurveyResponseBO SurveyResponse);
        /// <summary>
        /// Updates a SurveyResponse.
        /// </summary>
        /// <param name="SurveyResponse">SurveyResponse.</param>
        void UpdateSurveyResponse(SurveyResponseBO SurveyResponse);

        /// <summary>
        /// Deletes a SurveyResponse
        /// </summary>
        /// <param name="SurveyResponse">SurveyResponse.</param>
         void DeleteSurveyResponse(SurveyResponseBO SurveyResponse);

         void UpdatePassCode(UserAuthenticationRequestBO passcodeBO);
         UserAuthenticationResponseBO GetAuthenticationResponse(UserAuthenticationRequestBO passcodeBO);

         List<SurveyResponseBO> GetFormResponseByFormId(string FormId,int PageNumber, int PageSize);
         List<SurveyResponseBO> GetFormResponseByFormId(SurveyAnswerCriteria criteria);
         int GetFormResponseCount(string FormId);

         int GetFormResponseCount(SurveyAnswerCriteria Criteria);

         string GetResponseParentId(string ResponseId);
         SurveyResponseBO GetSingleResponse(string ResponseId);

         List<SurveyResponseBO> GetResponsesHierarchyIdsByRootId(string RootId);
         void DeleteSingleSurveyResponse(SurveyResponseBO SurveyResponse); 
         SurveyResponseBO GetFormResponseByParentRecordId(string ResponseId);
         List<SurveyResponseBO> GetAncestorResponseIdsByChildId(string ChildId);

         List<SurveyResponseBO> GetResponsesByRelatedFormId(string ResponseId, string SurveyId);
         List<SurveyResponseBO> GetResponsesByRelatedFormId(string ResponseId, SurveyAnswerCriteria Criteria);
         void DeleteSurveyResponseInEditMode(SurveyResponseBO SurveyResponse);

         SurveyResponseBO  GetResponseXml(string ResponseId);

         void DeleteResponseXml(ResponseXmlBO ResponseXmlBO);

         void InsertResponseXml(ResponseXmlBO item);
         bool ISResponseExists(Guid ResponseId);

        //bool ISResponseExists(SurveyAnswerCriteria Criteria);

         bool HasResponse(SurveyAnswerCriteria Criteria);

         void UpdateRecordStatus(SurveyResponseBO SurveyResponseBO);
    }
}
