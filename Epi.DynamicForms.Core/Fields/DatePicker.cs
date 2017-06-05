using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
using System.Globalization;
using System.Threading;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents a datepicker whichis is a textbox and the datepicker.
    /// </summary>
    [Serializable]
    public class DatePicker : DatePickerField
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder();
            var inputName = _fieldPrefix + _key;
            string ErrorStyle = string.Empty;
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string DateFormat = currentCulture.DateTimeFormat.ShortDatePattern;
            DateFormat = DateFormat.Remove(DateFormat.IndexOf("y"), 2);
            // prompt label
            var prompt = new TagBuilder("label");
            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("Id", "label" + inputName);
            prompt.Attributes.Add("class", "EpiLabel");

            StringBuilder StyleValues = new StringBuilder();
            StyleValues.Append(GetControlStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), _IsHidden));
            prompt.Attributes.Add("style", StyleValues.ToString());
            html.Append(prompt.ToString());

            // error label
            if (!IsValid)
            {
                ErrorStyle = ";border-color: red";
                
            }

            // input element
            var NewDateFormat = GetRightDateFormat(Response, "YYYY-MM-DD", DateFormat);
            var txt = new TagBuilder("input");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "text");
            txt.Attributes.Add("value", NewDateFormat);

            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            { 
                txt.Attributes.Add("onfocus", "return " + _key + "_before();"); //Before
            }
                       
            if (_MaxLength.ToString() != "0" && !string.IsNullOrEmpty(_MaxLength.ToString()))
            {
                txt.Attributes.Add("MaxLength", _MaxLength.ToString());
            }
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
            txt.Attributes.Add("class", GetControlClass(Response));

            string InputFieldStyle = GetInputFieldStyle(_InputFieldfontstyle.ToString(), _InputFieldfontSize, _InputFieldfontfamily.ToString());
            txt.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle + ";" + InputFieldStyle);            

            txt.MergeAttributes(_inputHtmlAttributes);
            html.Append(txt.ToString(TagRenderMode.SelfClosing));

            // If readonly then add the following jquery script to make the field disabled 
            if (ReadOnly || _IsDisabled)
                {
                var scriptReadOnlyText = new TagBuilder("script");
                //scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                scriptReadOnlyText.InnerHtml = "$(function(){  var List = new Array();List.push('" + _key + "');CCE_Disable(List, false);});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
                }
            // adding scripts for date picker
            var scriptDatePicker = new TagBuilder("script");
            //scriptDatePicker.InnerHtml = "$(function() { $('#" + inputName + "').datepicker({changeMonth: true,changeYear: true});});";
            /*Checkcode control after event...for datepicker, the onblur event fires on selecting a date from calender. Since the datepicker control itself is tied to after event which was firing before the datepicker
             textbox is populated the comparison was not working. For this reason, the control after steps are interjected inside datepicker onClose event, so the after event is fired when the datepicker is populated 
             */
            var MinYear = -110;
            var MaxYear = 10;
            if (!string.IsNullOrEmpty(Lower) && !string.IsNullOrEmpty(Upper))
            {
                int Year_Lower = GetYear(Lower,Pattern);
                int Year_Upper = GetYear(Upper, Pattern);

                MinYear = -(DateTime.Now.Year - Year_Lower);
                MaxYear =Year_Upper - DateTime.Now.Year ;
            }
            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {
                //scriptDatePicker.InnerHtml = "$('#" + inputName + "').datepicker({onClose:function(){" + _key + "_after();},changeMonth:true,changeYear:true});";
                //Note: datepicker seems to have a command inst.input.focus(); (I think) called after the onClose callback which resets the focus to the original input element. I'm wondering if there is way round this with bind(). 
                //http://stackoverflow.com/questions/7087987/change-the-focus-on-jqueryui-datepicker-on-close
                scriptDatePicker.InnerHtml = "$('#" + inputName + "').datepicker({onClose:function(){setTimeout(" + _key + "_after,100);},changeMonth:true,changeYear:true,yearRange:'" + MinYear + ":+" + MaxYear + "'});";
            }
            else
            {
                scriptDatePicker.InnerHtml = "$('#" + inputName + "').datepicker({changeMonth: true,changeYear: true,yearRange:'" + MinYear + ":+" + MaxYear + "'});";
            }
             html.Append(scriptDatePicker.ToString(TagRenderMode.Normal));

           

            var scriptDatePicker1 = new TagBuilder("script");
            scriptDatePicker1.InnerHtml = "$('#" + inputName + "').change(function() { ChangeDatePickerFormat('" + DateFormat + "');});";
            html.Append(scriptDatePicker1.ToString(TagRenderMode.Normal));

            //prevent date picker control to submit on enter click
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


        public string GetControlClass(string Value)
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string DateFormat = currentCulture.DateTimeFormat.ShortDatePattern;

            StringBuilder ControlClass = new StringBuilder();

            ControlClass.Append("validate[");


            if ((!string.IsNullOrEmpty(GetRightDateFormat(Lower,Pattern).ToString()) && (!string.IsNullOrEmpty(GetRightDateFormat(Upper,Pattern).ToString()))))
            {

                //   ControlClass.Append("customDate[date],future[" + GetRightDateFormat(Lower).ToString() + "],past[" + GetRightDateFormat(Upper).ToString() + "],");
                //dateRange
                ControlClass.Append("customDate[date],datePickerRange, " + GetRightDateFormat(Lower,Pattern).ToString() + "," + GetRightDateFormat(Upper,Pattern).ToString() + ",");
                if (Required == true)
                {

                    ControlClass.Append("required"); // working fine

                }
                ControlClass.Append("] text-input datepicker");

                return ControlClass.ToString();

            }
            else
            {
                if (Required == true)
                {
                    // ControlClass.Append("required,custom[date]] text-input datepicker");
                    ControlClass.Append("required,custom[" + DateFormat.ToUpper() + "]]  datepicker ");
                }
                else
                {
                    //ControlClass.Append("custom[date]] text-input datepicker");
                    ControlClass.Append("custom[" + DateFormat.ToUpper() + "]]  datepicker ");
                }
                return ControlClass.ToString();
            }
         

        }

        public string GetRightDateFormat(string Date, string patternIn,string patternOut ="")
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
                    splitChar=' ';
                    splitChar = '-';
                }
                else
                {

                    splitChar = '/';
                }
                string[] dateList = Date.Split((char)splitChar);
                switch (patternIn.ToString())
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
                if (string.IsNullOrEmpty(patternOut))
                {
                NewDateFormat.Append(YYYY);
                NewDateFormat.Append('/');
                NewDateFormat.Append(MM);
                NewDateFormat.Append('/');
                NewDateFormat.Append(DD);
                }else{
                    switch (patternOut.ToString())
                    {
                        case "dd/mm/yyyy":
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "dd/MM/yy":
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "mm/dd/yyyy":
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "M/d/yy":
                             NewDateFormat.Append(MM);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(YYYY);
                            break;
                    }
                }
            }
            else
            {
                NewDateFormat.Append("");

            }
            return NewDateFormat.ToString();
        }
        public int GetYear(string Date, string pattern)
        {

            string YYYY = "";
            char splitChar = '/';
            if (!string.IsNullOrEmpty(Date))
            {
                if (Date.Contains('-'))
                {
                    splitChar = '-';
                }
                else if (Date.Contains('/'))
                {

                    splitChar = '/';
                }
                string[] dateList = Date.Split((char)splitChar);
                switch (pattern.ToString())
                {
                    case "YYYY-MM-DD":

                        YYYY = dateList[0];
                        break;
                    case "MM-DD-YYYY":

                        YYYY = dateList[2];
                        break;
                }


            }
            int Year;
            bool result = Int32.TryParse(YYYY, out Year);
            if (result)
            {
                return Year;
            }
            else
            {
                return 0;
            }
        }
    }
}
