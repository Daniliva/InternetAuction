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
    public class RoleService : ICrud<RoleModel, string>
    {
        private readonly IUnitOfWorkMSSQL unitOfWorkMSSQL;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
        {
            this.unitOfWorkMSSQL = unitOfWorkMSSQL;
            _mapper = mapper;
        }

        public async Task AddAsync(RoleModel model)
        {
            var product = _mapper.Map<RoleModel, Role>(model);
            await unitOfWorkMSSQL.RoleRepository.AddAsync(product);
        }

        public async Task DeleteAsync(string modelId)
        {
            await unitOfWorkMSSQL.RoleRepository.DeleteByIdAsync(modelId);
        }

        public async Task<IEnumerable<RoleModel>> GetAllAsync()
        {
            return (await unitOfWorkMSSQL.RoleRepository.GetAllAsync()).Select((_mapper.Map<Role, RoleModel>));
        }

        public async Task<RoleModel> GetByIdAsync(string id)
        {
            return _mapper.Map<Role, RoleModel>(await unitOfWorkMSSQL.RoleRepository.GetByIdWithIncludeAsync(id));
        }

        public async Task UpdateAsync(RoleModel model)
        {
            unitOfWorkMSSQL.RoleRepository.Update(_mapper.Map<RoleModel, Role>(model));
            await unitOfWorkMSSQL.SaveAsync();
        }
    }
}