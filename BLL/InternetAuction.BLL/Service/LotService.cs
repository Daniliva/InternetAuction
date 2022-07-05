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
    public class LotService : ICrud<UserModel, string>
    {
        private readonly IUnitOfWorkMSSQL unitOfWorkMSSQL;
        private readonly IMapper _mapper;

        public LotService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
        {
            this.unitOfWorkMSSQL = unitOfWorkMSSQL;
            _mapper = mapper;
        }

        public async Task AddAsync(UserModel model)
        {
            var product = _mapper.Map<UserModel, User>(model);
            await unitOfWorkMSSQL.UserRepository.AddAsync(product);
        }

        public async Task DeleteAsync(string modelId)
        {
            await unitOfWorkMSSQL.UserRepository.DeleteByIdAsync(modelId);
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            return (await unitOfWorkMSSQL.UserRepository.GetAllAsync()).Select((_mapper.Map<User, UserModel>));
        }

        public async Task<UserModel> GetByIdAsync(string id)
        {
            return _mapper.Map<User, UserModel>(await unitOfWorkMSSQL.UserRepository.GetByIdWithIncludeAsync(id));
        }

        public async Task UpdateAsync(UserModel model)
        {
            unitOfWorkMSSQL.UserRepository.Update(_mapper.Map<UserModel, User>(model));
            await unitOfWorkMSSQL.SaveAsync();
        }
    }
}