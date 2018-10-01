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
    public class MobileDatePicker : DatePicker
    {
        public string GetDatePickerFormat(string DateFormat)
        {          

            string datePickerFormat = string.Empty;
            switch (DateFormat.ToLower().ToString())
            {
                case "mm-dd-yyyy":
                    datePickerFormat = "%m-%d-%Y";
                    break;                
                case "mm/dd/yyyy":
                    datePickerFormat = "%m/%d/%Y";
                    break;
                case "mm.dd.yyyy":
                    datePickerFormat = "%m.%d.%Y";
                    break;
                case "m-d-yyyy":
                    datePickerFormat = "%m-%d-%Y";
                    break;
                case "m/d/yyyy":
                    datePickerFormat = "%m/%d/%y";
                    break;
                case "m.d.yyyy":
                    datePickerFormat = "%m.%d.%Y";
                    break;
                case "dd-mm-yyyy":
                    datePickerFormat = "%d-%m-%Y";
                    break;
                case "dd/mm/yyyy":
                    datePickerFormat = "%d/%m/%Y";
                    break;
                case "dd.mm.yyyy":
                    datePickerFormat = "%d.%m.%Y";
                    break;
                case "d-m-yyyy":
                    datePickerFormat = "%d-%m-%Y";
                    break;
                case "d/m/yyyy":
                    datePickerFormat = "%d/%m/%Y";
                    break;
                case "d.m.yyyy":
                    datePickerFormat = "%d.%m.%Y";
                    break;
                case "yyyy-mm-dd":
                    datePickerFormat = "%Y-%m-%d";
                    break;
                case "yyyy/mm/dd":
                    datePickerFormat = "%Y/%m/%d";
                    break;
                case "yyyy.mm.dd":
                    datePickerFormat = "%Y.%m.%d";
                    break;
                case "yyyy-m-d":
                    datePickerFormat = "%Y-%m-%d";
                    break;
                case "yyyy/m/d":
                    datePickerFormat = "%Y/%m/%d";
                    break;
                case "yyyy.m.d":
                    datePickerFormat = "%Y.%m.%d";
                    break;
                default:
                    datePickerFormat = "%m/%d/%Y";
                    break;

            }
            return datePickerFormat;
        }

        public string GetDatePickerOrder(string DateFormat)
        {

            string datePickerFormat = string.Empty;
            switch (DateFormat.ToLower().ToString())
            {
                case "mm-dd-yyyy":
                    datePickerFormat = "[\"m\",\"d\",\"y\"]";
                    break;
                case "mm/dd/yyyy":
                    datePickerFormat = "[\"m\",\"d\",\"y\"]";
                    break;
                case "mm.dd.yyyy":
                    datePickerFormat = "[\"m\",\"d\",\"y\"]";
                    break;
                case "m-d-yyyy":
                    datePickerFormat = "[\"m\",\"d\",\"y\"]";
                    break;
                case "m/d/yyyy":
                    datePickerFormat = "[\"m\",\"d\",\"y\"]";
                    break;
                case "m.d.yyyy":
                    datePickerFormat = "[\"m\",\"d\",\"y\"]";
                    break;
                case "dd-mm-yyyy":
                    datePickerFormat = "[\"d\",\"m\",\"y\"]";
                    break;
                case "dd/mm/yyyy":
                    datePickerFormat = "[\"d\",\"m\",\"y\"]";
                    break;
                case "dd.mm.yyyy":
                    datePickerFormat = "[\"d\",\"m\",\"y\"]";
                    break;
                case "d-m-yyyy":
                    datePickerFormat = "[\"d\",\"m\",\"y\"]";
                    break;
                case "d/m/yyyy":
                    datePickerFormat = "[\"d\",\"m\",\"y\"]";
                    break;
                case "d.m.yyyy":
                    datePickerFormat = "[\"d\",\"m\",\"y\"]";
                    break;
                case "yyyy-mm-dd":
                    datePickerFormat = "[\"y\",\"m\",\"d\"]";
                    break;
                case "yyyy/mm/dd":
                    datePickerFormat = "[\"y\",\"m\",\"d\"]";
                    break;
                case "yyyy.mm.dd":
                    datePickerFormat = "[\"y\",\"m\",\"d\"]";
                    break;
                case "yyyy-m-d":
                    datePickerFormat = "[\"y\",\"m\",\"d\"]";
                    break;
                case "yyyy/m/d":
                    datePickerFormat = "[\"y\",\"m\",\"d\"]";
                    break;
                case "yyyy.m.d":
                    datePickerFormat = "[\"y\",\"m\",\"d\"]";
                    break;
                default:
                    datePickerFormat = "[\"m\",\"d\",\"y\"]";
                    break;


            }
            return datePickerFormat;
        }
        public override string RenderHtml()
        {
            var html = new StringBuilder();
            var inputName = _fieldPrefix + _key;
            string ErrorStyle = string.Empty;

            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string DateFormat = currentCulture.DateTimeFormat.ShortDatePattern;
            //DateFormat = DateFormat.Remove(DateFormat.IndexOf("y"), 2);

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("Id", "label" + inputName);
            prompt.Attributes.Add("class", "EpiLabel");

           // StringBuilder StyleValues = new StringBuilder();
           // StyleValues.Append(GetControlStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), _IsHidden));
           // prompt.Attributes.Add("style", StyleValues.ToString());
            html.Append(prompt.ToString());
            var NewDateFormat = string.Empty;
            if (!IsValid)
            {
                ErrorStyle = ";border-color: red";
                NewDateFormat = GetRightDateFormat(Response, "YYYY-MM-DD", DateFormat);
            }
            else
            {
                NewDateFormat = GetRightDateFormat(Response, "YYYY-MM-DD", DateFormat);
            }
            var txt = new TagBuilder("input");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            txt.Attributes.Add("type", "text");
            txt.Attributes.Add("Theme", "b");
            txt.Attributes.Add("data-role", "datebox");
           txt.Attributes.Add("data-options", "{\"mode\": \"datebox\", \"pickPageButtonTheme\": \"e\", \"pickPageInputTheme\":\"e\", \"pickPageFlipButtonTheme\":\"a\", \"pickPageTheme\":\"e\" ,  \"useNewStyle\":true,  \"dateFormat\":\"" + GetDatePickerFormat(DateFormat) + "\""+  "}");
         

            txt.Attributes.Add("value", NewDateFormat);
            //txt.Attributes.Add("data-date-format",DateFormat.ToLower());
            txt.Attributes.Add("onkeydown ", "return DateFormat(this, event.keyCode);");           


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

            //if (_IsDisabled)
            //{
            //    txt.Attributes.Add("disabled", "disabled");
            //}
           txt.Attributes.Add("class", GetControlClass(Response));          
                txt.Attributes.Add("data-prompt-position", "topLeft:15");

                txt.Attributes.Add("style", "" + _ControlWidth.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);          
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

            txt.MergeAttributes(_inputHtmlAttributes);
            html.Append(txt.ToString(TagRenderMode.SelfClosing));

            if (ReadOnly)
            {
                //var scriptReadOnlyText = new TagBuilder("script");
                //scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                //html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
         
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
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            string DateFormat = currentCulture.DateTimeFormat.ShortDatePattern;
            StringBuilder ControlClass = new StringBuilder();
            ControlClass.Append("IsDate validate[");

            if ((!string.IsNullOrEmpty(GetRightDateFormat(Lower,Pattern).ToString()) && (!string.IsNullOrEmpty(GetRightDateFormat(Upper,Pattern).ToString()))))
            {
                ControlClass.Append("customDate[date],datePickerRange, " + GetRightDateFormat(Lower,Pattern).ToString() + "," + GetRightDateFormat(Upper,Pattern).ToString() + ",");

                if (Required == true)
                {
                    ControlClass.Append("required"); 
                }
                ControlClass.Append("] text-input datepicker");

                return ControlClass.ToString();
            }
            else
            {
                if (Required == true)
                {
                    ControlClass.Append("required,custom[" + DateFormat.ToUpper() + "]] text-input datepicker({onClose:function(){setTimeout(" + _key + "_after,100);},changeMonth:true,changeYear:true});");
                }
                else
                {
                    ControlClass.Append("custom[" + DateFormat.ToUpper() + "]] text-input datepicker");
                }

                return ControlClass.ToString();
            }
        }

        public string GetRightDateFormat(string Date, string patternIn, string patternOut = "")
        {
            StringBuilder NewDateFormat = new StringBuilder();

            string MM = "";
            string DD = "";
            string YYYY = "";
            char splitChar = '/';
            if (!string.IsNullOrEmpty(Date) && Date != "SYSTEMDATE")
            {
                if (Date.Contains('-'))
                {
                    splitChar = ' ';
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
                }
                else
                {
                    switch (patternOut.ToLower().ToString())
                    {
                        case "dd/mm/yyyy":
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "dd/mm/yy":
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "d/m/yy":
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
                        case "m/d/yyyy":
                        case "m/d/yy":
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "yy/mm/dd":
                            NewDateFormat.Append(YYYY);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('/');
                            NewDateFormat.Append(DD);
                            break;
                        //.
                        case "dd.mm.yyyy":
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('.');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('.');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "dd.mm.yy":
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('.');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('.');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "mm.dd.yyyy":
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('.');
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('.');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "m.d.yy":
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('.');
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('.');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "yy.MM.dd":
                            NewDateFormat.Append(YYYY);
                            NewDateFormat.Append('.');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('.');
                            NewDateFormat.Append(DD);
                            break;
                        //-
                        case "dd-mm-yyyy":
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "dd-mm-yy":
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "d-m-yy":
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "mm-dd-yyyy":
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "m-d-yy":
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(DD);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(YYYY);
                            break;
                        case "yyyy-mm-dd":
                        case "yy-mm-dd":
                            NewDateFormat.Append(YYYY);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(MM);
                            NewDateFormat.Append('-');
                            NewDateFormat.Append(DD);
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
       
    }
}
