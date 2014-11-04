using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html textarea element.
    /// </summary>
    [Serializable]
    public class MobileTextArea : TextField
    {
        public override string RenderHtml()
        { 
            var html = new StringBuilder();
            var inputName = _form.FieldPrefix + _key;
            string ErrorStyle = string.Empty;
            //prompt label
            var prompt = new TagBuilder("label");

            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", "EpiLabel");
            prompt.Attributes.Add("Id", "label" + inputName);
            StringBuilder StyleValues = new StringBuilder();
            StyleValues.Append(GetContolStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), IsHidden));
            
            prompt.Attributes.Add("style", "");
            html.Append(prompt.ToString());
            // error label
            if (!IsValid)
            {
                //Add new Error to the error Obj

                 ErrorStyle = ";border-color: red";

            }

            // input element
            var txt = new TagBuilder("textarea");
            txt.Attributes.Add("name", inputName);
            txt.Attributes.Add("id", inputName);
            // txt.SetInnerText(Value);
            ////////////Check code start//////////////////
            EnterRule FunctionObjectAfter = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + _key);
            if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
            {
                txt.Attributes.Add("onblur", "return " + _key + "_after(this.id);"); //After
            }
            EnterRule FunctionObjectBefore = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=before&identifier=" + _key);
            if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
            {
                txt.Attributes.Add("onfocus", "return " + _key + "_before(this.id);"); //Before
            }

            ////////////Check code end//////////////////
            txt.SetInnerText(Value);
            if (_IsRequired == true)
            {
                txt.Attributes.Add("class", "validate[required] text-input");
                txt.Attributes.Add("data-prompt-position", "topRight:15");
            }
            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";
            //if (_IsHidden)
            //{
            //    IsHiddenStyle = "display:none";
            //}
            if (_IsHighlighted)
            {
                IsHighlightedStyle = "background-color:yellow";
            }

            //if (_IsDisabled)
            //{
            //    txt.Attributes.Add("disabled", "disabled");
            //}
            //txt.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px" + ";height:" + _ControlHeight.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
           // txt.Attributes.Add("style", "height:" + _ControlHeight.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
            txt.Attributes.Add("style", "height:" + "100" + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
            
            txt.MergeAttributes(_inputHtmlAttributes);
            html.Append(txt.ToString());


            // If readonly then add the following jquery script to make the field disabled 
            if (ReadOnly || _IsDisabled)
                {
                var scriptReadOnlyText = new TagBuilder("script");
                //scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                scriptReadOnlyText.InnerHtml = "$(function(){  var List = new Array();List.push('" + _key + "');CCE_Disable(List, false);});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
                }

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