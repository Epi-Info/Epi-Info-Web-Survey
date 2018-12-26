using Epi.Web.BLL;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.BusinessRule;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
using Epi.Web.EF;
using Epi.Web.MVC.Models;
using Epi.Web.MVC.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
 
namespace Epi.Web.MVC.Repositories
{
    public class SurveyResponseApiRepository : ISurveyResponseApiRepository
    {

        public Guid SurveyId
        {
            get; set;
        }

        public Guid OrgKey
        {
            get; set;
        }

        public Guid PublisherKey
        {
            get; set;
        }

        public string RootID
        {
            get;set;
        }

       

        public SurveyControlsResponse GetSurveyControlsList(SurveyControlsRequest pRequestMessage)
        {
            SurveyControlsResponse SurveyControlsResponse = new SurveyControlsResponse();
            try
            {
                Interfaces.DataInterfaces.ISurveyInfoDao ISurveyInfoDao = new EntitySurveyInfoDao();
                SurveyInfo Implementation = new BLL.SurveyInfo(ISurveyInfoDao);
                SurveyControlsResponse = Implementation.GetSurveyControlsforApi(pRequestMessage.SurveyId);
            }
            catch (Exception ex)
            {
                SurveyControlsResponse.Message = "Error";
                throw ex;
            }
            return SurveyControlsResponse;
        }               

        public List<SourceTableDTO> GetSourceTables(string SurveyId)
        {
            //List<SourceTableBO> list = new List<SourceTableBO>();
            SourceTablesResponse SourceTables = new SourceTablesResponse();
            SurveyControlsResponse SurveyControlsResponse = new SurveyControlsResponse();
            try
            {
                Interfaces.DataInterfaces.ISurveyInfoDao ISurveyInfoDao = new EntitySurveyInfoDao();
                SurveyInfo Implementation = new BLL.SurveyInfo(ISurveyInfoDao);
                SourceTables.List = Common.ObjectMapping.Mapper.ToSourceTableDTO(Implementation.GetSourceTables(SurveyId));
            }
            catch (Exception ex)
            {
                SurveyControlsResponse.Message = "Error";
                throw ex;
            }
            return SourceTables.List;
        }

        public Dictionary<string, string> ProcessModforRadioControls(IEnumerable<SurveyControlDTO> surveyControlList, Dictionary<string, string> SurveyQuestionAnswerList)
        {
            foreach (SurveyControlDTO s in surveyControlList)
            {
                if (SurveyQuestionAnswerList.Keys.Contains(s.ControlId))
                {
                    int val = Convert.ToInt32(SurveyQuestionAnswerList[s.ControlId]);
                    SurveyQuestionAnswerList[s.ControlId] = (val % 100).ToString();
                }
            }
            return SurveyQuestionAnswerList;
        }

        public Dictionary<string, string> ProcessValforCheckBoxControls(IEnumerable<SurveyControlDTO> surveyControlList, Dictionary<string, string> SurveyQuestionAnswerList)
        {
            foreach (SurveyControlDTO s in surveyControlList)
            {
                if (SurveyQuestionAnswerList.Keys.Contains(s.ControlId))
                {
                    string val = SurveyQuestionAnswerList[s.ControlId];
                    if (val != null && val.ToLower() == "false")
                        SurveyQuestionAnswerList[s.ControlId] = "No";
                    else if (val != null && val.ToLower() == "true")
                        SurveyQuestionAnswerList[s.ControlId] = "Yes";
                }
            }
            return SurveyQuestionAnswerList;
        }

        public Dictionary<string, string> ProcessValforYesNoControls(IEnumerable<SurveyControlDTO> surveyControlList, Dictionary<string, string> SurveyQuestionAnswerList)
        {
            foreach (SurveyControlDTO s in surveyControlList)
            {
                if (SurveyQuestionAnswerList.Keys.Contains(s.ControlId))
                {
                    string val = SurveyQuestionAnswerList[s.ControlId];
                    if (val != null && val == "2")
                        SurveyQuestionAnswerList[s.ControlId] = "1";
                    else if (val != null && val == "1")
                        SurveyQuestionAnswerList[s.ControlId] = "0";
                }
            }
            return SurveyQuestionAnswerList;
        }
        
