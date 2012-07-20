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
            // prompt label
            //var prompt = new TagBuilder("label");
            //prompt.Attributes.Add("class", _promptClass);
            //prompt.SetInnerText(Prompt);
            //html.Append(prompt.ToString());
            ///////////////////
            string Psize ;
            int Largest =0;
            for (int i = 0; i < choicesList.Count; i++)
            {
                Psize = choicesList[i].Key.Length.ToString();
                //if (choicesList[i].Key.Length > Largest)
                //{
                Largest = Largest + choicesList[i].Key.Length;
                //}
            
            }

            Largest = Largest / choicesList.Count;

            double ControlWidth = Largest * fontSize * 1.3;
           

            ///////////////////
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
            ul.Attributes.Add("style", "position:absolute; left:" + (_left + (fontSize * 1.5)).ToString() + "px;top:" + (_top + (fontSize * 1.3)).ToString() + "px" + ";width:" + _ControlWidth.ToString() + "px" + ";height:" + _ControlHeight.ToString() + "px");
           

            html.Append(ul.ToString(TagRenderMode.StartTag));

         
           
            for (int i = 0; i < choicesList.Count; i++)
            {
                string radId = inputName + i;
               
                // open list item
                var li = new TagBuilder("li");

                if (!_showTextOnRight)
                {
                    li.Attributes.Add("style", "float:left;text-align:Right; width:" + ControlWidth.ToString() + "px" + ";Height:" + (fontSize * 2.1).ToString() + "px");

                }
                else
                {

                    li.Attributes.Add("style", "float:left; width:" + ControlWidth.ToString() + "px" + ";Height:" + (fontSize * 2.1).ToString() + "px");
                }
                html.Append(li.ToString(TagRenderMode.StartTag));
                // checkbox label
                if (!_showTextOnRight)
                {
                    var Leftlbl = new TagBuilder("label");
                   // Leftlbl.Attributes.Add("for", radId);
                    Leftlbl.Attributes.Add("for", inputName);
                    Leftlbl.Attributes.Add("class", _inputLabelClass);
                    Leftlbl.Attributes.Add("Id", "label" + inputName + "_" + i);
                    StringBuilder StyleValues = new StringBuilder();
                    StyleValues.Append(GetRadioListStyle(_fontstyle.ToString(), null, null, null, null, IsHidden));
                    Leftlbl.Attributes.Add("style", StyleValues.ToString());
                    Leftlbl.SetInnerText(choicesList[i].Key);
                    html.Append(Leftlbl.ToString());
                     
                }

                // radio button input
                var rad = new TagBuilder("input");
                rad.Attributes.Add("type", "radio");
                rad.Attributes.Add("name", inputName);
                ////////////Check code start//////////////////
                EnterRule FunctionObjectAfter = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + _key);
                if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
                {

                    rad.Attributes.Add("onblur", "return " + _key + "_after();"); //After
                }
                //EnterRule FunctionObjectBefore = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=before&identifier=" + _key);
                //if (FunctionObjectBefore != null && !FunctionObjectBefore.IsNull())
                //{

                //    rad.Attributes.Add("onfocus", "return " + _key + "_before();"); //Before
                //}

                ////////////Check code end//////////////////
                rad.SetInnerText(choicesList[i].Key);
                rad.Attributes.Add("value",  i.ToString());
                rad.Attributes.Add("style", "display: inline;"); 
               //  if (choicesList[i].Value)

                 if (Value == i.ToString()) rad.Attributes.Add("checked", "checked");
                rad.MergeAttributes(_inputHtmlAttributes);
                html.Append(rad.ToString(TagRenderMode.SelfClosing));

                // checkbox label
                if (_showTextOnRight)
                {
                    var rightlbl = new TagBuilder("label");
                    rightlbl.Attributes.Add("for", inputName);
                    rightlbl.Attributes.Add("class", _inputLabelClass);
                    rightlbl.Attributes.Add("Id", "label" + inputName + "_" + i);
                    StringBuilder StyleValues = new StringBuilder();
                    StyleValues.Append(GetRadioListStyle(_fontstyle.ToString(), null, null, null, null, IsHidden));
                    rightlbl.Attributes.Add("style", StyleValues.ToString());
                    rightlbl.SetInnerText(choicesList[i].Key);
                    html.Append(rightlbl.ToString());
               
                }
                // close list item
                html.Append(li.ToString(TagRenderMode.EndTag));
            }

            html.Append(ul.ToString(TagRenderMode.EndTag));
                     

            var wrapper = new TagBuilder(_fieldWrapper);
            wrapper.Attributes["class"] = _fieldWrapperClass;
            wrapper.Attributes.Add("style", "width:" + _ControlWidth.ToString() + "px" + ";height:" + _ControlHeight.ToString() + "px");
           

            wrapper.InnerHtml = html.ToString();
            return wrapper.ToString();
        }
    }
}