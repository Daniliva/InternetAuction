using System.Collections.Generic;
using System.Threading.Tasks;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using Microsoft.EntityFrameworkCore;

namespace InternetAuction.DAL.MSSQL.Repositories.Data
{
    public class ImageIdRepository : IRepositoryMsSql<ImageId, int>
    {
        private readonly MsSqlContext _context;

        public ImageIdRepository(MsSqlContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ImageId entity)
        {
            await _context.ImageIds.AddAsync(entity);
        }

        public void Delete(ImageId entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(int id)
        {
            Delete(await GetByIdAsync(id));
        }

        public async Task<IEnumerable<ImageId>> GetAllAsync()
        {
            return await _context.ImageIds.ToListAsync();
        }

        public async Task<ImageId> GetByFiltererAsync(IsEqual func)
        {
            return await _context.ImageIds.FirstOrDefaultAsync(x => func(x));
        }

        public async Task<ImageId> GetByIdAsync(int id)
        {
            return await _context.ImageIds.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ImageId> GetByIdWithIncludeAsync(int id)
        {
            return await GetByIdAsync(id);
        }

        public void Update(ImageId entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}