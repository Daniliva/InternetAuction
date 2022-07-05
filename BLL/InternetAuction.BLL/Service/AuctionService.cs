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
    /// The auction service.
    /// </summary>
    public class AuctionService : ICrud<AutctionModel, int>
    {
        private readonly IUnitOfWorkMSSQL unitOfWorkMSSQL;
        private readonly IMapper _mapper;

        public AuctionService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
        {
            this.unitOfWorkMSSQL = unitOfWorkMSSQL;
            _mapper = mapper;
        }

        public async Task AddAsync(AutctionModel model)
        {
            var product = _mapper.Map<AutctionModel, Autction>(model);
            await unitOfWorkMSSQL.AutctionRepository.AddAsync(product);
        }

        public async Task DeleteAsync(int modelId)
        {
            await unitOfWorkMSSQL.AutctionRepository.DeleteByIdAsync(modelId);
        }

        public async Task<IEnumerable<AutctionModel>> GetAllAsync()
        {
            return (await unitOfWorkMSSQL.AutctionRepository.GetAllAsync()).Select((_mapper.Map<Autction, AutctionModel>));
        }

        public async Task<AutctionModel> GetByIdAsync(int id)
        {
            return _mapper.Map<Autction, AutctionModel>(await unitOfWorkMSSQL.AutctionRepository.GetByIdWithIncludeAsync(id));
        }

        public async Task UpdateAsync(AutctionModel model)
        {
            unitOfWorkMSSQL.AutctionRepository.Update(_mapper.Map<AutctionModel, Autction>(model));
            await unitOfWorkMSSQL.SaveAsync();
        }
    }
}