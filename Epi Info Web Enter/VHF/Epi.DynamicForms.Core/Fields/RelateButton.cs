using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
namespace MvcDynamicForms.Fields
    {
    [Serializable]
    public class RelateButton : InputField
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
            commandButtonTag.InnerHtml = Prompt;
            commandButtonTag.Attributes.Add("id", name);
            commandButtonTag.Attributes.Add("name", "Relate");
            //commandButtonTag.Attributes.Add("name", name);
            commandButtonTag.Attributes.Add("type", "button");
             
            commandButtonTag.Attributes.Add("onclick", "NavigateToChild(" + RelatedViewId + ");");
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
            commandButtonTag.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + ControlWidth.ToString() + "px" + ";height:" + ControlHeight.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
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
                commandButtonTag.Attributes.Add("onclick", "return " + _key + "_click();");
                }

         //   html.Append(commandButtonTag.ToString(TagRenderMode.SelfClosing));
            html.Append(commandButtonTag.ToString());
            var scriptBuilder = new TagBuilder("script");
            scriptBuilder.InnerHtml = "$('#" + name + "').BlockEnter('" + name + "');";
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
        }
    }
