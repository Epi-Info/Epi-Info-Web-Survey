using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;

namespace Epi.Web.Interfaces.DataInterfaces
{
    /// <summary>
    /// Defines methods to access SurveyInfos.
    /// </summary>
    /// <remarks>
    /// This is a database-independent interface. Implementations are database specific.
    /// </remarks>
    public interface ISurveyInfoDao
    {
        /// <summary>
        /// Gets SurveyInfo based on a list of ids
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>SurveyInfo.</returns>
        List<SurveyInfoBO> GetSurveyInfo(List<string> SurveyInfoIdList, int PageNumber = -1, int PageSize = -1);



        List<SurveyInfoBO> GetSurveyInfoByOrgKeyAndPublishKey(string SurveyId, string pOrganizationKey, Guid publishKey);

        List<SurveyInfoBO> GetSurveyInfoByOrgKey(string SurveyId, string pOrganizationKey);
        SurveyInfoBO GetDashboardSurveyInfo(string surveyid);



        /// <summary>
        /// Gets SurveyInfo Size data based on a list of ids
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>PageInfoBO.</returns>
        List<SurveyInfoBO> GetSurveySizeInfo(List<string> SurveyInfoIdList,  int PageNumber = -1, int PageSize = -1, int ResponsesTotalsize = -1);



        /// <summary>
        /// Gets SurveyInfo based on criteria
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>SurveyInfo.</returns>
        List<SurveyInfoBO> GetSurveyInfo(List<string> SurveyInfoIdList, DateTime pClosingDate, string Okey, int pSurveyType = -1, int PageNumber = -1, int PageSize = -1);




        /// <summary>
        /// Gets SurveyInfo Size data based on a list of ids
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>PageInfoBO.</returns>
        List<SurveyInfoBO> GetSurveySizeInfo(List<string> SurveyInfoIdList, DateTime pClosingDate, string Okey,  int pSurveyType = -1, int PageNumber = -1, int PageSize = -1, int ResponsesTotalsize = -1);



        /// <summary>
        /// Gets a sorted list of all SurveyInfos.
        /// </summary>
        /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of SurveyInfos.</returns>
       // List<SurveyInfoBO> GetSurveyInfos(string sortExpression = "SurveyInfoId ASC");
        
        /// <summary>
        /// Gets SurveyInfo given an order.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>SurveyInfo.</returns>
       // SurveyInfoBO GetSurveyInfoByOrder(int orderId);

        /// <summary>
        /// Gets SurveyInfos with order statistics in given sort order.
        /// </summary>
        /// <param name="SurveyInfos">SurveyInfo list.</param>
        /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of SurveyInfos with order statistics.</returns>
     //   List<SurveyInfoBO> GetSurveyInfosWithOrderStatistics(string sortExpression);

        /// <summary>
        /// Inserts a new SurveyInfo. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyInfo object will contain the new identifier.
        /// </remarks>
        /// <param name="SurveyInfo">SurveyInfo.</param>
       void InsertSurveyInfo(SurveyInfoBO SurveyInfo);

        /// <summary>
        /// Updates a SurveyInfo.
        /// </summary>
        /// <param name="SurveyInfo">SurveyInfo.</param>
        void UpdateSurveyInfo(SurveyInfoBO SurveyInfo);

        /// <summary>
        /// Deletes a SurveyInfo
        /// </summary>
        /// <param name="SurveyInfo">SurveyInfo.</param>
         void DeleteSurveyInfo(SurveyInfoBO SurveyInfo);

         /// <summary>
         /// Deletes a SurveyInfo
         /// </summary>
         /// <param name="SurveyInfo">SurveyInfo.</param>
         List<SurveyInfoBO> GetChildInfoByParentId(string ParentFormId , int ViewId);

         SurveyInfoBO GetParentInfoByChildId(string ChildId);



         List<SurveyInfoBO> GetFormsHierarchyIdsByRootId(string RootId);
         void InsertFormdefaultSettings(string FormId,bool  IsSqlProject,List<string> ControlsNameList);
         void UpdateParentId(string SurveyId, int ViewId, string ParentId);
         void InsertConnectionString(DbConnectionStringBO ConnectionString);


         /// <summary>
         /// Deletes a SurveyInfo
         /// </summary>
         /// <param name="SurveyInfo">SurveyInfo.</param>
         void UpdateConnectionString(DbConnectionStringBO ConnectionString);

         int GetOrganizationId(string OrgKey);

         List<SurveyInfoBO> GetAllSurveysByOrgKey(string OrgKey);

         void ValidateServername(SurveyInfoBO pRequestMessage);
         void InsertSourceTable(string SourcetableXml, string SourcetableName, string FormId);
         List<SourceTableBO> GetSourceTables(string SurveyId);

         void UpdateSourceTable(string p, string SourcetableName, string FormId);
    }
}
