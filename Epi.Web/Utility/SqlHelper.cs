using Epi.Web.EF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Results;
using System.Web.Mvc;
using Epi.Web.MVC.Models;
namespace Epi.Web.MVC.Utility
{
    public class SqlHelper
    {

        public static string GetHeadData(string surveyid)
        {
            SqlConnection conn = new SqlConnection(DataObjectFactory._ADOConnectionString);



            

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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Dictionary<string, string> NewKeys = new Dictionary<string, string>();

                NewKeys.Add("MetaData_ResponseStatus", "MetaData_ResponseStatus");
                foreach (var key in keys) {

                    NewKeys.Add(key, key);

                }
                
                var json = JsonConvert.SerializeObject(NewKeys);

                return json;
            }
            catch (Exception ex)
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

        public static string GetSurveyJsonData(string surveyid , bool IsDraft)
        {
            SqlConnection conn = new SqlConnection(DataObjectFactory._ADOConnectionString);

            StringBuilder json = new StringBuilder("[");

            
                try
                {
                    using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
                    {
                        connection.Open();
                    string commandString = "select ResponseJson , StatusId from SurveyResponse where ResponseJson is not null and surveyid = '" + surveyid + "'" + "and IsDraftMode = '" + IsDraft + "'";
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
                                       newJson.AddFirst(new JProperty("MetaData_ResponseStatus", GetStatus(reader.GetFieldValue<Int32>(1))));
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            

            json.Append("]");

            string retval = json.ToString();

            return retval;
        }


    }
}