using System.Runtime.Serialization;
using Epi.Web.Enter.Common.MessageBase;
using Epi.Web.Enter.Common.DTO;

namespace Epi.Web.Enter.Common.Message
    {
   public class UserRequest
        {
       public UserRequest() 
           {
           this.Organization = new OrganizationDTO();
           this.User = new UserDTO();
           }
       [DataMember]
       public UserDTO User;
       [DataMember]
       public OrganizationDTO  Organization;
       [DataMember]
       public string Action;
       [DataMember]
       public int CurrentUser;
       [DataMember]
       public int CurrentOrg;
        }
    }
