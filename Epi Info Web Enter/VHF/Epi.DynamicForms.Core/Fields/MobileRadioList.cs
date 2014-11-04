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
    public class MobileRadioList : OrientableField
    {
        private string _ChoicesList;

        public string ChoicesList
        {
            get
            {
                return _ChoicesList;
            }

            set
            {
                _ChoicesList = value;
            }
        }
        public override string RenderHtml()
        {
            var html = new StringBuilder();
            var inputName = _form.FieldPrefix + _key;
            var choicesList = _choices.ToList();
         
            var choicesList1 = GetChoices(_ChoicesList);
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

            //if (_IsHidden)
            //{
            //    //IsHiddenStyle = "visibility:hidden";
            //    IsHiddenStyle = "display:none";
            //}
            if (_IsHighlighted)
            {
                IsHighlightedStyle = "background:yellow";
            }

            //var Div = new TagBuilder("div");
            //Div.Attributes.Add("data-role", "fieldcontain");
            //html.Append(Div.ToString(TagRenderMode.StartTag));

            var fieldset = new TagBuilder("fieldset");
            fieldset.Attributes.Add("data-role", "controlgroup");
            
            html.Append(fieldset.ToString(TagRenderMode.StartTag));

            var legend = new TagBuilder("legend");
              
            legend.SetInnerText(Prompt);
            html.Append(legend.ToString());

          

         
         
            for (int i = 0; i < choicesList.Count; i++)
            {

                double innerTop = 0.0;
                double innerLeft = 0.0;
                string radId = inputName + i;
                // if (Pattern != null && !string.IsNullOrEmpty(Pattern[0]))
                if ((Pattern.Count) == choicesList.Count)
                {
                    List<string> TopLeft = Pattern[i].ToString().Split(':').ToList();

                    if (TopLeft.Count > 0)
                    {
                        innerTop = double.Parse(TopLeft[0]) * Height;
                        innerLeft = double.Parse(TopLeft[1]) * Width;

                    }
                }

 
                var rad = new TagBuilder("input");
                rad.Attributes.Add("type", "radio");
                rad.Attributes.Add("name", inputName);
                rad.Attributes.Add("class", inputName);
                rad.Attributes.Add("id", radId);
                
                //StringBuilder RadioButton = new StringBuilder();
                //RadioButton.Append("<input type='Radio'");
                //RadioButton.Append(" name='" + inputName + "'");
                //RadioButton.Append(" id='" + radId + "'/>");

                
                ////////////Check code start//////////////////
                EnterRule FunctionObjectAfter = (EnterRule)_form.FormCheckCodeObj.GetCommand("level=field&event=after&identifier=" + _key);
                if (FunctionObjectAfter != null && !FunctionObjectAfter.IsNull())
                {

                    // rad.Attributes.Add("onblur", "return " + _key + "_after();"); //After
                    rad.Attributes.Add("onclick", "return " + _key + "_after(this.id);"); //After
                }

                ////////////Check code end//////////////////
                rad.SetInnerText(choicesList[i].Key);
                rad.Attributes.Add("value", i.ToString());
               // rad.Attributes.Add("style", IsHiddenStyle);
                if (_IsDisabled)
                {
                    rad.Attributes.Add("disabled", "disabled");
                }

                if (Value == i.ToString()) rad.Attributes.Add("checked", "checked");
                rad.MergeAttributes(_inputHtmlAttributes);
                html.Append(rad.ToString(TagRenderMode.SelfClosing));

             
                    var rightlbl = new TagBuilder("label");
                    rightlbl.Attributes.Add("for", radId);
                    rightlbl.Attributes.Add("class", "label" + inputName);
                    //StringBuilder StyleValues2 = new StringBuilder();
                    //StyleValues2.Append(GetRadioListStyle(_fontstyle.ToString(), null, null, null, null, IsHidden));
                    ////rightlbl.Attributes.Add("style", StyleValues2.ToString() + ";" + IsHighlightedStyle + ";" + IsHiddenStyle);
                    //rightlbl.Attributes.Add("style",  "" + IsHighlightedStyle + ";" + IsHiddenStyle);
                    rightlbl.SetInnerText(choicesList[i].Key);
                    html.Append(rightlbl.ToString());

                 
            }
          
            html.Append(fieldset.ToString(TagRenderMode.EndTag));
            //html.Append(Div.ToString(TagRenderMode.EndTag));

            // add hidden tag, so that a value always gets sent for select tags
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

        private Dictionary<string, bool> GetChoices(string _ChoicesList)
        {
            string ListString = _ChoicesList;
            ListString = ListString.Replace("||", "|");
            List<string> Lists = ListString.Split('|').ToList<string>();

            Dictionary<string, bool> Choices = new Dictionary<string, bool>();
            Choices = GetChoices(Lists[0].Split(',').ToList<string>());
            return Choices;
        }

        public static Dictionary<string, bool> GetChoices(List<string> List)
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