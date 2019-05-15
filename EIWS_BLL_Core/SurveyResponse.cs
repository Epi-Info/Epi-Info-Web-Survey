using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using Epi.Web.Common.Message;
using Epi.Web.Common.ObjectMapping;
using Epi.Web.Interfaces.DataInterfaces;
using Epi.Web.Common.Xml;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.BusinessRule;
using Epi.Web.Common.Json;

namespace Epi.Web.BLL
{
    public class SurveyResponse
    {

        public enum Message
            {
              Failed = 1,
              Success = 2,
           
            }
        private Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao;
        private ISurveyInfoDao SurveyInfoDao;
        public SurveyResponse(Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao pSurveyResponseDao, ISurveyInfoDao pSurveyInfoDao)
        {
            this.SurveyResponseDao = pSurveyResponseDao;
            this.SurveyInfoDao = pSurveyInfoDao;
        }

        public List<SurveyResponseBO> GetSurveyResponseById(List<String> pId, Guid UserPublishKey)
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponse(pId, UserPublishKey);
            return result;
        }

        public List<SurveyResponseBO> GetFormResponseListById(string FormId, int PageNumber, bool IsMobile)
        {
            List<SurveyResponseBO> result = null;

            int PageSize;
            if (IsMobile)
            {
                PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE_Mobile"]);
            }
            else
            {
                PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE"]);
            }
            result = this.SurveyResponseDao.GetFormResponseByFormId(FormId, PageNumber, PageSize);
            return result;
        }
        public List<SurveyResponseBO> GetFormResponseListById(SurveyAnswerCriteria criteria)
        {
            List<SurveyResponseBO> result = null;

            //int PageSize;
            if (criteria.IsMobile)
            {
                criteria.PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE_Mobile"]);
            }
            else
            {
                criteria.PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE"]);
            }
            result = this.SurveyResponseDao.GetFormResponseByFormId(criteria);
            return result;
        }
        public int GetNumberOfPages(string FormId, bool IsMobile)
        {
            int PageSize;
            if (IsMobile)
            {
                PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE_Mobile"]);
            }
            else
            {
                PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE"]);
            }
            int result = this.SurveyResponseDao.GetFormResponseCount(FormId);
            if (PageSize > 0)
            {
                result = (result + PageSize - 1) / PageSize;
            }
            return result;
        }

