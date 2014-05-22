﻿using System.Configuration;
using System;
using Epi.Web.Common.Security; 

namespace Epi.Web.EF
{
    /// <summary>
    /// DataObjectFactory caches the connectionstring so that the context can be created quickly.
    /// </summary>
    public static class DataObjectFactory
    {
        private static readonly string _connectionString;

        /// <summary>
        /// Static constructor. Reads the connectionstring from web.config just once.
        /// </summary>
        static DataObjectFactory()
        {
            try
            {
                //  string connectionStringName = ConfigurationManager.AppSettings.Get("ConnectionStringName");
                string connectionStringName = "EIWSEntities";

                //Decrypt connection string here
                _connectionString = Cryptography.Decrypt(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
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
        public static Epi.Web.EF.EIWSEntities CreateContext()
        {
            return new Epi.Web.EF.EIWSEntities(_connectionString);
        }
        public static Epi.Web.EF.SurveyMetaData CreateSurveyMetaData()
        {
            return new Epi.Web.EF.SurveyMetaData();
        }

        public static Epi.Web.EF.SurveyResponse CreateSurveyResponse()
        {
            return new Epi.Web.EF.SurveyResponse();
        }
    }
}
