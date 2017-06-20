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
        public bool Wrap { get; set; }
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
                wrapper.Attributes["ID"] = "mvcdynamicfield_" + Name.ToLower() + "_fieldWrapper";
                StringBuilder StyleValues = new StringBuilder();

                StyleValues.Append(GetControlStyle(_fontstyle.ToString(), Math.Truncate(_top).ToString(), Math.Truncate(_left).ToString(), Math.Truncate(Width).ToString(), Math.Truncate(Height).ToString(), IsHidden));
                wrapper.Attributes.Add(new KeyValuePair<string, string>("style", StyleValues.ToString()));
                wrapper.InnerHtml = Html;

                return wrapper.ToString();
            }

            return Html;
        }
    }
}
