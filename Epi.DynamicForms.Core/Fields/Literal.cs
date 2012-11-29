using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents html to be rendered on the form.
    /// </summary>
    [Serializable]
    public class Literal : Field
    {
        /// <summary>
        /// Determines whether the rendered html will be wrapped by another element.
        /// </summary>
        /// 

        public string Name { get; set; }
        public bool Wrap { get; set; }
        /// <summary>
        /// The html to be rendered on the form.
        /// </summary>
        public string Html { get; set; }

        public override string RenderHtml()
        {
            if (Wrap)
            
            {
                
                var wrapper = new TagBuilder(_fieldWrapper);
                if (string.IsNullOrEmpty(this._cssClass))
                {
                    wrapper.Attributes["class"] = _fieldWrapperClass;
                }
                else
                {
                    wrapper.Attributes["class"] = this._cssClass;
                }


                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\r\n|\r|\n)+");

                string newText = regex.Replace(Html.Replace("  ", " &nbsp;"), "<br />");

                Html = MvcHtmlString.Create(newText).ToString();

               // wrapper.Attributes["ID"] = "labelmvcdynamicfield_" + Name.ToLower();
                wrapper.Attributes["ID"] = "mvcdynamicfield_" + Name.ToLower() + "_fieldWrapper";
                StringBuilder StyleValues = new StringBuilder();

                StyleValues.Append(GetContolStyle(_fontstyle.ToString(), _top.ToString(), _left.ToString(), Width.ToString(), Height.ToString(), IsHidden));
                //StyleValues.Append(";word-wrap:break-word;");
                 wrapper.Attributes.Add(new KeyValuePair<string, string>("style", StyleValues.ToString()));
               
                wrapper.InnerHtml = Html;
                return wrapper.ToString();
            }
            return Html;
        }
   
    }
}
