using AutoMapper;
using InternetAuction.BLL.Contract;
using InternetAuction.BLL.Contract.Validation;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionService"/> class.
        /// </summary>
        /// <param name="unitOfWorkMSSQL">The unit of work MSSQL.</param>
        /// <param name="mapper">The mapper.</param>
        public AuctionService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
        {
            this.unitOfWorkMSSQL = unitOfWorkMSSQL;
            _mapper = mapper;
        }

        /// <summary>
        /// The add async.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task AddAsync(AutctionModel model)
        {
            var product = _mapper.Map<AutctionModel, Autction>(model);
            if (!ModelValidation.AuctionCheck(product))
            {
                throw new InternetException("Some problem, please check your info!");
            }

            await unitOfWorkMSSQL.AutctionRepository.AddAsync(product);
            await unitOfWorkMSSQL.SaveAsync();
        }

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="modelId">The model id.</param>
        public async Task DeleteAsync(int modelId)
        {
            await unitOfWorkMSSQL.AutctionRepository.DeleteByIdAsync(modelId);
        }

        /// <summary>
        /// Deletes the object asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task DeleteObjectAsync(AutctionModel model)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// The get all async.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public async Task<IEnumerable<AutctionModel>> GetAllAsync()
        {
            return (await unitOfWorkMSSQL.AutctionRepository.GetAllAsync()).Select((_mapper.Map<Autction, AutctionModel>));
        }

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public async Task<AutctionModel> GetByIdAsync(int id)
        {
            return _mapper.Map<Autction, AutctionModel>(await unitOfWorkMSSQL.AutctionRepository.GetByIdWithIncludeAsync(id));
        }

        /// <summary>
        /// The update async.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task UpdateAsync(AutctionModel model)
        {
            var product = _mapper.Map<AutctionModel, Autction>(model);
            if (!ModelValidation.AuctionCheck(product))
            {
                throw new InternetException("Some problem, please check your info!");
            }
            unitOfWorkMSSQL.AutctionRepository.Update(product);

            await unitOfWorkMSSQL.SaveAsync();
        }
    }
}