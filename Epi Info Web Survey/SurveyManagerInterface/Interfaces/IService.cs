using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SurveyManagerInterface
{
    [ServiceContract]
    public interface ISurveyManager
    {
        [OperationContract]
        ISurveyRequestResult PublishSurvey(ISurveyRequest pRequestMessage);
    }

    [DataContract]
    public class ISurveyRequest : Epi.Web.Interfaces.ISurveyRequest
    {
        [DataMember]
        public DateTime ClosingDate {get;set;}

        [DataMember]
        public string SurveyName { get; set; }

        [DataMember]
        public string SurveyNumber { get; set; }

        [DataMember]
        public string OrganizationName { get; set; }

        [DataMember]
        public string DepartmentName { get; set; }

        [DataMember]
        public string IntroductionText { get; set; }

        [DataMember]
        public string TemplateXML { get; set; }

    }

    public interface ISurveyRequestResult : Epi.Web.Interfaces.ISurveyRequestResult
    {
        [DataMember]
        public bool IsPulished { get; set; }

        [DataMember]
        public string URL { get; set; }

        [DataMember]
        public string StatusText { get; set; }
    }
}
