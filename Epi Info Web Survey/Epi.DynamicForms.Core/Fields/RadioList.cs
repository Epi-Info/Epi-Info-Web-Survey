﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
using System.Web.UI;

namespace MvcDynamicForms.Fields
{
    [Serializable]
    public class RadioList : OrientableField
    {
        private string _options_position_string;

        public string Options_Positions
        {
            set
            {
                _options_position_string = value;

                string pipeString = _options_position_string.Replace("||", "|");
                List<string> lists = pipeString.Split('|').ToList<string>();
                string options = lists[0];
                string locations = lists[1];

                ChoiceKeyValuePairs.Clear();
                foreach (var option in options.Split(',').ToList<string>())
                {
                    ChoiceKeyValuePairs.Add(option, false);
                }

                Locations = locations.Split(',').ToList<string>();
            }
        }

        public override string RenderHtml()
        {
            StringBuilder html = new StringBuilder();
            string inputName = _fieldPrefix + _key;
            List<KeyValuePair<string, bool>> choiceList = ChoiceKeyValuePairs.ToList();

            if (!IsValid)
            {
                var error = new TagBuilder("label");
                error.Attributes.Add("class", _errorClass);
                error.SetInnerText(Error);
                html.Append(error.ToString());
            }
            
            string IsHiddenStyle = "";
            string IsHighlightedStyle = "";

            if (_IsHidden)
            {
            }
            
            if (_IsHighlighted)
            {
                IsHighlightedStyle = "background:yellow";
            }

            if (choiceList.Count == Locations.Count + 1)
            {
                string strindex = choiceList[Locations.Count].Key;
                bool bodex = choiceList[Locations.Count].Value;
                int index;

                if (int.TryParse(strindex, out index))
                {
                    KeyValuePair<string, bool> newKvp = new KeyValuePair<string, bool>(choiceList[index].Key,bodex);
                    choiceList[index] = newKvp;
                }
            }
            
            for (int i = 0; i < Locations.Count; i++)
            {
                double innerTop = 0.0;
                double innerLeft = 0.0;
                string radId = inputName + i;

                List<string> TopLeft = Locations[i].ToString().Split(':').ToList();

                if (TopLeft.Count > 0)
                {
                    innerTop = double.Parse(TopLeft[0]) * Height;
                    innerLeft = double.Parse(TopLeft[1]) * Width;
                }

                TagBuilder divTag = new TagBuilder("div");
                divTag.Attributes.Add("class", _orientation == Orientation.Vertical ? _verticalClass : _horizontalClass);
                divTag.Attributes["class"] += " " + _listClass;
                divTag.Attributes.Add("style", "position:absolute; left:" + (_left + innerLeft) + "px;top:" + (_top + innerTop) + "px" + ";width:" + _ControlWidth.ToString() + "px" + ";height:" + _ControlHeight.ToString() + "px");
                html.Append(divTag.ToString(TagRenderMode.StartTag));
               
                if (!_showTextOnRight)
                {
                    var Leftlbl = new TagBuilder("label");
                  
                    Leftlbl.Attributes.Add("for", inputName);
                    Leftlbl.Attributes.Add("class", "label" + inputName);  
                    Leftlbl.Attributes.Add("Id", "label" + inputName + "_" + i);
                    StringBuilder StyleValues1 = new StringBuilder();
                    StyleValues1.Append(GetRadioListStyle(_fontstyle.ToString(), null, null, null, null, IsHidden));
                    string InputFieldStyle_L = GetInputFieldStyle(_InputFieldfontstyle.ToString(), _InputFieldfontSize, _InputFieldfontfamily.ToString());
                    Leftlbl.Attributes.Add("style", StyleValues1.ToString() + ";" + IsHighlightedStyle + ";" + IsHiddenStyle + ";" + InputFieldStyle_L);
                    Leftlbl.SetInnerText(choiceList[i].Key);
                    html.Append(Leftlbl.ToString());
                }

                TagBuilder radioTag = new TagBuilder("input");
                radioTag.Attributes.Add("type", "radio");
                radioTag.Attributes.Add("name", inputName);
                radioTag.Attributes.Add("class", inputName);

                if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
                {
                    radioTag.Attributes.Add("onclick", "return " + _key + "_after();");
                }

                radioTag.SetInnerText(choiceList[i].Key);
                radioTag.Attributes.Add("value", i.ToString());
                radioTag.Attributes.Add("style", IsHiddenStyle);
                
                if (_IsDisabled)
                {
                    radioTag.Attributes.Add("disabled", "disabled");
                }

                if (choiceList[i].Value == true)
                {
                    radioTag.Attributes.Add("checked", "checked");
                }

                radioTag.MergeAttributes(_inputHtmlAttributes);
                html.Append(radioTag.ToString(TagRenderMode.SelfClosing));

                if (_showTextOnRight)
                {
                    var rightlbl = new TagBuilder("label");
                    rightlbl.Attributes.Add("for", inputName);
                    rightlbl.Attributes.Add("class", "label" + inputName);  
                    rightlbl.Attributes.Add("Id", "label" + inputName + "_" + i);
                    StringBuilder StyleValues2 = new StringBuilder();
                    StyleValues2.Append(GetRadioListStyle(_fontstyle.ToString(), null, null, null, null, IsHidden));
                    string InputFieldStyle_R = GetInputFieldStyle(_InputFieldfontstyle.ToString(), _InputFieldfontSize, _InputFieldfontfamily.ToString());
                    rightlbl.Attributes.Add("style", StyleValues2.ToString() + ";" + IsHighlightedStyle + ";" + IsHiddenStyle + ";" + InputFieldStyle_R);
                    rightlbl.SetInnerText(choiceList[i].Key);
                    html.Append(rightlbl.ToString());
                }

                html.Append(divTag.ToString(TagRenderMode.EndTag));
            }

            var hidden = new TagBuilder("input");
            hidden.Attributes.Add("type", "hidden");
            hidden.Attributes.Add("id", inputName);
            hidden.Attributes.Add("name", inputName);
            hidden.Attributes.Add("value", string.Empty);          
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