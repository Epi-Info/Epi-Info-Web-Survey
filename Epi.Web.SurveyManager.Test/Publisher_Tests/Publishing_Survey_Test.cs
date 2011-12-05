using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Epi.Web.BLL;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Interfaces.DataInterfaces;
using Epi.Web.EF;



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
        public void URL_Has_Value_When_Appropriate_Data_Provided_While_Posting()
        {
            //Arrange
            Publisher objPublisher = new Publisher();
            SurveyRequestResultBO objSurveyRequestResultBO;
            SurveyDataProvider DataObj = new SurveyDataProvider();
            //Act
            objSurveyRequestResultBO = objPublisher.PublishSurvey(DataObj.CreateSurveyRequestBOObject());
            //Assert
            Assert.IsNotEmpty(objSurveyRequestResultBO.URL);
            Assert.IsTrue(objSurveyRequestResultBO.IsPulished);

        }

        [Test]
        public void URL_Has_NO_Value_When_No_Data_Is_Provided_While_Posting()
        {

            //Arrange
            Publisher objPublisher = new Publisher();
            SurveyRequestResultBO objSurveyRequestResultBO;
            SurveyDataProvider DataObj = new SurveyDataProvider();
            //Act
            objSurveyRequestResultBO = objPublisher.PublishSurvey(DataObj.CreateSurveyRequestBOObjectWithNoData());
            //Assert
            Assert.IsEmpty(objSurveyRequestResultBO.URL);
            Assert.IsFalse(objSurveyRequestResultBO.IsPulished);

        }

        [Test]
        public void Close_out_date_is_properly_sent_At_Publish_time() {

            //Arrange
            Publisher objPublisher = new Publisher();
            SurveyDataProvider DataObj = new SurveyDataProvider();
            SurveyRequestBO objSurveyRequestBO;
            //Act
            objSurveyRequestBO = DataObj.CreateSurveyRequestBOObject();
            //Assert

            Assert.IsNotNull(objSurveyRequestBO.ClosingDate);
        
        
        }
        [Test]
        public void TemplateXML_properly_sent_At_Publish_time()
        {

            //Arrange
            Publisher objPublisher = new Publisher();
            SurveyDataProvider DataObj = new SurveyDataProvider();
            SurveyRequestBO objSurveyRequestBO;
            //Act
            objSurveyRequestBO = DataObj.CreateSurveyRequestBOObject();
            //Assert

            Assert.IsNotNull(objSurveyRequestBO.TemplateXML);


        }

        [Test]
        public void When_CloseDate_Provided_It_Is_Recorded()
        { 
            //Arrange
           

            ISurveyInfoDao objISurveryInfoDao = new EntitySurveyInfoDao();
            SurveyInfo objSurveyInfo = new SurveyInfo(objISurveryInfoDao);

            Publisher objPublisher = new Publisher(objISurveryInfoDao);

            SurveyDataProvider DataObj = new SurveyDataProvider();//Get Data

            SurveyRequestBO objSurveyRequestBO;
            SurveyRequestResultBO objSurveyResponseBO;
            SurveyInfoBO objSurveyInfoBO;
            DateTime closingDate;
            string surveyURL;
            string surveyID = string.Empty;
            //Act

            objSurveyRequestBO = DataObj.CreateSurveyRequestBOObject();
            closingDate = objSurveyRequestBO.ClosingDate;   //Closing date that is sent in    


            objSurveyResponseBO = objPublisher.PublishSurvey(objSurveyRequestBO);// publish survey and get Response back
           
            surveyURL = objSurveyResponseBO.URL;
            surveyID = surveyURL.Substring(surveyURL.LastIndexOf('/')+1);  //Get the ID from Url. 


            objSurveyInfoBO = objSurveyInfo.GetSurveyInfoById(surveyID);


          //Assert
            
            Assert.AreEqual(objSurveyInfoBO.ClosingDate , closingDate );
        }

    
        
        
    }
}
