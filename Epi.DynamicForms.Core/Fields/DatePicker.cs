using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents a datepicker whichis is a textbox and the datepicker.
    /// </summary>
    [Serializable]
    public class DatePicker : TextField
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
                var error = new TagBuilder("label");
                error.Attributes.Add("for", inputName);
                error.Attributes.Add("class", _errorClass);
                StringBuilder errorStyleValues = new StringBuilder();
                errorStyleValues.Append(GetContolStyle(_fontstyle.ToString(), (_Prompttop).ToString(), (_Promptleft).ToString(), _PromptWidth.ToString(), Height.ToString()));
                error.Attributes.Add("style", errorStyleValues.ToString());
                error.SetInnerText(Error);
                html.Append(error.ToString());
            }

            // input element
            var txt = new TagBuilder("input");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "text");
            txt.Attributes.Add("value", Value);

            //txt.Attributes.Add("class", "validate[required] text-input");
            txt.Attributes.Add("data-prompt-position", "topRight:15");


            txt.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px");

            txt.MergeAttributes(_inputHtmlAttributes);
            html.Append(txt.ToString(TagRenderMode.SelfClosing));


            // adding scripts for date picker
            var scriptDatePicker = new TagBuilder("script");
            scriptDatePicker.InnerHtml = "$(function() { $('#" + inputName + "').datepicker({changeMonth: true,changeYear: true});});";
            html.Append(scriptDatePicker.ToString(TagRenderMode.Normal));

            var wrapper = new TagBuilder(_fieldWrapper);
            wrapper.Attributes["class"] = _fieldWrapperClass;
            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
        }

    }
}