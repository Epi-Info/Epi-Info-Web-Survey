using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
namespace MvcDynamicForms.Fields
    {
    [Serializable]
    public class MobileRelateButton : InputField
        { 

        new private string _promptClass = "MvcDynamicCommandButtonPrompt";
        public string RelatedViewId;
        public override string RenderHtml()
            {
            string name = "mvcdynamicfield_" + _key;
            var html = new StringBuilder();
            string ErrorStyle = string.Empty;

            var commandButtonTag = new TagBuilder("button");

            //commandButtonTag.Attributes.Add("text", Prompt);
            // <button data-role="button" data-theme="submit2" data-inline="true" type="submit"  name="Submitbutton" value="Submit" >
            commandButtonTag.InnerHtml = Prompt;
            commandButtonTag.Attributes.Add("id", name);
            commandButtonTag.Attributes.Add("name", "Relate");
            commandButtonTag.Attributes.Add("data-role", "button");
            commandButtonTag.Attributes.Add("data-inline", "true");
            commandButtonTag.Attributes.Add("type", "submit");
            commandButtonTag.Attributes.Add("data-theme", "submit2");
            StringBuilder StyleValues = new StringBuilder();
            //StyleValues.Append(GetControlStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), _IsHidden));
            //commandButtonTag.Attributes.Add("style", StyleValues.ToString());

           commandButtonTag.Attributes.Add("onclick", "NavigateToChild(" + RelatedViewId + "); ");
       
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

            if (_IsDisabled)
                {
                commandButtonTag.Attributes.Add("disabled", "disabled");
                }

            // commandButtonTag.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _Width.ToString() + "px" + ";height:" + _Height.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
            //commandButtonTag.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + ControlWidth.ToString() + "px" + ";height:" + ControlHeight.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
            //commandButtonTag.Attributes.Add("style", "width:" + ControlWidth.ToString() + "px" + ";height:" + ControlHeight.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
            EnterRule FunctionObjectAfter = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + _key);
            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
                {
                commandButtonTag.Attributes.Add("onblur", "return " + _key + "_after();"); //After
                }
            EnterRule FunctionObjectBefore = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=before&identifier=" + _key);
            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
                {
                commandButtonTag.Attributes.Add("onfocus", "return " + _key + "_before();"); //Before
                }
            EnterRule FunctionObjectClick = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=click&identifier=" + _key);
            if (FunctionObjectClick != null && !FunctionObjectClick.IsNull())
                {
                commandButtonTag.Attributes.Add("onclick", "return " + _key + "_click(); ");
                }

            //   html.Append(commandButtonTag.ToString(TagRenderMode.SelfClosing));
            html.Append(commandButtonTag.ToString());
            var scriptBuilder = new TagBuilder("script");
           // scriptBuilder.InnerHtml = "$('#" + name + "').BlockEnter('" + name + "'); $('#" + name + "').on('click','#" + name + "' , function () {NavigateToChild(" + RelatedViewId + ");}');";
            scriptBuilder.ToString(TagRenderMode.Normal);
            html.Append(scriptBuilder.ToString(TagRenderMode.Normal));

            var wrapper = new TagBuilder(_fieldWrapper);
            wrapper.Attributes["class"] = _fieldWrapperClass;
            if (_IsHidden)
                {
                wrapper.Attributes["style"] = "display:none";

                }
            wrapper.Attributes["id"] = name + "_fieldWrapper";
            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
            }

        public override bool Validate()
            {
            ClearError();
            return true;
            }
        public string Value { get; set; }
        public override string Response
            {
            get { return Value; }
            set { Value = value; }
            }

        public  string GetControlStyle(string ControlFontStyle, string Top, string Left, string Width, string Height, bool IsHidden)
        {
            StringBuilder FontStyle = new StringBuilder();
            StringBuilder FontWeight = new StringBuilder();
            StringBuilder TextDecoration = new StringBuilder();
            StringBuilder CssStyles = new StringBuilder();

            char[] delimiterChars = { ' ', ',' };
            string[] Styles = ControlFontStyle.Split(delimiterChars);
            // CssStyles.Append("width: auto");

            foreach (string Style in Styles)
            {
                switch (Style.ToString())
                {
                    case "Italic":
                        FontStyle.Append(Style.ToString());
                        break;
                    case "Oblique":
                        FontStyle.Append(Style.ToString());
                        break;
                }

            }

            foreach (string Style in Styles)
            {
                switch (Style.ToString())
                {
                    case "Bold":
                        FontWeight.Append(Style.ToString());
                        break;
                    case "Normal":
                        FontWeight.Append(Style.ToString());
                        break;
                }
            }

            CssStyles.Append(" font:");//1

            if (!string.IsNullOrEmpty(FontStyle.ToString()))
            {
                CssStyles.Append(FontStyle);//2
                CssStyles.Append(" ");//3
            }

            CssStyles.Append(FontWeight);
            CssStyles.Append(" ");
            CssStyles.Append(_fontSize.ToString() + "pt ");
            CssStyles.Append(" ");
            CssStyles.Append(_fontfamily.ToString());

            foreach (string Style in Styles)
            {
                switch (Style.ToString())
                {
                    case "Strikeout":
                        TextDecoration.Append("line-through");
                        break;
                    case "Underline":
                        TextDecoration.Append(Style.ToString());
                        break;
                }
            }

            if (!string.IsNullOrEmpty(TextDecoration.ToString()))
            {
                CssStyles.Append(";text-decoration:");
            }

            if (IsHidden)
            {
                CssStyles.Append(";display:none");
            }

            CssStyles.Append(TextDecoration);

            return CssStyles.ToString();
        }
        }
    }
