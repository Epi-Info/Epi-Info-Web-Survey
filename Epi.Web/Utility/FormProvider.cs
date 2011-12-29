using System.Linq;
using MvcDynamicForms.Fields;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using MvcDynamicForms;
 


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
                foreach (var _FieldTypeID in _FieldsTypeIDs)
                {
                    switch (_FieldTypeID.Attribute("FieldTypeId").Value)
                    {
                        case "1":
                            var _TextBoxValue= GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("UniqueId").Value);
                            form.AddFields( GetTextBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _TextBoxValue));
                            break;
                       

                        case "2"://Label/Title
                             form.AddFields( GetLabel(_FieldTypeID,_Width,_Height ));
                            break;
                        case "3"://Label

                            break;
                        case "4"://MultiLineTextBox
                              
                            var _TextAreaValue= GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("UniqueId").Value);
                            form.AddFields( GetTextArea(_FieldTypeID, _Width,_Height,SurveyAnswer,_TextAreaValue));
                            break;
                        case "5"://NumericTextBox
                            
                            
                            var _NumericTextBoxValue = GetControlValue(SurveyAnswer, _FieldTypeID.Attribute("UniqueId").Value);
                            form.AddFields( GetNumericTextBox(_FieldTypeID, _Width, _Height, SurveyAnswer, _NumericTextBoxValue));
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

                

                //var month = new Select
                //{
                //    DisplayOrder = 70,
                //    Title = "Month Born",
                //    Prompt = "What month were you born in?",
                //    ShowEmptyOption = true,
                //    EmptyOption = "- Select One - "
                //};
                //month.AddChoices("January,February,March,April,May,June,July,August,September,October,November,December", ",");

                //var agree = new CheckBox
                //{
                //    DisplayOrder = 80,
                //    Title = "Agrees To Terms",
                //    Prompt = "I agree to all of the terms in the EULA.",
                //    Required = true,
                //    RequiredMessage = "You must agree to the EULA!"
                //};



            
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
                return 400;
                
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
                 
                return  800;
            }
        }

        public  static string GetControlValue( string Xml,string ControlId ) {

            string ControlValue = "";

            if (!string.IsNullOrEmpty(Xml))
            {

                XDocument xdoc = XDocument.Parse(Xml);


                var _ControlValues = from _ControlValue in
                                         xdoc.Descendants("ResponseDetail")
                                     where _ControlValue.Attribute("QuestionId").Value == ControlId.ToString()
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
                Title = "Name",
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = 20,
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
                Value = _ControlValue
                
            };
            return NumericTextBox;

        }
        private static Literal GetLabel(XElement _FieldTypeID, double _Width, double _Height)
        {


            var Label = new Literal
            {
                FieldWrapper = "div",
                Wrap = true,
                DisplayOrder = 10,
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
                Title = "Name",
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = 20,
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
                Title = "Name",
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = 20,
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
                Value = _ControlValue
                


            };
            return TextBox;

        }

       
    }
}
