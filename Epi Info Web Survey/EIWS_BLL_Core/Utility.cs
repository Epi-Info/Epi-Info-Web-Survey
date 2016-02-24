using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using System.Configuration;
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
                if (AvgResponseSize == 0)
                {
                    AvgResponseSize = 1;
                }
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



        //Validate Admin
        public static bool ValidateAdmin(string AdminKeyToValidate)
        {
            string AdminKey = ConfigurationManager.AppSettings["AdminKey"];
            string EncryptedAdminKey = Epi.Web.Common.Security.Cryptography.Decrypt(AdminKey);

            bool ISValidUser = false;

            if (!string.IsNullOrEmpty(EncryptedAdminKey) && !string.IsNullOrEmpty(AdminKeyToValidate))
            {

                if (EncryptedAdminKey == AdminKeyToValidate)
                {
                    ISValidUser = true;


                }
                else
                {
                    ISValidUser = false;
                }
            }
            return ISValidUser;
        }

        



    }
}
