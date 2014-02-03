using System.IO;
using System.Linq;
using MvcDynamicForms.Fields;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Web;
using MvcDynamicForms;
using System.Collections.Generic;
using System;
using System.Xml.XPath;
using Epi.Core.EnterInterpreter;
using Epi.Web.Common.DTO;
using Epi.Web.Utility;

namespace Epi.Web.MVC.Utility
{
    public static class FormProvider
    {
        public static Form GetForm(SurveyInfoDTO surveyInfo, int pageNumber, SurveyAnswerDTO surveyAnswer)
        {
            Form _form = new Form();
            string surveyId = surveyInfo.SurveyId;
            string cacheKey = surveyId + ",page:" + pageNumber.ToString();

            _form = CacheUtility.Get(cacheKey) as Form;

            if (_form == null)
            {
                _form = new Form();
                _form.SurveyInfo = surveyInfo;

                if (_form.SurveyInfo.IsDraftMode)
                {
                    _form.IsDraftModeStyleClass = "draft";
                }

                string xml = _form.SurveyInfo.XML;
                _form.CurrentPage = pageNumber;

                if (string.IsNullOrEmpty(xml))
                {
                    _form.NumberOfPages = 1;
                }
                else
                {
                    _form.NumberOfPages = GetNumberOfPages(XDocument.Parse(xml));
                }

                if (string.IsNullOrEmpty(xml) == false)
                {
                    XDocument xdocMetadata = XDocument.Parse(xml);

                    var _FieldsTypeIDs = from _FieldTypeID in xdocMetadata.Descendants("Field")
                                         select _FieldTypeID;

                    double width, height;
                    width = GetWidth(xdocMetadata);
                    height = GetHeight(xdocMetadata);
                    _form.PageId = GetPageId(xdocMetadata, pageNumber);
                    _form.Width = width;
                    _form.Height = height;

                    XElement ViewElement = xdocMetadata.XPathSelectElement("Template/Project/View");
                    string checkcode = ViewElement.Attribute("CheckCode").Value.ToString();
                    StringBuilder JavaScript = new StringBuilder();
                    StringBuilder VariableDefinitions = new StringBuilder();

                    XDocument xdocResponse = XDocument.Parse(surveyAnswer.XML);

                    _form.HiddenFieldsList = xdocResponse.Root.Attribute("HiddenFieldsList").Value;
                    _form.HighlightedFieldsList = xdocResponse.Root.Attribute("HighlightedFieldsList").Value;
                    _form.DisabledFieldsList = xdocResponse.Root.Attribute("DisabledFieldsList").Value;
                    _form.RequiredFieldsList = xdocResponse.Root.Attribute("RequiredFieldsList").Value;

                    _form.FormCheckCodeObj = _form.GetCheckCodeObj(xdocMetadata, xdocResponse, checkcode);
                    _form.FormCheckCodeObj.GetVariableJavaScript(VariableDefinitions);
                    _form.FormCheckCodeObj.GetSubroutineJavaScript(VariableDefinitions);

                    string pageName = GetPageName(xdocMetadata, pageNumber);

                    JavaScript.Append(GetPageLevelJS(pageNumber, _form, pageName, "Before"));
                    JavaScript.Append(GetPageLevelJS(pageNumber, _form, pageName, "After"));

                    foreach (var fieldElement in _FieldsTypeIDs)
                    {
                        string dropDownValues = "";
                        string formJavaScript = GetFormJavaScript(checkcode, _form, fieldElement.Attribute("Name").Value);
                        JavaScript.Append(formJavaScript);

                        if (fieldElement.Attribute("Position").Value != (pageNumber - 1).ToString())
                        {
                        }
                        else
                        {
                            switch (fieldElement.Attribute("FieldTypeId").Value)
                            {
                                case "1":
                                case "3":
                                    TextBox textBox = GetTextBox(fieldElement, width, height, _form);
                                    _form.Fields.Add(textBox.Name.ToLower(), textBox);
                                    break;

                                case "2":
                                    Literal literal = GetLabel(fieldElement, width, height, _form);
                                    _form.Fields.Add(literal.Name.ToLower(), literal);
                                    break;

                                case "4":
                                    TextArea textArea = GetTextArea(fieldElement, width, height, _form);
                                    _form.Fields.Add(textArea.Name.ToLower(), textArea);
                                    break;

                                case "5":
                                    NumericTextBox numericTextBox = GetNumericTextBox(fieldElement, width, height, _form);
                                    _form.Fields.Add(numericTextBox.Name.ToLower(), numericTextBox);
                                    break;

                                case "7":
                                    DatePicker datePicker = GetDatePicker(fieldElement, width, height, _form);
                                    _form.Fields.Add(datePicker.Name.ToLower(), datePicker);
                                    break;

                                case "8":
                                    TimePicker timePicker = GetTimePicker(fieldElement, width, height, _form);
                                    _form.Fields.Add(timePicker.Name.ToLower(), timePicker);
                                    break;

                                case "10":
                                    CheckBox checkBox = GetCheckBox(fieldElement, width, height, _form);
                                    _form.Fields.Add(checkBox.Name.ToLower(), checkBox);
                                    break;

                                case "11": // Yes/No
                                    Select select = GetDropDown(fieldElement, width, height, "Yes&#;No", 11, _form);
                                    _form.Fields.Add(select.Name.ToLower(), select);
                                    break;

                                case "12":
                                    GroupBox optionsContainer = GetOptionGroupBox(fieldElement, width + 12, height, _form);
                                    _form.Fields.Add(optionsContainer.Name.ToLower(), optionsContainer);
                                    RadioList radioList = GetRadioList(fieldElement, width, height, _form);
                                    _form.Fields.Add(radioList.Name.ToLower(), radioList);
                                    break;

                                case "13":
                                    CommandButton commandButton = GetCommandButton(fieldElement, width, height, _form);
                                    _form.Fields.Add(commandButton.Name.ToLower(), commandButton);
                                    break;

                                case "17": // LegalValues
                                    dropDownValues = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value);
                                    Select legalValueDropDown = GetDropDown(fieldElement, width, height, dropDownValues, 17, _form);
                                    _form.Fields.Add(legalValueDropDown.Name.ToLower(), legalValueDropDown);
                                    break;

                                case "18": // Codes
                                    dropDownValues = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value);
                                    Select codesDropDown = GetDropDown(fieldElement, width, height, dropDownValues, 18, _form);
                                    _form.Fields.Add(codesDropDown.Name.ToLower(), codesDropDown);
                                    break;

                                case "19": // CommentLegal
                                    dropDownValues = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value);
                                    Select commentLegalDropDown = GetDropDown(fieldElement, width, height, dropDownValues, 19, _form);
                                    _form.Fields.Add(commentLegalDropDown.Name.ToLower(), commentLegalDropDown);
                                    break;

                                case "21": //GroupBox
                                    GroupBox groupBox = GetGroupBox(fieldElement, width, height, _form);
                                    _form.Fields.Add(groupBox.Name.ToLower(), groupBox);
                                    break;
                            }
                        }
                    }

