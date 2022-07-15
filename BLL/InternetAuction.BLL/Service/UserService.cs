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
    /// The user service.
    /// </summary>
    public class UserService : IExpansionGetEmail<UserModel, string>
    {
        private readonly IUnitOfWorkMSSQL unitOfWorkMSSQL;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="unitOfWorkMSSQL">The unit of work MSSQL.</param>
        /// <param name="mapper">The mapper.</param>
        public UserService(IUnitOfWorkMSSQL unitOfWorkMSSQL, IMapper mapper)
        {
            this.unitOfWorkMSSQL = unitOfWorkMSSQL;
            _mapper = mapper;
        }

        /// <summary>
        /// The add async.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task AddAsync(UserModel model)
        {
            var product = _mapper.Map<UserModel, User>(model);
            var newList = new List<RoleUser>();
            foreach (var roleUser in product.RoleUsers)
            {
                newList.Add(new RoleUser() { Users = product });
            }

            product.RoleUsers = newList;
            if (!ModelValidation.UserCheck(product))
            {
                throw new InternetException("Some problem, please check your info!");
            }
            await unitOfWorkMSSQL.UserRepository.AddAsync(product);
            await unitOfWorkMSSQL.SaveAsync();
        }

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="modelId">The model id.</param>
        public async Task DeleteAsync(string modelId)
        {
            await unitOfWorkMSSQL.UserRepository.DeleteByIdAsync(modelId);
        }

        /// <summary>
        /// Deletes the object asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task DeleteObjectAsync(UserModel model)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// The get all async.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            return (await unitOfWorkMSSQL.UserRepository.GetAllAsync()).Select((_mapper.Map<User, UserModel>));
        }

        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public async Task<UserModel> GetByEmail(string email)
        {
            IsEqual isEqual = (object x) =>
            {
                if (x is User user1)
                {
                    return user1.Email == email;
                }
                else
                    return false;
            };
            var user = (await unitOfWorkMSSQL.UserRepository.GetByFiltererAsync(isEqual));
            return _mapper.Map<User, UserModel>(user);
        }

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The result.
        /// </returns>
        public async Task<UserModel> GetByIdAsync(string id)
        {
            var t = await unitOfWorkMSSQL.UserRepository.GetByIdWithIncludeAsync(id);
            return _mapper.Map<User, UserModel>(await unitOfWorkMSSQL.UserRepository.GetByIdWithIncludeAsync(id));
        }

        /// <summary>
        /// The update async.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task UpdateAsync(UserModel model)
        {
            var product = _mapper.Map<UserModel, User>(model);
            if (!ModelValidation.UserCheck(product))
            {
                throw new InternetException("Some problem, please check your info!");
            }
            unitOfWorkMSSQL.UserRepository.Update(product);
            await unitOfWorkMSSQL.SaveAsync();
        }
    }
}