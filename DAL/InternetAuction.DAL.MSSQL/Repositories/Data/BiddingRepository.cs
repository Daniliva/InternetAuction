using System.Collections.Generic;
using System.Threading.Tasks;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace InternetAuction.DAL.MSSQL.Repositories.Data
{
    public class BiddingRepository : IRepositoryMsSql<Bidding, int>
    {
        private readonly MsSqlContext _context;

        public BiddingRepository(MsSqlContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Bidding entity)
        {
            await _context.Biddings.AddAsync(entity);
        }

        public void Delete(Bidding entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(int id)
        {
            Delete(await GetByIdAsync(id));
        }

        public async Task<IEnumerable<Bidding>> GetAllAsync()
        {
            return await _context.Biddings.ToListAsync();
        }

        public async Task<Bidding> GetByFiltererAsync(IsEqual func)
        {
            return await _context.Biddings.FirstOrDefaultAsync(x => func(x));
        }

        public async Task<Bidding> GetByIdAsync(int id)
        {
            return await _context.Biddings.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Bidding> GetByIdWithIncludeAsync(int id)
        {
            return await _context.Biddings.
                Include(x => x.User).
                Include(x => x.Autction).ThenInclude(x => x.Status).
                Include(x => x.Autction).ThenInclude(x => x.Lot).
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Bidding entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}