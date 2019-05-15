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
 
                PreFilledAnswerResponse response;
                Epi.Web.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EF.EntitySurveyResponseDao();
                Interfaces.DataInterfaces.ISurveyInfoDao ISurveyInfoDao = new  EF.EntitySurveyInfoDao();
                Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao,ISurveyInfoDao);

                response = Implementation.SetSurveyAnswer(request);
                    
                return response;
                }
            catch (Exception ex)
                {
                PassCodeDTO DTOList = new Common.DTO.PassCodeDTO();
                PreFilledAnswerResponse response = new PreFilledAnswerResponse(DTOList);

                response.ErrorMessageList.Add("Failed", "Failed to insert Response");
                response.Status = ((Epi.Web.BLL.SurveyResponse.Message)1).ToString(); 
                return response;
                }
            }


        public SurveyControlsResponse GetSurveyControlList(SurveyControlsRequest pRequestMessage) { 
            
            SurveyControlsResponse SurveyControlsResponse = new SurveyControlsResponse();

            try
                {


                Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao ISurveyInfoDao = new EF.EntitySurveyInfoDao();
                Epi.Web.BLL.SurveyInfo Implementation = new Epi.Web.BLL.SurveyInfo(ISurveyInfoDao);
                SurveyControlsResponse = Implementation.GetSurveyControlList(pRequestMessage.SurveyId);
                
                }
            catch (Exception ex)
                {
                SurveyControlsResponse.Message = "Error";
                throw ex;
                }






            return SurveyControlsResponse;
            }
        }
    }
