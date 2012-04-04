using System.Runtime.Serialization;
using Epi.Web.Common.MessageBase;
namespace Epi.Web.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class UserAuthenticationResponse : ResponseBase
    {
        public UserAuthenticationResponse() { }

        [DataMember]
        public bool UserIsValid;
        [DataMember]
        public string PassCode;
    }
}
