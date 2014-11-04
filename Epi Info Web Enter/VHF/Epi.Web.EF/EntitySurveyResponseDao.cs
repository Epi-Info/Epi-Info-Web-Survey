using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
//using BusinessObjects;
//using DataObjects.EntityFramework.ModelMapper;
//using System.Linq.Dynamic;
using Epi.Web.Enter.Interfaces.DataInterfaces;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Common.Extension;
using System.Data;
using System.Data.SqlClient;
namespace Epi.Web.EF
{ 
    /// <summary>
    /// Entity Framework implementation of the ISurveyResponseDao interface.
    /// </summary> 
    public class EntitySurveyResponseDao : ISurveyResponseDao
    {

        private int sqlProjectResponsesCount;

        /// <summary>
        /// Reads Number of responses for SqlProject
        /// </summary>
        public int SqlProjectResponsesCount
        {
            get { return sqlProjectResponsesCount; }
            set { sqlProjectResponsesCount = value; }
        }

        private bool isSqlProject;
        /// <summary>
        /// Flag for IsSqlProject
        /// </summary>
        public bool IsSqlProject
        {
            get { return isSqlProject; }
            set { isSqlProject = value; }
        }


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

                    result.Add(Mapper.Map(Context.SurveyResponses.FirstOrDefault(x => x.ResponseId == Id)));
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


            List<SurveyResponseBO> resultRows = GetSurveyResponse(SurveyResponseIdList, UserPublishKey, PageNumber, PageSize);


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

