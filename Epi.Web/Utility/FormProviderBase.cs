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
using System.Drawing;
using System.Globalization;
using System.Web.Mvc;

namespace Epi.Web.MVC.Utility
{
    public class FormProviderBase
    {
       
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
                            case "RequiredFieldsList":
                                RequiredFieldsList = reader.Value.Split(new char[] { ',' }).ToList<string>();
                                break;
                        }
                    }
                }
                else if (reader.Name == "ResponseDetail")
                {
                    if (reader.IsEmptyElement == false && reader.NodeType != XmlNodeType.EndElement)
                    {
                        string fieldName = reader.GetAttribute("QuestionName");
                        reader.MoveToContent();
                        string fieldValue = reader.ReadString();
                        fieldValues.Add(fieldName.ToLower(), fieldValue);
                    }
                }
            }

            foreach (string fieldName in HiddenFieldsList)
            {
                if (form.Fields.Keys.Contains(fieldName.ToLower()))
                {
                    Field field = form.Fields[fieldName];

                    field.IsHidden = true;

                    if (field is RadioList)
                    {
                        string fieldNameCandidate = field.Name + "groupbox";

                        if (form.Fields.Keys.Contains(fieldNameCandidate.ToLower()))
                        {
                            form.Fields[fieldNameCandidate.ToLower()].IsHidden = true;
                        }
                    }
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
                    form.Fields[fieldName.ToLower()].IsDisabled = true;
                }
            }

            foreach (string fieldName in RequiredFieldsList)
            {
                if (form.Fields.Keys.Contains(fieldName.ToLower()))
                {
                    ((InputField)form.Fields[fieldName]).Required = true;
                }
            }

            foreach (KeyValuePair<string, string> kvp in fieldValues)
            {
                var NewList = form.Fields.Keys.ToList().ConvertAll(d => d.ToLower());
                if (NewList.Contains(kvp.Key))
                {
                    Field field = form.Fields[kvp.Key];

                    if (field is RadioList)
                    {
                        var Choices = ((MvcDynamicForms.Fields.ListField)(field)).ChoiceKeyValuePairs.ToList();
                        int Index = 0;
                        int.TryParse(kvp.Value.ToString(), out Index);


                        for (int i = 0; Choices.Count() > i; i++)
                        {
                            if (Choices[i].Key == kvp.Value.ToString())
                            {
                                Index = i;

                            }

                        }
                        //((RadioList)field).Response = kvp.Value;
                        //((RadioList)field).SelectedValue = kvp.Value; 

                    ((RadioList)field).Response = Choices[Index].Key;
                        ((RadioList)field).SelectedValue = Choices[Index].Key;
                    }
                    else if (field is ListField)
                    {
                        ((ListField)field).Response = kvp.Value;
                        ((Select)field).SelectedValue = ((ListField)field).Response;
                    }
                    else if (field is NumericTextBox)
                    {
                        string uiSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
                        string Value = kvp.Value;
                        if (uiSep == ".")
                        {
                            Value = Value.Replace(",", ".");
                        }
                        else
                        {
                            Value = Value.Replace(".", ",");
                        }

                            ((NumericTextBox)field).Response = Value;

                    }                  
                    else if (field is InputField)
                    {
                        ((InputField)field).Response = kvp.Value;
                    }

                    if (field is CheckBox)
                    {
                        ((CheckBox)field).Checked = ((InputField)field).Response == "Yes" ? true : ((InputField)field).Response == "true" ? true : false;
                    }
                }
            }
        }

        protected static double GetHeight(XDocument xdoc) 
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

        protected static double GetWidth(XDocument xdoc)
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

        protected static string GetOrientation(XDocument xdoc)
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

        protected static RadioList GetRadioList(XElement _FieldTypeID, int fieldTypeId, Form form)
        {
            InputField field;

            if (form.IsMobile)
            {
                field = new MobileRadioList();
            }
            else
            {
                field = new RadioList();
            }

            field.Title = _FieldTypeID.Attribute("Name").Value;
            field.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.Required = _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false;

            field.RequiredMessage = "This field is required";
            field.Key = _FieldTypeID.Attribute("Name").Value;

            field.Top = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.PromptWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;

            field.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            field.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            field.InputFieldfontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;

            field.Required = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value);
            field.Width = form.Width;
            field.Height = form.Height;
            field.Name = _FieldTypeID.Attribute("Name").Value;
            field.FieldTypeId = fieldTypeId;
            
            ((RadioList)field).ShowTextOnRight = bool.Parse(_FieldTypeID.Attribute("ShowTextOnRight").Value);
            ((RadioList)field).Options_Positions = _FieldTypeID.Attribute("List").Value; ;

            if (form.Height > form.Width)
            {
                ((RadioList)field).Orientation = (Orientation)0;
            }
            else
            {
                ((RadioList)field).Orientation = (Orientation)1;
            }

            SetFieldCommon(field, form);
            
            return (RadioList)field;
        }

        protected static NumericTextBox GetNumericTextBox(XElement _FieldTypeID, Form form)
        {
            InputField field;

            if (form.IsMobile)
            {
                field = new MobileNumericTextBox();
            }
            else
            {
                field = new NumericTextBox();
            }
            
            field.Title = _FieldTypeID.Attribute("Name").Value;
            field.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.RequiredMessage =  "This field is required";
            field.Key = _FieldTypeID.Attribute("Name").Value;
            field.PromptTop = form.Height * ParseDouble(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            field.PromptLeft = form.Width * ParseDouble(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            field.Top = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.PromptWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            field.InputFieldfontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            field.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            field.Required = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value);
            field.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);

            ((NumericTextBox)field).Lower = _FieldTypeID.Attribute("Lower").Value;
            ((NumericTextBox)field).Upper = _FieldTypeID.Attribute("Upper").Value;
            ((NumericTextBox)field).Pattern = _FieldTypeID.Attribute("Pattern").Value;

            return (NumericTextBox)field;
        }

        protected static Literal GetLabel(XElement _FieldTypeID, Form form)
        {
            Field field;

            if (form.IsMobile)
            {
                field = new MobileLiteral();
            }
            else
            {
                field = new Literal();
            }

            field.FieldWrapper = "div";
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.Top = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.CssClass = "EpiLabel";
            field.fontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            field.fontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            field.Height = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlHeightPercentage").Value);
            field.Name =_FieldTypeID.Attribute("Name").Value;
            field.Width = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);

            ((Literal)field).Html = _FieldTypeID.Attribute("PromptText").Value;
            ((Literal)field).Wrap = true;
            
            return (Literal)field;
        }

        protected static TextArea GetTextArea(XElement _FieldTypeID, Form form)
        {
            InputField field;

            if (form.IsMobile)
            {
                field = new MobileTextArea();
            }
            else
            {
                field = new TextArea();
            }

            field.Title = _FieldTypeID.Attribute("Name").Value;
            field.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.RequiredMessage = "This field is required";
            field.Key = _FieldTypeID.Attribute("Name").Value;
            field.PromptTop = form.Height * ParseDouble(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            field.PromptLeft = form.Width * ParseDouble(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            field.Top = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.PromptWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlHeight = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlHeightPercentage").Value);
            field.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            field.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            field.InputFieldfontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            field.Required = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value);
            field.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);

            return (TextArea)field;
        }

        protected static TextBox GetTextBox(XElement _FieldTypeID, Form form)
        {
            InputField field;

            if (form.IsMobile)
            {
                field = new MobileTextBox();
            }
            else
            {
                field = new TextBox();
            }
            
            field.Title = _FieldTypeID.Attribute("Name").Value;
            field.Prompt = _FieldTypeID.Attribute("PromptText").Value.Trim();
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.RequiredMessage = "This field is required";
            field.Key = _FieldTypeID.Attribute("Name").Value;
            field.PromptTop = form.Height * ParseDouble(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            field.PromptLeft = form.Width * ParseDouble(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            field.Top = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.PromptWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            field.Required = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value);
            field.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            field.InputFieldfontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            field.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            if (!string.IsNullOrEmpty(_FieldTypeID.Attribute("MaxLength").Value))
            {
            field.MaxLength = int.Parse(_FieldTypeID.Attribute("MaxLength").Value);
            }

            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);

            return (TextBox)field;
        }

        protected static UpperCaseTextBox GetUpperCaseTextBox(XElement _FieldTypeID, Form form)
        {
            InputField field;

            if (form.IsMobile)
            {
                field = new MobileUpperCaseTextBox();
            }
            else
            {
                field = new UpperCaseTextBox();
            }

            field.Title = _FieldTypeID.Attribute("Name").Value;
            field.Prompt = _FieldTypeID.Attribute("PromptText").Value.Trim();
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.RequiredMessage = "This field is required";
            field.Key = _FieldTypeID.Attribute("Name").Value;
            field.PromptTop = form.Height * ParseDouble(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            field.PromptLeft = form.Width * ParseDouble(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            field.Top = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.PromptWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            field.Required = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value);
            field.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            field.InputFieldfontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            field.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            if (!string.IsNullOrEmpty(_FieldTypeID.Attribute("MaxLength").Value))
            {
                field.MaxLength = int.Parse(_FieldTypeID.Attribute("MaxLength").Value);
            }          
            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);

            return (UpperCaseTextBox)field;
        }
        protected static CheckBox GetCheckBox(XElement _FieldTypeID, Form form)
        {
            InputField field;

            if (form.IsMobile)
            {
                field = new MobileCheckBox();
            }
            else
            {
                field = new CheckBox();
            }

            field.Title = _FieldTypeID.Attribute("Name").Value;
            field.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.RequiredMessage = "This field is required";
            field.Key = _FieldTypeID.Attribute("Name").Value;
            field.PromptTop = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value) + 2;
            field.PromptLeft = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value) + 20;
            field.Top = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.PromptWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlWidth = 10;
            field.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            field.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);

            return (CheckBox)field;
        }

        protected static CommandButton GetCommandButton(XElement fieldElement, Form form)
        {
            InputField field;

            if (form.IsMobile)
            {
                field = new MobileCommandButton();
            }
            else
            {
                field = new CommandButton();
            }
            
            field.Title = fieldElement.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(fieldElement.Attribute("TabIndex").Value);
            field.Prompt = fieldElement.Attribute("PromptText").Value;
            field.Key = fieldElement.Attribute("Name").Value;
            field.Name = fieldElement.Attribute("Name").Value;
            field.Top = form.Height * ParseDouble(fieldElement.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(fieldElement.Attribute("ControlLeftPositionPercentage").Value);
            field.Width = form.Width * ParseDouble(fieldElement.Attribute("ControlWidthPercentage").Value);
            field.Height = form.Height * ParseDouble(fieldElement.Attribute("ControlHeightPercentage").Value);
            field.fontstyle = fieldElement.Attribute("ControlFontStyle").Value;
            field.fontSize = ParseDouble(fieldElement.Attribute("ControlFontSize").Value);
            field.fontfamily = fieldElement.Attribute("ControlFontFamily").Value;
            SetFieldCommon(field, form);
            
            return (CommandButton)field;
        }

        //protected static CommandButton GetRelateButton(XElement fieldElement, Form form)
        //{
        //    InputField field;

        //    if (form.IsMobile)
        //    {
        //        field = new MobileCommandButton();
        //    }
        //    else
        //    {
        //        field = new CommandButton();
        //    }

        //    field.Title = fieldElement.Attribute("PromptText").Value;
        //    field.DisplayOrder = int.Parse(fieldElement.Attribute("TabIndex").Value);
        //    field.Prompt = fieldElement.Attribute("PromptText").Value;
        //    field.Key = fieldElement.Attribute("Name").Value;
        //    field.Name = fieldElement.Attribute("Name").Value;
        //    field.Top = form.Height * ParseDouble(fieldElement.Attribute("ControlTopPositionPercentage").Value);
        //    field.Left = form.Width * ParseDouble(fieldElement.Attribute("ControlLeftPositionPercentage").Value);
        //    field.Width = form.Width * ParseDouble(fieldElement.Attribute("ControlWidthPercentage").Value);
        //    field.Height = form.Height * ParseDouble(fieldElement.Attribute("ControlHeightPercentage").Value);
        //    field.fontstyle = fieldElement.Attribute("ControlFontStyle").Value;
        //    field.fontSize = ParseDouble(fieldElement.Attribute("ControlFontSize").Value);
        //    field.fontfamily = fieldElement.Attribute("ControlFontFamily").Value;
        //    SetFieldCommon(field, form);

        //    return (CommandButton)field;
        //}
        protected static RelateButton GetRelateButton(XElement fieldElement, Form form)
        {
            InputField field = new RelateButton(); 
 

            field.Title = fieldElement.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(fieldElement.Attribute("TabIndex").Value);
            field.Prompt = fieldElement.Attribute("PromptText").Value;
            field.Key = fieldElement.Attribute("Name").Value;
            field.Name = fieldElement.Attribute("Name").Value;
            field.Top = form.Height * ParseDouble(fieldElement.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(fieldElement.Attribute("ControlLeftPositionPercentage").Value);
            field.ControlWidth = form.Width * ParseDouble(fieldElement.Attribute("ControlWidthPercentage").Value);
            field.ControlHeight = form.Height * ParseDouble(fieldElement.Attribute("ControlHeightPercentage").Value);
            field.fontstyle = fieldElement.Attribute("ControlFontStyle").Value;
            field.fontSize = ParseDouble(fieldElement.Attribute("ControlFontSize").Value);
            field.fontfamily = fieldElement.Attribute("ControlFontFamily").Value;
             
                ((RelateButton)field).RelatedViewId = fieldElement.Attribute("RelatedViewId").Value;
            
            SetFieldCommon(field, form);

            return (RelateButton)field;
        }
        protected static MobileRelateButton GetMobileRelateButton(XElement fieldElement, Form form)
        {
            InputField field = new MobileRelateButton();

            

            field.Title = fieldElement.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(fieldElement.Attribute("TabIndex").Value);
            field.Prompt = fieldElement.Attribute("PromptText").Value;
            field.Key = fieldElement.Attribute("Name").Value;
            field.Name = fieldElement.Attribute("Name").Value;
            field.Top = form.Height * ParseDouble(fieldElement.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(fieldElement.Attribute("ControlLeftPositionPercentage").Value);
            field.ControlWidth = form.Width * ParseDouble(fieldElement.Attribute("ControlWidthPercentage").Value);
            field.ControlHeight = form.Height * ParseDouble(fieldElement.Attribute("ControlHeightPercentage").Value);
            field.fontstyle = fieldElement.Attribute("ControlFontStyle").Value;
            field.fontSize = ParseDouble(fieldElement.Attribute("ControlFontSize").Value);
            field.fontfamily = fieldElement.Attribute("ControlFontFamily").Value;
            
             ((MobileRelateButton)field).RelatedViewId = fieldElement.Attribute("RelatedViewId").Value;
            
            SetFieldCommon(field, form);

            return (MobileRelateButton)field;
        }
        protected static DatePicker GetDatePicker(XElement _FieldTypeID, Form form)
        {
            InputField field;

            if (form.IsMobile)
            {
                field = new MobileDatePicker();
            }
            else
            {
                field = new DatePicker();
            }

            field.Title = _FieldTypeID.Attribute("Name").Value;
            field.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.RequiredMessage = "This field is required";
            field.Key = _FieldTypeID.Attribute("Name").Value;
            field.PromptTop = form.Height * ParseDouble(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            field.PromptLeft = form.Width * ParseDouble(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            field.Top = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.PromptWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            field.Required = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value);
            field.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            field.InputFieldfontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            field.ReadOnly= bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);

            ((DatePicker)field).Lower = _FieldTypeID.Attribute("Lower").Value;
            ((DatePicker)field).Upper = _FieldTypeID.Attribute("Upper").Value;
            ((DatePicker)field).Pattern = _FieldTypeID.Attribute("Pattern").Value;
            
            return (DatePicker)field;
        }

        protected static TimePicker GetTimePicker(XElement _FieldTypeID, Form form)
        {
            InputField field;

            if (form.IsMobile)
            {
                field = new MobileTimePicker();
            }
            else
            {
                field = new TimePicker();
            }

            field.Title = _FieldTypeID.Attribute("Name").Value;
            field.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.RequiredMessage = "This field is required";
            field.Key = _FieldTypeID.Attribute("Name").Value;
            field.PromptTop = form.Height * ParseDouble(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            field.PromptLeft = form.Width * ParseDouble(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            field.Top = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.PromptWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            field.Required = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value);
            field.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            field.InputFieldfontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            field.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            field.Name = _FieldTypeID.Attribute("Name").Value;
            SetFieldCommon(field, form);

            ((TimePicker)field).Lower = _FieldTypeID.Attribute("Lower").Value;
            ((TimePicker)field).Upper = _FieldTypeID.Attribute("Upper").Value;
            ((TimePicker)field).Pattern = _FieldTypeID.Attribute("Pattern").Value;

            return (TimePicker)field;
        }

        protected static Select GetDropDown(XElement _FieldTypeID, string DropDownValues, int FieldTypeId, Form form)
        {
            InputField field;
            
            if (form.IsMobile)
            {
                field = new MobileSelect();
            }
            else
            {
                field = new Select();
            }
            
            field.Name = _FieldTypeID.Attribute("Name").Value;
            field.Title = _FieldTypeID.Attribute("Name").Value;
            field.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            field.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            field.RequiredMessage = "This field is required";
            field.Key = _FieldTypeID.Attribute("Name").Value;
            field.PromptTop = form.Height * ParseDouble(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            field.PromptLeft = form.Width * ParseDouble(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            field.Top = form.Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            field.Left = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            field.PromptWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.ControlWidth = form.Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            field.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            field.Required = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value);
            field.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            field.InputFieldfontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            field.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            field.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            field.Name = _FieldTypeID.Attribute("Name").Value;
            field.FieldTypeId = FieldTypeId;
            field.IsAndroidfield = form.IsAndroid;
            field.TextColumnName = _FieldTypeID.Attribute("TextColumnName").Value;
            field.FieldRelateCondition = _FieldTypeID.Attribute("RelateCondition").Value;
            field.SourceTable = _FieldTypeID.Attribute("SourceTableName").Value;
            if (!string.IsNullOrEmpty(_FieldTypeID.Attribute("Sort").Value))
            {
                field.Sort = bool.Parse(_FieldTypeID.Attribute("Sort").Value);
            }
            else { 
               field.Sort = false;
            }
            SetFieldCommon(field, form);

            ((Select)field).ShowEmptyOption = true;
            ((Select)field).ControlFontSize = ParseFloat(_FieldTypeID.Attribute("ControlFontSize").Value);
            ((Select)field).ControlFontStyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            if (FieldTypeId == 18)
            {
                ((Select)field).RelateCondition = true;
            }
            else {

                ((Select)field).RelateCondition = false;
            }
            ((Select)field).EmptyOption = "Select";
            ((Select)field).AddChoices(DropDownValues, "&#;", field.Sort);

            return (Select)field;
        }
        private static Select GetDropDown(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, string DropDownValues, int FieldTypeId, Form form)
        {
            Select DropDown;

            if (form.IsMobile)
            {
                DropDown = new MobileSelect();
            }
            else
            {
                DropDown = new Select();
            }
            DropDown.Name = _FieldTypeID.Attribute("Name").Value;
            DropDown.Title = _FieldTypeID.Attribute("Name").Value;
            DropDown.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            DropDown.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            // DropDown.Required = _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false;
            DropDown.RequiredMessage = "This field is required";
            DropDown.Key = _FieldTypeID.Attribute("Name").Value;
            DropDown.PromptTop = _Height * ParseDouble(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            DropDown.PromptLeft = _Width * ParseDouble(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            DropDown.Top = _Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            DropDown.Left = _Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            DropDown.PromptWidth = _Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            DropDown.ControlWidth = _Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            DropDown.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            DropDown.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            DropDown.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            //IsRequired = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
            DropDown.IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");
            DropDown.Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");
            DropDown.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            DropDown.InputFieldfontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            DropDown.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            DropDown.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            DropDown.ShowEmptyOption = true;
            DropDown.SelectType = FieldTypeId;
            DropDown.SelectedValue = _ControlValue;
            DropDown.IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList");
            DropDown.IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList");
            DropDown.IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList");
            DropDown.ControlFontSize = ParseFloat(_FieldTypeID.Attribute("ControlFontSize").Value);
            DropDown.ControlFontStyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            // DropDown.RelateCondition = bool.Parse(_FieldTypeID.Attribute("RelateCondition").Value);
            //  DropDown.RelateCondition = !string.IsNullOrEmpty(_FieldTypeID.Attribute("RelateCondition").Value) ? bool.Parse(_FieldTypeID.Attribute("RelateCondition").Value) : false;
            DropDown.EmptyOption = "Select";
            DropDown.FieldTypeId = FieldTypeId;
            if (FieldTypeId == 18)
            {
                DropDown.RelateCondition = true;
            }
            else
            {

                DropDown.RelateCondition = false;
            }
            DropDown.EmptyOption = "Select";
            DropDown.AddChoices(DropDownValues, "&#;", DropDown.Sort);

            return DropDown;



            //DropDown.AddChoices(DropDownValues, "&#;");

            //if (!string.IsNullOrWhiteSpace(_ControlValue) && DropDown.ChoiceKeyValuePairs.Any(s => s.Key == _ControlValue))
            //{
            //    DropDown.ChoiceKeyValuePairs[_ControlValue] = true;
            //}

            //return DropDown;
        }

        public static string GetDropDownValues(XDocument xdoc, string ControlName, string TableName, string CodeColumnName, string RelateCondition = "")
        {
            StringBuilder DropDownValues = new StringBuilder();




            if (!string.IsNullOrEmpty(xdoc.ToString()))
            {

                // XDocument xdoc = XDocument.Parse(Xml.ToString());


                var _ControlValues = from _ControlValue in
                                         xdoc.Descendants("SourceTable")
                                     where _ControlValue.Attribute("TableName").Value == TableName.ToString()
                                     select _ControlValue;

                foreach (var _ControlValue in _ControlValues)
                {


                    var _SourceTableValues = from _SourceTableValue in _ControlValues.Descendants("Item")

                                             select _SourceTableValue;

                    var ScriptRelateCondition = new TagBuilder("script");
                    foreach (var _SourceTableValue in _SourceTableValues)
                    {

                        // DropDownValues.Append(_SourceTableValue.LastAttribute.Value );
                        if (!string.IsNullOrEmpty(CodeColumnName))
                        {
                            string Xelement = _SourceTableValue.ToString();

                            XElement NewXElement = XElement.Parse(Xelement);
                            if (NewXElement.Attribute(CodeColumnName) != null)
                            {
                                DropDownValues.Append(NewXElement.Attribute(CodeColumnName).Value.Trim());

                            }
                            else
                            {

                                DropDownValues.Append(_SourceTableValue.Attributes().FirstOrDefault().Value.Trim());

                            }

                        }
                        else
                        {
                            DropDownValues.Append(_SourceTableValue.Attributes().FirstOrDefault().Value.Trim());


                        }
                        DropDownValues.Append("&#;");
                    }
                }
            }

            return DropDownValues.ToString();
        }


        protected static void SetFieldCommon(Field field, Form form)
        {
            field.FunctionObjectAfter = (EnterRule)form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + field.Key);
            field.FunctionObjectBefore = (EnterRule)form.FormCheckCodeObj.GetCommand("level=field&event=before&identifier=" + field.Key);
            field.FunctionObjectClick = (EnterRule)form.FormCheckCodeObj.GetCommand("level=field&event=click&identifier=" + field.Key);
        }

        private static string GetDropDownValues(XDocument xdoc, string ControlName, string TableName, string CodeColumnName, List<SourceTableDTO> SourceTable = null)
        {
            StringBuilder DropDownValues = new StringBuilder();

            if (!string.IsNullOrEmpty(xdoc.ToString()))
            {
                var _ControlValues = from _ControlValue in xdoc.Descendants("SourceTable")
                                     where _ControlValue.Attribute("TableName").Value == TableName.ToString()
                                     select _ControlValue;
                if (_ControlValues.Count()==0)
                {
                     if ( SourceTable!= null && SourceTable.Count() > 0)
                                {
                                    var SourceTableXml1 = SourceTable.Where(x => x.TableName == TableName.ToString()).Select(y => y.TableXml).ToList() ;
                        if (SourceTableXml1.Count() > 0)
                        {
                            XDocument SourceTableXml = XDocument.Parse(SourceTableXml1[0].ToString());
                            _ControlValues = from _ControlValue in SourceTableXml.Descendants("SourceTable")
                                                 // where _ControlValue.Attribute("TableName").Value == TableName.ToString()
                                             select _ControlValue;
                        }
                        else {
                            var SourceTableXml2 = SourceTable.Where(x => x.TableName == TableName.ToLower().ToString()).Select(y => y.TableXml).ToList();

                            if (SourceTableXml2.Count() > 0)
                            {
                                XDocument SourceTableXml = XDocument.Parse(SourceTableXml2[0].ToString());
                                _ControlValues = from _ControlValue in SourceTableXml.Descendants("SourceTable")
                                                      
                                                 select _ControlValue;
                            }

                        }
                                }
                
                }
                foreach (var _ControlValue in _ControlValues)
                {
                    var _SourceTableValues = from _SourceTableValue in _ControlValues.Descendants("Item")
                                             
                                             select _SourceTableValue;

                    foreach (var _SourceTableValue in _SourceTableValues)
                    {
                        if (!string.IsNullOrEmpty(CodeColumnName))
                        {
                            //XElement XElement =  XElement.Parse(_SourceTableValue.ToString().ToLower());
                            XElement XElement = XElement.Parse(_SourceTableValue.ToString());
                           try{
                           // DropDownValues.Append(XElement.Attribute(CodeColumnName.ToLower()).Value.Trim());
                            //DropDownValues.Append(XElement.Attribute(CodeColumnName.ToLower()).Value.Trim());
                                DropDownValues.Append(XElement.Attribute(CodeColumnName).Value.Trim());
                            }
                           catch (Exception ex)
                             {
                             DropDownValues.Append(_SourceTableValue.Attributes().FirstOrDefault().Value.Trim());
                            }
                        }
                        else
                        {
                               DropDownValues.Append(_SourceTableValue.Attributes().FirstOrDefault().Value.Trim());
                        
                        
                        }
                        DropDownValues.Append("&#;");
                    }
                }
            }

            return DropDownValues.ToString();
        }

        protected static GroupBox GetGroupBox(XElement fieldTypeID, Form form)
        {
            InputField field;

            if (form.IsMobile)
            {
                field = new MobileGroupBox();
            }
            else
            {
                field = new GroupBox();
            }

            field.fontstyle = fieldTypeID.Attribute("ControlFontStyle").Value;
            field.fontSize = ParseDouble(fieldTypeID.Attribute("ControlFontSize").Value);
            field.fontfamily = fieldTypeID.Attribute("ControlFontFamily").Value;
            if (fieldTypeID.Attribute("BackgroundColor") != null)
            {
                var color = Color.FromArgb((int)(int.Parse(fieldTypeID.Attribute("BackgroundColor").Value)) + unchecked((int)0xFF000000));
               string HexValue = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
               field.BackgroundColor = HexValue;
            }
            AssignCommonGroupProperties(fieldTypeID, form.Width, form.Height, field as GroupBox, form);
            return (GroupBox)field;
        }

        protected static GroupBox GetOptionGroupBox(XElement fieldTypeID, Form form)
        {
            InputField field;

            if (form.IsMobile)
            {
                field = new MobileGroupBox();
            }
            else
            {
                field = new GroupBox();
            }

            field.fontstyle = fieldTypeID.Attribute("PromptFontStyle").Value;
            field.fontSize = ParseDouble(fieldTypeID.Attribute("PromptFontSize").Value);
            field.fontfamily = fieldTypeID.Attribute("PromptFontFamily").Value;
            AssignCommonGroupProperties(fieldTypeID, form.Width + 12, form.Height, field as GroupBox, form);
            if (fieldTypeID.Attribute("BackgroundColor") != null)
            {
                var color = Color.FromArgb((int)(int.Parse(fieldTypeID.Attribute("BackgroundColor").Value)) + unchecked((int)0xFF000000));
                string HexValue = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
                field.BackgroundColor = HexValue;
            }
            return (GroupBox)field;
        }

        protected static void AssignCommonGroupProperties(XElement fieldTypeID, double _Width, double _Height, GroupBox groupBox, Form form)
        {
            groupBox.DisplayOrder = ParseDouble(fieldTypeID.Attribute("TabIndex").Value);
            groupBox.Html = fieldTypeID.Attribute("PromptText").Value;
            groupBox.Name = fieldTypeID.Attribute("Name").Value;
            groupBox.Title = fieldTypeID.Attribute("Name").Value;
            groupBox.Prompt = fieldTypeID.Attribute("PromptText").Value;
            groupBox.RequiredMessage = "This field is required";
            groupBox.Key = fieldTypeID.Attribute("Name").Value + "_GroupBox";
            groupBox.PromptTop = form.Height * ParseDouble(fieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            groupBox.PromptLeft = form.Width * ParseDouble(fieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            groupBox.Top = form.Height * ParseDouble(fieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            groupBox.Left = form.Width * ParseDouble(fieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            groupBox.ControlHeight = form.Height * ParseDouble(fieldTypeID.Attribute("ControlHeightPercentage").Value) - 12;
            groupBox.ControlWidth = form.Width * ParseDouble(fieldTypeID.Attribute("ControlWidthPercentage").Value) - 12;
         
                
           
            SetFieldCommon(groupBox, form);
        }

        protected static int GetNumberOfPages(XDocument Xml)
        {
            var _FieldsTypeIDs = from _FieldTypeID in Xml.Descendants("View") select  _FieldTypeID;
            return _FieldsTypeIDs.Elements().Count() ;
        }

        protected static bool GetControlState(XDocument xdoc, string ControlName, string ListName)
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
                  //  JavaScript.AppendLine("$('#myform').submit(function (e) {");
                     JavaScript.AppendLine("$(\"[href]\").click(function(e) {");
                     JavaScript.AppendLine("page" + PageNumber + "_after();  e.preventDefault(); })");

                     JavaScript.AppendLine("$(\"#ContinueButton\").click(function(e) {");
                     JavaScript.AppendLine("page" + PageNumber + "_after();  e.preventDefault(); })");

                     JavaScript.AppendLine("$(\"#PreviousButton\").click(function(e) {");
                     JavaScript.AppendLine("page" + PageNumber + "_after();  e.preventDefault(); })");
                    

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
            var fieldElementList = from fieldElements in xdocMetadata.Descendants("Field")
                                   where fieldElements.Parent.Attribute("Position").Value != (currentPage - 1).ToString()
                                   select fieldElements;
            double _Width, _Height;
            _Width = GetWidth(xdocMetadata);
            _Height = GetHeight(xdocMetadata);

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
                            field = GetTextBox(fieldElement, form);
                            break;
                        case "3":
                            field = GetUpperCaseTextBox(fieldElement, form);
                            break;

                        case "2":
                            field = GetLabel(fieldElement, form);
                            break;

                        case "4":
                            field = GetTextArea(fieldElement, form);
                            break;

                        case "5":
                            field = GetNumericTextBox(fieldElement, form);
                            break;

                        case "7":
                            field = GetDatePicker(fieldElement, form);
                            break;

                        case "8":
                            field = GetTimePicker(fieldElement, form);
                            break;

                        case "10":
                            field = GetCheckBox(fieldElement, form);
                            break;

                        case "11": 
                            field = GetDropDown(fieldElement, "Yes&#;No", 11, form);
                            break;

                        case "12": 
                            field = GetRadioList(fieldElement, 12, form);
                            break;                       

                        case "17":
                            string legalValues = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value, fieldElement.Attribute("CodeColumnName").Value, form.SourceTableList);
                            field = GetDropDown(fieldElement, legalValues, 17, form);
                           
                            ((Select)field).SelectedValue = value.Trim(new char[]{','});
                            break;

                        case "18":
                            string TableName2 = fieldElement.Attribute("SourceTableName").Value;
                            string codes = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value, fieldElement.Attribute("CodeColumnName").Value, form.SourceTableList);
                            field = GetDropDown(fieldElement, codes, 18, form);
                            ((Select)field).SelectedValue = value.Trim(new char[] { ',' });
                            string DropDownValues2 = "";
                            var _TextBoxValue1 = value;
                            if (form.SourceTableList != null && form.SourceTableList.Count() > 0)
                            {
                                var SourceTableXml2 = form.SourceTableList.Where(x => x.TableName == TableName2).Select(y => y.TableXml).ToList(); ;
                                DropDownValues2 = GetDropDownValues(XDocument.Parse(SourceTableXml2[0].ToString()), fieldElement.Attribute("Name").Value, TableName2, fieldElement.Attribute("TextColumnName").Value, fieldElement.Attribute("RelateCondition").Value);
                            }
                            var _DropDownSelectedValue2 = value;
                            var Dropdown = GetDropDown(fieldElement, _Width, _Height, xdocResponse, _DropDownSelectedValue2, DropDownValues2, 18, form);
                            if (Dropdown.ChoiceKeyValuePairs.Count() > 50)
                            {

                                form.AddFields(GetAutoComplete(fieldElement, _Width, _Height, xdocResponse, _DropDownSelectedValue2, DropDownValues2, 1, form));
                            }
                            else
                            {
                                form.AddFields(Dropdown);
                            }
                            break;

                        case "19":
                            string commentLegal = GetDropDownValues(xdocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value, fieldElement.Attribute("CodeColumnName").Value,form.SourceTableList);
                            field = GetDropDown(fieldElement, commentLegal, 19, form);
                            ((Select)field).SelectedValue = value.Trim(new char[] { ',' });
                            break;

                        case "20":
                            if (!form.IsMobile) {
                                field = GetRelateButton(fieldElement, form);
                            }
                            else {
                                field = GetMobileRelateButton(fieldElement, form);
                            }
                            break;

                        case "21": 
                            field = GetGroupBox(fieldElement, form);
                            break;
                    }

                    if (field != null)
                    {
                        field.IsPlaceHolder = true;
                        form.Fields.Add(field.Name.ToLower(), field);
                    }
                }
            }
        }

        protected static void AddFormFields(int pageNumber, Form form, SurveyAnswerDTO _SurveyAnswer, List<SourceTableDTO> SourceTableList = null, bool isMobile = false)
        {
            string XML = form.SurveyInfo.XML;
            string dropDownValues = "";
            form.Fields.Clear();
            XDocument xdoc = XDocument.Parse(XML);



            double _Width, _Height;
            _Width = GetWidth(xdoc);
            _Height = GetHeight(xdoc);

            XDocument xdocResponse = XDocument.Parse(_SurveyAnswer.XML);

            foreach (var fieldElement in form.FieldsTypeIDs)
            {
                var Value = GetControlValue(xdocResponse, fieldElement.Attribute("Name").Value);
                //JavaScript.Append(GetFormJavaScript(checkcode, form, _FieldTypeID.Attribute("Name").Value));

                if (fieldElement.Attribute("Position").Value == (pageNumber - 1).ToString())
                {
                    MvcDynamicForms.Fields.Field field = null;

                    switch (fieldElement.Attribute("FieldTypeId").Value)
                    {
                        case "1":
                            field = GetTextBox(fieldElement, form);
                            break;
                        case "3":
                            field = GetUpperCaseTextBox(fieldElement, form);
                            break;

                        case "2":
                            field = GetLabel(fieldElement, form);
                            break;

                        case "4":
                            field = GetTextArea(fieldElement, form);
                            break;

                        case "5":
                            field = GetNumericTextBox(fieldElement, form);
                            break;

                        case "7":
                            field = GetDatePicker(fieldElement, form);
                            break;

                        case "8":
                            field = GetTimePicker(fieldElement, form);
                            break;

                        case "10":
                            field = GetCheckBox(fieldElement, form);
                            break;

                        case "11":
                            field = GetDropDown(fieldElement, "Yes&#;No", 11, form);
                            break;

                        case "12":
                            if (form.IsMobile == false)
                            {
                                GroupBox optionsContainer = GetOptionGroupBox(fieldElement, form);
                                form.Fields.Add(optionsContainer.Name.ToLower() + "groupbox", optionsContainer);
                            }
                            field = GetRadioList(fieldElement, 12, form);
                            break;

                        case "13":
                            field = GetCommandButton(fieldElement, form);
                            break;                      

                        case "17"://DropDown LegalValues

                            string DropDownValues1 = "";
                            if (SourceTableList != null && SourceTableList.Count() > 0)
                            {
                                var SourceTableXml1 = SourceTableList.Where(x => x.TableName == fieldElement.Attribute("SourceTableName").Value).Select(y => y.TableXml).ToList();

                                DropDownValues1 = GetDropDownValues(XDocument.Parse(SourceTableXml1[0].ToString()), fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value, fieldElement.Attribute("CodeColumnName").Value, SourceTableList);
                            }
                            else {
                                DropDownValues1 = GetDropDownValues(form.XDocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value, fieldElement.Attribute("CodeColumnName").Value, form.SourceTableList);
                                 
                            }
                            var _DropDownSelectedValue1 = Value;
                            form.AddFields(GetDropDown(fieldElement, _Width, _Height, xdocResponse, _DropDownSelectedValue1, DropDownValues1, 17, form));

                            break;

                        case "18": //DropDown Codes


                            dropDownValues = GetDropDownValues(form.XDocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value, fieldElement.Attribute("TextColumnName").Value, form.SourceTableList);
                            var dropdown = GetDropDown(fieldElement, dropDownValues, 18, form);
                            if ((isMobile && dropdown.ChoiceKeyValuePairs.Count > 15) || (!isMobile && dropdown.ChoiceKeyValuePairs.Count > 50))
                            {
                              
                                if (isMobile)
                                {
                                    field = GetMobileAutoComplete(fieldElement, _Width, _Height, xdocResponse, Value, dropDownValues, 1, form);
                                }
                                else
                                {
                                    field = GetAutoComplete(fieldElement, _Width, _Height, xdocResponse, Value, dropDownValues, 1, form);
                                }
                            }
                            else
                            {
                                field = dropdown;
                            }

                            break;

                        case "19": //DropDown CommentLegal

                            dropDownValues = GetDropDownValues(form.XDocMetadata, fieldElement.Attribute("Name").Value, fieldElement.Attribute("SourceTableName").Value, fieldElement.Attribute("CodeColumnName").Value, form.SourceTableList);
                            field = GetDropDown(fieldElement, dropDownValues, 19, form);
                            break;

                        case "20":
                            if (!isMobile)
                            {
                                field = GetRelateButton(fieldElement, form);
                            }
                            else {
                                field = GetMobileRelateButton(fieldElement, form);
                            }
                            break;

                        case "21":
                            field = GetGroupBox(fieldElement, form);
                            break;
                    }

                    if (field != null && !string.IsNullOrEmpty(field.Name))
                    {

                        form.AddFields(field);
                    }


                }
            }
        }
        private static string GetControlValue(XDocument xdoc, string ControlName)
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
      

        private static bool GetRequiredControlState(string Requiredlist, string ControlName, string ListName)
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

        private static MobileAutoComplete GetMobileAutoComplete(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, string DropDownValues, int FieldTypeId, Form form)
        {



            MobileAutoComplete DropDown = new MobileAutoComplete();
            DropDown.Name = _FieldTypeID.Attribute("Name").Value;
            DropDown.Title = _FieldTypeID.Attribute("Name").Value;
            DropDown.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            DropDown.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            // DropDown.Required = _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false;
            DropDown.RequiredMessage = "This field is required";
            DropDown.Key = _FieldTypeID.Attribute("Name").Value;
            DropDown.PromptTop = _Height * ParseDouble(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            DropDown.PromptLeft = _Width * ParseDouble(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            DropDown.Top = _Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            DropDown.Left = _Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            DropDown.PromptWidth = _Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            DropDown.ControlWidth = _Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            DropDown.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            DropDown.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            DropDown.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            //IsRequired = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
            DropDown.IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");
            DropDown.Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");
            DropDown.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            DropDown.InputFieldfontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            DropDown.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            DropDown.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            DropDown.ShowEmptyOption = true;
            DropDown.SelectType = FieldTypeId;
            DropDown.SelectedValue = _ControlValue;
            DropDown.IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList");
            DropDown.IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList");
            DropDown.IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList");
            DropDown.ControlFontSize = ParseFloat(_FieldTypeID.Attribute("ControlFontSize").Value);
            DropDown.ControlFontStyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            DropDown.RelateCondition = _FieldTypeID.Attribute("RelateCondition").Value;
            DropDown.EmptyOption = "Select";
            DropDown.FieldTypeId = FieldTypeId;

            DropDown.Value = _ControlValue;
            DropDown.Response = _ControlValue;

            DropDown.AddChoices(DropDownValues, "&#;");

            if (!string.IsNullOrWhiteSpace(_ControlValue))
            {
                DropDown.Choices[_ControlValue] = true;
            }

            return DropDown;
        }
        private static AutoComplete GetAutoComplete(XElement _FieldTypeID, double _Width, double _Height, XDocument SurveyAnswer, string _ControlValue, string DropDownValues, int FieldTypeId, Form form)
        {



            AutoComplete DropDown = new AutoComplete();
            DropDown.Name = _FieldTypeID.Attribute("Name").Value;
            DropDown.Title = _FieldTypeID.Attribute("Name").Value;
            DropDown.Prompt = _FieldTypeID.Attribute("PromptText").Value;
            DropDown.DisplayOrder = int.Parse(_FieldTypeID.Attribute("TabIndex").Value);
            // DropDown.Required = _FieldTypeID.Attribute("IsRequired").Value == "True" ? true : false;
            DropDown.RequiredMessage = "This field is required";
            DropDown.Key = _FieldTypeID.Attribute("Name").Value;
            DropDown.PromptTop = _Height * ParseDouble(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value);
            DropDown.PromptLeft = _Width * ParseDouble(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value);
            DropDown.Top = _Height * ParseDouble(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value);
            DropDown.Left = _Width * ParseDouble(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value);
            DropDown.PromptWidth = _Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            DropDown.ControlWidth = _Width * ParseDouble(_FieldTypeID.Attribute("ControlWidthPercentage").Value);
            DropDown.fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value;
            DropDown.fontSize = ParseDouble(_FieldTypeID.Attribute("PromptFontSize").Value);
            DropDown.fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value;
            //IsRequired = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
            DropDown.IsRequired = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");
            DropDown.Required = GetRequiredControlState(form.RequiredFieldsList.ToString(), _FieldTypeID.Attribute("Name").Value, "RequiredFieldsList");
            DropDown.InputFieldfontstyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            DropDown.InputFieldfontSize = ParseDouble(_FieldTypeID.Attribute("ControlFontSize").Value);
            DropDown.InputFieldfontfamily = _FieldTypeID.Attribute("ControlFontFamily").Value;
            DropDown.ReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value);
            DropDown.ShowEmptyOption = true;
            DropDown.SelectType = FieldTypeId;
            DropDown.SelectedValue = _ControlValue;
            DropDown.IsHidden = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HiddenFieldsList");
            DropDown.IsHighlighted = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "HighlightedFieldsList");
            DropDown.IsDisabled = GetControlState(SurveyAnswer, _FieldTypeID.Attribute("Name").Value, "DisabledFieldsList");
            DropDown.ControlFontSize = ParseFloat(_FieldTypeID.Attribute("ControlFontSize").Value);
            DropDown.ControlFontStyle = _FieldTypeID.Attribute("ControlFontStyle").Value;
            DropDown.RelateCondition = _FieldTypeID.Attribute("RelateCondition").Value;
            DropDown.EmptyOption = "Select";
            DropDown.FieldTypeId = FieldTypeId;

            DropDown.Value = _ControlValue;
            DropDown.Response = _ControlValue;

            DropDown.AddChoices(DropDownValues, "&#;");

            if (!string.IsNullOrWhiteSpace(_ControlValue))
            {
                DropDown.Choices[_ControlValue] = true;
            }

            return DropDown;
        }

        public static double ParseDouble(string ParseString)
        {
            double result;

            try
            {

                if (ParseString.Contains(','))
                {
                    var NewValue = ParseString.Replace(',', '.');
                    //  double.TryParse(NewValue, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result);
                    // result = Math.Ceiling(result);
                    double.TryParse(NewValue, System.Globalization.NumberStyles.Any, CultureInfo.CreateSpecificCulture("en-US"), out result);

                }
                else
                {
                    double.TryParse(ParseString, System.Globalization.NumberStyles.Any, CultureInfo.CreateSpecificCulture("en-US"), out result);
                    //    result = Math.Ceiling(result);


                }

                // double.TryParse(ParseString, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            }
            catch (Exception ex)
            {
                double.TryParse(ParseString, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            }
            return result;
        }

        public static float ParseFloat(string ParseString)
        {
            float result;

            try
            {

                if (ParseString.Contains(','))
                {
                    var NewValue = ParseString.Replace(',', '.');
                    float.TryParse(NewValue, System.Globalization.NumberStyles.Any, CultureInfo.CreateSpecificCulture("en-US"), out result);

                }
                else
                {
                    float.TryParse(ParseString, System.Globalization.NumberStyles.Any, CultureInfo.CreateSpecificCulture("en-US"), out result);
                   
                }                
            }
            catch (Exception ex)
            {
                float.TryParse(ParseString, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            }
            return result;
        }                  
    }
}
