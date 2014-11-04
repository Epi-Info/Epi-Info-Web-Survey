using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
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
                ResponsesTotalsize = GetResponsesTotalsize(resultRows);

                AvgResponseSize = GetAvgResponseSize(resultRows);
              
               // NumberOfResponsPerPage = (int)Math.Ceiling((ResponseMaxSize * (BandwidthUsageFactor/100)) / AvgResponseSize);

                NumberOfResponsPerPage = (int)Math.Ceiling((int)(ResponseMaxSize * (BandwidthUsageFactor * 0.01)) / AvgResponseSize);
                result.PageSize = (int)Math.Ceiling(NumberOfResponsPerPage);
                result.NumberOfPages = (int)Math.Ceiling(NumberOfRows / NumberOfResponsPerPage);
            }



            return result;
        }

        private static decimal GetAvgResponseSize(List<SurveyResponseBO> resultRows)
            {
            int Average =0;
            List<int> ResponseSizeList = new List<int>();
            foreach (var item in resultRows)
                {
                //ResponseSizeList.Add ((int)item.ResponseHierarchyIds.Select(x => x.TemplateXMLSize).Average());
                ResponseSizeList.Add((int)item.ResponseHierarchyIds.Select(x => x.TemplateXMLSize).Sum());
                }
            Average = (int)ResponseSizeList.Average();

            return Average;
            }

        private static int GetResponsesTotalsize(List<SurveyResponseBO> resultRows)
            {
            int Sum = 0;
            //Sum = (int)resultRows.Select(x => x.TemplateXMLSize).Sum();
            foreach (var item in resultRows)
            {

            Sum = Sum + (int)item.ResponseHierarchyIds.Select(x => x.TemplateXMLSize).Sum();
                
            }
            return Sum;
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
            string EncryptedAdminKey = Epi.Web.Enter.Common.Security.Cryptography.Decrypt(AdminKey);

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
