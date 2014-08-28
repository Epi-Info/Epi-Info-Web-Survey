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
                try
                {
                    InputField dynField = form.InputFields.SingleOrDefault(f => f.Key == fieldKey);

                    if (dynField != default(InputField))
                    {
                        if (dynField is TextField)
                        {
                            var txtField = (TextField)dynField;
                            txtField.Response = postedForm[key];
                        }

                        else if (dynField is NumericTextField)
                        {
                            var numerictxtField = (NumericTextField)dynField;
                            numerictxtField.Response = postedForm[key];
                        }

                        else if (dynField is DatePickerField)
                        {
                            var datepickerField = (DatePickerField)dynField;
                            datepickerField.Response = postedForm[key];
                        }
                        else if (dynField is TimePickerField)
                        {
                            var timepickerField = (TimePickerField)dynField;
                            timepickerField.Value = postedForm[key];
                        }
                        else if (dynField is ListField)
                        {
                            var listField = (ListField)dynField;

                            foreach (string choiceKey in listField.ChoiceKeyValuePairs.Keys.ToList())
                            {
                                listField.ChoiceKeyValuePairs[choiceKey] = false;
                            }
                            var choiceArray = listField.ChoiceKeyValuePairs.Keys.ToArray();

                          
                            foreach (string value in postedForm.GetValues(key))
                            {
                           //listField.ChoiceKeyValuePairs[value] = true;
                            if (!string.IsNullOrEmpty(value))
                                {
                                var Key = choiceArray[int.Parse(value)];
                                listField.ChoiceKeyValuePairs[Key] = true;
                                }


                            }

                           //listField.ChoiceKeyValuePairs.Remove("");
                        }
                        else if (dynField is CheckBox)
                        {
                            var chkField = (CheckBox)dynField;

                            bool test;
                            if (bool.TryParse(postedForm.GetValues(key)[0], out test))
                            {
                                chkField.Checked = test;
                            }
                        }
                        else if (dynField is MobileCheckBox)
                        {
                            var chkField = (MobileCheckBox)dynField;
                            bool test;
                            if (bool.TryParse(postedForm.GetValues(key)[0], out test))
                            {
                                chkField.Checked = test;
                            }
                        }
                    }
                }
                catch (System.InvalidOperationException ex)
                {

                    //continue;
                    //form.AddFields
                    //(new Field[]  
                    //    { new Hidden
                    //    {
                    //        Title = fieldKey,
                    //        Key = fieldKey,
                    //        IsPlaceHolder = true,
                    //        Value =  postedForm[key]
                    //    }
                    //    }
                    //);
                }
            }

            return form;
        }
    }
}
