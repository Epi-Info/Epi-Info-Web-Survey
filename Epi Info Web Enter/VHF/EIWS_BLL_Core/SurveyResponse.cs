using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using System.Configuration;
using Epi.Web.Enter.Common.Extension;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Common.Message;
using Epi.Web.Enter.Common.ObjectMapping;
using System.Xml;
using System.Xml.Linq;
using Epi.Web.Enter.Common.Xml;
namespace Epi.Web.BLL
{
    public class SurveyResponse
    {
         public enum Message
            {
              Failed = 1,
              Success = 2,
           
            }
        private Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao;

        public SurveyResponse(Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao pSurveyResponseDao)
        {
            this.SurveyResponseDao = pSurveyResponseDao;
        }

     
        public List<SurveyResponseBO> GetSurveyResponseById(SurveyAnswerCriteria Criteria, List<SurveyInfoBO> SurveyBOList = null)
        {

            //Check if this Response exists in EWE DataBase
        Guid Id = new Guid(Criteria.SurveyAnswerIdList[0]);
        bool ResponseExists = this.SurveyResponseDao.ISResponseExists(Id);
           List<SurveyResponseBO> result = new List<SurveyResponseBO>();
           if (ResponseExists)
               {
               result = this.SurveyResponseDao.GetSurveyResponse(Criteria.SurveyAnswerIdList, Criteria.UserPublishKey);
              
               }
           else 
               {

               //Get Form Name
              // string 
               //Retrieve response data sets from Epi 7 DataBase
               SurveyAnswerCriteria SurveyAnswerCriteria = new Enter.Common.Criteria.SurveyAnswerCriteria ();
               SurveyAnswerCriteria.GetAllColumns = true;
               SurveyAnswerCriteria.SurveyId = Criteria.SurveyId;
               SurveyAnswerCriteria.SurveyAnswerIdList.Add(Criteria.SurveyAnswerIdList[0]);
               SurveyAnswerCriteria.PageSize = 1;
               SurveyAnswerCriteria.PageNumber = 1;
               SurveyAnswerCriteria.IsSqlProject = Criteria.IsSqlProject;
               result = this.SurveyResponseDao.GetFormResponseByFormId(SurveyAnswerCriteria);

               var DataList = result[0].SqlData.ToList();
               DataList.RemoveAt(0);
              
               //Build Response Xml
                PreFilledAnswerRequest Request = new PreFilledAnswerRequest();
                Request.AnswerInfo.ResponseId = new Guid(Criteria.SurveyAnswerIdList[0]);
                Request.AnswerInfo.SurveyId = new Guid(Criteria.SurveyId);
                Request.AnswerInfo.UserId = Criteria.UserId;
                Request.AnswerInfo.SurveyQuestionAnswerList = new Dictionary<string, string>();
                foreach (var item in DataList)
                    {
                     
                   
                    Request.AnswerInfo.SurveyQuestionAnswerList.Add(item.Key, item.Value);

                    }
              //  Request.AnswerInfo.OrganizationKey = new Guid ( "a4b6a687-610d-442a-a80c-d1c781087181");
              var response = SetSurveyAnswer(Request);
              // string Xml = CreateResponseXml(  Request,  SurveyBOList);

                //Insert response xml into EWE

              result = this.SurveyResponseDao.GetSurveyResponse(Criteria.SurveyAnswerIdList, Criteria.UserPublishKey);
               }
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
            List<string> ResponseIdList = new List<string>();
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
        public void SavePassCode(UserAuthenticationRequestBO pValue)
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

        public List<SurveyResponseBO> GetSurveyResponse(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId)
        {
            List<SurveyResponseBO> result = this.SurveyResponseDao.GetSurveyResponse(SurveyAnswerIdList, pSurveyId, pDateCompleted, pStatusId);
            return result;
        }

        public SurveyResponseBO InsertSurveyResponse(SurveyResponseBO pValue)
        {
            SurveyResponseBO result = pValue;
            this.SurveyResponseDao.InsertSurveyResponse(pValue);
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



        public SurveyResponseBO InsertChildSurveyResponse(SurveyResponseBO pValue, SurveyInfoBO ParentSurveyInfo, string RelateParentId)
        {

            SurveyResponseBO result = pValue;
            pValue.ParentId = ParentSurveyInfo.ParentId;
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
        public List<SurveyResponseBO> UpdateSurveyResponse(List<SurveyResponseBO> pValue, int Status)
        {
            List<SurveyResponseBO> result = pValue;
            //Check if this respose has prent
            foreach (var Obj in pValue.ToList())
            {
                //string ParentId = SurveyResponseDao.GetResponseParentId(Obj.ResponseId);
                //if (!string.IsNullOrEmpty(ParentId) && ParentId != Guid.Empty.ToString() && Status == 2)
                //{
                //    //read the child 

                //    SurveyResponseBO Child = this.SurveyResponseDao.GetSingleResponse(Obj.ResponseId);
                //    // read the parent
                //    SurveyResponseBO Parent = this.SurveyResponseDao.GetSingleResponse(ParentId);
                //    //copy and update
                //    Parent.XML = Child.XML;
                //    Parent.Status = Status;
                //    this.SurveyResponseDao.UpdateSurveyResponse(Parent);
                //    result.Add(Parent);
                //    // Set  child recod UserId
                //    Child.UserId = Obj.UserId;
                //    // delete the child
                //    this.DeleteSurveyResponse(Child);

                //}
                //else
                //{
                    Obj.Status = Status;
                    this.SurveyResponseDao.UpdateSurveyResponse(Obj);
               // }
            }
            return result;
        }
        public void UpdateFormResponse(SurveyResponseBO pValue)
            {
          
                this.SurveyResponseDao.UpdateSurveyResponse(pValue);
             }
        public bool DeleteSurveyResponse(SurveyResponseBO pValue)
        {
            bool result = false;

            this.SurveyResponseDao.DeleteSurveyResponse(pValue);
            result = true;

            return result;
        }
        public bool DeleteSurveyResponseInEditMode(SurveyResponseBO pValue)
        {
            bool result = false;
            List<SurveyResponseBO> Children = this.GetResponsesHierarchyIdsByRootId(pValue.ResponseId);

            foreach (var child in Children)
            {
                //Get the original copy of the xml
                SurveyResponseBO ResponseXml = this.SurveyResponseDao.GetResponseXml(child.ResponseId);
                if (!ResponseXml.IsNewRecord)
                {
                    child.XML = ResponseXml.XML;
                    this.SurveyResponseDao.UpdateSurveyResponse(child);
                }
                else
                {
                    child.UserId = pValue.UserId;
                    this.SurveyResponseDao.DeleteSurveyResponse(child);

                }
                // delete record from ResponseXml Table

                ResponseXmlBO ResponseXmlBO = new ResponseXmlBO();
                ResponseXmlBO.ResponseId = child.ResponseId;
                this.SurveyResponseDao.DeleteResponseXml(ResponseXmlBO);

            }

            result = true;

            return result;
        }
        public bool DeleteSingleSurveyResponse(SurveyResponseBO pValue)
        {
            bool result = false;

            this.SurveyResponseDao.DeleteSingleSurveyResponse(pValue);
            result = true;

            return result;
        }

        public PageInfoBO GetResponseSurveySize(List<string> SurveyResponseIdList, string SurveyId, DateTime pClosingDate, int BandwidthUsageFactor, int pSurveyType = -1, int pPageNumber = -1, int pPageSize = -1, int pResponseMaxSize = -1)
        {
            List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseSize(SurveyResponseIdList, SurveyId, pClosingDate, pSurveyType, pPageNumber, pPageSize, pResponseMaxSize);
            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, pResponseMaxSize);
            return result;
        }

        public PageInfoBO GetSurveyResponseBySurveyIdSize(List<string> SurveyIdList, Guid UserPublishKey, int BandwidthUsageFactor, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {
            List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseBySurveyIdSize(SurveyIdList, UserPublishKey, PageNumber, PageSize, ResponseMaxSize);

            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, ResponseMaxSize);
            return result;

        }
        public PageInfoBO GetSurveyResponseSize(List<string> SurveyResponseIdList, Guid UserPublishKey, int BandwidthUsageFactor, int PageNumber = -1, int PageSize = -1, int ResponseMaxSize = -1)
        {

            List<SurveyResponseBO> SurveyResponseBOList = this.SurveyResponseDao.GetSurveyResponseSize(SurveyResponseIdList, UserPublishKey, PageNumber, PageSize, ResponseMaxSize);

            PageInfoBO result = new PageInfoBO();

            result = Epi.Web.BLL.Common.GetSurveySize(SurveyResponseBOList, BandwidthUsageFactor, ResponseMaxSize);
            return result;
        }
        public int GetNumberOfResponses(string FormId)
        {

            int result = this.SurveyResponseDao.GetFormResponseCount(FormId);

            return result;
        }

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

            SurveyResponseBO = this.SurveyResponseDao.GetResponsesByRelatedFormId(ResponseId, Criteria);


            return SurveyResponseBO;

        }

        public SurveyResponseBO GetResponseXml(string ResponseId)
        {
            SurveyResponseBO SurveyResponseBO = new SurveyResponseBO();

            SurveyResponseBO = this.SurveyResponseDao.GetResponseXml(ResponseId);

            return SurveyResponseBO;
        }

        public void DeleteResponseXml(ResponseXmlBO ResponseXmlBO)
        {

            this.SurveyResponseDao.DeleteResponseXml(ResponseXmlBO);
        }
        public PreFilledAnswerResponse SetSurveyAnswer(PreFilledAnswerRequest request)
            {
            string SurveyId = request.AnswerInfo.SurveyId.ToString();
            string ResponseId = request.AnswerInfo.ResponseId.ToString();
            Guid ParentRecordId = request.AnswerInfo.ParentRecordId;
            Dictionary<string, string> ErrorMessageList = new Dictionary<string, string>();
            PreFilledAnswerResponse response ;


            SurveyResponseBO SurveyResponse = new SurveyResponseBO();
            UserAuthenticationRequestBO UserAuthenticationRequestBO = new UserAuthenticationRequestBO();
            //Get Survey Info (MetaData)
                List<SurveyInfoBO> SurveyBOList = GetSurveyInfo(request);
                //Build Survey Response Xml

                string Xml = CreateResponseXml(request, SurveyBOList);
                //Validate Response values

                ErrorMessageList = ValidateResponse(SurveyBOList, request);

                if (ErrorMessageList.Count() > 0)
                    {
                    response = new PreFilledAnswerResponse();
                    response.ErrorMessageList = ErrorMessageList;
                    response.Status = ((Message)1).ToString();
                    }
                else
                    {
                    //Insert Survey Response
                    SurveyResponse = this.SurveyResponseDao.GetSingleResponse(request.AnswerInfo.ResponseId.ToString());
                    if (SurveyResponse.SurveyId == null)
                        {
                    SurveyResponse = InsertSurveyResponse(Mapper.ToBusinessObject(Xml, request.AnswerInfo.SurveyId.ToString(), request.AnswerInfo.ParentRecordId.ToString(), request.AnswerInfo.ResponseId.ToString(),request.AnswerInfo.UserId));
                    response = new PreFilledAnswerResponse();
                    response.Status = ((Message)2).ToString(); 
                        }
                    else{
                    UpdateFormResponse(Mapper.ToBusinessObject(Xml, request.AnswerInfo.SurveyId.ToString(), request.AnswerInfo.ParentRecordId.ToString(), request.AnswerInfo.ResponseId.ToString(), request.AnswerInfo.UserId));
                    response = new PreFilledAnswerResponse();
                    response.Status = ((Message)2).ToString(); 
                        }
                    }
                
            
            return response;
            }
      
        private Dictionary<string, string> ValidateResponse(List<SurveyInfoBO> SurveyBOList, PreFilledAnswerRequest request)
            {

            XDocument SurveyXml = new XDocument();
            foreach (var item in SurveyBOList)
                {
                SurveyXml = XDocument.Parse(item.XML);
                }
            Dictionary<string, string> MessageList = new Dictionary<string, string>();
            Dictionary<string, string> FieldNotFoundList = new Dictionary<string, string>();
            Dictionary<string, string> WrongFieldTypeList = new Dictionary<string, string>();
            SurveyResponseXML Implementation = new SurveyResponseXML(request, SurveyXml);
            FieldNotFoundList = Implementation.ValidateResponseFileds();
            //WrongFieldTypeList = Implementation.ValidateResponseFiledTypes();
            MessageList = MessageList.Union(FieldNotFoundList).Union(WrongFieldTypeList).ToDictionary(k => k.Key, v => v.Value);
            return MessageList;


            }
        private List<SurveyInfoBO> GetSurveyInfo(PreFilledAnswerRequest request)
            {

            List<string> SurveyIdList = new List<string>();
            string SurveyId = request.AnswerInfo.SurveyId.ToString();
            string OrganizationId = request.AnswerInfo.OrganizationKey.ToString();
            //Guid UserPublishKey = request.AnswerInfo.UserPublishKey;
            List<SurveyInfoBO> SurveyBOList = new List<SurveyInfoBO>();



            SurveyIdList.Add(SurveyId);

            SurveyInfoRequest pRequest = new SurveyInfoRequest();
            var criteria = pRequest.Criteria as Epi.Web.Enter.Common.Criteria.SurveyInfoCriteria;

            var entityDaoFactory = new EF.EntityDaoFactory();
            var surveyInfoDao = entityDaoFactory.SurveyInfoDao;
            SurveyInfo implementation = new SurveyInfo(surveyInfoDao);

            SurveyBOList = implementation.GetSurveyInfo(SurveyIdList, criteria.ClosingDate, OrganizationId, criteria.SurveyType, criteria.PageNumber, criteria.PageSize);//Default 

            return SurveyBOList;

            }
        private string CreateResponseXml(Epi.Web.Enter.Common.Message.PreFilledAnswerRequest request, List<SurveyInfoBO> SurveyBOList)
            {

            string ResponseXml;

            XDocument SurveyXml = new XDocument();

            foreach (var item in SurveyBOList)
                {
                SurveyXml = XDocument.Parse(item.XML);
                }
            SurveyResponseXML Implementation = new SurveyResponseXML(request, SurveyXml);
            ResponseXml = Implementation.CreateResponseDocument(SurveyXml).ToString();


            return ResponseXml;
            }


        public bool HasResponse(string SurveyId, string ResponseId)
            {
            SurveyAnswerCriteria SurveyAnswerCriteria = new SurveyAnswerCriteria();
            SurveyAnswerCriteria.SurveyId = SurveyId;
            SurveyAnswerCriteria.SurveyAnswerIdList = new List<string>();
            SurveyAnswerCriteria.SurveyAnswerIdList.Add(ResponseId);

            return this.SurveyResponseDao.HasResponse(SurveyAnswerCriteria);
            }

        public void UpdateRecordStatus(SurveyResponseBO SurveyResponseBO)
            {
            if(SurveyResponseBO.Status == 1)
                {
                SurveyResponseBO.Status = 2;
                }

            this.SurveyResponseDao.UpdateRecordStatus(SurveyResponseBO);
            }
    }
}
