using System.Linq;
using MvcDynamicForms.Fields;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using MvcDynamicForms;
 


namespace MvcDynamicForms.Demo.Models
{
    public static class FormProvider
    {
        public static Form GetForm(object SurveyMetaData ,int PageNumber)
        {
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
                        case "1"://TextBox
                            var name = new TextBox
                            {
                                Title = "Name",
                                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                                DisplayOrder = 20,
                                Required = true,
                                RequiredMessage = "Your full name is required",
                                Key = _FieldTypeID.Attribute("Name").Value,
                                //,
                                 Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value)
                            };
                            form.AddFields(name);
                            break;

                        case "2"://Label/Title

                            var Label = new Literal
                            {
                                FieldWrapper = "div",
                                Wrap = true,
                                DisplayOrder = 10,
                                Html = _FieldTypeID.Attribute("PromptText").Value,
                                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                                CssClass =   "EpiLabel",
                                fontSize = double.Parse(_FieldTypeID.Attribute("ControlFontSize").Value),
                                fontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value,
                                fontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value,
                                Height = _Height * double.Parse(_FieldTypeID.Attribute("ControlHeightPercentage").Value),
                                Width = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value)

                            };
                            form.AddFields(Label);
                            break;
                        case "3"://Label

                            break;
                    }

                }






                //var name = new TextBox
                //{
                //    Title = "Name",
                //    Prompt = "Enter your full name:",
                //    DisplayOrder = 20,
                //    Required = true,
                //    RequiredMessage = "Your full name is required",
                //    Key = "1233"
                //};


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

                //var states = new Select
                //{
                //    DisplayOrder = 50,
                //    Title = "Visited States",
                //    MultipleSelection = true,
                //    Size = 10,
                //    Prompt = "What US states have you visited? (Use the ctrl key to select multiple states.)"
                //};
                //states.AddChoices("Alabama,Alaska,Arizona,Arkansas,California,Colorado,Connecticut,Delaware,Florida,Georgia,Hawaii,Idaho,Illinois,Indiana,Iowa,Kansas,Kentucky,Louisiana,Maine,Maryland,Massachusetts,Michigan,Minnesota,Mississippi,Missouri,Montana,Nebraska,Nevada,New Hampshire,New Jersey,New Mexico,New York,North Carolina,North Dakota,Ohio,Oklahoma,Oregon,Pennsylvania,Rhode Island,South Carolina,South Dakota,Tennessee,Texas,Utah,Vermont,Virginia,Washington,West Virginia,Wisconsin,Wyoming", ",");

                //var bio = new TextArea
                //{
                //    DisplayOrder = 60,
                //    Title = "Bio",
                //    Prompt = "Describe yourself:"
                //};
                //bio.InputHtmlAttributes.Add("cols", "40");
                //bio.InputHtmlAttributes.Add("rows", "6");

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



                // create form and add fields to it
                //var form = new Form();
                ////form.AddFields(description, name, Age, gender, email, sports, states, bio, month, agree );
                //form.AddFields(description);
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
       
    }
}
