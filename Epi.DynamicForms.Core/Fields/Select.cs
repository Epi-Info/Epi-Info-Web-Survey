using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using  System.Web;
using Epi.Core.EnterInterpreter;
using System.Drawing;

namespace MvcDynamicForms.Fields
{
    [Serializable]
    public class Select : ListField
    {
        
        public int Size
        {
            get
            {
                string size;
                return _inputHtmlAttributes.TryGetValue("size", out size) ? int.Parse(size) : 1;
            }
            set { _inputHtmlAttributes["size"] = value.ToString(); }
        }

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
        public int SelectType { get; set; }
        public string EmptyOption { get; set; }
        public bool ShowEmptyOption { get; set; }
       
        public override string RenderHtml()
        {
            var html = new StringBuilder();

            var inputName = _fieldPrefix + _key;
            string ErrorStyle = string.Empty;
            
            var prompt = new TagBuilder("label");
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\r\n|\r|\n)+");
            string newText = regex.Replace(Prompt.Replace(" ", "&nbsp;"), "<br />");
            string NewPromp = System.Web.Mvc.MvcHtmlString.Create(newText).ToString();

            prompt.InnerHtml=NewPromp;
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", "EpiLabel");
            prompt.Attributes.Add("Id", "label" + inputName);
            
            StringBuilder StyleValues = new StringBuilder();
            StyleValues.Append(GetControlStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(),_IsHidden));
            prompt.Attributes.Add("style", StyleValues.ToString());
            html.Append(prompt.ToString());

            if (!IsValid)
            {
                ErrorStyle = ";border-color: red";
            }

            //var select = new TagBuilder("select");
            TagBuilder select = null;
            if (this._choices.Count() < 100)
            {
                select = new TagBuilder("select");
            }
            else
            {

                select = new TagBuilder("input");
                select.Attributes.Add("list", inputName + "_DataList");
                select.Attributes.Add("data-autofirst", "true");
            }
            select.Attributes.Add("id", inputName);
            select.Attributes.Add("name", inputName);

