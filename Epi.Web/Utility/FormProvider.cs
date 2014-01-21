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
            Form form = new Form();
            string surveyId = surveyInfo.SurveyId;
            string cacheKey = surveyId + ",page:" + pageNumber.ToString();

            form = CacheUtility.Get(cacheKey) as Form;

            if (form == null)
            {
                form = new Form();
                form.SurveyInfo = surveyInfo;
                
                if (form.SurveyInfo.IsDraftMode)
                {
                    form.IsDraftModeStyleClass = "draft";
                }

                string xml = form.SurveyInfo.XML;
                form.CurrentPage = pageNumber;

                if (string.IsNullOrEmpty(xml))
                {
                    form.NumberOfPages = 1;
                }
                else
                {
                    form.NumberOfPages = GetNumberOfPages(XDocument.Parse(xml));
                }

                if (string.IsNullOrEmpty(xml) == false)
                {
                    XDocument xdocMetadata = XDocument.Parse(xml);

                    var _FieldsTypeIDs = from _FieldTypeID in xdocMetadata.Descendants("Field")
                                         select _FieldTypeID;

                    double width, height;
                    width = GetWidth(xdocMetadata);
                    height = GetHeight(xdocMetadata);
                    form.PageId = GetPageId(xdocMetadata, pageNumber);
                    form.Width = width;
                    form.Height = height;

                    XElement ViewElement = xdocMetadata.XPathSelectElement("Template/Project/View");
                    string checkcode = ViewElement.Attribute("CheckCode").Value.ToString();
                    StringBuilder JavaScript = new StringBuilder();
                    StringBuilder VariableDefinitions = new StringBuilder();

                    XDocument xdocResponse = XDocument.Parse(surveyAnswer.XML);

                    form.HiddenFieldsList = xdocResponse.Root.Attribute("HiddenFieldsList").Value;
                    form.HighlightedFieldsList = xdocResponse.Root.Attribute("HighlightedFieldsList").Value;
                    form.DisabledFieldsList = xdocResponse.Root.Attribute("DisabledFieldsList").Value;
                    form.RequiredFieldsList = xdocResponse.Root.Attribute("RequiredFieldsList").Value;

                    form.FormCheckCodeObj = form.GetCheckCodeObj(xdocMetadata, xdocResponse, checkcode);
                    form.FormCheckCodeObj.GetVariableJavaScript(VariableDefinitions);
                    form.FormCheckCodeObj.GetSubroutineJavaScript(VariableDefinitions);

                    string pageName = GetPageName(xdocMetadata, pageNumber);

                    JavaScript.Append(GetPageLevelJS(pageNumber, form, pageName, "Before"));
                    JavaScript.Append(GetPageLevelJS(pageNumber, form, pageName, "After"));

                    foreach (var fieldElement in _FieldsTypeIDs)
                    {
                        var Value = GetControlValue(xdocResponse, fieldElement.Attribute("Name").Value);
                        string dropDownValues = "";
                        string formJavaScript = GetFormJavaScript(checkcode, form, fieldElement.Attribute("Name").Value);
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
                                    TextBox textBox = GetTextBox(fieldElement, width, height, form);
                                    form.AddFields(textBox);
                                    break;

                                case "2":
                                    Literal literal = GetLabel(fieldElement, width, height);
                                    form.AddFields(literal);
                                    break;

                                case "4":
                                    TextArea textArea = GetTextArea(fieldElement, width, height, form);
                                    form.AddFields(textArea);
                                    break;

                                case "5":
                                    NumericTextBox numericTextBox = GetNumericTextBox(fieldElement, width, height, form);
                                    form.AddFields(numericTextBox);
                                    break;

                                case "7":
                                    DatePicker datePicker = GetDatePicker(fieldElement, width, height, form);
                                    form.AddFields(datePicker);
                                    break;

                                case "8":
                                    TimePicker timePicker = GetTimePicker(fieldElement, width, height, form);
                                    form.AddFields(timePicker);
                                    break;

                                case "10":
                                    CheckBox checkBox = GetCheckBox(fieldElement, width, height);
                                    form.AddFields(checkBox);
                                    break;

                                case "11": // Yes/No
                                    Select select = GetDropDown(fieldElement, width, height, "Yes&#;No", 11, form);
                                    form.AddFields(select);
                                    break;

                                case "12":
                                    GroupBox optionsContainer = GetOptionGroupBox(fieldElement, width + 12, height);
                                    form.AddFields(optionsContainer);
                                    RadioList radioList = GetRadioList(fieldElement, width, height, form);
                                    form.AddFields(radioList);
                                    break;

                                case "13":
                                    CommandButton commandButton = GetCommandButton(fieldElement, width, height);
                                    form.AddFields(commandButton);
                                    break;

                                case "17": // LegalValues
                                    dropDownValues = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value);
                                    form.AddFields(GetDropDown(fieldElement, width, height, dropDownValues, 17, form));
                                    break;

                                case "18": // Codes
                                    dropDownValues = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value);
                                    form.AddFields(GetDropDown(fieldElement, width, height, dropDownValues, 18, form));
                                    break;

                                case "19": // CommentLegal
                                    dropDownValues = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value);
                                    form.AddFields(GetDropDown(fieldElement, width, height, dropDownValues, 19, form));
                                    break;

                                case "21": //GroupBox
                                    GroupBox groupBox = GetGroupBox(fieldElement, width, height);
                                    form.AddFields(groupBox);
                                    break;
                            }
                        }
                    }

                    form.FormJavaScript = VariableDefinitions.ToString() + "\n" + JavaScript.ToString();
                }

                CacheUtility.Insert(cacheKey, form, surveyId);
            }

            Form clone = form.Clone() as Form;
            clone.ResponseId = surveyAnswer.ResponseId;
            FormProvider.SetStates(clone, surveyAnswer);

            return clone;
        }

        public static void SetStates(Form form, SurveyAnswerDTO answer)
        {
            XDocument xdocResponse = XDocument.Parse(answer.XML);

            foreach (Field field in form.Fields)
            {
                SetState(field, xdocResponse);
            }
        }
                
        public static void SetState(Field field, XDocument xdocResponse)
        {
            if (field is TextBox || field is TextArea)
            {
                SetFieldStates(xdocResponse, field);
                ((TextField)field).Value = GetControlValue(xdocResponse, ((InputField)field).Title);
            }
            else if (field is NumericTextField)
            {
                SetFieldStates(xdocResponse, field); 
                ((NumericTextField)field).Value = GetControlValue(xdocResponse, ((InputField)field).Title);
            }
            else if (field is DatePickerField)
            {
                SetFieldStates(xdocResponse, field); 
                ((DatePickerField)field).Value = GetControlValue(xdocResponse, ((InputField)field).Title);
            }
            else if (field is TimePickerField)
            {
                SetFieldStates(xdocResponse, field); 
                ((TimePickerField)field).Value = GetControlValue(xdocResponse, ((InputField)field).Title);
            }
            else if (field is Literal)
            {
                field.IsHidden = GetControlState(xdocResponse, ((Literal)field).Name, "HiddenFieldsList");
            }
            else if (field is CheckBox)
            {
                SetFieldStates(xdocResponse, field);
                string isChecked = GetControlValue(xdocResponse, ((InputField)field).Title);
                ((CheckBox)field).Checked = isChecked == "Yes" ? true : isChecked == "true" ? true : false;
            }
            else if (field is CommandButton)
            {
                SetFieldStates(xdocResponse, field);
            }
            else if (field is GroupBox)
            {
                SetFieldStates(xdocResponse, field);
            }
            else if (field is RadioList)
            {
                SetFieldStates(xdocResponse, field);
                ((RadioList)field).Value = GetControlValue(xdocResponse, ((InputField)field).Title);
            }
            else if (field is Select)
            {
                SetFieldStates(xdocResponse, field);

                switch (((Select)field).SelectType.ToString())
                {
                    case "11":
                        string selectedYesNoValue = GetControlValue(xdocResponse, ((InputField)field).Title);

                        if (selectedYesNoValue == "1" || selectedYesNoValue == "true")
                        {
                            selectedYesNoValue = "Yes";
                        }

                        if (selectedYesNoValue == "0" || selectedYesNoValue == "false")
                        {
                            selectedYesNoValue = "No";
                        }
                        ((Select)field).SelectedValue = selectedYesNoValue;
                        break;

                    case "17":
                        ((Select)field).SelectedValue = GetControlValue(xdocResponse, ((InputField)field).Title);
                        break;

                    case "18":
                        ((Select)field).SelectedValue = GetControlValue(xdocResponse, ((InputField)field).Title);
                        break;

                    case "19":
                        ((Select)field).SelectedValue = GetControlValue(xdocResponse, ((InputField)field).Title);
                        break;
                }

                if (!string.IsNullOrWhiteSpace(((Select)field).SelectedValue))
                {
                    ((Select)field).Choices[((Select)field).SelectedValue] = true;
                }
            }
        }

        private static void SetFieldStates(XDocument xdocResponse, Field field)
        {
            field.IsHidden = GetControlState(xdocResponse, ((InputField)field).Title, "HiddenFieldsList");
            field.IsHighlighted = GetControlState(xdocResponse, ((InputField)field).Title, "HighlightedFieldsList");
            field.IsDisabled = GetControlState(xdocResponse, ((InputField)field).Title, "DisabledFieldsList");
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
            var RadioList = new RadioList();
            string ListString = _FieldTypeID.Attribute("List").Value;
            ListString = ListString.Replace("||", "|");
            List<string> Lists = ListString.Split('|').ToList<string>();

            Dictionary<string, bool> Choices = new Dictionary<string, bool>();

            List<string> Pattern = new List<string>();
            Choices = GetChoices(Lists[0].Split(',').ToList<string>());
            Pattern = Lists[1].Split(',').ToList<string>();

            RadioList.Title = _FieldTypeID.Attribute("Name").Value;
            RadioList.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            RadioList.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            RadioList.Required = _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false;

            RadioList.RequiredMessage = "This field is required";
            RadioList.Key = _FieldTypeID.Attribute("Name").Value;

            RadioList.Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            RadioList.Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            RadioList.PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            RadioList.ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            RadioList.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            RadioList.fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value);
            RadioList.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;

            RadioList.IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            RadioList.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            RadioList.InputFieldfontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value);
            RadioList.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;

            RadioList.IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");

            RadioList.ShowTextOnRight = bool.Parse(_FieldTypeID.Attribute("ShowTextOnRight").Value);
            RadioList.Choices = Choices;
            RadioList.Width = _Width;
            RadioList.Height = _Height;
            RadioList.Pattern = Pattern;
            RadioList.ChoicesList = ListString;

            if (_Height > _Width)
            {
                RadioList.Orientation = (Orientation)0;
            }
            else
            {

                RadioList.Orientation = (Orientation)1;
            }

            return RadioList;
        }

        private static NumericTextBox GetNumericTextBox(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var NumericTextBox = new NumericTextBox
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

            return NumericTextBox;
        }

        private static Literal GetLabel(XElement _FieldTypeID, double _Width, double _Height)
        {
            var Label = new Literal
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

            return Label;
        }
        
        private static TextArea GetTextArea(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var TextArea = new TextArea
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
        
            return TextArea;
        }
        
        private static Hidden GetHiddenField(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue)
        {
            var result = new Hidden
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Key = _FieldTypeID.Attribute("Name").Value,
                IsPlaceHolder = true,
                Value = _ControlValue
            };
            
            return result;
        }

        private static TextBox GetTextBox(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var TextBox = new TextBox
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

            return TextBox;
        }

        private static CheckBox GetCheckBox(XElement _FieldTypeID, double _Width, double _Height)
        {
            var CheckBox = new CheckBox
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
 
            return CheckBox;
        }

        private static CommandButton GetCommandButton(XElement fieldElement, double width, double height)
        {
            CommandButton commandButton = new CommandButton();
            commandButton.Title = fieldElement.Attribute("PromptText").Value;
            commandButton.DisplayOrder = int.Parse(fieldElement.Attribute("TabIndex").Value);
            commandButton.Prompt = fieldElement.Attribute("PromptText").Value;
            commandButton.Key = fieldElement.Attribute("Name").Value;
            commandButton.Top = height * double.Parse(fieldElement.Attribute("ControlTopPositionPercentage").Value);
            commandButton.Left = width * double.Parse(fieldElement.Attribute("ControlLeftPositionPercentage").Value);
            commandButton.Width = width * double.Parse(fieldElement.Attribute("ControlWidthPercentage").Value);
            commandButton.Height = height * double.Parse(fieldElement.Attribute("ControlHeightPercentage").Value);
            commandButton.fontstyle = fieldElement.Attribute("ControlFontStyle").Value;
            commandButton.fontSize = double.Parse(fieldElement.Attribute("ControlFontSize").Value);
            commandButton.fontfamily = fieldElement.Attribute("ControlFontFamily").Value;
            return commandButton;
        }

        private static DatePicker GetDatePicker(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var DatePicker = new DatePicker
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

            return DatePicker;
        }

        private static TimePicker GetTimePicker(XElement _FieldTypeID, double _Width, double _Height, Form form)
        {
            var TimePicker = new TimePicker
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

            return TimePicker;
        }

        private static Select GetDropDown(XElement _FieldTypeID, double _Width, double _Height, string DropDownValues, int FieldTypeId, Form form)
        {
            Select DropDown = new Select();

            DropDown.Title = _FieldTypeID.Attribute("Name").Value;
            DropDown.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            DropDown.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            DropDown.RequiredMessage = "This field is required";
            DropDown.Key = _FieldTypeID.Attribute("Name").Value;
            DropDown.PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            DropDown.PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            DropDown.Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            DropDown.Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            DropDown.PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            DropDown.ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            DropDown.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            DropDown.fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value);
            DropDown.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            DropDown.IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");
            DropDown.Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");
            DropDown.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            DropDown.InputFieldfontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value);
            DropDown.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            DropDown.IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            DropDown.ShowEmptyOption = true;
            DropDown.SelectType=FieldTypeId;
            DropDown.ControlFontSize = float.Parse(_FieldTypeID.Attribute("ControlFontSize").Value);
            DropDown.ControlFontStyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
                     
            DropDown.EmptyOption = "Select";
            DropDown.AddChoices(DropDownValues, "&#;");

            return DropDown;
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

        private static GroupBox GetGroupBox(XElement fieldTypeID, double width, double height)
        {
            GroupBox GroupBox = new GroupBox();

            GroupBox.fontstyle = fieldTypeID.Attribute("ControlFontStyle").Value;
            GroupBox.fontSize = double.Parse(fieldTypeID.Attribute("ControlFontSize").Value);
            GroupBox.fontfamily = fieldTypeID.Attribute("ControlFontFamily").Value;

            AssignCommonGroupProperties(fieldTypeID, width, height,  GroupBox);

            return GroupBox;
        }

        private static GroupBox GetOptionGroupBox(XElement fieldTypeID, double width, double height)
        {
            GroupBox GroupBox = new GroupBox();

            GroupBox.fontstyle = fieldTypeID.Attribute("PromptFontStyle").Value;
            GroupBox.fontSize = double.Parse(fieldTypeID.Attribute("PromptFontSize").Value);
            GroupBox.fontfamily = fieldTypeID.Attribute("PromptFontFamily").Value;

            AssignCommonGroupProperties(fieldTypeID, width, height,  GroupBox);

            return GroupBox;
        }

        private static void AssignCommonGroupProperties(XElement fieldTypeID, double width, double height, GroupBox GroupBox)
        {
            GroupBox.Title = fieldTypeID.Attribute("Name").Value;
            GroupBox.Prompt = fieldTypeID.Attribute("PromptText").Value;
            GroupBox.RequiredMessage = "This field is required";
            GroupBox.Key = fieldTypeID.Attribute("Name").Value + "_GroupBox";
            GroupBox.PromptTop = height * double.Parse(fieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            GroupBox.PromptLeft = width * double.Parse(fieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            GroupBox.Top = height * double.Parse(fieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            GroupBox.Left = width * double.Parse(fieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            GroupBox.ControlHeight = height * double.Parse(fieldTypeID.Attribute("ControlHeightPercentage").Value) - 12;
            GroupBox.ControlWidth = width * double.Parse(fieldTypeID.Attribute("ControlWidthPercentage").Value) - 12;
        }

        private static int GetNumberOfPages(XDocument Xml)
        {
            var _FieldsTypeIDs = from _FieldTypeID in Xml.Descendants("View")
                                 select  _FieldTypeID;

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
                            field = GetLabel(fieldElement, width, height);
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
                            field = GetCheckBox(fieldElement, width, height);
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
                            field = GetGroupBox(fieldElement, width, height);
                            break;
                    }

                    if (field != null)
                    {
                        field.IsPlaceHolder = true;
                        form.AddFields(field);
                        SetState(field, xdocResponse);
                    }
                }
            }
        }
    }
}
