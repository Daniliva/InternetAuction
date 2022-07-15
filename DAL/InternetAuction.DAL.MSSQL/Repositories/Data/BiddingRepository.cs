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
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == entity.User.Id);
            var autction = await _context.Autctions.FirstOrDefaultAsync(x => x.Id == entity.Autction.Id);
            entity.Autction = null;
            entity.User = null;
            await _context.Biddings.AddAsync(entity);
            _context.SaveChanges();
            if (user.Biddings == null)
                user.Biddings = new List<Bidding>();
            if (autction.Biddings == null)
                autction.Biddings = new List<Bidding>();
            user.Biddings.Add(entity);
            autction.Biddings.Add(entity);
            _context.SaveChanges();
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
            var result = await _context.Biddings.
                Include(x => x.User).
                Include(x => x.Autction).ThenInclude(x => x.Status).
                Include(x => x.Autction).ThenInclude(x => x.Lot).
                FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public void Update(Bidding entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}