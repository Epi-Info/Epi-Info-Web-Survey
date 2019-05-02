using System.Linq;
using System.Web.Mvc;
using MvcDynamicForms.Fields;
using MvcDynamicForms.Utilities;
using System;
 
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
                       
                        if (dynField is AutoComplete)
                        {
                            var txtField = (AutoComplete)dynField;
                            txtField.Response = postedForm[key].TrimEnd(',');
                        }
                        else if (dynField is MobileAutoComplete)
                        {
                            var AutoCompleteField = (MobileAutoComplete)dynField;
                            AutoCompleteField.Response = postedForm[key].TrimEnd(',');
                        }
                        else  if (dynField is TextField)
                        {
                            var txtField = (TextField)dynField;
                            txtField.Response = postedForm[key];
                        }
                        else if (dynField is NumericTextField)
                        {
                            var numerictxtField = (NumericTextField)dynField;

                            if (postedForm[key].Contains(","))
                            {
                                numerictxtField.Response = postedForm[key].Replace(",", ".");
                            }
                            else
                            {
                                numerictxtField.Response = postedForm[key];
                            }
                        }

                        else if (dynField is DatePickerField)
                        {
                            var datepickerField = (DatePickerField)dynField;
                            DateTime dt;
                           // datepickerField.Response = DateTimeOffset.Parse(postedForm[key]).UtcDateTime.ToString()  ;
                              var isValidDate=DateTime.TryParse(postedForm[key], out dt);
                              if (!string.IsNullOrEmpty(postedForm[key]) && isValidDate)
                              {
                                  string date = DateTimeOffset.Parse(postedForm[key]).ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                  datepickerField.Response = date.Remove(date.IndexOf('T'));
                              }
                              else {
                                  datepickerField.Response = postedForm[key];
                              }
                        }
                        else if (dynField is TimePickerField)
                        {
                            var timepickerField = (TimePickerField)dynField;
                            if (postedForm[key].Contains(','))
                            {
                                timepickerField.Value = postedForm[key].Remove(postedForm[key].IndexOf(','));
                            }
                            else
                            {
                                timepickerField.Value = postedForm[key];
                            }
                        }
                        //else if (dynField is MobileRadioList )
                        //    {
                        //    var listField = (MobileRadioList)dynField;

                        //    foreach (string choiceKey in listField.ChoiceKeyValuePairs.Keys.ToList())
                        //        {
                        //        listField.ChoiceKeyValuePairs[choiceKey] = false;
                        //        }
                        //    var choiceArray = listField.ChoiceKeyValuePairs.Keys.ToArray();


                        //    foreach (string value in postedForm.GetValues(key))
                        //        {
                        //        if (!string.IsNullOrEmpty(value))
                        //            {
                        //            var _Key = choiceArray[int.Parse(value)];
                        //            listField.ChoiceKeyValuePairs[_Key] = true;
                        //            }

                        //        }
                        //    }
                        //else if (dynField is RadioList  )
                        //    {
                        //    var listField = (RadioList)dynField;

                        //    foreach (string choiceKey in listField.ChoiceKeyValuePairs.Keys.ToList())
                        //        {
                        //        listField.ChoiceKeyValuePairs[choiceKey] = false;
                        //        }
                        //    var choiceArray = listField.ChoiceKeyValuePairs.Keys.ToArray();


                        //    foreach (string value in postedForm.GetValues(key))
                        //        {
                        //        if (!string.IsNullOrEmpty(value))
                        //            {
                        //            var _Key = choiceArray[int.Parse(value)];
                        //            listField.ChoiceKeyValuePairs[_Key] = true;
                        //            }

                        //        }
                        //    }
                        else if (dynField is ListField)
                        {
                            var listField = (ListField)dynField;

                            foreach (string choiceKey in listField.ChoiceKeyValuePairs.Keys.ToList())
                            {
                                listField.ChoiceKeyValuePairs[choiceKey] = false;
                            }
                          
                          
                            foreach (string value in postedForm.GetValues(key))
                            {
                                 

                                try
                                {
                                    var CurrentValue = listField.ChoiceKeyValuePairs[value];
                                    listField.ChoiceKeyValuePairs[value] = true;
                                }
                                catch (Exception ex) {
                                    if (!string.IsNullOrEmpty(value)) {
                                        var array = listField.ChoiceKeyValuePairs.ToArray();
                                        var NewValue = array[int.Parse(value)];
                                        listField.ChoiceKeyValuePairs[NewValue.Key] = true;
                                    }

                                }
                         //  listField.ChoiceKeyValuePairs[value] = true;
                           
                            }

                           listField.ChoiceKeyValuePairs.Remove("");
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
