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

        public LotCategoryService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
        {
            this.unitOfWorkMSSQL = unitOfWorkMSSQL;
            _mapper = mapper;
        }

        public async Task AddAsync(LotCategoryModel model)
        {
            var product = _mapper.Map<LotCategoryModel, LotCategory>(model);
            await unitOfWorkMSSQL.LotCategoryRepository.AddAsync(product);
        }

        public async Task DeleteAsync(int modelId)
        {
            await unitOfWorkMSSQL.LotCategoryRepository.DeleteByIdAsync(modelId);
        }

        public Task DeleteObjectAsync(LotCategoryModel model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<LotCategoryModel>> GetAllAsync()
        {
            var returnValue = await unitOfWorkMSSQL.LotCategoryRepository.GetAllAsync();
            var result = returnValue.Select((_mapper.Map<LotCategory, LotCategoryModel>)).ToList();

            return result;
        }

        public async Task<LotCategoryModel> GetByIdAsync(int id)
        {
            return _mapper.Map<LotCategory, LotCategoryModel>(await unitOfWorkMSSQL.LotCategoryRepository.GetByIdWithIncludeAsync(id));
        }

        public async Task UpdateAsync(LotCategoryModel model)
        {
            unitOfWorkMSSQL.LotCategoryRepository.Update(_mapper.Map<LotCategoryModel, LotCategory>(model));
            await unitOfWorkMSSQL.SaveAsync();
        }
    }
}