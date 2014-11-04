using System.Linq;
using System.Collections.Generic;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.DTO;
using Epi.Web.Enter.Common.Message;
using Epi.Web.Enter.Common.Constants;
using System;
using System.Configuration;
namespace Epi.Web.Enter.Common.ObjectMapping
{
    /// <summary>
    /// Maps DTOs (Data Transfer Objects) to BOs (Business Objects) and vice versa.
    /// </summary>
    public static class Mapper
        {

        /// <summary>
        /// Maps SurveyMetaData entity to SurveyInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A SurveyInfoBO business object.</returns>
        public static SurveyInfoBO ToBusinessObject(SurveyInfoDTO pDTO)
            {
            return new SurveyInfoBO
            {
                SurveyId = pDTO.SurveyId,
                SurveyName = pDTO.SurveyName,
                SurveyNumber = pDTO.SurveyNumber,
                XML = pDTO.XML,
                IntroductionText = pDTO.IntroductionText,
                ExitText = pDTO.ExitText,
                OrganizationName = pDTO.OrganizationName,
                DepartmentName = pDTO.DepartmentName,
                ClosingDate = pDTO.ClosingDate,
                UserPublishKey = pDTO.UserPublishKey,
                SurveyType = pDTO.SurveyType,
                OrganizationKey = pDTO.OrganizationKey,
                IsDraftMode = pDTO.IsDraftMode,
                StartDate = pDTO.StartDate,
                OwnerId = pDTO.OwnerId,
                DBConnectionString = pDTO.DBConnectionString,
                IsSqlProject = pDTO.IsSqlProject,
                ViewId = pDTO.ViewId
                 
            };
            }

        public static FormInfoDTO ToFormInfoDTO(FormInfoBO BO)
            {
            return new FormInfoDTO
            {
                IsSQLProject = BO.IsSQLProject,
                FormId = BO.FormId,
                FormNumber = BO.FormNumber,
                FormName = BO.FormName,
                OrganizationName = BO.OrganizationName,
                OrganizationId = BO.OrganizationId,
                IsDraftMode = BO.IsDraftMode,
                UserId = BO.UserId,
                IsOwner = BO.IsOwner,
                OwnerFName = BO.OwnerFName,
                OwnerLName = BO.OwnerLName

            };
            }


        public static OrganizationBO ToBusinessObject(OrganizationDTO pDTO)
            {
            return new OrganizationBO
            {
                IsEnabled = pDTO.IsEnabled,
                Organization = pDTO.Organization,
                OrganizationKey = pDTO.OrganizationKey,
                OrganizationId = pDTO.OrganizationId
                // AdminId = pDTO.AdminId,

            };
            }
        public static OrganizationBO ToOrgBusinessObject(OrganizationDTO pDTO)
            {
            return new OrganizationBO
            {
                IsEnabled = pDTO.IsEnabled,
                Organization = pDTO.Organization,
                OrganizationKey = pDTO.OrganizationKey,
                OrganizationId = pDTO.OrganizationId

            };
            }
        public static OrganizationDTO ToDataTransferObjects(OrganizationBO pBO)
            {

            return new OrganizationDTO
            {
                //  AdminId = pBO.AdminId,
                IsEnabled = pBO.IsEnabled,
                Organization = pBO.Organization,
                OrganizationKey = Epi.Web.Enter.Common.Security.Cryptography.Decrypt(pBO.OrganizationKey),
                OrganizationId = pBO.OrganizationId

            };

            }