        public Dictionary<string, string> ProcessValforLegalControls(IEnumerable<SurveyControlDTO> surveyControlList, Dictionary<string, string> SurveyQuestionAnswerList)
        {
            List<SourceTableDTO> SourceTables = null;
            if(string.IsNullOrEmpty(RootID))
            SourceTables = GetSourceTables(SurveyId.ToString());
            else
                SourceTables = GetSourceTables(RootID);           
            if (SourceTables.Count>0)
            {
                foreach (SurveyControlDTO s in surveyControlList)
                {
                    if (SurveyQuestionAnswerList.Keys.Contains(s.ControlId))
                    {
                        string val = SurveyQuestionAnswerList[s.ControlId];
                        var SourceTableXml1 = SourceTables.Where(x => x.TableName == s.SourceTableName).Select(y => y.TableXml).ToList();
                        XDocument SourceTableXml = XDocument.Parse(SourceTableXml1[0].ToString());
                       var _ControlValues = from _ControlValue in SourceTableXml.Descendants("SourceTable")                                            
                                          select _ControlValue;
                        List<string> legalvals = new List<string>();
                        foreach (var _ControlValue in _ControlValues)
                        {
                            var _SourceTableValues = from _SourceTableValue in _ControlValues.Descendants("Item")

                                                     select _SourceTableValue;

                            foreach (var _SourceTableValue in _SourceTableValues)
                            {
                                legalvals.Add(_SourceTableValue.Attributes().FirstOrDefault().Value.Trim());                              
                            }
                        }
                                SurveyQuestionAnswerList[s.ControlId] = legalvals.ElementAt(Convert.ToInt32(val)-1);                      
                    }
                }
            }
            return SurveyQuestionAnswerList;
        }

