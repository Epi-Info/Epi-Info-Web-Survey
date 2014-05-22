using System;
using System.Collections.Generic;
using System.Text;
using Epi.Web.Interfaces.DataInterfaces;

namespace Epi.Web.EF
{
    /// <summary>
    /// Entity Framework specific factory that creates data access objects.
    /// </summary>
    /// <remarks>
    /// GoF Design Patterns: Factory.
    /// </remarks>
    public class EntityDaoFactory : IDaoFactory
    {
        /// <summary>
        /// Gets an Entity Framework specific Sur data access object.
        /// </summary>
        public ISurveyInfoDao SurveyInfoDao
        {
            get { return new EntitySurveyInfoDao(); }
        }


        public ISurveyResponseDao SurveyResponseDao
        {
            get { return new EntitySurveyResponseDao(); }  
        }

        public IOrganizationDao OrganizationDao {
            get{return new EntityOrganizationDao();}
        
        }
    }
}
