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
    /// Entity Framework implementation of the ISurveyResponseDao interface.
    /// </summary>
    public class EntitySurveyResponseDao : ISurveyResponseDao
    {
        /// <summary>
        /// Gets a specific SurveyResponse.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyResponseIdList, Guid UserPublishKey, int PageNumber = -1, int PageSize = -1)
        {

            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            if (SurveyResponseIdList.Count > 0)
            {
                foreach (string surveyResponseId in SurveyResponseIdList.Distinct())
                {
                    Guid Id = new Guid(surveyResponseId);

                    using (var Context = DataObjectFactory.CreateContext())
                    {

                        result.Add(Mapper.Map(Context.SurveyResponses.FirstOrDefault(x => x.ResponseId == Id )));
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

            if (PageNumber > 0 && PageSize > 0)
            {
                result.Sort(CompareByDateCreated);
                // remove the items to skip
                if (PageNumber * PageSize - PageSize > 0)
                {
                    result.RemoveRange(0, PageSize);
                }

                if (PageNumber * PageSize < result.Count)
                {
                    result.RemoveRange(PageNumber * PageSize, result.Count - PageNumber * PageSize);
                }
            }


            return result;
        }


        public List<SurveyResponseBO> GetSurveyResponseSize(List<string> SurveyResponseIdList, Guid UserPublishKey, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {
         

            List<SurveyResponseBO> resultRows =  GetSurveyResponse(SurveyResponseIdList,  UserPublishKey,  PageNumber ,  PageSize );


            return resultRows;
        }

        /// <summary>
        /// Gets SurveyResponses per a SurveyId.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        public List<SurveyResponseBO> GetSurveyResponseBySurveyId(List<string> SurveyIdList, Guid UserPublishKey, int PageNumber = -1, int PageSize = -1)
        {

            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            try {
            foreach (string surveyResponseId in SurveyIdList.Distinct())
            {
                Guid Id = new Guid(surveyResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {

                    result.Add(Mapper.Map(Context.SurveyResponses.FirstOrDefault(x => x.SurveyId == Id )));
                }
            }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            if (PageNumber > 0 && PageSize > 0)
            {
                result.Sort(CompareByDateCreated);
                // remove the items to skip
                if (PageNumber * PageSize - PageSize > 0)
                {
                    result.RemoveRange(0, PageSize);
                }

                if (PageNumber * PageSize < result.Count)
                {
                    result.RemoveRange(PageNumber * PageSize, result.Count - PageNumber * PageSize);
                }
            }


            return result;
        }


        public List<SurveyResponseBO> GetSurveyResponseBySurveyIdSize(List<string> SurveyIdList, Guid UserPublishKey , int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {
         
        

            List<SurveyResponseBO> resultRows =  GetSurveyResponseBySurveyId(SurveyIdList,  UserPublishKey,  PageNumber ,  PageSize );

        
            return resultRows;
         }

        /// <summary>
        /// Gets SurveyResponses depending on criteria.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, bool pIsDraftMode = false,int pStatusId = -1, int PageNumber = -1, int PageSize = -1)
        {
         List<SurveyResponseBO> Finalresult = new List<SurveyResponseBO>();
         IEnumerable<SurveyResponseBO> result;
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
                        throw (ex);
                    }
                }
            }
            else
            {
                try{
                using (var Context = DataObjectFactory.CreateContext())
                {
                    if (!string.IsNullOrEmpty(pSurveyId))
                    {
                        Guid Id = new Guid(pSurveyId);
                       // responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id).ToList();
                        //if (pStatusId>0)
                        //{
                        //responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id && x.StatusId == pStatusId && x.IsDraftMode == pIsDraftMode).OrderBy(x=>x.DateCompleted).ToList();
                        //}
                        //else{
                        //    pStatusId = -1;
                        //    responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id && x.StatusId != 4 && x.IsDraftMode == pIsDraftMode).OrderBy(x => x.DateCompleted).ToList();
                        
                        //}
                        if (pStatusId == 0) // All 2,3,and 4 if available
                        {
                        responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id  && x.IsDraftMode == pIsDraftMode).OrderBy(x=>x.DateCompleted).ToList();
                        }
                        if (pStatusId == 3) // Only 3
                        {
                            
                            responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id && x.StatusId == pStatusId && x.IsDraftMode == pIsDraftMode).OrderBy(x => x.DateCompleted).ToList();

                        }
                       if (pStatusId == 4) //   3 and 4
                       {

                           responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id && (x.StatusId == pStatusId || x.StatusId == 3)&& x.IsDraftMode == pIsDraftMode).OrderBy(x => x.DateCompleted).ToList();

                       }
                    }
                    
                   
                }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }


            //if(! string.IsNullOrEmpty(pSurveyId))
            //{
            //    Guid Id = new Guid(pSurveyId);
            //    List<SurveyResponse> surveyList = new List<SurveyResponse>();
            //    surveyList.AddRange(responseList.Where(x => x.SurveyId == Id));
            //    responseList = surveyList;
            //}

            //if (pStatusId > -1)
            //{
            //    List<SurveyResponse> statusList = new List<SurveyResponse>();
            //    statusList.AddRange(responseList.Where(x => x.StatusId == pStatusId));
            //    responseList = statusList;
            //}

            if (pDateCompleted > DateTime.MinValue)
            {
                List<SurveyResponse> dateList = new List<SurveyResponse>();

                //dateList.AddRange(responseList.Where(x => x.DateCompleted.Value.Month ==  pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
                dateList.AddRange(responseList.Where(x =>x.DateCompleted !=null && x.DateCompleted.Value.Month == pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
                responseList = dateList;
            }

           
          
                
                //if (PageNumber > 0 && PageSize > 0)
                //{
                //    result.Sort(CompareByDateCreated);
                //    // remove the items to skip
                //    if (PageNumber * PageSize - PageSize > 0)
                //    {
                //        result.RemoveRange(0, PageSize);
                //    }

                //    if (PageNumber * PageSize < result.Count)
                //    {
                //        result.RemoveRange(PageNumber * PageSize, result.Count - PageNumber * PageSize);
                //    }
                //}

            if(PageSize!=-1 && PageNumber != -1){
             result = Mapper.Map(responseList);
               result = result.Skip((PageNumber - 1) * PageSize).Take(PageSize);
                foreach(var item in result)
                    {
                    Finalresult.Add(item);
                
                    }
                    return Finalresult;
                }
            else
                {

               
               Finalresult = Mapper.Map(responseList);

                  
                return Finalresult;
                }
           
              
           

            
        }


        public List<SurveyResponseBO> GetSurveyResponseSize(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted,bool pIsDraftMode = false,int pStatusId = -1, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {


            List<SurveyResponseBO> resultRows = GetSurveyResponse(SurveyAnswerIdList, pSurveyId, pDateCompleted, pIsDraftMode,pStatusId, PageNumber, PageSize);
 

            return resultRows;
         
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
            try
            {
            using (var Context = DataObjectFactory.CreateContext() ) 
            {
                SurveyResponse SurveyResponseEntity = Mapper.ToEF(SurveyResponse);
                Context.AddToSurveyResponses(SurveyResponseEntity);
               
                Context.SaveChanges();
            }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
             
        }

        /// <summary>
        /// Updates a SurveyResponse.
        /// </summary>
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public void UpdateSurveyResponse(SurveyResponseBO SurveyResponse)
        {
            try{
            Guid Id = new Guid(SurveyResponse.ResponseId);

        //Update Survey
            using (var Context = DataObjectFactory.CreateContext())
            {
                var Query = from response in Context.SurveyResponses
                            where response.ResponseId == Id 
                            select response;

                var DataRow = Query.Single();

              

                DataRow.ResponseXML = SurveyResponse.XML;
                //DataRow.DateCompleted = DateTime.Now;
                DataRow.DateCompleted = SurveyResponse.DateCompleted;
                DataRow.StatusId = SurveyResponse.Status;
                DataRow.DateUpdated = DateTime.Now;
             //   DataRow.ResponsePasscode = SurveyResponse.ResponsePassCode;
                DataRow.IsDraftMode = SurveyResponse.IsDraftMode;
                DataRow.ResponseXMLSize = RemoveWhitespace(SurveyResponse.XML).Length; 
                Context.SaveChanges();
            }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public static string RemoveWhitespace(string xml)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@">\s*<");
            xml = regex.Replace(xml, "><");

            return xml.Trim();
        }
        public void UpdatePassCode(UserAuthenticationRequestBO passcodeBO) {

            try 
            {
            Guid Id = new Guid(passcodeBO.ResponseId);

            //Update Survey
            using (var Context = DataObjectFactory.CreateContext())
            {
                var Query = from response in Context.SurveyResponses
                            where response.ResponseId == Id
                            select response;

                var DataRow = Query.Single();
                
                DataRow.ResponsePasscode = passcodeBO.PassCode;
                Context.SaveChanges();
            }
            }
            catch (Exception ex)
            {
                throw (ex);
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
                    SurveyResponse surveyResponse = Context.SurveyResponses.First(x => x.ResponseId == Id);
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
        /// Deletes a SurveyResponse
        /// </summary>
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public void DeleteSurveyResponse(SurveyResponseBO SurveyResponse)
        {

           //Delete Survey
       
       }



        private static int CompareByDateCreated(SurveyResponseBO x, SurveyResponseBO y)
        {
            return x.DateCreated.CompareTo(y.DateCreated);
        }



        public void UpdateRecordStatus(SurveyResponseBO SurveyResponseBO)
        {

            try
            {
                Guid Id = new Guid(SurveyResponseBO.ResponseId);

                
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from response in Context.SurveyResponses
                                where response.ResponseId == Id
                                select response;

                    var DataRow = Query.Single();

                    if (DataRow.StatusId == 3 && SurveyResponseBO.Status==4)
                     {

                           DataRow.StatusId = SurveyResponseBO.Status;
                     }

                    if (  SurveyResponseBO.Status != 4)
                    {

                        DataRow.StatusId = SurveyResponseBO.Status;
                    }


                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        
        }
       
    }

    
}
