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
    
    public partial class SurveyReport
    {
        public System.Guid ReportId { get; set; }
        public System.Guid SurveyId { get; set; }
        public string ReportHtml { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int Version { get; set; }
    }
}