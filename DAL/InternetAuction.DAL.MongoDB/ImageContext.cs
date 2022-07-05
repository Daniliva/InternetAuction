using InternetAuction.DAL.Entities.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InternetAuction.DAL.MongoDB
{
    /// <summary>
    /// The image context.
    /// </summary>
    public class ImageContext
    {
        private IMongoDatabase database; // база данных
        private IGridFSBucket gridFS;   // файловое хранилище

        public ImageContext(string connectionString)
        {
            // строка подключения
            //  string connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            database = client.GetDatabase(connection.DatabaseName);
            // получаем доступ к файловому хранилищу
            gridFS = new GridFSBucket(database);
            //var image = db.GetImage(1).;
        }

        // обращаемся к коллекции Phones
        public IMongoCollection<Image> Images
        {
            get { return database.GetCollection<Image>("Images"); }
        }

        // получаем один документ по id
        public async Task<Image> GetImage(string id)
        {
            return await Images.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Image>> GetImages(string ImageId)
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<Image>();
            var filter = builder.Empty; // фильтр для выборки всех документов
            // фильтр по имени
            if (!String.IsNullOrWhiteSpace(ImageId))
            {
                filter = filter & builder.Regex("ImageId", new BsonRegularExpression(ImageId));
            }

            return await Images.Find(filter).ToListAsync();
        }

        // добавление документа
        public async Task Create(Image c)
        {
            await Images.InsertOneAsync(c);
        }

        // обновление документа
        public async Task Update(Image c)
        {
            await Images.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(c.ImageId)), c);
        }

        // удаление документа
        public async Task Remove(string id)
        {
            await Images.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }

        // получение изображения
        public async Task<byte[]> GetImageById(string id)
        {
            return await gridFS.DownloadAsBytesAsync(new ObjectId(id));
        }

        // сохранение изображения
        public async Task StoreImage(string id, Stream imageStream, string imageName)
        {
            Image c = await GetImage(id);
            if (c.HasImage())
            {
                // если ранее уже была прикреплена картинка, удаляем ее
                await gridFS.DeleteAsync(new ObjectId(c.ImageId));
            }
            // сохраняем изображение
            ObjectId imageId = await gridFS.UploadFromStreamAsync(imageName, imageStream);
            // обновляем данные по документу
            c.ImageId = imageId.ToString();
            var filter = Builders<Image>.Filter.Eq("_id", new ObjectId(c.ImageId));
            var update = Builders<Image>.Update.Set("ImageId", c.ImageId);
            await Images.UpdateOneAsync(filter, update);
        }
    }
}