using System.Collections.Generic;
using System.Threading.Tasks;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MSSQL;
using Microsoft.EntityFrameworkCore;
using InternetAuction.DAL.MongoDB;
using System.Linq;
using InternetAuction.DAL.Entities.MongoDB;
using System.IO;

namespace InternetAuction.DAL.MSSQL.Repositories.Data
{
    public class LotRepository : IRepositoryMsSqlWithImage<Lot, int>
    {
        private readonly MsSqlContext _context;
        private readonly ImageContext imageContext;

        public LotRepository(MsSqlContext context, ImageContext imageContext)

        {
            _context = context;
            this.imageContext = imageContext;
        }

        public async Task AddAsync(Lot entity)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == entity.Author.Id);
            var autctionStatuses = _context.AutctionStatuses.FirstOrDefault(x => x.Id == entity.Autction.Status.Id);
            var lotCategories = _context.LotCategories.First(x => x.Id == entity.Category.Id);
            if (lotCategories.Lots == null)
                lotCategories.Lots = new List<Lot>();
            entity.Author = null;
            entity.Autction.Status = null;
            entity.Category = null;
            autctionStatuses.Autctions = new List<Autction>();
            autctionStatuses.Autctions.Add(entity.Autction);
            lotCategories.Lots.Add(entity);
            user.Lots.Add(entity);
            _context.SaveChanges();
            //   entity.Autction.Lot.Add();
            var lot = await _context.Lots.FirstAsync(x => x.Id == entity.Id);
            Image image = new Image();
            image.ImageId = lot.Id.ToString();
            await imageContext.Create(image);
            var images = await imageContext.GetImages(lot.Id.ToString());
            ImageId imageId = new ImageId();
            imageId.ImageeId = images.First().Id;
            await _context.ImageIds.AddAsync(imageId);
            await _context.SaveChangesAsync();
            lot.PhotoCurrent = _context.ImageIds.First(x => x.ImageeId == imageId.ImageeId);
            Update(lot);
            _context.SaveChanges();
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

        public async Task<(Lot, byte[])> GetByIdWithImageAsync(int id)
        {
            var result = await _context.Lots.Include(x => x.PhotoCurrent).FirstOrDefaultAsync(x => x.Id == id);
            if (result != null)
            {
                var image = await imageContext.GetImageById(result.PhotoCurrent.ImageeId);
                return (result, image);
            }
            else
                return (result, new byte[0]);
        }

        public Task<(Lot, byte[])> GetByIdWithIncludeAndImageAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Lot> GetByIdWithIncludeAsync(int id)
        {
            return await _context.Lots.
                Include(x => x.Autction).ThenInclude(x => x.Status).
                Include(x => x.Author).
                Include(x => x.Category).
                Include(x => x.PhotoCurrent).
                Include(x => x.Photos).
                FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Update(Lot entity)
        {
            var d = GetByIdWithIncludeAsync(entity.Id).Result;

            if (d != null)
            {
                d.CostMin = entity.CostMin;
                //d.Autction = entity.Autction;
                d.Name = entity.Name;
                d.Description = entity.Description;
                //   d.Category=entity.Category;
                _context.Lots.Update(d);
                _context.SaveChanges();
                var category = _context.LotCategories.First(x => x.Id == entity.Category.Id);
                if (category.Lots == null)
                    category.Lots = new List<Lot>();
                category.Lots.Add(d);
                _context.LotCategories.Update(category);
                _context.SaveChanges();
                //   d.Category.Lots.Remov
            }
            /*   var user = _context.Users.FirstOrDefault(x => x.Id == entity.Author.Id);
               var autctionStatuses = _context.AutctionStatuses.FirstOrDefault(x => x.Id == entity.Autction.Status.Id);
               var lotCategories = _context.LotCategories.First(x => x.Id == entity.Category.Id);
               if (lotCategories.Lots == null)
                   lotCategories.Lots = new List<Lot>();*/

            //     _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Update(Lot entity, Stream imageStream)
        {
            var result = await GetByIdWithIncludeAsync(entity.Id);
            await imageContext.StoreImage(result.PhotoCurrent.ImageeId, imageStream, entity.Id.ToString());
        }
    }
}