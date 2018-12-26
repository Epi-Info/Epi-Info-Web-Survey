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
using Epi.Web.Common.Extension;
using System.Data.SqlClient;
using System.Data;

namespace Epi.Web.EF
{
    /// <summary>
    /// Entity Framework implementation of the ISurveyResponseDao interface.
    /// </summary>
    public class EntitySurveyResponseDao : ISurveyResponseDao
    {
        private int sqlProjectResponsesCount;
        int DataAccessRuleId;
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
                      
                        // New client Epi info version 7.2
                        if (pStatusId == 0) // All 2,3,and 4 if available
                        {
                                if (PageSize != -1 && PageNumber != -1)
                                {
                                   var responseList0 = Context.SurveyResponses.Where(x => x.SurveyId == Id && x.IsDraftMode == pIsDraftMode)
                                    .OrderBy(x => x.DateCompleted).Skip((PageNumber - 1) * PageSize).Take(PageSize);
                                    responseList = responseList0.ToList();
                                }
                                else {

                                    responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id && x.IsDraftMode == pIsDraftMode)
                                    .OrderBy(x => x.DateCompleted).ToList();

                                }
                        }
                        if (pStatusId == 3) // Only 3
                        {
                                if (PageSize != -1 && PageNumber != -1)
                                {
                                    var responseList3 = Context.SurveyResponses.Where(x => x.SurveyId == Id && x.StatusId == pStatusId && x.IsDraftMode == pIsDraftMode)
                                    .OrderBy(x => x.DateCompleted).Take(PageSize);
                                    responseList = responseList3.ToList();
                                }
                                else {
                                    responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id && x.StatusId == pStatusId && x.IsDraftMode == pIsDraftMode)
                                   .OrderBy(x => x.DateCompleted).ToList();
                                }


                        }
                       if (pStatusId == 4) //   3 and 4
                       {
                                if (PageSize != -1 && PageNumber != -1)
                                {
                                    var responseList34 = Context.SurveyResponses.Where(x => x.SurveyId == Id && (x.StatusId == pStatusId || x.StatusId == 3) && x.IsDraftMode == pIsDraftMode)
                                    .OrderBy(x => x.DateCompleted).Skip((PageNumber - 1) * PageSize).Take(PageSize);
                                    responseList = responseList34.ToList();
                                }
                                else {
                                    responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id && (x.StatusId == pStatusId || x.StatusId == 3) && x.IsDraftMode == pIsDraftMode)
                                    .OrderBy(x => x.DateCompleted).ToList();
                                }
                       }
                        // Old client Epi info version 7.1.5.2
                       if (pStatusId == -1) // All 2,3,and 4 if available
                       {
                           responseList = Context.SurveyResponses.Where(x => x.SurveyId == Id ).OrderBy(x => x.DateCompleted).ToList();
                       }

                    }
                    
             if (pDateCompleted > DateTime.MinValue)
            {
                List<SurveyResponse> dateList = new List<SurveyResponse>();

                //dateList.AddRange(responseList.Where(x => x.DateCompleted.Value.Month ==  pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
                dateList.AddRange(responseList.Where(x =>x.DateCompleted !=null && x.DateCompleted.Value.Month == pDateCompleted.Month && x.DateCompleted.Value.Year == pDateCompleted.Year && x.DateCompleted.Value.Day == pDateCompleted.Day));
                responseList = dateList;
            }

           
          
                
             

            if(PageSize!=-1 && PageNumber != -1){
             result = Mapper.Map(responseList);
                 
                if (pStatusId == 3) // Only 3
                {
                    result = result.Take(PageSize);
                }
                else {

                    result = result.Skip((PageNumber - 1) * PageSize).Take(PageSize);

                }
                foreach(var item in result)
                    {
                      List<SurveyResponseBO> ResponsesHierarchy = this.GetResponsesHierarchyIdsByRootId(item.ResponseId.ToString());
                      item.ResponseHierarchyIds = ResponsesHierarchy;
                      Finalresult.Add(item);
                
                    }
                  
                }
            else
                {

               
               Finalresult = Mapper.Map(responseList);

               
               
                }


                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
               
            }

            return Finalresult;
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
                try
                {
                    SurveyResponseEntity.RecordSourceId = Context.lk_RecordSource.Where(u => u.RecordSource == "EIWS").Select(u => u.RecordSourceId).SingleOrDefault();
                }
                catch(Exception)
                {

                }
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
        /// Inserts a new SurveyResponse via API. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyResponse object will contain the new identifier.
        /// </remarks>  
        /// <param name="SurveyResponse">SurveyResponse.</param>
        public void InsertSurveyResponseApi(SurveyResponseBO SurveyResponse)
        {
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
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

        public bool HasResponse(SurveyAnswerCriteria Criteria)
        {
            bool Exists = false;
            IsSqlProject = IsEISQLProject(Criteria.SurveyId);
            if (IsSqlProject)
            {
                string tableName = ReadEI7DatabaseName(Criteria.SurveyId);

                string EI7ConnectionString = DataObjectFactory._ADOConnectionString.Substring(0, DataObjectFactory._ADOConnectionString.LastIndexOf('=')) + "=" + tableName;

                SqlConnection EI7Connection = new SqlConnection(EI7ConnectionString);

                //string EI7Query = BuildEI7ResponseQuery(Criteria.SurveyAnswerIdList[0], Criteria.SurveyId, Criteria.SortOrder, Criteria.Sortfield, EI7ConnectionString, true);

                //string EI7Query = BuildEI7Query(Criteria.SurveyId, Criteria.SortOrder, Criteria.Sortfield, EI7ConnectionString, "", true, 1, 1, true, Criteria.SurveyAnswerIdList[0],Criteria.UserId,Criteria.IsShareable,Criteria.UserOrganizationId);
                string EI7Query = BuildEI7Query(Criteria.SurveyId, null, null, EI7ConnectionString, "", true, 1, 1, true, Criteria.SurveyAnswerIdList[0], 0, false, Criteria.UserOrganizationId);

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

                        IQueryable<SurveyResponse> SurveyResponseList = Context.SurveyResponses.Where(x => x.ResponseId == new Guid(Criteria.SurveyAnswerIdList[0]));
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
        private bool IsEISQLProject(string FormId)
        {
          

            bool IsSqlProj = false;
            Guid Id = new Guid(FormId);

            using (var Context = DataObjectFactory.CreateContext())
            {



                var Response = from r in Context.SurveyMetaDatas
                               where r.SurveyId == Id
                               select new
                               {
                                   IsSQLProject = r.IsSQLProject,

                               };

                if (Response != null)
                {
                    IsSqlProj = (bool)Response.First().IsSQLProject;

                }

            }
            return IsSqlProj;
        }
        private string ReadEI7DatabaseName(string FormId)
        {
            SqlConnection EweConnection = new SqlConnection(DataObjectFactory._ADOConnectionString);

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
            string ResponseId = "",
            int UserId = -1,
            bool IsShareable = false,
            int UserOrgId = -1,
            int DataAccessRulrId = -1

            )
        {
            SqlConnection EweConnection = new SqlConnection(DataObjectFactory._ADOConnectionString);
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
            SqlCommand EI7Command;
            try
            {
                EI7Command = new SqlCommand(" SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + EweDS.Tables[0].Rows[0][1] + "'", EI7Connection);
            }
            catch (Exception ex)
            {

                throw ex;
            }

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

            //EW-236
            //  stringBuilder.Append(" SELECT ROW_NUMBER() OVER( " + sortBuilder.ToString() + ") RowNumber," + EweDS.Tables[0].Rows[0]["ViewTableName"] + ".LastSaveTime," + EweDS.Tables[0].Rows[0]["TableName"] + ".GlobalRecordId,");
            stringBuilder.Append(" SELECT ROW_NUMBER() OVER( " + sortBuilder.ToString() + ") RowNumber," + EweDS.Tables[0].Rows[0]["ViewTableName"] + ".LastSaveTime," + EweDS.Tables[0].Rows[0]["ViewTableName"] + ".GlobalRecordId,");
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
                    //EW-236
                    //stringBuilder.Append(" INNER JOIN " + TableNames.Rows[i + 1]["TableName"]);
                    stringBuilder.Append(" FULL JOIN " + TableNames.Rows[i + 1]["TableName"]);
                    stringBuilder.Append(" ON " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId =" + TableNames.Rows[i + 1]["TableName"] + ".GlobalRecordId");

                }
            }
            //EW-236
            //stringBuilder.Append(" INNER JOIN " + EweDS.Tables[0].Rows[0]["ViewTableName"] + " ON " + EweDS.Tables[0].Rows[0]["TableName"] + ".GlobalRecordId =" + EweDS.Tables[0].Rows[0]["ViewTableName"] + ".GlobalRecordId");

            stringBuilder.Append(" FULL JOIN " + EweDS.Tables[0].Rows[0]["ViewTableName"] + " ON " + EweDS.Tables[0].Rows[0]["TableName"] + ".GlobalRecordId =" + EweDS.Tables[0].Rows[0]["ViewTableName"] + ".GlobalRecordId");

            stringBuilder.Append(" WHERE RECSTATUS = 1 ");
            //  User filter Start 

            // if (ConfigurationManager.AppSettings["FilterByUser"].ToUpper() == "TRUE" && UserId != -1)
            //{
            //    stringBuilder.Append(" AND " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId in ");
            //    stringBuilder.Append(" (Select SurveyResponse.ResponseId from [" + EweConnection.Database + "].[dbo].SurveyResponse");
            //    stringBuilder.Append(" INNER JOIN [" + EweConnection.Database + "].[dbo].SurveyResponseUser on SurveyResponse.ResponseId =SurveyResponseUser.ResponseId");
            //    stringBuilder.Append(" Where " + "UserId =" + UserId +")");
            //}

            if (IsShareable)
            {
                //if (DataAccessRuleId != -1)
                //{
                //    stringBuilder.Append(" AND " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId in ");
                //    stringBuilder.Append(" (Select SurveyResponse.ResponseId from [" + EweConnection.Database + "].[dbo].SurveyResponse");
                //    //stringBuilder.Append(" INNER JOIN [" + EweConnection.Database + "].[dbo].SurveyResponseUser on SurveyResponse.ResponseId =SurveyResponseUser.ResponseId and SurveyResponse.SurveyId ='" + FormId+"'");
                //    //stringBuilder.Append(" INNER JOIN [" + EweConnection.Database + "].[dbo].UserOrganization on UserOrganization.UserID = SurveyResponseUser.UserId");
                //    //stringBuilder.Append(" Where " + "UserOrganization.OrganizationID =" + UserOrgId + ")");
                //    stringBuilder.Append(" Where " + "OrganizationId =" + UserOrgId + " And SurveyId ='" + FormId + "')");
                //}
                //else {

                //    stringBuilder.Append(" AND " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId in ");
                //    stringBuilder.Append(" (Select SurveyResponse.ResponseId from [" + EweConnection.Database + "].[dbo].SurveyResponse");
                //    stringBuilder.Append(" Where  SurveyId ='" + FormId + "')");

                //   }

                //Shareable
                switch (DataAccessRuleId)
                {
                    case 1: //   Organization users can only access the data of there organization
                        stringBuilder.Append(" AND " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId in ");
                        stringBuilder.Append(" (Select SurveyResponse.ResponseId from [" + EweConnection.Database + "].[dbo].SurveyResponse");
                        stringBuilder.Append(" Where " + "OrganizationId =" + UserOrgId + " And SurveyId ='" + FormId + "')");
                        break;
                    case 2:    // All users in host organization will have access to all data of all organizations  

                        // get All the users of Host organization
                        var Context = DataObjectFactory.CreateContext();

                        Guid FormGuid = new Guid(FormId);
                        var HostOrg = Context.SurveyMetaDatas.Where(x => x.SurveyId == FormGuid).SingleOrDefault().OrganizationId;

                        var Users = Context.UserOrganizations.Where(x => x.OrganizationID == HostOrg && x.Active == true).ToList();

                        int Count = Users.Where(x => x.UserID == UserId).Count();
                        if (Count > 0)
                        {
                            stringBuilder.Append(" AND " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId in ");
                            stringBuilder.Append(" (Select SurveyResponse.ResponseId from [" + EweConnection.Database + "].[dbo].SurveyResponse");
                            stringBuilder.Append(" Where  SurveyId ='" + FormId + "')");
                        }
                        else
                        {
                            stringBuilder.Append(" AND " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId in ");
                            stringBuilder.Append(" (Select SurveyResponse.ResponseId from [" + EweConnection.Database + "].[dbo].SurveyResponse");
                            stringBuilder.Append(" Where " + "OrganizationId =" + UserOrgId + " And SurveyId ='" + FormId + "')");

                        }
                        break;
                    case 3: // All users of all organizations can access all data 
                        stringBuilder.Append(" AND " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId in ");
                        stringBuilder.Append(" (Select SurveyResponse.ResponseId from [" + EweConnection.Database + "].[dbo].SurveyResponse");
                        stringBuilder.Append(" Where  SurveyId ='" + FormId + "')");
                        break;
                    default:
                        stringBuilder.Append(" AND " + TableNames.Rows[0]["TableName"] + ".GlobalRecordId in ");
                        stringBuilder.Append(" (Select SurveyResponse.ResponseId from [" + EweConnection.Database + "].[dbo].SurveyResponse");
                        stringBuilder.Append(" Where " + "OrganizationId =" + UserOrgId + " And SurveyId ='" + FormId + "')");
                        break;



                }
            }

            //User filter End 

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
        public void InsertChildSurveyResponse(SurveyResponseBO SurveyResponse)
        {



            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    SurveyResponse SurveyResponseEntity = Mapper.ToEF(SurveyResponse);
                    //User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyResponse.UserId);
                    //SurveyResponseEntity.Users.Add(User);
                    Context.AddToSurveyResponses(SurveyResponseEntity);

                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
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

        public string GetResponseParentId(string ResponseId)
        {

            SurveyResponseBO result = new SurveyResponseBO();

            try
            {

                Guid Id = new Guid(ResponseId);

                using (var Context = DataObjectFactory.CreateContext())
                {


                    SurveyResponse Response = Context.SurveyResponses.Where(x => x.ResponseId == Id).First();
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

        public List<SurveyResponseBO> GetResponsesHierarchyIdsByRootId(string RootId)
        {



            List<SurveyResponseBO> result = new List<SurveyResponseBO>();

            List<string> list = new List<string>();
            try
            {

                Guid Id = new Guid(RootId);

                using (var Context = DataObjectFactory.CreateContext())
                {
                    IQueryable<SurveyResponse> Query = Context.SurveyResponses.Where(x => x.ResponseId == Id).Traverse(x => x.SurveyResponse1).AsQueryable();
                    result = Mapper.Map(Query);



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


                    var Response = Context.SurveyResponses.Where(x => x.ParentRecordId == Id);
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
                    IQueryable<SurveyResponse> Query = Context.SurveyResponses.Where(x => x.ResponseId == Id).Traverse(x => Context.SurveyResponses.Where(y => x.RelateParentId == y.ResponseId)).AsQueryable();

                    result = Mapper.Map(Query);



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


        List<SurveyResponseBO> ISurveyResponseDao.GetFormResponseByFormId(string FormId, int PageNumber, int PageSize)
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            try
            {

                Guid Id = new Guid(FormId);

                using (var Context = DataObjectFactory.CreateContext())
                {

                    IQueryable<SurveyResponse> SurveyResponseList = Context.SurveyResponses.Where(x => x.SurveyId == Id
                        //&& string.IsNullOrEmpty(x.ParentRecordId.ToString()) == true 
                        //&& string.IsNullOrEmpty(x.RelateParentId.ToString()) == true 
                        && (x.ParentRecordId == null || x.ParentRecordId == Guid.Empty)
                        && (x.RelateParentId == null || x.RelateParentId == Guid.Empty)
                        && x.StatusId > 1).OrderByDescending(x => x.DateUpdated);

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

          //  DataAccessRuleId = this.GetDataAccessRule(criteria.SurveyId, criteria.UserId);

            if (IsSqlProject)
            {
                //make a connection to datasource table to read the connection string.
                //do a read to see which column belongs to which page/table.
                //do a read from ResponseDisplaySettings to read the column names. if for a given survey they dont exist 
                //read the first 5 columns from EI7 sql server database.

                string tableName = ReadEI7DatabaseName(criteria.SurveyId);

                string EI7ConnectionString = DataObjectFactory._ADOConnectionString.Substring(0, DataObjectFactory._ADOConnectionString.LastIndexOf('=')) + "=" + tableName;


                SqlConnection EI7Connection = new SqlConnection(EI7ConnectionString);
                string EI7Query;
                if (!criteria.GetAllColumns)
                {
                    EI7Query = BuildEI7Query(criteria.SurveyId, criteria.SortOrder, criteria.Sortfield, EI7ConnectionString, criteria.SearchCriteria, false, criteria.PageSize, criteria.PageNumber, criteria.IsChild, criteria.ParentResponseId, criteria.UserId,false, criteria.UserOrganizationId, DataAccessRuleId);

                }
                else
                {
                    EI7Query = BuildEI7ResponseAllFieldsQuery(criteria.SurveyAnswerIdList[0].ToString(), criteria.SurveyId, EI7ConnectionString, criteria.UserId);
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
                    SurveyResponseBO SurveyResponseBO = new Common.BusinessObject.SurveyResponseBO();
                    for (int j = 0; j < EI7DS.Tables[0].Columns.Count; j++)
                    {
                        rowDic.Add(EI7DS.Tables[0].Columns[j].ColumnName, EI7DS.Tables[0].Rows[i][j].ToString());
                    }
                    //.Skip((PageNumber - 1) * PageSize).Take(PageSize); ;
                    //IEnumerable<KeyValuePair<string, string>> temp = rowDic.AsEnumerable();
                    //temp.Skip((PageNumber - 1) * PageSize).Take(PageSize); 

                    //SurveyResponseBO.SqlData = rowDic;
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

                        IQueryable<SurveyResponse> SurveyResponseList;
                        

                            //SurveyResponseList = Context.SurveyResponses.Where(x => x.SurveyId == Id
                            // && string.IsNullOrEmpty(x.ParentRecordId.ToString()) == true
                            //    && string.IsNullOrEmpty(x.RelateParentId.ToString()) == true
                            //    && x.StatusId >= 1).OrderByDescending(x => x.DateUpdated); 

                            SurveyResponseList = Context.SurveyResponses.Where(x => x.SurveyId == Id
                            && (x.ParentRecordId == null || x.ParentRecordId == Guid.Empty)
                               && (x.RelateParentId == null || x.RelateParentId == Guid.Empty)
                               && x.StatusId >= 1).OrderByDescending(x => x.DateUpdated);



                       


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


        public int GetFormResponseCount(string FormId)
        {
            int ResponseCount = 0;

            //If SqlProject read responses from property SqlProjectResponsesCount.
            IsSqlProject = IsEISQLProject(FormId);
            if (IsSqlProject)
            {
                //ResponseCount = SqlProjectResponsesCount;


                string tableName = ReadEI7DatabaseName(FormId);

                string EI7ConnectionString = DataObjectFactory._ADOConnectionString.Substring(0, DataObjectFactory._ADOConnectionString.LastIndexOf('=')) + "=" + tableName;

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

                        IQueryable<SurveyResponse> SurveyResponseList = Context.SurveyResponses.Where(x => x.SurveyId == Id
                            // && string.IsNullOrEmpty(x.ParentRecordId.ToString()) == true 
                            && (x.ParentRecordId == null || x.ParentRecordId == Guid.Empty)

                            && x.StatusId > 1);
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

        public int GetFormResponseCount(SurveyAnswerCriteria Criteria)
        {
            int ResponseCount = 0;

            //If SqlProject read responses from property SqlProjectResponsesCount.
            IsSqlProject = IsEISQLProject(Criteria.SurveyId);
            if (IsSqlProject)
            {
                //ResponseCount = SqlProjectResponsesCount;


                string tableName = ReadEI7DatabaseName(Criteria.SurveyId);

                string EI7ConnectionString = DataObjectFactory._ADOConnectionString.Substring(0, DataObjectFactory._ADOConnectionString.LastIndexOf('=')) + "=" + tableName;

                SqlConnection EI7Connection = new SqlConnection(EI7ConnectionString);

                string EI7Query = BuildEI7Query(Criteria.SurveyId, Criteria.SortOrder, Criteria.Sortfield, EI7ConnectionString, Criteria.SearchCriteria, true, -1, -1, false, "", Criteria.UserId,false, Criteria.UserOrganizationId, DataAccessRuleId);

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
                    IQueryable<SurveyResponse> SurveyResponseList;
                    using (var Context = DataObjectFactory.CreateContext())
                    {
                       

                            //   SurveyResponseList = Context.SurveyResponses.Where(x => x.SurveyId == Id && string.IsNullOrEmpty(x.ParentRecordId.ToString()) == true && x.StatusId >= 1);
                            SurveyResponseList = Context.SurveyResponses.Where(x => x.SurveyId == Id
                                && (x.ParentRecordId == null || x.ParentRecordId == Guid.Empty)
                                && x.StatusId >= 1);



                        
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


        private string BuildEI7ResponseAllFieldsQuery(string ResponseId, string SurveyId, string EI7Connectionstring, int UserId)
        {
            SqlConnection EweConnection = new SqlConnection(DataObjectFactory._ADOConnectionString);
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
        /// Inserts a new ErrorLog. 
        /// </summary>
        /// <remarks>       
        /// </remarks>  
        /// <param name="pValue">ErrorText.</param>
        public void InsertErrorLog(Dictionary<string, string> pValue)
        {            
            try
            {
                Guid surveyId=Guid.Empty, responseId=Guid.Empty; StringBuilder ErrText = new StringBuilder();
                foreach (KeyValuePair<string, string> kvp in pValue)
                {
                    if (kvp.Key == "SurveyId")
                    {
                        surveyId = new Guid(kvp.Value.ToString());
                    }
                    else if (kvp.Key == "ResponseId")
                    {
                        responseId = new Guid(kvp.Value.ToString());
                    }
                    else
                        ErrText.Append(" " + kvp.Key + " " + kvp.Value + ". ");
                }
                using (var Context = DataObjectFactory.CreateContext())
                {
                    Context.usp_log_to_errorlog(surveyId,responseId, "SurveyAPI Error", ErrText.ToString(), null,null,null,null,null,null,null,null);                                       
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
