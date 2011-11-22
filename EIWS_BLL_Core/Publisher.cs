using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;

namespace Epi.Web.BLL
{
   /// <summary>
   /// 
   /// </summary>
  
    public class Publisher
    {
        /// <summary>
        ///  This class is used to process the object sent from the WCF service “SurveyManager”, 
        ///  save survey info into SurvayMetaData Table 
        ///  and then returns a URL, StatusText and IsPublished indicator.
        /// </summary>
        /// <param name="pRequestMessage"></param>
        /// <returns></returns>
        /// 

        public Epi.Web.Common.DTO.cSurveyRequestResult PublishSurvey(Epi.Web.Common.DTO.cSurveyRequest pRequestMessage)
        {

            Epi.Web.Common.DTO.cSurveyRequestResult result = new Epi.Web.Common.DTO.cSurveyRequestResult();

            var SurveyId = Guid.NewGuid();
            var  connectionString = GetconnectionString();
            
            if (pRequestMessage != null) {
                try
                {
                    var Entities = new Epi.Web.EF.EIWSEntities(connectionString);
                    //var Entities = new Epi.Web.EF.EIWSEntities();

                    Epi.Web.EF.SurveyMetaData SurveyMetaData = new Epi.Web.EF.SurveyMetaData();


                SurveyMetaData.SurveyId = SurveyId;
                SurveyMetaData.ClosingDate = pRequestMessage.ClosingDate;
               
                SurveyMetaData.IntroductionText = pRequestMessage.IntroductionText;

                SurveyMetaData.DepartmentName = pRequestMessage.DepartmentName;
                SurveyMetaData.OrganizationName = pRequestMessage.OrganizationName;

                SurveyMetaData.SurveyNumber = pRequestMessage.SurveyNumber;

                SurveyMetaData.TemplateXML = pRequestMessage.TemplateXML;

                SurveyMetaData.SurveyName = pRequestMessage.SurveyName;

                Entities.AddToSurveyMetaDatas(SurveyMetaData);
                
                try
                {
                  
                    Entities.SaveChanges();
                }
                catch 
                {
                    Entities.ObjectStateManager.GetObjectStateEntry(SurveyMetaData).Delete();
              
                }
                }
                catch (Exception ex)
                {
                    throw (ex);

                }
                if (pRequestMessage.SurveyNumber != "" && pRequestMessage.SurveyNumber != null)
                {

                    result.URL = GetURL(pRequestMessage, SurveyId);
                    result.IsPulished = true;
                }
                else {

                    result.URL = "";
                    result.IsPulished = false;
                    result.StatusText = "An Error has occurred while publishing your survey.";
                }
                
                 
            }
            return result;
        }

        public string GetURL(Epi.Web.Common.DTO.cSurveyRequest pRequestMessage , Guid SurveyId )
        {
            System.Text.StringBuilder URL = new System.Text.StringBuilder();
            URL.Append(System.Configuration.ConfigurationManager.AppSettings["URL"]);
            URL.Append("/");
            URL.Append(pRequestMessage.SurveyNumber.ToString());
            URL.Append("/");
            URL.Append(SurveyId.ToString());
            return URL.ToString();
        }
        public string GetconnectionString() {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EIWSEntities"].ConnectionString;
            return connectionString;
        }
    }
}
