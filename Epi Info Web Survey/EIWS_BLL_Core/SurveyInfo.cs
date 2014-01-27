﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using System.Xml;
using System.Xml.Linq;
namespace Epi.Web.BLL
{

  public  class SurveyInfo
    {
      private Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao SurveyInfoDao;


        public SurveyInfo(Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao pSurveyInfoDao)
        {
            this.SurveyInfoDao = pSurveyInfoDao;
        }

        public SurveyInfoBO GetSurveyInfoById(string pId)
        {
            List<string> IdList = new List<string>();
            if (! string.IsNullOrEmpty(pId))
            {
                IdList.Add(pId);
            }
            List<SurveyInfoBO> result = this.SurveyInfoDao.GetSurveyInfo(IdList);
            if (result.Count > 0)
            {
                return result[0];
            }
            else
            {
                return null;
            }
        }


     

        /// <summary>
        /// Gets SurveyInfo based on criteria
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>SurveyInfo.</returns>
        public List<SurveyInfoBO> GetSurveyInfoById(List<string> pIdList)
        {
            List<SurveyInfoBO> result = this.SurveyInfoDao.GetSurveyInfo(pIdList);
            return result;
        }

        public PageInfoBO GetSurveySizeInfo(List<string> pIdList,int BandwidthUsageFactor, int pResponseMaxSize = -1)
        { 
            List<SurveyInfoBO> SurveyInfoBOList = this.SurveyInfoDao.GetSurveySizeInfo(pIdList, -1, -1, pResponseMaxSize);

            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.BLL.Common.GetSurveySize(SurveyInfoBOList,BandwidthUsageFactor, pResponseMaxSize);
            return result;


        }


