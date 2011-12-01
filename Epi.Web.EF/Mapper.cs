using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;

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
            return new SurveyInfoBO
            {
                SurveyId = entity.SurveyId.ToString(),
                SurveyName = entity.SurveyName,
                SurveyNumber = entity.SurveyNumber,
                XML = entity.TemplateXML,
                IntroductionText = entity.IntroductionText,
                OrganizationName = entity.OrganizationName,
                DepartmentName = entity.DepartmentName
            };
        }

        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyMetaData entity.
        /// </summary>
        /// <param name="customer">A SurveyInfoBO business object.</param>
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
                OrganizationName = businessobject.OrganizationName,
                DepartmentName = businessobject.DepartmentName
            };
        }

    }
}
