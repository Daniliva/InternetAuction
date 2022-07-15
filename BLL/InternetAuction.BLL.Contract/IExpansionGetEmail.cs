using System.Threading.Tasks;

namespace InternetAuction.BLL.Contract
{
    /// <summary>
    /// The user service.
    /// </summary>
    public interface IExpansionGetEmail<TModel, TKey> : ICrud<TModel, TKey> where TModel : class
    {
        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<TModel> GetByEmail(string email);
    }
}