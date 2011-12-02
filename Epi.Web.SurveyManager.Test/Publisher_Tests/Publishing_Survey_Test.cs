using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Epi.Web.BLL;
using Epi.Web.Common.BusinessObject;
namespace Epi.Web.SurveyManager.Test
{
    [Category("Publisher")]
    public class cPublishing_Survey
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
     
        public void URL_Has_Value()
        {

            Assert.IsNotEmpty(Get_Positive_Data().URL);

        }
        [Test]

        public void URL_Has_NO_Value()
        {

            Assert.IsNotEmpty(Get_Negative_Data().URL);

        }
        [Test]
        public void IsPulished_is_True()
        {
            Assert.IsTrue(Get_Positive_Data().IsPulished);
        }
        public SurveyRequestResultBO Get_Positive_Data()
        {
            var _Publisher = new Publisher();
            SurveyRequestResultBO result = new SurveyRequestResultBO();
            SurveyRequestBO pRequestMessage = new SurveyRequestBO();
            pRequestMessage.ClosingDate = DateTime.Now;
            pRequestMessage.DepartmentName = "DepartmentName";
            pRequestMessage.IntroductionText = "Survey one";
            pRequestMessage.IsSingleResponse = true;
            pRequestMessage.OrganizationName = "OrganizationName";
            pRequestMessage.SurveyName = "Survey Name";
            pRequestMessage.SurveyNumber = "A";
            pRequestMessage.TemplateXML = "";
            result.IsPulished = _Publisher.PublishSurvey(pRequestMessage).IsPulished;
            result.StatusText = _Publisher.PublishSurvey(pRequestMessage).StatusText;
            result.URL = _Publisher.PublishSurvey(pRequestMessage).URL;

         return result;
        }
        public SurveyRequestResultBO Get_Negative_Data()
        {
            var _Publisher = new Publisher();
            SurveyRequestResultBO result = new SurveyRequestResultBO();
            SurveyRequestBO pRequestMessage = new SurveyRequestBO();
            pRequestMessage.DepartmentName = "";
            pRequestMessage.IntroductionText = "";
            pRequestMessage.IsSingleResponse = false;
            pRequestMessage.OrganizationName = "";
            pRequestMessage.SurveyName = "";
            pRequestMessage.SurveyNumber = "";
            pRequestMessage.TemplateXML = "";
            result.IsPulished = _Publisher.PublishSurvey(pRequestMessage).IsPulished;
            result.StatusText = _Publisher.PublishSurvey(pRequestMessage).StatusText;
            result.URL = _Publisher.PublishSurvey(pRequestMessage).URL;

            return result;
        } 
    }
}
