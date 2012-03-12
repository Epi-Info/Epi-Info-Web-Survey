using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using Epi.Web.Common.BusinessObject;

namespace Epi.Web.BLL
{
   /// <summary>
   /// 
   /// </summary>
  
    public class Publisher
    {
        private Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao;
        #region"Public members"
        /// <summary>
        ///  This class is used to process the object sent from the WCF service “SurveyManager”, 
        ///  save survey info into SurvayMetaData Table 
        ///  and then returns a URL, StatusText and IsPublished indicator.
        /// </summary>
        /// <param name="pRequestMessage"></param>
        /// <returns></returns>
        /// 
        public Publisher(Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao pSurveyInfoDao)
        {
            this.SurveyInfoDao = pSurveyInfoDao;
        }
        public Publisher()
        {
            
        }
        public SurveyRequestResultBO PublishSurvey(SurveyInfoBO  pRequestMessage)
        {

            SurveyRequestResultBO result = new SurveyRequestResultBO();

            var SurveyId = Guid.NewGuid();

            if (pRequestMessage != null)
            {

                if (! string.IsNullOrEmpty(pRequestMessage.SurveyNumber))
                {
                    try
                    {

                        Epi.Web.Common.BusinessObject.SurveyInfoBO BO = new Common.BusinessObject.SurveyInfoBO();

                        BO.SurveyId = SurveyId.ToString();
                        BO.ClosingDate =  pRequestMessage.ClosingDate;

                        BO.IntroductionText = pRequestMessage.IntroductionText;
                        BO.ExitText = pRequestMessage.ExitText;
                        BO.DepartmentName = pRequestMessage.DepartmentName;
                        BO.OrganizationName = pRequestMessage.OrganizationName;

                        BO.SurveyNumber = pRequestMessage.SurveyNumber;

                        BO.XML = pRequestMessage.XML;

                        BO.SurveyName = pRequestMessage.SurveyName;

                        BO.SurveyType = pRequestMessage.SurveyType;
                        BO.UserPublishKey = pRequestMessage.UserPublishKey;

                        try
                        {

                            this.SurveyInfoDao.InsertSurveyInfo(BO);
                        }
                        catch (Exception ex)
                        {
                            System.Console.Write(ex.ToString());
                            //Entities.ObjectStateManager.GetObjectStateEntry(SurveyMetaData).Delete();

                        }
                    }
                    catch (Exception ex)
                    {
                        throw (ex);

                    }


                    result.URL = GetURL(pRequestMessage, SurveyId);
                    result.IsPulished = true;
                }
                else
                {

                    result.URL = "";
                    result.IsPulished = false;
                    result.StatusText = "An Error has occurred while publishing your survey.";
                }


            }
            return result;
        }

        #endregion

        #region "Private members"
        private string GetURL(SurveyInfoBO  pRequestMessage, Guid SurveyId)
        {
            System.Text.StringBuilder URL = new System.Text.StringBuilder();
            URL.Append(System.Configuration.ConfigurationManager.AppSettings["URL"]);
           // URL.Append("/");
            //URL.Append(pRequestMessage.SurveyNumber.ToString());
            //URL.Append("/");
            URL.Append(SurveyId.ToString());
            return URL.ToString();
        }
        #endregion
        
        
      
    }
}
