using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
namespace MvcDynamicForms.Fields
{
    [Serializable]
    public class MobileCommandButton : CommandButton 
    {
        new private string _promptClass = "MvcDynamicCommandButtonPrompt";

        public override string RenderHtml()
        {
            string name = _fieldPrefix + _key;
            var html = new StringBuilder();
            string ErrorStyle = string.Empty;

            var commandButtonTag = new TagBuilder("input");
            commandButtonTag.Attributes.Add("value", Prompt);
            commandButtonTag.Attributes.Add("id", name);
            commandButtonTag.Attributes.Add("name", name);
            commandButtonTag.Attributes.Add("type", "button");          

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

            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {
                commandButtonTag.Attributes.Add("onblur", "return " + _key + "_after(this.id);"); //After
            }

            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {
                commandButtonTag.Attributes.Add("onfocus", "return " + _key + "_before(this.id);"); //Before
            }

            if (FunctionObjectClick != null && !FunctionObjectClick.IsNull())
            {
                commandButtonTag.Attributes.Add("onclick", "return " + _key + "_click(this.id);"); 
            }

            html.Append(commandButtonTag.ToString(TagRenderMode.SelfClosing));

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
