using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Web;
using Epi.Web.MVC.Models;
using NUnit.Framework;
using Moq;
using NUnit.Mocks;
using Epi.Web.Common.DTO;
using Epi.Web.MVC.Repositories;
using Epi.Web.MVC.Mock;

namespace Epi.Web.MVC.Controllers.HomeController_Tests
{
    class When_Submit_Survey
    {
        [Test]
        public void Then_Show_Finalized_Page()
        { 
          //Setup
            Epi.Web.MVC.Facade.ISurveyFacade iSurveyFacade;
            Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest;
          //Arrange
            SurveyInfoModel surveyInfoModel = new TestSurveyFacade().GetSurveyInfoModel("");
            surveyInfoRequest = new Epi.Web.Common.Message.SurveyInfoRequest();
            surveyInfoRequest.Criteria.SurveyIdList = "1";
            iSurveyFacade = new TestSurveyFacade(surveyInfoRequest);
            var controller = new Epi.Web.MVC.Controllers.FinalController(iSurveyFacade);
            ViewResult c = controller.Index("1","final") as ViewResult;
          //Assert
            Assert.AreEqual("PostSubmit", c.ViewName); /*Is it returning the right view name?*/
            Assert.AreEqual("Epi.Web.MVC.Models.SurveyInfoModel", c.Model.ToString());/*Is it returning the right view to the final view?*/

        }
    }
}
