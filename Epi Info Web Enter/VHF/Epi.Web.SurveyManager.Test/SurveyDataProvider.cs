using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Epi.Web.BLL;
using Epi.Web.Common.BusinessObject;
using System.Xml;
using System.Xml.Linq;
 
namespace Epi.Web.SurveyManager.Test
{
   public  class SurveyDataProvider
    {
           private DateTime ClosingDate =DateTime.Now;
           private string    DepartmentName = "DepartmentName1";
           private string       IntroductionText = "Survey one";
           private Boolean      IsSingleResponse = true;
           private string    OrganizationName = "OrganizationName1";
           private string     SurveyName = "Survey Name";
           private string   SurveyNumber = "ABC";
           private string TemplateXML =  GetXML();
           private int SurveyType = 1;
           
           private string _DepartmentName = "";
           private string _IntroductionText = "";
           private Boolean _IsSingleResponse = false;
           private string _OrganizationName = "";
           private string _SurveyName = "";
           private string _SurveyNumber = "";
           private string _TemplateXML = "";
           private int _SurveyType = 0 ;


    public SurveyRequestBO CreateSurveyRequestBOObject()

        {
            SurveyRequestBO pRequestMessage = new SurveyRequestBO();
            pRequestMessage.ClosingDate = ClosingDate;
            pRequestMessage.DepartmentName = DepartmentName;
            pRequestMessage.IntroductionText = IntroductionText;
            pRequestMessage.IsSingleResponse = IsSingleResponse;
            pRequestMessage.OrganizationName = OrganizationName;
            pRequestMessage.SurveyName = SurveyName;
            pRequestMessage.SurveyNumber = SurveyNumber;
            pRequestMessage.TemplateXML = TemplateXML;
            pRequestMessage.SurveyType = SurveyType;
            return pRequestMessage;
        }

        public SurveyRequestBO CreateSurveyRequestBOObjectWithNoData()
        {
            SurveyRequestBO pRequestMessage = new SurveyRequestBO();

            pRequestMessage.DepartmentName = _DepartmentName;
            pRequestMessage.IntroductionText = _IntroductionText;
            pRequestMessage.IsSingleResponse = _IsSingleResponse;
            pRequestMessage.OrganizationName = _OrganizationName;
            pRequestMessage.SurveyName = _SurveyName;
            pRequestMessage.SurveyNumber = _SurveyNumber;
            pRequestMessage.TemplateXML = _TemplateXML;
            pRequestMessage.SurveyType = _SurveyType;

            return pRequestMessage;
        }
        public SurveyInfoBO CreateSurveyInfoBOObject()
        {
            SurveyInfoBO pRequestMessage = new SurveyInfoBO();
            pRequestMessage.ClosingDate = ClosingDate;
            pRequestMessage.DepartmentName = DepartmentName;
            pRequestMessage.IntroductionText = IntroductionText;
           // pRequestMessage.IsSingleResponse = IsSingleResponse;
            pRequestMessage.OrganizationName = OrganizationName;
            pRequestMessage.SurveyName = SurveyName;
            pRequestMessage.SurveyNumber = SurveyNumber;
            pRequestMessage.XML = TemplateXML;
            pRequestMessage.SurveyType = SurveyType;
            return pRequestMessage;
        }
        private static string GetXML()
        {
             XDocument xdoc = XDocument.Load("../../MetaDataXML.xml");
            
            return xdoc.ToString();
        }
    }
}
