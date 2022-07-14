using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace InternetAuction.DAL.MSSQL.Repositories.Identity
{
	public class RoleUserRepository : IRepositoryMsSql<RoleUser, string>
	{
		private readonly MsSqlContext _context;

		public RoleUserRepository(MsSqlContext context)
		{
			_context = context;
		}

		public async Task AddAsync(RoleUser entity)
		{
			//entity.Role = null;
			//  entity.User;
			await _context.RoleUser.AddAsync(entity);
		}

		public void Delete(RoleUser entity)
		{
			_context.RoleUser.Remove(entity);
			_context.Entry(entity).State = EntityState.Deleted;
		}

		public async Task DeleteByIdAsync(string id)
		{
			Delete(await GetByIdAsync(id));
		}

		public async Task<IEnumerable<RoleUser>> GetAllAsync()
		{
			return await _context.RoleUser.ToListAsync();
		}

		public async Task<RoleUser> GetByFiltererAsync(IsEqual func)
		{
			return await _context.RoleUser.FirstOrDefaultAsync(x => func(x));
		}

		public async Task<RoleUser> GetByIdAsync(string id)
		{
			// var result = (await GetAllAsync()).FirstOrDefault(x => x.Id == id);
			//return result;
			return await _context.RoleUser.FirstOrDefaultAsync(x => x.UsersId == id);
		}

		public async Task<RoleUser> GetByIdWithIncludeAsync(string id)
		{
			return await GetByIdAsync(id);
		}

		public void Update(RoleUser entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}