        /// <summary>
        /// Inserts SurveyResponse 
        /// </summary>
        /// <param name="SurveyResponseApiModel"></param>
        /// <returns>response </returns>
        public PreFilledAnswerResponse SetSurveyAnswer(SurveyResponseApiModel request)
        {
            PreFilledAnswerResponse response;
            SurveyControlsResponse SurveyControlsResponse = new SurveyControlsResponse();           
            SurveyControlsRequest surveyControlsRequest = new SurveyControlsRequest();
            surveyControlsRequest.SurveyId = request.SurveyId.ToString();           
            try
            {
                Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EntitySurveyResponseDao();
                BLL.SurveyResponse Implementation = new BLL.SurveyResponse(SurveyResponseDao);
                PreFilledAnswerRequest prefilledanswerRequest = new PreFilledAnswerRequest();
                Dictionary<string, string> Values = new Dictionary<string, string>();
                prefilledanswerRequest.AnswerInfo.UserPublishKey = request.PublisherKey;
                prefilledanswerRequest.AnswerInfo.OrganizationKey = request.OrgKey;
                prefilledanswerRequest.AnswerInfo.SurveyId = request.SurveyId;
                prefilledanswerRequest.AnswerInfo.UserPublishKey = request.PublisherKey;               
                List<SurveyInfoBO> SurveyBOList=  GetSurveyInfo(prefilledanswerRequest);
                GetRootFormId(prefilledanswerRequest);
                prefilledanswerRequest.AnswerInfo.SurveyId = request.SurveyId;
                SurveyControlsResponse = GetSurveyControlsList(surveyControlsRequest);              
                Dictionary<string, string> FilteredAnswerList = new Dictionary<string, string>();
                var radiolist = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "GroupBoxRadioList");
                FilteredAnswerList = ProcessModforRadioControls(radiolist, request.SurveyQuestionAnswerListField);
                var checkboxLsit = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "CheckBox");
                FilteredAnswerList = ProcessValforCheckBoxControls(checkboxLsit, FilteredAnswerList);
                var yesNoList = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "YesNo");
                FilteredAnswerList = ProcessValforYesNoControls(yesNoList, FilteredAnswerList);
                var legalvalList = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "LegalValues");
                FilteredAnswerList = ProcessValforLegalControls(legalvalList, FilteredAnswerList);
                foreach (KeyValuePair<string, string> entry in FilteredAnswerList)
                {
                    Values.Add(entry.Key, entry.Value);
                }
                prefilledanswerRequest.AnswerInfo.SurveyQuestionAnswerList = Values;
                response = Implementation.SetSurveyAnswer(prefilledanswerRequest);
                return response;
            }
            catch (Exception ex)
            {
                PassCodeDTO DTOList = new PassCodeDTO();
                response = new PreFilledAnswerResponse(DTOList);
                if (response.ErrorMessageList != null)
                    response.ErrorMessageList.Add("Failed", "Failed to insert Response");
                response.Status = ((BLL.SurveyResponse.Message)1).ToString();
                return response;
            }
        }

        public void GetRootFormId(PreFilledAnswerRequest request)
        {
            Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EntitySurveyResponseDao();
            BLL.SurveyResponse Implementation = new BLL.SurveyResponse(SurveyResponseDao);
            List<SurveyInfoBO> SurveyBOList = Implementation.GetSurveyInfo(request);//
            if ( string.IsNullOrEmpty(SurveyBOList[0].ParentId))
            {
                RootID= SurveyBOList[0].SurveyId;
            }
            else
            {
                request.AnswerInfo.SurveyId = new Guid(SurveyBOList[0].ParentId);
                GetRootFormId(request);
            }            
        }

        public List<SurveyInfoBO> GetSurveyInfo(PreFilledAnswerRequest request)
        {
            Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EntitySurveyResponseDao();
            BLL.SurveyResponse Implementation = new BLL.SurveyResponse(SurveyResponseDao);
            List<SurveyInfoBO> SurveyBOList = Implementation.GetSurveyInfo(request);//           
            return SurveyBOList;
        }

        /// <summary>
        /// Updates SurveyResponse 
        /// </summary>
        /// <param name="SurveyResponseApiModel",name="ResponseId"></param>
        /// <returns>response </returns>
        public PreFilledAnswerResponse Update(SurveyResponseApiModel request, string ResponseId)
        {
            PreFilledAnswerResponse response;
            SurveyControlsResponse SurveyControlsResponse = new SurveyControlsResponse();           
            SurveyControlsRequest surveyControlsRequest = new SurveyControlsRequest();
            surveyControlsRequest.SurveyId = request.SurveyId.ToString();
           
            try
            {
                Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EntitySurveyResponseDao();
                BLL.SurveyResponse Implementation = new BLL.SurveyResponse(SurveyResponseDao);
                PreFilledAnswerRequest prefilledanswerRequest = new PreFilledAnswerRequest();
                Dictionary<string, string> Values = new Dictionary<string, string>();
                prefilledanswerRequest.AnswerInfo.UserPublishKey = request.PublisherKey;
                prefilledanswerRequest.AnswerInfo.OrganizationKey = request.OrgKey;
                prefilledanswerRequest.AnswerInfo.SurveyId = request.SurveyId;
                prefilledanswerRequest.AnswerInfo.UserPublishKey = request.PublisherKey;               
                List<SurveyInfoBO> SurveyBOList = GetSurveyInfo(prefilledanswerRequest);
                GetRootFormId(prefilledanswerRequest);
                prefilledanswerRequest.AnswerInfo.SurveyId = request.SurveyId;
                SurveyControlsResponse = GetSurveyControlsList(surveyControlsRequest);              
                Dictionary<string, string> FilteredAnswerList = new Dictionary<string, string>();
                var radiolist = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "GroupBoxRadioList");
                FilteredAnswerList = ProcessModforRadioControls(radiolist, request.SurveyQuestionAnswerListField);
                var checkboxLsit = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "CheckBox");
                FilteredAnswerList = ProcessValforCheckBoxControls(checkboxLsit, FilteredAnswerList);
                var yesNoList = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "YesNo");
                FilteredAnswerList = ProcessValforYesNoControls(yesNoList, FilteredAnswerList);
                var legalvalList = SurveyControlsResponse.SurveyControlList.Where(x => x.ControlType == "LegalValues");
                FilteredAnswerList = ProcessValforLegalControls(legalvalList, FilteredAnswerList);

                var updatedtime = FilteredAnswerList.Where(x => x.Key.ToLower() == "_updatestamp").FirstOrDefault();
                var Responsekey = FilteredAnswerList.Where(x => x.Key.ToLower() == "responseid" || x.Key.ToLower() == "id").FirstOrDefault().Key;
                var fkey = FilteredAnswerList.Where(x => x.Key.ToLower() == "fkey").FirstOrDefault();
                foreach (KeyValuePair<string, string> entry in FilteredAnswerList)
                {
                    Values.Add(entry.Key, entry.Value);
                }

                try
                {
                    var survey = Implementation.GetSurveyResponseById(new List<string> { ResponseId }, request.PublisherKey);
                }
                catch (Exception ex)
                {
                    prefilledanswerRequest.AnswerInfo.SurveyQuestionAnswerList = Values;
                    response = Implementation.SetSurveyAnswer(prefilledanswerRequest);
                    response.Status = "Created";
                    return response;
                }


                Values.Remove(Responsekey);
                if (updatedtime.Key != null)
                    Values.Remove(updatedtime.Key);
                if (fkey.Key != null)
                    Values.Remove(fkey.Key);

                prefilledanswerRequest.AnswerInfo.SurveyQuestionAnswerList = Values;

                Dictionary<string, string> ErrorMessageList = new Dictionary<string, string>();
              
                string Xml = Implementation.CreateResponseXml(prefilledanswerRequest, SurveyBOList);//
                ErrorMessageList = Implementation.ValidateResponse(SurveyBOList, prefilledanswerRequest);//
                if (fkey.Key != null)
                {
                    try
                    {
                        var survey = Implementation.GetSurveyResponseById(new List<string> { fkey.Value }, request.PublisherKey);
                    }
                    catch (Exception ex)
                    {
                        SurveyResponseBO surveyresponsebO = new SurveyResponseBO();
                        surveyresponsebO.SurveyId = SurveyBOList[0].ParentId;
                        surveyresponsebO.ResponseId = fkey.Value.ToString();
                        surveyresponsebO.XML = "  ";
                        surveyresponsebO.Status = 3;
                        surveyresponsebO.RecrodSourceId = (int)ValidationRecordSourceId.MA;
                        surveyresponsebO.DateUpdated = DateTime.Now;
                        surveyresponsebO.DateCreated = surveyresponsebO.DateUpdated;
                        surveyresponsebO.DateCompleted = surveyresponsebO.DateUpdated;
                        surveyresponsebO = Implementation.InsertSurveyResponseApi(surveyresponsebO);

                    }
                }

                if (ErrorMessageList.Count() > 0)
                {
                    response = new PreFilledAnswerResponse();
                    response.ErrorMessageList = ErrorMessageList;
                    response.ErrorMessageList.Add("SurveyId", request.SurveyId.ToString());
                    response.ErrorMessageList.Add("ResponseId", ResponseId);
                    response.Status = ((Epi.Web.BLL.SurveyResponse.Message)1).ToString();
                    Implementation.InsertErrorLog(response.ErrorMessageList);
                }
                SurveyResponseBO surveyresponseBO = new SurveyResponseBO(); SurveyResponseBO SurveyResponse = new SurveyResponseBO();
                UserAuthenticationRequestBO UserAuthenticationRequestBO = new UserAuthenticationRequestBO();
                surveyresponseBO.SurveyId = request.SurveyId.ToString();
                surveyresponseBO.ResponseId = ResponseId.ToString();
                surveyresponseBO.XML = Xml;
                System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                if (updatedtime.Key != null)
                {
                    surveyresponseBO.DateUpdated = dateTime.AddMilliseconds(Convert.ToDouble(updatedtime.Value.ToString())).ToLocalTime();
                    surveyresponseBO.DateCompleted = dateTime.AddMilliseconds(Convert.ToDouble(updatedtime.Value.ToString())).ToLocalTime();
                }
                else
                {
                    surveyresponseBO.DateUpdated = DateTime.Now;
                    surveyresponseBO.DateCompleted = DateTime.Now;
                }
                if (fkey.Key != null)
                {
                    surveyresponseBO.RelateParentId = fkey.Value;
                }
                surveyresponseBO.Status = 3;
                SurveyResponse = Implementation.UpdateSurveyResponse(surveyresponseBO);
                UserAuthenticationRequestBO = Web.Common.ObjectMapping.Mapper.ToBusinessObject(ResponseId);
                Implementation.SavePassCode(UserAuthenticationRequestBO);

                //return Response
                string ResponseUrl = ConfigurationManager.AppSettings["ResponseURL"];
                response = new PreFilledAnswerResponse(Web.Common.ObjectMapping.Mapper.ToDataTransferObjects(UserAuthenticationRequestBO));
                response.SurveyResponseUrl = ResponseUrl + UserAuthenticationRequestBO.ResponseId;
                response.Status = ((Epi.Web.BLL.SurveyResponse.Message)2).ToString();
                return response;
            }
            catch (Exception ex)
            {
                PassCodeDTO DTOList = new PassCodeDTO();
                response = new PreFilledAnswerResponse(DTOList);
                if (response.ErrorMessageList != null)
                    response.ErrorMessageList.Add("Failed", "Failed to insert Response");
                response.Status = ((BLL.SurveyResponse.Message)1).ToString();
                return response;
            }
        }

        /// <summary>
        /// Validate Header information coming from the request
        /// </summary>
        /// <param name="SurveyId"></param>
        /// <returns>response </returns>
        public SurveyInfoBO GetSurveyInfoById(string SurveyId)
        {
            Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = new EF.EntitySurveyInfoDao();
            BLL.SurveyInfo SurveyInfo = new BLL.SurveyInfo(surveyInfoDao);
            try
            {
                var surveyInfo = SurveyInfo.GetSurveyInfoById(SurveyId);
                if (surveyInfo != null)
                {
                    Interfaces.DataInterfaces.IOrganizationDao OrgDao = new EF.EntityOrganizationDao();
                    BLL.Organization Organization = new BLL.Organization(OrgDao);
                    var OrgBo = Organization.GetOrganizationById(surveyInfo.OrganizationId);
                    surveyInfo.OrganizationKey = new Guid(OrgBo.OrganizationKey);
                }
                return surveyInfo;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Updates SurveyResponse 
        /// </summary>
        /// <param name="id"></param>
        public void Remove(string id)
        {
            Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EntitySurveyResponseDao();
            BLL.SurveyResponse Implementation = new BLL.SurveyResponse(SurveyResponseDao);
            SurveyResponseBO surveyresponseBO = new SurveyResponseBO();
            surveyresponseBO.ResponseId = id;
            surveyresponseBO.Status = 0;
            Implementation.UpdateRecordStatus(surveyresponseBO);
        }

    }
}