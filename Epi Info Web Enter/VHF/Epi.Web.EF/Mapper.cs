using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.DTO;

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
            SurveyInfoBO result = new SurveyInfoBO();

            result.SurveyId = entity.SurveyId.ToString();
            result.SurveyName = entity.SurveyName;
            result.SurveyNumber = entity.SurveyNumber;
            result.XML = entity.TemplateXML;
            result.IntroductionText = entity.IntroductionText;
            result.ExitText = entity.ExitText;
            result.OrganizationName = entity.OrganizationName;
            result.DepartmentName = entity.DepartmentName;
            result.ClosingDate = entity.ClosingDate;
            result.TemplateXMLSize = (long)entity.TemplateXMLSize;
            result.DateCreated = entity.DateCreated;
            result.IsDraftMode = entity.IsDraftMode;
            result.StartDate = entity.StartDate;
            result.IsSqlProject = (bool)entity.IsSQLProject;
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
            return result;
        }

        public static FormInfoBO ToFormInfoBO(SurveyMetaData entity)
        {
            return new FormInfoBO
            {
                FormId = entity.SurveyId.ToString(),
                FormNumber = entity.SurveyNumber,
                FormName = entity.SurveyName,
                OrganizationName = entity.OrganizationName,
                OrganizationId = entity.OrganizationId,
                IsDraftMode = entity.IsDraftMode,
                UserId = entity.OwnerId,
                ParentId = (entity.ParentId != null) ? entity.ParentId.ToString() : ""

            };
        }

        /// <summary>
        /// Maps the Entity User to BO
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="user"></param>
        public static UserBO MapToUserBO(User user)
        {
            UserBO Result = new UserBO();
            Result.UserId = user.UserID;
            Result.UserName = user.UserName;
            Result.EmailAddress = user.EmailAddress;
            Result.FirstName = user.FirstName;
            Result.LastName = user.LastName;
            Result.PhoneNumber = user.PhoneNumber;
            Result.ResetPassword = user.ResetPassword;


            return Result;
        }



        /// <summary>
        /// Maps SurveyMetaData entity to FormInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A FormInfoBO business object.</returns>
        internal static FormInfoBO MapToFormInfoBO(SurveyMetaData entity, User UserEntity, bool GetXml = false)
        {
            FormInfoBO result = new FormInfoBO();
            result.IsSQLProject = (entity.IsSQLProject == null) ? false : (bool)entity.IsSQLProject;
            result.FormId = entity.SurveyId.ToString();
            result.FormName = entity.SurveyName;
            result.FormNumber = entity.SurveyNumber;
            result.OrganizationName = entity.OrganizationName;
            result.OrganizationId = entity.OrganizationId;
            result.IsDraftMode = entity.IsDraftMode;
            result.UserId = entity.OwnerId;
            result.OwnerFName = UserEntity.FirstName;
            result.OwnerLName = UserEntity.LastName;
            if (GetXml)
            {
                result.Xml = entity.TemplateXML;
            }
            result.ParentId = entity.ParentId.ToString();
            return result;
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

        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyMetaData entity.
        /// </summary>
        /// <param name="businessobject">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyMetaData entity.</returns>
        internal static SurveyMetaData Map(SurveyInfoBO businessobject)
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
            SurveyMetaData.OwnerId = businessobject.OwnerId;
            SurveyMetaData.ViewId = businessobject.ViewId;
            SurveyMetaData.IsSQLProject = businessobject.IsSqlProject;
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
        internal static SurveyResponseBO Map(SurveyResponse entity, User User = null)
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
            SurveyResponseBO.IsLocked = entity.IsLocked;
            if (entity.SurveyMetaData != null)
            {
                SurveyResponseBO.ViewId = (int)entity.SurveyMetaData.ViewId;
            }
            if (entity.ParentRecordId != null)
            {
                SurveyResponseBO.ParentRecordId = entity.ParentRecordId.ToString();
            }
            if (entity.RelateParentId != null)
            {
                SurveyResponseBO.RelateParentId = entity.RelateParentId.ToString();
            }
            if (User != null)
            {
                SurveyResponseBO.UserEmail = User.EmailAddress;
            }

            return SurveyResponseBO;
        }

        internal static List<SurveyResponseBO> Map(List<SurveyResponse> entities)
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            foreach (var surveyResponse in entities)
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
        internal static Organization ToEF(OrganizationBO pBo)
        {
            return new Organization
            {
                Organization1 = pBo.Organization,
                IsEnabled = pBo.IsEnabled,
                OrganizationKey = pBo.OrganizationKey,


            };
        }
        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyMetaData entity.
        /// </summary>
        /// <param name="businessobject">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyMetaData entity.</returns>
        internal static SurveyResponse ToEF(SurveyResponseBO pBO)
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
            return SurveyResponse;
        }
        internal static UserAuthenticationResponseBO ToAuthenticationResponseBO(UserAuthenticationRequestBO AuthenticationRequestBO)
        {


            return new UserAuthenticationResponseBO
            {
                PassCode = AuthenticationRequestBO.PassCode,

            };

        }

        internal static ResponseDisplaySetting ToColumnName(KeyValuePair<int, string> ColumnList, Guid FormId)
        {
            return new ResponseDisplaySetting
            {
                SortOrder = ColumnList.Key + 1,
                ColumnName = ColumnList.Value,
                FormId = FormId
            };
        }

        internal static SurveyMetaData ToEF(SurveyInfoBO SurveyInfo)
        {
            SurveyMetaData DataRow = new SurveyMetaData();
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
            return DataRow;
        }
        private static string RemoveWhitespace(string xml)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@">\s*<");
            xml = regex.Replace(xml, "><");

            return xml.Trim();
        }

        internal static SurveyMetaData ToEF(FormInfoBO FormInfoBO)
        {
            return new SurveyMetaData
            {
                IsDraftMode = FormInfoBO.IsDraftMode
            };
        }



        internal static List<SurveyInfoBO> Map(IEnumerable<SurveyMetaData> iEnumerable)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();
            foreach (SurveyMetaData Obj in iEnumerable)
            {
                result.Add(Map(Obj));

            }
            return result;
        }




        internal static List<SurveyResponseBO> Map(IEnumerable<SurveyResponse> entities)
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            foreach (SurveyResponse surveyResponse in entities)
            {
                result.Add(Map(surveyResponse));
            }

            return result;
        }


        internal static SurveyResponseBO Map(ResponseXml ResponseXml)
        {
            SurveyResponseBO SurveyResponseBO = new SurveyResponseBO();
            SurveyResponseBO.ResponseId = ResponseXml.ResponseId.ToString();
            SurveyResponseBO.XML = ResponseXml.Xml;
            SurveyResponseBO.UserId = (int)ResponseXml.UserId;
            SurveyResponseBO.IsNewRecord = (bool)ResponseXml.IsNewRecord;
            return SurveyResponseBO;
        }

        internal static ResponseXml ToEF(ResponseXmlBO ResponseXmlBO)
        {
            ResponseXml ResponseXml = new ResponseXml();
            ResponseXml.ResponseId = new Guid(ResponseXmlBO.ResponseId);
            ResponseXml.Xml = ResponseXmlBO.Xml;
            ResponseXml.UserId = ResponseXmlBO.User;
            ResponseXml.IsNewRecord = ResponseXmlBO.IsNewRecord;
            return ResponseXml;
        }

        internal static ResponseDisplaySetting Map(string FormId, int i, string Column)
        {
            ResponseDisplaySetting ResponseDisplaySetting = new ResponseDisplaySetting();
            ResponseDisplaySetting.FormId = new Guid(FormId);
            ResponseDisplaySetting.ColumnName = Column;
            ResponseDisplaySetting.SortOrder = i;
            return ResponseDisplaySetting;

        }

        internal static Organization MapToOrganizationBO(OrganizationBO organizationBO)
        {
            throw new NotImplementedException();
        }

        internal static User ToUserEntity(UserBO User)
        {
            User UserEntity = new User();
            UserEntity.EmailAddress = User.EmailAddress;
            UserEntity.UserName = User.EmailAddress;
            UserEntity.LastName = User.LastName;
            UserEntity.FirstName = User.FirstName;
            UserEntity.PhoneNumber = User.PhoneNumber;
            UserEntity.ResetPassword = User.ResetPassword;
            UserEntity.PasswordHash = User.PasswordHash;

            return UserEntity;
        }

        internal static UserOrganization ToUserOrganizationEntity(bool IsActive, UserBO User, OrganizationBO Organization)
        {
            UserOrganization UserOrganization = new UserOrganization();
            UserOrganization.Active = IsActive;

            UserOrganization.RoleId = User.Role;

            User UserInfo = new EF.User();
            Organization OrganizationInfo = new EF.Organization();
            UserInfo.EmailAddress = User.EmailAddress;
            UserInfo.UserName = User.EmailAddress;
            UserInfo.LastName = User.LastName;
            UserInfo.FirstName = User.FirstName;
            UserInfo.PhoneNumber = User.PhoneNumber;
            UserInfo.ResetPassword = User.ResetPassword; //false;
            UserInfo.PasswordHash = User.PasswordHash; //"PassWord1";
            UserOrganization.User = UserInfo;


            OrganizationInfo.Organization1 = Organization.Organization;
            OrganizationInfo.IsEnabled = Organization.IsEnabled;
            OrganizationInfo.OrganizationKey = Organization.OrganizationKey;

            UserOrganization.Organization = OrganizationInfo;

            return UserOrganization;
        }

        internal static UserOrganization ToUserOrganizationEntity(bool IsActive, int UserId, int RoleId, OrganizationBO Organization)
        {
            UserOrganization UserOrganization = new UserOrganization();
            UserOrganization.Active = IsActive;

            UserOrganization.RoleId = RoleId;
            UserOrganization.UserID = UserId;

            Organization OrganizationInfo = new EF.Organization();



            OrganizationInfo.Organization1 = Organization.Organization;
            OrganizationInfo.IsEnabled = Organization.IsEnabled;
            OrganizationInfo.OrganizationKey = Organization.OrganizationKey;
            OrganizationInfo.OrganizationId = Organization.OrganizationId;
            UserOrganization.Organization = OrganizationInfo;

            return UserOrganization;
        }
        internal static UserOrganization ToUserOrganizationEntity(UserBO User, OrganizationBO Organization)
        {
            UserOrganization UserOrganization = new UserOrganization();
            UserOrganization.Active = User.IsActive;
            UserOrganization.RoleId = User.Role;
            UserOrganization.OrganizationID = Organization.OrganizationId;


            return UserOrganization;
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
    }
}
