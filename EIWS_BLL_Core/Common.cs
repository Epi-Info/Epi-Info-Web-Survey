using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
namespace Epi.Web.BLL
{
    public class Common
    {

        //SurveyResponseBO
        public static PageInfoBO GetSurveySize(List<SurveyResponseBO> resultRows, int BandwidthUsageFactor, int ResponseMaxSize = -1)
        {

           PageInfoBO result = new PageInfoBO();

            int NumberOfRows = 0;
            int ResponsesTotalsize = 0;
            decimal AvgResponseSize = 0;
            decimal NumberOfResponsPerPage = 0;

            if (resultRows.Count > 0)
            {
                NumberOfRows = resultRows.Count;
                ResponsesTotalsize = (int)resultRows.Select(x => x.TemplateXMLSize).Sum();

                AvgResponseSize = (int)resultRows.Select(x => x.TemplateXMLSize).Average();
               // NumberOfResponsPerPage = (int)Math.Ceiling((ResponseMaxSize * (BandwidthUsageFactor/100)) / AvgResponseSize);

                NumberOfResponsPerPage = (int)Math.Ceiling((int)(ResponseMaxSize * (BandwidthUsageFactor * 0.01)) / AvgResponseSize);
                result.PageSize = (int)Math.Ceiling(NumberOfResponsPerPage);
                result.NumberOfPages = (int)Math.Ceiling(NumberOfRows / NumberOfResponsPerPage);
            }



            return result;
        }

        //SurveyInfoBO
        public static PageInfoBO GetSurveySize(List<SurveyInfoBO> resultRows, int BandwidthUsageFactor, int ResponseMaxSize = -1)
        {

            PageInfoBO result = new PageInfoBO();

            int NumberOfRows = 0;
            int ResponsesTotalsize = 0;
            decimal AvgResponseSize = 0;
            decimal NumberOfResponsPerPage = 0;

            if (resultRows.Count > 0)
            {
                NumberOfRows = resultRows.Count;
                ResponsesTotalsize = (int)resultRows.Select(x => x.TemplateXMLSize).Sum();

                AvgResponseSize = (int)resultRows.Select(x => x.TemplateXMLSize).Average();
               // NumberOfResponsPerPage = (int)Math.Ceiling((ResponseMaxSize * (BandwidthUsageFactor / 100)) / AvgResponseSize);

                NumberOfResponsPerPage = (int)Math.Ceiling((int)(ResponseMaxSize * (BandwidthUsageFactor * 0.01)) / AvgResponseSize);

                result.PageSize = (int)Math.Ceiling(NumberOfResponsPerPage);
                result.NumberOfPages = (int)Math.Ceiling(NumberOfRows / NumberOfResponsPerPage);
            }



            return result;
        }
        
         

    }
}
