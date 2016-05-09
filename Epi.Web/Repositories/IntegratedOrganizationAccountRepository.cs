using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC.Repositories.Core;
using Epi.Web.MVC.DataServiceClient;
using Epi.Web.Common.Message;
using Epi.Web.Common.Exception;
using System.ServiceModel;
 using System.Web.Caching;
using System.Configuration;
namespace Epi.Web.MVC.Repositories
    {
    public class IntegratedOrganizationAccountRepository :  IOrganizationAccountRepository
        {
         private Epi.Web.WCF.SurveyService.IDataService _iDataService;

         public IntegratedOrganizationAccountRepository(Epi.Web.WCF.SurveyService.IDataService iDataService)
        {
            _iDataService = iDataService;
        }


         public OrganizationAccountResponse CreateAccount(OrganizationAccountRequest Request) { 
             
             OrganizationAccountResponse Response =  new OrganizationAccountResponse();
             Response = _iDataService.CreateAccount(Request);
             return Response;
             }
         public OrganizationAccountResponse GetStateList(OrganizationAccountRequest Request) {


         OrganizationAccountResponse Response = new OrganizationAccountResponse();
         Response = _iDataService.GetStateList(Request);
         return Response;
             
             }


         #region stubcode
         public List<Common.DTO.SurveyInfoDTO> GetList(Criterion criterion = null)
             {
             throw new NotImplementedException();
             }

         public Common.DTO.SurveyInfoDTO Get(int id)
             {
             throw new NotImplementedException();
             }

         public int GetCount(Criterion criterion = null)
             {
             throw new NotImplementedException();
             }

         public void Insert(Common.DTO.SurveyInfoDTO t)
             {
             throw new NotImplementedException();
             }

         public void Update(Common.DTO.SurveyInfoDTO t)
             {
             throw new NotImplementedException();
             }

         public void Delete(int id)
             {
             throw new NotImplementedException();
             }
         #endregion


         List<SurveyInfoResponse> IRepository<SurveyInfoResponse>.GetList(Criterion criterion = null)
             {
             throw new NotImplementedException();
             }

         SurveyInfoResponse IRepository<SurveyInfoResponse>.Get(int id)
             {
             throw new NotImplementedException();
             }

         public void Insert(SurveyInfoResponse t)
             {
             throw new NotImplementedException();
             }

         public void Update(SurveyInfoResponse t)
             {
             throw new NotImplementedException();
             }

        }
    }