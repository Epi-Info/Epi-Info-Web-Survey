using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.DTO;

namespace Epi.Web.EF
{
    /// <summary>
    /// Maps Entity Framework entities to business objects and vice versa.
    /// </summary>
    public static class Mapper
    {
        /// <summary>
        /// Maps SurveyMetaData entity to SurveyInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A SurveyInfoBO business object.</returns>
        public static SurveyInfoBO Map(SurveyMetaData entity)
        {
            SurveyInfoBO result =  new SurveyInfoBO();
            
                result.SurveyId = entity.SurveyId.ToString();
                result.SurveyName = entity.SurveyName;
                result.SurveyNumber = entity.SurveyNumber;
                result.XML = entity.TemplateXML;
                result.IntroductionText = entity.IntroductionText;
                result.ExitText = entity.ExitText;
                result.OrganizationName = entity.OrganizationName;
                result.DepartmentName = entity.DepartmentName;
                result.ClosingDate = entity.ClosingDate;
                result.TemplateXMLSize = (long) entity.TemplateXMLSize;
                result.DateCreated = entity.DateCreated;
                result.IsDraftMode = entity.IsDraftMode;
                result.StartDate = entity.StartDate;
            result.IsSqlProject = (bool)entity.IsSQLProject;
            result.OrganizationId = entity.OrganizationId;
            // result.OwnerId = entity.OwnerId;
            if (entity.UserPublishKey != null)
            {
                // result.UserPublishKey = (Guid)entity.UserPublishKey.Value;
                result.UserPublishKey = entity.UserPublishKey;
            }
            result.SurveyType = entity.SurveyTypeId;
            result.ParentId = entity.ParentId.ToString();
            if (entity.ViewId != null)
            {
                result.ViewId = (int)entity.ViewId;
            }
            //result. = (bool)entity.ShowAllRecords;
           // result.IsShareable = (bool)entity.IsShareable;
            return result;
        }
        public static List<SurveyResponseBO> Map(IQueryable<SurveyResponse> entities)
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            foreach (SurveyResponse surveyResponse in entities)
            {
                result.Add(Map(surveyResponse));
            }

            return result;
        }

