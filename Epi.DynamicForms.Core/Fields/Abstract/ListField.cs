using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcDynamicForms.Fields
{
    /// <summary>
    /// Represents an html input field that contains choices for the end user to choose from.
    /// </summary>
    [Serializable]
    public abstract class ListField : InputField
    {
        protected Dictionary<string, bool> _choices = new Dictionary<string, bool>();
        protected string _responseDelimiter = ", ";
        protected float _ControlFontSize;
        protected string _ControlFontStyle;
        private string _selectedValue = string.Empty;
        protected bool _RelateCondition;

        public bool RelateCondition
        {
            get
            {
                return _RelateCondition;
            }
            set
            {
                _RelateCondition = value;
            }
        }
        /// <summary>
        /// The choices that the end user can choose from.
        /// </summary>
        public Dictionary<string, bool> ChoiceKeyValuePairs
        {
            get
            {
                return _choices;
            }
            set
            {
                _choices = value;
            }
        }
        
        /// <summary>
        /// The text used to delimit multiple choices from the end user.
        /// </summary>
        public string ResponseDelimiter
        {
            get
            {
                return _responseDelimiter;
            }
            set
            {
                _responseDelimiter = value;
            }
        }

        public float ControlFontSize 
        { 
            get
            {
                return _ControlFontSize;
            }
            
            set
            {
                _ControlFontSize = value;
            } 
        }

        public string ControlFontStyle
        {
            get
            {
                return _ControlFontStyle;
            }

            set
            {
                _ControlFontStyle = value;
            }
        }
        public override string Response
        {
            get
            {
                var value = new StringBuilder();

                foreach (var choice in _choices)
                {
                    value.Append(choice.Value ? choice.Key + _responseDelimiter : string.Empty);
                }

                return value.ToString().TrimEnd(_responseDelimiter.ToCharArray()).Trim();
            }
            set 
            {
                switch (FieldTypeId.ToString())
                {
                    case "11":
                        Dictionary<string, bool> yesno = new Dictionary<string, bool>();
                           
                        foreach (var choice in _choices)
                        {
                            string choiceValue = value.ToLower();

                            if (choiceValue == "1" || choiceValue == "true" || choiceValue == "yes")
                            {
                                choiceValue = "Yes";
                            }
                            else
                            {
                                choiceValue = "No";
                            }

                            if (choice.Key == choiceValue)
                            {
                                yesno.Add(choice.Key.ToString(), true);
                            }
                            else 
                            {
                                yesno.Add(choice.Key.ToString(), false);
                            }
                        }

                        ChoiceKeyValuePairs = yesno;
                        break;

                    case "12":
                        Dictionary<string, bool> options = new Dictionary<string, bool>();
                        List<KeyValuePair<string, bool>> list = _choices.ToList();
                       
                    for (int i = 0; i < list.Count; i++)
                        {
                           /// int index = -1;
                           // int.TryParse(value, out index);

                            if (list[i].Key == value)
                                {
                                options.Add(list[i].Key.ToString(), true);
                                }
                            else
                                {
                                options.Add(list[i].Key.ToString(), false);
                                }
                            
                        }

                        ChoiceKeyValuePairs = options;
                        break;

                    case "17":
                        Dictionary<string, bool> legalValues = new Dictionary<string, bool>();
                        foreach (var choice in _choices)
                        {
                            if (choice.Key == value)
                            {
                                legalValues.Add(choice.Key.ToString(), true);
                            }
                            else 
                            {
                                legalValues.Add(choice.Key.ToString(), false);
                            }
                                
                        }
                        
                        ChoiceKeyValuePairs = legalValues;
                        break;

                    case "18":
                        Dictionary<string, bool> codes = new Dictionary<string, bool>();
                        foreach (var choice in _choices)
                        {
                            if (choice.Key == value)
                            {
                                codes.Add(choice.Key.ToString(), true);
                            }
                            else 
                            {
                                codes.Add(choice.Key.ToString(), false);
                            }
                                
                        }
                        
                        ChoiceKeyValuePairs = codes;
                        break;

                    case "19":
                        Dictionary<string, bool> commentLegal = new Dictionary<string, bool>();
                        foreach (var choice in _choices)
                        {
                            if (value.IndexOf('-') == -1)
                            {
                                if (choice.Key.Split('-')[0].Trim() == value)
                                {
                                    commentLegal.Add(choice.Key.ToString(), true);
                                }
                                else
                                {
                                    commentLegal.Add(choice.Key.ToString(), false);
                                }
                            }
                            else
                            {
                                if (choice.Key == value)
                                {
                                    commentLegal.Add(choice.Key.ToString(), true);
                                }
                                else
                                {
                                    commentLegal.Add(choice.Key.ToString(), false);
                                }
                            }
                        }
                        
                        ChoiceKeyValuePairs = commentLegal;
                        break;
                    }
                }
            }
        
        public override bool Validate(string DateFormat)
        {
            if (Required && !_choices.Values.Contains(true))
            {
                // invalid: required and no checkbox was selected
                Error = _requiredMessage;
                return false;
            }

            // valid
            ClearError();
            return true;
        }
        /// <summary>
        /// Provides a convenient way to add choices.
        /// </summary>
        /// <param name="choices">A delimited string of choices.</param>
        /// <param name="delimiter">The delimiter used to seperate the choices.</param>
        public void AddChoices(string choices, string delimiter,bool Sort = false )
        {
            
                if (string.IsNullOrEmpty(choices)) return;
                if (!Sort)
                {
                    choices.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Distinct()
                        .ToList() 
                        .ForEach(c => _choices.Add(c, false));
                }
                else
                {
                    choices.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                     .Distinct()
                     .ToList().OrderBy(a=>a).ToList()
                     .ForEach(c => _choices.Add(c, false));
               }
            
        }

        public string SelectedValue
        {
            get
            {
                return _selectedValue;
            }
            set
            {
                _selectedValue = value;
            }
        }
    }
}