        public bool IsSurveyInfoValidByOrgKeyAndPublishKey(string SurveyId, string Okey, Guid publishKey)
        {

            string EncryptedKey = Epi.Web.Common.Security.Cryptography.Encrypt(Okey);
            List<SurveyInfoBO> result = this.SurveyInfoDao.GetSurveyInfoByOrgKeyAndPublishKey(SurveyId, EncryptedKey, publishKey);

             
            if (result != null && result.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public bool IsSurveyInfoValidByOrgKey(string SurveyId, string pOrganizationKey)
        {

            string EncryptedKey = Epi.Web.Common.Security.Cryptography.Encrypt(pOrganizationKey);
            List<SurveyInfoBO> result = this.SurveyInfoDao.GetSurveyInfoByOrgKey(SurveyId, EncryptedKey);


            if (result != null && result.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



      /// <summary>
        /// Gets SurveyInfo based on criteria
        /// </summary>
        /// <param name="SurveyInfoId">Unique SurveyInfo identifier.</param>
        /// <returns>SurveyInfo.</returns>
        public List<SurveyInfoBO> GetSurveyInfo(List<string> SurveyInfoIdList, DateTime pClosingDate, string Okey, int pSurveyType = -1, int pPageNumber = -1, int pPageSize = -1)
        {
            string EncryptedKey = Epi.Web.Common.Security.Cryptography.Encrypt(Okey);
            List<SurveyInfoBO> result = this.SurveyInfoDao.GetSurveyInfo(SurveyInfoIdList, pClosingDate, EncryptedKey, pSurveyType, pPageNumber, pPageSize);
            return result;
        }
        public PageInfoBO GetSurveySizeInfo(List<string> SurveyInfoIdList, DateTime pClosingDate, string Okey, int BandwidthUsageFactor, int pSurveyType = -1, int pPageNumber = -1, int pPageSize = -1, int pResponseMaxSize = -1)
        {

            string EncryptedKey = Epi.Web.Common.Security.Cryptography.Encrypt(Okey);

            List<SurveyInfoBO> SurveyInfoBOList = this.SurveyInfoDao.GetSurveySizeInfo(SurveyInfoIdList, pClosingDate, EncryptedKey, pSurveyType, pPageNumber, pPageSize, pResponseMaxSize);

            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.BLL.Common.GetSurveySize(SurveyInfoBOList, BandwidthUsageFactor, pResponseMaxSize);
            return result;

        }
      
        public SurveyInfoBO InsertSurveyInfo(SurveyInfoBO pValue)
        {
            SurveyInfoBO result = pValue;
            this.SurveyInfoDao.InsertSurveyInfo(pValue);
            return result;
        }



        public SurveyInfoBO UpdateSurveyInfo(SurveyInfoBO pValue)
        {
            SurveyInfoBO result = pValue;
            if (ValidateSurveyFields(pValue))
            {
                this.SurveyInfoDao.UpdateSurveyInfo(pValue);
                result.StatusText = "Successfully updated survey information.";
            }else{
                result.StatusText = "One or more survey required fields are missing values.";
            
            }
            
            return result;
        }

        public bool DeleteSurveyInfo(SurveyInfoBO pValue)
        {
            bool result = false;

            this.SurveyInfoDao.DeleteSurveyInfo(pValue);
            result = true;

            return result;
        }
        private static bool ValidateSurveyFields(SurveyInfoBO pRequestMessage)
        {

            bool isValid = true;


            if (pRequestMessage.ClosingDate == null)
            {

                isValid = false;

            }

           
            else if (string.IsNullOrEmpty(pRequestMessage.SurveyName))
            {

                isValid = false;
            }
 



            return isValid;
        }


        public Web.Common.Message.SurveyControlsResponse GetSurveyControlList(string SurveyId)
            {
            Web.Common.Message.SurveyControlsResponse SurveyControlsResponse = new Web.Common.Message.SurveyControlsResponse();
            SurveyInfoBO SurveyInfoBO = new SurveyInfoBO(); 
            SurveyInfoBO = GetSurveyInfoById(SurveyId);
            List<Web.Common.DTO.SurveyControlDTO> SurveyControlList = new List<Web.Common.DTO.SurveyControlDTO>();
            SurveyControlList = GetSurveyControls(SurveyInfoBO);
            SurveyControlsResponse.SurveyControlList = SurveyControlList;
            return SurveyControlsResponse;
            }
        private List<Web.Common.DTO.SurveyControlDTO> GetSurveyControls(SurveyInfoBO SurveyInfoBO)
            {
            List<Web.Common.DTO.SurveyControlDTO> List = new List<Web.Common.DTO.SurveyControlDTO>();
            Web.Common.DTO.SurveyControlDTO SurveyControlDTO = new Web.Common.DTO.SurveyControlDTO();
            XDocument xdoc = XDocument.Parse(SurveyInfoBO.XML);


            var _FieldsTypeIDs = from _FieldTypeID in
                                      xdoc.Descendants("Field")
                                      select _FieldTypeID;
            
            foreach (var _FieldTypeID in _FieldsTypeIDs)
                {
                SurveyControlDTO.ControlId = _FieldTypeID.Attribute("Name").Value.ToString();
                SurveyControlDTO.ControlPrompt = _FieldTypeID.Attribute("PromptText").Value.ToString();
                SurveyControlDTO.ControlType = GetControlType(_FieldTypeID.Attribute("FieldTypeId").Value);
                List.Add(SurveyControlDTO);
                     
                }
            return List;

            }
        private string GetControlType(string Type) 
            {
            string ControlType="";
            switch (Type)
                {
                case "1":
                case "3"://TextBox
                    ControlType="TextBox";
                    break;

                case "2"://Literal
                    ControlType = "Literal";
                    break;

                case "4"://TextArea
                    ControlType = "TextArea"; 
                    break;

                case "5"://NumericTextBox
                    ControlType = "NumericTextBox";
                    break;

                case "7"://DatePicker
                    ControlType = "Date";
                    break;

                case "8"://TimePicker
                    ControlType = "Time"; 
                    break;

                case "10"://CheckBox
                    ControlType = "CheckBox"; 
                    break;

                case "11": // YesNo
                    ControlType = "YesNo";
                    break;

                case "12"://GroupBoxRadioList
                    ControlType = "GroupBoxRadioList";
                    break;

                case "13"://Button
                    ControlType = "Button";
                    break;

                case "17": // LegalValues/DropDown
                    ControlType = "LegalValues";
                    break;

                case "18": // Codes
                    ControlType = "Codes";
                    break;

                case "19": // CommentLegal
                    ControlType = "CommentLegal";
                    break;

                case "21": //GroupBox
                    ControlType = "GroupBox";
                    break;
                }

            return ControlType;
            }

    }
}