            try
            {
                foreach (string surveyResponseId in SurveyIdList.Distinct())
                {
                    Guid Id = new Guid(surveyResponseId);

                    using (var Context = DataObjectFactory.CreateContext())
                    {

                        result.Add(Mapper.Map(Context.SurveyResponses.FirstOrDefault(x => x.SurveyId == Id)));
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


        public List<SurveyResponseBO> GetSurveyResponseBySurveyIdSize(List<string> SurveyIdList, Guid UserPublishKey, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {



            List<SurveyResponseBO> resultRows = GetSurveyResponseBySurveyId(SurveyIdList, UserPublishKey, PageNumber, PageSize);


            return resultRows;
        }

        /// <summary>
        /// Gets SurveyResponses depending on criteria.
        /// </summary>
        /// <param name="SurveyResponseId">Unique SurveyResponse identifier.</param>
        /// <returns>SurveyResponse.</returns>
        //public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId = -1, int PageNumber = -1, int PageSize = -1)
        //{
        //    List<SurveyResponseBO> result = new List<SurveyResponseBO>();
        //    List<SurveyResponse> responseList = new List<SurveyResponse>();

        //    if (SurveyAnswerIdList.Count > 0)
        //    {
        //        foreach (string surveyResponseId in SurveyAnswerIdList.Distinct())
        //        {
        //            try
        //            {
        //                Guid Id = new Guid(surveyResponseId);


        //                using (var Context = DataObjectFactory.CreateContext())
        //                {
        //                    SurveyResponse surveyResponse = Context.SurveyResponses.First(x => x.ResponseId == Id );
        //                    if (surveyResponse != null)
        //                    {
        //                        responseList.Add(surveyResponse);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw (ex);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        try{
        //        using (var Context = DataObjectFactory.CreateContext())
        //        {
        //            if (!string.IsNullOrEmpty(pSurveyId))
        //            {
        //                Guid Id = new Guid(pSurveyId);
        //                responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id).ToList();
        //            }


        //        }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw (ex);
        //        }
        //    }


        //    //if(! string.IsNullOrEmpty(pSurveyId))
        //    //{
        //    //    Guid Id = new Guid(pSurveyId);
        //    //    List<SurveyResponse> surveyList = new List<SurveyResponse>();
        //    //    surveyList.AddRange(responseList.Where(x => x.SurveyId == Id));
        //    //    responseList = surveyList;
        //    //}

        //    if (pStatusId > -1)
        //    {
        //        List<SurveyResponse> statusList = new List<SurveyResponse>();
        //        statusList.AddRange(responseList.Where(x => x.StatusId == pStatusId));
        //        responseList = statusList;
        //    }

        //    if (pDateCompleted > DateTime.MinValue)
        //    {
        //        List<SurveyResponse> dateList = new List<SurveyResponse>();

        //        //dateList.AddRange(responseList.Where(x => x.DateCompleted.Value.Month ==  pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
        //        dateList.AddRange(responseList.Where(x =>x.DateCompleted !=null && x.DateCompleted.Value.Month == pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
        //        responseList = dateList;
        //    }

        //    result = Mapper.Map(responseList);

        //    if (PageNumber > 0 && PageSize > 0)
        //    {
        //        result.Sort(CompareByDateCreated);
        //        // remove the items to skip
        //        if (PageNumber * PageSize - PageSize > 0)
        //        {
        //            result.RemoveRange(0, PageSize);
        //        }

        //        if (PageNumber * PageSize < result.Count)
        //        {
        //            result.RemoveRange(PageNumber * PageSize, result.Count - PageNumber * PageSize);
        //        }
        //    }


        //    return result;
        //}
        public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId = -1, int PageNumber = -1, int PageSize = -1)
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

                        var Context = DataObjectFactory.CreateContext();

                        SurveyResponse surveyResponse = Context.SurveyResponses.First(x => x.ResponseId == Id);
                        if (surveyResponse != null)
                        {
                            responseList.Add(surveyResponse);
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
                try
                {

                    if (!string.IsNullOrEmpty(pSurveyId))
                    {
                        var Context = DataObjectFactory.CreateContext();
                        Guid Id = new Guid(pSurveyId);
                        responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id).ToList();



                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }


            //if (pStatusId > -1)
            //    {
            //    List<SurveyResponse> statusList = new List<SurveyResponse>();
            //    statusList.AddRange(responseList.Where(x => x.StatusId == pStatusId));
            //    responseList = statusList;
            //    }

            //if (pDateCompleted > DateTime.MinValue)
            //    {
            //    List<SurveyResponse> dateList = new List<SurveyResponse>();

            //    dateList.AddRange(responseList.Where(x => x.DateCompleted != null && x.DateCompleted.Value.Month == pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
            //    responseList = dateList;
            //    }










            if (PageSize != -1 && PageNumber != -1)
            {
                result = Mapper.Map(responseList);

                foreach (SurveyResponseBO item in result)
                {
                    List<SurveyResponseBO> ResponsesHierarchy = this.GetResponsesHierarchyIdsByRootId(item.ResponseId.ToString());

                    result.Where(x => x.ResponseId == item.ResponseId).Single().ResponseHierarchyIds = ResponsesHierarchy;

                }


                result = result.Skip((PageNumber - 1) * PageSize).Take(PageSize);
                foreach (var item in result)
                {
                    Finalresult.Add(item);

                }
                return Finalresult;
            }
            else
            {


                Finalresult = Mapper.Map(responseList);
                foreach (SurveyResponseBO item in Finalresult)
                {
                    List<SurveyResponseBO> ResponsesHierarchy = this.GetResponsesHierarchyIdsByRootId(item.ResponseId.ToString());

                    Finalresult.Where(x => x.ResponseId == item.ResponseId).Single().ResponseHierarchyIds = ResponsesHierarchy;

                }

                return Finalresult;
            }





        }


        public List<SurveyResponseBO> GetSurveyResponseSize(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId = -1, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {


            List<SurveyResponseBO> resultRows = GetSurveyResponse(SurveyAnswerIdList, pSurveyId, pDateCompleted, pStatusId, PageNumber, PageSize);


            return resultRows;

        }

        /// <summary>
        /// Inserts a new SurveyResponse. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyResponse object will contain the new identifier.
        /// </remarks>  
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public void InsertSurveyResponse(SurveyResponseBO SurveyResponse)
        {
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    SurveyResponse SurveyResponseEntity = Mapper.ToEF(SurveyResponse);
                    //SurveyResponseEntity.Users.Add(new User { UserID = 2 });
                    User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);
                    SurveyResponseEntity.Users.Add(User);
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
        /// Inserts a new SurveyResponse. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyResponse object will contain the new identifier.
        /// </remarks>  
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public void InsertChildSurveyResponse(SurveyResponseBO SurveyResponse)
        {



            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    SurveyResponse SurveyResponseEntity = Mapper.ToEF(SurveyResponse);
                    User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);
                    SurveyResponseEntity.Users.Add(User);
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
            try
            {
                Guid Id = new Guid(SurveyResponse.ResponseId);

                //Update Survey
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from response in Context.SurveyResponses
                                where response.ResponseId == Id
                                select response;

                    var DataRow = Query.Single();

                    if (!string.IsNullOrEmpty(SurveyResponse.RelateParentId) && SurveyResponse.RelateParentId != Guid.Empty.ToString())
                    {
                        DataRow.RelateParentId = new Guid(SurveyResponse.RelateParentId);
                    }
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
        public void UpdatePassCode(UserAuthenticationRequestBO passcodeBO)
        {

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





            try
            {
                List<SurveyResponseBO> result = new List<SurveyResponseBO>();
                Guid Id = new Guid(SurveyResponse.ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {
                    result = Mapper.Map(Context.SurveyResponses.Where(x => x.ResponseId == Id).OrderBy(x => x.DateCreated).Traverse(x => x.SurveyResponse1));
                    foreach (var Obj in result)
                    {

                        if (!string.IsNullOrEmpty(Obj.ResponseId))
                        {
                            Guid NewId = new Guid(Obj.ResponseId);

                            User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);

                            if (User == null)
                                {
                                var ResponseXml = Context.ResponseXmls.FirstOrDefault(x => x.ResponseId == new Guid(SurveyResponse.ResponseId));
                                User = Context.Users.FirstOrDefault(x => x.UserID == ResponseXml.UserId);
                              }


                            SurveyResponse Response = Context.SurveyResponses.First(x => x.ResponseId == NewId);
                            Response.Users.Remove(User);
                           
                            
                            Context.SurveyResponses.DeleteObject(Response);

                            Context.SaveChanges();
                        }


                    }


                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }



        }

        public void DeleteSurveyResponseInEditMode(SurveyResponseBO SurveyResponse)
        {


            string parentRecordId;


            try
            {
                List<SurveyResponseBO> result = new List<SurveyResponseBO>();
                Guid Id = new Guid(SurveyResponse.ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {
                    result = Mapper.Map(Context.SurveyResponses.Where(x => x.ResponseId == Id).OrderBy(x => x.DateCreated).Traverse(x => x.SurveyResponse1));
                    foreach (var Obj in result)
                    {
                        parentRecordId = "";
                        if (!string.IsNullOrEmpty(Obj.ParentRecordId))
                        {

                            parentRecordId = Obj.ParentRecordId;
                        }
                        if (!string.IsNullOrEmpty(Obj.ResponseId))
                        {
                            Guid NewId = new Guid(Obj.ResponseId);

                            User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);

                            SurveyResponse Response = Context.SurveyResponses.First(x => x.ResponseId == NewId);
                            Response.Users.Remove(User);

                            Context.SurveyResponses.DeleteObject(Response);

                            Context.SaveChanges();
                        }

                        if (!string.IsNullOrEmpty(parentRecordId))
                        {
                            Guid pId = new Guid(parentRecordId);
                            User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);

                            SurveyResponse Response = Context.SurveyResponses.First(x => x.ResponseId == pId);
                            Response.Users.Remove(User);

                            Context.SurveyResponses.DeleteObject(Response);

                            Context.SaveChanges();


                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }



        }



        public void DeleteSingleSurveyResponse(SurveyResponseBO SurveyResponse)
        {


            try
            {

                Guid Id = new Guid(SurveyResponse.ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {

                    User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);

                    SurveyResponse Response = Context.SurveyResponses.First(x => x.ResponseId == Id);
                    Response.Users.Remove(User);

                    Context.SurveyResponses.DeleteObject(Response);

                    Context.SaveChanges();

                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }



        }


        private static int CompareByDateCreated(SurveyResponseBO x, SurveyResponseBO y)
        {
            return x.DateCreated.CompareTo(y.DateCreated);
        }


        List<SurveyResponseBO> ISurveyResponseDao.GetFormResponseByFormId(string FormId, int PageNumber, int PageSize)
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            try
            {

                Guid Id = new Guid(FormId);

                using (var Context = DataObjectFactory.CreateContext())
                {

                    IEnumerable<SurveyResponse> SurveyResponseList = Context.SurveyResponses.ToList().Where(x => x.SurveyId == Id && string.IsNullOrEmpty(x.ParentRecordId.ToString()) == true && string.IsNullOrEmpty(x.RelateParentId.ToString()) == true && x.StatusId > 1).OrderByDescending(x => x.DateUpdated);

                    SurveyResponseList = SurveyResponseList.Skip((PageNumber - 1) * PageSize).Take(PageSize);


                    foreach (SurveyResponse Response in SurveyResponseList)
                    {

                        result.Add(Mapper.Map(Response, Response.Users.First()));

                    }


                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result;
        }

        List<SurveyResponseBO> ISurveyResponseDao.GetFormResponseByFormId(SurveyAnswerCriteria criteria)
        {

            List<SurveyResponseBO> result = new List<SurveyResponseBO>();


            IsSqlProject = IsEISQLProject(criteria.SurveyId);//Checks to see if current form is SqlProject

            if (IsSqlProject)
            {
                //make a connection to datasource table to read the connection string.
                //do a read to see which column belongs to which page/table.
                //do a read from ResponseDisplaySettings to read the column names. if for a given survey they dont exist 
                //read the first 5 columns from EI7 sql server database.

                string tableName = ReadEI7DatabaseName(criteria.SurveyId);

                string EI7ConnectionString = DataObjectFactory.EWEADOConnectionString.Substring(0, DataObjectFactory.EWEADOConnectionString.LastIndexOf('=')) + "=" + tableName;


                SqlConnection EI7Connection = new SqlConnection(EI7ConnectionString);
                string EI7Query;
                if (!criteria.GetAllColumns)
                {
                    EI7Query = BuildEI7Query(criteria.SurveyId, criteria.SortOrder, criteria.Sortfield, EI7ConnectionString, criteria.SearchCriteria, false, criteria.PageSize, criteria.PageNumber);

                }
                else
                {
                    EI7Query = BuildEI7ResponseAllFieldsQuery(criteria.SurveyAnswerIdList[0].ToString(), criteria.SurveyId, EI7ConnectionString);
                }
                if (EI7Query == string.Empty)
                {
                    return result;
                }

                SqlCommand EI7Command = new SqlCommand(EI7Query, EI7Connection);
                EI7Command.CommandType = CommandType.Text;

                SqlDataAdapter EI7Adapter = new SqlDataAdapter(EI7Command);

                DataSet EI7DS = new DataSet();

                EI7Connection.Open();

                try
                {
                    EI7Adapter.Fill(EI7DS);
                    EI7Connection.Close();
                }
                catch (Exception)
                {
                    EI7Connection.Close();
                    throw;
                }


                // List<Dictionary<string, string>> DataRows = new List<Dictionary<string, string>>();

                for (int i = 0; i < EI7DS.Tables[0].Rows.Count; i++)
                {
                    Dictionary<string, string> rowDic = new Dictionary<string, string>();
                    SurveyResponseBO SurveyResponseBO = new Enter.Common.BusinessObject.SurveyResponseBO();
                    for (int j = 0; j < EI7DS.Tables[0].Columns.Count; j++)
                    {
                        rowDic.Add(EI7DS.Tables[0].Columns[j].ColumnName, EI7DS.Tables[0].Rows[i][j].ToString());
                    }
                    //.Skip((PageNumber - 1) * PageSize).Take(PageSize); ;
                    //IEnumerable<KeyValuePair<string, string>> temp = rowDic.AsEnumerable();
                    //temp.Skip((PageNumber - 1) * PageSize).Take(PageSize); 

                    SurveyResponseBO.SqlData = rowDic;
                    result.Add(SurveyResponseBO);
                }

                //SqlProjectResponsesCount = EI7DS.Tables[0].Rows.Count;

                //result = result.Skip((criteria.PageNumber - 1) * criteria.PageSize).Take(criteria.PageSize).ToList();
                //SurveyResponseBO.SqlResponseDataBO.SqlData = DataRows;
            }
            else
            {


                try
                {

                    Guid Id = new Guid(criteria.SurveyId);

                    using (var Context = DataObjectFactory.CreateContext())
                    {

                        IEnumerable<SurveyResponse> SurveyResponseList = Context.SurveyResponses.ToList().Where(x => x.SurveyId == Id && string.IsNullOrEmpty(x.ParentRecordId.ToString()) == true && string.IsNullOrEmpty(x.RelateParentId.ToString()) == true && x.StatusId > 1).OrderByDescending(x => x.DateUpdated);

                        SurveyResponseList = SurveyResponseList.Skip((criteria.PageNumber - 1) * criteria.PageSize).Take(criteria.PageSize);


                        foreach (SurveyResponse Response in SurveyResponseList)
                        {

                            result.Add(Mapper.Map(Response, Response.Users.First()));

                        }


                    }

                }
                catch (Exception ex)
                {
                    throw (ex);
                }

            }


            return result;
        }


        /// <summary>
        /// Builds SQL Select query by reading the columns, tablename from the EWE database.
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        //private string BuildEI7Query(string FormId, string SortOrder, string Sortfield, string EI7Connectionstring, string SearchCriteria = "", bool IsReadingResponseCount = false,
        //    int PageSize = 1, int PageNumber = 1)
        //{
        //    SqlConnection EweConnection = new SqlConnection(DataObjectFactory.EWEADOConnectionString);
        //    EweConnection.Open();

        //    SqlCommand EweCommand = new SqlCommand("usp_GetResponseFieldsInfo", EweConnection);//send formid for stored procedure to look for common columns between the two tables
        //    //Stored procedure that goes queries ResponseDisplaySettings and new table SurveyResonpseTranslate(skinny table) for a given FormId

        //    EweCommand.Parameters.Add("@FormId", SqlDbType.VarChar);
        //    EweCommand.Parameters["@FormId"].Value = FormId.Trim();

        //    EweCommand.CommandType = CommandType.StoredProcedure;
        //    //EweCommand.CreateParameter(  EweCommand.Parameters.Add(new SqlParameter("FormId"), FormId);



        //    SqlDataAdapter EweDataAdapter = new SqlDataAdapter(EweCommand);

        //    DataSet EweDS = new DataSet();

        //    try
        //    {
        //        EweDataAdapter.Fill(EweDS);
        //        EweConnection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        EweConnection.Close();
        //        throw ex;
        //    }
        //    SqlConnection EI7Connection = new SqlConnection(EI7Connectionstring);

        //    EI7Connection.Open();

        //    SqlCommand EI7Command = new SqlCommand(" SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + EweDS.Tables[0].Rows[0][1] + "'", EI7Connection);
        //    object eI7CommandExecuteScalar;
        //    try
        //    {
        //        eI7CommandExecuteScalar = EI7Command.ExecuteScalar();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //    if (EweDS == null || EweDS.Tables.Count == 0 || EweDS.Tables[0].Rows.Count == 0
        //        || eI7CommandExecuteScalar == null)
        //    {
        //        EI7Connection.Close();
        //        return string.Empty;
        //    }

        //    StringBuilder stringBuilder = new StringBuilder();
        //    StringBuilder tableNameBuilder = new StringBuilder();
        //    StringBuilder pagingQueryBuilder = new StringBuilder();

        //    StringBuilder cteSelectBuilder = new StringBuilder();

        //    StringBuilder sortBuilder = new StringBuilder(" ORDER BY ");
        //    if (Sortfield != null && SortOrder != null)
        //    {
        //        sortBuilder.Append(Sortfield + " " + SortOrder);
        //    }
        //    else
        //    {
        //        //sortBuilder.Append(EweDS.Tables[0].Rows[0]["ColumnName"]);
        //        sortBuilder.Append("LastSaveTime"); //default sort on lastsavetime 
        //    }


        //    stringBuilder.Append(" SELECT ROW_NUMBER() OVER( " + sortBuilder.ToString() + ") RowNumber," + EweDS.Tables[0].Rows[0]["ViewTableName"] + ".LastSaveTime," + EweDS.Tables[0].Rows[0]["TableName"] + ".GlobalRecordId,");
        //    cteSelectBuilder.Append(" RowNumber, GlobalRecordId, LastSaveTime, ");
        //    // Builds the select part of the query.
        //    foreach (DataRow row in EweDS.Tables[0].Rows)
        //    {
        //        stringBuilder.Append(row["TableName"] + "." + row["ColumnName"] + ", ");
        //        cteSelectBuilder.Append(row["ColumnName"] + ", ");

        //    }
        //    stringBuilder.Remove(stringBuilder.Length - 2, 1);
        //    cteSelectBuilder.Remove(cteSelectBuilder.Length - 2, 1);

        //    stringBuilder.Append(" FROM ");
        //    //Following code gives distinct data values.
        //    DataView view = new DataView(EweDS.Tables[0]);
        //    DataTable TableNames = view.ToTable(true, "TableName");

        //    stringBuilder.Append(TableNames.Rows[0][0]);
        //    //Builds the JOIN part of the query.
        //    for (int i = 0; i < TableNames.Rows.Count - 1; i++)
        //    {
        //        if (i + 1 < TableNames.Rows.Count)
        //        {
        //            stringBuilder.Append(" INNER JOIN " + TableNames.Rows[i + 1]["TableName"]);
        //            stringBuilder.Append(" ON " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId =" + TableNames.Rows[i + 1]["TableName"] + ".GlobalRecordId");

        //        }
        //    }
        //    stringBuilder.Append(" INNER JOIN " + EweDS.Tables[0].Rows[0]["ViewTableName"] + " ON " + EweDS.Tables[0].Rows[0]["TableName"] + ".GlobalRecordId =" + EweDS.Tables[0].Rows[0]["ViewTableName"] + ".GlobalRecordId");

        //    if (SearchCriteria != null && SearchCriteria.Length > 0)
        //    {
        //        SearchCriteria = " WHERE " + SearchCriteria;
        //    }


        //    pagingQueryBuilder.Append("WITH CTE AS (" + stringBuilder.ToString() + SearchCriteria + ")");

        //    if (IsReadingResponseCount)
        //    {
        //        pagingQueryBuilder.Append(" SELECT COUNT(*) AS RESPONSECOUNT FROM CTE");
        //        //return pagingQueryBuilder.ToString();
        //    }
        //    else
        //    {
        //        pagingQueryBuilder.Append(" SELECT " + cteSelectBuilder.ToString() + " FROM CTE");
        //    }


        //    StringBuilder whereClause = new StringBuilder(" WHERE 1=1");

        //    //if (SearchCriteria.Length > 0)
        //    //{
        //    //    whereClause.Append(" WHERE " + SearchCriteria);
        //    //}
        //    //else
        //    //{
        //    //    whereClause.Append(" WHERE  1 = 1 ");
        //    //}


        //    pagingQueryBuilder.Append(whereClause);

        //    if (!IsReadingResponseCount)
        //    {
        //        pagingQueryBuilder.Append(" AND RowNumber between " + (((PageNumber * PageSize) - (PageSize)) + 1) + " AND " + ((PageNumber * (PageSize))));
        //        pagingQueryBuilder.Append(sortBuilder.ToString());
        //    }




        //    return pagingQueryBuilder.ToString();
        //}

        /// <summary>
        /// Builds SQL Select query by reading the columns, tablename from the EWE database.
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        private string BuildEI7Query(
            string FormId,
            string SortOrder,
            string Sortfield,
            string EI7Connectionstring,
            string SearchCriteria = "",
            bool IsReadingResponseCount = false,
            int PageSize = 1,
            int PageNumber = 1,
            bool IsChild = false,
            string ResponseId = "")
        {
            SqlConnection EweConnection = new SqlConnection(DataObjectFactory.EWEADOConnectionString);
            EweConnection.Open();

            SqlCommand EweCommand = new SqlCommand("usp_GetResponseFieldsInfo", EweConnection);//send formid for stored procedure to look for common columns between the two tables
            //Stored procedure that goes queries ResponseDisplaySettings and new table SurveyResonpseTranslate(skinny table) for a given FormId

            EweCommand.Parameters.Add("@FormId", SqlDbType.VarChar);
            EweCommand.Parameters["@FormId"].Value = FormId.Trim();

            EweCommand.CommandType = CommandType.StoredProcedure;
            //EweCommand.CreateParameter(  EweCommand.Parameters.Add(new SqlParameter("FormId"), FormId);



            SqlDataAdapter EweDataAdapter = new SqlDataAdapter(EweCommand);

            DataSet EweDS = new DataSet();

            try
            {
                EweDataAdapter.Fill(EweDS);
                EweConnection.Close();
            }
            catch (Exception ex)
            {
                EweConnection.Close();
                throw ex;
            }
            SqlConnection EI7Connection = new SqlConnection(EI7Connectionstring);

            EI7Connection.Open();

            SqlCommand EI7Command = new SqlCommand(" SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + EweDS.Tables[0].Rows[0][1] + "'", EI7Connection);
            object eI7CommandExecuteScalar;
            try
            {
                eI7CommandExecuteScalar = EI7Command.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw;
            }

            if (EweDS == null || EweDS.Tables.Count == 0 || EweDS.Tables[0].Rows.Count == 0
                || eI7CommandExecuteScalar == null)
            {
                EI7Connection.Close();
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder tableNameBuilder = new StringBuilder();
            StringBuilder pagingQueryBuilder = new StringBuilder();

            StringBuilder cteSelectBuilder = new StringBuilder();

            StringBuilder sortBuilder = new StringBuilder(" ORDER BY ");
            if (Sortfield != null && SortOrder != null)
            {
                sortBuilder.Append(Sortfield + " " + SortOrder);
            }
            else
            {
                //sortBuilder.Append(EweDS.Tables[0].Rows[0]["ColumnName"]);
                sortBuilder.Append(" LastSaveTime DESC "); //default sort on lastsavetime 
            }


            stringBuilder.Append(" SELECT ROW_NUMBER() OVER( " + sortBuilder.ToString() + ") RowNumber," + EweDS.Tables[0].Rows[0]["ViewTableName"] + ".LastSaveTime," + EweDS.Tables[0].Rows[0]["TableName"] + ".GlobalRecordId,");
            cteSelectBuilder.Append(" RowNumber, GlobalRecordId, LastSaveTime, ");
            // Builds the select part of the query.
            foreach (DataRow row in EweDS.Tables[0].Rows)
            {
                stringBuilder.Append(row["TableName"] + ".[" + row["ColumnName"] + "], ");
                cteSelectBuilder.Append("[" + row["ColumnName"] + "], ");

            }
            stringBuilder.Remove(stringBuilder.Length - 2, 1);
            cteSelectBuilder.Remove(cteSelectBuilder.Length - 2, 1);

            stringBuilder.Append(" FROM ");
            //Following code gives distinct data values.
            DataView view = new DataView(EweDS.Tables[0]);
            DataTable TableNames = view.ToTable(true, "TableName");

            stringBuilder.Append(TableNames.Rows[0][0]);
            //Builds the JOIN part of the query.
            for (int i = 0; i < TableNames.Rows.Count - 1; i++)
            {
                if (i + 1 < TableNames.Rows.Count)
                {
                    stringBuilder.Append(" INNER JOIN " + TableNames.Rows[i + 1]["TableName"]);
                    stringBuilder.Append(" ON " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId =" + TableNames.Rows[i + 1]["TableName"] + ".GlobalRecordId");

                }
            }
            stringBuilder.Append(" INNER JOIN " + EweDS.Tables[0].Rows[0]["ViewTableName"] + " ON " + EweDS.Tables[0].Rows[0]["TableName"] + ".GlobalRecordId =" + EweDS.Tables[0].Rows[0]["ViewTableName"] + ".GlobalRecordId");

            stringBuilder.Append(" WHERE RECSTATUS = 1 ");

            if (SearchCriteria != null && SearchCriteria.Length > 0)
            {
                stringBuilder.Append(" AND " + SearchCriteria);
            }

            if (IsChild)
            {
                stringBuilder.Append(" AND " + EweDS.Tables[0].Rows[0][4] + ".FKEY ='" + ResponseId + "'");

            }


            pagingQueryBuilder.Append("WITH CTE AS (" + stringBuilder.ToString() + ")");

            if (IsReadingResponseCount)
            {
                pagingQueryBuilder.Append(" SELECT COUNT(*) AS RESPONSECOUNT FROM CTE");
                //return pagingQueryBuilder.ToString();
            }
            else
            {
                pagingQueryBuilder.Append(" SELECT " + cteSelectBuilder.ToString() + " FROM CTE");
            }


            StringBuilder whereClause = new StringBuilder(" WHERE 1=1");

            pagingQueryBuilder.Append(whereClause);

            if (!IsReadingResponseCount && !IsChild)
            {
                pagingQueryBuilder.Append(" AND RowNumber between " + (((PageNumber * PageSize) - (PageSize)) + 1) + " AND " + ((PageNumber * (PageSize))));
                pagingQueryBuilder.Append(sortBuilder.ToString());
            }




            return pagingQueryBuilder.ToString();
        }
        /// <summary>
        /// Builds SQL Select query by reading the columns, tablename from the EWE database.
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        //private string BuildEI7ResponseQuery(string ResponseId, string SurveyId, string SortOrder, string Sortfield, string EI7Connectionstring, bool IsUsedForCount = false)
        //{
        //    SqlConnection EweConnection = new SqlConnection(DataObjectFactory.EWEADOConnectionString);
        //    EweConnection.Open();

        //    SqlCommand EweCommand = new SqlCommand("usp_GetResponseFieldsInfo", EweConnection);//send formid for stored procedure to look for common columns between the two tables
        //    //Stored procedure that goes queries ResponseDisplaySettings and new table SurveyResonpseTranslate(skinny table) for a given FormId

        //    EweCommand.Parameters.Add("@FormId", SqlDbType.VarChar);
        //    EweCommand.Parameters["@FormId"].Value = SurveyId.Trim();

        //    EweCommand.CommandType = CommandType.StoredProcedure;
        //    //EweCommand.CreateParameter(  EweCommand.Parameters.Add(new SqlParameter("FormId"), FormId);



        //    SqlDataAdapter EweDataAdapter = new SqlDataAdapter(EweCommand);

        //    DataSet EweDS = new DataSet();

        //    try
        //    {
        //        EweDataAdapter.Fill(EweDS);
        //        EweConnection.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        EweConnection.Close();
        //        throw ex;
        //    }
        //    SqlConnection EI7Connection = new SqlConnection(EI7Connectionstring);

        //    EI7Connection.Open();

        //    SqlCommand EI7Command = new SqlCommand(" SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + EweDS.Tables[0].Rows[0][1] + "'", EI7Connection);
        //    object eI7CommandExecuteScalar;
        //    try
        //    {
        //        eI7CommandExecuteScalar = EI7Command.ExecuteScalar();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }

        //    if (EweDS == null || EweDS.Tables.Count == 0 || EweDS.Tables[0].Rows.Count == 0
        //        || eI7CommandExecuteScalar == null)
        //    {
        //        EI7Connection.Close();
        //        return string.Empty;
        //    }

        //    StringBuilder stringBuilder = new StringBuilder();
        //    StringBuilder tableNameBuilder = new StringBuilder();
        //    StringBuilder sortBuilder = new StringBuilder(" ORDER BY ");
        //    if (IsUsedForCount)
        //    {
        //        stringBuilder.Append(" SELECT COUNT(*), ");
        //    }
        //    else
        //    {
        //        stringBuilder.Append(" SELECT " + EweDS.Tables[0].Rows[0][1] + ".GlobalRecordId,");
        //        if (Sortfield != null && SortOrder != null)
        //        {
        //            sortBuilder.Append(Sortfield + " " + SortOrder);
        //        }
        //        else
        //        {
        //            sortBuilder.Append(EweDS.Tables[0].Rows[0][1] + "." + EweDS.Tables[0].Rows[0][0]);
        //        }
        //        // Builds the select part of the query.
        //        foreach (DataRow row in EweDS.Tables[0].Rows)
        //        {
        //            stringBuilder.Append(row[1] + "." + row[0] + ", ");

        //        }
        //    }






        //    stringBuilder.Remove(stringBuilder.Length - 2, 1);

        //    stringBuilder.Append(" FROM ");
        //    //Following code gives distinct data values.
        //    DataView view = new DataView(EweDS.Tables[0]);
        //    DataTable TableNames = view.ToTable(true, "TableName");

        //    stringBuilder.Append(TableNames.Rows[0][0]);
        //    //Builds the JOIN part of the query.
        //    for (int i = 0; i < TableNames.Rows.Count - 1; i++)
        //    {
        //        if (i + 1 < TableNames.Rows.Count)
        //        {
        //            stringBuilder.Append(" INNER JOIN " + TableNames.Rows[i + 1][0]);
        //            stringBuilder.Append(" ON " + TableNames.Rows[0][0] + ".GlobalRecordId =" + TableNames.Rows[i + 1][0] + ".GlobalRecordId");

        //        }
        //    }

        //    stringBuilder.Append(" INNER JOIN " + EweDS.Tables[0].Rows[0][4] + " ON " + EweDS.Tables[0].Rows[0][1] + ".GlobalRecordId =" + EweDS.Tables[0].Rows[0][4] + ".GlobalRecordId");
        //    stringBuilder.Append(" WHERE " + EweDS.Tables[0].Rows[0][4] + ".FKEY ='" + ResponseId + "'");

        //    if (IsUsedForCount)
        //    {
        //        return stringBuilder.ToString();
        //    }


        //    return stringBuilder.Append(sortBuilder.ToString()).ToString();
        //}

        /// <summary>
        /// Builds SQL Select query by reading the columns, tablename from the EWE database.
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        private string BuildEI7ResponseAllFieldsQuery(string ResponseId, string SurveyId, string EI7Connectionstring)
        {
            SqlConnection EweConnection = new SqlConnection(DataObjectFactory.EWEADOConnectionString);
            EweConnection.Open();

            SqlCommand EweCommand = new SqlCommand("usp_GetResponseAllFieldsInfo", EweConnection);//Gets all the fields for given survey.

            EweCommand.Parameters.Add("@FormId", SqlDbType.VarChar);
            EweCommand.Parameters["@FormId"].Value = SurveyId.Trim();

            EweCommand.CommandType = CommandType.StoredProcedure;
            //EweCommand.CreateParameter(  EweCommand.Parameters.Add(new SqlParameter("FormId"), FormId);



            SqlDataAdapter EweDataAdapter = new SqlDataAdapter(EweCommand);

            DataSet EweDS = new DataSet();

            try
            {
                EweDataAdapter.Fill(EweDS);
                EweConnection.Close();
            }
            catch (Exception ex)
            {
                EweConnection.Close();
                throw ex;
            }
            SqlConnection EI7Connection = new SqlConnection(EI7Connectionstring);

            EI7Connection.Open();

            SqlCommand EI7Command = new SqlCommand(" SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + EweDS.Tables[0].Rows[0][1] + "'", EI7Connection);
            object eI7CommandExecuteScalar;
            try
            {
                eI7CommandExecuteScalar = EI7Command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (EweDS == null || EweDS.Tables.Count == 0 || EweDS.Tables[0].Rows.Count == 0
                || eI7CommandExecuteScalar == null)
            {
                EI7Connection.Close();
                return string.Empty;
            }

            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder tableNameBuilder = new StringBuilder();
            stringBuilder.Append(" SELECT " + EweDS.Tables[0].Rows[0]["TableName"] + ".GlobalRecordId,");



            // Builds the select part of the query.
            foreach (DataRow row in EweDS.Tables[0].Rows)
            {
                stringBuilder.Append(row["TableName"] + "." + row["FieldName"] + ", ");

            }
            stringBuilder.Remove(stringBuilder.Length - 2, 1);

            stringBuilder.Append(" FROM ");
            //Following code gives distinct data values.
            DataView view = new DataView(EweDS.Tables[0]);
            DataTable TableNames = view.ToTable(true, "TableName");

            stringBuilder.Append(TableNames.Rows[0]["TableName"]);
            //Builds the JOIN part of the query.
            for (int i = 0; i < TableNames.Rows.Count - 1; i++)
            {
                if (i + 1 < TableNames.Rows.Count)
                {
                    stringBuilder.Append(" INNER JOIN " + TableNames.Rows[i + 1]["TableName"]);
                    stringBuilder.Append(" ON " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId =" + TableNames.Rows[i + 1]["TableName"] + ".GlobalRecordId");

                }
            }

            //stringBuilder.Append(" INNER JOIN " + EweDS.Tables[0].Rows[0]["ViewTableName"] + " ON " + EweDS.Tables[0].Rows[0][1] + ".GlobalRecordId =" + EweDS.Tables[0].Rows[0]["ViewTableName"] + ".GlobalRecordId");
            //stringBuilder.Append(" WHERE " + EweDS.Tables[0].Rows[0]["ViewTableName"] + ".FKEY ='" + ResponseId + "'");
            stringBuilder.Append(" WHERE " + EweDS.Tables[0].Rows[0]["TableName"] + ".GlobalRecordId ='" + ResponseId + "'");

            return stringBuilder.ToString();
        }




        /// <summary>
        /// Validates if current form is Sql Project
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        private bool IsEISQLProject(string FormId)
        {
            //SqlConnection EweConnection = new SqlConnection(DataObjectFactory.EWEADOConnectionString);

            //EweConnection.Open();

            //SqlCommand EweCommand = new SqlCommand("usp_IsSQLProject", EweConnection);
            //EweCommand.CommandType = CommandType.StoredProcedure;
            //EweCommand.Parameters.Add("@FormId", SqlDbType.VarChar);
            //EweCommand.Parameters["@FormId"].Value = FormId;


            //SqlDataAdapter EweDataAdapter = new SqlDataAdapter(EweCommand);

            //bool IsSqlProj = false;
            //try
            //    {
            //    object issqlprj = EweDataAdapter.SelectCommand.ExecuteScalar();

            //    if (issqlprj != DBNull.Value)
            //        {
            //        IsSqlProj = Convert.ToBoolean(issqlprj);
            //        }


            //    EweConnection.Close();
            //    }
            //catch (Exception ex)
            //    {
            //    EweConnection.Close();
            //    throw ex;
            //    }

            bool IsSqlProj = false;
            Guid Id = new Guid(FormId);

            using (var Context = DataObjectFactory.CreateContext())
            {


                var Response = Context.SurveyMetaDatas.Single(x => x.SurveyId == Id);
                if (Response != null)
                {
                    IsSqlProj = (bool)Response.IsSQLProject;

                }

            }
            return IsSqlProj;
        }

        /// <summary>
        /// Reads connection string from Datasource table
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        private string ReadEI7DatabaseName(string FormId)
        {
            SqlConnection EweConnection = new SqlConnection(DataObjectFactory.EWEADOConnectionString);

            EweConnection.Open();

            SqlCommand EweCommand = new SqlCommand("usp_GetDatasourceConnectionString", EweConnection);
            EweCommand.CommandType = CommandType.StoredProcedure;
            EweCommand.Parameters.Add("@FormId", SqlDbType.VarChar);
            EweCommand.Parameters["@FormId"].Value = FormId;
            //EweCommand.Parameters["@FormId"].Value = FormId;

            SqlDataAdapter EweDataAdapter = new SqlDataAdapter(EweCommand);

            string ConnectionString;
            try
            {
                // EweDataAdapter.Fill(DSConnstr);
                ConnectionString = Convert.ToString(EweCommand.ExecuteScalar());
                EweConnection.Close();
            }
            catch (Exception ex)
            {
                EweConnection.Close();
                throw ex;
            }

            //ConnectionString = DSConnstr.Tables[0].Rows[0][0] + "";

            //return ConnectionString;

            return ConnectionString.Substring(ConnectionString.LastIndexOf('=') + 1);
        }
        public bool ISResponseExists(Guid ResponseId)
        {
            bool Exists = false;

            try
            {



                using (var Context = DataObjectFactory.CreateContext())
                {

                    IEnumerable<SurveyResponse> SurveyResponseList = Context.SurveyResponses.ToList().Where(x => x.ResponseId == ResponseId);
                    if (SurveyResponseList.Count() > 0)
                    {
                        Exists = true;
                    }

                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Exists;
        }

        public bool HasResponse(SurveyAnswerCriteria Criteria)
        {
            bool Exists = false;
            IsSqlProject = IsEISQLProject(Criteria.SurveyId);
            if (IsSqlProject)
            {
                string tableName = ReadEI7DatabaseName(Criteria.SurveyId);

                string EI7ConnectionString = DataObjectFactory.EWEADOConnectionString.Substring(0, DataObjectFactory.EWEADOConnectionString.LastIndexOf('=')) + "=" + tableName;

                SqlConnection EI7Connection = new SqlConnection(EI7ConnectionString);

                //string EI7Query = BuildEI7ResponseQuery(Criteria.SurveyAnswerIdList[0], Criteria.SurveyId, Criteria.SortOrder, Criteria.Sortfield, EI7ConnectionString, true);

                string EI7Query = BuildEI7Query(Criteria.SurveyId, Criteria.SortOrder, Criteria.Sortfield, EI7ConnectionString, "", true, 1, 1, true, Criteria.SurveyAnswerIdList[0]);

                SqlCommand EI7Command = new SqlCommand(EI7Query, EI7Connection);
                EI7Command.CommandType = CommandType.Text;


                EI7Connection.Open();

                try
                {
                    int count = (int)EI7Command.ExecuteScalar();

                    EI7Connection.Close();
                    if (count > 0)
                    {
                        Exists = true;
                    }


                }
                catch (Exception)
                {
                    EI7Connection.Close();
                    throw;
                }


            }
            else
            {
                try
                {



                    using (var Context = DataObjectFactory.CreateContext())
                    {

                        IEnumerable<SurveyResponse> SurveyResponseList = Context.SurveyResponses.ToList().Where(x => x.ResponseId == new Guid(Criteria.SurveyAnswerIdList[0]));
                        if (SurveyResponseList.Count() > 0)
                        {
                            Exists = true;
                        }

                    }

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            return Exists;
        }
        public int GetFormResponseCount(string FormId)
        {
            int ResponseCount = 0;

            //If SqlProject read responses from property SqlProjectResponsesCount.
            IsSqlProject = IsEISQLProject(FormId);
            if (IsSqlProject)
            {
                //ResponseCount = SqlProjectResponsesCount;


                string tableName = ReadEI7DatabaseName(FormId);

                string EI7ConnectionString = DataObjectFactory.EWEADOConnectionString.Substring(0, DataObjectFactory.EWEADOConnectionString.LastIndexOf('=')) + "=" + tableName;

                SqlConnection EI7Connection = new SqlConnection(EI7ConnectionString);

                string EI7Query = BuildEI7Query(FormId, null, null, EI7ConnectionString, "", true);

                SqlCommand EI7Command = new SqlCommand(EI7Query, EI7Connection);
                EI7Command.CommandType = CommandType.Text;

                EI7Connection.Open();

                try
                {
                    ResponseCount = (int)EI7Command.ExecuteScalar();
                    EI7Connection.Close();
                }
                catch (Exception)
                {
                    EI7Connection.Close();
                    throw;
                }
            }
            else
            {


                try
                {

                    Guid Id = new Guid(FormId);

                    using (var Context = DataObjectFactory.CreateContext())
                    {

                        IEnumerable<SurveyResponse> SurveyResponseList = Context.SurveyResponses.ToList().Where(x => x.SurveyId == Id && string.IsNullOrEmpty(x.ParentRecordId.ToString()) == true && x.StatusId > 1);
                        ResponseCount = SurveyResponseList.Count();

                    }

                }
                catch (Exception ex)
                {
                    throw (ex);
                }


            }
            return ResponseCount;


        }



        public SurveyResponseBO GetFormResponseByResponseId(string ResponseId)
        {

            SurveyResponseBO result = new SurveyResponseBO();

            try
            {

                Guid Id = new Guid(ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {


                    SurveyResponse Response = Context.SurveyResponses.ToList().Where(x => x.ResponseId == Id).First();
                    result = (Mapper.Map(Response));




                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }




            return result;
        }

        public string GetResponseParentId(string ResponseId)
        {

            SurveyResponseBO result = new SurveyResponseBO();

            try
            {

                Guid Id = new Guid(ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {


                    SurveyResponse Response = Context.SurveyResponses.ToList().Where(x => x.ResponseId == Id).First();
                    result = (Mapper.Map(Response));

                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (!string.IsNullOrEmpty(result.ParentRecordId))
            {
                return result.ParentRecordId;
            }
            else
            {
                return "";
            }

        }

        public SurveyResponseBO GetSingleResponse(string ResponseId)
        {
            SurveyResponseBO result = new SurveyResponseBO();

            try
            {

                Guid Id = new Guid(ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {


                    var Response = Context.SurveyResponses.Where(x => x.ResponseId == Id);//.First();
                    if (Response.Count() > 0)
                    {
                        result = (Mapper.Map(Response.First()));

                    }


                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }




            return result;

        }

        public List<SurveyResponseBO> GetResponsesHierarchyIdsByRootId(string RootId)
        {



            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            List<string> list = new List<string>();
            try
            {

                Guid Id = new Guid(RootId);

                using (var Context = DataObjectFactory.CreateContext())
                {

                    result = Mapper.Map(Context.SurveyResponses.Where(x => x.ResponseId == Id).Traverse(x => x.SurveyResponse1));



                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result;



        }

        public SurveyResponseBO GetFormResponseByParentRecordId(string ParentRecordId)
        {

            SurveyResponseBO result = new SurveyResponseBO();

            try
            {

                Guid Id = new Guid(ParentRecordId);

                using (var Context = DataObjectFactory.CreateContext())
                {


                    var Response = Context.SurveyResponses.ToList().Where(x => x.ParentRecordId == Id);
                    if (Response.Count() > 0)
                    {
                        result = (Mapper.Map(Response.Single()));

                    }


                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }




            return result;
        }

        public List<SurveyResponseBO> GetAncestorResponseIdsByChildId(string ChildId)
        {

            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            List<string> list = new List<string>();
            try
            {

                Guid Id = new Guid(ChildId);

                using (var Context = DataObjectFactory.CreateContext())
                {

                    result = Mapper.Map(Context.SurveyResponses.Where(x => x.ResponseId == Id).Traverse(x => Context.SurveyResponses.Where(y => x.RelateParentId == y.ResponseId)));



                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result;



        }

        public List<SurveyResponseBO> GetResponsesByRelatedFormId(string ResponseId, string SurveyId)
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();


            try
            {

                Guid RId = new Guid(ResponseId);
                Guid SId = new Guid(SurveyId);

                using (var Context = DataObjectFactory.CreateContext())
                {

                    result = Mapper.Map(Context.SurveyResponses.Where(x => x.RelateParentId == RId && x.SurveyId == SId)).OrderBy(x => x.DateCreated).ToList();

                }


            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return result;

        }


        public List<SurveyResponseBO> GetResponsesByRelatedFormId(string ResponseId, SurveyAnswerCriteria Criteria)
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            IsSqlProject = IsEISQLProject(Criteria.SurveyId);//Checks to see if current form is SqlProject

            if (IsSqlProject)
            {
                //make a connection to datasource table to read the connection string.
                //do a read to see which column belongs to which page/table.
                //do a read from ResponseDisplaySettings to read the column names. if for a given survey they dont exist 
                //read the first 5 columns from EI7 sql server database.

                string tableName = ReadEI7DatabaseName(Criteria.SurveyId);

                string EI7ConnectionString = DataObjectFactory.EWEADOConnectionString.Substring(0, DataObjectFactory.EWEADOConnectionString.LastIndexOf('=')) + "=" + tableName;

                SqlConnection EI7Connection = new SqlConnection(EI7ConnectionString);

                //string EI7Query = BuildEI7ResponseQuery(ResponseId, Criteria.SurveyId, Criteria.SortOrder, Criteria.Sortfield, EI7ConnectionString);

                string EI7Query = BuildEI7Query(Criteria.SurveyId, Criteria.SortOrder, Criteria.Sortfield, EI7ConnectionString, "", false, 1, 1, true, ResponseId);

                SqlCommand EI7Command = new SqlCommand(EI7Query, EI7Connection);
                EI7Command.CommandType = CommandType.Text;

                SqlDataAdapter EI7Adapter = new SqlDataAdapter(EI7Command);

                DataSet EI7DS = new DataSet();

                EI7Connection.Open();

                try
                {
                    EI7Adapter.Fill(EI7DS);
                    EI7Connection.Close();
                }
                catch (Exception)
                {
                    EI7Connection.Close();
                    throw;
                }


                // List<Dictionary<string, string>> DataRows = new List<Dictionary<string, string>>();

                for (int i = 0; i < EI7DS.Tables[0].Rows.Count; i++)
                {
                    Dictionary<string, string> rowDic = new Dictionary<string, string>();
                    SurveyResponseBO SurveyResponseBO = new Enter.Common.BusinessObject.SurveyResponseBO();
                    for (int j = 0; j < EI7DS.Tables[0].Columns.Count; j++)
                    {
                        rowDic.Add(EI7DS.Tables[0].Columns[j].ColumnName, EI7DS.Tables[0].Rows[i][j].ToString());
                    }
                    //.Skip((PageNumber - 1) * PageSize).Take(PageSize); ;
                    //IEnumerable<KeyValuePair<string, string>> temp = rowDic.AsEnumerable();
                    //temp.Skip((PageNumber - 1) * PageSize).Take(PageSize); 

                    SurveyResponseBO.SqlData = rowDic;
                    result.Add(SurveyResponseBO);
                }

                //SqlProjectResponsesCount = EI7DS.Tables[0].Rows.Count;

                //result = result.Skip((Criteria.PageNumber - 1) * Criteria.PageSize).Take(Criteria.PageSize).ToList();
                //SurveyResponseBO.SqlResponseDataBO.SqlData = DataRows;
            }
            else
            {

                try
                {

                    Guid RId = new Guid(ResponseId);
                    Guid SId = new Guid(Criteria.SurveyId);

                    using (var Context = DataObjectFactory.CreateContext())
                    {

                        result = Mapper.Map(Context.SurveyResponses.Where(x => x.RelateParentId == RId && x.SurveyId == SId)).OrderBy(x => x.DateCreated).ToList();

                    }


                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            return result;

        }


        /*
         * 
         * */

        public SurveyResponseBO GetResponseXml(string ResponseId)
        {


            SurveyResponseBO result = new SurveyResponseBO();

            try
            {

                Guid Id = new Guid(ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {


                    var Response = Context.ResponseXmls.Where(x => x.ResponseId == Id);
                    if (Response.Count() > 0)
                    {
                        result = (Mapper.Map(Response.Single()));

                    }

                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }




            return result;

        }


        public void DeleteResponseXml(ResponseXmlBO ResponseXmlBO)
        {


            Guid Id = new Guid(ResponseXmlBO.ResponseId);

            using (var Context = DataObjectFactory.CreateContext())
            {

                ResponseXml Response = Context.ResponseXmls.First(x => x.ResponseId == Id);


                Context.ResponseXmls.DeleteObject(Response);


                //Update Status
                var Query = from response in Context.SurveyResponses
                            where response.ResponseId == Id
                            select response;
                if (Query.Count() > 0)
                    {
                      var DataRow = Query.Single();
                      DataRow.StatusId = 2;
                      Context.SaveChanges();
                    }
            }

        }




        public void InsertResponseXml(ResponseXmlBO ResponseXmlBO)
        {

            try
            {
                Guid Id = new Guid(ResponseXmlBO.ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {
                    ResponseXml ResponseXml = Mapper.ToEF(ResponseXmlBO);
                    Context.AddToResponseXmls(ResponseXml);

                    //Update Status
                    var Query = from response in Context.SurveyResponses
                                where response.ResponseId == Id
                                select response;

                    var DataRow = Query.Single();
                    DataRow.StatusId = 1;
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }


        }



        public int GetFormResponseCount(SurveyAnswerCriteria Criteria)
        {
            int ResponseCount = 0;

            //If SqlProject read responses from property SqlProjectResponsesCount.
            IsSqlProject = IsEISQLProject(Criteria.SurveyId);
            if (IsSqlProject)
            {
                //ResponseCount = SqlProjectResponsesCount;


                string tableName = ReadEI7DatabaseName(Criteria.SurveyId);

                string EI7ConnectionString = DataObjectFactory.EWEADOConnectionString.Substring(0, DataObjectFactory.EWEADOConnectionString.LastIndexOf('=')) + "=" + tableName;

                SqlConnection EI7Connection = new SqlConnection(EI7ConnectionString);

                string EI7Query = BuildEI7Query(Criteria.SurveyId, Criteria.SortOrder, Criteria.Sortfield, EI7ConnectionString, Criteria.SearchCriteria, true);

                SqlCommand EI7Command = new SqlCommand(EI7Query, EI7Connection);
                EI7Command.CommandType = CommandType.Text;

                EI7Connection.Open();

                try
                {
                if (!string.IsNullOrEmpty(EI7Query))
                    ResponseCount = (int)EI7Command.ExecuteScalar();
                    EI7Connection.Close();
                }
                catch (Exception)
                {
                    EI7Connection.Close();
                    throw;
                }
            }
            else
            {


                try
                {

                    Guid Id = new Guid(Criteria.SurveyId);

                    using (var Context = DataObjectFactory.CreateContext())
                    {

                        IEnumerable<SurveyResponse> SurveyResponseList = Context.SurveyResponses.ToList().Where(x => x.SurveyId == Id && string.IsNullOrEmpty(x.ParentRecordId.ToString()) == true && x.StatusId > 1);
                        ResponseCount = SurveyResponseList.Count();

                    }

                }
                catch (Exception ex)
                {
                    throw (ex);
                }


            }
            return ResponseCount;



        }
        public void UpdateRecordStatus(SurveyResponseBO SurveyResponse)
            {

            try
                {
                List<SurveyResponseBO> result = new List<SurveyResponseBO>();
                Guid Id = new Guid(SurveyResponse.ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                    {
                    result = Mapper.Map(Context.SurveyResponses.Where(x => x.ResponseId == Id).OrderBy(x => x.DateCreated).Traverse(x => x.SurveyResponse1));
                    if (result.Count()>0)
                        {
                    foreach (var Obj in result)
                        {

                        if (!string.IsNullOrEmpty(Obj.ResponseId))
                            {
                            Guid NewId = new Guid(Obj.ResponseId);


                            Obj.Status = SurveyResponse.Status;
                            UpdateSurveyResponse(Obj);

                            //Context.usp_soft_delete_Epi7_record(new Guid(Obj.ResponseId), new Guid(Obj.SurveyId), Obj.Status);
                            //Context.SaveChanges();
                             
                            }


                        }
                        }else{

                        Context.usp_soft_delete_Epi7_record(new Guid(SurveyResponse.ResponseId), new Guid(SurveyResponse.SurveyId),false);
                        Context.SaveChanges();
                        }

                    }
                }
            catch (Exception ex)
                {
                throw (ex);
                }




            }

        }
}
