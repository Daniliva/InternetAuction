using InternetAuction.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternetAuction.DAL.Contract
{
    public delegate bool IsEqual(object x);

    /// <summary>
    /// The repository ms sql.
    /// </summary>
    public interface IRepositoryMsSql<TEntity, in TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(TKey id);

        Task<TEntity> GetByFiltererAsync(IsEqual func);

        Task<TEntity> GetByIdWithIncludeAsync(TKey id);

        Task AddAsync(TEntity entity);

        void Delete(TEntity entity);

        Task DeleteByIdAsync(TKey id);

        void Update(TEntity entity);
    }
}