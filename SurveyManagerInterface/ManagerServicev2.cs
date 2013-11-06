using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.ObjectMapping;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Exception;
using Epi.Web.BLL;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using Epi.Web.Common.Security;
namespace Epi.Web.WCF.SurveyService
    {
    public class ManagerServiceV2 : ManagerService, IManagerServiceV2
        {
        public PreFilledAnswerResponse SetSurveyAnswer(PreFilledAnswerRequest request)
            {
            try
                {
                Dictionary<string, string> ErrorMessageList = new Dictionary<string, string>();
                PreFilledAnswerResponse response;
                Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EF.EntitySurveyResponseDao();
                Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);

                SurveyResponseBO SurveyResponse = new SurveyResponseBO();
                UserAuthenticationRequestBO UserAuthenticationRequestBO = new Common.BusinessObject.UserAuthenticationRequestBO();

                //Build Survey Response Xml

                string Xml = Implementation.CreateResponseXml(GetSurveyInfo(request), request);
                //Validate Response values

                ErrorMessageList = Implementation.ValidateResponse(GetSurveyInfo(request), request);

                if (ErrorMessageList.Count() > 0)
                    {
                    response = new PreFilledAnswerResponse();
                    response.ErrorMessageList = ErrorMessageList;
                    response.Status = "Failed";
                    }
                else
                    {
                    //Insert Survey Response

                    SurveyResponse = Implementation.InsertSurveyResponse(Mapper.ToBusinessObject(Xml, request.AnswerInfo.SurveyId.ToString()));

                    //Save PassCode

                    UserAuthenticationRequestBO = Mapper.ToBusinessObject(SurveyResponse.ResponseId);
                    Implementation.SavePassCode(UserAuthenticationRequestBO);

                    //return Response
                    string ResponseUrl = ConfigurationManager.AppSettings["ResponseURL"];
                    response = new PreFilledAnswerResponse(Mapper.ToDataTransferObjects(UserAuthenticationRequestBO));
                    response.SurveyResponseUrl = ResponseUrl + UserAuthenticationRequestBO.ResponseId;
                    response.Status = "Success";

                    }
                return response;
                }
            catch (Exception ex)
                {
                PassCodeDTO DTOList = new Common.DTO.PassCodeDTO();
                PreFilledAnswerResponse response = new PreFilledAnswerResponse(DTOList);

                response.ErrorMessageList.Add("Failed", "Failed to insert Response");
                response.Status = "Failed";
                return response;
                }
            }


        private static List<SurveyInfoBO> GetSurveyInfo(PreFilledAnswerRequest request)
            {
            string OrganizationId = "";
            List<string> SurveyIdList = new List<string>();
            OrganizationId = request.AnswerInfo.OrganizationKey.ToString();
            SurveyIdList.Add(request.AnswerInfo.SurveyId.ToString());
            List<SurveyInfoBO> SurveyBOList = new List<SurveyInfoBO>();
            SurveyInfoRequest pRequest = new SurveyInfoRequest();
            var criteria = pRequest.Criteria as SurveyInfoCriteria;

            Epi.Web.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();
            Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao surveyInfoDao = entityDaoFactory.SurveyInfoDao;
            Epi.Web.BLL.SurveyInfo implementation = new Epi.Web.BLL.SurveyInfo(surveyInfoDao);

            SurveyBOList = implementation.GetSurveyInfo(SurveyIdList, criteria.ClosingDate, OrganizationId, criteria.SurveyType, criteria.PageNumber, criteria.PageSize);//Default 

            return SurveyBOList;

            }

        }
    }
