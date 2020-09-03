using Epi.Web.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epi.Web.MVC.Repositories.Core
{
    public interface IReportRepository : IRepository<Epi.Web.Common.Message.PublishReportResponse>
    {
        PublishReportResponse GetSurveyReportList(PublishReportRequest publishReportRequest);
        PublishReportResponse GetSurveyReport(PublishReportRequest publishReportRequest);
    }
}
