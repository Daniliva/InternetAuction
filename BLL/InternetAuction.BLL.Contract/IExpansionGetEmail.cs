﻿using System.Threading.Tasks;

namespace InternetAuction.BLL.Contract
{
    /// <summary>
    /// The user service.
    /// </summary>
    public interface IExpansionGetEmail<TModel, TKey> : ICrud<TModel, TKey> where TModel : class
	{
		Task<TModel> GetByEmail(string email);
	}
}