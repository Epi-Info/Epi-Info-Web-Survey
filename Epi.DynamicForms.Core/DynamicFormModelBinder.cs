using System.Linq;
using System.Web.Mvc;
using MvcDynamicForms.Fields;
using MvcDynamicForms.Utilities;

namespace MvcDynamicForms
{
    class DynamicFormModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var postedForm = controllerContext.RequestContext.HttpContext.Request.Form;

            var form = (Form)bindingContext.Model;
            if (form == null && !string.IsNullOrEmpty(postedForm[MagicStrings.MvcDynamicSerializedForm]))
            {
                form = SerializationUtility.Deserialize<Form>(postedForm[MagicStrings.MvcDynamicSerializedForm]);
            }

            foreach (var key in postedForm.AllKeys.Where(x => x.StartsWith(form.FieldPrefix)))
            {
                string fieldKey = key.Remove(0, form.FieldPrefix.Length);
                InputField dynField = form.InputFields.Single(f => f.Key == fieldKey);

                if (dynField is TextField)
                {
                    var txtField = (TextField)dynField;
                    txtField.Value = postedForm[key];
                }
                else if (dynField is ListField)
                {
                    var lstField = (ListField)dynField;

                    // clear all choice selections                    
                    foreach (string k in lstField.Choices.Keys.ToList())
                        lstField.Choices[k] = false;

                    // set current selections
                    foreach (string value in postedForm.GetValues(key))
                        lstField.Choices[value] = true;

                    lstField.Choices.Remove("");
                }
                else if (dynField is CheckBox)
                {
                    var chkField = (CheckBox)dynField;
                    chkField.Checked = bool.Parse(postedForm.GetValues(key)[0]);
                }
            }

            return form;
        }
    }
}
