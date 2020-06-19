using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Epi.Web.Common.Json
{
    public class SurveyResponseJson
    {

        public string GetSurveyResponseJson(Epi.Web.Common.DTO.SurveyAnswerDTO surveyAnswer, List<FormsHierarchyDTO> FormsHierarchyDTOList, SurveyControlsResponse List)
        {
            if (!string.IsNullOrEmpty(surveyAnswer.XML))
            {
                ResponseDetail Responsedetail = new ResponseDetail();

                var ChildFormsHierarchy = FormsHierarchyDTOList.Where(x => x.IsRoot == false);
                Dictionary<string, object> ResponseQA = new Dictionary<string, object>();
                Dictionary<string, object> RootResponseQA = new Dictionary<string, object>();


                XDocument xdoc = XDocument.Parse(surveyAnswer.XML);
                int NumberOfPages = GetNumberOfPags(surveyAnswer.XML);

                Responsedetail.ResponseId = surveyAnswer.ResponseId;
                Responsedetail.FormId = surveyAnswer.SurveyId;
                if (FormsHierarchyDTOList.Count()>0) {
                    Responsedetail.OKey = FormsHierarchyDTOList[0].SurveyInfo.OrganizationKey.ToString().Substring(0, 8);
                }
                for (int i = 1; NumberOfPages + 1 > i; i++)
                {
                    try
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
                            if (!string.IsNullOrEmpty(item.Value))
                            {
                                try
                                {
                                    string ControlId = item.Attribute("QuestionName").Value;
                                    bool IsCheckBox = (bool)List.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType == "CheckBox");
                                    bool ISNumericTextBox = (bool)List.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType == "NumericTextBox");

                                    if (ISNumericTextBox && !string.IsNullOrEmpty(item.Value))
                                    {
                                        string uiSep = ".";
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
                                        else
                                            RootResponseQA.Add(item.Attribute("QuestionName").Value, Ischecked);
                                    }
                                    else
                                    {
                                        RootResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                            else {

                                RootResponseQA.Add(item.Attribute("QuestionName").Value, "");

                            }
                        }
                    }
                    catch (System.Exception ex)
                    {

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
                                    bool IsCheckBox = (bool)List.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType == "CheckBox");
                                    bool ISNumericTextBox = (bool)List.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType == "NumericTextBox");
                                    if (ISNumericTextBox && item.Value != null)
                                    {
                                        string uiSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
                                        if (item.Value.Contains(uiSep))
                                            ResponseQA.Add(item.Attribute("QuestionName").Value, Convert.ToDecimal(item.Value));
                                        else
                                            ResponseQA.Add(item.Attribute("QuestionName").Value, Convert.ToInt32(item.Value));
                                    }
                                    else if (IsCheckBox)
                                    {
                                        bool Ischecked = false;
                                        if (item.Value == "Yes")
                                            ResponseQA.Add(item.Attribute("QuestionName").Value, !Ischecked);
                                        else
                                            ResponseQA.Add(item.Attribute("QuestionName").Value, Ischecked);
                                    }
                                    else
                                        ResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);
                                    ResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                        }

                        childresponseDetail.ResponseQA = ResponseQA;
                        Responsedetail.ChildResponseDetailList.Add(childresponseDetail);
                    }

                }

                var json = JsonConvert.SerializeObject(Responsedetail);

                return json;
            }
            else
            {

                return "";
            }
        }



        public string GetSurveyResponseJson(Epi.Web.Common.BusinessObject.SurveyResponseBO surveyAnswer,  SurveyControlsResponse List)
        {
            if (!string.IsNullOrEmpty(surveyAnswer.XML))
            {
                ResponseDetail Responsedetail = new ResponseDetail();

               // var ChildFormsHierarchy = FormsHierarchyDTOList.Where(x => x.IsRoot == false);
                Dictionary<string, object> ResponseQA = new Dictionary<string, object>();
                Dictionary<string, object> RootResponseQA = new Dictionary<string, object>();


                XDocument xdoc = XDocument.Parse(surveyAnswer.XML);
                int NumberOfPages = GetNumberOfPags(surveyAnswer.XML);

                Responsedetail.ResponseId = surveyAnswer.ResponseId;
                Responsedetail.FormId = surveyAnswer.SurveyId;
                //if (FormsHierarchyDTOList.Count() > 0)
                //{
                //    Responsedetail.OKey = FormsHierarchyDTOList[0].SurveyInfo.OrganizationKey.ToString().Substring(0, 8);
                //}
                for (int i = 1; NumberOfPages + 1 > i; i++)
                {
                    try
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
                            if (!string.IsNullOrEmpty(item.Value))
                            {
                                try
                                {
                                    string ControlId = item.Attribute("QuestionName").Value;
                                    bool IsCheckBox = (bool)List.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType == "CheckBox");
                                    bool ISNumericTextBox = (bool)List.SurveyControlList.Any(x => x.ControlId == ControlId && x.ControlType == "NumericTextBox");

                                    if (ISNumericTextBox && !string.IsNullOrEmpty(item.Value))
                                    {
                                        string uiSep = ".";
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
                                        else
                                            RootResponseQA.Add(item.Attribute("QuestionName").Value, Ischecked);
                                    }
                                    else
                                    {
                                        RootResponseQA.Add(item.Attribute("QuestionName").Value, item.Value);
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
                Responsedetail.ResponseQA = RootResponseQA;



                var json = JsonConvert.SerializeObject(Responsedetail);

                return json;
            }
            else
            {

                return "";
            }
        }



        public static int GetNumberOfPags(string ResponseXml)
        {

            XDocument xdoc = XDocument.Parse(ResponseXml);
            int PageNumber = 0;
            PageNumber = xdoc.Root.Elements("Page").Count();

            return PageNumber;


        }

        public static string XmlToJson(string xml) {

            StringBuilder JsonString = new StringBuilder();
            //   var mystring =GetDropDownValues(XDocument.Parse(xml));
            XmlDocument doc = new XmlDocument();
           // doc.LoadXml(xml);
            //var json = JsonConvert.SerializeObject(xml);
            var _ControlValues = from _ControlValue in XDocument.Parse(xml).Descendants("SourceTable")
                                    
                                 select _ControlValue;


            var _SourceTableValues = from _SourceTableValue in _ControlValues.Descendants("Item")

                                     select _SourceTableValue;


            var list = _SourceTableValues.ToList();
            JsonString.Append("[");
            for (int i = 0; list.Count()> i; i++)
            {
                doc.LoadXml(list[i].ToString());
                 JsonString.Append(JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true));
                if (i < list.Count()-1)
                {
                    JsonString.Append(",");
                }
            }
            //object transactObject1 = JsonConvert.DeserializeObject(json);
            JsonString.Append("]");
            return JsonString.ToString();

        }
       
    }
}
