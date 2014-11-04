using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;

namespace MvcDynamicForms.Fields
{
     [Serializable]
    public class MobileTextBox : TextField
    {

         public override string RenderHtml()
         { 
             var html = new StringBuilder();
             var inputName = _form.FieldPrefix + _key;
             string ErrorStyle = string.Empty;
             //Jquery Mobile
             //var Div = new TagBuilder("Div");
             //Div.Attributes.Add("data-role", "fieldcontain");
             //html.Append(Div.ToString());

             // prompt label
             var prompt = new TagBuilder("label");

             prompt.SetInnerText(Prompt);
             prompt.Attributes.Add("for", inputName);
             prompt.Attributes.Add("class", "EpiLabel");
             prompt.Attributes.Add("Id", "label" + inputName);
             StringBuilder StyleValues = new StringBuilder();
             //StyleValues.Append(GetContolStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), _IsHidden));
             //prompt.Attributes.Add("style", StyleValues.ToString());
             //prompt.Attributes.Add("style", "width: auto; white-space: nowrap");
             prompt.Attributes.Add("style", "width: auto");
             html.Append(prompt.ToString());

             // error label
             if (!IsValid)
             {
                 //Add new Error to the error Obj
                 ErrorStyle = ";border-color: red";
             }

             // input element
             var txt = new TagBuilder("input");
             txt.Attributes.Add("name", inputName);
             txt.Attributes.Add("id", inputName);
             txt.Attributes.Add("type", "text");

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

             txt.Attributes.Add("value", Value);
             if (_IsRequired == true)
             {
                 //txt.Attributes.Add("class", "validate[required] text-input");
                 txt.Attributes.Add("class", "validate[required]  ");
                txt.Attributes.Add("data-prompt-position", "topLeft:15");
             }

             if (_MaxLength > 0 && _MaxLength <= 255)
             {
                 txt.Attributes.Add("MaxLength", _MaxLength.ToString());
             }
             else
             {
                 txt.Attributes.Add("MaxLength", "255");
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
            // txt.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle);
           //  txt.Attributes.Add("style", "" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle + ";color:#111; background-color:white" + ";width:" + _ControlWidth.ToString() + "px");

            // txt.Attributes.Add("style", "" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle  + ";width:" + _ControlWidth.ToString() + "px");
            // txt.Attributes.Add("style", "" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle + ";width: auto");
             txt.Attributes.Add("style", "" + ErrorStyle + ";" + IsHiddenStyle + ";" + IsHighlightedStyle );
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

             //prevent text box control to submit on enter click
             var scriptBuilder = new TagBuilder("script");
             scriptBuilder.InnerHtml = "$('#" + inputName + "').BlockEnter('" + inputName + "');";
             scriptBuilder.ToString(TagRenderMode.Normal);
             html.Append(scriptBuilder.ToString(TagRenderMode.Normal));

             var wrapper = new TagBuilder(_fieldWrapper);
             wrapper.Attributes["class"] = _fieldWrapperClass;
             //wrapper.Attributes.Add("data-role", "fieldcontain");
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
