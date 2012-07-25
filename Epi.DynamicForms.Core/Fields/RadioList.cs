using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Epi.Core.EnterInterpreter;
using System.Web.UI;

 

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
            var choicesList = _choices.ToList();
     
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
                IsHiddenStyle = "visibility:hidden";
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
               // if (Pattern != null && !string.IsNullOrEmpty(Pattern[0]))
                if ((Pattern.Count ) == choicesList.Count )
                {
                    List<string> TopLeft = Pattern[i].ToString().Split(':').ToList();

                    if (TopLeft.Count > 0)
                    {
                        innerTop = double.Parse(TopLeft[0]) * Height;
                        innerLeft = double.Parse(TopLeft[1]) * Width;

                    }
                }

               

                var Div = new TagBuilder("Div");
                Div.Attributes.Add("class", _orientation == Orientation.Vertical ? _verticalClass : _horizontalClass);
                Div.Attributes["class"] += " " + _listClass;
                Div.Attributes.Add("style", "position:absolute; left:" + (_left + innerLeft) + "px;top:" + (_top + innerTop) + "px" + ";width:" + _ControlWidth.ToString() + "px" + ";height:" + _ControlHeight.ToString() + "px" + ";" + IsHiddenStyle);
                html.Append(Div.ToString(TagRenderMode.StartTag));
               
                if (!_showTextOnRight)
                {
                    var Leftlbl = new TagBuilder("label");
                  
                    Leftlbl.Attributes.Add("for", inputName);
                    //Leftlbl.Attributes.Add("class", _inputLabelClass);
                    Leftlbl.Attributes.Add("class", "label" + inputName);  
                    Leftlbl.Attributes.Add("Id", "label" + inputName + "_" + i);
                    StringBuilder StyleValues1 = new StringBuilder();
                    StyleValues1.Append(GetRadioListStyle(_fontstyle.ToString(), null, null, null, null, IsHidden));
                    Leftlbl.Attributes.Add("style", StyleValues1.ToString() + ";" + IsHighlightedStyle);
                    Leftlbl.SetInnerText(choicesList[i].Key);
                    html.Append(Leftlbl.ToString());
                     
                }

                // radio button input
                var rad = new TagBuilder("input");
                rad.Attributes.Add("type", "radio");
                rad.Attributes.Add("name", inputName);
                rad.Attributes.Add("class", inputName);
               // rad.Attributes.Add("onClick", "return document.getElementById('" + inputName + "').value = this.value;"); //After
                ////////////Check code start//////////////////
                EnterRule FunctionObjectAfter = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + _key);
                if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
                {

                    rad.Attributes.Add("onblur", "return " + _key + "_after();"); //After
                }
              
                ////////////Check code end//////////////////
                rad.SetInnerText(choicesList[i].Key);
                rad.Attributes.Add("value",  i.ToString());
                rad.Attributes.Add("style", "display: inline;"); 
                if (_IsDisabled)
                {
                rad.Attributes.Add("disabled", "disabled");
                }

                 if (Value == i.ToString()) rad.Attributes.Add("checked", "checked");
                rad.MergeAttributes(_inputHtmlAttributes);
                html.Append(rad.ToString(TagRenderMode.SelfClosing));

                // checkbox label
                if (_showTextOnRight)
                {
                    var rightlbl = new TagBuilder("label");
                    rightlbl.Attributes.Add("for", inputName);
                   // rightlbl.Attributes.Add("class", _inputLabelClass);  
                    rightlbl.Attributes.Add("class", "label" + inputName);  
                    rightlbl.Attributes.Add("Id", "label" + inputName + "_" + i);
                    StringBuilder StyleValues2 = new StringBuilder();
                    StyleValues2.Append(GetRadioListStyle(_fontstyle.ToString(), null, null, null, null, IsHidden));
                    rightlbl.Attributes.Add("style", StyleValues2.ToString() + ";" + IsHighlightedStyle);
                    rightlbl.SetInnerText(choicesList[i].Key);
                    html.Append(rightlbl.ToString());
               
                }
               
                html.Append(Div.ToString(TagRenderMode.EndTag));
            }

          
              // add hidden tag, so that a value always gets sent for select tags
            var hidden = new TagBuilder("input");
            hidden.Attributes.Add("type", "hidden");
            hidden.Attributes.Add("id", inputName);
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