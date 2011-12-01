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
    /// Entity Framework implementation of the ISurveyInfoDao interface.
    /// </summary>
    public class EntitySurveyInfoDao : ISurveyInfoDao
    {
        /// <summary>
        /// Gets a specific SurveyInfo.
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>SurveyInfo.</returns>
        SurveyInfoBO GetSurveyInfo(string SurveyInfoId)
        {

            SurveyInfoBO result = new SurveyInfoBO();
            Guid Id = new Guid(SurveyInfoId);

            using (var Context = DataObjectFactory.CreateContext())
            {

                result = Mapper.Map(Context.SurveyMetaDatas.FirstOrDefault(x => x.SurveyId == Id));
            }

            return result;
        }
        /// <summary>
        /// Gets a sorted list of all SurveyInfos.
        /// </summary>
        /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of SurveyInfos.</returns>
        List<SurveyInfoBO> GetSurveyInfos(string sortExpression = "SurveyInfoId ASC");

        /// <summary>
        /// Gets SurveyInfo given an order.
        /// </summary>
        /// <param name="orderId">Unique order identifier.</param>
        /// <returns>SurveyInfo.</returns>
        SurveyInfoBO GetSurveyInfoByOrder(int orderId);

        /// <summary>
        /// Gets SurveyInfos with order statistics in given sort order.
        /// </summary>
        /// <param name="SurveyInfos">SurveyInfo list.</param>
        /// <param name="sortExpression">Sort order.</param>
        /// <returns>Sorted list of SurveyInfos with order statistics.</returns>
        List<SurveyInfoBO> GetSurveyInfosWithOrderStatistics(string sortExpression);

        /// <summary>
        /// Inserts a new SurveyInfo. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyInfo object will contain the new identifier.
        /// </remarks>
        /// <param name="SurveyInfo">SurveyInfo.</param>
        SurveyInfoBO InsertSurveyInfo(SurveyInfoBO SurveyInfo)
        {
            using (var Context = new Epi.Web.EF.EIWSEntities( DataObjectFactory.CreateContext().ToString()))
            {
                
            }

            return null;
        }

        /// <summary>
        /// Updates a SurveyInfo.
        /// </summary>
        /// <param name="SurveyInfo">SurveyInfo.</param>
        //void UpdateSurveyInfo(SurveyInfoBO SurveyInfo);

        /// <summary>
        /// Deletes a SurveyInfo
        /// </summary>
        /// <param name="SurveyInfo">SurveyInfo.</param>
       //void DeleteSurveyInfo(SurveyInfoBO SurveyInfo);
       
    }
}
