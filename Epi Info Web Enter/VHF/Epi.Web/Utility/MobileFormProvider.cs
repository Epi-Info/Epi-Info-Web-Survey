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

namespace Epi.Web.MVC.Utility
{
    public class MobileFormProvider
    {

        public static List<Epi.Web.Enter.Common.DTO.SurveyAnswerDTO> SurveyAnswerList;
        public static List<Epi.Web.Enter.Common.DTO.SurveyInfoDTO> SurveyInfoList;
        
        public static Form GetForm(object SurveyMetaData, int PageNumber, Epi.Web.Enter.Common.DTO.SurveyAnswerDTO _SurveyAnswer)
        {
            string SurveyAnswer;

            if (_SurveyAnswer != null)
            {
                SurveyAnswer = _SurveyAnswer.XML;

            }
            else { SurveyAnswer = ""; }

            var form = new Form();

            form.ResponseId = _SurveyAnswer.ResponseId;

            form.SurveyInfo = (Epi.Web.Enter.Common.DTO.SurveyInfoDTO)(SurveyMetaData);

            string XML = form.SurveyInfo.XML;

            form.CurrentPage = PageNumber;
            if (form.SurveyInfo.IsDraftMode)
                {
                form.IsDraftModeStyleClass = "draft";
                }
           
            XDocument xdoc = XDocument.Parse(XML);
            if (string.IsNullOrEmpty(XML))
            {

                form.NumberOfPages = 1;
            }
            else
            {
                form.NumberOfPages = GetNumberOfPages(xdoc);
            }
            if (string.IsNullOrEmpty(XML))
            {
                // no XML what to do?
            }
            else
            {
               // XDocument xdoc = XDocument.Parse(XML);


                var _FieldsTypeIDs = from _FieldTypeID in
                                         xdoc.Descendants("Field")
                                     //where _FieldTypeID.Attribute("Position").Value == (PageNumber - 1).ToString()
                                     select _FieldTypeID;


                double _Width, _Height;
                _Width = GetWidth(xdoc);
                _Height = GetHeight(xdoc);
                form.FormWrapperClass = "MvcDynamicMobileForm";
                //form.FormWrapperClass = "";
                form.PageId = GetPageId(xdoc, PageNumber);
                form.IsMobile = true;
                
                //form.Width = _Width;
                //form.Height = _Height;
                //form.Width = 550;
                //form.Height =300;
                //Add checkcode to Form
                XElement ViewElement = xdoc.XPathSelectElement("Template/Project/View");
                string checkcode = ViewElement.Attribute("CheckCode").Value.ToString();
                StringBuilder JavaScript = new StringBuilder();
                StringBuilder VariableDefinitions = new StringBuilder();
                string defineFormat = "cce_Context.define(\"{0}\", \"{1}\", \"{2}\", \"{3}\");";
                string defineNumberFormat = "cce_Context.define(\"{0}\", \"{1}\", \"{2}\", new Number({3}));";

                XDocument xdocResponse = XDocument.Parse(_SurveyAnswer.XML);

                form.HiddenFieldsList = xdocResponse.Root.Attribute("HiddenFieldsList").Value;
                form.HighlightedFieldsList = xdocResponse.Root.Attribute("HighlightedFieldsList").Value;
                form.DisabledFieldsList = xdocResponse.Root.Attribute("DisabledFieldsList").Value;
                form.RequiredFieldsList = xdocResponse.Root.Attribute("RequiredFieldsList").Value;
                // Adding Required fileds from MetaData to the list

                if (SurveyAnswerList != null)
                    {

                    form.FormCheckCodeObj = form.GetRelateCheckCodeObj(GetRelateFormObj(), checkcode);
                    }
                else
                    {
                    form.FormCheckCodeObj = form.GetCheckCodeObj(xdoc, xdocResponse, checkcode);
                    }

                form.FormCheckCodeObj.GetVariableJavaScript(VariableDefinitions);
                form.FormCheckCodeObj.GetSubroutineJavaScript(VariableDefinitions);

                string PageName = GetPageName(xdoc, PageNumber);


                //Generate page level Java script (Before)
                JavaScript.Append(GetPageLevelJS(PageNumber, form, PageName, "Before"));
                //Generate page level Java script (After)
                JavaScript.Append(GetPageLevelJS(PageNumber, form, PageName, "After"));

                foreach (var _FieldTypeID in _FieldsTypeIDs)
                {
                    var Value = GetControlValue(xdocResponse, _FieldTypeID.Attribute("Name").Value);

                    JavaScript.Append(GetFormJavaScript(checkcode, form, _FieldTypeID.Attribute("Name").Value));


                    if (_FieldTypeID.Attribute("Position").Value != (PageNumber - 1).ToString())
                    {
                        //    form.AddFields(GetHiddenField(_FieldTypeID, _Width, _Height, SurveyAnswer, Value));
                    }
                    else
                    {
                        switch (_FieldTypeID.Attribute("FieldTypeId").Value)
                        {
                            case "1": // textbox

                                var _TextBoxValue = Value;
                                form.AddFields(GetTextBox(_FieldTypeID, _Width, _Height, xdocResponse, _TextBoxValue, form));
                                //                                             pName, pType, pSource
                                //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value,"textbox","datasource",Value)); 
                                break;

                            case "2"://Label/Title
                                form.AddFields(GetLabel(_FieldTypeID, _Width, _Height, xdocResponse));
                                //                                             pName, pType, pSource
                                //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "lable", "datasource",Value)); 
                                break;
                            case "3"://Label

                                break;
                            case "4"://MultiLineTextBox

                                var _TextAreaValue = Value;
                                form.AddFields(GetTextArea(_FieldTypeID, _Width, _Height, xdocResponse, _TextAreaValue, form));
                                //                                             pName, pType, pSource
                                //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "multiline", "datasource",Value)); 
                                break;
                            case "5"://NumericTextBox

                                var _NumericTextBoxValue = Value;
                                form.AddFields(GetNumericTextBox(_FieldTypeID, _Width, _Height, xdocResponse, _NumericTextBoxValue, form));
                                //                                             pName, pType, pSource
                                //VariableDefinitions.AppendLine(string.Format(defineNumberFormat, _FieldTypeID.Attribute("Name").Value, "number", "datasource", Value)); 
                                break;
                            // 7 DatePicker
                            case "7"://NumericTextBox

                                var _DatePickerValue = Value;
                                form.AddFields(GetDatePicker(_FieldTypeID, _Width, _Height, xdocResponse, _DatePickerValue, form));
                                //                                             pName, pType, pSource
                                //VariableDefinitions.AppendLine(string.Format(defineNumberFormat, _FieldTypeID.Attribute("Name").Value, "number", "datasource", Value)); 
                                break;
                            case "8": //TimePicker
                                var _timePickerValue = Value;
                                form.AddFields(GetTimePicker(_FieldTypeID, _Width, _Height, xdocResponse, _timePickerValue, form));

                                break;
                            case "10"://CheckBox

                                var _CheckBoxValue = Value;
                                form.AddFields(GetCheckBox(_FieldTypeID, _Width, _Height, xdocResponse, _CheckBoxValue));
                                //                                             pName, pType, pSource
                                //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "checkbox", "datasource",Value)); 
                                break;

                            case "11"://DropDown Yes/No


                                var _DropDownSelectedValueYN = Value;

                                if (_DropDownSelectedValueYN == "1" || _DropDownSelectedValueYN == "true")
                                {
                                    _DropDownSelectedValueYN = "Yes";
                                }

                                if (_DropDownSelectedValueYN == "0" ||  _DropDownSelectedValueYN == "false")
                                {

                                    _DropDownSelectedValueYN = "No";
                                }




                                form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, xdocResponse, _DropDownSelectedValueYN, "Yes&#;No", 11, form));
                                //                                             pName, pType, pSource
                                //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "yesno", "datasource",Value)); 

                                break;
                            case "12"://RadioList
                                //var _GroupBoxValue1 = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("UniqueId").Value);
                                //form.AddFields(GetGroupBox(_FieldTypeID, _Width + 12, _Height, SurveyAnswer, _GroupBoxValue1));
                                var _RadioListSelectedValue1 = Value;
                                string RadioListValues1 = "";
                                RadioListValues1 = _FieldTypeID.Attribute("List").Value;
                                form.AddFields(GetRadioList(_FieldTypeID, _Width, _Height, xdocResponse, _RadioListSelectedValue1, RadioListValues1, form));

                                break;
                            case "17"://DropDown LegalValues

                                string DropDownValues1 = "";
                                DropDownValues1 = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                                var _DropDownSelectedValue1 = Value;
                                form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, xdocResponse, _DropDownSelectedValue1, DropDownValues1, 17, form));
                                //                                             pName, pType, pSource
                                //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "legalvalue", "datasource",Value)); 

                                break;
                            case "18"://DropDown Codes

                                string DropDownValues2 = "";
                                DropDownValues2 = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                                var _DropDownSelectedValue2 = Value;
                                form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, xdocResponse, _DropDownSelectedValue2, DropDownValues2, 18, form));
                                //                                             pName, pType, pSource
                                //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "code", "datasource",Value)); 

