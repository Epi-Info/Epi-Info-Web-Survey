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
        private Epi.Web.Interfaces.DataInterfaces.IOrganizationDao OrganizationDao;
        #region"Public members"
        /// <summary>
        ///  This class is used to process the object sent from the WCF service “SurveyManager”, 
        ///  save survey info into SurvayMetaData Table 
        ///  and then returns a URL, StatusText and IsPublished indicator.
        /// </summary>
        /// <param name="pRequestMessage"></param>
        /// <returns></returns>
        /// 
        public Publisher(Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao pSurveyInfoDao, Epi.Web.Interfaces.DataInterfaces.IOrganizationDao pPrganizationDao)
        {
            this.SurveyInfoDao = pSurveyInfoDao;
            this.OrganizationDao = pPrganizationDao;
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

                //if (! string.IsNullOrEmpty(pRequestMessage.SurveyNumber)  &&  ValidateOrganizationKey(pRequestMessage.OrganizationKey))
                if (ValidateOrganizationKey(pRequestMessage.OrganizationKey))
                {

                                        if (ValidateSurveyFields(pRequestMessage))
                                        {
                                            try
                                            {

                                                Epi.Web.Common.BusinessObject.SurveyInfoBO BO = new Epi.Web.Common.BusinessObject.SurveyInfoBO();

                                                BO.SurveyId = SurveyId.ToString();
                                                BO.ClosingDate = pRequestMessage.ClosingDate;

                                                BO.IntroductionText = pRequestMessage.IntroductionText;
                                                BO.ExitText = pRequestMessage.ExitText;
                                                BO.DepartmentName = pRequestMessage.DepartmentName;
                                                BO.OrganizationName = pRequestMessage.OrganizationName;

                                                BO.SurveyNumber = pRequestMessage.SurveyNumber;

                                                BO.XML = pRequestMessage.XML;

                                                BO.SurveyName = pRequestMessage.SurveyName;

                                                BO.SurveyType = pRequestMessage.SurveyType;
                                                BO.UserPublishKey = pRequestMessage.UserPublishKey;
                                                BO.OrganizationKey = pRequestMessage.OrganizationKey;
                                                BO.OrganizationKey = pRequestMessage.OrganizationKey;
                                                BO.TemplateXMLSize = pRequestMessage.TemplateXMLSize;


                                                this.SurveyInfoDao.InsertSurveyInfo(BO);
                                                result.URL = GetURL(pRequestMessage, SurveyId);
                                                result.IsPulished = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                System.Console.Write(ex.ToString());
                                                //Entities.ObjectStateManager.GetObjectStateEntry(SurveyMetaData).Delete();
                                                result.URL = "";
                                                result.IsPulished = false;
                                                result.StatusText = "An Error has occurred while publishing your survey.";
                                            }




                                        }
                                        else
                                        {

                                            result.URL = "";
                                            result.IsPulished = false;
                                            result.StatusText = "One or more survey required fields are missing values.";
                                        }

                }
                else {

                    result.URL = "";
                    result.IsPulished = false;
                    result.StatusText = "Organization Key is invalid.";
                
                }
            }
            return result;
        }

        private static bool ValidateSurveyFields(SurveyInfoBO pRequestMessage)
        {

            bool isValid = true;


            if (pRequestMessage.ClosingDate == null)
            {

                isValid = false;

            }
            
            else if (string.IsNullOrEmpty(pRequestMessage.XML))
            {

                isValid = false;
            }
            else if (string.IsNullOrEmpty(pRequestMessage.SurveyName))
            {

                isValid = false;
            }
            
            else if ( string.IsNullOrEmpty(pRequestMessage.UserPublishKey.ToString()))
            {

                isValid = false;
            }


 
            return isValid;        
        }
     
        /// <summary>
        /// validate the Organization key passed with the list of Organization keys retrieved from database 
        /// through EF
        /// </summary>
        /// <param name="OrganizationKey"></param>
        /// <returns></returns>
        private bool ValidateOrganizationKey(Guid gOrganizationKey)
        {
            string strOrgKeyEncrypted = Epi.Web.Common.Security.Cryptography.Encrypt(gOrganizationKey.ToString());
            List<OrganizationBO> OrganizationBoList = this.OrganizationDao.GetOrganizationInfoByOrgKey(strOrgKeyEncrypted);
            if (OrganizationBoList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;    
            }
            
           
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
