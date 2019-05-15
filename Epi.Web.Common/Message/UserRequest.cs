using System.Runtime.Serialization;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.DTO;

namespace Epi.Web.Common.Message
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
       [DataMember]
       public bool IsAuthenticated;
        }
    }
