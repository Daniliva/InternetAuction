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
	public class LotService : ICrud<LotModel, int>
	{
		private readonly IUnitOfWorkMSSQL unitOfWorkMSSQL;
		private readonly IMapper _mapper;

		public LotService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
		{
			this.unitOfWorkMSSQL = unitOfWorkMSSQL;
			_mapper = mapper;
		}

		public async Task AddAsync(LotModel model)
		{
			var product = _mapper.Map<LotModel, Lot>(model);
			await unitOfWorkMSSQL.LotRepository.AddAsync(product);
		}

		public async Task DeleteAsync(int modelId)
		{
			await unitOfWorkMSSQL.LotRepository.DeleteByIdAsync(modelId);
		}

		public Task DeleteObjectAsync(LotModel model)
		{
			throw new System.NotImplementedException();
		}

		public async Task<IEnumerable<LotModel>> GetAllAsync()
		{
			return (await unitOfWorkMSSQL.LotRepository.GetAllAsync()).Select((_mapper.Map<Lot, LotModel>));
		}

		public async Task<LotModel> GetByIdAsync(int id)
		{
			return _mapper.Map<Lot, LotModel>(await unitOfWorkMSSQL.LotRepository.GetByIdWithIncludeAsync(id));
		}

		public async Task UpdateAsync(LotModel model)
		{
			unitOfWorkMSSQL.LotRepository.Update(_mapper.Map<LotModel, Lot>(model));
			await unitOfWorkMSSQL.SaveAsync();
		}
	}
}