using System.Linq;
using MvcDynamicForms.Fields;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using MvcDynamicForms;
using System.Collections.Generic;
using System;
namespace Epi.Web.MVC.Utility
{
    public static class FormProvider
    {
       
        public static Form GetForm(object SurveyMetaData ,int PageNumber, Epi.Web.Common.DTO.SurveyAnswerDTO _SurveyAnswer)
        {
            string SurveyAnswer;
            
            if ( _SurveyAnswer != null)
            {
                SurveyAnswer = _SurveyAnswer.XML;

            }
            else { SurveyAnswer = ""; }

            var form = new Form();
            

            form.SurveyInfo = (Epi.Web.Common.DTO.SurveyInfoDTO)(SurveyMetaData);
            
            string XML = form.SurveyInfo.XML;


            form.CurrentPage = PageNumber;
            if (string.IsNullOrEmpty(XML))
            {

                form.NumberOfPages = 1;
            }
            else
            {
                form.NumberOfPages = GetNumberOfPages(XDocument.Parse(XML));
            }
            if (string.IsNullOrEmpty(XML))
            {
                // no XML what to do?
            }
            else
            {
                XDocument xdoc = XDocument.Parse(XML);
                
  
                var _FieldsTypeIDs = from _FieldTypeID in
                                         xdoc.Descendants("Field")
                                     where _FieldTypeID.Attribute("Position").Value == (PageNumber - 1).ToString()
                                     select _FieldTypeID;


                double _Width, _Height;
                _Width = GetWidth(xdoc);
                _Height= GetHeight(xdoc);

                form.Width = _Width;
                form.Height = _Height;
                
                foreach (var _FieldTypeID in _FieldsTypeIDs)
                {
                    switch (_FieldTypeID.Attribute("FieldTypeId").Value)
                    {
                        case "1":
                            var _TextBoxValue= GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("Name").Value);
                            form.AddFields( GetTextBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _TextBoxValue));
                            break;
                       

                        case "2"://Label/Title
                             form.AddFields( GetLabel(_FieldTypeID,_Width,_Height ));
                            break;
                        case "3"://Label

                            break;
                        case "4"://MultiLineTextBox

                            var _TextAreaValue = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("Name").Value);
                            form.AddFields( GetTextArea(_FieldTypeID, _Width,_Height,SurveyAnswer,_TextAreaValue));
                            break;
                        case "5"://NumericTextBox

                            var _NumericTextBoxValue = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("Name").Value);
                            form.AddFields( GetNumericTextBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _NumericTextBoxValue));
                            break;
                        // 7 DatePicker
                        case "7"://NumericTextBox

