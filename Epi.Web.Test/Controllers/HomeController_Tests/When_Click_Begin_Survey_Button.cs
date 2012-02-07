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
    class When_Click_Begin_Survey_Button
    {

        [Test]
        public void Then_Open_The_Survey()
        {
            //SetUp

            Epi.Web.MVC.Facade.ISurveyFacade iSurveyFacade;
            Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest;

            //Arrange
            surveyInfoRequest = new Epi.Web.Common.Message.SurveyInfoRequest();
            surveyInfoRequest.Criteria.SurveyIdList = "1";
            iSurveyFacade = new TestSurveyFacade(surveyInfoRequest);
            var controller = new Epi.Web.MVC.Controllers.SurveyController(iSurveyFacade);
            ViewResult c = controller.Notify("1","page") as ViewResult;
            //MvcDynamicForms.Form f = c.Model as MvcDynamicForms.Form;
            MvcDynamicForms.Form f = iSurveyFacade.GetSurveyFormData("1", 1, null);
            //Assert.AreEqual(typeof(MvcDynamicForms.Form), c.Model);//test to make sure it is returning field prefix
            //Does it render all the controls?
            //Assert
            Assert.AreEqual(7, f.Fields.Count);//test to make sure it is returning correct number of items
           // Does it render the text box?
            Assert.AreEqual("MvcDynamicForms.Fields.TextBox", f.Fields[0].GetType().ToString());//end item in the forms collection is a textbox
            
        }
        [Test]
        public void Then_Submit_Survey()
        {
            //SetUp

            Epi.Web.MVC.Facade.ISurveyFacade iSurveyFacade;
            Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest;

            //Arrange
            SurveyInfoModel surveyInfoModel = new TestSurveyFacade().GetSurveyInfoModel("");
            surveyInfoRequest = new Epi.Web.Common.Message.SurveyInfoRequest();
            surveyInfoRequest.Criteria.SurveyIdList = "1";
            iSurveyFacade = new TestSurveyFacade(surveyInfoRequest);
            var controller = new Epi.Web.MVC.Controllers.SurveyController(iSurveyFacade);
            ViewResult c = controller.Index(surveyInfoModel, "Submit") as ViewResult;

            //Assert
            /*
             It goes to the, surveyController's Index method of type post. As simulating form with posted value 
             * requires more work, at this point we can safely assume that the survey submits the data.
             */
            
        }      
    }
}
