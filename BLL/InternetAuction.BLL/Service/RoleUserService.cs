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
            await unitOfWorkMSSQL.RoleUserRepository.AddAsync(product);
        }

        public async Task DeleteAsync(string modelId)
        {
            await unitOfWorkMSSQL.RoleUserRepository.DeleteByIdAsync(modelId);
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
    }
}