            /*if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {
                select.Attributes.Add("onblur", "return " + _key + "_after();"); //After
            }
            if (FunctionObjectClick != null && !FunctionObjectClick.IsNull())
            {
                if( this.RelateCondition)
                {
                    select.Attributes.Add("onchange", "return SetCodes_Val(this,'" + "null" + "','" + _key + "');" + _key + "_click();"); //click
                }
                else
                {
                    select.Attributes.Add("onchange", "return " + _key + "_click();"); //click
                }
            }
            else {
                if (this.RelateCondition)
                {
                    select.Attributes.Add("onchange", "return SetCodes_Val(this,'" + "null"+ "','" + _key + "');"  ); //click
                }
            }
            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {
                select.Attributes.Add("onfocus", "return " + _key + "_before();"); //Before
            }*/
            ////////////Check code start//////////////////
            EnterRule FunctionObjectAfter = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + _key);
            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {

                select.Attributes.Add("onblur", "return " + _key + "_after();"); //After
                // select.Attributes.Add("onchange", "return " + _key + "_after();"); //After
            }
            EnterRule FunctionObjectBefore = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=before&identifier=" + _key);
            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {

                select.Attributes.Add("onfocus", "return " + _key + "_before();"); //Before
            }
            EnterRule FunctionObjectClick = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=click&identifier=" + _key);

            if (FunctionObjectClick != null && !FunctionObjectClick.IsNull())
            {
                select.Attributes.Add("onclick", "return " + _key + "_click();"); //click
            }
            if (this.RelateCondition)
            {
                select.Attributes.Add("onchange", "return SetCodes_Val(this,'" + _form.SurveyInfo.SurveyId + "','" + _key + "');"); //click


            }
            ////////////Check code end//////////////////
            int LargestChoiseLength =0 ;
            string measureString = "";
            
            foreach (var choice in _choices) 
            {
                if (choice.Key.ToString().Length > LargestChoiseLength) 
                {
                    LargestChoiseLength = choice.Key.ToString().Length;
                    measureString = choice.Key.ToString();
                } 
            }

            Font stringFont = new Font(ControlFontStyle, _ControlFontSize);

            SizeF size = new SizeF() ;
            
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                size = g.MeasureString(measureString.ToString(), stringFont);
            }

            if (Required == true)
            {
                if (this._choices.Count() < 100)
                {
                    if ((size.Width) > _ControlWidth)
                    {
                        select.Attributes.Add("class", GetControlClass() + "fix-me");
                    }
                    else
                    {
                        select.Attributes.Add("class", GetControlClass());
                    }

                    select.Attributes.Add("data-prompt-position", "topRight:10");
                }
                else
                {
                    if ((size.Width) > _ControlWidth)
                    {
                        select.Attributes.Add("class", GetControlClass() + "fix-me ");
                    }
                    else
                    {
                        select.Attributes.Add("class", GetControlClass() + " awesomplete");
                    }

                    select.Attributes.Add("data-prompt-position", "topRight:10");

                }
            }
            else
            {
                if (this._choices.Count() < 100)
                {
                    //select.Attributes.Add("class", GetControlClass() + "text-input fix-me");
                    if ((size.Width) > _ControlWidth)
                    {
                        select.Attributes.Add("class", GetControlClass() + "fix-me ");
                    }
                    else
                    {

                        select.Attributes.Add("class", GetControlClass());
                    }
                    select.Attributes.Add("data-prompt-position", "topRight:10");
                }
                else
                {
                    if ((size.Width) > _ControlWidth)
                    {
                        select.Attributes.Add("class", GetControlClass() + "fix-me awesomplete");
                    }
                    else
                    {

                        select.Attributes.Add("class", GetControlClass() + " awesomplete");
                    }
                    select.Attributes.Add("data-prompt-position", "topRight:10");

                }

            }


            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";

            if(_IsHidden)
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
            select.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px ; font-size:" + _ControlFontSize + "pt;" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle + ";" + InputFieldStyle);
            select.MergeAttributes(_inputHtmlAttributes);
            html.Append(select.ToString(TagRenderMode.StartTag));

            if (ReadOnly || _IsDisabled)
                {
                var scriptReadOnlyText = new TagBuilder("script");
                //scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                scriptReadOnlyText.InnerHtml = "$(function(){  var List = new Array();List.push('" + _key + "');CCE_Disable(List, false);});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
                }
            if (this._choices.Count() > 100)
            {

                var scriptReadOnlyText = new TagBuilder("script");
                StringBuilder Script = new StringBuilder();
                Script.Append("$(window).load(function () {  ");
                //Script.Append(" $( '#" + inputName + "' ).next().css( 'width', '" + _ControlWidth.ToString() + "px' );  ");
                Script.Append(" $( '#" + inputName + "' ).next().css( 'left', '" + _left.ToString() + "px' );  ");
                Script.Append(" $( '#" + inputName + "' ).next().css( 'top', '" + (_top + 20).ToString() + "px' );  ");
               
                Script.Append("});");
                scriptReadOnlyText.InnerHtml = Script.ToString();
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            }

            // initial empty option
            if (this._choices.Count() < 100)
            {

                if (ShowEmptyOption)
                {
                    var opt = new TagBuilder("option");
                    opt.Attributes.Add("value", null);
                    opt.SetInnerText(EmptyOption);
                    html.Append(opt.ToString());
                }
            }


            //options
            //Build codes relatecondition Script Object

            if (this._choices.Count() < 100 && this.SelectedValue.ToString() != "18")
            {
                switch (FieldTypeId.ToString())
                {
                    case "11":
                        foreach (var choice in _choices)
                        {
                            var opt = new TagBuilder("option");
                            var optSelectedVale = "";
                            if (!string.IsNullOrEmpty(SelectedValue.ToString()))
                            {
                                optSelectedVale = SelectedValue.ToString();//=="1"? "Yes" : "No";
                            }
                            opt.Attributes.Add("value", (choice.Key == "Yes" ? "1" : "0"));
                            if (choice.Key == optSelectedVale.ToString())
                            {
                                opt.Attributes.Add("selected", "selected");

                            }
                            if (choice.Key == "Yes" || choice.Key == "No")
                            {
                                opt.SetInnerText(choice.Key);
                                html.Append(opt.ToString());
                            }
                        }
                        break;
                    case "17":
                        foreach (var choice in _choices)
                        {
                            var opt = new TagBuilder("option");
                            opt.Attributes.Add("value", choice.Key);
                            if (choice.Key == SelectedValue.ToString()) opt.Attributes.Add("selected", "selected");
                            opt.SetInnerText(choice.Key);
                            html.Append(opt.ToString());
                        }

                        break;
                    case "18":
                        foreach (var choice in _choices)
                        {
                            var opt = new TagBuilder("option");
                            opt.Attributes.Add("value", choice.Key);
                            if (choice.Key == SelectedValue.ToString()) opt.Attributes.Add("selected", "selected");
                            opt.SetInnerText(choice.Key);
                            html.Append(opt.ToString());
                        }

                        break;
                    case "19":
                        foreach (var choice in _choices)
                        {
                            var opt = new TagBuilder("option");

                            if (choice.Key.Contains("-"))
                            {
                                string[] keyValue = choice.Key.Split(new char[] { '-' }, 2);
                                string comment = keyValue[0].Trim();
                                string description = keyValue[1].Trim();

                                opt.Attributes.Add("value", comment);

                                if (choice.Value || comment == SelectedValue.ToString())
                                {
                                    opt.Attributes.Add("selected", "selected");
                                }

                                opt.SetInnerText(description);
                            }

                            html.Append(opt.ToString());
                        }
                        break;
                }
            }
            else
            {
                
                var datalist = new TagBuilder("datalist ");
                datalist.Attributes.Add("id", inputName + "_DataList");
                html.Append(datalist.ToString(TagRenderMode.StartTag));
                foreach (var choice in _choices)
                {
                    var opt = new TagBuilder("option");
                    opt.Attributes.Add("value", choice.Key);
                    if (choice.Key == SelectedValue.ToString()) opt.Attributes.Add("selected", "selected");
                    {
                        opt.SetInnerText(choice.Key);
                    }
                    html.Append(opt.ToString());
                }

            }
 
            html.Append(select.ToString(TagRenderMode.EndTag));

            var hidden = new TagBuilder("input");
            hidden.Attributes.Add("type", "hidden");
            hidden.Attributes.Add("id", inputName + "_hidden");
            hidden.Attributes.Add("name", inputName);
            hidden.Attributes.Add("value", string.Empty);          
            html.Append(hidden.ToString(TagRenderMode.SelfClosing));

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

            if (Required == true)
            {
                ControlClass.Append("required");
            }

            ControlClass.Append("]");
            return ControlClass.ToString();
        }
    }
}
