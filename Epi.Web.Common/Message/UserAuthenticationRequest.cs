using System.Runtime.Serialization;
using Epi.Web.Common.MessageBase;

namespace Epi.Web.Common.Message
{
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class UserAuthenticationRequest: RequestBase
    {
         public UserAuthenticationRequest() { }
         [DataMember]
         public string SurveyResponseId;
         [DataMember]
         public string PassCode;
    }
}
