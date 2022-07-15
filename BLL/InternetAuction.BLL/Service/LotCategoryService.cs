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
    /// The lot category service.
    /// </summary>
    public class LotCategoryService : ICrud<LotCategoryModel, int>
    {
        private readonly IUnitOfWorkMSSQL unitOfWorkMSSQL;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LotCategoryService"/> class.
        /// </summary>
        /// <param name="unitOfWorkMSSQL">The unit of work MSSQL.</param>
        /// <param name="mapper">The mapper.</param>
        public LotCategoryService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
        {
            this.unitOfWorkMSSQL = unitOfWorkMSSQL;
            _mapper = mapper;
        }

        /// <summary>
        /// The add async.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task AddAsync(LotCategoryModel model)
        {
            var product = _mapper.Map<LotCategoryModel, LotCategory>(model);
            await unitOfWorkMSSQL.LotCategoryRepository.AddAsync(product);
        }

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="modelId">The model id.</param>
        public async Task DeleteAsync(int modelId)
        {
            await unitOfWorkMSSQL.LotCategoryRepository.DeleteByIdAsync(modelId);
        }

        /// <summary>
        /// Deletes the object asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task DeleteObjectAsync(LotCategoryModel model)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// The get all async.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public async Task<IEnumerable<LotCategoryModel>> GetAllAsync()
        {
            var returnValue = await unitOfWorkMSSQL.LotCategoryRepository.GetAllAsync();
            var result = returnValue.Select((_mapper.Map<LotCategory, LotCategoryModel>)).ToList();

            return result;
        }

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public async Task<LotCategoryModel> GetByIdAsync(int id)
        {
            return _mapper.Map<LotCategory, LotCategoryModel>(await unitOfWorkMSSQL.LotCategoryRepository.GetByIdWithIncludeAsync(id));
        }

        /// <summary>
        /// The update async.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task UpdateAsync(LotCategoryModel model)
        {
            unitOfWorkMSSQL.LotCategoryRepository.Update(_mapper.Map<LotCategoryModel, LotCategory>(model));
            await unitOfWorkMSSQL.SaveAsync();
        }
    }
}