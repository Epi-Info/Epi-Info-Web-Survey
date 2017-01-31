using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html textarea element.
    /// </summary>
    [Serializable]
    public class MobileTextArea : TextArea
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder();
            var inputName = _fieldPrefix + _key;
            string ErrorStyle = string.Empty;
            var prompt = new TagBuilder("label");

            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", "EpiLabel");
            prompt.Attributes.Add("Id", "label" + inputName);
            StringBuilder StyleValues = new StringBuilder();
            StyleValues.Append(GetControlStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), IsHidden));
            
           // prompt.Attributes.Add("style", "");
            prompt.Attributes.Add("style", StyleValues.ToString());
            html.Append(prompt.ToString());

            if (!IsValid)
            {
                 ErrorStyle = ";border-color: red";
            }

            var txt = new TagBuilder("textarea");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            string InputFieldStyle = GetInputFieldStyle(_InputFieldfontstyle.ToString(), _InputFieldfontSize, _InputFieldfontfamily.ToString());            
            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {
                txt.Attributes.Add("onblur", "return " + _key + "_after(this.id);"); //After
            }

            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {
                txt.Attributes.Add("onfocus", "return " + _key + "_before(this.id);"); //Before
            }

            txt.SetInnerText(Response);

            if (Required == true)
            {
                txt.Attributes.Add("class", "validate[required] text-input");
                txt.Attributes.Add("data-prompt-position", "topRight:15");
            }
            
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
            txt.Attributes.Add("style", "height:" + "100" + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle + InputFieldStyle);
            
            txt.MergeAttributes(_inputHtmlAttributes);
            html.Append(txt.ToString());

            if (ReadOnly || _IsDisabled)
                {
                var scriptReadOnlyText = new TagBuilder("script");
                //scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                scriptReadOnlyText.InnerHtml = "$(function(){  var List = new Array();List.push('" + _key + "');CCE_Disable(List, false);});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
                }

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
        public override string GetControlStyle(string ControlFontStyle, string Top, string Left, string Width, string Height, bool IsHidden)
        {
            StringBuilder FontStyle = new StringBuilder();
            StringBuilder FontWeight = new StringBuilder();
            StringBuilder TextDecoration = new StringBuilder();
            StringBuilder CssStyles = new StringBuilder();

            char[] delimiterChars = { ' ', ',' };
            string[] Styles = ControlFontStyle.Split(delimiterChars);
          //  CssStyles.Append("width: auto");

            foreach (string Style in Styles)
            {
                switch (Style.ToString())
                {
                    case "Italic":
                        FontStyle.Append(Style.ToString());
                        break;
                    case "Oblique":
                        FontStyle.Append(Style.ToString());
                        break;
                }

            }

            foreach (string Style in Styles)
            {
                switch (Style.ToString())
                {
                    case "Bold":
                        FontWeight.Append(Style.ToString());
                        break;
                    case "Normal":
                        FontWeight.Append(Style.ToString());
                        break;
                }
            }

            CssStyles.Append(" font:");//1

            if (!string.IsNullOrEmpty(FontStyle.ToString()))
            {
                CssStyles.Append(FontStyle);//2
                CssStyles.Append(" ");//3
            }

            CssStyles.Append(FontWeight);
            CssStyles.Append(" ");
            CssStyles.Append(_fontSize.ToString() + "pt ");
            CssStyles.Append(" ");
            CssStyles.Append(_fontfamily.ToString());

            foreach (string Style in Styles)
            {
                switch (Style.ToString())
                {
                    case "Strikeout":
                        TextDecoration.Append("line-through");
                        break;
                    case "Underline":
                        TextDecoration.Append(Style.ToString());
                        break;
                }
            }

            if (!string.IsNullOrEmpty(TextDecoration.ToString()))
            {
                CssStyles.Append(";text-decoration:");
            }

            if (IsHidden)
            {
                CssStyles.Append(";display:none");
            }

            CssStyles.Append(TextDecoration);

            return CssStyles.ToString();
        }
    }
}