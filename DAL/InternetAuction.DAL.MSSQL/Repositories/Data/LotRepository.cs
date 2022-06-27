using System.Collections.Generic;
using System.Threading.Tasks;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace InternetAuction.DAL.MSSQL.Repositories.Data
{
    public class LotRepository : IRepositoryMsSql<Lot, int>
    {
        private readonly MsSqlContext _context;

        public LotRepository(MsSqlContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Lot entity)
        {
            await _context.Lots.AddAsync(entity);
        }

        public void Delete(Lot entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(int id)
        {
            Delete(await GetByIdAsync(id));
        }

        public async Task<IEnumerable<Lot>> GetAllAsync()
        {
            return await _context.Lots.ToListAsync();
        }

        public async Task<Lot> GetByFiltererAsync(IsEqual func)
        {
            return await _context.Lots.FirstOrDefaultAsync(x => func(x));
        }

        public async Task<Lot> GetByIdAsync(int id)
        {
            return await _context.Lots.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Lot> GetByIdWithIncludeAsync(int id)
        {
            return await _context.Lots.
                Include(x => x.Autction).
                Include(x => x.Author).
                Include(x => x.Category).
                Include(x => x.PhotoCurrent).
                Include(x => x.Photos).
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Lot entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}