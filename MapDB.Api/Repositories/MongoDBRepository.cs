using MapDB.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MapDB.Api.Repositories{
    public class MongoDBRepository : IPinsRepository
    {

        private const string dbName = "MapDB";
        private const string collectionName = "Pins";
        private readonly IMongoCollection<Pin> PinsCollection;
        private readonly FilterDefinitionBuilder<Pin> filterBuilder = Builders<Pin>.Filter;

        public MongoDBRepository(IMongoClient mongoClient){
            IMongoDatabase db = mongoClient.GetDatabase(dbName);
            PinsCollection = db.GetCollection<Pin>(collectionName);
        }

        public async Task CreatePinAsync(Pin Pin) // async
        {
           await PinsCollection.InsertOneAsync(Pin); // await
        }

        public async Task<Pin> GetPinAsync(Guid ID)
        {
            var filter = filterBuilder.Eq(Pin => Pin.ID, ID);
            return await PinsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Pin>> GetPinsAsync()
        {            
            return await PinsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdatePinAsync(Pin Pin)
        {
             var filter = filterBuilder.Eq(existingPin => existingPin.ID, Pin.ID);
             await PinsCollection.ReplaceOneAsync(filter, Pin);
        }

    public async Task DeletePinAsync(Guid ID)
        {
            var filter = filterBuilder.Eq(existingPin => existingPin.ID, ID);
            await PinsCollection.DeleteOneAsync(filter);
        }
    }
}