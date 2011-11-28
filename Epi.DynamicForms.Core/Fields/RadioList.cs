using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents a list of html radio button inputs.
    /// </summary>
    [Serializable]
    public class RadioList : OrientableField
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

            // list of radio buttons            
            var ul = new TagBuilder("ul");
            ul.Attributes.Add("class", _orientation == Orientation.Vertical ? _verticalClass : _horizontalClass);
            ul.Attributes["class"] += " " + _listClass;
            html.Append(ul.ToString(TagRenderMode.StartTag));

            var choicesList = _choices.ToList();
            for (int i = 0; i < choicesList.Count; i++)
            {
                string radId = inputName + i;

                // open list item
                var li = new TagBuilder("li");
                html.Append(li.ToString(TagRenderMode.StartTag));

                // radio button input
                var rad = new TagBuilder("input");
                rad.Attributes.Add("type", "radio");
                rad.Attributes.Add("name", inputName);
                rad.Attributes.Add("id", radId);
                rad.Attributes.Add("value", choicesList[i].Key);
                if (choicesList[i].Value) rad.Attributes.Add("checked", "checked");
                rad.MergeAttributes(_inputHtmlAttributes);
                html.Append(rad.ToString(TagRenderMode.SelfClosing));

                // checkbox label
                var lbl = new TagBuilder("label");
                lbl.Attributes.Add("for", radId);
                lbl.Attributes.Add("class", _inputLabelClass);
                lbl.SetInnerText(choicesList[i].Key);
                html.Append(lbl.ToString());

                // close list item
                html.Append(li.ToString(TagRenderMode.EndTag));
            }

            html.Append(ul.ToString(TagRenderMode.EndTag));
                     

            var wrapper = new TagBuilder(_fieldWrapper);
            wrapper.Attributes["class"] = _fieldWrapperClass;
            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
        }
    }
}