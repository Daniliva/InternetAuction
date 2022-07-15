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
    /// The bidding service.
    /// </summary>
    public class BiddingService : ICrud<BiddingModel, int>
    {
        private readonly IUnitOfWorkMSSQL unitOfWorkMSSQL;
        private readonly IMapper _mapper;

        public BiddingService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
        {
            this.unitOfWorkMSSQL = unitOfWorkMSSQL;
            _mapper = mapper;
        }

        public async Task AddAsync(BiddingModel model)
        {
            var product = _mapper.Map<BiddingModel, Bidding>(model);
            await unitOfWorkMSSQL.BiddingRepository.AddAsync(product);
        }

        public async Task DeleteAsync(int modelId)
        {
            await unitOfWorkMSSQL.BiddingRepository.DeleteByIdAsync(modelId); 
            await unitOfWorkMSSQL.SaveAsync();
        }

        public Task DeleteObjectAsync(BiddingModel model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<BiddingModel>> GetAllAsync()
        {
            return (await unitOfWorkMSSQL.BiddingRepository.GetAllAsync()).Select((_mapper.Map<Bidding, BiddingModel>));
        }

        public async Task<BiddingModel> GetByIdAsync(int id)
        {
            var bidding = await unitOfWorkMSSQL.BiddingRepository.GetByIdWithIncludeAsync(id);
            return _mapper.Map<Bidding, BiddingModel>(bidding);
        }

        public async Task UpdateAsync(BiddingModel model)
        {
            unitOfWorkMSSQL.BiddingRepository.Update(_mapper.Map<BiddingModel, Bidding>(model));
            await unitOfWorkMSSQL.SaveAsync();
        }
    }
}