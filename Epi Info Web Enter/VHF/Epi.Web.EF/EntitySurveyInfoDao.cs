using System;
using System.Linq;
using System.Collections.Generic;

//using BusinessObjects;
//using DataObjects.EntityFramework.ModelMapper;
//using System.Linq.Dynamic;
using Epi.Web.Enter.Interfaces.DataInterfaces;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Extension;
using System.Data.Objects;
using System.Data.SqlClient;

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
            //using (var Context = DataObjectFactory.CreateContext())
            //{
               
            //    OrganizationId =  Context.Organizations.FirstOrDefault(x => x.OrganizationKey == pOrganizationKey).OrganizationId;
            //}
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
                                //responseList.Add(Context.SurveyMetaDatas.FirstOrDefault(x => x.SurveyId == Id && x.OrganizationId == OrganizationId));
                                responseList.Add(Context.SurveyMetaDatas.FirstOrDefault(x => x.SurveyId == Id ));
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
                       string OrgKey = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(SurveyInfo.OrganizationKey.ToString());
                       OrganizationId = ContextOrg.Organizations.FirstOrDefault(x => x.OrganizationKey == OrgKey).OrganizationId;
                   }

                   SurveyInfo.TemplateXMLSize = RemoveWhitespace(SurveyInfo.XML).Length;
                   SurveyInfo.DateCreated = DateTime.Now;

                   

                   var SurveyMetaDataEntity = Mapper.Map(SurveyInfo);
                   User User = Context.Users.FirstOrDefault(x => x.UserID == SurveyInfo.OwnerId);
                   SurveyMetaDataEntity.Users.Add(User);

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
                    //var Query = from response in Context.SurveyMetaDatas
                    //            where response.SurveyId == Id
                    //            select response;

                    //var DataRow = Query.Single();
                    //DataRow = Mapper.ToEF(SurveyInfo);

                SurveyMetaData Row = Context.SurveyMetaDatas.First(x=>x.SurveyId == Id);
                Row.IsSQLProject = SurveyInfo.IsSqlProject;
                Row.TemplateXML = SurveyInfo.XML;
                Row.IsDraftMode = SurveyInfo.IsDraftMode;
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

                result = Mapper.Map(Context.SurveyMetaDatas.Where(x => x.SurveyId == Id).Traverse(x => x.SurveyMetaData1));


 
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

               Context.AddToEIDatasources(DataRow);

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
