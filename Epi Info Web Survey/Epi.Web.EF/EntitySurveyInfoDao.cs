using System;
using System.Linq;
using System.Collections.Generic;

//using BusinessObjects;
//using DataObjects.EntityFramework.ModelMapper;
//using System.Linq.Dynamic;
using Epi.Web.Interfaces.DataInterfaces;
using Epi.Web.Common.BusinessObject;
using System.Data.SqlClient;
using System.Data;

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
                try
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
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            else
            {
                try
                {
                    using (var Context = DataObjectFactory.CreateContext())
                    {
                        result = Mapper.Map(Context.SurveyMetaDatas.ToList());
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            // remove the items to skip
            // remove the items after the page size
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


        /// <summary>
        /// Gets SurveyInfo based on criteria
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>SurveyInfo.</returns>
        public List<SurveyInfoBO> GetSurveyInfo(List<string> SurveyInfoIdList, DateTime pClosingDate,string pOrganizationKey ,int pSurveyType = -1, int PageNumber = -1, int PageSize = -1)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();

            List<SurveyMetaData> responseList = new List<SurveyMetaData>();

            int  OrganizationId =0;
            try {
            using (var Context = DataObjectFactory.CreateContext())
            {
               
                OrganizationId =  Context.Organizations.FirstOrDefault(x => x.OrganizationKey == pOrganizationKey).OrganizationId;
            }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            if (SurveyInfoIdList.Count > 0)
            {
                foreach (string surveyInfoId in SurveyInfoIdList.Distinct())
                {
                    Guid Id = new Guid(surveyInfoId);
                    try{
                            using (var Context = DataObjectFactory.CreateContext())
                            {
                                responseList.Add(Context.SurveyMetaDatas.FirstOrDefault(x => x.SurveyId == Id && x.OrganizationId == OrganizationId));
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
                using (var Context = DataObjectFactory.CreateContext())
                {
                    responseList = Context.SurveyMetaDatas.ToList();
                  
                }
            }


            if (responseList.Count > 0 && responseList[0] != null)
            {

                if (pSurveyType > -1)
                {
                    List<SurveyMetaData> statusList = new List<SurveyMetaData>();
                    statusList.AddRange(responseList.Where(x => x.SurveyTypeId == pSurveyType));
                    responseList = statusList;
                }

                if (OrganizationId > 0)
                {
                    List<SurveyMetaData> OIdList = new List<SurveyMetaData>();
                    OIdList.AddRange(responseList.Where(x => x.OrganizationId == OrganizationId));
                    responseList = OIdList;

                }

                if (pClosingDate != null)
                {
                    if (pClosingDate > DateTime.MinValue)
                    {
                        List<SurveyMetaData> dateList = new List<SurveyMetaData>();

                        dateList.AddRange(responseList.Where(x => x.ClosingDate.Month >= pClosingDate.Month && x.ClosingDate.Year >= pClosingDate.Year && x.ClosingDate.Day >= pClosingDate.Day));
                        responseList = dateList;
                    }
                }
                result = Mapper.Map(responseList);

                // remove the items to skip
                // remove the items after the page size
                if (PageNumber > 0 && PageSize > 0)
                {
                    result.Sort(CompareByDateCreated);

                    if (PageNumber * PageSize - PageSize > 0)
                    {
                        result.RemoveRange(0, PageSize);
                    }

                    if (PageNumber * PageSize < result.Count)
                    {
                        result.RemoveRange(PageNumber * PageSize, result.Count - PageNumber * PageSize);
                    }
                }
            }
            return result;
        }

        public int GetOrganizationId(string OrgKey) {

            int OrganizationId = -1;
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {

                    var Query = (from response in Context.Organizations
                                 where response.OrganizationKey == OrgKey
                                 select response).SingleOrDefault();

                    if (Query != null)
                    {
                        OrganizationId = Query.OrganizationId;
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return OrganizationId;
        
        }

       
        public List<SurveyInfoBO> GetAllSurveysByOrgKey(string Okey)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();

            List<SurveyMetaData> responseList = new List<SurveyMetaData>();

            int OrganizationId = 0;
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {

                    var Query = (from response in Context.Organizations
                                 where response.OrganizationKey == Okey
                                 select response).SingleOrDefault();

                    if (Query != null)
                    {
                        OrganizationId = Query.OrganizationId;
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

           
                try
                {
                    
                    using (var Context = DataObjectFactory.CreateContext())
                    {
                       responseList= Context.SurveyMetaDatas.Where(x =>  x.OrganizationId == OrganizationId ).ToList();
                       if (responseList.Count() > 0 && responseList[0] != null)
                        {
                            result = Mapper.Map(responseList);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
             

            return result;
        }
        public List<SurveyInfoBO> GetSurveyInfoByOrgKeyAndPublishKey(string SurveyId, string Okey, Guid publishKey)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();

            List<SurveyMetaData> responseList = new List<SurveyMetaData>();

            int OrganizationId = 0;
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {

                    var Query = (from response in Context.Organizations
                                 where response.OrganizationKey == Okey
                                 select response).SingleOrDefault();

                    if (Query != null) {
                        OrganizationId = Query.OrganizationId;
                    }
                
                }
            }
           catch (Exception ex)
           {
               throw (ex);
           }
           
           if (!string.IsNullOrEmpty(SurveyId))
           {
                try
                {
                    Guid Id = new Guid(SurveyId);
                    using (var Context = DataObjectFactory.CreateContext())
                    {
                        responseList.Add(Context.SurveyMetaDatas.FirstOrDefault(x => x.SurveyId == Id && x.OrganizationId == OrganizationId && x.UserPublishKey == publishKey));
                        if (responseList[0] != null)
                        {
                            result = Mapper.Map(responseList);
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


        public List<SurveyInfoBO> GetSurveyInfoByOrgKey(string SurveyId, string Okey)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();

            List<SurveyMetaData> responseList = new List<SurveyMetaData>();

            int OrganizationId = 0;
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {

                    var Query = (from response in Context.Organizations
                                 where response.OrganizationKey == Okey
                                 select response).SingleOrDefault();

                    if (Query != null)
                    {
                        OrganizationId = Query.OrganizationId;
                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            if (!string.IsNullOrEmpty(SurveyId))
            {
                try
                {
                    Guid Id = new Guid(SurveyId);
                    using (var Context = DataObjectFactory.CreateContext())
                    {
                        responseList.Add(Context.SurveyMetaDatas.FirstOrDefault(x => x.SurveyId == Id && x.OrganizationId == OrganizationId));
                        if (responseList[0] != null)
                        {
                            result = Mapper.Map(responseList);
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
        /// Inserts a new SurveyInfo. 
        /// </summary>
        /// <remarks>
        /// Following insert, SurveyInfo object will contain the new identifier.
        /// </remarks>  
        /// <param name="SurveyInfo">SurveyInfo.</param>
        public  void InsertSurveyInfo(SurveyInfoBO SurveyInfo)
        {
           int OrganizationId = 0;
           try
           {
               using (var Context = DataObjectFactory.CreateContext())
               {

                   //retrieve OrganizationId based on OrganizationKey
                   using (var ContextOrg = DataObjectFactory.CreateContext())
                   {
                       string OrgKey = Epi.Web.Common.Security.Cryptography.Encrypt(SurveyInfo.OrganizationKey.ToString());
                       OrganizationId = ContextOrg.Organizations.FirstOrDefault(x => x.OrganizationKey == OrgKey).OrganizationId;
                   }

                   SurveyInfo.TemplateXMLSize = RemoveWhitespace(SurveyInfo.XML).Length;
                   SurveyInfo.DateCreated = DateTime.Now;
                   SurveyInfo.LastUpdate = DateTime.Now;
                   SurveyInfo.IsSqlProject = SurveyInfo.IsSqlProject;                   
                   var SurveyMetaDataEntity = Mapper.Map(SurveyInfo);
                   SurveyMetaDataEntity.OrganizationId = OrganizationId;
                   Context.AddToSurveyMetaDatas(SurveyMetaDataEntity);

                   Context.SaveChanges();
               }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Updates a SurveyInfo.
        /// </summary>
        /// <param name="SurveyInfo">SurveyInfo.</param>
        public void UpdateSurveyInfo(SurveyInfoBO SurveyInfo)
        { 
            try
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
                    DataRow.TemplateXMLSize = RemoveWhitespace(SurveyInfo.XML).Length;
                    DataRow.IsDraftMode = SurveyInfo.IsDraftMode;
                    DataRow.StartDate = SurveyInfo.StartDate;
                    DataRow.LastUpdate = DateTime.Now;
                    DataRow.IsSQLProject = SurveyInfo.IsSqlProject;

                    Context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw (ex);
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

        /// <summary>
        /// Gets SurveyInfo Size Data on a list of ids
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>PageInfoBO.</returns>
        public List<SurveyInfoBO> GetSurveySizeInfo(List<string> SurveyInfoIdList,int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {
            List<SurveyInfoBO> resultRows = GetSurveyInfo(SurveyInfoIdList, PageNumber, PageSize);
            return resultRows;
        }


        /// <summary>
        /// Gets SurveyInfo Size Data based on criteria
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>PageInfoBO.</returns>
        public List<SurveyInfoBO> GetSurveySizeInfo(List<string> SurveyInfoIdList, DateTime pClosingDate, string Okey,  int pSurveyType = -1, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {
            List<SurveyInfoBO> resultRows =  GetSurveyInfo(SurveyInfoIdList, pClosingDate,Okey, pSurveyType, PageNumber, PageSize);
            return resultRows;
        }

        private static int CompareByDateCreated(SurveyInfoBO x, SurveyInfoBO y)
        {
            return x.DateCreated.CompareTo(y.DateCreated);
        }

        public static string RemoveWhitespace(string xml)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@">\s*<");
            xml = regex.Replace(xml, "><");

            return xml.Trim();
        }

        public void InsertConnectionString(DbConnectionStringBO ConnectionString)
        {
            try
            {


                using (var Context = DataObjectFactory.CreateContext())
                {
                    //Context.usp_AddDatasource(ConnectionString.DatasourceServerName, ConnectionString.DatabaseType, ConnectionString.InitialCatalog, ConnectionString.PersistSecurityInfo, ConnectionString.DatabaseUserID, ConnectionString.SurveyId, ConnectionString.Password);

                    //Context.SaveChanges();
                    Context.AddToEIDatasources(Mapper.Map(ConnectionString));
                    Context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public void UpdateConnectionString(DbConnectionStringBO ConnectionString)
        {
            try
            {


                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from DataSource in Context.EIDatasources
                                where DataSource.SurveyId == ConnectionString.SurveyId
                                select DataSource;

                    var DataRow = Query.Single();
                    DataRow = Mapper.Map(ConnectionString);

                    

                    Context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void ValidateServername(SurveyInfoBO pRequestMessage)
        {
            var Context = DataObjectFactory.CreateContext();
            string eweAdostring = Context.Connection.ConnectionString.Substring(Context.Connection.ConnectionString.ToLower().IndexOf("data source="), Context.Connection.ConnectionString.Substring(Context.Connection.ConnectionString.ToLower().IndexOf("data source=")).IndexOf(";"));

            string epiDBstring = pRequestMessage.DBConnectionString.Substring(0, pRequestMessage.DBConnectionString.IndexOf(";"));

            if (eweAdostring.ToLower() != epiDBstring.ToLower())
            {
                pRequestMessage.IsSqlProject = false;
            }
        }
       public void InsertSourceTable(string SourcetableXml, string SourcetableName, string FormId)
       {

            
           string ConnectionString = DataObjectFactory._ADOConnectionString;
           SqlConnection Connection = new SqlConnection(ConnectionString);
           
         
             Connection.Open();
            // SqlCommand Command = new SqlCommand(Query, EWEConnection);
                SqlCommand Command = new SqlCommand();
                Command.Connection = Connection;
              try
               {
                   Guid Id = new Guid(FormId);
                Command.CommandType = CommandType.Text;
                //Command.CommandText = "Insert into Sourcetables (SourceTableName, FormId,SourceTableXml) values ('@SourceTableName','@FormId','@SourceTableXml')";
                //Command.Parameters.AddWithValue("SourceTableName", SourcetableName);
                //Command.Parameters.AddWithValue("FormId", Id);
                //Command.Parameters.AddWithValue("SourceTableXml", SourcetableXml);


              Command.CommandText = "Insert into Sourcetables (SourceTableName, FormId,SourceTableXml) values ('" + SourcetableName + "','" + FormId + "','" + SourcetableXml.Replace("'","''")  + "')";
               
                Command.ExecuteNonQuery();
                //SqlDataAdapter  Adapter = new SqlDataAdapter( Command);

               // DataSet  DS = new DataSet();

               

               
                      
                     Connection.Close();
                }
                catch (Exception)
                {
                    Connection.Close();
                    
                }

       
       }
       public void UpdateSourceTable(string SourcetableXml, string SourcetableName, string FormId)
       {
           string ConnectionString = DataObjectFactory._ADOConnectionString;
           SqlConnection Connection = new SqlConnection(ConnectionString);


           Connection.Open();
           // SqlCommand Command = new SqlCommand(Query, EWEConnection);
           SqlCommand Command = new SqlCommand();
           Command.Connection = Connection;
           try
           {
               Guid Id = new Guid(FormId);
               Command.CommandType = CommandType.Text;


               Command.CommandText = "UPDATE Sourcetables  SET SourceTableXml ='" + SourcetableXml.Replace("'", "''") + "'  where FormId =" + "'" + FormId + "' And  SourcetableName='" + SourcetableName + "'";

               Command.ExecuteNonQuery();
                

               Connection.Close();
           }
           catch (Exception)
           {
               Connection.Close();

           }
       
       
       }
       public List<SourceTableBO> GetSourceTables(string FormId)
       {
           List<SourceTableBO> result = new List<SourceTableBO>();
           string ConnectionString = DataObjectFactory._ADOConnectionString;
           SqlConnection Connection = new SqlConnection(ConnectionString);


           Connection.Open();
          
           SqlCommand Command = new SqlCommand();
           Command.Connection = Connection;
           try
           {
              Command.CommandType = CommandType.Text;
              Command.CommandText = "select * from Sourcetables  where  FormId ='" + FormId+"'";
             // Command.ExecuteNonQuery();
              SqlDataAdapter  Adapter = new SqlDataAdapter( Command);
              DataSet  DS = new DataSet();
              Adapter.Fill(DS);
               if(DS.Tables.Count>0){
              result = Mapper.MapToSourceTableBO(DS.Tables[0]);
               }
              Connection.Close();
           }
           catch (Exception)
           {
               Connection.Close();

           }

           return result;
       }
    }
}
