using System.Collections.Generic;
using System.Threading.Tasks;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace InternetAuction.DAL.MSSQL.Repositories.Data
{
    public class LotCategoryRepository : IRepositoryMsSql<LotCategory, int>
    {
        private readonly MsSqlContext _context;

        public LotCategoryRepository(MsSqlContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LotCategory entity)
        {
            await _context.LotCategories.AddAsync(entity);
        }

        public void Delete(LotCategory entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(int id)
        {
            Delete(await GetByIdAsync(id));
        }

        public async Task<IEnumerable<LotCategory>> GetAllAsync()
        {
            var result = await _context.LotCategories.ToListAsync();
            return result;
        }

        public async Task<LotCategory> GetByFiltererAsync(IsEqual func)
        {
            return await _context.LotCategories.FirstOrDefaultAsync(x => func(x));
        }

        public async Task<LotCategory> GetByIdAsync(int id)
        {
            return await _context.LotCategories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<LotCategory> GetByIdWithIncludeAsync(int id)
        {
            return await _context.LotCategories.
                Include(x => x.Lots).ThenInclude(x => x.Author).
                Include(x => x.Lots).ThenInclude(x => x.Autction).
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(LotCategory entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}