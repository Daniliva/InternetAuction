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

        /// <summary>
        /// Initializes a new instance of the <see cref="BiddingService"/> class.
        /// </summary>
        /// <param name="unitOfWorkMSSQL">The unit of work MSSQL.</param>
        /// <param name="mapper">The mapper.</param>
        public BiddingService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
        {
            this.unitOfWorkMSSQL = unitOfWorkMSSQL;
            _mapper = mapper;
        }

        /// <summary>
        /// The add async.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task AddAsync(BiddingModel model)
        {
            var product = _mapper.Map<BiddingModel, Bidding>(model);
            await unitOfWorkMSSQL.BiddingRepository.AddAsync(product);
        }

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="modelId">The model id.</param>
        public async Task DeleteAsync(int modelId)
        {
            await unitOfWorkMSSQL.BiddingRepository.DeleteByIdAsync(modelId);
            await unitOfWorkMSSQL.SaveAsync();
        }

        /// <summary>
        /// Deletes the object asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task DeleteObjectAsync(BiddingModel model)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// The get all async.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public async Task<IEnumerable<BiddingModel>> GetAllAsync()
        {
            return (await unitOfWorkMSSQL.BiddingRepository.GetAllAsync()).Select((_mapper.Map<Bidding, BiddingModel>));
        }

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public async Task<BiddingModel> GetByIdAsync(int id)
        {
            var bidding = await unitOfWorkMSSQL.BiddingRepository.GetByIdWithIncludeAsync(id);
            return _mapper.Map<Bidding, BiddingModel>(bidding);
        }

        /// <summary>
        /// The update async.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task UpdateAsync(BiddingModel model)
        {
            unitOfWorkMSSQL.BiddingRepository.Update(_mapper.Map<BiddingModel, Bidding>(model));
            await unitOfWorkMSSQL.SaveAsync();
        }
    }
}