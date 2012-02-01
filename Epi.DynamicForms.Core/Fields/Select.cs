using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html select element.
    /// </summary>
    [Serializable]
    public class Select : ListField
    {
        /// <summary>
        /// The number of options to display at a time.
        /// </summary>
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
            var html = new StringBuilder();
            var inputName = _form.FieldPrefix + _key;
            string ErrorStyle = string.Empty;
            // prompt
            var prompt = new TagBuilder("label");
            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", "EpiLabel");

            StringBuilder StyleValues = new StringBuilder();
            StyleValues.Append(GetContolStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString()));
            prompt.Attributes.Add("style", StyleValues.ToString());
            html.Append(prompt.ToString());

            // error label
            if (!IsValid)
            {
                //Add new Error to the error Obj

                ErrorStyle = ";border-color: red";
             
            }

            // open select element
            var select = new TagBuilder("select");
            select.Attributes.Add("id", inputName);
            select.Attributes.Add("name", inputName);



         

            if (_IsRequired == true)
            {
                select.Attributes.Add("class", "validate[required] text-input");
                select.Attributes.Add("data-prompt-position", "topRight:10");
            }
            select.Attributes.Add("style", "position:absolute;left:" + _left.ToString() + "px;top:" + _top.ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px"  + ErrorStyle);
            select.MergeAttributes(_inputHtmlAttributes);
            html.Append(select.ToString(TagRenderMode.StartTag));
            // If readonly then add the following jquery script to make the field disabled 
            if (ReadOnly)
            {
                var scriptReadOnlyText = new TagBuilder("script");
                scriptReadOnlyText.InnerHtml = "$(function(){$('#" + inputName + "').attr('disabled','disabled')});";
                html.Append(scriptReadOnlyText.ToString(TagRenderMode.Normal));
            }

            // initial empty option
            if (ShowEmptyOption)
            {
                var opt = new TagBuilder("option");
                opt.Attributes.Add("value", null);
                opt.SetInnerText(EmptyOption);
                html.Append(opt.ToString());
            }

            // options

            switch (this.SelectType.ToString())
            {
                case "11":
                    foreach (var choice in _choices)
                    {
                        var opt = new TagBuilder("option");
                        var optSelectedVale = "";
                        if (!string.IsNullOrEmpty(SelectedValue.ToString()))
                        {
                         optSelectedVale = SelectedValue.ToString()=="1"? "Yes" : "No";
                        }
                        opt.Attributes.Add("value",( choice.Key =="Yes"? "1":"0"));
                        if (choice.Key == optSelectedVale.ToString()) opt.Attributes.Add("selected", "selected");
                        opt.SetInnerText(choice.Key);
                        html.Append(opt.ToString());
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
                        opt.Attributes.Add("value", choice.Key.Remove(choice.Key.IndexOf("-")));
                        if (choice.Key.Remove(choice.Key.IndexOf("-")) == SelectedValue.ToString()) opt.Attributes.Add("selected", "selected");
                        opt.SetInnerText(choice.Key.Substring(choice.Key.IndexOf("-") + 1));
                        html.Append(opt.ToString());
                    }
                    break;
            }
 
            // close select element
            html.Append(select.ToString(TagRenderMode.EndTag));

            // add hidden tag, so that a value always gets sent for select tags
            var hidden = new TagBuilder("input");
            hidden.Attributes.Add("type", "hidden");
            hidden.Attributes.Add("id", inputName + "_hidden");
            hidden.Attributes.Add("name", inputName);
            hidden.Attributes.Add("value", string.Empty);          
            html.Append(hidden.ToString(TagRenderMode.SelfClosing));

            var wrapper = new TagBuilder(_fieldWrapper);
            wrapper.Attributes["class"] = _fieldWrapperClass;
            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
        }
    }
}
