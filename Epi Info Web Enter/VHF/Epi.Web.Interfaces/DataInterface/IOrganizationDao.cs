using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;

namespace Epi.Web.Enter.Interfaces.DataInterfaces
{
    /// <summary>
    /// Defines methods to access Organizations.
    /// </summary>
    /// <remarks>
    /// This is a database-independent interface. Implementations are database specific.
    /// </remarks>
    public interface IOrganizationDao
    {
        /// <summary>
        /// Gets a specific Organization.
        /// </summary>
        /// <param name="OrganizationId">Unique Organization identifier.</param>
        /// <returns>Organization.</returns>
        List<OrganizationBO> GetOrganizationKeys(string OrganizationName );
        List<OrganizationBO> GetOrganizationInfoByOrgKey(string gOrgKeyEncrypted);
        List<OrganizationBO> GetOrganizationInfo();
        List<OrganizationBO> GetOrganizationNames();
        OrganizationBO GetOrganizationInfoByKey(string key);
     //   /// <summary>
     //   /// Gets a specific Organization.
     //   /// </summary>
     //   /// <param name="OrganizationId">Unique Organization identifier.</param>
     //   /// <returns>Organization.</returns>
     //   List<OrganizationBO> GetOrganizationBySurveyId(List<string> SurveyIdList, Guid UserPublishKey);


        
     //   /// <summary>
     //   /// Get Organizations based on criteria.
     //   /// </summary>
     //   /// <param name="OrganizationId">Unique Organization identifier.</param>
     //   /// <returns>Organization.</returns>
     //   List<OrganizationBO> GetOrganization(List<string> SurveyAnswerIdList, string pSurveyId, DateTime pDateCompleted, int pStatusId = -1 );


     //   /// <summary>
     //   /// Gets a sorted list of all Organizations.
     //   /// </summary>
     //   /// <param name="sortExpression">Sort order.</param>
     //   /// <returns>Sorted list of Organizations.</returns>
     //  // List<OrganizationBO> GetOrganizations(string sortExpression = "OrganizationId ASC");
        
     //   /// <summary>
     //   /// Gets Organization given an order.
     //   /// </summary>
     //   /// <param name="orderId">Unique order identifier.</param>
     //   /// <returns>Organization.</returns>
     //  // OrganizationBO GetOrganizationByOrder(int orderId);

     //   /// <summary>
     //   /// Gets Organizations with order statistics in given sort order.
     //   /// </summary>
     //   /// <param name="Organizations">Organization list.</param>
     //   /// <param name="sortExpression">Sort order.</param>
     //   /// <returns>Sorted list of Organizations with order statistics.</returns>
     ////   List<OrganizationBO> GetOrganizationsWithOrderStatistics(string sortExpression);

        /// <summary>
        /// Inserts a new Organization. 
        /// </summary>
        /// <remarks>
        /// Following insert, Organization object will contain the new identifier.
        /// </remarks>
        /// <param name="Organization">Organization.</param>
        void InsertOrganization(OrganizationBO Organization);

        /// <summary>
        /// Updates a Organization.
        /// </summary>
        /// <param name="Organization">Organization.</param>
        bool UpdateOrganization(OrganizationBO Organization);

        /// <summary>
        /// Deletes a Organization
        /// </summary>
        /// <param name="Organization">Organization.</param>
         void DeleteOrganization(OrganizationBO Organization);


         List<OrganizationBO> GetOrganizationInfoByUserId(int UserId, int UserRole);

        /// <summary>
        /// Brings all the organizations user is part of.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
         List<OrganizationBO> GetOrganizationsByUserId(int UserId);

         List<OrganizationBO> GetOrganizationInfoForAdmin(int UserId, int UserRole);
         bool InsertOrganization(OrganizationBO Organization,UserBO User);
         UserBO GetUserByEmail(UserBO User);
         bool InsertOrganization(OrganizationBO Organization, int UserId,int RoleId);
         

         OrganizationBO GetOrganizationByOrgId(int OrganizationId);
    }
}
