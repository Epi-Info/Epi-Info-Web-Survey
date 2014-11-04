using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System;
 
namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents a list of html checkbox inputs.
    /// </summary>
    [Serializable]
    public class CheckBoxList : OrientableField
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder();
            var inputName = _form.FieldPrefix + _key;

            // prompt label
            var prompt = new TagBuilder("label");
            prompt.Attributes.Add("class", _promptClass);
            prompt.SetInnerText(Prompt);
            html.Append(prompt.ToString());

            // error label
            if (!IsValid)
            {
                var error = new TagBuilder("label");
                error.Attributes.Add("class", _errorClass);
                error.SetInnerText(Error);
                html.Append(error.ToString());
            }

            // list of checkboxes
            var ul = new TagBuilder("ul");            
            ul.Attributes.Add("class", _orientation == Orientation.Vertical ? _verticalClass : _horizontalClass);
            ul.Attributes["class"] += " " + _listClass;
            html.Append(ul.ToString(TagRenderMode.StartTag));
            
            var choicesList = _choices.ToList();
            for (int i = 0; i < choicesList.Count; i++)
            {
                string chkId = inputName + i;

                // open list item
                var li = new TagBuilder("li");
                html.Append(li.ToString(TagRenderMode.StartTag));

                // checkbox input
                var chk = new TagBuilder("input");
                chk.Attributes.Add("type", "checkbox");
                chk.Attributes.Add("name", inputName);
                chk.Attributes.Add("id", chkId);
                chk.Attributes.Add("value", choicesList[i].Key);
                if (choicesList[i].Value) chk.Attributes.Add("checked", "checked");
                chk.MergeAttributes(_inputHtmlAttributes);
                html.Append(chk.ToString(TagRenderMode.SelfClosing));
                
                // checkbox label
                var lbl = new TagBuilder("label");
                lbl.Attributes.Add("for", chkId);
                lbl.Attributes.Add("class", _inputLabelClass);
                lbl.SetInnerText(choicesList[i].Key);
                html.Append(lbl.ToString());

                // close list item
                html.Append(li.ToString(TagRenderMode.EndTag));
            }

            html.Append(ul.ToString(TagRenderMode.EndTag));

            // add hidden tag, so that a value always gets sent
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