                                break;
                            case "19"://DropDown CommentLegal

                                string DropDownValues = "";
                                DropDownValues = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                                var _DropDownSelectedValue = Value;
                                form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, xdocResponse, _DropDownSelectedValue, DropDownValues, 19, form));
                                //                                             pName, pType, pSource
                                //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "commentlegal", "datasource",Value)); 

                                break;
                                
                            case "20"://RelateButton
                                form.AddFields(GetRelateButton(_FieldTypeID, _Width, _Height, xdocResponse, form));
                                break;
                            case "21"://GroupBox
                                //var _GroupBoxValue = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("UniqueId").Value);
                                //form.AddFields(GetGroupBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _GroupBoxValue));

                                form.AddFields(GetGroupBox(_FieldTypeID, _Width, _Height, xdocResponse));

                              

                                break;
                        }
                    }

                }



                //var gender = new RadioList
                //{
                //    DisplayOrder = 30,
                //    Title = "Gender",
                //    Prompt = "Select your gender:",
                //    Required = true,
                //    Orientation = Orientation.Vertical
                //};
                //gender.AddChoices("Male,Female", ",");


                //var sports = new CheckBoxList
                //{
                //    DisplayOrder = 40,
                //    Title = "Favorite Sports",
                //    Prompt = "What are your favorite sports?",
                //    Orientation = Orientation.Horizontal
                //};
                //sports.AddChoices("Baseball,Football,Soccer,Basketball,Tennis,Boxing,Golf", ",");







                form.FormJavaScript = VariableDefinitions.ToString() + "\n" + JavaScript.ToString();
            }

            return form;
        }

        public static double GetHeight(XDocument xdoc)
        {

            try
            {
                if (GetOrientation(xdoc) == "Portrait")
                {
                    var _top = from Node in
                                   xdoc.Descendants("View")
                               select Node.Attribute("Height").Value;

                    return double.Parse(_top.First());
                }
                else
                {

                    var _top = from Node in
                                   xdoc.Descendants("View")
                               select Node.Attribute("Width").Value;

                    return double.Parse(_top.First());

                }

            }
            catch (System.Exception ex)
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
            catch (System.Exception ex)
            {

                return 1024;
            }
        }
        // Orientation="Landscape"
        public static string GetOrientation(XDocument xdoc)
        {

            try
            {

                var Orientation = (from Node in
                                       xdoc.Descendants("View")
                                   select Node.Attribute("Orientation").Value);
                return Orientation.First().ToString();
            }
            catch (System.Exception ex)
            {

                return null;
            }
        }


        public static string GetControlValue(XDocument xdoc, string ControlName)
        {

            string ControlValue = "";

            if (!string.IsNullOrEmpty(xdoc.ToString()))
            {

                //XDocument xdoc = XDocument.Parse(Xml);


                var _ControlValues = from _ControlValue in
                                         xdoc.Descendants("ResponseDetail")
                                     where _ControlValue.Attribute("QuestionName").Value == ControlName.ToString()
                                     select _ControlValue;

                foreach (var _ControlValue in _ControlValues)
                {
                    ControlValue = _ControlValue.Value;
                }
            }

            return ControlValue;
        }

        private static MobileRadioList GetRadioList(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, string RadioListValues, Form form)
        {

            var RadioList = new MobileRadioList();
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
            RadioList.Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");//_FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false;

            RadioList.RequiredMessage = "This field is required";
            RadioList.Key = _FieldTypeID.Attribute("Name").Value;

            RadioList.Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            RadioList.Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            RadioList.PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            RadioList.ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            RadioList.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            RadioList.fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value);
            RadioList.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            RadioList.Required =  GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");//bool.Parse(_FieldTypeID.Attribute("IsRequired").Value);
            RadioList.IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);

            RadioList.Value = _ControlValue;
            RadioList.IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList");
            RadioList.IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList");
            RadioList.IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList");
            RadioList.ShowTextOnRight = bool.Parse(_FieldTypeID.Attribute("ShowTextOnRight").Value);
            RadioList.Choices = Choices;
            RadioList.Width = _Width;
            RadioList.Height = _Height;
            RadioList.Pattern = Pattern;
            RadioList.ChoicesList = ListString;
            // if (RadioList.Pattern[0] == "Vertical")
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

        private static MobileNumericTextBox GetNumericTextBox(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, Form form)
        {

            MobileNumericTextBox MobileNumericTextBox = new MobileNumericTextBox();

            MobileNumericTextBox.Title = _FieldTypeID.Attribute("Name").Value;
                 MobileNumericTextBox.Prompt = _FieldTypeID.Attribute("PromptText").Value;
                 MobileNumericTextBox.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
                 MobileNumericTextBox.Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");//_FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false;
                //RequiredMessage = _FieldTypeID.Attribute("PromptText").Value + " is required",
                 MobileNumericTextBox.RequiredMessage = "This field is required";
                 MobileNumericTextBox.Key = _FieldTypeID.Attribute("Name").Value;
                 MobileNumericTextBox.PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
                 MobileNumericTextBox.PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
                 MobileNumericTextBox.Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
                 MobileNumericTextBox.Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
                 MobileNumericTextBox.PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
                 MobileNumericTextBox.ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
                 MobileNumericTextBox.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
                 MobileNumericTextBox.fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value);
                 MobileNumericTextBox.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
                 MobileNumericTextBox.IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");//bool.Parse(_FieldTypeID.Attribute("IsRequired").Value);
                 MobileNumericTextBox.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
                 MobileNumericTextBox.Lower = _FieldTypeID.Attribute("Lower").Value;
                 MobileNumericTextBox.Upper = _FieldTypeID.Attribute("Upper").Value;
                 MobileNumericTextBox.Value = _ControlValue;
                 MobileNumericTextBox.IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList");
                 MobileNumericTextBox.IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList");
                 MobileNumericTextBox.IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList");
                 MobileNumericTextBox.Pattern = _FieldTypeID.Attribute("Pattern").Value;

            
            return MobileNumericTextBox;

        }
        private static MobileLiteral GetLabel(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer)
        {


            var Label = new MobileLiteral
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
                IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList"),
                Name = _FieldTypeID.Attribute("Name").Value,
                Width = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value)

            };
            return Label;

        }
        private static MobileTextArea GetTextArea(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, Form form)
        {


            var MobileTextArea = new MobileTextArea
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                Required =GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),// _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false,
                //RequiredMessage = _FieldTypeID.Attribute("PromptText").Value + " is required",
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
                IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),//bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
                ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList"),
                IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList"),
                IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList"),
                Value = _ControlValue


            };
            return MobileTextArea;

        }
        private static Hidden GetHiddenField(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue)
        {


            var result = new Hidden
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                //Prompt = _FieldTypeID.Attribute("PromptText").Value,
                //DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                //Required = _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false,
                //RequiredMessage = _FieldTypeID.Attribute("PromptText").Value + " is required",
                //RequiredMessage = "This field is required",
                Key = _FieldTypeID.Attribute("Name").Value,
                //PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                //PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                //Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                //Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                //PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                //ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                //fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                //fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                //fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                //IsRequired = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
                //IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                //MaxLength = int.Parse(_FieldTypeID.Attribute("MaxLength").Value),
                IsPlaceHolder = true,
                //IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList"),
                //IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList"),
                Value = _ControlValue



            };
            return result;

        }


        private static MobileTextBox GetTextBox(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, Form form)
        {


            MobileTextBox TextBox = new MobileTextBox();
           
              TextBox.Title = _FieldTypeID.Attribute("Name").Value;
                  TextBox.Prompt = _FieldTypeID.Attribute("PromptText").Value;
                  TextBox.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
                  TextBox.Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");//_FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false;
                //RequiredMessage = _FieldTypeID.Attribute("PromptText").Value + " is required",
                  TextBox.RequiredMessage = "This field is required";
                  TextBox.Key = _FieldTypeID.Attribute("Name").Value;
                  TextBox.PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
                  TextBox.PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
                  TextBox.Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
                  TextBox.Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
                  TextBox.PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
                  TextBox.ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
                  TextBox.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
                  TextBox.fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value);
                  TextBox.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
                  TextBox.IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");// bool.Parse(_FieldTypeID.Attribute("IsRequired").Value);
                  TextBox.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
                  TextBox.MaxLength = int.Parse(_FieldTypeID.Attribute("MaxLength").Value);
                  TextBox.IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList");
                  TextBox.IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList");
                  TextBox.IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList");
                  TextBox.Value = _ControlValue;



            
            return TextBox;

        }
        private static MobileCheckBox GetCheckBox(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue)
        {


            var MobileCheckBox = new MobileCheckBox
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                RequiredMessage = "This field is required",
                Key = _FieldTypeID.Attribute("Name").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value) + 2,
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value) + 20,
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                //ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = 10,
                Checked = _ControlValue == "Yes" ? true : false,
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList"),
                IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList"),
                IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList"),
                ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value)



            };


            return MobileCheckBox;

        }
        private static MobileDatePicker GetDatePicker(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, Form form)
        {

            var MobileDatePicker = new MobileDatePicker
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                Required =GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),// _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false,
                //RequiredMessage = _FieldTypeID.Attribute("PromptText").Value + " is required",
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
                IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),//bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Lower = _FieldTypeID.Attribute("Lower").Value,
                Upper = _FieldTypeID.Attribute("Upper").Value,
                Value = _ControlValue,
                IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList"),
                IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList"),
                IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList"),
                ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Pattern = _FieldTypeID.Attribute("Pattern").Value

            };
            return MobileDatePicker;

        }

        private static MobileTimePicker GetTimePicker(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, Form form)
        {

            var MobileTimePicker = new MobileTimePicker
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                Required =GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),// _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false,
                //RequiredMessage = _FieldTypeID.Attribute("PromptText").Value + " is required",
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
                IsRequired =GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),// bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Lower = _FieldTypeID.Attribute("Lower").Value,
                Upper = _FieldTypeID.Attribute("Upper").Value,
                Value = _ControlValue,
                IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList"),
                IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList"),
                IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList"),
                ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Pattern = _FieldTypeID.Attribute("Pattern").Value

            };
            return MobileTimePicker;

        }
        private static MobileRelateButton GetRelateButton(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, Form form)
            {


            MobileRelateButton RelateButton = new MobileRelateButton();

            RelateButton.Title = _FieldTypeID.Attribute("Name").Value;
            RelateButton.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            RelateButton.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            RelateButton.RequiredMessage = "This field is required";
            RelateButton.Key = _FieldTypeID.Attribute("Name").Value;
            RelateButton.Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            RelateButton.Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            RelateButton.ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            RelateButton.ControlHeight = _Height * double.Parse(_FieldTypeID.Attribute("ControlHeightPercentage").Value);
            RelateButton.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            RelateButton.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            RelateButton.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            RelateButton.InputFieldfontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value);
            RelateButton.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            //RelateButton.IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value); as is done in FormProvider class.
            RelateButton.IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList");
            RelateButton.IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList");
            RelateButton.IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList");
            RelateButton.RelatedViewId = _FieldTypeID.Attribute("RelatedViewId").Value;
            return RelateButton;
            }



        private static MobileSelect GetDropDown(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, string DropDownValues, int FieldTypeId, Form form)
        {



            var DropDown = new MobileSelect
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),//_FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false,
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
                IsRequired =GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList"),// bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
                ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                ShowEmptyOption = true,
                SelectType = FieldTypeId,
                SelectedValue = _ControlValue,
                IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList"),
                IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList"),
                IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList"),
                ControlFontSize = float.Parse(_FieldTypeID.Attribute("ControlFontSize").Value),
                ControlFontStyle = _FieldTypeID.Attribute("ControlFontStyle").Value,

                EmptyOption = "Select"

            };



            DropDown.AddChoices(DropDownValues, "&#;");

            if (!string.IsNullOrWhiteSpace(_ControlValue))
            {
                DropDown.Choices[_ControlValue] = true;
            }

            return DropDown;
        }

        public static string GetDropDownValues(XDocument xdoc, string ControlName, string TableName)
        {
            StringBuilder DropDownValues = new StringBuilder();


            if (!string.IsNullOrEmpty(xdoc.ToString()))
            {

                //XDocument xdoc = XDocument.Parse(Xml.ToString());


                var _ControlValues = from _ControlValue in
                                         xdoc.Descendants("SourceTable")
                                     where _ControlValue.Attribute("TableName").Value == TableName.ToString()
                                     select _ControlValue;

                foreach (var _ControlValue in _ControlValues)
                {


                    var _SourceTableValues = from _SourceTableValue in _ControlValues.Descendants("Item")

                                             select _SourceTableValue;

                    foreach (var _SourceTableValue in _SourceTableValues)
                    {

                        // DropDownValues.Append(_SourceTableValue.LastAttribute.Value );
                        DropDownValues.Append(_SourceTableValue.FirstAttribute.Value.Trim());
                        DropDownValues.Append("&#;");
                    }
                }
            }

            return DropDownValues.ToString();
        }
        private static MobileGroupBox GetGroupBox(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer)
        {


            var GroupBox = new MobileGroupBox();

            string[] TabIndex= _FieldTypeID.Attribute("TabIndex").Value.Split('.');
           

            GroupBox.FieldWrapper = "div";
            GroupBox.Wrap = true;
            GroupBox.DisplayOrder = int.Parse(TabIndex[0].ToString());
            GroupBox.Html = _FieldTypeID.Attribute("PromptText").Value;
            GroupBox.Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            GroupBox.Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            GroupBox.CssClass = "EpiLabel";
            GroupBox.fontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value);
            GroupBox.fontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            GroupBox.fontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            GroupBox.Height = _Height * double.Parse(_FieldTypeID.Attribute("ControlHeightPercentage").Value);
            GroupBox.IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList");
            GroupBox.Name = _FieldTypeID.Attribute("Name").Value;
            GroupBox.Width = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);


            return GroupBox;
          





        }
        private static int GetNumberOfPages(XDocument Xml)
        {
            var _FieldsTypeIDs = from _FieldTypeID in
                                     Xml.Descendants("View")

                                 select _FieldTypeID;

            return _FieldsTypeIDs.Elements().Count();
        }

        //check if the control should be hidden
        public static bool GetControlState(XDocument xdoc, string ControlName, string ListName)
        {

            bool _Val = false;

            if (!string.IsNullOrEmpty(xdoc.ToString()))
            {
               // XDocument xdoc = XDocument.Parse(Xml);

                if (!string.IsNullOrEmpty(xdoc.Root.Attribute(ListName).Value.ToString()))
                {
                    string List = xdoc.Root.Attribute(ListName).Value;
                    string[] ListArray = List.Split(',');
                    for (var i = 0; i < ListArray.Length; i++)
                    {
                        if (ListArray[i] == ControlName.ToLower())
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
        //get pegeid for xml
        public static string GetPageId(XDocument xdoc, int PageNumber)
        {
           // XDocument xdoc = XDocument.Parse(Xml);
        if (PageNumber== 0)
            {
            PageNumber = 1; 
                
             }
            XElement XElement = xdoc.XPathSelectElement("Template/Project/View/Page[@Position = '" + (PageNumber - 1).ToString() + "']");



            return XElement.Attribute("PageId").Value.ToString();
        }

        public static string GetPageName(XDocument xdoc, int PageNumber)
        {
            //XDocument xdoc = XDocument.Parse(Xml);

        if (PageNumber == 0)
            {
            PageNumber = 1;

            }

            XElement XElement = xdoc.XPathSelectElement("Template/Project/View/Page[@Position = '" + (PageNumber - 1).ToString() + "']");



            return XElement.Attribute("Name").Value.ToString();
        }

        public static string GetFormJavaScript(string CheckCode, Form form, string controlName)
        {// controlName

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

            return B_JavaScript.ToString() + "  " + A_JavaScript.ToString();
        }

        public static string GetPageLevelJS(int PageNumber, Form form, string PageName, string BeforeOrAfter)
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

        public static Dictionary<string, bool> GetChoices(List<string> List)
        {

            Dictionary<string, bool> NewList = new Dictionary<string, bool>();
            foreach (var _List in List)
            {

                NewList.Add(_List, false);

            }

            return NewList;

        }

        public static void UpdateHiddenFields(int CurrentPage, Form form, XDocument xdoc, XDocument xdocResponse, System.Collections.Specialized.NameValueCollection pPostedForm)
        {
            double _Width, _Height;
            _Width = 1024;
            _Height = 768;

            var _FieldsTypeIDs = from _FieldTypeID in
                                     xdoc.Descendants("Field")
                                 where _FieldTypeID.Parent.Attribute("Position").Value != (CurrentPage - 1).ToString()
                                 select _FieldTypeID;

            foreach (var _FieldTypeID in _FieldsTypeIDs)
            {
                bool IsFound = false;
                string Value = null;
                MvcDynamicForms.Fields.Field  field = null;

                foreach (var key in pPostedForm.AllKeys.Where(x => x.StartsWith(form.FieldPrefix)))
                {
                    string fieldKey = key.Remove(0, form.FieldPrefix.Length);

                    if (fieldKey.Equals(_FieldTypeID.Attribute("Name").Value,StringComparison.OrdinalIgnoreCase))
                    {
                        Value = pPostedForm[key];
                        IsFound = true;
                        break;
                    }

                }

                if (IsFound)
                {
                    switch (_FieldTypeID.Attribute("FieldTypeId").Value)
                    {
                        case "1": // textbox

                            var _TextBoxValue = Value;

                            //GetTextBox(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, Form form)
                            field = GetTextBox(_FieldTypeID, _Width, _Height, xdocResponse, _TextBoxValue, form);
                            //                                             pName, pType, pSource
                            //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value,"textbox","datasource",Value)); 
                            break;

                        case "2"://Label/Title
                            field = GetLabel(_FieldTypeID, _Width, _Height, xdocResponse);
                            //                                             pName, pType, pSource
                            //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "lable", "datasource",Value)); 
                            break;
                        case "3"://Label

                            break;
                        case "4"://MultiLineTextBox

                            var _TextAreaValue = Value;
                            field = GetTextArea(_FieldTypeID, _Width, _Height, xdocResponse, _TextAreaValue, form);
                            //                                             pName, pType, pSource
                            //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "multiline", "datasource",Value)); 
                            break;
                        case "5"://NumericTextBox

                            var _NumericTextBoxValue = Value;
                            field = GetNumericTextBox(_FieldTypeID, _Width, _Height, xdocResponse, _NumericTextBoxValue, form);
                            //                                             pName, pType, pSource
                            //VariableDefinitions.AppendLine(string.Format(defineNumberFormat, _FieldTypeID.Attribute("Name").Value, "number", "datasource", Value)); 
                            break;
                        // 7 DatePicker
                        case "7"://NumericTextBox

                            var _DatePickerValue = Value;
                            field = GetDatePicker(_FieldTypeID, _Width, _Height, xdocResponse, _DatePickerValue, form);
                            //                                             pName, pType, pSource
                            //VariableDefinitions.AppendLine(string.Format(defineNumberFormat, _FieldTypeID.Attribute("Name").Value, "number", "datasource", Value)); 
                            break;
                        case "8": //TimePicker
                            var _timePickerValue = Value;
                            field = GetTimePicker(_FieldTypeID, _Width, _Height, xdocResponse, _timePickerValue, form);

                            break;
                        case "10"://CheckBox

                            var _CheckBoxValue = Value;
                            field = GetCheckBox(_FieldTypeID, _Width, _Height, xdocResponse, _CheckBoxValue);
                            //                                             pName, pType, pSource
                            //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "checkbox", "datasource",Value)); 
                            break;

                        case "11"://DropDown Yes/No


                            var _DropDownSelectedValueYN = Value;

                            if (_DropDownSelectedValueYN == "1")
                            {
                                _DropDownSelectedValueYN = "Yes";
                            }

                            if (_DropDownSelectedValueYN == "0")
                            {

                                _DropDownSelectedValueYN = "No";
                            }




                            field = GetDropDown(_FieldTypeID, _Width, _Height, xdocResponse, _DropDownSelectedValueYN, "Yes&#;No", 11, form);
                            //                                             pName, pType, pSource
                            //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "yesno", "datasource",Value)); 

                            break;
                        case "12"://RadioList
                            //var _GroupBoxValue1 = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("UniqueId").Value);
                            //field = GetGroupBox(_FieldTypeID, _Width + 12, _Height, SurveyAnswer, _GroupBoxValue1));
                            var _RadioListSelectedValue1 = Value;
                            string RadioListValues1 = "";
                            RadioListValues1 = _FieldTypeID.Attribute("List").Value;
                            field = GetRadioList(_FieldTypeID, _Width, _Height, xdocResponse, _RadioListSelectedValue1, RadioListValues1, form);

                            break;
                        case "17"://DropDown LegalValues

                            string DropDownValues1 = "";
                            DropDownValues1 = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                            var _DropDownSelectedValue1 = Value;
                            field = GetDropDown(_FieldTypeID, _Width, _Height, xdocResponse, _DropDownSelectedValue1, DropDownValues1, 17, form);
                            //                                             pName, pType, pSource
                            //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "legalvalue", "datasource",Value)); 

                            break;
                        case "18"://DropDown Codes

                            string DropDownValues2 = "";
                            DropDownValues2 = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                            var _DropDownSelectedValue2 = Value;
                            field = GetDropDown(_FieldTypeID, _Width, _Height, xdocResponse, _DropDownSelectedValue2, DropDownValues2, 18, form);
                            //                                             pName, pType, pSource
                            //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "code", "datasource",Value)); 

                            break;
                        case "19"://DropDown CommentLegal

                            string DropDownValues = "";
                            DropDownValues = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                            var _DropDownSelectedValue = Value;
                            field = GetDropDown(_FieldTypeID, _Width, _Height, xdocResponse, _DropDownSelectedValue, DropDownValues, 19, form);
                            //                                             pName, pType, pSource
                            //VariableDefinitions.AppendLine(string.Format(defineFormat, _FieldTypeID.Attribute("Name").Value, "commentlegal", "datasource",Value)); 

                            break;
                        case "21"://GroupBox
                            //var _GroupBoxValue = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("UniqueId").Value);
                            //field = GetGroupBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _GroupBoxValue));

                            field = GetGroupBox(_FieldTypeID, _Width, _Height, xdocResponse);
                            break;
                    }

                    if (field != null)
                    {
                        field.IsPlaceHolder = true;
                        form.AddFields(field);
                    }
                }

            }

        }

        public Epi.Core.EnterInterpreter.Rule_Context GetRelateCheckCodeObj(List<Epi.Web.Enter.Common.Helper.RelatedFormsObj> Obj, string FormCheckCode)
            {
            Epi.Core.EnterInterpreter.EpiInterpreterParser EIP = new Epi.Core.EnterInterpreter.EpiInterpreterParser(Epi.Core.EnterInterpreter.EpiInterpreterParser.GetEnterCompiledGrammarTable());
            Epi.Core.EnterInterpreter.Rule_Context result = (Epi.Core.EnterInterpreter.Rule_Context)EIP.Context;
            foreach (var item in Obj)
                {
                result.LoadTemplate(item.MetaData, item.Response);
                }
            EIP.Execute(FormCheckCode);

            return result;
            }
        private static List<Epi.Web.Enter.Common.Helper.RelatedFormsObj> GetRelateFormObj()
            {

            List<Epi.Web.Enter.Common.Helper.RelatedFormsObj> List = new List<Enter.Common.Helper.RelatedFormsObj>();


            for (int i = 0; SurveyAnswerList.Count() > i; i++)
                {


                Epi.Web.Enter.Common.Helper.RelatedFormsObj RelatedFormsObj = new Epi.Web.Enter.Common.Helper.RelatedFormsObj();
                XDocument xdocResponse1 = XDocument.Parse(SurveyAnswerList[i].XML);
                XDocument xdoc1 = XDocument.Parse(SurveyInfoList[i].XML.ToString());
                RelatedFormsObj.MetaData = xdoc1;
                RelatedFormsObj.Response = xdocResponse1;


                List.Add(RelatedFormsObj);
                }

            return List;
            }

        }
}
