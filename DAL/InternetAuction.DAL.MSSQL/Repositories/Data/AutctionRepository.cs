using System.Collections.Generic;
using System.Threading.Tasks;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace InternetAuction.DAL.MSSQL.Repositories.Data
{
    public class AutctionRepository : IRepositoryMsSql<Autction, int>
    {
        private readonly MsSqlContext _context;

        public AutctionRepository(MsSqlContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Autction entity)
        {
            await _context.Autctions.AddAsync(entity);
        }

        public void Delete(Autction entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(int id)
        {
            Delete(await GetByIdAsync(id));
        }

        public async Task<IEnumerable<Autction>> GetAllAsync()
        {
            return await _context.Autctions.ToListAsync();
        }

        public async Task<Autction> GetByFiltererAsync(IsEqual func)
        {
            return await _context.Autctions.FirstOrDefaultAsync(x => func(x));
        }

        public async Task<Autction> GetByIdAsync(int id)
        {
            return await _context.Autctions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Autction> GetByIdWithIncludeAsync(int id)
        {
            return await _context.Autctions.Include(x => x.Biddings).Include(x => x.Lot).Include(x => x.Status).FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Autction entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}