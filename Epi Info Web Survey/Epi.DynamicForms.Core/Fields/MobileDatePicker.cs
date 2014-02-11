﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents a datepicker whichis is a textbox and the datepicker.
    /// </summary>
    [Serializable]
    public class MobileDatePicker : DatePicker
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder();
            var inputName = _fieldPrefix + _key;
            string ErrorStyle = string.Empty;
            // prompt label
            var prompt = new TagBuilder("label");
            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("Id", "label" + inputName);
            prompt.Attributes.Add("class", "EpiLabel");

            StringBuilder StyleValues = new StringBuilder();
            StyleValues.Append(GetControlStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), _IsHidden));

            html.Append(prompt.ToString());

            if (!IsValid)
            {
                ErrorStyle = ";border-color: red";
            }

            var txt = new TagBuilder("input");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "date");
            txt.Attributes.Add("Theme", "b");
            txt.Attributes.Add("data-role", "datebox");
            txt.Attributes.Add("data-options", "{\"mode\": \"datebox\", \"pickPageButtonTheme\": \"e\", \"pickPageInputTheme\":\"e\", \"pickPageFlipButtonTheme\":\"a\", \"pickPageTheme\":\"e\" ,  \"useNewStyle\":true}");
            txt.Attributes.Add("value", Response);

            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {
                txt.Attributes.Add("onchange", "return " + _key + "_after();"); //After
            }

            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {
                txt.Attributes.Add("onfocus", "return " + _key + "_before(this.id);"); //Before
            }

            if (_MaxLength.ToString() != "0" && !string.IsNullOrEmpty(_MaxLength.ToString()))
            {
                txt.Attributes.Add("MaxLength", _MaxLength.ToString());
            }

            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";

            if (_IsDisabled)
            {
                txt.Attributes.Add("disabled", "disabled");
            }
            txt.Attributes.Add("class", GetControlClass(Response));
            txt.Attributes.Add("data-prompt-position", "topLeft:15");
            
            txt.Attributes.Add("style", "" + _ControlWidth.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);

            txt.MergeAttributes(_inputHtmlAttributes);
            html.Append(txt.ToString(TagRenderMode.SelfClosing));

            if (ReadOnly)
            {
                var scriptReadOnlyText = new TagBuilder("script");
                scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            }

            var wrapper = new TagBuilder(_fieldWrapper);

            if (!IsValid)
            {

                wrapper.Attributes["class"] = _fieldWrapperClass + " DatePickerNotValid";
            }
            else
            {
                wrapper.Attributes["class"] = _fieldWrapperClass;

            }
            if (_IsHidden)
            {
                wrapper.Attributes["style"] = "display:none";
                
            }
            wrapper.Attributes["id"] = inputName + "_fieldWrapper";
            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
        }

        public string GetControlClass(string Value)
        {
            StringBuilder ControlClass = new StringBuilder();
            ControlClass.Append("validate[");

            if ((!string.IsNullOrEmpty(GetRightDateFormat(Lower,Pattern).ToString()) && (!string.IsNullOrEmpty(GetRightDateFormat(Upper,Pattern).ToString()))))
            {
                ControlClass.Append("customDate[date],datePickerRange, " + GetRightDateFormat(Lower,Pattern).ToString() + "," + GetRightDateFormat(Upper,Pattern).ToString() + ",");

                if (_IsRequired == true)
                {
                    ControlClass.Append("required"); 

                }
                ControlClass.Append("] text-input datepicker");

                return ControlClass.ToString();
            }
            else
            {
                if (_IsRequired == true)
                {
                    ControlClass.Append("required,custom[date]] text-input datepicker({onClose:function(){setTimeout(" + _key + "_after,100);},changeMonth:true,changeYear:true});");
                }
                else
                {
                    ControlClass.Append("custom[date]] text-input datepicker");
                }

                return ControlClass.ToString();
            }
        }

        public string GetRightDateFormat(string Date, string pattern)
        {
            StringBuilder NewDateFormat = new StringBuilder();

            string MM = "";
            string DD = "";
            string YYYY = "";
            char splitChar = '/';

            if (!string.IsNullOrEmpty(Date))
            {
                if (Date.Contains('-'))
                {
                    splitChar = '-';
                }
                else
                {

                    splitChar = '/';
                }
                string[] dateList = Date.Split((char)splitChar);
                switch (pattern.ToString())
                {
                    case "YYYY-MM-DD":
                        MM = dateList[1];
                        DD = dateList[2];
                        YYYY = dateList[0];
                        break;
                    case "MM-DD-YYYY":
                        MM = dateList[0];
                        DD = dateList[1];
                        YYYY = dateList[2];
                        break;
                }

                NewDateFormat.Append(YYYY);
                NewDateFormat.Append('/');
                NewDateFormat.Append(MM);
                NewDateFormat.Append('/');
                NewDateFormat.Append(DD);
            }
            else
            {
                NewDateFormat.Append("");

            }
            return NewDateFormat.ToString();
        }
    }
}
