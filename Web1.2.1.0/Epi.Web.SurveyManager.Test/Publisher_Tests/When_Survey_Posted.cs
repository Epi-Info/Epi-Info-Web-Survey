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

namespace Epi.Web.SurveyManager.Test.Publisher_Tests
{
    class When_Survey_Posted
    {
        [Test]
        public void Then_Receive_Unique_Indetifier()
        {
             //Arrange
            Publisher objPublisher = new Publisher();
            SurveyRequestResultBO objSurveyRequestResultBO;
           // SurveyDataProvider DataObj = new SurveyDataProvider();
            SurveyInfoBO DataObj = new SurveyInfoBO();
            //Act
            //objSurveyRequestResultBO = objPublisher.PublishSurvey(DataObj.CreateSurveyRequestBOObject());
            objSurveyRequestResultBO = objPublisher.PublishSurvey(DataObj);
            //Assert
            Assert.IsNotEmpty(objSurveyRequestResultBO.URL);
            Assert.IsTrue(objSurveyRequestResultBO.IsPulished);
        }

        [Test]
        public void Then_Do_Not_Receive_Unique_Identifier_When_No_Data_Provided()
        {
             //Arrange
            Publisher objPublisher = new Publisher();
            SurveyRequestResultBO objSurveyRequestResultBO;
            // SurveyDataProvider DataObj = new SurveyDataProvider();
            SurveyInfoBO DataObj = new SurveyInfoBO();
            //Act
            //objSurveyRequestResultBO = objPublisher.PublishSurvey(DataObj.CreateSurveyRequestBOObjectWithNoData());
            objSurveyRequestResultBO = objPublisher.PublishSurvey(DataObj );
            //Assert
            Assert.IsEmpty(objSurveyRequestResultBO.URL);
            Assert.IsFalse(objSurveyRequestResultBO.IsPulished);
        }
   //[Test]
   //     public void Then_Survey_Is_Accessible_Using_The_URL()
   // {
   //      //Arrange
   //         Publisher objPublisher = new Publisher();
   //         SurveyRequestResultBO objSurveyRequestResultBO;
   //         SurveyDataProvider DataObj = new SurveyDataProvider();
   //         //Act
   //         objSurveyRequestResultBO = objPublisher.PublishSurvey(DataObj.CreateSurveyRequestBOObject());
   //         //Assert
   //         Assert.IsNotEmpty(objSurveyRequestResultBO.URL);
   //         Assert.IsTrue(objSurveyRequestResultBO.IsPulished);
   //     }




    }
}
