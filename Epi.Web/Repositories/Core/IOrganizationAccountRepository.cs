using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.Common.Message;
namespace Epi.Web.MVC.Repositories.Core
    {
    public interface IOrganizationAccountRepository : IRepository<Epi.Web.Common.Message.SurveyInfoResponse>
        {

        OrganizationAccountResponse CreateAccount(OrganizationAccountRequest Request);

        OrganizationAccountResponse GetStateList(OrganizationAccountRequest Request);

        OrganizationAccountResponse GetUserOrgId(OrganizationAccountRequest Request);
        }   
    }