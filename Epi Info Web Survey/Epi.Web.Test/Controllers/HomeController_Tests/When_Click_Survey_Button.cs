using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Web;
using Epi.Web.MVC.Models;
using NUnit.Framework;
//using Moq;
using NUnit.Mocks;
using Epi.Web.Common.DTO;
using Epi.Web.MVC.Repositories;
using Epi.Web.MVC.Mock;
namespace Epi.Web.MVC.Controllers.HomeController_Tests
{
    class When_Click_Survey_Button
    {

        [Test]
        public void Then_Open_The_Survey()
        {
            //SetUp

            Epi.Web.MVC.Facade.ISurveyFacade iSurveyFacade;
            Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest;

            //Arrange
            surveyInfoRequest = new Epi.Web.Common.Message.SurveyInfoRequest();
            surveyInfoRequest.Criteria.SurveyId = "1";
            iSurveyFacade = new TestSurveyFacade(surveyInfoRequest);
            var controller = new Epi.Web.MVC.Controllers.SurveyController(iSurveyFacade);
            ViewResult c = controller.Index("1","page") as ViewResult;
            //Assert
            MvcDynamicForms.Form f = c.Model as MvcDynamicForms.Form;
            
           // Assert.AreEqual(typeof(MvcDynamicForms.Form), c.Model);//test to make sure it is displaying the survey form data

            Assert.AreEqual("MvcDynamicField_", f.FieldPrefix);//test to make sure it is returning field prefix
        }
//        [Test]
//        public void Then_Able_To_See_Survey()
//        {
//            //SetUp
//            Epi.Web.MVC.Repositories.Core.ISurveyInfoRepository _iSurveyInfoRepository;
//            _iSurveyInfoRepository = new Epi.Web.Test.Mock.SurveyRepository();
//            //Arrange
//            var controller = new Epi.Web.MVC.Controllers.HomeController(_iSurveyInfoRepository);
//            ViewResult c = controller.ListSurvey("1") as ViewResult;
//            SurveyInfoModel sfo1 = c.Model as SurveyInfoModel;
//            //Assert
//            /*Is it rendering the right survey meta page?*/
//            Assert.AreEqual("SurveyIntroduction", c.ViewName);
//            /*Is it rendering the right Survey Name based on the SurveyId? */
//            Assert.AreEqual("Abc Survey", sfo1.SurveyName); 
//        }

//        [Test]
//        public void Then_Able_To_See_Survery_If_Open()
//        {
//            //SetUp
//            Epi.Web.Repositories.Core.ISurveyInfoRepository _iSurveyInfoRepository;
//            _iSurveyInfoRepository = new Epi.Web.Test.Mock.SurveyRepository();
//            //Arrange
//            var controller = new Epi.Web.Controllers.HomeController(_iSurveyInfoRepository);
//            ViewResult c = controller.ListSurvey("1") as ViewResult;
//            SurveyInfoModel sfo1 = c.Model as SurveyInfoModel;
//            //Assert
//            /*We will measure this indirectly. We will retrieve the closing date. If the closing date
//             is more than or equal to today's date then the survey is open otherwise closed. The way the view retrieves the survey information based on closing date
//             has been kept on the view end. We will evaluate whether we can put the logic on the controller end.*/
//            Assert.GreaterOrEqual(sfo1.ClosingDate.Date,DateTime.Now.Date); 
//        }

//        [Test]
//        public void Then_See_Close_Message_If_Closed()
//        {
//            /*We will measure it indirectly as the previous example. If the closing date is less than todays date 
//             then we should see the close message
//             */
//            //SetUp
//            Epi.Web.Repositories.Core.ISurveyInfoRepository _iSurveyInfoRepository;
//            _iSurveyInfoRepository = new Epi.Web.Test.Mock.SurveyRepository();
//            //Arrange
//            var controller = new Epi.Web.Controllers.HomeController(_iSurveyInfoRepository);
//            ViewResult c = controller.ListSurvey("1") as ViewResult;
//            SurveyInfoModel sfo1 = c.Model as SurveyInfoModel;
//            //Assert
//            Assert.Less(sfo1.ClosingDate.Date, DateTime.Now.Date);
//        }

//        [Test]
//        public void Then_See_Page_With_Survey_Info_And_Begin_Button()
//        {
//            //SetUp
//            Epi.Web.Repositories.Core.ISurveyInfoRepository _iSurveyInfoRepository;
//            _iSurveyInfoRepository = new Epi.Web.Test.Mock.SurveyRepository();
//            //Arrange
//            var controller = new Epi.Web.Controllers.HomeController(_iSurveyInfoRepository);
//            ViewResult c = controller.ListSurvey("1") as ViewResult;
//            SurveyInfoModel sfo1 = c.Model as SurveyInfoModel;
//            //Assert
//            /*We will measure this indirectly. We will retrieve the closing date. If the closing date
//             is more than or equal to today's date then the survey is open otherwise closed. We will verify whether we can see
//             the Survey Name, Number*/
//            Assert.GreaterOrEqual(sfo1.ClosingDate.Date, DateTime.Now.Date);
//            Assert.AreEqual("Abc Survey", sfo1.SurveyName);
//            Assert.AreEqual("11",sfo1.SurveyNumber);
//        }



//        [Test]
//        public void Survey_Does_Not_Exist_Message_When_A_Valid_URL_Does_Not_Exist()
//        {
//            //SetUp
//            Epi.Web.Repositories.Core.ISurveyInfoRepository _iSurveyInfoRepository;
//            _iSurveyInfoRepository = new Epi.Web.Test.Mock.SurveyRepository();
//            //Arrange
//            var controller = new Epi.Web.Controllers.HomeController(_iSurveyInfoRepository);
//            ViewResult c = controller.ListSurvey("55") as ViewResult;
//            SurveyInfoModel sfo1 = c.Model as SurveyInfoModel;
//            //Assert
//            /*We will test this case first by passing a surveyid that does not exists. Then check whether it is rendering the 
//             view named Exception. If it does then the test passes*/
//            /*Is it rendering the right survey meta page?*/
//            Assert.AreEqual("Exception", c.ViewName);
            

//        }
    }
}
