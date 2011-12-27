using System.Linq;
using MvcDynamicForms.Fields;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using MvcDynamicForms;

namespace Epi.Web.MVC.Utility
{
    public class NumericTextBoxProvider
    {
        public NumericTextBox GetNumericTextBox(XElement _FieldTypeID, double _Width, double _Height, string SurveyAnswer, string _ControlValue)
        {

            var NumericTextBox = new NumericTextBox
            {
                Title = "Name",
                Prompt = _FieldTypeID.Attribute("PromptText").Value,
                DisplayOrder = 20,
                Required = true,
                RequiredMessage = _FieldTypeID.Attribute("PromptText").Value + " is required",
                Key = _FieldTypeID.Attribute("UniqueId").Value,
                PromptTop = _Height * double.Parse(_FieldTypeID.Attribute("PromptTopPositionPercentage").Value),
                PromptLeft = _Width * double.Parse(_FieldTypeID.Attribute("PromptLeftPositionPercentage").Value),
                Top = _Height * double.Parse(_FieldTypeID.Attribute("ControlTopPositionPercentage").Value),
                Left = _Width * double.Parse(_FieldTypeID.Attribute("ControlLeftPositionPercentage").Value),
                PromptWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                ControlWidth = _Width * double.Parse(_FieldTypeID.Attribute("ControlWidthPercentage").Value),
                fontstyle = _FieldTypeID.Attribute("PromptFontStyle").Value,
                fontSize = double.Parse(_FieldTypeID.Attribute("PromptFontSize").Value),
                fontfamily = _FieldTypeID.Attribute("PromptFontFamily").Value,
                IsRequired = bool.Parse(_FieldTypeID.Attribute("IsRequired").Value),
                IsReadOnly = bool.Parse(_FieldTypeID.Attribute("IsReadOnly").Value),
                Lower = _FieldTypeID.Attribute("Lower").Value,
                Upper = _FieldTypeID.Attribute("Upper").Value,
                ControlValue = _ControlValue
            };
            return NumericTextBox;

        }

    }
}