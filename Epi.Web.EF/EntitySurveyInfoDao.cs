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
        /// Gets SurveyInfo based on a list of ids
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>SurveyInfo.</returns>
        public List<SurveyInfoBO> GetSurveyInfo(List<string> SurveyInfoIdList, int PageNumber = -1, int PageSize = -1)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();
            if (SurveyInfoIdList.Count > 0)
            {
                foreach (string surveyInfoId in SurveyInfoIdList.Distinct())
                {
                    Guid Id = new Guid(surveyInfoId);

                    using (var Context = DataObjectFactory.CreateContext())
                    {
                        result.Add(Mapper.Map(Context.SurveyMetaDatas.FirstOrDefault(x => x.SurveyId == Id)));
                    }
                }
            }
            else
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    result = Mapper.Map(Context.SurveyMetaDatas.ToList());
                }
            }

            return result;
        }


        /// <summary>
        /// Gets SurveyInfo based on criteria
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>SurveyInfo.</returns>
        public List<SurveyInfoBO> GetSurveyInfo(List<string> SurveyInfoIdList, DateTime pClosingDate, int pSurveyType = -1, int PageNumber = -1, int PageSize = -1)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();

            List<SurveyMetaData> responseList = new List<SurveyMetaData>();
            if (SurveyInfoIdList.Count > 0)
            {
                foreach (string surveyInfoId in SurveyInfoIdList.Distinct())
                {
                    Guid Id = new Guid(surveyInfoId);

                    using (var Context = DataObjectFactory.CreateContext())
                    {
                        responseList.Add(Context.SurveyMetaDatas.FirstOrDefault(x => x.SurveyId == Id));
                    }
                }
            }
            else
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    responseList = Context.SurveyMetaDatas.ToList();
                }
            }

            if (pSurveyType > -1)
            {
                List<SurveyMetaData> statusList = new List<SurveyMetaData>();
                statusList.AddRange(responseList.Where(x => x.SurveyTypeId == pSurveyType));
                responseList = statusList;
            }

            if (pClosingDate > DateTime.MinValue)
            {
                List<SurveyMetaData> dateList = new List<SurveyMetaData>();

                dateList.AddRange(responseList.Where(x => x.ClosingDate.Month >= pClosingDate.Month && x.ClosingDate.Year >= pClosingDate.Year && x.ClosingDate.Day >= pClosingDate.Day));
                responseList = dateList;
            }

            result = Mapper.Map(responseList);
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
                var SurveyMetaDataEntity = Mapper.Map(SurveyInfo);
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
            Guid Id = new Guid(SurveyInfo.SurveyId);

            //Update Survey
            using (var Context = DataObjectFactory.CreateContext())
            {
                var Query = from response in Context.SurveyMetaDatas
                            where response.SurveyId == Id
                            select response;

                var DataRow = Query.Single();
                DataRow.SurveyName = SurveyInfo.SurveyName;
                DataRow.SurveyNumber = SurveyInfo.SurveyNumber;
                DataRow.TemplateXML = SurveyInfo.XML;
                DataRow.IntroductionText = SurveyInfo.IntroductionText;
                DataRow.ExitText = SurveyInfo.ExitText;
                DataRow.OrganizationName = SurveyInfo.OrganizationName;
                DataRow.DepartmentName = SurveyInfo.DepartmentName;
                DataRow.ClosingDate = SurveyInfo.ClosingDate;
                DataRow.SurveyTypeId = SurveyInfo.SurveyType;
                DataRow.UserPublishKey = SurveyInfo.UserPublishKey;

                Context.SaveChanges();
            }

        
        }

        /// <summary>
        /// Deletes a SurveyInfo
        /// </summary>
        /// <param name="SurveyInfo">SurveyInfo.</param>
        public void DeleteSurveyInfo(SurveyInfoBO SurveyInfo)
        {

           //Delete Survey
       
       }

        public PageInfoBO GetSurveySizeInfo( DateTime pClosingDate, int pSurveyType = -1, int ResponseMaxSize = -1)
        {
            PageInfoBO result = new PageInfoBO();

        
           int NumberOfRows = 0;
           int ResponsesTotalsize = 0;
           decimal AvgResponseSize = 0;
           decimal NumberOfResponsPerPage = 0;
             
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from response in Context.SurveyMetaDatas
                               
                                select response;


              
                    NumberOfRows = Query.Select(x => x.TemplateXMLSize).Count();
                    ResponsesTotalsize = (int)Query.Select(x => x.TemplateXMLSize).Sum();

                    AvgResponseSize =  ResponsesTotalsize / NumberOfRows;
                    NumberOfResponsPerPage = (int)Math.Ceiling(ResponseMaxSize / AvgResponseSize);


                    result.PageSize = (int) Math.Ceiling( NumberOfResponsPerPage);
                    result.NumberOfPages = (int) Math.Ceiling(NumberOfRows / NumberOfResponsPerPage);
                }
            
            
            return result;
        }
       
    }
}
