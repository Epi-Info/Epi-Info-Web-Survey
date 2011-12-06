using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Web;
using Epi.Web.Models;
using NUnit.Framework;
using Moq;
using NUnit.Mocks;
using Epi.Web.Common.DTO;
using Epi.Web.Repositories;

namespace Epi.Web.Test.Controllers.HomeController_Tests
{
    class When_Click_Button
    {
        [Test]
        public void Then_See_First_Data_Entry_Page()
        {
            //SetUp
            Epi.Web.Repositories.Core.ISurveyInfoRepository _iSurveyInfoRepository;
            _iSurveyInfoRepository = new Epi.Web.Test.Mock.SurveyRepository();
            //Arrange
            SurveyInfoModel surveyInfoModel = new SurveyInfoModel();
            surveyInfoModel.SurveyId = "1";
            surveyInfoModel.SurveyName = "ABC Survey";
            surveyInfoModel.SurveyNumber = "1A";
            surveyInfoModel.OrganizationName = "ABC Organization";
            surveyInfoModel.IntroductionText = "ABC Introduction test";
            surveyInfoModel.IsSuccess = true;
            surveyInfoModel.XML = "";

            var controller = new Epi.Web.Controllers.HomeController(_iSurveyInfoRepository);
            ViewResult c = controller.ListSurvey(surveyInfoModel) as ViewResult;
            MvcDynamicForms.Form F = c.Model as MvcDynamicForms.Form;

            //Assert
            Assert.AreEqual("Survey", c.ViewName); /*Is it returning the right view name?*/
            Assert.AreEqual("MvcDynamicField_", F.FieldPrefix);  /*Is it rendering form prefix*/
        }
    }
}
