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
    public class RadioList : OrientableField
    {
        private string _choicesList;

        public string ChoicesList
        {
            get
            {
                return _choicesList;
            }

            set
            {
                _choicesList = value;
            }
        }

        public override string RenderHtml()
        {
            var html = new StringBuilder();
            var inputName = _fieldPrefix + _key;
            var choicesList = _choices.ToList();
            var choicesList1 = GetChoices(_choicesList);
            choicesList = choicesList1.ToList();

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
           
            for (int i = 0; i < choicesList.Count; i++)
            {
                double innerTop = 0.0;
                double innerLeft = 0.0;
                string radId = inputName + i;

                if ((Pattern.Count ) == choicesList.Count )
                {
                    List<string> TopLeft = Pattern[i].ToString().Split(':').ToList();

                    if (TopLeft.Count > 0)
                    {
                        innerTop = double.Parse(TopLeft[0]) * Height;
                        innerLeft = double.Parse(TopLeft[1]) * Width;
                    }
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
                    Leftlbl.SetInnerText(choicesList[i].Key);
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

                radioTag.SetInnerText(choicesList[i].Key);
                radioTag.Attributes.Add("value", i.ToString());
                radioTag.Attributes.Add("style", IsHiddenStyle); 
                
                if (_IsDisabled)
                {
                    radioTag.Attributes.Add("disabled", "disabled");
                }

                if (Value == i.ToString())
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
                    rightlbl.SetInnerText(choicesList[i].Key);
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

        protected Dictionary<string, bool> GetChoices(string _ChoicesList)
        {
            string ListString = _ChoicesList;
            ListString = ListString.Replace("||", "|");
            List<string> Lists = ListString.Split('|').ToList<string>();

            Dictionary<string, bool> Choices = new Dictionary<string, bool>();
            Choices = GetChoices(Lists[0].Split(',').ToList<string>());
            return Choices;
        }

        protected static Dictionary<string, bool> GetChoices(List<string> List)
        {
            Dictionary<string, bool> NewList = new Dictionary<string, bool>();
            foreach (var _List in List)
            {
                NewList.Add(_List, false);

            }
            return NewList;
        }
    }
}