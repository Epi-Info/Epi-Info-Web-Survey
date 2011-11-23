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
        public Epi.Web.Common.DTO.cSurveyInfo GetSurveyInfoById(String pId){

            Epi.Web.Common.DTO.cSurveyInfo result = new Epi.Web.Common.DTO.cSurveyInfo();



            using (var Context = new Epi.Web.EF.EIWSEntities(GetconnectionString()))
           {
               var Surveys = from p in Context.SurveyMetaDatas
                              // where p.SurveyId == pId
                             where p.SurveyNumber == "33333"

                               select p;

               foreach (var Survey in Surveys)
               {
                   result.SurveyName = Survey.SurveyName;
                   result.SurveyNumber = Survey.SurveyNumber;
                   result.XML = Survey.TemplateXML;
                   result.SurveyName = Survey.SurveyName;
                   result.IntroductionText = Survey.IntroductionText;
 
               }

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
