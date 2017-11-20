using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Core.EnterInterpreter;
using System.Drawing;
//using Newtonsoft.Json;
using System.Configuration;
using System.Web.Mvc;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html select element.
    /// </summary>
    [Serializable]
    public class AutoComplete :TextField
    {
        /// <summary>
        /// The number of options to display at a time.
        /// </summary>
        public Dictionary<string, bool> _choices = new Dictionary<string, bool>();
        public string RelateCondition { get; set; }
        public float ControlFontSize { get; set; }
        public string ControlFontStyle { get; set; }
        public string SelectedValue { get; set; }
        public Dictionary<string, bool> Choices
        {
            get
            {
                return _choices;
            }
            set
            {
                _choices = value;
            }
        }
        public int Size
        {
            get
            {
                string size;
                return _inputHtmlAttributes.TryGetValue("size", out size) ? int.Parse(size) : 1;
            }
            set { _inputHtmlAttributes["size"] = value.ToString(); }
        }
        /// <summary>
        /// Determines whether the select element will accept multiple selections.
        /// </summary>
        public bool MultipleSelection
        {
            get
            {
                string multiple;
                if (_inputHtmlAttributes.TryGetValue("multiple", out multiple))
                {
                    return multiple.ToLower() == "multiple";
                }
                return false;
            }
            set { _inputHtmlAttributes["multiple"] = value.ToString(); }
        }
        private string CacheIsOn = "";
        private string IsCacheSlidingExpiration = "";
        private int CacheDuration = 0;
        public int SelectType { get; set; }

        /// <summary>
        /// The text to be rendered as the first option in the select list when ShowEmptyOption is set to true.
        /// </summary>
        /// 
        public string EmptyOption { get; set; }
        /// <summary>
        /// Determines whether a valueless option is rendered as the first option in the list.
        /// </summary>
        public bool ShowEmptyOption { get; set; }

        public override string RenderHtml()
        {
            int DropDownLimitSize = 1;
            int.TryParse(ConfigurationManager.AppSettings["CACHE_DURATION"].ToString(), out CacheDuration);
            CacheIsOn = ConfigurationManager.AppSettings["CACHE_IS_ON"];//false;
            IsCacheSlidingExpiration = ConfigurationManager.AppSettings["CACHE_SLIDING_EXPIRATION"].ToString();
            var html = new StringBuilder();

            var inputName = _form.FieldPrefix + _key;
            string ErrorStyle = string.Empty;

            // prompt
            var prompt = new TagBuilder("label");

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\r\n|\r|\n)+");

            string newText = regex.Replace(Prompt.Replace(" ", "&nbsp;"), "<br />");

            string NewPromp = System.Web.Mvc.MvcHtmlString.Create(newText).ToString();


            prompt.InnerHtml = NewPromp;
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", "EpiLabel");
            prompt.Attributes.Add("Id", "label" + inputName);


            StringBuilder StyleValues = new StringBuilder();
            StyleValues.Append(GetControlStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), _IsHidden));
            prompt.Attributes.Add("style", StyleValues.ToString());
            html.Append(prompt.ToString());

            // error label
            if (!IsValid)
            {
                //Add new Error to the error Obj

                ErrorStyle = ";border-color: red";

            }

     
            TagBuilder input = null;
            input = new TagBuilder("input");
            input.Attributes.Add("list", inputName + "_DataList");
            input.Attributes.Add("data-autofirst", "true");
            input.Attributes.Add("value", Value);
            input.Attributes.Add("id", inputName);
            input.Attributes.Add("name", inputName);
            ////////////Check code start//////////////////
            EnterRule FunctionObjectAfter = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + _key);
            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {

                input.Attributes.Add("onblur", "return " + _key + "_after();"); //After
                // select.Attributes.Add("onchange", "return " + _key + "_after();"); //After
            }
            EnterRule FunctionObjectBefore = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=before&identifier=" + _key);
            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {

                input.Attributes.Add("onfocus", "return " + _key + "_before();"); //Before
            }
            EnterRule FunctionObjectClick = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=click&identifier=" + _key);

            
            //if (!string.IsNullOrEmpty(this.RelateCondition))
            //{
            //    input.Attributes.Add("onchange", "SetCodes_Val(this,'" + _form.SurveyInfo.SurveyId + "','" + _key + "');  "); //click
               
               


            //}
            ////////////Check code end//////////////////
            int LargestChoiseLength = 0;
            string measureString = "";
            foreach (var choise in _choices)
            {

                if (choise.Key.ToString().Length > LargestChoiseLength)
                {
                    LargestChoiseLength = choise.Key.ToString().Length;

                    measureString = choise.Key.ToString();
                }


            }

            // LargestChoiseLength = LargestChoiseLength * _ControlFontSize;

            Font stringFont = new Font(ControlFontStyle, ControlFontSize);

            SizeF size = new SizeF();
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                size = g.MeasureString(measureString.ToString(), stringFont);
            }



            // stringSize = (int) Graphics.MeasureString(measureString.ToString(), stringFont).Width;


            if (_IsRequired == true)
            {
                //awesomplete
                if (this._choices.Count() < DropDownLimitSize)
                {
                    if ((size.Width) > _ControlWidth)
                    {
                        // select.Attributes.Add("class", GetControlClass() + "text-input fix-me");
                        input.Attributes.Add("class", GetControlClass() + "fix-me");
                    }
                    else
                    {
                        // select.Attributes.Add("class", GetControlClass() + "text-input");
                        input.Attributes.Add("class", GetControlClass());
                    }
                    input.Attributes.Add("data-prompt-position", "topRight:10");

                }
                else
                {
                    if ((size.Width) > _ControlWidth)
                    {

                        // select.Attributes.Add("class", GetControlClass() + "text-input fix-me");
                        input.Attributes.Add("class", GetControlClass() + "fix-me awesomplete Search");
                    }
                    else
                    {
                        // select.Attributes.Add("class", GetControlClass() + "text-input");
                        input.Attributes.Add("class", GetControlClass() + " awesomplete Search");
                    }
                    input.Attributes.Add("data-prompt-position", "topRight:10");

                }
            }
            else
            {
                //awesomplete
                if (this._choices.Count() < DropDownLimitSize)
                {
                    //select.Attributes.Add("class", GetControlClass() + "text-input fix-me");
                    if ((size.Width) > _ControlWidth)
                    {
                        input.Attributes.Add("class", GetControlClass() + "fix-me ");
                    }
                    else
                    {

                        input.Attributes.Add("class", GetControlClass());
                    }
                    input.Attributes.Add("data-prompt-position", "topRight:10");
                }
                else
                {
                    if ((size.Width) > _ControlWidth)
                    {
                        input.Attributes.Add("class", GetControlClass() + "fix-me awesomplete Search");
                    }
                    else
                    {

                        input.Attributes.Add("class", GetControlClass() + " awesomplete Search");
                    }
                    input.Attributes.Add("data-prompt-position", "topRight:10");



                }
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
            //    select.Attributes.Add("disabled", "disabled");
            //}

            string InputFieldStyle = GetInputFieldStyle(_InputFieldfontstyle.ToString(), _InputFieldfontSize, _InputFieldfontfamily.ToString());
            input.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px ; font-size:" + ControlFontSize + "pt;" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle + ";" + InputFieldStyle);
            input.MergeAttributes(_inputHtmlAttributes);
            html.Append(input.ToString(TagRenderMode.StartTag));

            if (ReadOnly || _IsDisabled)
            {
                var scriptReadOnlyText = new TagBuilder("script");
                //scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                scriptReadOnlyText.InnerHtml = "$(function(){  var List = new Array();List.push('" + _key + "');CCE_Disable(List, false);});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            }

            if (this._choices.Count() > DropDownLimitSize)
            {

                var scriptReadOnlyText = new TagBuilder("script");
                StringBuilder Script = new StringBuilder();
                Script.Append("$(window).load(function () {  ");

                Script.Append(" $( '#" + inputName + "' ).next().css( 'position', 'absolute' );  ");
                Script.Append(" $( '#" + inputName + "' ).next().css( 'left', '" + _left.ToString() + "px' );  ");
                Script.Append(" $( '#" + inputName + "' ).next().css( 'top', '" + (_top + 29).ToString() + "px' );  ");

               
                Script.Append("});");
                scriptReadOnlyText.InnerHtml = Script.ToString();
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));


                var scriptReadOnlyText1 = new TagBuilder("script");
                StringBuilder Script1 = new StringBuilder();
                Script1.Append("  $(document).ready(function () {");

              
                Script1.Append("$( '#" + inputName + "').blur(function() { ");
                Script1.Append("SetCodes_Val(this,'" + _form.SurveyInfo.SurveyId + "','" + _key + "');  ");
              
                Script1.Append("});");
                Script1.Append("});");
                scriptReadOnlyText1.InnerHtml = Script1.ToString();
                html.Append(scriptReadOnlyText1.ToString(TagRenderMode.Normal));

            }

            
            
                   
                ///////////////////////////

                var datalist = new TagBuilder("datalist ");
                datalist.Attributes.Add("id", inputName + "_DataList");
                html.Append(datalist.ToString(TagRenderMode.StartTag));
                foreach (var choice in _choices)
                {
                    var opt = new TagBuilder("option");
                    opt.Attributes.Add("value", choice.Key);
                    opt.SetInnerText(choice.Key);
                  
                    html.Append(opt.ToString());
               




                ///////////////////////////
            }

            // close select element
            html.Append(input.ToString(TagRenderMode.EndTag));

            // add hidden tag, so that a value always gets sent for select tags
            var hidden1 = new TagBuilder("input");
            hidden1.Attributes.Add("type", "hidden");
            hidden1.Attributes.Add("id", inputName + "_hidden");
            hidden1.Attributes.Add("name", inputName);
            hidden1.Attributes.Add("value", string.Empty);
            html.Append(hidden1.ToString(TagRenderMode.SelfClosing));

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


        public string GetControlClass()
        {

            StringBuilder ControlClass = new StringBuilder();

            ControlClass.Append("validate[");


            if (_IsRequired == true)
            {

                ControlClass.Append("required");

            }
            ControlClass.Append("]");

            return ControlClass.ToString();

        }
        public void AddChoices(string choices, string delimiter)
        {

            if (string.IsNullOrEmpty(choices)) return;

            choices.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList()
                .ForEach(c => _choices.Add(c, false));




        }
    }
}
