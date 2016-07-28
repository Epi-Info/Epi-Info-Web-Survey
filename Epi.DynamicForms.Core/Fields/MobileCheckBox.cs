using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents a single html checkbox input field.
    /// </summary>
    [Serializable]
    public class MobileCheckBox : CheckBox
    {
        public override string RenderHtml()
        {
            var inputName = _fieldPrefix + _key;
            var html = new StringBuilder();
            string ErrorStyle = string.Empty;

            if (!IsValid)
            {
                ErrorStyle = ";border-color: red";
            }

            var checkboxTag = new TagBuilder("input");
            checkboxTag.Attributes.Add("id", inputName);
            checkboxTag.Attributes.Add("name", inputName);
            checkboxTag.Attributes.Add("type", "checkbox");

            if (Checked)
            {
                checkboxTag.Attributes.Add("checked", "checked");
            }

            checkboxTag.Attributes.Add("value", bool.TrueString);
            
            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";

            if (_IsHighlighted)
            {
                IsHighlightedStyle = "background-color:yellow";
            }
            
            //if (_IsDisabled)
            //{
            //    checkboxTag.Attributes.Add("disabled", "disabled");
            //}

            checkboxTag.Attributes.Add("style", "" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
            checkboxTag.MergeAttributes(_inputHtmlAttributes);

            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {
                //checkboxTag.Attributes.Add("onblur", "return " + _key + "_after(this.id);"); //After
                checkboxTag.Attributes.Add("onclick", "return " + _key + "_after(this.id);");
            }

            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {
                //checkboxTag.Attributes.Add("onfocus", "return " + _key + "_before(this.id);"); //Before
                checkboxTag.Attributes.Add("onclick", "return " + _key + "_before(this.id);");
            }

            if (FunctionObjectClick != null && !FunctionObjectClick.IsNull())
            {
                checkboxTag.Attributes.Add("onclick", "return " + _key + "_click(this.id);");
            }

            html.Append(checkboxTag.ToString(TagRenderMode.SelfClosing));

            var prompt = new TagBuilder("label");
            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", "EpiLabel");
            prompt.Attributes.Add("Id", "label" + inputName);
            html.Append(prompt.ToString());

            //if (ReadOnly)
            //{
            //    var scriptReadOnlyText = new TagBuilder("script");
            //    scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
            //    html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            //}
            if (ReadOnly || _IsDisabled)
                {
                var scriptReadOnlyText = new TagBuilder("script");
                //scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                scriptReadOnlyText.InnerHtml = "$(function(){  var List = new Array();List.push('" + _key + "');CCE_Disable(List, false);});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
                }
            var hdn = new TagBuilder("input");
            hdn.Attributes.Add("type", "hidden");
            hdn.Attributes.Add("id", inputName + "_hidden");
            hdn.Attributes.Add("name", inputName);
            hdn.Attributes.Add("value", bool.FalseString);
            html.Append(hdn.ToString(TagRenderMode.SelfClosing));

            //prevent check box control to submit on enter click
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
    }
}
