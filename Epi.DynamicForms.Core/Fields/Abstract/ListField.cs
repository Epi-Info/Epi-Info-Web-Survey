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
        
        /// <summary>
        /// The choices that the end user can choose from.
        /// </summary>
        public Dictionary<string, bool> Choices
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
        public int SelectType { get; set; }
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
                // builds a delimited list of the responses
                var value = new StringBuilder();

                foreach (var choice in _choices)
                {
                    value.Append(choice.Value ? choice.Key + _responseDelimiter : string.Empty);
                }

                return value.ToString().TrimEnd(_responseDelimiter.ToCharArray()).Trim();
            }
            set 
            {
                 
                    switch (this.SelectType.ToString())
                    {
                        case "11"://Yes/No

                            Dictionary<string, bool> choices11 = new Dictionary<string, bool>();
                           
                            foreach (var choice in _choices)
                            {

                                string choiceValue = value == "True" ? "Yes" : "No";

                                if (choice.Key == choiceValue)
                                {
                                    choices11.Add(choice.Key.ToString(), true);

                                }
                                else {
                                    choices11.Add(choice.Key.ToString(), false);
                                }
                                
                            }
                            Choices = choices11;
                            break;
                        case "17"://DropDown LegalValues
                           Dictionary<string, bool> choices17 = new Dictionary<string, bool>();
                           
                            foreach (var choice in _choices)
                            {
                                 

                               
                                if (choice.Key == value)
                                {
                                    choices17.Add(choice.Key.ToString(), true);

                                }
                                else {
                                    choices17.Add(choice.Key.ToString(), false);
                                }
                                
                            }
                            Choices = choices17;
                            break;
                        case "18":
                         Dictionary<string, bool> choices18 = new Dictionary<string, bool>();
                           
                            foreach (var choice in _choices)
                            {
                                 

                               
                                if (choice.Key == value)
                                {
                                    choices18.Add(choice.Key.ToString(), true);

                                }
                                else {
                                    choices18.Add(choice.Key.ToString(), false);
                                }
                                
                            }
                            Choices = choices18;

                            break;
                        case "19"://Comment Legal
                          Dictionary<string, bool> choices19 = new Dictionary<string, bool>();
                           
                            foreach (var choice in _choices)
                            {


                                if (value.IndexOf('-') == -1)
                                {
                                    if (choice.Key.Split('-')[0] == value)
                                    {
                                        choices19.Add(choice.Key.ToString(), true);

                                    }
                                    else
                                    {
                                        choices19.Add(choice.Key.ToString(), false);
                                    }
                                }
                                else
                                {

                                    if (choice.Key == value)
                                    {
                                        choices19.Add(choice.Key.ToString(), true);

                                    }
                                    else
                                    {
                                        choices19.Add(choice.Key.ToString(), false);
                                    }
                                }
                                
                            }
                            Choices = choices19;
                            break;


                    }
                }
                
                       
                        
            }
        
        public override bool Validate()
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
        public void AddChoices(string choices, string delimiter)
        {
            if (string.IsNullOrEmpty(choices)) return;

            choices.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToList()
                .ForEach(c => _choices.Add(c, false));
        }
        public string SelectedValue{ get;set;}
    }
}