        public static List<SurveyInfoBO> Map(IQueryable<SurveyMetaData> iQueryable)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();
            foreach (SurveyMetaData Obj in iQueryable)
            {
                result.Add(Map(Obj));

            }
            return result;
        }
        public static CacheDependencyBO MapDependency(SurveyMetaData entity)
        {
            CacheDependencyBO cacheDependencyBO = new CacheDependencyBO();

            cacheDependencyBO.SurveyId = entity.SurveyId.ToString();

            if (entity.LastUpdate != null)
            { 
                cacheDependencyBO.LastUpdate = (DateTime)entity.LastUpdate;
            }

            return cacheDependencyBO;
        }

        public static List<SurveyInfoBO> Map(List<SurveyMetaData> entities)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();
            foreach (SurveyMetaData surveyMetaData in entities)
            {
                result.Add(Map(surveyMetaData));
            }

            return result;
        }

        public static void Map(SurveyMetaData entity, out CacheDependencyBO cacheDependencyBO)
        {
            cacheDependencyBO = new CacheDependencyBO();

            cacheDependencyBO.SurveyId = entity.SurveyId.ToString();

            if (entity.LastUpdate == null)
            {
                entity.LastUpdate = entity.DateCreated;
            }

            cacheDependencyBO.LastUpdate = (DateTime)entity.LastUpdate;
        }

        public static void Map(List<SurveyMetaData> entities, out List<CacheDependencyBO> list)
        {
            list = new List<CacheDependencyBO>();

            foreach (SurveyMetaData surveyMetaData in entities)
            {
                CacheDependencyBO cacheDependencyBO = new CacheDependencyBO();
                Map(surveyMetaData, out cacheDependencyBO);
                list.Add(cacheDependencyBO);
            }
        }

        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyMetaData entity.
        /// </summary>
        /// <param name="businessobject">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyMetaData entity.</returns>
        public static SurveyMetaData Map(SurveyInfoBO businessobject)
        {
            SurveyMetaData SurveyMetaData = new SurveyMetaData();

            SurveyMetaData.SurveyId = new Guid(businessobject.SurveyId);
            SurveyMetaData.SurveyName = businessobject.SurveyName;
            SurveyMetaData.SurveyNumber = businessobject.SurveyNumber;
            SurveyMetaData.TemplateXML = businessobject.XML;
            SurveyMetaData.IntroductionText = businessobject.IntroductionText;
            SurveyMetaData.ExitText = businessobject.ExitText;
            SurveyMetaData.OrganizationName = businessobject.OrganizationName;
            SurveyMetaData.DepartmentName = businessobject.DepartmentName;
            SurveyMetaData.ClosingDate = businessobject.ClosingDate;
            SurveyMetaData.UserPublishKey = businessobject.UserPublishKey;
            SurveyMetaData.SurveyTypeId = businessobject.SurveyType;
            SurveyMetaData.TemplateXMLSize = businessobject.TemplateXMLSize;
            SurveyMetaData.DateCreated = businessobject.DateCreated;
            SurveyMetaData.IsDraftMode = businessobject.IsDraftMode;
            SurveyMetaData.StartDate = businessobject.StartDate;
           // SurveyMetaData.OwnerId = businessobject.OwnerId;
            SurveyMetaData.ViewId = businessobject.ViewId;
            SurveyMetaData.IsSQLProject = businessobject.IsSqlProject;
           // SurveyMetaData.IsShareable = businessobject.IsShareable;
           // SurveyMetaData.DataAccessRuleId = businessobject.DataAccessRuleId;
          //  SurveyMetaData.DataAccessRuleId = businessobject.;
            if (!string.IsNullOrEmpty(businessobject.ParentId))
            {
                SurveyMetaData.ParentId = new Guid(businessobject.ParentId);
            }
            return SurveyMetaData;
        }

        /// <summary>
        /// Maps SurveyMetaData entity to SurveyInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A SurveyInfoBO business object.</returns>
        public static SurveyResponseBO Map(SurveyAnswerDTO entity)
        {
            return new SurveyResponseBO
            {
                SurveyId = entity.SurveyId.ToString(),
                ResponseId = entity.ResponseId.ToString(),
                XML = entity.XML,
                Status = entity.Status,
                UserPublishKey = entity.UserPublishKey,
                DateUpdated = entity.DateUpdated,
                DateCompleted = entity.DateCompleted
            };
        }

        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyMetaData entity.
        /// </summary>
        /// <param name="businessobject">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyMetaData entity.</returns>
        public static SurveyAnswerDTO Map(SurveyResponseBO businessobject)
        {
            return new SurveyAnswerDTO
            {
                SurveyId = businessobject.SurveyId,
                ResponseId = businessobject.ResponseId,
                XML = businessobject.XML,
                Status = businessobject.Status,
                UserPublishKey = businessobject.UserPublishKey,
                DateUpdated = businessobject.DateUpdated,
                DateCompleted = businessobject.DateCompleted

            };
        }

        /// <summary>
        /// Maps SurveyMetaData entity to SurveyInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A SurveyInfoBO business object.</returns>
        //internal static SurveyResponseBO Map(SurveyResponse entity, User User = null, int LastActiveUseerId = -1)
        //{

        //    SurveyResponseBO SurveyResponseBO = new SurveyResponseBO();
        //    SurveyResponseBO.SurveyId = entity.SurveyId.ToString();
        //    SurveyResponseBO.ResponseId = entity.ResponseId.ToString();
        //    SurveyResponseBO.XML = entity.ResponseXML;
        //    SurveyResponseBO.Status = entity.StatusId;
        //    SurveyResponseBO.DateUpdated = entity.DateUpdated;
        //    SurveyResponseBO.DateCompleted = entity.DateCompleted;
        //    SurveyResponseBO.TemplateXMLSize = (long)entity.ResponseXMLSize;
        //    SurveyResponseBO.DateCreated = entity.DateCreated;
        //    SurveyResponseBO.IsDraftMode = entity.IsDraftMode;
        //    SurveyResponseBO.IsLocked = entity.IsLocked;
        //    SurveyResponseBO.LastActiveUserId = LastActiveUseerId;
        //    if (entity.SurveyMetaData != null)
        //    {
        //        SurveyResponseBO.ViewId = (int)entity.SurveyMetaData.ViewId;
        //    }
        //    if (entity.ParentRecordId != null)
        //    {
        //        SurveyResponseBO.ParentRecordId = entity.ParentRecordId.ToString();
        //    }
        //    if (entity.RelateParentId != null)
        //    {
        //        SurveyResponseBO.RelateParentId = entity.RelateParentId.ToString();
        //    }
        //    if (User != null)
        //    {
        //        SurveyResponseBO.UserEmail = User.EmailAddress;
        //    }

        //    return SurveyResponseBO;


        //}

        public static List<SurveyResponseBO> Map(List<SurveyResponse> entities)
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            foreach (SurveyResponse surveyResponse in entities)
            {
                result.Add(Map(surveyResponse));
            }

            return result;
        }
        public static OrganizationBO Map(Organization entity)
        {
            return new OrganizationBO
            {
                Organization = entity.Organization1,
                IsEnabled = entity.IsEnabled,
                OrganizationKey = entity.OrganizationKey,
                OrganizationId = entity.OrganizationId

            };
        }
        public static OrganizationBO Map(string Organization)
        {
            return new OrganizationBO
            {
                Organization = Organization
                 


            };
        }
        public static AdminBO MapAdminEmail(string AdminEmail)
            {
            return new AdminBO
            {
                AdminEmail = AdminEmail



            };
            }
        public static Organization ToEF(OrganizationBO pBo)
        {
            return new Organization
            {
                Organization1 = pBo.Organization,
                IsEnabled = pBo.IsEnabled,
                OrganizationKey = pBo.OrganizationKey,


            };
        }

        public static Admin ToEF(AdminBO pBo)
            {
            Guid AdminId = Guid.NewGuid();
            //int id;
           // int.TryParse(pBo.OrganizationId, out id);
                    return new Admin
                    {
                         AdminEmail = pBo.AdminEmail,
                         AdminId = AdminId,
                         IsActive = pBo.IsActive,
                         Notify = false,
                         OrganizationId = pBo.OrganizationId,
                         FirstName = pBo.FirstName,
                         LastName = pBo.LastName,
                         PhoneNumber = pBo.PhoneNumber,
                       //  AddressId = pBo.AddressId,
                    };
            }
        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyMetaData entity.
        /// </summary>
        /// <param name="businessobject">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyMetaData entity.</returns>
        internal static SurveyResponse ToEF(SurveyResponseBO pBO,int OrgId = -1)
        {

            SurveyResponse SurveyResponse = new SurveyResponse();
            Guid RelateParentId = Guid.Empty;
            if (!string.IsNullOrEmpty(pBO.RelateParentId))
            {
                RelateParentId = new Guid(pBO.RelateParentId);
            }
            Guid ParentRecordId = Guid.Empty;
            if (!string.IsNullOrEmpty(pBO.ParentRecordId))
            {
                ParentRecordId = new Guid(pBO.ParentRecordId);
            }
            SurveyResponse.SurveyId = new Guid(pBO.SurveyId);
            SurveyResponse.ResponseId = new Guid(pBO.ResponseId);
            SurveyResponse.ResponseXML = pBO.XML;
            SurveyResponse.StatusId = pBO.Status;
            SurveyResponse.ResponseXMLSize = pBO.TemplateXMLSize;
            SurveyResponse.DateUpdated = pBO.DateUpdated;
            SurveyResponse.DateCompleted = pBO.DateCompleted;
            SurveyResponse.DateCreated = pBO.DateCreated;
            SurveyResponse.IsDraftMode = pBO.IsDraftMode;
            SurveyResponse.RecordSourceId = pBO.RecrodSourceId;
            if (!string.IsNullOrEmpty(pBO.RelateParentId) && RelateParentId != Guid.Empty)
            {
                SurveyResponse.RelateParentId = new Guid(pBO.RelateParentId);
            }
            if (!string.IsNullOrEmpty(pBO.ParentRecordId) && ParentRecordId != Guid.Empty)
            {
                SurveyResponse.ParentRecordId = new Guid(pBO.ParentRecordId);
            }
            if (OrgId != -1)
            {
                SurveyResponse.OrganizationId = OrgId;
            }
            return SurveyResponse;
        }
        public static UserAuthenticationResponseBO ToAuthenticationResponseBO(UserAuthenticationRequestBO AuthenticationRequestBO)
        {


            return new UserAuthenticationResponseBO
            {
                PassCode = AuthenticationRequestBO.PassCode,

            };
        
        }

        public static StateBO Map(State Row)
            {
            StateBO StateBO = new StateBO();
            StateBO.StateCode = Row.StateCode;
            StateBO.StateName = Row.StateName;
            StateBO.StateId = Row.StateProvinceId;
            return StateBO;
            }

        public static   Address ToAddressEF(AdminBO Admin)
            {
            return new Address
            {

                AddressLine1 = Admin.AdressLine1,
                AddressLine2 = Admin.AdressLine2,
                City = Admin.City,
                StateProvinceId = Admin.StateId,
                PostalCode = Admin.Zip,
                AdminId = Admin.AdminId
            };
            }
        public static EIDatasource Map(DbConnectionStringBO ConnectionString)
        {
            EIDatasource Datasource = new EIDatasource();
            Datasource.DatabaseType = ConnectionString.DatabaseType;
            Datasource.DatabaseUserID = ConnectionString.DatabaseUserID;
            Datasource.DatasourceID = ConnectionString.DatasourceID;
            Datasource.DatasourceServerName = ConnectionString.DatasourceServerName;
            Datasource.InitialCatalog = ConnectionString.InitialCatalog;
            Datasource.Password = ConnectionString.Password;
            Datasource.SurveyId = ConnectionString.SurveyId;
            Datasource.PersistSecurityInfo = ConnectionString.PersistSecurityInfo;
            return Datasource;
        }
        public static List<SourceTableBO> MapToSourceTableBO(System.Data.DataTable dataTable)
        {
            List<SourceTableBO> List = new List<SourceTableBO>();
          

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                 SourceTableBO SourceTableBO = new SourceTableBO();
                 SourceTableBO.TableName = dataTable.Rows[i][0].ToString();
                 SourceTableBO.TableXml = dataTable.Rows[i][2].ToString();
                 


                List.Add(SourceTableBO);
            }
            return List;
            
        }
        public static SurveyResponseBO Map(ResponseXml ResponseXml)
        {
            SurveyResponseBO SurveyResponseBO = new SurveyResponseBO();
            SurveyResponseBO.ResponseId = ResponseXml.ResponseId.ToString();
            SurveyResponseBO.XML = ResponseXml.Xml;
          //  SurveyResponseBO.UserId = (int)ResponseXml.UserId;
          //  SurveyResponseBO.IsNewRecord = (bool)ResponseXml.IsNewRecord;
            return SurveyResponseBO;
        }

        public static ResponseXml ToEF(ResponseXmlBO ResponseXmlBO)
        {
            ResponseXml ResponseXml = new ResponseXml();
            ResponseXml.ResponseId = new Guid(ResponseXmlBO.ResponseId);
            ResponseXml.Xml = ResponseXmlBO.Xml;
            ResponseXml.UserId = ResponseXmlBO.User;
            ResponseXml.IsNewRecord = ResponseXmlBO.IsNewRecord;
            return ResponseXml;
        }
        public static ResponseDisplaySetting ToColumnName(KeyValuePair<int, string> ColumnList, Guid FormId)
        {
            return new ResponseDisplaySetting
            {
                SortOrder = ColumnList.Key + 1,
                ColumnName = ColumnList.Value,
                FormId = FormId
            };
        }
        public static ResponseDisplaySetting Map(string FormId, int i, string Column)
        {
            ResponseDisplaySetting ResponseDisplaySetting = new ResponseDisplaySetting();
            ResponseDisplaySetting.FormId = new Guid(FormId);
            ResponseDisplaySetting.ColumnName = Column;
            ResponseDisplaySetting.SortOrder = i;
            return ResponseDisplaySetting;

        }
        public static SurveyResponseBO Map(SurveyResponse entity, User User = null, int LastActiveUseerId = -1)
        {
            SurveyResponseBO SurveyResponseBO = new SurveyResponseBO();

            SurveyResponseBO.SurveyId = entity.SurveyId.ToString();
            SurveyResponseBO.ResponseId = entity.ResponseId.ToString();
            SurveyResponseBO.XML = entity.ResponseXML;
            SurveyResponseBO.Status = entity.StatusId;
            SurveyResponseBO.DateUpdated = entity.DateUpdated;
            SurveyResponseBO.DateCompleted = entity.DateCompleted;
            SurveyResponseBO.TemplateXMLSize = (long)entity.ResponseXMLSize;
            SurveyResponseBO.DateCreated = entity.DateCreated;
            SurveyResponseBO.IsDraftMode = entity.IsDraftMode;
            //SurveyResponseBO.IsLocked = entity.IsLocked;
            /// SurveyResponseBO.LastActiveUserId = LastActiveUseerId;
            if (entity.SurveyMetaData != null && entity.SurveyMetaData.ViewId!= null)
            {
                SurveyResponseBO.ViewId = (int)entity.SurveyMetaData.ViewId;
            }
            if (entity.ParentRecordId != null && entity.ParentRecordId!= null)
            {
                SurveyResponseBO.ParentRecordId = entity.ParentRecordId.ToString();
            }
            if (entity.RelateParentId != null && entity.RelateParentId!= null)
            {
                SurveyResponseBO.RelateParentId = entity.RelateParentId.ToString();
            }
            //if (User != null)
            //{
            //    SurveyResponseBO.UserEmail = User.EmailAddress;
            //}

            return SurveyResponseBO;
        }
    }
}
