using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents a single html checkbox input field.
    /// </summary>
    [Serializable]
    public class CheckBox : InputField
    {
        private string _checkedValue = "Yes";
        private string _uncheckedValue = "No";
        new private string _promptClass = "MvcDynamicCheckboxPrompt";

        /// <summary>
        /// The text to be used as the user's response when they check the checkbox.
        /// </summary>
        public string CheckedValue
        {
            get
            {
                return _checkedValue;
            }
            set
            {
                _checkedValue = value;
            }
        }
        /// <summary>
        /// The text to be used as the user's response when they do not check the checkbox.
        /// </summary>
        public string UncheckedValue
        {
            get
            {
                return _uncheckedValue;
            }
            set
            {
                _uncheckedValue = value;
            }
        }
        /// <summary>
        /// The state of the checkbox.
        /// </summary>
        public bool Checked { get; set; }

        public override string Response
        {
            get
            {
                return Checked ? _checkedValue : _uncheckedValue;
            }
            set
            {
               
            }
        }

        public override bool Validate()
        {
            if (Required && !Checked)
            {
                // Isn't valid
                Error = _requiredMessage;
                return false;
            }

            // Is Valid
            ClearError();
            return true;
        }

        public override string RenderHtml()
        {
            var inputName = _form.FieldPrefix + _key;
            var html = new StringBuilder();


            // error label
            if (!IsValid)
            {
                var error = new TagBuilder("label");
                error.SetInnerText(Error);
                error.Attributes.Add("for", inputName);
                error.Attributes.Add("class", _errorClass);
                html.Append(error.ToString());
            }

            // checkbox input
            var chk = new TagBuilder("input");
            chk.Attributes.Add("id", inputName);
            chk.Attributes.Add("name", inputName);
            chk.Attributes.Add("type", "checkbox");
            if (Checked) chk.Attributes.Add("checked", "checked");
            chk.Attributes.Add("value", bool.TrueString);
            chk.MergeAttributes(_inputHtmlAttributes);
            html.Append(chk.ToString(TagRenderMode.SelfClosing));

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.SetInnerText(Prompt);
            prompt.Attributes.Add("for", inputName);
            prompt.Attributes.Add("class", _promptClass);
            html.Append(prompt.ToString());

            // hidden input (so that value is posted when checkbox is unchecked)
            var hdn = new TagBuilder("input");
            hdn.Attributes.Add("type", "hidden");
            hdn.Attributes.Add("id", inputName + "_hidden");
            hdn.Attributes.Add("name", inputName);
            hdn.Attributes.Add("value", bool.FalseString);
            html.Append(hdn.ToString(TagRenderMode.SelfClosing));


            var wrapper = new TagBuilder(_fieldWrapper);
            wrapper.Attributes["class"] = _fieldWrapperClass;
            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
        }
    }
}
