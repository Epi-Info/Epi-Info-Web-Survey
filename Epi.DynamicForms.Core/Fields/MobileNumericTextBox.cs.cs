using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
using System.Globalization;

namespace MvcDynamicForms.Fields
{
    [Serializable]
    public class MobileNumericTextBox : NumericTextBox 
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder();
            var inputName = _fieldPrefix + _key;
            string ErrorStyle = string.Empty;

            var prompt = new TagBuilder("label");
            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("Id", "label" + inputName);
            prompt.Attributes.Add("class", "EpiLabel");

            //StringBuilder StyleValues = new StringBuilder();
            //StyleValues.Append(GetControlStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), _IsHidden));
           // prompt.Attributes.Add("style", StyleValues.ToString());
            html.Append(prompt.ToString());

            if (!IsValid)
            {
                ErrorStyle = ";border-color: red";
            }

            var txt = new TagBuilder("input");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "text");           
         
            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";

            if (_IsHighlighted)
            {
                IsHighlightedStyle = "background-color:yellow";
            }

            //if (_IsDisabled)
            //{
            //    txt.Attributes.Add("disabled", "disabled");
            //}

            if (ReadOnly || _IsDisabled)
                {
                var scriptReadOnlyText = new TagBuilder("script");
                //scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                scriptReadOnlyText.InnerHtml = "$(function(){  var List = new Array();List.push('" + _key + "');CCE_Disable(List, false);});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
                }


            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {
                txt.Attributes.Add("onblur", "return " + _key + "_after(this.id);"); //After
            }

            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {
                txt.Attributes.Add("onfocus", "return " + _key + "_before(this.id);"); //Before
            }

            txt.Attributes.Add("value", Response);
            txt.Attributes.Add("class", GetControlClass());
            txt.Attributes.Add("data-prompt-position", "topLeft:15");
            txt.Attributes.Add("style", "" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
            txt.MergeAttributes(_inputHtmlAttributes);
            html.Append(txt.ToString(TagRenderMode.SelfClosing));

            var scriptNumeric = new TagBuilder("script");
            string uiSep = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;

            if (uiSep == ".")
            {
                scriptNumeric.InnerHtml = "$(function() { $('#" + inputName + "').numeric();});";
            }
            else
            {
                string DecimalSeperator = "\"" + uiSep + "\"";

                scriptNumeric.InnerHtml = "$(function() { $('#" + inputName + "').numeric({ decimal :" + DecimalSeperator + "});});";
            }
            html.Append(scriptNumeric.ToString(TagRenderMode.Normal));

            if (!string.IsNullOrEmpty(Pattern))
            {
                string maskedPatternEq = GetMaskedPattern(Pattern);
                var scriptMaskedInput = new TagBuilder("script");
                scriptMaskedInput.InnerHtml = "$(function() { $('#" + inputName + "').mask('" + maskedPatternEq + "');});";
                html.Append(scriptMaskedInput.ToString(TagRenderMode.Normal));
            }

            if (ReadOnly)
            {
                var scriptReadOnlyText = new TagBuilder("script");
                scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            }

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
                    maskedPattern = "9999";
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
        public string GetControlClass()
        {
            StringBuilder ControlClass = new StringBuilder();
            ControlClass.Append("validate[");

            if ((!string.IsNullOrEmpty(Lower)) && (!string.IsNullOrEmpty(Upper)))
            {
                ControlClass.Append("min[" + Lower + "],max[" + Upper + "],");
            }

            if (Required == true)
            {
                ControlClass.Append("required");
            }

            ControlClass.Append("]");
            return ControlClass.ToString();
        }
        
    }
}
