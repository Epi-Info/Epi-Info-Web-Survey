using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
using System.Web.UI;

namespace MvcDynamicForms.Fields
{
    [Serializable]
    public class MobileRadioList : RadioList
    {
        public override string RenderHtml()
        {
            var html = new StringBuilder();
            var inputName = _fieldPrefix + _key;
            List<KeyValuePair<string, bool>> choiceList = ChoiceKeyValuePairs.ToList();
            var selectedValue = string.Empty;
            var IsAfterControl = false;
            
            if (!IsValid)
            {
                var error = new TagBuilder("label");
                error.Attributes.Add("class", _errorClass);
                error.SetInnerText(Error);
                html.Append(error.ToString());
            }
            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";

            if (_IsHighlighted)
            {
                IsHighlightedStyle = "background:yellow";
            }

            var fieldset = new TagBuilder("fieldset");
            fieldset.Attributes.Add("data-role", "controlgroup");
            
            html.Append(fieldset.ToString(TagRenderMode.StartTag));

            var legend = new TagBuilder("legend");
              
            legend.SetInnerText(Prompt);
            //StringBuilder StyleValues = new StringBuilder();
           // StyleValues.Append(GetControlStyle(_fontstyle.ToString(), _Prompttop.ToString(), _Promptleft.ToString(), null, Height.ToString(), IsHidden));
            //legend.Attributes.Add("style", StyleValues.ToString());
            html.Append(legend.ToString());

            for (int i = 0; i < choiceList.Count; i++)
            {
                double innerTop = 0.0;
                double innerLeft = 0.0;
                string radId = inputName + i;

                if ((Locations.Count) == choiceList.Count)
                {
                    List<string> TopLeft = Locations[i].ToString().Split(':').ToList();

                    if (TopLeft.Count > 0)
                    {
                        innerTop = ParseDouble(TopLeft[0]) * Height;
                        innerLeft = ParseDouble(TopLeft[1]) * Width;
                    }
                }

                var radioTag = new TagBuilder("input");
                radioTag.Attributes.Add("type", "radio");
                radioTag.Attributes.Add("name", inputName);
                radioTag.Attributes.Add("class", inputName);
                radioTag.Attributes.Add("id", radId);                               
           
                if (FunctionObjectAfter != null && ((Epi.Core.EnterInterpreter.Rules.Rule_Begin_After_Statement)FunctionObjectAfter).Statements != null)
                {
                    //radioTag.Attributes.Add("onclick", "return " + _key + "_after();");
                    radioTag.Attributes.Add("onchange", "$('#" + inputName + "').val('" + i.ToString() + "');return " + _key + "_after(this.id);"); //After
                    IsAfterControl = true;
                }

                //   if (FunctionObjectClick != null && !FunctionObjectClick.IsNull())
                if (FunctionObjectClick != null)
                {
                    //radioTag.Attributes.Add("onclick", "return " + _key + "_after();");
                    radioTag.Attributes.Add("onclick", "return " + _key + "_click(this.id);"); //click
                    IsAfterControl = true;
                }
                if (!IsAfterControl)
                {
                    radioTag.Attributes.Add("onchange", "$('#" + inputName + "').val('" + i.ToString() + "');"); //click
                }
                radioTag.SetInnerText(choiceList[i].Key);
                radioTag.Attributes.Add("value", i.ToString());

                if (_IsDisabled)
                {
                    radioTag.Attributes.Add("disabled", "disabled");
                }

                if (Response == choiceList[i].Key)
                {
                    radioTag.Attributes.Add("checked", "checked");
                    selectedValue = i.ToString();
                }

                radioTag.MergeAttributes(_inputHtmlAttributes);
                html.Append(radioTag.ToString(TagRenderMode.SelfClosing));

                var rightlbl = new TagBuilder("label");
                rightlbl.Attributes.Add("for", radId);
                rightlbl.Attributes.Add("class", "label" + inputName);
                rightlbl.SetInnerText(choiceList[i].Key);
                html.Append(rightlbl.ToString());
            }
          
            html.Append(fieldset.ToString(TagRenderMode.EndTag));

            var hidden = new TagBuilder("input");
            hidden.Attributes.Add("type", "hidden");
            hidden.Attributes.Add("id", inputName);
            hidden.Attributes.Add("name", inputName);

            hidden.Attributes.Add("value", selectedValue);
            html.Append(hidden.ToString(TagRenderMode.SelfClosing));

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