        public static List<SurveyInfoBO> ToBusinessObject(List<SurveyInfoDTO> pSurveyInfoList)
            {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();
            foreach (SurveyInfoDTO surveyInfo in pSurveyInfoList)
                {
                result.Add(ToBusinessObject(surveyInfo));
                };

            return result;
            }


        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyInfoDTO entity.
        /// </summary>
        /// <param name="SurveyInfo">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyInfoDTO.</returns>
        public static SurveyInfoDTO ToDataTransferObject(SurveyInfoBO pBO)
            {
            return new SurveyInfoDTO
            {
                SurveyId = pBO.SurveyId,
                SurveyName = pBO.SurveyName,
                SurveyNumber = pBO.SurveyNumber,
                XML = pBO.XML,
                IntroductionText = pBO.IntroductionText,
                ExitText = pBO.ExitText,
                OrganizationName = pBO.OrganizationName,
                DepartmentName = pBO.DepartmentName,
                SurveyType = pBO.SurveyType,
                ClosingDate = pBO.ClosingDate,
                IsDraftMode = pBO.IsDraftMode,
                StartDate = pBO.StartDate,
                IsSqlProject =pBO.IsSqlProject,
                UserPublishKey = pBO.UserPublishKey



            };
            }
        public static List<SurveyInfoDTO> ToDataTransferObject(List<SurveyInfoBO> pSurveyInfoList)
            {
            List<SurveyInfoDTO> result = new List<SurveyInfoDTO>();
            foreach (SurveyInfoBO surveyInfo in pSurveyInfoList)
                {
                result.Add(ToDataTransferObject(surveyInfo));
                };

            return result;
            }

        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyInfoDTO entity.
        /// </summary>
        /// <param name="SurveyInfo">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyInfoDTO.</returns>
        public static SurveyAnswerDTO ToDataTransferObject(SurveyResponseBO pBO)
            {
            SurveyAnswerDTO SurveyAnswerDTO = new SurveyAnswerDTO();

            SurveyAnswerDTO.SurveyId = pBO.SurveyId;
            SurveyAnswerDTO.ResponseId = pBO.ResponseId;
            SurveyAnswerDTO.DateUpdated = pBO.DateUpdated;
            SurveyAnswerDTO.XML = pBO.XML;
            SurveyAnswerDTO.DateCompleted = pBO.DateCompleted;
            SurveyAnswerDTO.DateCreated = pBO.DateCreated;
            SurveyAnswerDTO.Status = pBO.Status;
            SurveyAnswerDTO.IsDraftMode = pBO.IsDraftMode;
            SurveyAnswerDTO.IsLocked = pBO.IsLocked;
            SurveyAnswerDTO.ParentRecordId = pBO.ParentRecordId;
            SurveyAnswerDTO.UserEmail = pBO.UserEmail;
            SurveyAnswerDTO.ViewId = pBO.ViewId;
            SurveyAnswerDTO.RelateParentId = pBO.RelateParentId;
            SurveyAnswerDTO.SqlData = pBO.SqlData;
            if (pBO.ResponseHierarchyIds != null)
                {
                SurveyAnswerDTO.ResponseHierarchyIds = ToDataTransferObject(pBO.ResponseHierarchyIds);
                }
            return SurveyAnswerDTO;
            }
        public static List<SurveyAnswerDTO> ToDataTransferObject(List<SurveyResponseBO> pSurveyResposneList)
            {
            List<SurveyAnswerDTO> result = new List<SurveyAnswerDTO>();
            foreach (SurveyResponseBO surveyResponse in pSurveyResposneList)
                {
                result.Add(ToDataTransferObject(surveyResponse));
                };

            return result;
            }

        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyInfoDTO entity.
        /// </summary>
        /// <param name="SurveyInfo">A SurveyResponseDTO business object.</param>
        /// /// <returns>A SurveyResponseBO.</returns>
        public static SurveyResponseBO ToBusinessObject(SurveyAnswerDTO pDTO, int UserId = 0)
            {
            return new SurveyResponseBO
            {
                SurveyId = pDTO.SurveyId,
                ResponseId = pDTO.ResponseId,
                DateUpdated = pDTO.DateUpdated,
                XML = pDTO.XML,
                DateCompleted = pDTO.DateCompleted,
                DateCreated = pDTO.DateCreated,
                Status = pDTO.Status,
                IsDraftMode = pDTO.IsDraftMode,
                UserId = UserId,
                ParentRecordId = pDTO.ParentRecordId,
                RecrodSourceId = pDTO.RecordSourceId

            };
            }

        public static List<SurveyResponseBO> ToBusinessObject(List<SurveyAnswerDTO> pSurveyAnswerList, int UserId)
            {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            foreach (SurveyAnswerDTO surveyAnswer in pSurveyAnswerList)
                {
                result.Add(ToBusinessObject(surveyAnswer));
                };

            return result;
            }

        /// <summary>
        /// Maps SurveyRequestResultBO business object to PublishInfoDTO.
        /// </summary>
        /// <param name="SurveyInfo">A SurveyRequestResultBO business object.</param>
        /// <returns>A PublishInfoDTO.</returns>
        public static PublishInfoDTO ToDataTransferObject(SurveyRequestResultBO pBO)
            {
            return new PublishInfoDTO
            {
                IsPulished = pBO.IsPulished,
                StatusText = pBO.StatusText,
                URL = pBO.URL,
                ViewIdAndFormIdList = pBO.ViewIdAndFormIdList
            };
            }

        public static UserAuthenticationRequestBO ToPassCodeBO(UserAuthenticationRequest UserAuthenticationObj)
            {
            return new UserAuthenticationRequestBO
            {
                ResponseId = UserAuthenticationObj.SurveyResponseId,
                PassCode = UserAuthenticationObj.PassCode

            };
            }

        public static UserBO ToUserBO(UserDTO User)
            {
            return new UserBO
            {
                UserId = User.UserId,
                UserName = User.UserName,
                FirstName = User.FirstName,
                LastName = User.LastName,
                EmailAddress = User.EmailAddress,
                PhoneNumber = User.PhoneNumber,
                PasswordHash = User.PasswordHash,
                ResetPassword = User.ResetPassword,
                Role = User.Role,
                IsActive = User.IsActive,
                Operation = (Constant.OperationMode)User.Operation
            };
            }

