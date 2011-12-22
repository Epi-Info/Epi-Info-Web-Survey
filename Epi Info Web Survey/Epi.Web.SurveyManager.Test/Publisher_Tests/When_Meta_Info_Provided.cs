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
    class When_Meta_Info_Provided
    {
        [Test]
        public void Then_Single_Response_Value_Recorded()
        {
            //Arrange


            ISurveyInfoDao objISurveryInfoDao = new EntitySurveyInfoDao();
            SurveyInfo objSurveyInfo = new SurveyInfo(objISurveryInfoDao);

            Publisher objPublisher = new Publisher(objISurveryInfoDao);

            SurveyDataProvider DataObj = new SurveyDataProvider();//Get Data

            SurveyRequestBO objSurveyRequestBO;
            SurveyRequestResultBO objSurveyResponseBO;
            SurveyInfoBO objSurveyInfoBO = new SurveyInfoBO();
            int ResonseType;
            string surveyURL;
            string surveyID = string.Empty;
            
            //Act

            objSurveyRequestBO = DataObj.CreateSurveyRequestBOObject();
            objSurveyRequestBO.SurveyType = 1;
            ResonseType = objSurveyRequestBO.SurveyType;

            //objSurveyResponseBO = objPublisher.PublishSurvey(objSurveyRequestBO);// publish survey and get Response back
            objSurveyInfoBO = DataObj.CreateSurveyInfoBOObject(); 
            objSurveyResponseBO = objPublisher.PublishSurvey(objSurveyInfoBO);
            surveyURL = objSurveyResponseBO.URL;
            surveyID = surveyURL.Substring(surveyURL.LastIndexOf('/') + 1);  //Get the ID from Url. 


            objSurveyInfoBO = objSurveyInfo.GetSurveyInfoById(surveyID);


            //Assert

            Assert.AreEqual(objSurveyInfoBO.SurveyType, ResonseType);


        }

        [Test]
        public void Then_Multiple_Response_Value_Recorded()
        {
            //Arrange


            ISurveyInfoDao objISurveryInfoDao = new EntitySurveyInfoDao();
            SurveyInfo objSurveyInfo = new SurveyInfo(objISurveryInfoDao);

            Publisher objPublisher = new Publisher(objISurveryInfoDao);

            SurveyDataProvider DataObj = new SurveyDataProvider();//Get Data

            SurveyRequestBO objSurveyRequestBO;
            SurveyRequestResultBO objSurveyResponseBO;
            SurveyInfoBO objSurveyInfoBO = new SurveyInfoBO() ;
            int ResonseType;
            string surveyURL;
            string surveyID = string.Empty;
            
            //Act

            objSurveyRequestBO = DataObj.CreateSurveyRequestBOObject();
            objSurveyRequestBO.SurveyType = 2;

            ResonseType = objSurveyRequestBO.SurveyType;
            objSurveyInfoBO = DataObj.CreateSurveyInfoBOObject(); 
           // objSurveyResponseBO = objPublisher.PublishSurvey(objSurveyRequestBO);// publish survey and get Response back
            objSurveyResponseBO = objPublisher.PublishSurvey(objSurveyInfoBO);

            surveyURL = objSurveyResponseBO.URL;
            surveyID = surveyURL.Substring(surveyURL.LastIndexOf('/') + 1);  //Get the ID from Url. 


            objSurveyInfoBO = objSurveyInfo.GetSurveyInfoById(surveyID);


            //Assert

            Assert.AreEqual(objSurveyInfoBO.SurveyType, ResonseType);


        }

        [Test]
        public void Then_ClosingDate_Recorded()
        {
            //Arrange
            ISurveyInfoDao objISurveryInfoDao = new EntitySurveyInfoDao();
            SurveyInfo objSurveyInfo = new SurveyInfo(objISurveryInfoDao);

            Publisher objPublisher = new Publisher(objISurveryInfoDao);

            SurveyDataProvider DataObj = new SurveyDataProvider();//Get Data

            SurveyRequestBO objSurveyRequestBO;
            SurveyRequestResultBO objSurveyResponseBO;
            SurveyInfoBO objSurveyInfoBO = new SurveyInfoBO() ;
            DateTime closingDate;
            string surveyURL;
            string surveyID = string.Empty;
            //Act

            objSurveyRequestBO = DataObj.CreateSurveyRequestBOObject();
            closingDate = objSurveyRequestBO.ClosingDate;   //Closing date that is sent in    


            //objSurveyResponseBO = objPublisher.PublishSurvey(objSurveyRequestBO);// publish survey and get Response back
            objSurveyInfoBO = DataObj.CreateSurveyInfoBOObject(); 
            objSurveyResponseBO = objPublisher.PublishSurvey(objSurveyInfoBO);

            surveyURL = objSurveyResponseBO.URL;
            surveyID = surveyURL.Substring(surveyURL.LastIndexOf('/')+1);  //Get the ID from Url. 


            objSurveyInfoBO = objSurveyInfo.GetSurveyInfoById(surveyID);


          //Assert
            
            Assert.AreEqual(objSurveyInfoBO.ClosingDate , closingDate );
        }
    
    }
}
