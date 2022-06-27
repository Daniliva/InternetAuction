using System.Collections.Generic;
using System.Threading.Tasks;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace InternetAuction.DAL.MSSQL.Repositories.Identity
{
    /// <summary>
    /// The user repository.
    /// </summary>
    public class UserRepository : IRepositoryMsSql<User, string>
    {
        private readonly MsSqlContext _context;

        public UserRepository(MsSqlContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
        }

        public void Delete(User entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(string id)
        {
            Delete(await GetByIdAsync(id));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByFiltererAsync(IsEqual func)
        {
            return await _context.Users.FirstOrDefaultAsync(x => func(x));
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetByIdWithIncludeAsync(string id)
        {
            return await _context.Users.
                Include(x => x.AvatarCurrent).
                Include(x => x.Avatars).
                Include(X => X.UserRoles).ThenInclude(x => x.Role).
                Include(x => x.Biddings).ThenInclude(x => x.Autction).ThenInclude(x => x.Lot).
                Include(x => x.Lots).FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}