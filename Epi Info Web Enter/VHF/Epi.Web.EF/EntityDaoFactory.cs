using System;
using System.Collections.Generic;
using System.Text;
using Epi.Web.Enter.Interfaces.DataInterfaces;
using Epi.Web.Enter.Interfaces.DataInterface;

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

        public IFormInfoDao FormInfoDao
            {
            get { return new  EntityFormInfoDao(); }
            }

        public ISurveyResponseDao SurveyResponseDao
        {
            get { return new EntitySurveyResponseDao(); }  
        }

        public IOrganizationDao OrganizationDao {
            get{return new EntityOrganizationDao();}
        
        }

        public IFormSettingDao FormSettingDao
            {
            get { return new EntityFormSettingDao(); }

            }

        public IUserDao UserDao
        {
            get { return new EntityUserDao(); }
        }
    }
}
