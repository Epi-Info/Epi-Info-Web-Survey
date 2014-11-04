using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Web.Mvc;
using Epi.Web;
using Epi.Web.MVC.Models;
using NUnit.Framework;
//using Moq;
//using NUnit.Mocks;
using Epi.Web.Common.DTO;
using Epi.Web.MVC.Repositories;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Utility;
using Epi.Web.MVC.Repositories.Core;
using System.Xml;
using System.Xml.Linq;
 
namespace Epi.Web.MVC.Mock
{
    public class TestSurveyFacade : ISurveyFacade
    {
        // declare ISurveyInfoRepository which inherits IRepository of SurveyInfoResponse object
        private ISurveyInfoRepository _iSurveyInfoRepository;

        // declare ISurveyResponseRepository which inherits IRepository of SurveyResponseResponse object
        private ISurveyAnswerRepository _iSurveyResponseRepository;

        //declare SurveyInfoRequest
        private Epi.Web.Common.Message.SurveyInfoRequest _surveyInfoRequest;

        //declare SurveyResponseRequest
        private Epi.Web.Common.Message.SurveyAnswerRequest _surveyAnswerRequest;

        //declare SurveyAnswerDTO
        private Common.DTO.SurveyAnswerDTO _surveyAnswerDTO;

        //declare SurveyResponseXML object
        private SurveyResponseXML _surveyResponseXML;

        /// <summary>
        /// Injectinting ISurveyInfoRepository through Constructor
        /// </summary>
        /// <param name="iSurveyInfoRepository"></param>
        public TestSurveyFacade(ISurveyInfoRepository iSurveyInfoRepository, ISurveyAnswerRepository iSurveyResponseRepository,
                                  Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest, Epi.Web.Common.Message.SurveyAnswerRequest surveyResponseRequest,
                                  Common.DTO.SurveyAnswerDTO surveyAnswerDTO,
                                   SurveyResponseXML surveyResponseXML)
        {
            _iSurveyInfoRepository = iSurveyInfoRepository;
            _iSurveyResponseRepository = iSurveyResponseRepository;
            _surveyInfoRequest = surveyInfoRequest;
            _surveyAnswerRequest = surveyResponseRequest;
            _surveyAnswerDTO = surveyAnswerDTO;
            _surveyResponseXML = surveyResponseXML;
        }

        public TestSurveyFacade(Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest)
        {
            _surveyInfoRequest = surveyInfoRequest;
        }
        public MvcDynamicForms.Form GetSurveyFormData(string surveyId, int pageNumber, SurveyAnswerDTO surveyAnswerDTO)
        {
            SurveyInfoModel surveyInfoModel = GetSurveyInfoModel(surveyId);
            SurveyInfoDTO surveyInfoDTO = Epi.Web.MVC.Models.Mapper.ToSurveyInfoDTO(surveyInfoModel);
            
            MvcDynamicForms.Form form = Epi.Web.MVC.Utility.FormProvider.GetForm(surveyInfoDTO, 1, null);
            return form;
        }

        public void CreateSurveyAnswer(string surveyId, string responseId)
        {
            //throw new NotImplementedException();
        }

        public void UpdateSurveyResponse(SurveyInfoModel surveyInfoModel, string responseId, MvcDynamicForms.Form form)
        {
            throw new NotImplementedException();
        }

        public SurveyInfoModel GetSurveyInfoModel(string surveyId)
        {
            //_surveyInfoRequest.Criteria.SurveyId = surveyId;
            //Epi.Web.Common.Message.SurveyInfoResponse surveyInfoResponse = _iSurveyInfoRepository.GetSurveyInfo(_surveyInfoRequest);
            //SurveyInfoModel s = Mapper.ToSurveyInfoModel(surveyInfoResponse.SurveyInfo);
            //return s;
         //   SurveyDataProvider DataObj = new SurveyDataProvider();//Get Data
            SurveyInfoModel surveyInfoModel = new SurveyInfoModel();
            surveyInfoModel.SurveyId = "1";
            surveyInfoModel.SurveyName = "ABC Survey";
            surveyInfoModel.SurveyNumber = "1A";
            surveyInfoModel.OrganizationName = "ABC Organization";
            surveyInfoModel.IntroductionText = "ABC Introduction test";
            surveyInfoModel.IsSuccess = true;
            surveyInfoModel.XML = GetXML();
            return surveyInfoModel;
        }

        public Common.Message.SurveyAnswerResponse GetSurveyAnswerResponse(string responseId)
        {
            throw new NotImplementedException();
        }
        private static string GetXML()
        {
            XDocument xdoc = XDocument.Load("../../MetaDataXML.xml");

            return xdoc.ToString();
        }
    }
}
