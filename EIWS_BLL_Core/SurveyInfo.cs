using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.BLL
{

  public  class SurveyInfo
    {

        public SurveyInfo()
        {
        }
        public Epi.Web.Common.DTO.SurveyInfoDTO GetSurveyInfoById(String pId)
        {
            Epi.Web.Common.DTO.SurveyInfoDTO result = new Epi.Web.Common.DTO.SurveyInfoDTO();
            Guid Id = new Guid(pId);


            using (var Context = new Epi.Web.EF.EIWSEntities(GetconnectionString()))
           {
               var Surveys = from p in Context.SurveyMetaDatas
                             where p.SurveyId.Equals(Id)

                               select p;

                var Survey = Surveys.Single();

                result.SurveyId = Survey.SurveyId.ToString();
                result.SurveyName = Survey.SurveyName;
                result.SurveyNumber = Survey.SurveyNumber;
                result.XML = Survey.TemplateXML;
                result.SurveyName = Survey.SurveyName;
                result.IntroductionText = Survey.IntroductionText;
                result.OrganizationName = Survey.OrganizationName;
                result.DepartmentName = Survey.DepartmentName;
               

           }

            return result;

          
        
        }
        public string GetconnectionString()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EIWSEntities"].ConnectionString;
            return connectionString;
        }
    }
}
