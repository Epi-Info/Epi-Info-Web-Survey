using System.Linq;
using System.Collections.Generic;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
using System;
using System.Configuration;
namespace Epi.Web.Common.ObjectMapping
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
                UserPublishKey=pDTO.UserPublishKey,
                SurveyType = pDTO.SurveyType,
                OrganizationKey = pDTO.OrganizationKey,
                 IsDraftMode = pDTO.IsDraftMode,
                 StartDate  = pDTO.StartDate,
                DBConnectionString = pDTO.DBConnectionString,
                IsSqlProject = pDTO.IsSqlProject,
            };
        }

        public static OrganizationBO ToBusinessObject(OrganizationDTO pDTO)
        {
            return new OrganizationBO
            {
                 IsEnabled = pDTO.IsEnabled,
                 Organization = pDTO.Organization,
                 OrganizationKey = pDTO.OrganizationKey,
                 // AdminId = pDTO.AdminId,
                  
            };
        }
        public static AdminBO ToBusinessObject(AdminDTO pDTO)
            {
            return new AdminBO
            {
                 AdminEmail = pDTO.AdminEmail,
                 IsActive = pDTO.IsActive,
                // Notify = pDTO.Notify,
                 OrganizationId = pDTO.OrganizationId,
                 //AdminId = pDTO.AdminId
                StateId = pDTO.StateId,
                 AdressLine1 = pDTO.AdressLine1,
                 AdressLine2 = pDTO.AdressLine2,
                 City = pDTO.City,
                 Zip = pDTO.Zip,
                 LastName = pDTO.LastName,
                 FirstName = pDTO.FirstName ,
                 PhoneNumber =pDTO.PhoneNumber,
            };
            }
        public static OrganizationDTO ToDataTransferObjects(OrganizationBO pBO)
        {

            return new OrganizationDTO
            {
              //  AdminId = pBO.AdminId,
                IsEnabled = pBO.IsEnabled,
                Organization = pBO.Organization,
                OrganizationKey = pBO.OrganizationKey

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

        public static CacheDependencyDTO ToDataTransferObject(CacheDependencyBO pBO)
        {
            return new CacheDependencyDTO
            {
                SurveyId = pBO.SurveyId,
                LastUpdate = pBO.LastUpdate
            };
        }

        public static List<CacheDependencyDTO> ToDataTransferObject(List<CacheDependencyBO> pSurveyInfoList)
        {
            List<CacheDependencyDTO> list = new List<CacheDependencyDTO>();
            foreach (CacheDependencyBO surveyInfo in pSurveyInfoList)
            {
                list.Add(ToDataTransferObject(surveyInfo));
            };
            return list;
        }

        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyInfoDTO entity.
        /// </summary>
        /// <param name="SurveyInfo">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyInfoDTO.</returns>
        public static SurveyAnswerDTO ToDataTransferObject(SurveyResponseBO pBO)
        {
            return new SurveyAnswerDTO
            {
                SurveyId = pBO.SurveyId,
                ResponseId = pBO.ResponseId,
                DateUpdated = pBO.DateUpdated,
                XML = pBO.XML,
                DateCompleted = pBO.DateCompleted,
                DateCreated = pBO.DateCreated, 
                Status = pBO.Status,
                IsDraftMode = pBO.IsDraftMode
            };
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
        public static SurveyResponseBO ToBusinessObject(SurveyAnswerDTO pDTO)
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
                IsDraftMode = pDTO.IsDraftMode
                
            };
        }

        public static List<SurveyResponseBO> ToBusinessObject(List<SurveyAnswerDTO> pSurveyAnswerList)
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
                URL = pBO.URL

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
        public static SurveyResponseBO ToBusinessObject(string Xml , string SurveyId)
            {
            Guid SurveyResponseId = Guid.NewGuid();
            return new SurveyResponseBO
            {

                SurveyId = SurveyId,
                ResponseId = SurveyResponseId.ToString(),
                XML = Xml,
                DateCreated = DateTime.Now,
                Status = 2,
                IsDraftMode = false

            };


            }

        public static UserAuthenticationRequestBO ToBusinessObject(string ResponseId)
            {
           
            Guid NewGuid = Guid.NewGuid();
            return new UserAuthenticationRequestBO
            {

                PassCode = (NewGuid.ToString()).Substring(0, 4),
                ResponseId = ResponseId

            };
            }

        public static PassCodeDTO ToDataTransferObjects(UserAuthenticationRequestBO UserAuthenticationRequestBO)
            {
          
            PassCodeDTO PassCodeDTO = new DTO.PassCodeDTO();
            PassCodeDTO.PassCode = UserAuthenticationRequestBO.PassCode.ToString();
            PassCodeDTO.ResponseId =  UserAuthenticationRequestBO.ResponseId;



            return PassCodeDTO;
            }


        public static List<StateDTO> ToStateDTO(List<StateBO> list)
            {
            List<StateDTO>  DTOList = new List<StateDTO>();
           foreach (var item in list){

           StateDTO StateDTO = new StateDTO();
           StateDTO.StateCode = item.StateCode;
           StateDTO.StateId = item.StateId; 
           StateDTO.StateName = item.StateName;
           DTOList.Add(StateDTO);
               }
           return DTOList;
            }
        public static List<SourceTableDTO> ToSourceTableDTO(List<SourceTableBO> list)
        {
            List<SourceTableDTO> DTOList = new List<SourceTableDTO>();
           
            foreach (var item in list)
            {
                SourceTableDTO SourceTableDTO = new SourceTableDTO();
                SourceTableDTO.TableName = item.TableName;
                SourceTableDTO.TableXml  = item.TableXml ;
                DTOList.Add(SourceTableDTO);
           }
            return DTOList;
        }
    }
}
