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
    
    public partial class Organization
    {
        public Organization()
        {
            this.UserOrganizations = new HashSet<UserOrganization>();
            this.SurveyMetaDatas = new HashSet<SurveyMetaData>();
            this.SurveyResponses = new HashSet<SurveyResponse>();
            this.Admins = new HashSet<Admin>();
            this.SurveyMetaDatas1 = new HashSet<SurveyMetaData>();
            this.Datasources = new HashSet<Datasource>();
        }
    
        public int OrganizationId { get; set; }
        public string OrganizationKey { get; set; }
        public string Organization1 { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsHostOrganization { get; set; }
    
        public virtual ICollection<UserOrganization> UserOrganizations { get; set; }
        public virtual ICollection<SurveyMetaData> SurveyMetaDatas { get; set; }
        public virtual ICollection<SurveyResponse> SurveyResponses { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<SurveyMetaData> SurveyMetaDatas1 { get; set; }
        public virtual ICollection<Datasource> Datasources { get; set; }
    }
}
