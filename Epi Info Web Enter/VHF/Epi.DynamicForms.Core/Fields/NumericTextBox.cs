using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html textbox input element.
    /// </summary>
    [Serializable]
    public class NumericTextBox : NumericTextField
    {
        public override string RenderHtml()
        { 
            var html = new StringBuilder();
            var inputName = _form.FieldPrefix + _key;
            string ErrorStyle = string.Empty;
            // prompt label
            var prompt = new TagBuilder("label");
            
            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("Id", "label" + inputName);
            //  prompt.Attributes.Add("class", _promptClass);
            prompt.Attributes.Add("class", "EpiLabel");

            StringBuilder StyleValues = new StringBuilder();

            //StyleValues.Append(GetContolStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), _PromptWidth.ToString(), Height.ToString()));
            StyleValues.Append(GetContolStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), IsHidden));
            prompt.Attributes.Add("style", StyleValues.ToString());
            html.Append(prompt.ToString());

            // error label
            if (!IsValid)
            {
                //Add new Error to the error Obj
                 ErrorStyle = ";border-color: red";
            }
            
            // input element
            var txt = new TagBuilder("input");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "text");


            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";
            if (_IsHidden)
            {
                IsHiddenStyle = "display:none";
            }
            if (_IsHighlighted)
            {
                IsHighlightedStyle = "background-color:yellow";
            }
             
            //if (_IsDisabled)
            //{
            //    txt.Attributes.Add("disabled", "disabled");
            //}
            
            // txt.Attributes.Add("value", Value);
            ////////////Check code start//////////////////
            EnterRule FunctionObjectAfter = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + _key);
            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            { 
                txt.Attributes.Add("onblur", "return " + _key + "_after();"); //After
            }
            EnterRule FunctionObjectBefore = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=before&identifier=" + _key);
            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            { 
                txt.Attributes.Add("onfocus", "return " + _key + "_before();"); //Before
            }

            ////////////Check code end//////////////////
            txt.Attributes.Add("value",Value);
            txt.Attributes.Add("class", GetControlClass());
            txt.Attributes.Add("data-prompt-position", "topRight:15");
            txt.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
            txt.MergeAttributes(_inputHtmlAttributes);
            html.Append(txt.ToString(TagRenderMode.SelfClosing));

            //adding numeric text box validation jquery script tag
            var scriptNumeric = new TagBuilder("script");
            scriptNumeric.InnerHtml = "$(function() { $('#" + inputName + "').numeric();});";
            html.Append(scriptNumeric.ToString(TagRenderMode.Normal));

            //if masked input not empty appy the pattern jquery plugin
            if (!string.IsNullOrEmpty(Pattern))
            {
                string maskedPatternEq = GetMaskedPattern(Pattern);
                var scriptMaskedInput = new TagBuilder("script");
                scriptMaskedInput.InnerHtml = "$(function() { $('#" + inputName + "').mask('"+maskedPatternEq+"');});";
                html.Append(scriptMaskedInput.ToString(TagRenderMode.Normal));
            }
            // If readonly then add the following jquery script to make the field disabled 
            if (ReadOnly || _IsDisabled)
                {
                var scriptReadOnlyText = new TagBuilder("script");
                //scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                scriptReadOnlyText.InnerHtml = "$(function(){  var List = new Array();List.push('" + _key + "');CCE_Disable(List, false);});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
                }

            //prevent numeric text box control to submit on enter click
            var scriptBuilder = new TagBuilder("script");
            scriptBuilder.InnerHtml = "$('#" + inputName + "').BlockEnter('" + inputName + "');";
            scriptBuilder.ToString(TagRenderMode.Normal);
            html.Append(scriptBuilder.ToString(TagRenderMode.Normal));

            var wrapper = new TagBuilder(_fieldWrapper);
            wrapper.Attributes["class"] = _fieldWrapperClass;
            if (_IsHidden)
            {
                wrapper.Attributes["style"] = "display:none";

            }
            wrapper.Attributes["id"] = inputName + "_fieldWrapper";
            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
        }

        private string GetMaskedPattern(string pattern)
        {
            string maskedPattern = string.Empty;
            switch (pattern)
            {
                case "#":
                    maskedPattern = "9";
                    break;
                case "##":
                    maskedPattern = "99";
                    break;
                case "###":
                    maskedPattern = "999";
                    break;
                case "####":
                    maskedPattern = "9999" ;
                    break;
                case "##.##":
                    maskedPattern = "99.99";
                    break;
                case "##.###":
                    maskedPattern = "99.999";
                    break;
                   
            }
            return maskedPattern;
        }
        public string GetControlClass() {

            StringBuilder ControlClass = new StringBuilder();

            ControlClass.Append("validate[");

            if ((!string.IsNullOrEmpty(Lower)) && (!string.IsNullOrEmpty(Upper)))
            {

                ControlClass.Append("min[" + Lower + "],max[" + Upper + "],");
            }
            if (_IsRequired == true)
            {

                ControlClass.Append("required");

            }
            ControlClass.Append("]");

            return ControlClass.ToString();
        
        }
    }
}
