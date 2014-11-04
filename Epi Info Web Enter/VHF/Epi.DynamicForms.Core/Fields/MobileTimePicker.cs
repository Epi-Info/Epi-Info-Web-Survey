using System;
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
    public class MobileTimePicker : TimePickerField
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
            prompt.Attributes.Add("class", "EpiLabel");

            StringBuilder StyleValues = new StringBuilder();
            StyleValues.Append(GetContolStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), _IsHidden));
           // prompt.Attributes.Add("style", StyleValues.ToString());
            html.Append(prompt.ToString());

            // error label
            if (!IsValid)
            {
                ErrorStyle = ";border-color: red";

            }

            // input element
            var txt = new TagBuilder("input");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "date");
            txt.Attributes.Add("data-role", "datebox");
            txt.Attributes.Add("data-options", "{\"mode\": \"timebox\" , \"themeInput\":\"e\" , \"themeButton\" : \"e\", \"pickPageButtonTheme\": \"e\", \"pickPageInputTheme\":\"e\", \"pickPageFlipButtonTheme\":\"a\", \"pickPageTheme\":\"e\"}");
            //txt.Attributes.Add("data-theme", "pickPageInputTheme");
           
            
            
            txt.Attributes.Add("value", Value);
            ////////////Check code start//////////////////
            EnterRule FunctionObjectAfter = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + _key);
            //if Pattern is empty and the control has after event then treat it like a text box
            //if (string.IsNullOrEmpty(Pattern))
            //{
                if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
                {
                   // txt.Attributes.Add("onblur", "return " + _key + "_after(this.id);"); //After
                    txt.Attributes.Add("onchange", "return " + _key + "_after(this.id);"); //After
                  
                }
            //}
            EnterRule FunctionObjectBefore = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=before&identifier=" + _key);
            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {
                txt.Attributes.Add("onfocus", "return " + _key + "_before(this.id);"); //Before
            }

            ////////////Check code end//////////////////

            if (_MaxLength.ToString() != "0" && !string.IsNullOrEmpty(_MaxLength.ToString()))
            {
                txt.Attributes.Add("MaxLength", _MaxLength.ToString());
            }
            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";
            //if (_IsHidden)
            //{
            //    IsHiddenStyle = "display:none";
            //}
          /*  if (_IsHighlighted)
            {
                IsHighlightedStyle = "background-color:yellow";
            }*/

            //if (_IsDisabled)
            //{
            //    txt.Attributes.Add("disabled", "disabled");
            //}
            //todo: add validation
            //txt.Attributes.Add("class", GetControlClass(Value));

            if (_IsRequired == true)
            {
                //txt.Attributes.Add("class", "validate[custom[time],required] text-input datepicker");
                txt.Attributes.Add("class", "validate[required,custom[time]] text-input datepicker");
                
                txt.Attributes.Add("data-prompt-position", "topLeft:15");
            }
            else
            {
                txt.Attributes.Add("class", "validate[custom[time]] text-input datepicker");
                txt.Attributes.Add("data-prompt-position", "topLeft:15");
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
            //if (!string.IsNullOrEmpty(Pattern))
            //{
            //    // adding scripts for date picker
            //    var scripttimePicker = new TagBuilder("script");
            //    //scriptDatePicker.InnerHtml = "$(function() { $('#" + inputName + "').datepicker({changeMonth: true,changeYear: true});});";
            //    /*Checkcode control after event...for datepicker, the onblur event fires on selecting a date from calender. Since the datepicker control itself is tied to after event which was firing before the datepicker
            //     textbox is populated the comparison was not working. For this reason, the control after steps are interjected inside datepicker onClose event, so the after event is fired when the datepicker is populated 
            //     */
            //    if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            //    {
            //        //scriptDatePicker.InnerHtml = "$('#" + inputName + "').datepicker({onClose:function(){" + _key + "_after();},changeMonth:true,changeYear:true});";
            //        //Note: datepicker seems to have a command inst.input.focus(); (I think) called after the onClose callback which resets the focus to the original input element. I'm wondering if there is way round this with bind(). 
            //        //http://stackoverflow.com/questions/7087987/change-the-focus-on-jqueryui-datepicker-on-close

            //        if (Pattern == "HH:MM:SS AMPM")
            //        {
            //            scripttimePicker.InnerHtml = "$('#" + inputName + "').timepicker({onClose:function(){setTimeout(" + _key + "_after,100);},ampm : true, showSecond:true,timeFormat: 'hh:mm:ss TT'});";
            //        }
            //        else
            //        {
            //            scripttimePicker.InnerHtml = "$('#" + inputName + "').timepicker({onClose:function(){setTimeout(" + _key + "_after,100);}, showSecond:true,timeFormat: 'hh:mm:ss'});";
            //        }

            //    }
            //    else
            //    {
            //        if (Pattern == "HH:MM:SS AMPM")
            //        {
            //            scripttimePicker.InnerHtml = "$('#" + inputName + "').timepicker({ampm : true, showSecond:true,timeFormat: 'hh:mm:ss TT'});";
            //        }
            //        else
            //        {
            //            scripttimePicker.InnerHtml = "$('#" + inputName + "').timepicker({showSecond:true,timeFormat: 'hh:mm:ss'});";
            //        }
            //    }

            //    html.Append(scripttimePicker.ToString(TagRenderMode.Normal));
            //}

            ////prevent date picker control to submit on enter click
            //var scriptBuilder = new TagBuilder("script");
            //scriptBuilder.InnerHtml = "$('#" + inputName + "').BlockEnter('" + inputName + "');";
            //scriptBuilder.ToString(TagRenderMode.Normal);
            //html.Append(scriptBuilder.ToString(TagRenderMode.Normal));

            var wrapper = new TagBuilder(_fieldWrapper);
            //wrapper.Attributes["class"] = _fieldWrapperClass;

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

                //   ControlClass.Append("customDate[date],future[" + GetRightDateFormat(Lower).ToString() + "],past[" + GetRightDateFormat(Upper).ToString() + "],");
                //dateRange
                ControlClass.Append("customDate[date],datePickerRange, " + GetRightDateFormat(Lower).ToString() + "," + GetRightDateFormat(Upper).ToString() + ",");
            }
            if (_IsRequired == true)
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
