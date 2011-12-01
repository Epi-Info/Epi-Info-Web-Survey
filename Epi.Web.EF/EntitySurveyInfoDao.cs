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
        public SurveyInfoBO GetSurveyInfo(string SurveyInfoId)
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
        /// Inserts a new SurveyInfo. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyInfo object will contain the new identifier.
        /// </remarks>  
        /// <param name="SurveyInfo">SurveyInfo.</param>
        public  void InsertSurveyInfo(SurveyInfoBO SurveyInfo)
        {
            using (var Context = DataObjectFactory.CreateContext() ) 
            {
                var SurveyMetaDataEntity = DataObjectFactory.CreateSurveyMetaData();
                SurveyMetaDataEntity = Mapper.Map(SurveyInfo);
                Context.AddToSurveyMetaDatas(SurveyMetaDataEntity);
               
                Context.SaveChanges();
            }

             
        }

        /// <summary>
        /// Updates a SurveyInfo.
        /// </summary>
        /// <param name="SurveyInfo">SurveyInfo.</param>
        public void UpdateSurveyInfo(SurveyInfoBO SurveyInfo)
        { 
        //Update Survey
        
        }

        /// <summary>
        /// Deletes a SurveyInfo
        /// </summary>
        /// <param name="SurveyInfo">SurveyInfo.</param>
        public void DeleteSurveyInfo(SurveyInfoBO SurveyInfo)
        {

           //Delete Survey
       
       }
       
    }
}
