using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.MVC.Repositories.Core
{
    /// <summary>
    /// Generic repository base interface. 
    /// Provides basic CRUD (Create, Read, Update, Delete) and a couple more methods.
    /// </summary>
    /// <typeparam name="T">The business object type.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Reads a list of items.
        /// </summary>
        /// <param name="criterion">The order and filter criteria.</param>
        /// <returns></returns>
        List<T> GetList(Criterion criterion = null);

        /// <summary>
        /// Reads an individual item.
        /// </summary>
        /// <param name="id">The business object's id.</param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// Get a count of the number of items.
        /// </summary>
        /// <param name="criterion">The order and filter criteria</param>
        /// <returns></returns>
        int GetCount(Criterion criterion = null);

        /// <summary>
        /// Inserts a new item.
        /// </summary>
        /// <param name="t">The business object. </param>
        void Insert(T t);

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        /// <param name="t">The business object.</param>
        void Update(T t);

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="id">The business object's id</param>
        void Delete(int id);
    }
}
