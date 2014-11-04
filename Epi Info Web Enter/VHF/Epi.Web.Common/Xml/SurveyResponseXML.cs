using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Epi.Web.Enter.Common.Message;
using System.Xml.XPath;

namespace Epi.Web.Enter.Common.Xml
    {
   public  class SurveyResponseXML
        {

        Dictionary<string, string> ResponseDetailList = new Dictionary<string, string>();
        Dictionary<string, string> SurveyFileds = new Dictionary<string, string>();
        IEnumerable<XElement> PageFields;
        private string RequiredList = "";

        public SurveyResponseXML(PreFilledAnswerRequest Request, XDocument SurveyXml) 
            {
            ResponseDetailList = Request.AnswerInfo.SurveyQuestionAnswerList;
          

            var _FieldsTypeIDs = from _FieldTypeID in
                                     SurveyXml.Descendants("Field")

                                 select _FieldTypeID;

            foreach (var _FieldTypeID in _FieldsTypeIDs)
                {

                SurveyFileds.Add(_FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("FieldTypeId").Value);

                }
            }

        //public void Add(MvcDynamicForms.Form pForm)
        //    {
        //    ResponseDetailList.Clear();
        //    foreach (var field in pForm.InputFields)
        //        {
        //        if (!field.IsPlaceHolder)
        //            {
        //            if (this.ResponseDetailList.ContainsKey(field.Title))
        //                {
        //                this.ResponseDetailList[field.Title] = field.Response;
        //                }
        //            else
        //                {
        //                this.ResponseDetailList.Add(field.Title, field.Response);
        //                }
        //            }
        //        }
        //    }

        //public void Add(MvcDynamicForms.Fields.InputField pField)
        //    {
        //    if (this.ResponseDetailList.ContainsKey(pField.Title))
        //        {
        //        this.ResponseDetailList[pField.Title] = pField.GetXML();
        //        }
        //    else
        //        {
        //        this.ResponseDetailList.Add(pField.Title, pField.GetXML());
        //        }
        //    }

        public void SetValue(string pKey, string pXMLValue)
            {
            if (this.ResponseDetailList.ContainsKey(pKey))
                {
                this.ResponseDetailList[pKey] = pXMLValue;
                }
            else
                {
                this.ResponseDetailList.Add(pKey, pXMLValue);
                }
            }


        public string GetValue(string pKey)
            {
            string result = null;

            if (this.ResponseDetailList.ContainsKey(pKey))
                {
                result = this.ResponseDetailList[pKey];
                }

            return result;
            }

        private XmlDocument CreateResponseXml(string SurveyId, bool AddRoot, int CurrentPage, string Pageid)
            {

            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("SurveyResponse");

            if (CurrentPage == 0)
                {
                root.SetAttribute("SurveyId", SurveyId);
                root.SetAttribute("LastPageVisited", "1");
                root.SetAttribute("HiddenFieldsList", "");
                root.SetAttribute("HighlightedFieldsList", "");
                root.SetAttribute("DisabledFieldsList", "");
                root.SetAttribute("RequiredFieldsList", "");
                root.SetAttribute("RecordBeforeFlag", "");
                xml.AppendChild(root);
                }

            XmlElement PageRoot = xml.CreateElement("Page");
            if (CurrentPage != 0)
                {
                PageRoot.SetAttribute("PageNumber", CurrentPage.ToString());
                PageRoot.SetAttribute("PageId", Pageid); 
                xml.AppendChild(PageRoot);
                }

           
            PageRoot = SetResponseValues(PageRoot, xml);
            return xml;
            }

        private static XDocument MergeXml(XDocument SavedXml, XDocument CurrentPageResponseXml, int Pagenumber)
            {

            XDocument xdoc = XDocument.Parse(SavedXml.ToString());
            XElement oldXElement = xdoc.XPathSelectElement("SurveyResponse/Page[@PageNumber = '" + Pagenumber.ToString() + "']");


            if (oldXElement == null)
                {
                SavedXml.Root.Add(CurrentPageResponseXml.Elements());
                return SavedXml;
                }

            else
                {
                oldXElement.Remove();
                xdoc.Root.Add(CurrentPageResponseXml.Elements());
                return xdoc;
                }


            }
        private static int GetNumberOfPages(XDocument Xml)
            {
            var _FieldsTypeIDs = from _FieldTypeID in
                                     Xml.Descendants("View")
                                 select _FieldTypeID;

            return _FieldsTypeIDs.Elements().Count();
            }
        private static XDocument ToXDocument(XmlDocument xmlDocument)
            {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
                {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
                }
            }
        public string CreateResponseDocument(XDocument pMetaData)
            {
            
            XDocument XmlResponse = new XDocument();
            int NumberOfPages = GetNumberOfPages(pMetaData);
            for (int i = 0; NumberOfPages > i-1; i++)
                {
                var _FieldsTypeIDs = from _FieldTypeID in
                                         pMetaData.Descendants("Field")
                                     where _FieldTypeID.Attribute("Position").Value == (i-1).ToString()
                                    
                                     select _FieldTypeID;

                PageFields = _FieldsTypeIDs;

                XDocument CurrentPageXml = ToXDocument(CreateResponseXml("", false, i, i.ToString()));

                if (i == 0)
                    {
                    XmlResponse = ToXDocument(CreateResponseXml("", true, i, i.ToString()));
                    }
                else
                    {
                    XmlResponse = MergeXml(XmlResponse, CurrentPageXml, i);
                    }
                PageFields = null;
                }
            XmlResponse.Root.SetAttributeValue("RequiredFieldsList", RequiredList);
            return XmlResponse.ToString();
            }

        private XmlElement SetResponseValues(XmlElement PageRoot, XmlDocument xml)
            {
         
            foreach (var Field in this.PageFields)
                {
                SetRequiredList(Field);
                if (IsValidType(Field.Attribute("FieldTypeId").Value))
                    {
                        XmlElement child = xml.CreateElement("ResponseDetail");
                        child.SetAttribute("QuestionName", Field.Attribute("Name").Value);
                        foreach (KeyValuePair<string, string> pair in this.ResponseDetailList)
                            {
                            if (Field.Attribute("Name").Value == pair.Key)
                                {

                                child.InnerText =GetControlValue(pair.Value,Field.Attribute("FieldTypeId").Value);
                                break;
                                }
                            else
                                {
                                child.InnerText = Field.Value;
                                }
                            }

                        PageRoot.AppendChild(child);
                    }
                }
            return PageRoot ;  
            }

        private string GetControlValue(string value, string Type)
            {
            string ControlValue = value;
            
                switch (Type)
                    {
                    case "1": // textbox
                      
                        break;

                    case "2"://Label/Title
                       
                        break;
                    case "3"://Label
                        
                        break;
                    case "4"://MultiLineTextBox
                         
                        break;
                    case "5"://NumericTextBox
                        
                        break;

                    case "7":// 7 DatePicker
                        if (!string.IsNullOrEmpty(value))
                            {
                         DateTime DateTime = new DateTime();
                        DateTime.TryParse(value, out DateTime);
                        ControlValue = DateTime.Date.Month + "/" + DateTime.Date.Day + "/" + DateTime.Date.Year;
                         }
                        else {
                            ControlValue = value;
                            }
                        break;
                    case "8": //TimePicker
                        if (!string.IsNullOrEmpty(value))
                            {
                            DateTime Time = new DateTime();
                            DateTime.TryParse(value, out Time);
                            ControlValue = Time.TimeOfDay.ToString();
                            }
                        else {
                            ControlValue = value;
                            }
                        break;
                    case "10"://CheckBox
                        if (value.ToUpper() == "TRUE")
                            {
                            ControlValue = "Yes";
                            }
                        else {
                              ControlValue = "No";
                            }
                        break;

                    case "11"://DropDown Yes/No
                         
                        break;
                    case "12"://RadioList
                         
                        break;
                    case "17"://DropDown LegalValues

                        break;
                    case "18"://DropDown Codes

                        break;
                    case "19"://DropDown CommentLegal

                        break;
                    case "21"://GroupBox
                        break;
                    }

                return ControlValue;
                 
            }
        public void SetRequiredList(XElement _Fields)
            {
            bool isRequired = false;
            string value = _Fields.Attribute("IsRequired").Value;

            if (bool.TryParse(value, out isRequired))
                {
                if (isRequired)
                    {
                    if (!RequiredList.Contains(_Fields.Attribute("Name").Value))
                        {
                        if (RequiredList != "")
                            {
                            RequiredList = RequiredList + "," + _Fields.Attribute("Name").Value.ToLower();
                            }
                        else
                            {
                            RequiredList = _Fields.Attribute("Name").Value.ToLower();
                            }
                        }
                    }
                }
            }
        public Dictionary<string, string> ValidateResponseFileds()
            {
           
            Dictionary<string, string> ErrorList = new Dictionary<string, string>();
               foreach (var Item in ResponseDetailList)
                    {
            
                     if (!SurveyFileds.ContainsKey(Item.Key))
                        {

                        ErrorList.Add(Item.Key, "Field Name Not Found");
                      
                        }
                     }
             
                return ErrorList;
             
            }
        public Dictionary<string, string> ValidateResponseFiledTypes()
            {

            Dictionary<string, string> ErrorList = new Dictionary<string, string>();
            foreach (var Item in ResponseDetailList)
                {

               
                string FieldType ;
                 SurveyFileds.TryGetValue(Item.Key,out FieldType);
                 if (!MatchValueAndType(Item.Value, FieldType))
                    {

                    ErrorList.Add(Item.Key +" Type", "Wrong Value Type");

                    }
                }

            return ErrorList;

            }
        private bool MatchValueAndType(string Value,string Type) 
            {
            bool IsValidType = false;
            try{
            switch (Type)
                {
                case "1": // textbox
                    IsValidType = true;
                    break;

                case "2"://Label/Title
                    IsValidType = true;
                    break;
                case "3"://Label
                    IsValidType = true;
                    break;
                case "4"://MultiLineTextBox
                    IsValidType = true;
                    break;
                case "5"://NumericTextBox
                    IsValidType = IsNumeric(Value);
                    break;

                case "7":// 7 DatePicker
                     
                    break;
                case "8": //TimePicker
                    break;
                case "10"://CheckBox
                    IsValidType = IsYesNo(Value);
                    break;

                case "11"://DropDown Yes/No
                    IsValidType = IsBit(Value);
                    break;
                case "12"://RadioList
                    IsValidType = IsNumeric(Value);
                    break;
                case "17"://DropDown LegalValues

                    break;
                case "18"://DropDown Codes

                    break;
                case "19"://DropDown CommentLegal

                    break;
                case "21"://GroupBox
                    break;
                }

               return IsValidType;
            }
            catch  
            {
                  return false;      
                        
            }

            }
        private bool IsNumeric(string Value ) 
            {
            int num;
            return int.TryParse(Value, out num);
            }
        private bool IsBoolean(string Value)
            {
            bool bol = false;
            if(Value.ToUpper() == "TRUE")
                {
                bol = true;
                }
            return bol;
            }
        private bool IsYesNo(string Value)
            {
            bool bol = false;
            if (Value.ToUpper() == "NO" || Value.ToUpper() == "YES")
                {
                bol = true;
                }
            return bol;
            }
        private bool IsBit(string Value)
            {
            int num;
            bool bol = false;
              if (IsNumeric(Value))
                  {
                    num = int.Parse(Value);
                    if (num == 1 || num == 0)
                        {
                        bol = true;
                        }
                }
            return bol;
            }




        private bool IsValidType( string Type)
            {
            bool IsValidType = true;
            try
                {
                switch (Type)
                    {
                  

                    case "2"://Label/Title
                        IsValidType = false;
                        break;
                    case "3"://Label
                        IsValidType = false;
                        break;
                   
                    }

                return IsValidType;
                }
            catch
                {
                return false;

                }

            }

       }
    }
