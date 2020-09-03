using Epi.Web.Common.Exception;
using Epi.Web.Common.Message;
using Epi.Web.MVC.DataServiceClient;
using Epi.Web.MVC.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Epi.Web.MVC.Repositories
{
    public class ReportInfoRepository : IReportRepository
    {
        private Epi.Web.WCF.SurveyService.IDataService _iDataService;
        public ReportInfoRepository(Epi.Web.WCF.SurveyService.IDataService iDataService)
        {
            _iDataService = iDataService;
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public PublishReportResponse Get(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public PublishReportResponse GetSurveyReport(PublishReportRequest publishReportRequest)
        {
            try
            {
                PublishReportResponse Response = _iDataService.GetSurveyReport(publishReportRequest);
                return Response;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PublishReportResponse GetSurveyReportList(PublishReportRequest publishReportRequest)
        {
            try
            {
                PublishReportResponse Response = _iDataService.GetSurveyReportList(publishReportRequest);
                return Response;
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<PublishReportResponse> GetList(Criterion criterion = null)
        {
            throw new NotImplementedException();
        }

        public void Insert(PublishReportResponse t)
        {
            throw new NotImplementedException();
        }

        public void Update(PublishReportResponse t)
        {
            throw new NotImplementedException();
        }
    }
}