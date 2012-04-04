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
                if (entity.UserPublishKey != null)
                {
                    result.UserPublishKey = (Guid)entity.UserPublishKey.Value;
                }
                result.SurveyType = entity.SurveyTypeId; 
            


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
                SurveyTypeId = businessobject.SurveyType 

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
                DateLastUpdated = entity.DateLastUpdated,
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
                DateLastUpdated = businessobject.DateLastUpdated,
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
                
                DateLastUpdated = entity.DateLastUpdated,
                DateCompleted = entity.DateCompleted.Value,
                
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
                
                DateLastUpdated = pBO.DateLastUpdated,
                DateCompleted = pBO.DateCompleted

            };
        }
        internal static UserAuthenticationResponseBO ToAuthenticationResponseBO(UserAuthenticationRequestBO AuthenticationRequestBO)
        {


            return new UserAuthenticationResponseBO
            {
                PassCode = AuthenticationRequestBO.PassCode,

            };
        
        }
    }
}