                    _form.FormJavaScript = VariableDefinitions.ToString() + "\n" + JavaScript.ToString();
                }

                CacheUtility.Insert(cacheKey, _form, surveyId);
            }

            Form clone = _form.Clone() as Form;
            clone.ResponseId = surveyAnswer.ResponseId;

            if (surveyAnswer.XML.Contains("ResponseDetail"))
            {
                FormProvider.SetStates(clone, surveyAnswer);
            }

            return clone;
        }

        public static void SetStates(Form form, SurveyAnswerDTO answer)
        {
            Dictionary<string, string> fieldValues = new Dictionary<string, string>();
            List<string> HiddenFieldsList = new List<string>();
            List<string> HighlightedFieldsList = new List<string>();
            List<string> DisabledFieldsList = new List<string>();
            List<string> RequiredFieldsList = new List<string>();

            XmlReader reader = XmlReader.Create(new StringReader(answer.XML));

            while (reader.Read())
            {
                if (reader.Name == "SurveyResponse")
                {
                    while (reader.MoveToNextAttribute())
                    {
                        switch (reader.Name)
                        {
                            case "SurveyId":
                                break;
                            case "LastPageVisited":
                                break;
                            case "HiddenFieldsList":
                                HiddenFieldsList = reader.Value.Split(new char[] { ',' }).ToList<string>();
                                break;
                            case "HighlightedFieldsList":
                                HighlightedFieldsList = reader.Value.Split(new char[] { ',' }).ToList<string>();
                                break;
                            case "DisabledFieldsList":
                                DisabledFieldsList = reader.Value.Split(new char[] { ',' }).ToList<string>();
                                break;
                        }
                    }
                }
                else if (reader.Name == "ResponseDetail")
                {
                    if (reader.IsEmptyElement == false && reader.NodeType != XmlNodeType.EndElement)
                    {
                        string fieldName = reader.GetAttribute("QuestionName");
                        string fieldValue = reader.ReadElementContentAsString();
                        fieldValues.Add(fieldName.ToLower(), fieldValue);
                    }
                }
            }

            foreach (string fieldName in HiddenFieldsList)
            {
                if (form.Fields.Keys.Contains(fieldName.ToLower()))
                {
                    form.Fields[fieldName].IsHidden = true;
                }
            }

            foreach (string fieldName in HighlightedFieldsList)
            {
                if (form.Fields.Keys.Contains(fieldName.ToLower()))
                {
                    form.Fields[fieldName].IsHighlighted = true;
                }
            }

            foreach (string fieldName in DisabledFieldsList)
            {
                if (form.Fields.Keys.Contains(fieldName.ToLower()))
                {
                    form.Fields[fieldName].IsDisabled = true;
                }
            }

            foreach (KeyValuePair<string, string> kvp in fieldValues)
            {
                if (form.Fields.Keys.Contains(kvp.Key))
                { 
                    Field field = form.Fields[kvp.Key];

                    if (field is InputField)
                    {
                        ((InputField)field).Response = kvp.Value;
                    }

                    if (field is CheckBox)
                    {
                        ((CheckBox)field).Checked = ((InputField)field).Response == "Yes" ? true : ((InputField)field).Response == "true" ? true : false;
                    }
                    else if (field is Select)
                    {
                        switch (((Select)field).SelectType.ToString())
                        {
                            case "11":
                                if (((InputField)field).Response == "1" || ((InputField)field).Response == "true")
                                {
                                    ((InputField)field).Response = "Yes";
                                }

                                if (((InputField)field).Response == "0" || ((InputField)field).Response == "false")
                                {
                                    ((InputField)field).Response = "No";
                                }

                                ((Select)field).SelectedValue = ((InputField)field).Response;
                                break;

                            case "17":
                            case "18":
                            case "19":
                                ((Select)field).SelectedValue = ((InputField)field).Response;
                                break;
                        }

                        if (!string.IsNullOrWhiteSpace(((Select)field).SelectedValue))
                        {
                            ((Select)field).Choices[((Select)field).SelectedValue] = true;
                        }
                    }
                }
            }
        }
        
        private static void SetFieldCommon(Field field, Form form)
        {
            field.FunctionObjectAfter = (EnterRule)form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + field.Key);
            field.FunctionObjectBefore = (EnterRule)form.FormCheckCodeObj.GetCommand("level=field&event=before&identifier=" + field.Key);
            field.FunctionObjectClick = (EnterRule)form.FormCheckCodeObj.GetCommand("level=field&event=click&identifier=" + field.Key);
        }

        public static double GetHeight(XDocument xdoc) 
        {
            try
            {
                if (GetOrientation(xdoc) == "Portrait")
                {
                    var top = from Node in
                               xdoc.Descendants("View")
                               select Node.Attribute("Height").Value;

                    return double.Parse(top.First());
                }
                else 
                {
                    var top = from Node in
                               xdoc.Descendants("View")
                               select Node.Attribute("Width").Value;

                    return double.Parse(top.First());
                }
            }
            catch
            {
                return 768;
            }
        }

        public static double GetWidth(XDocument xdoc)
        {
            try
            {
                if (GetOrientation(xdoc) == "Portrait")
                {
                    var _left = (from Node in
                                 xdoc.Descendants("View")
                                 select Node.Attribute("Width").Value);

                    return double.Parse(_left.First());
                }
                else
                {

                    var _top = from Node in
                               xdoc.Descendants("View")
                               select Node.Attribute("Height").Value;

                    return double.Parse(_top.First());
                }
            }
            catch
            {
                return  1024;
            }
        }

        public static string GetOrientation(XDocument xdoc)
        {
            try
            {
                var Orientation = (from Node in
                                  xdoc.Descendants("View")
                                  select Node.Attribute("Orientation").Value);

                return Orientation.First().ToString();
            }
            catch
            {
                return null;
            }
        }

        public static string GetControlValue(XDocument xdoc, string ControlName) 
        {
            string ControlValue = "";

            if (!string.IsNullOrEmpty(xdoc.ToString()))
            {
                var _ControlValues = from _ControlValue in
                                     xdoc.Descendants("ResponseDetail")
                                     where _ControlValue.Attribute("QuestionName").Value == ControlName.ToString()
                                     select _ControlValue;

                foreach (var _ControlValue in _ControlValues)
                {
                    ControlValue = _ControlValue.Value ;
                }
            }

            return ControlValue;
        }

        private static RadioList GetRadioList(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            RadioList field = new RadioList();
            string ListString = _FieldTypeID.Attribute("List").Value;
            ListString = ListString.Replace("||", "|");
            List<string> Lists = ListString.Split('|').ToList<string>();

            Dictionary<string, bool> Choices = new Dictionary<string, bool>();

            List<string> Pattern = new List<string>();
            Choices = GetChoices(Lists[0].Split(',').ToList<string>());
            Pattern = Lists[1].Split(',').ToList<string>();

            field.Title = _FieldTypeID.Attribute("Name").Value;
            field.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.Required = _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false;

            field.RequiredMessage = "This field is required";
            field.Key = _FieldTypeID.Attribute("Name").Value;

            field.Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;

            field.IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            field.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            field.InputFieldfontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;

            field.IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");

            field.ShowTextOnRight = bool.Parse(_FieldTypeID.Attribute("ShowTextOnRight").Value);
            field.Choices = Choices;
            field.Width = _Width;
            field.Height = _Height;
            field.Pattern = Pattern;
            field.ChoicesList = ListString;

            if (_Height > _Width)
            {
                field.Orientation = (Orientation)0;
            }
            else
            {

                field.Orientation = (Orientation)1;
            }

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);
            return field;
        }

        private static NumericTextBox GetNumericTextBox(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var field = new NumericTextBox
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                RequiredMessage =  "This field is required",
                Key = _FieldTypeID.Attribute("Name").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),
                Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),
                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Lower = _FieldTypeID.Attribute("Lower").Value,
                Upper = _FieldTypeID.Attribute("Upper").Value,
                Pattern = _FieldTypeID.Attribute("Pattern").Value
            };

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);
            return field;
        }

        private static Literal GetLabel(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var field = new Literal
            {
                FieldWrapper = "div",
                Wrap = true,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                Html = _FieldTypeID.Attribute("PromptText").Value,
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                CssClass = "EpiLabel",
                fontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value,
                fontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value,
                Height = _Height * double.Parse(_FieldTypeID.Attribute("ControlHeightPercentage").Value),
                Name =_FieldTypeID.Attribute("Name").Value,
                Width = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value)
            };

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);
            return field;
        }
        
        private static TextArea GetTextArea(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var field = new TextArea
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                RequiredMessage = "This field is required",
                Key = _FieldTypeID.Attribute("Name").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlHeight = _Height * double.Parse(_FieldTypeID.Attribute("ControlHeightPercentage").Value),
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,

                InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value,
                InputFieldfontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value),
                InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value,

                IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),
                Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),
                
                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value)
            };

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);
            return field;
        }
        
        private static Hidden GetHiddenField(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, Form form)
        {
            var field = new Hidden
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Key = _FieldTypeID.Attribute("Name").Value,
                IsPlaceHolder = true,
                Response = _ControlValue
            };

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);
            return field;
        }

        private static TextBox GetTextBox(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var field = new TextBox
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value.Trim(),
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                RequiredMessage = "This field is required",
                Key = _FieldTypeID.Attribute("Name").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),
                Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),
                InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value,
                InputFieldfontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value),
                InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value,

                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                MaxLength = int.Parse(_FieldTypeID.Attribute("MaxLength").Value),
            };

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);
            return field;
        }

        private static CheckBox GetCheckBox(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var field = new CheckBox
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                RequiredMessage = "This field is required",
                Key = _FieldTypeID.Attribute("Name").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value)+2,
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value)+20,
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = 10,
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value) 
            };

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);
            return field;
        }

        private static CommandButton GetCommandButton(XElement fieldElement, double width, double height, Form form)
        {
            CommandButton field = new CommandButton();
            field.Title = fieldElement.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(fieldElement.Attribute("TabIndex").Value);
            field.Prompt = fieldElement.Attribute("PromptText").Value;
            field.Key = fieldElement.Attribute("Name").Value;
            field.Name = fieldElement.Attribute("Name").Value;
            field.Top = height * double.Parse(fieldElement.Attribute("ControlTopPositionPercentage").Value);
            field.Left = width * double.Parse(fieldElement.Attribute("ControlLeftPositionPercentage").Value);
            field.Width = width * double.Parse(fieldElement.Attribute("ControlWidthPercentage").Value);
            field.Height = height * double.Parse(fieldElement.Attribute("ControlHeightPercentage").Value);
            field.fontstyle = fieldElement.Attribute("ControlFontStyle").Value;
            field.fontSize = double.Parse(fieldElement.Attribute("ControlFontSize").Value);
            field.fontfamily = fieldElement.Attribute("ControlFontFamily").Value;
            SetFieldCommon(field, form);
            return field;
        }

        private static DatePicker GetDatePicker(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var field = new DatePicker
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                RequiredMessage = "This field is required",
                Key = _FieldTypeID.Attribute("Name").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),
                Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),
                InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value,
                InputFieldfontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value),
                InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value,
                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Lower = _FieldTypeID.Attribute("Lower").Value,
                Upper = _FieldTypeID.Attribute("Upper").Value,
                ReadOnly= bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Pattern = _FieldTypeID.Attribute("Pattern").Value
            };

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);
            return field;
        }

        private static TimePicker GetTimePicker(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var field = new TimePicker
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                RequiredMessage = "This field is required",
                Key = _FieldTypeID.Attribute("Name").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),
                Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),
                InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value,
                InputFieldfontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value),
                InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value,
                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Lower = _FieldTypeID.Attribute("Lower").Value,
                Upper = _FieldTypeID.Attribute("Upper").Value,
                ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Pattern = _FieldTypeID.Attribute("Pattern").Value
            };

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);
            return field;
        }

        private static Select GetDropDown(XElement _FieldTypeID, double _Width, double _Height, string DropDownValues, int FieldTypeId, Form form)
        {
            Select field = new Select();

            field.Name = _FieldTypeID.Attribute("Name").Value;
            field.Title = _FieldTypeID.Attribute("Name").Value;
            field.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.RequiredMessage = "This field is required";
            field.Key = _FieldTypeID.Attribute("Name").Value;
            field.PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            field.PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            field.Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            field.IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");
            field.Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");
            field.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            field.InputFieldfontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            field.IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            field.ShowEmptyOption = true;
            field.SelectType = FieldTypeId;
            field.ControlFontSize = float.Parse(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.ControlFontStyle = _FieldTypeID.Attribute("ControlFontStyle").Value;

            field.EmptyOption = "Select";
            field.AddChoices(DropDownValues, "&#;");

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);
            return field;
        }

        public static string GetDropDownValues(XDocument xdoc, string ControlName, string TableName)
        {
            StringBuilder DropDownValues = new StringBuilder();

            if (!string.IsNullOrEmpty(xdoc.ToString()))
            {
                var _ControlValues = from _ControlValue in xdoc.Descendants("SourceTable")
                                     where _ControlValue.Attribute("TableName").Value == TableName.ToString()
                                     select _ControlValue;

                foreach (var _ControlValue in _ControlValues)
                {
                    var _SourceTableValues = from _SourceTableValue in _ControlValues.Descendants("Item")
                                             select _SourceTableValue;

                    foreach (var _SourceTableValue in _SourceTableValues)
                    {
                        DropDownValues.Append(_SourceTableValue.FirstAttribute.Value.Trim());
                        DropDownValues.Append("&#;");
                    }
                }
            }

            return DropDownValues.ToString();
        }

        private static GroupBox GetGroupBox(XElement fieldTypeID, double width, double height, Form form)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.fontstyle = fieldTypeID.Attribute("ControlFontStyle").Value;
            groupBox.fontSize = double.Parse(fieldTypeID.Attribute("ControlFontSize").Value);
            groupBox.fontfamily = fieldTypeID.Attribute("ControlFontFamily").Value;
            AssignCommonGroupProperties(fieldTypeID, width, height, groupBox, form);
            return groupBox;
        }

        private static GroupBox GetOptionGroupBox(XElement fieldTypeID, double width, double height, Form form)
        {
            GroupBox groupBox = new GroupBox();
            groupBox.fontstyle = fieldTypeID.Attribute("PromptFontStyle").Value;
            groupBox.fontSize = double.Parse(fieldTypeID.Attribute("PromptFontSize").Value);
            groupBox.fontfamily = fieldTypeID.Attribute("PromptFontFamily").Value;
            AssignCommonGroupProperties(fieldTypeID, width, height, groupBox, form);
            return groupBox;
        }

        private static void AssignCommonGroupProperties(XElement fieldTypeID, double width, double height, GroupBox groupBox, Form form)
        {
            groupBox.Name = fieldTypeID.Attribute("Name").Value;
            groupBox.Title = fieldTypeID.Attribute("Name").Value;
            groupBox.Prompt = fieldTypeID.Attribute("PromptText").Value;
            groupBox.RequiredMessage = "This field is required";
            groupBox.Key = fieldTypeID.Attribute("Name").Value + "_GroupBox";
            groupBox.PromptTop = height * double.Parse(fieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            groupBox.PromptLeft = width * double.Parse(fieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            groupBox.Top = height * double.Parse(fieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            groupBox.Left = width * double.Parse(fieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            groupBox.ControlHeight = height * double.Parse(fieldTypeID.Attribute("ControlHeightPercentage").Value) - 12;
            groupBox.ControlWidth = width * double.Parse(fieldTypeID.Attribute("ControlWidthPercentage").Value) - 12;
            SetFieldCommon(groupBox, form);
        }

        private static int GetNumberOfPages(XDocument Xml)
        {
            var _FieldsTypeIDs = from _FieldTypeID in Xml.Descendants("View") select  _FieldTypeID;
            return _FieldsTypeIDs.Elements().Count() ;
        }

        public static bool GetControlState(XDocument xdoc, string ControlName, string ListName)
        {
            bool _Val = false;

            if (!string.IsNullOrEmpty(xdoc.ToString()))
            {
                if (!string.IsNullOrEmpty(xdoc.Root.Attribute(ListName).Value.ToString()))
                {
                    string List =xdoc.Root.Attribute(ListName).Value;
                    string[] ListArray = List.Split(',');
                    for (var i = 0; i < ListArray.Length; i++)
                    {
                        if (ListArray[i]  == ControlName.ToLower())
                        {
                            _Val = true;
                            break;
                        }
                        else
                        {
                            _Val = false;
                        }
                    }
                }
            }

            return _Val;
        }

        public static bool GetRequiredControlState(string Requiredlist, string ControlName, string ListName)
        {
            bool _Val = false;

            if (!string.IsNullOrEmpty(Requiredlist))
            {
                if (!string.IsNullOrEmpty(Requiredlist))
                {
                    string List = Requiredlist;
                    string[] ListArray = List.Split(',');
                    for (var i = 0; i < ListArray.Length; i++)
                    {
                        if (ListArray[i].ToLower() == ControlName.ToLower())
                        {
                            _Val = true;
                            break;
                        }
                        else
                        {
                            _Val = false;
                        }
                    }
                }
            }

            return _Val;
        }

        public static string GetPageId(XDocument xdoc, int PageNumber)
        {
            XElement XElement = xdoc.XPathSelectElement("Template/Project/View/Page[@Position = '" + (PageNumber - 1).ToString() + "']");
            return   XElement.Attribute("PageId").Value.ToString();
        }

        public static string GetPageName(XDocument xdoc, int PageNumber)
        {
            XElement XElement = xdoc.XPathSelectElement("Template/Project/View/Page[@Position = '" + (PageNumber - 1).ToString() + "']");
            return XElement.Attribute("Name").Value.ToString();
        }

        public static string GetFormJavaScript(string CheckCode, Form form, string controlName)
        {
          StringBuilder B_JavaScript = new StringBuilder();
          EnterRule FunctionObject_B = (EnterRule)form.FormCheckCodeObj.GetCommand("level=field&event=before&identifier=" + controlName);
          if (FunctionObject_B != null && !FunctionObject_B.IsNull())
          {
              B_JavaScript.Append("function " + controlName.ToLower());
              FunctionObject_B.ToJavaScript(B_JavaScript);
          }

          StringBuilder A_JavaScript = new StringBuilder();
          EnterRule FunctionObject_A = (EnterRule)form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + controlName);
          if (FunctionObject_A != null && !FunctionObject_A.IsNull())
          {
              A_JavaScript.Append("function " + controlName.ToLower());
              FunctionObject_A.ToJavaScript(A_JavaScript);
          }

          EnterRule FunctionObject = (EnterRule)form.FormCheckCodeObj.GetCommand("level=field&event=click&identifier=" + controlName);
          if (FunctionObject != null && !FunctionObject.IsNull())
          {
              A_JavaScript.Append("function " + controlName.ToLower());
              FunctionObject.ToJavaScript(A_JavaScript);
          }

          return  B_JavaScript.ToString() + "  " + A_JavaScript.ToString();
        }

        public static string GetPageLevelJS(int PageNumber, Form form,string PageName,string BeforeOrAfter)
        {
            StringBuilder JavaScript = new StringBuilder();

            if (BeforeOrAfter == "Before")
            {
                Epi.Core.EnterInterpreter.Rules.Rule_Begin_Before_Statement FunctionObject_B = (Epi.Core.EnterInterpreter.Rules.Rule_Begin_Before_Statement)form.FormCheckCodeObj.GetCommand("level=page&event=before&identifier=" + PageName);
                if (FunctionObject_B != null && !FunctionObject_B.IsNull())
                {
                    JavaScript.Append("$(document).ready(function () {  ");
                    JavaScript.Append("page" + PageNumber + "_before();");
                    JavaScript.Append("});");

                    JavaScript.Append("\n\nfunction page" + PageNumber);
                    FunctionObject_B.ToJavaScript(JavaScript);
                }
            }
            
            if (BeforeOrAfter == "After")
            {
                Epi.Core.EnterInterpreter.Rules.Rule_Begin_After_Statement FunctionObject_A = (Epi.Core.EnterInterpreter.Rules.Rule_Begin_After_Statement)form.FormCheckCodeObj.GetCommand("level=page&event=after&identifier=" + PageName);
                if (FunctionObject_A != null && !FunctionObject_A.IsNull())
                {
                    JavaScript.AppendLine("$(document).ready(function () {");
                    JavaScript.AppendLine("$('#myform').submit(function () {");
                    JavaScript.AppendLine("page" + PageNumber + "_after();})");
                    JavaScript.AppendLine("});");

                    JavaScript.Append("\n\nfunction page" + PageNumber);
                    FunctionObject_A.ToJavaScript(JavaScript);
                }
            }

            return JavaScript.ToString();
        }

        public static Dictionary<string, bool> GetChoices(List<string> List) 
        {
            Dictionary<string, bool> NewList = new Dictionary<string, bool>();
            foreach (var _List in List)
            {
                NewList.Add(_List, false);
            }
            return NewList;
        }

        public static void UpdateHiddenFields(int currentPage, Form form, XDocument xdocMetadata, XDocument xdocResponse, System.Collections.Specialized.NameValueCollection postedForm)
        {
            double width = 1024;
            double height = 768;

            var fieldElementList = from fieldElements in xdocMetadata.Descendants("Field")
                                   where fieldElements.Parent.Attribute("Position").Value != (currentPage - 1).ToString()
                                   select fieldElements;

            foreach (var fieldElement in fieldElementList)
            {
                bool isFound = false;
                string value = null;

                foreach (var key in postedForm.AllKeys.Where(x => x.StartsWith(form.FieldPrefix)))
                {
                    string fieldKey = key.Remove(0, form.FieldPrefix.Length);

                    if (fieldKey.Equals(fieldElement.Attribute("Name").Value, StringComparison.OrdinalIgnoreCase))
                    {
                        value = postedForm[key];
                        isFound = true;
                        break;
                    }
                }

                if (isFound)
                {
                    MvcDynamicForms.Fields.Field field = null;

                    switch (fieldElement.Attribute("FieldTypeId").Value)
                    {
                        case "1":
                        case "3":
                            field = GetTextBox(fieldElement, width, height, form);
                            break;

                        case "2":
                            field = GetLabel(fieldElement, width, height, form);
                            break;

                        case "4":
                            field = GetTextArea(fieldElement, width, height, form);
                            break;

                        case "5":
                            field = GetNumericTextBox(fieldElement, width, height, form);
                            break;

                        case "7":
                            field = GetDatePicker(fieldElement, width, height, form);
                            break;

                        case "8":
                            field = GetTimePicker(fieldElement, width, height, form);
                            break;

                        case "10":
                            field = GetCheckBox(fieldElement, width, height, form);
                            break;

                        case "11": // Yes/No
                            field = GetDropDown(fieldElement, width, height, "Yes&#;No", 11, form);
                            break;

                        case "12": //RadioList
                            field = GetRadioList(fieldElement, width, height, form);
                            break;

                        case "17": // LegalValues
                            string legalValues = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value);
                            field = GetDropDown(fieldElement, width, height, legalValues, 17, form);
                            break;

                        case "18": // Codes
                            string codes = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value);
                            field = GetDropDown(fieldElement, width, height, codes, 18, form);
                            break;

                        case "19": // CommentLegal
                            string commentLegal = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value);
                            field = GetDropDown(fieldElement, width, height, commentLegal, 19, form);
                            break;

                        case "21": //GroupBox
                            field = GetGroupBox(fieldElement, width, height, form);
                            break;
                    }

                    if (field != null)
                    {
                        field.IsPlaceHolder = true;
                        form.Fields.Add(field.Name, field);
                        
                        //form.AddFields(field);
                        //SetState(field, xdocResponse);
                    }
                }
            }
        }
    }
}
