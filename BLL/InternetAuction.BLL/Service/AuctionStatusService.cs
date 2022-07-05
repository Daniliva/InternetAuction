using AutoMapper;
using InternetAuction.BLL.Contract;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetAuction.BLL.Service
{
    /// <summary>
    /// The auction status service.
    /// </summary>
    public class AuctionStatusService : ICrud<AutctionStatusModel, int>
    {
        private readonly IUnitOfWorkMSSQL unitOfWorkMSSQL;
        private readonly IMapper _mapper;

        public AuctionStatusService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
        {
            this.unitOfWorkMSSQL = unitOfWorkMSSQL;
            _mapper = mapper;
        }

        public async Task AddAsync(AutctionStatusModel model)
        {
            var product = _mapper.Map<AutctionStatusModel, AutctionStatus>(model);
            await unitOfWorkMSSQL.AutctionStatusRepository.AddAsync(product);
        }

        public async Task DeleteAsync(int modelId)
        {
            await unitOfWorkMSSQL.AutctionStatusRepository.DeleteByIdAsync(modelId);
        }

        public async Task<IEnumerable<AutctionStatusModel>> GetAllAsync()
        {
            return (await unitOfWorkMSSQL.AutctionStatusRepository.GetAllAsync()).Select((_mapper.Map<AutctionStatus, AutctionStatusModel>));
        }

        public async Task<AutctionStatusModel> GetByIdAsync(int id)
        {
            return _mapper.Map<AutctionStatus, AutctionStatusModel>(await unitOfWorkMSSQL.AutctionStatusRepository.GetByIdWithIncludeAsync(id));
        }

        public async Task UpdateAsync(AutctionStatusModel model)
        {
            unitOfWorkMSSQL.AutctionStatusRepository.Update(_mapper.Map<AutctionStatusModel, AutctionStatus>(model));
            await unitOfWorkMSSQL.SaveAsync();
        }
    }
}