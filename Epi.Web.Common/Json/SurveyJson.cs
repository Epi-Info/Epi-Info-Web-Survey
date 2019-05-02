using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Epi.Web.Common.Json
{
   public class SurveyResponseJson
    {

        public string GetSurveyResponseJson(Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswer, List<FormsHierarchyDTO> FormsHierarchyDTOList, SurveyControlsResponse List)
        {
            ResponseDetail Responsedetail = new ResponseDetail();
            var json = "";
            var ChildFormsHierarchy = FormsHierarchyDTOList.Where(x => x.IsRoot == false);
            Dictionary<string, object> ResponseQA = new Dictionary<string, object>();
            Dictionary<string, object> RootResponseQA = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(surveyAnswer.XML))
            {
                XDocument xdoc = XDocument.Parse(surveyAnswer.XML);
                int NumberOfPages = GetNumberOfPags(surveyAnswer.XML);

                Responsedetail.ResponseId = surveyAnswer.ResponseId;
                Responsedetail.FormId = surveyAnswer.SurveyId;
                SurveyControlsResponse surveylist = List.Where(x => x.Key == surveyAnswer.SurveyId).Select(d => d.Value).Single();

                Responsedetail.OKey = FormsHierarchyDTOList[0].SurveyInfo.OrganizationKey.ToString().Substring(0, 8);
                for (int i = 1; NumberOfPages + 1 > i; i++)
                {
                    var _FieldsTypeIDs = from _FieldTypeID in
                   xdoc.Descendants("Page")

                                         where _FieldTypeID.Attribute("PageNumber").Value == (i).ToString()
                                         select _FieldTypeID;

                    var _PageFieldsTypeIDs = from _FieldTypeID1 in
                                                 _FieldsTypeIDs.Descendants("ResponseDetail")

                                             select _FieldTypeID1;
                    foreach (var item in _PageFieldsTypeIDs)
                    {
                        try
                        {
                            string ControlId = item.Attribute("QuestionName").Value;
                            bool IsCheckBox = (bool)surveylist.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType == "CheckBox");
                            bool ISNumericTextBox = (bool)surveylist.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType == "NumericTextBox");
                            if (ISNumericTextBox && !string.IsNullOrEmpty(item.Value))
                            {
                                string uiSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
                                if (item.Value.Contains(uiSep))
                                    RootResponseQA.Add(item.Attribute("QuestionName").Value, Convert.ToDecimal(item.Value));
                                else
                                    RootResponseQA.Add(item.Attribute("QuestionName").Value, Convert.ToInt64(item.Value));
                            }
                            else if (IsCheckBox)
                            {
                                bool Ischecked = false;
                                if (item.Value == "Yes")
                                    RootResponseQA.Add(item.Attribute("QuestionName").Value, !Ischecked);
                                else if (item.Value == "No")
                                    RootResponseQA.Add(item.Attribute("QuestionName").Value, Ischecked);
                                else
                                    RootResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);
                            }
                            else
                                RootResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);
                        }
                        catch(System.Exception ex)
                        {
                            RootResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);
                        }
                    }

                }
                Responsedetail.ResponseQA = RootResponseQA;

                foreach (var child in ChildFormsHierarchy)
                {
                    List<SurveyAnswerDTO> childResponses = child.ResponseIds;
                    foreach (var childresponse in childResponses)
                    {
                        ResponseDetail childresponseDetail = new ResponseDetail();
                        childresponseDetail.FormId = childresponse.SurveyId;
                        childresponseDetail.ResponseId = childresponse.ResponseId;
                        childresponseDetail.ParentResponseId = childresponse.RelateParentId;
                        childresponseDetail.ParentFormId = childresponse.ParentRecordId;
                        ResponseQA = new Dictionary<string, object>();
                        ResponseQA.Add("FKEY", childresponse.RelateParentId);
                        ResponseQA.Add("ResponseId", childresponse.ResponseId);
                        var childsurveylist = List.Where(x => x.Key == childresponse.SurveyId).Select(d => d.Value).Single();
                        if (!string.IsNullOrEmpty(childresponse.XML))
                        { 
                            XDocument xdochild = XDocument.Parse(childresponse.XML);
                            int NumberOfPagesChild = GetNumberOfPags(childresponse.XML);
                            for (int i = 1; NumberOfPagesChild + 1 > i; i++)
                            {
                                var _FieldsTypeIDs = from _FieldTypeID in
                               xdochild.Descendants("Page")
                                                 where _FieldTypeID.Attribute("PageNumber").Value == (i).ToString()
                                                 select _FieldTypeID;

                                var _PageFieldsTypeIDs = from _FieldTypeID1 in
                                                         _FieldsTypeIDs.Descendants("ResponseDetail")

                                                     select _FieldTypeID1;
                                foreach (var item in _PageFieldsTypeIDs)
                                {
                                    try
                                    {
                                        string ControlId = item.Attribute("QuestionName").Value;
                                        bool IsCheckBox = (bool)childsurveylist.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType == "CheckBox");
                                        bool ISNumericTextBox = (bool)childsurveylist.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType == "NumericTextBox");
                                        if (ISNumericTextBox && !string.IsNullOrEmpty(item.Value))
                                        {
                                            string uiSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
                                            if (item.Value.Contains(uiSep))
                                                ResponseQA.Add(item.Attribute("QuestionName").Value, Convert.ToDecimal(item.Value));
                                            else
                                                ResponseQA.Add(item.Attribute("QuestionName").Value, Convert.ToInt64(item.Value));
                                        }
                                        else if (IsCheckBox)
                                        {
                                            bool Ischecked = false;
                                            if (item.Value == "Yes")
                                                ResponseQA.Add(item.Attribute("QuestionName").Value, !Ischecked);
                                            else if (item.Value == "No")
                                                ResponseQA.Add(item.Attribute("QuestionName").Value, Ischecked);
                                            else
                                                ResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);
                                        }
                                        else
                                            ResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);                                        
                                    }
                                    catch (System.Exception ex)
                                    {
                                        ResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);
                                    }
                                }
                            }
                        childresponseDetail.ResponseQA = ResponseQA;
                        Responsedetail.ChildResponseDetailList.Add(childresponseDetail);
                    }
                  }               
                }
                 json = JsonConvert.SerializeObject(Responsedetail);
            }

            return json;
        }





        public static int GetNumberOfPags(string ResponseXml)
        {

            XDocument xdoc = XDocument.Parse(ResponseXml);
            int PageNumber = 0;
            PageNumber = xdoc.Root.Elements("Page").Count();

            return PageNumber;


        }
    }
}
