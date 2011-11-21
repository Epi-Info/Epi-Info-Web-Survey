using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.BLL
{
    public class Publisher
    {

        public Epi.Web.Interfaces.ISurveyRequestResult PublishSurvey(Epi.Web.Interfaces.ISurveyRequest pRequestMessage)
        {

            Epi.Web.Interfaces.ISurveyRequestResult result = new cSurveyRequestResult();
            if (pRequestMessage != null) {

                var Entities = new Epi.Web.EF.EIWSEntities();
                Epi.Web.EF.SurveyMetaData SurveyMetaData = new Epi.Web.EF.SurveyMetaData();
                SurveyMetaData.SurveyId = Guid.NewGuid();
                SurveyMetaData.ClosingDate = pRequestMessage.ClosingDate;
                SurveyMetaData.DepartmentName = pRequestMessage.DepartmentName;
                SurveyMetaData.IntroductionText = pRequestMessage.IntroductionText;
                SurveyMetaData.OrganizationName = pRequestMessage.OrganizationName;
                SurveyMetaData.SurveyNumber = pRequestMessage.SurveyNumber;
                SurveyMetaData.TemplateXML = pRequestMessage.TemplateXML;
                SurveyMetaData.SurveyName = pRequestMessage.SurveyName;
                Entities.AddToSurveyMetaDatas(SurveyMetaData);
                Entities.SaveChanges();
            
            }
            return result;
        }
    }
}