        public int GetNumberOfPages(SurveyAnswerCriteria Criteria)
        {
            //int PageSize;
            if (Criteria.IsMobile)
            {
                Criteria.PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE_Mobile"]);
            }
            else
            {
                Criteria.PageSize = Int32.Parse(ConfigurationManager.AppSettings["RESPONSE_PAGE_SIZE"]);
            }
            int result = this.SurveyResponseDao.GetFormResponseCount(Criteria);
            if (Criteria.PageSize > 0)
            {
                result = (result + Criteria.PageSize - 1) / Criteria.PageSize;
            }
            return result;
        }
        //Validate User
        public bool ValidateUser(UserAuthenticationRequestBO PassCodeBoObj)
        {
            string PassCode = PassCodeBoObj.PassCode;
            string ResponseId = PassCodeBoObj.ResponseId;
            List<string> ResponseIdList = new List<string> ();
            ResponseIdList.Add(PassCodeBoObj.ResponseId);
 
            UserAuthenticationResponseBO results = this.SurveyResponseDao.GetAuthenticationResponse(PassCodeBoObj);
                
             

            bool ISValidUser = false;

            if (results != null && !string.IsNullOrEmpty(PassCode))
            {

                if (results.PassCode == PassCode)
                {
                    ISValidUser = true;


                }
                else
                {
                    ISValidUser = false;
                }
            }
            return ISValidUser;
        }
        //Save Pass code 
        public void SavePassCode( UserAuthenticationRequestBO pValue)
        {
            UserAuthenticationRequestBO result = pValue;
            this.SurveyResponseDao.UpdatePassCode(pValue);
          

           
        }
        // Get Authentication Response
        public UserAuthenticationResponseBO GetAuthenticationResponse(UserAuthenticationRequestBO pValue)
        {
            UserAuthenticationResponseBO result = this.SurveyResponseDao.GetAuthenticationResponse(pValue);

            return result; 

        }
        public List<SurveyResponseBO> GetSurveyResponseBySurveyId(List<String> pSurveyIdList, Guid UserPublishKey)
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponseBySurveyId(pSurveyIdList, UserPublishKey);
            return result;
        }

        public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted,bool pIsDraftMode  ,int pStatusId,int PageNumber,int PageSize )
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponse(SurveyAnswerIdList, pSurveyId, pDateCompleted,pIsDraftMode, pStatusId,PageNumber,PageSize);
            return result;
        }

        public SurveyResponseBO InsertSurveyResponse(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            this.SurveyResponseDao.InsertSurveyResponse(pValue);
            return result;
        }

        public SurveyResponseBO InsertSurveyResponseApi(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            this.SurveyResponseDao.InsertSurveyResponseApi(pValue);
            return result;
        }
        public List<SurveyResponseBO> InsertSurveyResponse(List<SurveyResponseBO> pValue, int UserId, bool IsNewRecord = false)
        {

            foreach (var item in pValue)
            {
                ResponseXmlBO ResponseXmlBO = new ResponseXmlBO();
                ResponseXmlBO.User = UserId;
                ResponseXmlBO.ResponseId = item.ResponseId;
                ResponseXmlBO.Xml = item.XML;
                ResponseXmlBO.IsNewRecord = IsNewRecord;
                this.SurveyResponseDao.InsertResponseXml(ResponseXmlBO);

            }

            return pValue;
        }

        //public List<SurveyResponseBO> InsertSurveyResponse(List<SurveyResponseBO> pValue, int UserId, bool IsNewRecord = false)
        //{

        //    foreach (var item in pValue)
        //    {
        //        ResponseXmlBO ResponseXmlBO = new ResponseXmlBO();
        //        ResponseXmlBO.User = UserId;
        //        ResponseXmlBO.ResponseId = item.ResponseId;
        //        ResponseXmlBO.Xml = item.XML;
        //        ResponseXmlBO.IsNewRecord = IsNewRecord;
        //        this.SurveyResponseDao.InsertResponseXml(ResponseXmlBO);

        //    }

        //    return pValue;
        //}



        public SurveyResponseBO InsertChildSurveyResponse(SurveyResponseBO pValue, SurveyInfoBO ParentSurveyInfo, string RelateParentId)
        {

            SurveyResponseBO result = pValue;
            pValue.ParentRecordId = ParentSurveyInfo.ParentId;
            pValue.RelateParentId = RelateParentId;
            this.SurveyResponseDao.InsertChildSurveyResponse(pValue);
            return result;
        }

        public SurveyResponseBO UpdateSurveyResponse(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            //Check if this respose has prent
            string ParentId = SurveyResponseDao.GetResponseParentId(pValue.ResponseId);
            Guid ParentIdGuid = Guid.Empty;
            if (!string.IsNullOrEmpty(ParentId) )
                {
                       ParentIdGuid = new Guid(ParentId);
                }
            
            //if ( pValue.Status == 2 && ParentIdGuid!= Guid.Empty )
            //{
            //if (!string.IsNullOrEmpty(ParentId) && ParentId != Guid.Empty.ToString() && pValue.Status == 2)
            //    {
            //    //read the child 

            //    SurveyResponseBO Child = this.SurveyResponseDao.GetSingleResponse(pValue.ResponseId);
            //    // read the parent
            //    SurveyResponseBO Parent = this.SurveyResponseDao.GetSingleResponse(ParentId);
            //    //copy and update
            //    Parent.XML = Child.XML;
            //    this.SurveyResponseDao.UpdateSurveyResponse(Parent);
            //    result = Parent;
            //    //Check if this child has a related form (subchild)
            //    List<SurveyResponseBO> Children = this.GetResponsesHierarchyIdsByRootId(Child.ResponseId);
            //    if (Children.Count() > 1)
            //    {
            //        SurveyResponseBO NewChild = Children[1];
            //        NewChild.RelateParentId = Parent.ResponseId;
            //        this.SurveyResponseDao.UpdateSurveyResponse(NewChild);
            //    }
            //    // Set  child recod UserId
            //    Child.UserId = pValue.UserId;
            //    // delete the child
            //    this.DeleteSingleSurveyResponse(Child);

            //}
            //else
            //{
                //Check if the record existes.If it does update otherwise insert new 
                this.SurveyResponseDao.UpdateSurveyResponse(pValue);

                SurveyResponseBO SurveyResponseBO = SurveyResponseDao.GetResponseXml(pValue.ResponseId);



          //  }
            return result;
        }

        public bool DeleteSurveyResponse(SurveyResponseBO pValue)
        {
            bool result = false;

            this.SurveyResponseDao.DeleteSurveyResponse(pValue);
            result = true;

            return result;
        }

        public PageInfoBO GetResponseSurveySize(List<string> SurveyResponseIdList, string SurveyId, DateTime pClosingDate, int BandwidthUsageFactor, bool pIsDraftMode = false, int pSurveyType = -1, int pPageNumber = -1, int pPageSize = -1, int pResponseMaxSize = -1)
        {
            List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseSize(SurveyResponseIdList, SurveyId, pClosingDate,pIsDraftMode, pSurveyType, pPageNumber, pPageSize, pResponseMaxSize);
            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, pResponseMaxSize);
            return result;
        }

        //public PageInfoBO GetSurveyResponseBySurveyIdSize(List<string> SurveyIdList, Guid UserPublishKey, int BandwidthUsageFactor, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        //{
        //    List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseBySurveyIdSize(SurveyIdList, UserPublishKey, PageNumber, PageSize, ResponseMaxSize);

        //    PageInfoBO result = new PageInfoBO();

        //    result = Epi.Web.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, ResponseMaxSize);
        //    return result;
         
        //}
        //public PageInfoBO GetSurveyResponseSize(List<string> SurveyResponseIdList, Guid UserPublishKey, int BandwidthUsageFactor, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        //{

        //    List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseSize(SurveyResponseIdList, UserPublishKey, PageNumber, PageSize, ResponseMaxSize);

        //    PageInfoBO result = new PageInfoBO();

        //    result = Epi.Web.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, ResponseMaxSize);
        //    return result;
        //}
        //public int GetNumberOfResponses(string FormId)
        //{

        //    int result = this.SurveyResponseDao.GetFormResponseCount(FormId);

        //    return result;
        //}

        public int GetNumberOfResponses(SurveyAnswerCriteria Criteria)
        {

            int result = this.SurveyResponseDao.GetFormResponseCount(Criteria);

            return result;
        }
        public List<SurveyResponseBO> GetResponsesHierarchyIdsByRootId(string RootId)
        {
            List<SurveyResponseBO> SurveyResponseBO = new List<SurveyResponseBO>();

            SurveyResponseBO = this.SurveyResponseDao.GetResponsesHierarchyIdsByRootId(RootId);


            return SurveyResponseBO;

        }
        public SurveyResponseBO GetFormResponseByParentRecordId(string ParentRecordId)
        {
            SurveyResponseBO SurveyResponseBO = new SurveyResponseBO();

            SurveyResponseBO = this.SurveyResponseDao.GetFormResponseByParentRecordId(ParentRecordId);
            return SurveyResponseBO;
        }

        public List<SurveyResponseBO> GetAncestorResponseIdsByChildId(string ChildId)
        {
            List<SurveyResponseBO> SurveyResponseBO = new List<SurveyResponseBO>();

            SurveyResponseBO = this.SurveyResponseDao.GetAncestorResponseIdsByChildId(ChildId);


            return SurveyResponseBO;

        }
        public List<SurveyResponseBO> GetResponsesByRelatedFormId(string ResponseId, string SurveyId)
        {
            List<SurveyResponseBO> SurveyResponseBO = new List<SurveyResponseBO>();

            SurveyResponseBO = this.SurveyResponseDao.GetResponsesByRelatedFormId(ResponseId, SurveyId);


            return SurveyResponseBO;

        }

        public List<SurveyResponseBO> GetResponsesByRelatedFormId(string ResponseId, SurveyAnswerCriteria Criteria)
        {
            List<SurveyResponseBO> SurveyResponseBO = new List<SurveyResponseBO>();

            SurveyResponseBO = this.SurveyResponseDao.GetResponsesByRelatedFormId(ResponseId, Criteria.SurveyId);


            return SurveyResponseBO;

        }
        public string CreateResponseXml(Epi.Web.Common.Message.PreFilledAnswerRequest request, List<SurveyInfoBO> SurveyBOList)
            
            {
           
            string ResponseXml;
          
            XDocument SurveyXml = new XDocument();

            foreach (var item in SurveyBOList)
                {
                SurveyXml = XDocument.Parse(item.XML);
                }
             SurveyResponseXML Implementation = new  SurveyResponseXML(request, SurveyXml);
            ResponseXml = Implementation.CreateResponseDocument(SurveyXml).ToString();


            return ResponseXml;
            }
        public Dictionary<string, string> ValidateResponse(List<SurveyInfoBO> SurveyBOList,  PreFilledAnswerRequest request)
            {
             
            XDocument SurveyXml = new XDocument();
            foreach (var item in SurveyBOList)
                {
                SurveyXml = XDocument.Parse(item.XML);
                }
              Dictionary<string, string> MessageList = new Dictionary<string, string>();
              Dictionary<string, string> FieldNotFoundList = new Dictionary<string, string>();
              Dictionary<string, string> WrongFieldTypeList = new Dictionary<string, string>();
            SurveyResponseXML Implementation = new  SurveyResponseXML(request, SurveyXml);
            FieldNotFoundList = Implementation.ValidateResponseFileds();
            WrongFieldTypeList = Implementation.ValidateResponseFiledTypes();
            MessageList = MessageList.Union(FieldNotFoundList).Union(WrongFieldTypeList).ToDictionary(k => k.Key, v => v.Value);
            return MessageList;


            }
        public List<SurveyInfoBO> GetSurveyInfo( PreFilledAnswerRequest request)
            {
            
            List<string> SurveyIdList = new List<string>();
            string SurveyId =request.AnswerInfo.SurveyId.ToString();
            string OrganizationId = request.AnswerInfo.OrganizationKey.ToString();
            Guid UserPublishKey = request.AnswerInfo.UserPublishKey;
            List<SurveyInfoBO> SurveyBOList = new List<SurveyInfoBO>();


           
            SurveyIdList.Add(SurveyId);
 
            Epi.Web.Common.Message.SurveyInfoRequest pRequest = new Epi.Web.Common.Message.SurveyInfoRequest();
            var criteria = pRequest.Criteria as Epi.Web.Common.Criteria.SurveyInfoCriteria;

             IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
             ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;
             SurveyInfo implementation = new  SurveyInfo(surveyInfoDao);

            SurveyBOList = implementation.GetSurveyInfo(SurveyIdList, criteria.ClosingDate, OrganizationId, criteria.SurveyType, criteria.PageNumber, criteria.PageSize);//Default 
               
            return SurveyBOList;

            }
        public  PreFilledAnswerResponse SetSurveyAnswer( PreFilledAnswerRequest request)
            {
            string SurveyId = request.AnswerInfo.SurveyId.ToString();
            string OrganizationId = request.AnswerInfo.OrganizationKey.ToString();
            Guid UserPublishKey = request.AnswerInfo.UserPublishKey;
            Dictionary<string, string> ErrorMessageList = new Dictionary<string, string>();
            PreFilledAnswerResponse response;
            

            SurveyResponseBO SurveyResponse = new SurveyResponseBO();
            UserAuthenticationRequestBO UserAuthenticationRequestBO = new  UserAuthenticationRequestBO();

            bool IsValidOrgKeyAndPublishKey = IsSurveyInfoValidByOrgKeyAndPublishKey(SurveyId, OrganizationId, UserPublishKey);


            if (IsValidOrgKeyAndPublishKey)
                {
                //Get Survey Info (MetaData)
                List<SurveyInfoBO> SurveyBOList = GetSurveyInfo(request);
                //Build Survey Response Xml

                string Xml = CreateResponseXml(request, SurveyBOList);
                //Validate Response values
                var responseid = request.AnswerInfo.SurveyQuestionAnswerList.Where(x => x.Key.ToLower() == "responseid" || x.Key.ToLower() == "id").FirstOrDefault();
                if (responseid.Key != null)
                {
                    request.AnswerInfo.SurveyQuestionAnswerList.Remove(responseid.Key);
                }
                var relateparentid = request.AnswerInfo.SurveyQuestionAnswerList.Where(x => x.Key.ToLower() == "fkey").FirstOrDefault();
                if (relateparentid.Key != null)
                {
                    request.AnswerInfo.SurveyQuestionAnswerList.Remove(relateparentid.Key);
                }

                var updatedtime = request.AnswerInfo.SurveyQuestionAnswerList.Where(x => x.Key.ToLower() == "_updatestamp").FirstOrDefault();
                if (updatedtime.Key != null)
                {
                    request.AnswerInfo.SurveyQuestionAnswerList.Remove(updatedtime.Key);
                }

                ErrorMessageList = ValidateResponse(SurveyBOList, request);

                if (ErrorMessageList.Count() > 0)
                    {
                    response = new Epi.Web.Common.Message.PreFilledAnswerResponse();
                    response.ErrorMessageList = ErrorMessageList;
                    response.Status = ((Message)1).ToString();
                    }
                if (responseid.Value == null && updatedtime.Value == null)
                {
                    //Insert Survey Response

                    SurveyResponse = InsertSurveyResponse(Mapper.ToBusinessObject(Xml, request.AnswerInfo.SurveyId.ToString()));
                }
                else
                {//Insert  Response Api
                    if (relateparentid.Value == null)
                    {
                        SurveyResponseBO surveyresponseBO = new SurveyResponseBO();
                        surveyresponseBO.SurveyId = request.AnswerInfo.SurveyId.ToString();
                        surveyresponseBO.ResponseId = responseid.Value.ToString();
                        surveyresponseBO.XML = Xml;
                        surveyresponseBO.Status = 3;
                        surveyresponseBO.RecrodSourceId = (int)ValidationRecordSourceId.MA;
                        System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        if (updatedtime.Value != null)
                        {
                            surveyresponseBO.DateUpdated = dateTime.AddMilliseconds(Convert.ToDouble(updatedtime.Value.ToString())).ToLocalTime();
                        }
                        else
                        {
                            surveyresponseBO.DateUpdated = DateTime.Now;
                        }
                        surveyresponseBO.DateCreated = surveyresponseBO.DateUpdated;
                        surveyresponseBO.DateCompleted = surveyresponseBO.DateUpdated;
                        SurveyResponse = InsertSurveyResponseApi(surveyresponseBO);
                    }
                    else
                    {
                        try
                        {
                            var survey = GetSurveyResponseById(new List<string> { relateparentid.Value }, UserPublishKey);
                        }
                        catch (Exception ex)//insert parent response
                        {
                            SurveyResponseBO surveyresponsebO = new SurveyResponseBO();
                            surveyresponsebO.SurveyId = SurveyBOList[0].ParentId;
                            surveyresponsebO.ResponseId = relateparentid.Value.ToString();
                            surveyresponsebO.XML = "  ";
                            surveyresponsebO.Status = 3;
                            surveyresponsebO.RecrodSourceId = (int)ValidationRecordSourceId.MA;
                            surveyresponsebO.DateUpdated = DateTime.Now;
                            surveyresponsebO.DateCreated = surveyresponsebO.DateUpdated;
                            surveyresponsebO.DateCompleted = surveyresponsebO.DateUpdated;
                            surveyresponsebO = InsertSurveyResponseApi(surveyresponsebO);
                        }
                        //insert child response
                        SurveyResponseBO surveyresponseBO = new SurveyResponseBO();
                        surveyresponseBO.SurveyId = request.AnswerInfo.SurveyId.ToString();
                        surveyresponseBO.ResponseId = responseid.Value.ToString();
                        surveyresponseBO.XML = Xml;
                        surveyresponseBO.Status = 3;
                        surveyresponseBO.RecrodSourceId = (int)ValidationRecordSourceId.MA;
                        surveyresponseBO.RelateParentId = relateparentid.Value;
                        System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        if (updatedtime.Value != null)
                        {
                            surveyresponseBO.DateUpdated = dateTime.AddMilliseconds(Convert.ToDouble(updatedtime.Value.ToString())).ToLocalTime();
                        }
                        else
                        {
                            surveyresponseBO.DateUpdated = DateTime.Now;
                        }
                        surveyresponseBO.DateCreated = surveyresponseBO.DateUpdated;
                        surveyresponseBO.DateCompleted = surveyresponseBO.DateUpdated;
                        SurveyResponse = InsertSurveyResponseApi(surveyresponseBO);

                    }
                }

                //Save PassCode

                UserAuthenticationRequestBO =  Mapper.ToBusinessObject(SurveyResponse.ResponseId);
                    SavePassCode(UserAuthenticationRequestBO);

                    //return Response
                    string ResponseUrl = ConfigurationManager.AppSettings["ResponseURL"];
                    response = new  PreFilledAnswerResponse( Mapper.ToDataTransferObjects(UserAuthenticationRequestBO));
                    response.SurveyResponseUrl = ResponseUrl + UserAuthenticationRequestBO.ResponseId;
                    response.Status = ((Message)2).ToString(); ;

                    }
                
            else
                {
                PassCodeDTO DTOList = new   PassCodeDTO();
                response = new PreFilledAnswerResponse(DTOList);
                ErrorMessageList.Add("Keys", "Organization key and/or User publish key are invalid.");
                response.ErrorMessageList = ErrorMessageList;
                response.Status = ((Message)1).ToString(); 
                }
            return response;
            }
        public bool HasResponse(string SurveyId, string ResponseId)
            {
            SurveyAnswerCriteria SurveyAnswerCriteria = new SurveyAnswerCriteria();
            SurveyAnswerCriteria.SurveyId = SurveyId;
            SurveyAnswerCriteria.SurveyAnswerIdList = new List<string>();
            SurveyAnswerCriteria.SurveyAnswerIdList.Add(ResponseId);

            return SurveyResponseDao.HasResponse(SurveyAnswerCriteria);
            }
        private bool IsSurveyInfoValidByOrgKeyAndPublishKey(string SurveyId, string Orgkey, Guid publishKey)
            {
            IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;

            string EncryptedKey = Epi.Web.Common.Security.Cryptography.Encrypt(Orgkey.ToString());
            List<SurveyInfoBO> result = surveyInfoDao.GetSurveyInfoByOrgKeyAndPublishKey(SurveyId.ToString(), EncryptedKey, publishKey);


            if (result != null && result.Count > 0)
                {
                return true;
                }
            else
                {
                return false;
                }
            }

        public void UpdateRecordStatus(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            this.SurveyResponseDao.UpdateRecordStatus(pValue);
            
        }

        public void InsertErrorLog(Dictionary<string, string> pValue)
        {
            this.SurveyResponseDao.InsertErrorLog(pValue);
        }

        public bool  SetJsonColumn(string json , string responseid)
        {
            try
            {
                this.SurveyResponseDao.SetJsonColumn(json, responseid);
                return true;
            } catch (Exception ex) {

                return false;

            }
        }

        

    }
}
