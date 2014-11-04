using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessRule;

namespace Epi.Web.Enter.Common.BusinessObject
{
    /// <summary>
    /// Abstract base class for business objects.
    /// Contains basic business rule infrastructure.
    /// </summary>
    public abstract class BusinessObject
    {
        /// <summary>
        /// Default value for version number (used in LINQ's optimistic concurrency)
        /// </summary>
        protected static readonly string _versionDefault = "NotSet";

        // List of business rules
        private List<BusinessRule.BusinessRule> _businessRules = new List<BusinessRule.BusinessRule>();

        // List of validation errors (following validation failure)
        private List<string> _validationErrors = new List<string>();

        /// <summary>
        /// Gets list of validations errors.
        /// </summary>
        public List<string> ValidationErrors
        {
            get { return _validationErrors; }
        }

        /*
        /// <summary>
        /// Adds a business rule to the business object.
        /// </summary>
        /// <param name="rule"></param>
        protected void AddRule(BusinessRule rule)
        {
            _businessRules.Add(rule);
        }*/

        /// <summary>
        /// Determines whether business rules are valid or not.
        /// Creates a list of validation errors when appropriate.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool isValid = true;

            _validationErrors.Clear();

            foreach (BusinessRule.BusinessRule rule in _businessRules)
            {
                if (!rule.Validate(this))
                {
                    isValid = false;
                    _validationErrors.Add(rule.ErrorMessage);
                }
            }
            return isValid;
        }
    }
}