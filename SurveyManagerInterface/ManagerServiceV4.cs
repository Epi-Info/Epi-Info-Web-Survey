using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.ObjectMapping;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Exception;

namespace Epi.Web.WCF.SurveyService
{
    public class ManagerServiceV4 : ManagerServiceV3, IManagerServiceV4
    {
       public bool ValidateOrganization(OrganizationRequest Request)
        {
            bool IsValid;
            try
            {
                

                Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao ISurveyInfoDao = new EF.EntitySurveyInfoDao();
                Epi.Web.BLL.SurveyInfo Implementation = new Epi.Web.BLL.SurveyInfo(ISurveyInfoDao);
                IsValid = Implementation.ValidateOrganization(Request);

            }
            catch (Exception ex)
            {
               
                throw ex;
            }
            return IsValid;
        }

        public SurveyInfoResponse GetAllSurveysByOrgKey(string OrgKey)

        {
            SurveyInfoResponse SurveyInfoResponse = new SurveyInfoResponse();
            try
            {
                
                Epi.Web.Interfaces.DataInterfaces.ISurveyInfoDao ISurveyInfoDao = new EF.EntitySurveyInfoDao();
                Epi.Web.BLL.SurveyInfo Implementation = new Epi.Web.BLL.SurveyInfo(ISurveyInfoDao);
                SurveyInfoResponse.SurveyInfoList = Mapper.ToDataTransferObject(Implementation.GetAllSurveysByOrgKey(OrgKey));
             }
            catch (Exception ex)
            {
               
                throw ex;
            }
                return SurveyInfoResponse;
        }

    }
}
