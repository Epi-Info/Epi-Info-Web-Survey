using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
namespace MvcDynamicForms.Fields
{
    [Serializable]
    public class MobileTimePicker : TimePicker
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

           // StringBuilder StyleValues = new StringBuilder();
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
            txt.Attributes.Add("type", "date");
            txt.Attributes.Add("data-role", "datebox");
          ///  txt.Attributes.Add("data-options", "{\"mode\": \"timebox\" , \"themeInput\":\"e\" , \"themeButton\" : \"e\", \"pickPageButtonTheme\": \"e\", \"pickPageInputTheme\":\"e\", \"pickPageFlipButtonTheme\":\"a\", \"pickPageTheme\":\"e\"}");
            txt.Attributes.Add("data-options", "{\"mode\": \"durationbox\" , \"timeFormat\":\"24\"}");
          
            txt.Attributes.Add("value", Value);            

            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {
                txt.Attributes.Add("onchange", "return " + _key + "_after(this.id);"); //After
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

            //if (_IsDisabled)
            //{
            //    txt.Attributes.Add("disabled", "disabled");
            //}

            //if (Required == true)
            //{
            //    txt.Attributes.Add("class", "validate[required,custom[time]] text-input datepicker");
            //    txt.Attributes.Add("data-prompt-position", "topLeft:15");
            //}
            //else
            //{
            //    txt.Attributes.Add("class", "validate[custom[time]] text-input datepicker");
            //    txt.Attributes.Add("data-prompt-position", "topLeft:15");
            //}
            if (Required == true)
            {
                
                txt.Attributes.Add("class", "validate[required,custom[time]]  text-input  datepicker IsTime");
                txt.Attributes.Add("data-prompt-position", "topRight:15");
            }
            else
            {
                 
                txt.Attributes.Add("class", "validate[custom[time]]   text-input  datepicker IsTime");
                txt.Attributes.Add("data-prompt-position", "topRight:15");
            }
            //if (ReadOnly)
            //    {
            //    txt.Attributes.Add("disabled", "disabled");
            //    }
            if (ReadOnly || _IsDisabled)
                {
                var scriptReadOnlyText = new TagBuilder("script");
                //scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                scriptReadOnlyText.InnerHtml = "$(function(){  var List = new Array();List.push('" + _key + "');CCE_Disable(List, false);});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
                }
            txt.Attributes.Add("style", "" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);

            txt.MergeAttributes(_inputHtmlAttributes);
            html.Append(txt.ToString(TagRenderMode.SelfClosing));

            //if (ReadOnly)
            //{
            //    var scriptReadOnlyText = new TagBuilder("script");
            //    scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
            //    html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            //}

            var hdn = new TagBuilder("input");
            hdn.Attributes.Add("type", "hidden");
            hdn.Attributes.Add("id", inputName + "_hidden");
            hdn.Attributes.Add("name", inputName);
            hdn.Attributes.Add("value", Value);
           
            html.Append(hdn.ToString(TagRenderMode.SelfClosing));



            var wrapper = new TagBuilder(_fieldWrapper);

            if (!IsValid)
            {
                wrapper.Attributes["class"] = _fieldWrapperClass + " TimePickerNotValid";
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

            if ((!string.IsNullOrEmpty(GetRightDateFormat(Lower).ToString()) && (!string.IsNullOrEmpty(GetRightDateFormat(Upper).ToString()))))
            {
                ControlClass.Append("customDate[date],datePickerRange, " + GetRightDateFormat(Lower).ToString() + "," + GetRightDateFormat(Upper).ToString() + ",");
            }

            if (Required == true)
            {

                ControlClass.Append("required"); // working fine

            }
            
            ControlClass.Append("] text-input datepicker");

            return ControlClass.ToString();
        }

        public string GetRightDateFormat(string Date)
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
                MM = dateList[0];
                DD = dateList[1];
                YYYY = dateList[2];
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
