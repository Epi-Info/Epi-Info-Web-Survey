using System;
using System.Linq;
using System.Collections.Generic;

//using BusinessObjects;
//using DataObjects.EntityFramework.ModelMapper;
//using System.Linq.Dynamic;
using Epi.Web.Interfaces.DataInterfaces;
using Epi.Web.Common.BusinessObject;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Data;
using Epi.Web.Common.Extension;

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
            string OrganizationName = ""; int OrgId = 0;
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
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    OrgId = Convert.ToInt32(result[0].OrganizationId);
                    OrganizationName = Context.Organizations.FirstOrDefault(x => x.OrganizationId == OrgId).Organization1;
                    result[0].OrganizationName = OrganizationName;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
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
                    //var responseList1= Context.SurveyMetaDatas.Where(x =>  x.OrganizationId == OrganizationId );
                    var responseList1 = from r in Context.SurveyMetaDatas
                                   where r.OrganizationId == OrganizationId
                                        select new
                                   {
                                       SurveyName = r.SurveyName,
                                            SurveyId = r.SurveyId

                                        };

                    if (responseList1.Count() > 0)
                        {
                        //result = Mapper.Map(responseList1);
                        foreach (var item in responseList1) {
                            SurveyInfoBO SurveyInfoBO = new SurveyInfoBO();
                            SurveyInfoBO.SurveyId = item.SurveyId.ToString();
                            SurveyInfoBO.SurveyName = item.SurveyName;
                            result.Add(SurveyInfoBO);
                        }
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
                    if (!string.IsNullOrEmpty(SurveyInfo.XML)) {
                        DataRow.TemplateXML = SurveyInfo.XML;
                        DataRow.TemplateXMLSize = RemoveWhitespace(SurveyInfo.XML).Length;
                    }
                    DataRow.IntroductionText = SurveyInfo.IntroductionText;
                    DataRow.ExitText = SurveyInfo.ExitText;
                    DataRow.OrganizationName = SurveyInfo.OrganizationName;
                    DataRow.DepartmentName = SurveyInfo.DepartmentName;
                    DataRow.ClosingDate = SurveyInfo.ClosingDate;
                    DataRow.SurveyTypeId = SurveyInfo.SurveyType;
                    DataRow.UserPublishKey = SurveyInfo.UserPublishKey;
                    
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

        public List<SurveyInfoBO> GetChildInfoByParentId(string ParentFormId, int ViewId) 
            {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();
            try
                {

                Guid Id = new Guid(ParentFormId);

                    using (var Context = DataObjectFactory.CreateContext())
                        {
                        result.Add(Mapper.Map(Context.SurveyMetaDatas.FirstOrDefault(x => x.ParentId == Id && x.ViewId == ViewId)));
                        }
                    
                }
            catch (Exception ex)
                {
                throw (ex);
                }
            return result;
            }

        public SurveyInfoBO GetParentInfoByChildId(string ChildId)
        {
        SurveyInfoBO result = new SurveyInfoBO();
        try
            {

            Guid Id = new Guid(ChildId);

            using (var Context = DataObjectFactory.CreateContext())
                {
                result = Mapper.Map(Context.SurveyMetaDatas.FirstOrDefault(x => x.SurveyId == Id ));
                }

            }
        catch (Exception ex)
            {
            throw (ex);
            }
        return result;
        }

   public List<SurveyInfoBO> GetFormsHierarchyIdsByRootId(string RootId)
        {

      List<SurveyInfoBO> result = new List<SurveyInfoBO>();
         
            List<string> list = new List<string>();
        try
            {

            Guid Id = new Guid(RootId);

            using (var Context = DataObjectFactory.CreateContext())
                {
                    IQueryable<SurveyMetaData> Query = Context.SurveyMetaDatas.Where(x => x.SurveyId == Id).Traverse(x => x.SurveyMetaData1).AsQueryable();
                   result = Mapper.Map(Query);


 
                }

            }
        catch (Exception ex)
            {
            throw (ex);
            }
        return result;

        }


   public void InsertFormdefaultSettings(string FormId, bool IsSqlProject, List<string> ControlsNameList)
       {
      
       try
           {
             //Delete old columns
               using (var Context = DataObjectFactory.CreateContext())
               {
                   Guid Id = new Guid(FormId);
                   IQueryable<ResponseDisplaySetting> ColumnList = Context.ResponseDisplaySettings.Where(x => x.FormId == Id);

                   
                   foreach (var item in ColumnList)
                   {
                       Context.ResponseDisplaySettings.DeleteObject(item);
                   }
                   Context.SaveChanges();
               }
           // Adding new columns
           List<string> ColumnNames = new List<string>();
           if (!IsSqlProject)
               {
                  ColumnNames = MetaDaTaColumnNames();
               }
           else
               {
                   ColumnNames = ControlsNameList;
               }
           int i = 1;
           foreach (string Column in ColumnNames)
               {
              
               using (var Context = DataObjectFactory.CreateContext())
                   {

                   ResponseDisplaySetting SettingEntity = Mapper.Map(FormId, i, Column);

                   Context.AddToResponseDisplaySettings(SettingEntity);

                   Context.SaveChanges();
                   
                   }
               i++;
               }
           }
       catch (Exception ex)
           {
           throw (ex);
           }
       }
   public void UpdateParentId(string SurveyId ,int ViewId , string ParentId)
       {
       try
           {
           Guid Id = new Guid(SurveyId);
           Guid PId = new Guid(ParentId);

           //Update Survey
           using (var Context = DataObjectFactory.CreateContext())
               {
               var Query = from Form in Context.SurveyMetaDatas
                           where Form.SurveyId == Id && Form.ViewId == ViewId
                           select Form;

               var DataRow = Query.Single();
               DataRow.ParentId = PId;
               Context.SaveChanges();
               }

           }
       catch (Exception ex)
           {
           throw (ex);
           }
       }
   private static List<string> MetaDaTaColumnNames()
       {

       List<string> columns = new List<string>();
       columns.Add("_UserEmail");
       columns.Add("_DateUpdated");
       columns.Add("_DateCreated");
       // columns.Add("IsDraftMode");
       columns.Add("_Mode");
       return columns;

       }


   public void InsertConnectionString(DbConnectionStringBO ConnectionString)
       { 
        try
           {
             
              
               using (var Context = DataObjectFactory.CreateContext())
                   { 
                   Context.usp_AddDatasource(ConnectionString.DatasourceServerName, ConnectionString.DatabaseType, ConnectionString.InitialCatalog, ConnectionString.PersistSecurityInfo, ConnectionString.DatabaseUserID, ConnectionString.SurveyId, ConnectionString.Password);

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
            //var Context = DataObjectFactory.CreateContext();
            //string eweAdostring = Context.Connection.Substring(Context.Connection.ToLower().IndexOf("data source="), Context.Connection.Substring(Context.Connection.ToLower().IndexOf("data source=")).IndexOf(";"));

            //string epiDBstring = pRequestMessage.DBConnectionString.Substring(0, pRequestMessage.DBConnectionString.IndexOf(";"));

            //if (eweAdostring.ToLower() != epiDBstring.ToLower())
            //{
            //    pRequestMessage.IsSqlProject = false;
            //}
        }
       public void InsertSourceTable(string SourcetableXml, string SourcetableName, string FormId)
       {

            
           string ConnectionString = DataObjectFactory._ADOConnectionString;
           SqlConnection Connection = new SqlConnection(ConnectionString);
           
         
             Connection.Open();
           
              try
               {
                   Guid Id = new Guid(FormId);
                 




                SqlCommand Command = new SqlCommand("INSERT INTO Sourcetables VALUES(@SourceTableName , @FormId ,@SourcetableXml)", Connection);

                Command.Parameters.AddWithValue("SourceTableName", SourcetableName);
                Command.Parameters.AddWithValue("FormId", Id);
                Command.Parameters.AddWithValue("SourceTableXml", SourcetableXml);

               



                Command.ExecuteNonQuery();
                 
               

               
                      
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
           //SqlCommand Command = new SqlCommand();
           //Command.Connection = Connection;
           try
           {
               Guid Id = new Guid(FormId);
                //Command.CommandType = CommandType.Text;

                // var Sourcetable_Xml = SourcetableXml.Replace("'", "''");
                // Command.CommandText = "UPDATE Sourcetables  SET SourceTableXml = @Sourcetable_Xml   where FormId = @Form_Id And  SourcetableName =  @Source_table_Name  ";


                // var param = new SqlParameter("Form_Id", SqlDbType.VarChar);
                // param.Value = FormId;
                // Command.Parameters.Add(param);

                // var param1 = new SqlParameter("Source_table_Name", SqlDbType.VarChar);
                // param1.Value = FormId;
                // Command.Parameters.Add(param1);
                // var param2 = new SqlParameter("Sourcetable_Xml", SqlDbType.VarChar);
                // param2.Value = Sourcetable_Xml;
                // Command.Parameters.Add(param2);

                SqlCommand Command = new SqlCommand("UPDATE Sourcetables  SET SourceTableXml = @SourceTableXml   where FormId = @FormId And  SourcetableName =  @SourceTableName ", Connection);

                Command.Parameters.AddWithValue("SourceTableName", SourcetableName);
                Command.Parameters.AddWithValue("FormId", Id);
                Command.Parameters.AddWithValue("SourceTableXml", SourcetableXml);


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
              Command.CommandText = "select * from Sourcetables  where  FormId = @Form_Id"  ;
              var param = new SqlParameter("Form_Id", SqlDbType.VarChar);
              param.Value = FormId;
              Command.Parameters.Add(param);
             // Command.ExecuteNonQuery();
                SqlDataAdapter Adapter = new SqlDataAdapter( Command);
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
       public bool  TableExist(string FormId, string Tablename)
       {
           List<SourceTableBO> result = new List<SourceTableBO>();
           string ConnectionString = DataObjectFactory._ADOConnectionString;
           SqlConnection Connection = new SqlConnection(ConnectionString);
           Connection.Open();
           bool TableExist = false;
           SqlCommand Command = new SqlCommand();
           Command.Connection = Connection;
           try
           {
               Command.CommandType = CommandType.Text;
               Command.CommandText = "select * from Sourcetables  where  FormId ='" + FormId + "' And SourceTableName='" + Tablename + "'";
               // Command.ExecuteNonQuery();
               SqlDataAdapter Adapter = new SqlDataAdapter(Command);
               DataSet DS = new DataSet();
               Adapter.Fill(DS);
               if (DS.Tables.Count > 0)
               {
                   if (DS.Tables[0].Rows.Count>0)
               {
                   TableExist =  true;
               }
               else {
                    TableExist=  false;

               }
               }
               Connection.Close();
               
           }
           catch (Exception)
           {
               Connection.Close();

           }
           return TableExist;
            
       }
    }
}
