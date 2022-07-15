using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetAuction.BLL.Contract
{
    /// <summary>
    /// The factory.
    /// </summary>
    public interface IFactory
    {
        T Get<T>();
    }

    /// <summary>
    /// The i crud.
    /// </summary>
    public interface ICrud<TModel, TKey> where TModel : class
    {
        /// <summary>
        /// The get all async.
        /// </summary>
        /// <returns>The result.</returns>
        Task<IEnumerable<TModel>> GetAllAsync();

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The result.</returns>
        Task<TModel> GetByIdAsync(TKey id);

        /// <summary>
        /// The add async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The result.</returns>
        Task AddAsync(TModel model);

        /// <summary>
        /// The update async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The result.</returns>
        Task UpdateAsync(TModel model);

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="modelId">The model id.</param>
        /// <returns>The result.</returns>
        Task DeleteAsync(TKey modelId);

        /// <summary>
        /// Deletes the object asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task DeleteObjectAsync(TModel model);
    }
}