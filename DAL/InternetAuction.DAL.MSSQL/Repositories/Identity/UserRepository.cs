using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InternetAuction.DAL.Contract;
using InternetAuction.DAL.Entities.MongoDB;
using InternetAuction.DAL.Entities.MSSQL;
using InternetAuction.DAL.MongoDB;
using Microsoft.EntityFrameworkCore;

namespace InternetAuction.DAL.MSSQL.Repositories.Identity
{
    /// <summary>
    /// The user repository.
    /// </summary>
    public class UserRepository : IRepositoryMsSqlWithImage<User, string>
    {
        private readonly MsSqlContext _context;
        private readonly ImageContext imageContext;

        public UserRepository(MsSqlContext context, ImageContext imageContext)
        {
            _context = context;
            this.imageContext = imageContext;
        }

        public async Task AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);

            var user = await _context.Users.FirstAsync(x => x.UserName == entity.UserName);
            Image image = new Image();
            image.ImageId = user.Id;
            await imageContext.Create(image);
            var images = await imageContext.GetImages(user.Id);
            ImageId imageId = new ImageId();
            imageId.ImageeId = images.First().Id;
            await _context.ImageIds.AddAsync(imageId);
            await _context.SaveChangesAsync();
            user.AvatarCurrent = _context.ImageIds.First(x => x.ImageeId == imageId.ImageeId);
            Update(user);
        }

        public void Delete(User entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(string id)
        {
            Delete(await GetByIdAsync(id));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByFiltererAsync(IsEqual func)
        {
            return await _context.Users.FirstOrDefaultAsync(x => func(x));
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetByIdWithIncludeAsync(string id)
        {
            return await _context.Users.
                Include(x => x.AvatarCurrent).
                Include(x => x.Avatars).
                Include(X => X.UserRoles).ThenInclude(x => x.Role).
                Include(x => x.Biddings).ThenInclude(x => x.Autction).ThenInclude(x => x.Lot).
                Include(x => x.Lots).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(User, byte[])> GetByIdWithImageAsync(string id)
        {
            var result = await _context.Users.Include(x => x.AvatarCurrent).FirstAsync(x => x.Id == id);
            if (result != null)
            {
                var image = await imageContext.GetImageById(result.AvatarCurrent.ImageeId);
                return (result, image);
            }
            else
                return (result, new byte[0]);
        }

        public async Task<(User, byte[])> GetByIdWithIncludeAndImageAsync(string id)
        {
            var result = await _context.Users.
                Include(x => x.AvatarCurrent).
                Include(x => x.Avatars).
                Include(X => X.UserRoles).ThenInclude(x => x.Role).
                Include(x => x.Biddings).ThenInclude(x => x.Autction).ThenInclude(x => x.Lot).
                Include(x => x.Lots).FirstOrDefaultAsync(x => x.Id == id);
            if (result != null)
            {
                var image = await imageContext.GetImageById(result.AvatarCurrent.ImageeId);
                return (result, image);
            }
            else
                return (result, new byte[0]);
        }

        public void Update(User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Update(User user, Stream imageStream)
        {
            var result = await GetByIdWithIncludeAsync(user.Id);
            await imageContext.StoreImage(result.AvatarCurrent.ImageeId, imageStream, user.Id); ;
        }
    }

    // public class ApplicationRoleUserManager:RoleUser
}