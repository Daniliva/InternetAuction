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
    public class LotService : ICrud<LotModel, int>
    {
        private readonly IUnitOfWorkMSSQL unitOfWorkMSSQL;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LotService"/> class.
        /// </summary>
        /// <param name="unitOfWorkMSSQL">The unit of work MSSQL.</param>
        /// <param name="mapper">The mapper.</param>
        public LotService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
        {
            this.unitOfWorkMSSQL = unitOfWorkMSSQL;
            _mapper = mapper;
        }

        /// <summary>
        /// The add async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="InternetAuction.BLL.Contract.Validation.InternetException">Some problem, please check your info!</exception>
        public async Task AddAsync(LotModel model)
        {
            var product = _mapper.Map<LotModel, Lot>(model);
            var product = _mapper.Map<LotModel, Lot>(model);
            if (!ModelValidation.LotCheck(product))
            {
                throw new InternetException("Some problem, please check your info!");
            }
            await unitOfWorkMSSQL.LotRepository.AddAsync(product);
        }

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="modelId">The model id.</param>
        public async Task DeleteAsync(int modelId)
        {
            await unitOfWorkMSSQL.LotRepository.DeleteByIdAsync(modelId);
        }

        /// <summary>
        /// Deletes the object asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task DeleteObjectAsync(LotModel model)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// The get all async.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public async Task<IEnumerable<LotModel>> GetAllAsync()
        {
            return (await unitOfWorkMSSQL.LotRepository.GetAllAsync()).Select((_mapper.Map<Lot, LotModel>));
        }

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public async Task<LotModel> GetByIdAsync(int id)
        {
            return _mapper.Map<Lot, LotModel>(await unitOfWorkMSSQL.LotRepository.GetByIdWithIncludeAsync(id));
        }

        /// <summary>
        /// The update async.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="InternetAuction.BLL.Contract.Validation.InternetException">Some problem, please check your info!</exception>
        public async Task UpdateAsync(LotModel model)
        {
            var product = _mapper.Map<LotModel, Lot>(model);
            if (!ModelValidation.LotCheck(product))
            {
                throw new InternetException("Some problem, please check your info!");
            }
            unitOfWorkMSSQL.LotRepository.Update(product);
            await unitOfWorkMSSQL.SaveAsync();
        }
    }
}