                            var _DatePickerValue = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("Name").Value);
                            form.AddFields(GetDatePicker(_FieldTypeID, _Width, _Height, SurveyAnswer, _DatePickerValue));
                            break;
                        case "10"://CheckBox

                            var _CheckBoxValue = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("Name").Value);
                            form.AddFields(GetCheckBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _CheckBoxValue));
                            break;

                        case "11"://DropDown Yes/No

                            var _DropDownSelectedValueYN = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("Name").Value);
                            form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, SurveyAnswer, _DropDownSelectedValueYN, "Yes,No",11));
                            break;
                        case "17"://DropDown LegalValues
                            string DropDownValues1 = "";
                            DropDownValues1 = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                            var _DropDownSelectedValue1 = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("Name").Value);
                            form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, SurveyAnswer, _DropDownSelectedValue1, DropDownValues1,17));
                            break;
                        case "18"://DropDown Codes
                            string DropDownValues2 = "";
                            DropDownValues2 = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                            var _DropDownSelectedValue2 = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("Name").Value);
                            form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, SurveyAnswer, _DropDownSelectedValue2, DropDownValues2,18));
                            break;
                        case "19"://DropDown CommentLegal
                            string DropDownValues = "";
                            DropDownValues = GetDropDownValues(xdoc, _FieldTypeID.Attribute("Name").Value, _FieldTypeID.Attribute("SourceTableName").Value);
                            var _DropDownSelectedValue = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("Name").Value);
                            form.AddFields(GetDropDown(_FieldTypeID, _Width, _Height, SurveyAnswer, _DropDownSelectedValue, DropDownValues,19));
                            break;
                        case "21"://GroupBox
                            var _GroupBoxValue = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("UniqueId").Value);
                            form.AddFields(GetGroupBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _GroupBoxValue));
                            break;
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

                





            
            }

            return form;
        }

        public static double GetHeight(XDocument xdoc) 
        
        {
             
            try
            {
                var _top = from Node in
                               xdoc.Descendants("View")
                           select Node.Attribute("Height").Value;

                 return double.Parse(_top.First());

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
                
                var _left = (from Node in
                                 xdoc.Descendants("View")
                             select Node.Attribute("Width").Value);
                return double.Parse(_left.First());
            }
            catch (System.Exception ex)
            {
                 
                return  1024;
            }
        }

        public  static string GetControlValue( string Xml,string ControlName ) {

            string ControlValue = "";

            if (!string.IsNullOrEmpty(Xml))
            {

                XDocument xdoc = XDocument.Parse(Xml);


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
        private static NumericTextBox GetNumericTextBox(XElement _FieldTypeID, double _Width, double _Height, string SurveyAnswer, string _ControlValue)
        {

            var NumericTextBox = new NumericTextBox
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                Required =  _FieldTypeID.Attribute("IsRequired").Value == "True"?true:false ,
                //RequiredMessage = _FieldTypeID.Attribute("PromptText").Value + " is required",
                RequiredMessage =  "This field is required",
                Key = _FieldTypeID.Attribute("UniqueId").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                IsRequired = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Lower = _FieldTypeID.Attribute("Lower").Value,
                Upper = _FieldTypeID.Attribute("Upper").Value,
                Value = _ControlValue,
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
                Width = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value)

            };
            return Label;

        }
        private static TextArea GetTextArea(XElement _FieldTypeID, double _Width, double _Height, string SurveyAnswer, string _ControlValue)
        {


            var TextArea = new TextArea
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                Required = _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false,
                //RequiredMessage = _FieldTypeID.Attribute("PromptText").Value + " is required",
                RequiredMessage = "This field is required",
                Key = _FieldTypeID.Attribute("UniqueId").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlHeight = _Width * double.Parse(_FieldTypeID.Attribute("ControlHeightPercentage").Value),
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                IsRequired = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Value = _ControlValue


            };
            return TextArea;

        }
        private static TextBox GetTextBox(XElement _FieldTypeID, double _Width, double _Height, string SurveyAnswer, string _ControlValue)
        {


            var TextBox = new TextBox
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                Required = _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false,
                //RequiredMessage = _FieldTypeID.Attribute("PromptText").Value + " is required",
                RequiredMessage = "This field is required",
                Key = _FieldTypeID.Attribute("UniqueId").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                IsRequired = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                MaxLength = int.Parse(_FieldTypeID.Attribute("MaxLength").Value),
                Value = _ControlValue
                


            };
            return TextBox;

        }
        private static CheckBox GetCheckBox(XElement _FieldTypeID, double _Width, double _Height, string SurveyAnswer, string _ControlValue)
        {


            var CheckBox = new CheckBox
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                RequiredMessage = "This field is required",
                Key = _FieldTypeID.Attribute("UniqueId").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value)+2,
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value)+20,
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                 PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                 //ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = 10,
                Checked = _ControlValue =="Yes"?true:false,
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value) 
                 
              
      
            };

 
            return CheckBox;
        
        }
        private static DatePicker GetDatePicker(XElement _FieldTypeID, double _Width, double _Height, string SurveyAnswer, string _ControlValue)
        {

            var DatePicker = new DatePicker
            {
                Title = _FieldTypeID.Attribute("Name").Value,
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                Required = _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false,
                //RequiredMessage = _FieldTypeID.Attribute("PromptText").Value + " is required",
                RequiredMessage = "This field is required",
                Key = _FieldTypeID.Attribute("UniqueId").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                IsRequired = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Lower = _FieldTypeID.Attribute("Lower").Value,
                Upper = _FieldTypeID.Attribute("Upper").Value,
                Value = _ControlValue, 
                Pattern = _FieldTypeID.Attribute("Pattern").Value

            };
            return DatePicker;

        }
        private static Select GetDropDown(XElement _FieldTypeID, double _Width, double _Height, string SurveyAnswer, string _ControlValue, string DropDownValues, int FieldTypeId)
        {
                        
            
            
            var DropDown = new Select
                {
                    Title = _FieldTypeID.Attribute("Name").Value,
                    Prompt = _FieldTypeID.Attribute("PromptText").Value,
                    DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value),
                    Required = _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false,
                    RequiredMessage = "This field is required",
                    Key = _FieldTypeID.Attribute("UniqueId").Value,
                    PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                    PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                    Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                    Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                    PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                    ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                    fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                    fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                    fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                    IsRequired = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
                    IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                    ShowEmptyOption = true,
                    SelectType=FieldTypeId,
                    SelectedValue = _ControlValue,
                    EmptyOption = "Select"
                };
            DropDown.AddChoices(DropDownValues, ",");

            return DropDown;
        }
         
        public static   string  GetDropDownValues(XDocument Xml, string ControlName,string TableName)
        {
            StringBuilder DropDownValues = new StringBuilder();
           

            if (!string.IsNullOrEmpty(Xml.ToString()))
            {

                XDocument xdoc = XDocument.Parse(Xml.ToString());


                var _ControlValues = from _ControlValue in
                                         xdoc.Descendants("SourceTable")
                                     where _ControlValue.Attribute("TableName").Value == TableName.ToString()
                                     select _ControlValue;

                foreach (var _ControlValue in _ControlValues)
                {


                    var _SourceTableValues = from _SourceTableValue in  _ControlValues.Descendants("Item")
                                           
                                             select _SourceTableValue;

                    foreach (var _SourceTableValue in _SourceTableValues)
                    {

                       // DropDownValues.Append(_SourceTableValue.LastAttribute.Value );
                        DropDownValues.Append(_SourceTableValue.FirstAttribute.Value);
                        DropDownValues.Append(",");
                    }
                }
            }

            return DropDownValues.ToString();
        }
        private static GroupBox GetGroupBox(XElement _FieldTypeID, double _Width, double _Height, string SurveyAnswer, string _ControlValue)
        {


            var GroupBox = new GroupBox();
               
                    GroupBox.Title = _FieldTypeID.Attribute("Name").Value;
                    GroupBox.Prompt = _FieldTypeID.Attribute("PromptText").Value;
                    GroupBox.RequiredMessage = "This field is required";
                    GroupBox.Key = _FieldTypeID.Attribute("UniqueId").Value;
                    GroupBox.PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value) ;
                    GroupBox.PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value) ;
                    GroupBox.Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
                    GroupBox.Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
                    //GroupBox.PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
                    GroupBox.ControlHeight = _Height * double.Parse(_FieldTypeID.Attribute("ControlHeightPercentage").Value)-12;
                    GroupBox.ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value)-12;
                    GroupBox.fontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
                    GroupBox.fontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value);
                    GroupBox.fontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
                    GroupBox.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);



                
                return GroupBox;
           
           

           

        }
        private static int GetNumberOfPages(XDocument Xml)
        {
            var _FieldsTypeIDs = from _FieldTypeID in
                                     Xml.Descendants("View")
                                
                                 select  _FieldTypeID;

            return _FieldsTypeIDs.Elements().Count() ;
        }
    }
}
