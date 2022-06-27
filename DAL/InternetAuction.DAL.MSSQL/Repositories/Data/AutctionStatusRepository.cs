using System.Collections.Generic;
using System.Threading.Tasks;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace InternetAuction.DAL.MSSQL.Repositories.Data
{
    public class AutctionStatusRepository : IRepositoryMsSql<AutctionStatus, int>
    {
        private readonly MsSqlContext _context;

        public AutctionStatusRepository(MsSqlContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AutctionStatus entity)
        {
            await _context.AutctionStatuses.AddAsync(entity);
        }

        public void Delete(AutctionStatus entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(int id)
        {
            Delete(await GetByIdAsync(id));
        }

        public async Task<IEnumerable<AutctionStatus>> GetAllAsync()
        {
            return await _context.AutctionStatuses.ToListAsync();
        }

        public async Task<AutctionStatus> GetByFiltererAsync(IsEqual func)
        {
            return await _context.
                AutctionStatuses.
                FirstOrDefaultAsync(x => func(x));
        }

        public async Task<AutctionStatus> GetByIdAsync(int id)
        {
            return await _context.
                AutctionStatuses.
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AutctionStatus> GetByIdWithIncludeAsync(int id)
        {
            return await _context.
                AutctionStatuses.
                Include(x => x.Autctions).
                ThenInclude(x => x.Lot).
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(AutctionStatus entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}