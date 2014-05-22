using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Epi.Web.BLL;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Interfaces.DataInterfaces;
using Epi.Web.EF;
using System.Xml;
using Epi.Web.MVC.Facade;
using Epi.Web.MVC.Models;
using Epi.Web.MVC.Mock;
namespace Epi.Web.SurveyManager.Test.Publisher_Tests

{
    class When_Survey_Is_Accessed
    {
        [Test]
        public void Then_singlelineTextBox_Should_Display_on_Screen()
        {
            //SetUp

            Epi.Web.MVC.Facade.ISurveyFacade iSurveyFacade;
            Epi.Web.Common.Message.SurveyInfoRequest surveyInfoRequest;

            //Arrange

            surveyInfoRequest = new Epi.Web.Common.Message.SurveyInfoRequest();
            List<string> SurveyIdList = new List<string>();
            SurveyIdList.Add("1");
            surveyInfoRequest.Criteria.SurveyIdList = SurveyIdList;
            iSurveyFacade = new TestSurveyFacade(surveyInfoRequest);
            var surveyId = "7696d742-e42d-45d1-8352-ec8c3f0db3c2";
            int CurrentPage = 1;



            //Act

            var form = iSurveyFacade.GetSurveyFormData(surveyId, CurrentPage, null);
        
            //Assert
            Assert.NotNull(form);
            Assert.Greater(form.Fields.Count,0);
          
           

        }

    }
      
}
