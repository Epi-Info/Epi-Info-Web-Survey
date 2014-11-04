using System.Runtime.Serialization;
using Epi.Web.Enter.Common.MessageBase;
using Epi.Web.Enter.Common.DTO;

namespace Epi.Web.Enter.Common.Message
{
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class UserAuthenticationRequest: RequestBase
    {
         public UserAuthenticationRequest() { }
         //This code stays but will not be used in WebEnter
         //Starts
         [DataMember]
         public string SurveyResponseId;
         [DataMember]
         public string PassCode;
         //Ends

         [DataMember]
         public UserDTO User;
         
    }
}
