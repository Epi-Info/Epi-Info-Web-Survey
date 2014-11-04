using System.Configuration;
using System;
using Epi.Web.Enter.Common.Security;

namespace Epi.Web.EF
{
    /// <summary>
    /// DataObjectFactory caches the connectionstring so that the context can be created quickly.
    /// </summary>
    public static class DataObjectFactory
    {
        private static readonly string _connectionString;

        private static readonly string _eweAdoConnectionString;

        /// <summary>
        /// Static constructor. Reads the connectionstring from web.config just once.
        /// </summary>
        static DataObjectFactory()
        {
            try
            {
                //  string connectionStringName = ConfigurationManager.AppSettings.Get("ConnectionStringName");
                string connectionStringName = "EIWSEntities";

                string EWEADOconnectionStringName = "EWEADO";

                //Decrypt connection string here
                _connectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
                _eweAdoConnectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings[EWEADOconnectionStringName].ConnectionString);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Creates the Context using the current connectionstring.
        /// </summary>
        /// <remarks>
        /// Gof pattern: Factory method. 
        /// </remarks>
        /// <returns>Action Entities context.</returns>
        public static Epi.Web.EF.OSELS_EWEEntities CreateContext()
        {
            return new Epi.Web.EF.OSELS_EWEEntities(_connectionString);
        }
        public static Epi.Web.EF.SurveyMetaData CreateSurveyMetaData()
        {
            return new Epi.Web.EF.SurveyMetaData();
        }

        public static Epi.Web.EF.SurveyResponse CreateSurveyResponse()
        {
            return new Epi.Web.EF.SurveyResponse();
        }

        /// <summary>
        /// Property to read connection string without meta information
        /// </summary>
        public static string EWEADOConnectionString
        {
            get
            {
                //string connstr = _connectionString.Substring(_connectionString.IndexOf("connection string"));
                //connstr = connstr.Replace("connection string=", "");
                //connstr = connstr.Replace("\"", "");
                //return connstr;

                return _eweAdoConnectionString;
            }
        }
    }
}
