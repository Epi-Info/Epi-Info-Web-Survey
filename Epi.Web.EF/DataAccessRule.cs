//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Epi.Web.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class DataAccessRule
    {
        public DataAccessRule()
        {
            this.SurveyMetaDatas = new HashSet<SurveyMetaData>();
        }
    
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public string RuleDescription { get; set; }
    
        public virtual ICollection<SurveyMetaData> SurveyMetaDatas { get; set; }
    }
}
