//using Epi.Web.EF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
//using System.ServiceModel.Channels;
using System.Text;
using System.Web;
//using System.Web.Helpers;
//using System.Web.Http.Results;
//using System.Web.Mvc;
using Epi.Web.Common.Json;
using Epi.Web.Common.Security;
using System.Configuration;

namespace Epi.Web.Common.Helper
{
    public class SqlHelper
    {

        public static string GetHeadData(string surveyid)
        {
          var  _ADOConnectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings["EIWSADO"].ConnectionString);
            SqlConnection conn = new SqlConnection(_ADOConnectionString);



            

            try
            {

                IEnumerable<string> keys = new List<string>();


                try
                {
                    using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
                    {
                        connection.Open();
                        string commandString = "select ResponseJson from SurveyResponse where ResponseJson is not null and surveyid = '" + surveyid + "'";
                        using (SqlCommand command = new SqlCommand(commandString, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string row = JsonConvert.DeserializeObject<JsonMessage>(reader.GetFieldValue<string>(0)).ResponseQA.ToString();
                                    JObject obj = JObject.Parse(row);

                                    keys = keys.Union(obj.Properties().Select(p => p.Name).ToList());
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Dictionary<string, string> NewKeys = new Dictionary<string, string>();
                NewKeys.Add("MetaData_DateUpdated", "MetaData_DateUpdated");
                NewKeys.Add("MetaData_ResponseId", "MetaData_ResponseId");
                NewKeys.Add("MetaData_ResponseStatus", "MetaData_ResponseStatus");
                
                foreach (var key in keys) {

                    NewKeys.Add(key, key);

                }
                
                var json = JsonConvert.SerializeObject(NewKeys);

                return json;
            }
            catch (System.Exception ex)
            {
                return null;
            }

        }

        private static string GetStatus(int responseStatuse)
        {
            switch (responseStatuse)
            {
                case 1:
                    return "InProgress";
                    break;
                case 2:
                    return "Saved";
                    break;
                case 3:
                    return "Submited";
                    break;
                case 4:
                    return "Downloadeded";
                    break;
                default:
                    return "";
                    break;
            }
        }

        public static string GetSurveyJsonData(string surveyid , bool IsDraft , int PageSize , int PageNumber )
        {
            var _ADOConnectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings["EIWSADO"].ConnectionString);
            SqlConnection conn = new SqlConnection(_ADOConnectionString);

            StringBuilder json = new StringBuilder("[");

            int OffSet = 0;
            
                try
                {
                    using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
                    {
                        connection.Open();
                    string commandString = "";
                    if (PageNumber == 1) {
                          commandString = "select top("+ PageSize + ")ResponseJson , StatusId , ResponseId, DateUpdated, DateCreated from SurveyResponse where ResponseJson is not null and surveyid = '" + surveyid + "'" + "and IsDraftMode = '" + IsDraft + "' order by DateCreated ";
                    }
                    else {
                        OffSet = PageSize * (PageNumber-1);
                          commandString = "select ResponseJson , StatusId , ResponseId, DateUpdated, DateCreated from SurveyResponse where ResponseJson is not null and surveyid = '" + surveyid + "'" + "and IsDraftMode = '" + IsDraft + "' order by DateCreated OFFSET " + OffSet + " ROWS FETCH NEXT " + PageSize  + " ROWS ONLY";

                    }

                    // string commandString = "select ResponseJson from SurveyResponse r inner join SurveyMetaData m on r.SurveyId = m.SurveyId inner join UserOrganization uo on m.OrganizationId = uo.OrganizationID inner join [User] u on uo.UserID = u.UserID where r.ResponseJson is not null and r.SurveyId = '" + surveyid + "' and u.UserName = '" + userName + "' order by DateUpdated desc";
                    using (SqlCommand command = new SqlCommand(commandString, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        string row = JsonConvert.DeserializeObject<JsonMessage>(reader.GetFieldValue<string>(0)).ResponseQA.ToString();
                                       JObject newJson =   JObject.Parse(row);
                                    newJson.AddFirst(new JProperty("MetaData_DateUpdated",  reader.GetFieldValue<DateTime>(3)));
                                    newJson.AddFirst(new JProperty("MetaData_ResponseStatus", GetStatus(reader.GetFieldValue<Int32>(1))));
                                       newJson.AddFirst(new JProperty("MetaData_ResponseId", reader.GetFieldValue<Guid>(2)));
                                    row = newJson.ToString();
                                        if (!row.Equals("{}"))
                                        {
                                            json.Append(row);
                                            json.Append(",");
                                        }
                                    }
                                    if (json.Length > 1)
                                    {
                                        json.Remove(json.Length - 1, 1);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            

            json.Append("]");

            string retval = json.ToString();

            return retval;
        }
        public static string GetJsonResponseAll(string surveyid)
        {
            var _ADOConnectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings["EIWSADO"].ConnectionString);
            SqlConnection conn = new SqlConnection(_ADOConnectionString);

            StringBuilder json = new StringBuilder("[");


            try
            {
                using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
                {
                    connection.Open();
                    string commandString = "select ResponseJson , StatusId , ResponseId , DateUpdated from SurveyResponse where ResponseJson is not null and surveyid = '" + surveyid + "'";
                    // string commandString = "select ResponseJson from SurveyResponse r inner join SurveyMetaData m on r.SurveyId = m.SurveyId inner join UserOrganization uo on m.OrganizationId = uo.OrganizationID inner join [User] u on uo.UserID = u.UserID where r.ResponseJson is not null and r.SurveyId = '" + surveyid + "' and u.UserName = '" + userName + "' order by DateUpdated desc";
                    using (SqlCommand command = new SqlCommand(commandString, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string row = JsonConvert.DeserializeObject<JsonMessage>(reader.GetFieldValue<string>(0)).ResponseQA.ToString();
                                    JObject newJson = JObject.Parse(row);
                                    newJson.AddFirst(new JProperty("MetaData_DateUpdated", reader.GetFieldValue<DateTime>(3)));
                                    newJson.AddFirst(new JProperty("MetaData_ResponseStatus", GetStatus(reader.GetFieldValue<Int32>(1))));
                                    newJson.AddFirst(new JProperty("MetaData_ResponseId", reader.GetFieldValue<Guid>(2)));
                                    row = newJson.ToString();
                                    if (!row.Equals("{}"))
                                    {
                                        json.Append(row);
                                        json.Append(",");
                                    }
                                }
                                if (json.Length > 1)
                                {
                                    json.Remove(json.Length - 1, 1);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            json.Append("]");

            string retval = json.ToString();

            return retval;
        }

        public static string GetReportXml(string ReportID) 
        {

            var _ADOConnectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings["EIWSADO"].ConnectionString);
            SqlConnection conn = new SqlConnection(_ADOConnectionString);


            string xml = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
                {
                    connection.Open();
                    string commandString = "select ReportXml from SurveyReportsInfo where Reportid = '" + ReportID + "'";
                   
                    using (SqlCommand command = new SqlCommand(commandString, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                   
                                    xml = reader.GetFieldValue<string>(0);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

 

            return xml;

        }

        public static int GetResponseJsonSize(string surveyid, bool isDraft)
        {
            var _ADOConnectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings["EIWSADO"].ConnectionString);
            SqlConnection conn = new SqlConnection(_ADOConnectionString);


         
            string size = "0";
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
                {
                    connection.Open();
                    string commandString = "select sum(ResponseJsonSize) as Size from SurveyResponse where Surveyid = '" + surveyid + "' and IsDraftMode ='" + isDraft  + "'";

                    using (SqlCommand command = new SqlCommand(commandString, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                //while (reader.Read())

                                    if (reader.Read())
                                    size = reader["Size"].ToString();
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



            return int.Parse(size);
        }

        public static int GetAvgResponseSize(string surveyid, bool isDraft)
        {
            var _ADOConnectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings["EIWSADO"].ConnectionString);
            SqlConnection conn = new SqlConnection(_ADOConnectionString);



            string AvgSize = "0";
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
                {
                    connection.Open();
                    string commandString = "select avg(ResponseJsonSize) as AvgSize from SurveyResponse where Surveyid = '" + surveyid + "' and IsDraftMode ='" + isDraft + "'";

                    using (SqlCommand command = new SqlCommand(commandString, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                //while (reader.Read())

                                if (reader.Read())
                                    AvgSize = reader["AvgSize"].ToString();
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



            return int.Parse(AvgSize);
        }

        public static int GetResponseCount(string surveyid, bool isDraft)
        {
            var _ADOConnectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings["EIWSADO"].ConnectionString);
            SqlConnection conn = new SqlConnection(_ADOConnectionString);



            string Count = "0";
            try
            {
                using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
                {
                    connection.Open();
                    string commandString = "select count(ResponseJsonSize) as Count from SurveyResponse where Surveyid = '" + surveyid + "' and IsDraftMode ='" + isDraft + "'";

                    using (SqlCommand command = new SqlCommand(commandString, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                //while (reader.Read())

                                if (reader.Read())
                                    Count = reader["Count"].ToString();
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



            return int.Parse(Count);
        }
    }
}