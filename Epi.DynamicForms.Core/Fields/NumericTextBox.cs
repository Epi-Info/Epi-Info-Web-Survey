using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

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

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            //  prompt.Attributes.Add("class", _promptClass);
            prompt.Attributes.Add("class", "EpiLabel");

            StringBuilder StyleValues = new StringBuilder();

            StyleValues.Append(GetContolStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), _PromptWidth.ToString(), Height.ToString()));
            prompt.Attributes.Add("style", StyleValues.ToString());
            html.Append(prompt.ToString());

            // error label
            if (!IsValid)
            {
                //Add new Error to the error Obj
            }

            // input element
            var txt = new TagBuilder("input");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "text");
            // txt.Attributes.Add("value", Value);
            txt.Attributes.Add("value",Value);
            txt.Attributes.Add("class", GetControlClass());
            txt.Attributes.Add("data-prompt-position", "topRight:15");
            txt.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px");
            txt.MergeAttributes(_inputHtmlAttributes);
            html.Append(txt.ToString(TagRenderMode.SelfClosing));

            //adding numeric text box validation jquery script tag
            var scriptNumeric = new TagBuilder("script");
            scriptNumeric.InnerHtml = "$(function() { $('#" + inputName + "').numeric();});";
            html.Append(scriptNumeric.ToString(TagRenderMode.Normal));

            // If readonly then add the following jquery script to make the field disabled 
            if (ReadOnly)
            {
                var scriptReadOnlyText = new TagBuilder("script");
                scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            }

            var wrapper = new TagBuilder(_fieldWrapper);
            wrapper.Attributes["class"] = _fieldWrapperClass;
            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
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
