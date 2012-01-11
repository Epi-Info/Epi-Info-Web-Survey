using System.Web.Mvc;
using NUnit.Framework;
using Epi.Web.MVC.Mock;
namespace Epi.Web.MVC.Test.Controllers.HomeController_Tests
{
    class When_Arrive_At_Application
    {
      
        [Test]
        public void Then_See_First_Data_Entry_Page()
        {
            //SetUp

            Epi.Web.MVC.Facade.ISurveyFacade iSurveyFacade;
            Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest;
            
            //Arrange
            surveyInfoRequest=new Epi.Web.Common.Message.SurveyInfoRequest();
            surveyInfoRequest.Criteria.SurveyIdList = "1";
            iSurveyFacade = new TestSurveyFacade(surveyInfoRequest);
            var controller = new Epi.Web.MVC.Controllers.HomeController(iSurveyFacade);
            ViewResult c = controller.Index("1") as ViewResult;
            //MvcDynamicForms.Form F = c.Model as MvcDynamicForms.Form;

            //Assert
            Assert.AreEqual("Index", c.ViewName); /*Is it returning the right view name?*/
            //Assert.AreEqual("MvcDynamicField_", F.FieldPrefix);  /*Is it rendering form prefix*/
        }


    }




   
}