        public static UserAuthenticationResponse ToAuthenticationResponse(UserAuthenticationResponseBO AuthenticationRequestBO)
            {

            return new UserAuthenticationResponse
            {

                PassCode = AuthenticationRequestBO.PassCode,

            };


            }
        /// <summary>
        /// Transforms list of SurveyInfoBO BOs list of category DTOs.
        /// </summary>
        /// <param name="SurveyInfoBO">List of categories BOs.</param>
        /// <returns>List of SurveyInfoDTO DTOs.</returns>
        public static IList<SurveyInfoDTO> ToDataTransferObjects(IEnumerable<SurveyInfoBO> pBO)
            {
            if (pBO == null) return null;
            return pBO.Select(c => ToDataTransferObject(c)).ToList();
            }

        public static AdminDTO ToAdminDTO(AdminBO AdminBO)
            {



            return new AdminDTO
            {

                AdminEmail = AdminBO.AdminEmail,
                IsActive = AdminBO.IsActive,
                OrganizationId = AdminBO.OrganizationId

            };

            }

        public static FormSettingDTO ToDataTransferObject(FormSettingBO pBO)
            {
            return new FormSettingDTO
            {
                ColumnNameList = pBO.ColumnNameList,
                FormControlNameList = pBO.FormControlNameList,
                AssignedUserList = pBO.AssignedUserList,
                UserList = pBO.UserList

            };
            }



        public static UserDTO ToUserDTO(UserBO result)
            {
            return new UserDTO()
            {
                UserId = result.UserId,
                UserName = result.UserName,
                FirstName = result.FirstName,
                LastName = result.LastName,
                PasswordHash = result.PasswordHash,
                PhoneNumber = result.PhoneNumber,
                ResetPassword = result.ResetPassword,
                Role = result.Role,
                Operation = Constant.OperationMode.NoChange,
                EmailAddress = result.EmailAddress,
                IsActive = result.IsActive
            };
            }
        public static UserDTO ToDataTransferObject(UserBO result)
            {
            return new UserDTO()
            {
                UserId = result.UserId,
                UserName = result.UserName,
                FirstName = result.FirstName,
                LastName = result.LastName,
                PhoneNumber = result.PhoneNumber,
                Role = result.Role,
                IsActive = result.IsActive,
                EmailAddress = result.EmailAddress
            };
            }
        public static List<FormsHierarchyDTO> ToFormHierarchyDTO(List<FormsHierarchyBO> AllChildIDsList)
            {
            List<FormsHierarchyDTO> result = new List<FormsHierarchyDTO>();
            foreach (FormsHierarchyBO Obj in AllChildIDsList)
                {
                FormsHierarchyDTO FormsHierarchyDTO = new FormsHierarchyDTO();
                FormsHierarchyDTO.FormId = Obj.FormId;
                FormsHierarchyDTO.ViewId = Obj.ViewId;
                if (Obj.ResponseIds != null)
                    {
                    FormsHierarchyDTO.ResponseIds = ToSurveyAnswerDTO(Obj.ResponseIds);
                    }
                result.Add(FormsHierarchyDTO);
                }
            return result;
            }
        public static SurveyResponseBO ToBusinessObject(string Xml, string SurveyId,string ParentRecordId,string ResponseId,int UserId)
            {
           
            return new SurveyResponseBO
            {

                SurveyId = SurveyId,
                ResponseId = ResponseId,
                XML = Xml,
                DateCreated = DateTime.Now,
                Status = 2,
                IsDraftMode = false,
                ParentId = ParentRecordId,
                RelateParentId = ParentRecordId, 
                TemplateXMLSize = RemoveWhitespace(Xml).Length,
                RecrodSourceId = 2,
                ParentRecordId = ParentRecordId,
                UserId = UserId

            };

            }
        private static List<SurveyAnswerDTO> ToSurveyAnswerDTO(List<SurveyResponseBO> list)
            {
            List<SurveyAnswerDTO> ModelList = new List<SurveyAnswerDTO>();
            foreach (var Obj in list)
                {
                SurveyAnswerDTO SurveyAnswerModel = new SurveyAnswerDTO();
                SurveyAnswerModel.ResponseId = Obj.ResponseId;
                SurveyAnswerModel.SurveyId = Obj.SurveyId;
                SurveyAnswerModel.DateUpdated = Obj.DateUpdated;
                SurveyAnswerModel.DateCompleted = Obj.DateCompleted;
                SurveyAnswerModel.Status = Obj.Status;
                SurveyAnswerModel.XML = Obj.XML;
                SurveyAnswerModel.ParentRecordId = Obj.ParentRecordId;
                SurveyAnswerModel.RelateParentId = Obj.RelateParentId;
                ModelList.Add(SurveyAnswerModel);
                }
            return ModelList;
            }

        public static string RemoveWhitespace(string xml)
            {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@">\s*<");
            xml = regex.Replace(xml, "><");

            return xml.Trim();
            }
        }
}
