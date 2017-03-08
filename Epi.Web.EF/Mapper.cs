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
    public class Mapper
    {
        /// <summary>
        /// Maps SurveyMetaData entity to SurveyInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A SurveyInfoBO business object.</returns>
        internal static SurveyInfoBO Map(SurveyMetaData entity)
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
                
                if (entity.UserPublishKey != null)
                {
                    result.UserPublishKey = entity.UserPublishKey;
                }
                result.SurveyType = entity.SurveyTypeId; 
            
            return result;
        }

        internal static CacheDependencyBO MapDependency(SurveyMetaData entity)
        {
            CacheDependencyBO cacheDependencyBO = new CacheDependencyBO();

            cacheDependencyBO.SurveyId = entity.SurveyId.ToString();

            if (entity.LastUpdate != null)
            { 
                cacheDependencyBO.LastUpdate = (DateTime)entity.LastUpdate;
            }

            return cacheDependencyBO;
        }

        internal static List<SurveyInfoBO> Map(List<SurveyMetaData> entities)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();
            foreach (SurveyMetaData surveyMetaData in entities)
            {
                result.Add(Map(surveyMetaData));
            }

            return result;
        }

        internal static void Map(SurveyMetaData entity, out CacheDependencyBO cacheDependencyBO)
        {
            cacheDependencyBO = new CacheDependencyBO();

            cacheDependencyBO.SurveyId = entity.SurveyId.ToString();

            if (entity.LastUpdate == null)
            {
                entity.LastUpdate = entity.DateCreated;
            }

            cacheDependencyBO.LastUpdate = (DateTime)entity.LastUpdate;
        }

        internal static void Map(List<SurveyMetaData> entities, out List<CacheDependencyBO> list)
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
        internal static SurveyMetaData Map(SurveyInfoBO businessobject)
        {
            return new SurveyMetaData
            {
                SurveyId = new Guid(businessobject.SurveyId),
                SurveyName = businessobject.SurveyName,
                SurveyNumber = businessobject.SurveyNumber,
                TemplateXML = businessobject.XML,
                IntroductionText = businessobject.IntroductionText,
                ExitText = businessobject.ExitText,
                OrganizationName = businessobject.OrganizationName,
                DepartmentName = businessobject.DepartmentName,
                ClosingDate = businessobject.ClosingDate ,
                UserPublishKey=businessobject.UserPublishKey,
                SurveyTypeId = businessobject.SurveyType,
                TemplateXMLSize = businessobject.TemplateXMLSize,
                DateCreated = businessobject.DateCreated,
                IsDraftMode = businessobject.IsDraftMode,
                StartDate = businessobject.StartDate,
                IsSQLProject = businessobject._IsSqlProject
            };
        }

        /// <summary>
        /// Maps SurveyMetaData entity to SurveyInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A SurveyInfoBO business object.</returns>
        internal static SurveyResponseBO Map(SurveyAnswerDTO entity)
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
        internal static SurveyAnswerDTO Map(SurveyResponseBO businessobject)
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
        internal static SurveyResponseBO Map(SurveyResponse entity)
        {
           
            return new SurveyResponseBO
            {
                SurveyId = entity.SurveyId.ToString(),
                ResponseId = entity.ResponseId.ToString(),
                XML = entity.ResponseXML,
                Status = entity.StatusId,
                 DateUpdated = entity.DateUpdated,
                DateCompleted = entity.DateCompleted,
                TemplateXMLSize = (long)entity.ResponseXMLSize,
                 DateCreated = entity.DateCreated,
                 IsDraftMode = entity.IsDraftMode ,
            };
        }

        internal static List<SurveyResponseBO> Map(List<SurveyResponse> entities)
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            foreach (SurveyResponse surveyResponse in entities)
            {
                result.Add(Map(surveyResponse));
            }

            return result;
        }
        internal static OrganizationBO Map(Organization entity)
        {
            return new OrganizationBO
            {
                Organization = entity.Organization1,
                IsEnabled = entity.IsEnabled,
                OrganizationKey = entity.OrganizationKey,
                OrganizationId = entity.OrganizationId

            };
        }
        internal static OrganizationBO Map(string Organization)
        {
            return new OrganizationBO
            {
                Organization = Organization
                 


            };
        }
        internal static AdminBO MapAdminEmail(string AdminEmail)
            {
            return new AdminBO
            {
                AdminEmail = AdminEmail



            };
            }
        internal static Organization ToEF(OrganizationBO pBo)
        {
            return new Organization
            {
                Organization1 = pBo.Organization,
                IsEnabled = pBo.IsEnabled,
                OrganizationKey = pBo.OrganizationKey,


            };
        }

        internal static Admin ToEF(AdminBO pBo)
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
        internal static SurveyResponse ToEF(SurveyResponseBO pBO)
        {
            return new SurveyResponse
            {
                SurveyId = new Guid(pBO.SurveyId),
                ResponseId = new Guid(pBO.ResponseId),
                ResponseXML = pBO.XML,
                StatusId = pBO.Status,
                 ResponseXMLSize = pBO.TemplateXMLSize,
                DateUpdated = pBO.DateUpdated,
                DateCompleted = pBO.DateCompleted,
                DateCreated = pBO.DateCreated,
                IsDraftMode = pBO.IsDraftMode

            };
        }
        internal static UserAuthenticationResponseBO ToAuthenticationResponseBO(UserAuthenticationRequestBO AuthenticationRequestBO)
        {


            return new UserAuthenticationResponseBO
            {
                PassCode = AuthenticationRequestBO.PassCode,

            };
        
        }

        internal static StateBO Map(State Row)
            {
            StateBO StateBO = new StateBO();
            StateBO.StateCode = Row.StateCode;
            StateBO.StateName = Row.StateName;
            StateBO.StateId = Row.StateProvinceId;
            return StateBO;
            }

        internal static   Address ToAddressEF(AdminBO Admin)
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
        internal static EIDatasource Map(DbConnectionStringBO ConnectionString)
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
        internal static List<SourceTableBO> MapToSourceTableBO(System.Data.DataTable dataTable)
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
    }
}
