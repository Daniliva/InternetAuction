using System.Collections.Generic;
using System.Threading.Tasks;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace InternetAuction.DAL.MSSQL.Repositories.Identity
{
	public class RoleRepository : IRepositoryMsSql<Role, string>
	{
		private readonly MsSqlContext _context;

		public RoleRepository(MsSqlContext context)
		{
			_context = context;
		}

		public async Task AddAsync(Role entity)
		{
			await _context.Roles.AddAsync(entity);
		}

		public void Delete(Role entity)
		{
			_context.Entry(entity).State = EntityState.Deleted;
		}

		public async Task DeleteByIdAsync(string id)
		{
			Delete(await GetByIdAsync(id));
		}

		public async Task<IEnumerable<Role>> GetAllAsync()
		{
			return await _context.Roles.ToListAsync();
		}

		public async Task<Role> GetByFiltererAsync(IsEqual func)
		{
			return await _context.Roles.FirstOrDefaultAsync(x => func(x));
		}

		public async Task<Role> GetByIdAsync(string id)
		{
			return await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Role> GetByIdWithIncludeAsync(string id)
		{
			return await _context.Roles.Include(x => x.RoleUsers).FirstOrDefaultAsync(x => x.Id == id);
			;
		}

		public void Update(Role entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}
	}
}