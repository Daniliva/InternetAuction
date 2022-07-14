using AutoMapper;
using InternetAuction.BLL.Contract;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetAuction.BLL.Service
{
	/// <summary>
	/// The RoleUser user service.
	/// </summary>
	public class RoleUserService : ICrud<RoleUserModel, string>
	{
		private readonly IUnitOfWorkMSSQL unitOfWorkMSSQL;
		private readonly IMapper _mapper;

		public RoleUserService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
		{
			this.unitOfWorkMSSQL = unitOfWorkMSSQL;
			_mapper = mapper;
		}

		public async Task AddAsync(RoleUserModel model)
		{
			var product = _mapper.Map<RoleUserModel, RoleUser>(model);

			// product.UserId = product.UserId;
			// product.RoleId = product.RoleId;
			product.Roles = null;
			product.Users = null;
			await unitOfWorkMSSQL.RoleUserRepository.AddAsync(product);
			await unitOfWorkMSSQL.SaveAsync();
		}

		public async Task DeleteAsync(string modelId)
		{
			await unitOfWorkMSSQL.RoleUserRepository.DeleteByIdAsync(modelId);
			await unitOfWorkMSSQL.SaveAsync();
		}

		public async Task<IEnumerable<RoleUserModel>> GetAllAsync()
		{
			return (await unitOfWorkMSSQL.RoleUserRepository.GetAllAsync()).Select((_mapper.Map<RoleUser, RoleUserModel>));
		}

		public async Task<RoleUserModel> GetByIdAsync(string id)
		{
			return _mapper.Map<RoleUser, RoleUserModel>(await unitOfWorkMSSQL.RoleUserRepository.GetByIdWithIncludeAsync(id));
		}

		public async Task UpdateAsync(RoleUserModel model)
		{
			unitOfWorkMSSQL.RoleUserRepository.Update(_mapper.Map<RoleUserModel, RoleUser>(model));
			await unitOfWorkMSSQL.SaveAsync();
		}

		public async Task DeleteObjectAsync(RoleUserModel model)
		{
			var product = _mapper.Map<RoleUserModel, RoleUser>(model);
			product.Roles = null;
			product.Users = null;
			unitOfWorkMSSQL.RoleUserRepository.Delete(product);
			await unitOfWorkMSSQL.SaveAsync();
		}
	}
}