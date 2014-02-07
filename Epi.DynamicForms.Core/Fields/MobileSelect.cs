using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using Epi.Core.EnterInterpreter;
using System.Drawing;

namespace MvcDynamicForms.Fields
{
    [Serializable]
    public class MobileSelect : Select
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

        public string EmptyOption { get; set; }
        public bool ShowEmptyOption { get; set; }

        public override string RenderHtml()
        {
            var html = new StringBuilder();
            var inputName = _fieldPrefix + _key;
            string ErrorStyle = string.Empty;
            var prompt = new TagBuilder("label");

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\r\n|\r|\n)+");
            string newText = regex.Replace(Prompt.Replace("  ", "&nbsp;"), "<br />");
            string NewPromp = System.Web.Mvc.MvcHtmlString.Create(newText).ToString();

            prompt.InnerHtml = NewPromp;
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", "select");
            prompt.Attributes.Add("Id", "label" + inputName);

            StringBuilder StyleValues = new StringBuilder();
            prompt.Attributes.Add("style", "display:block !important; ");
            html.Append(prompt.ToString());

            if (!IsValid)
            {
                ErrorStyle = ";border-color: red";
            }

            var select = new TagBuilder("select");
            select.Attributes.Add("id", inputName);
            select.Attributes.Add("name", inputName);

            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {
                select.Attributes.Add("onchange", "return " + _key + "_after(this.id);"); //After
            }

            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {
                select.Attributes.Add("onfocus", "return " + _key + "_before(this.id);"); //Before
            }

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
            
            Font stringFont = new Font(ControlFontStyle, _ControlFontSize);

            SizeF size = new SizeF();
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                size = g.MeasureString(measureString.ToString(), stringFont);
            }

            if (_IsRequired == true)
            {
                if ((size.Width) > _ControlWidth)
                {
                    select.Attributes.Add("class", "validate[required] text-input fix-me");
                }
                else
                {
                    select.Attributes.Add("class", "validate[required] text-input");
                }
                select.Attributes.Add("data-prompt-position", "topLeft:10");
            }
            else
            {
                if ((size.Width) > _ControlWidth)
                {
                    select.Attributes.Add("class", "fix-me");
                }
            }
            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";

            if (_IsDisabled)
            {
                select.Attributes.Add("disabled", "disabled");
            }

            select.Attributes.Add("style", "" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
            select.MergeAttributes(_inputHtmlAttributes);
            html.Append(select.ToString(TagRenderMode.StartTag));

            if (ReadOnly)
            {
                var scriptReadOnlyText = new TagBuilder("script");
                scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            }

            if (ShowEmptyOption)
            {
                var opt = new TagBuilder("option");
                opt.Attributes.Add("value", null);
                opt.SetInnerText(EmptyOption);
                html.Append(opt.ToString());
            }

            switch (this.SelectType.ToString())
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
                            opt.Attributes.Add("value", choice.Key.Remove(choice.Key.IndexOf("-")));

                            if (choice.Value || choice.Key.Remove(choice.Key.IndexOf("-")) == SelectedValue.ToString())
                            {
                                opt.Attributes.Add("selected", "selected");
                            }

                            opt.SetInnerText(choice.Key.Substring(choice.Key.IndexOf("-") + 1));
                        }
                        html.Append(opt.ToString());
                    }
                    break;
            }

            html.Append(select.ToString(TagRenderMode.EndTag));
            var hidden = new TagBuilder("input");
            hidden.Attributes.Add("type", "hidden");
            hidden.Attributes.Add("id", inputName + "_hidden");
            hidden.Attributes.Add("name", inputName);
            hidden.Attributes.Add("value", string.Empty);
            html.Append(hidden.ToString(TagRenderMode.SelfClosing));

            var wrapper = new TagBuilder(_fieldWrapper);

            if (!IsValid)
            {
                wrapper.Attributes["class"] = _fieldWrapperClass + " SelectNotValid";
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
        public string GetStyle(bool _IsHidden, bool _IsHighlighted)
        {
            string Style = "";
            if (_IsHidden)
            {
                Style += " display:none";

            }
            if (_IsHighlighted)
            {
                Style += " background-color:yellow";
            }

            return  Style;
        }
    }
}
