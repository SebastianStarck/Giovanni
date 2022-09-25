using System.Collections.Generic;
using System.Threading.Tasks;
using Giovanni.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Giovanni.Services
{
    public class UsersService
    {
        private static string Table = "Users";
        private readonly IMongoCollection<User> _usersCollection;

        public UsersService()
        {
            var mongoClient = new MongoClient(MongoDBSettings.Port);

            var mongoDatabase = mongoClient.GetDatabase(MongoDBSettings.DB);

            _usersCollection = mongoDatabase.GetCollection<User>(Table);
        }

        public async Task<List<User>> GetAsync() =>
            await _usersCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string id) =>
            await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newBook) =>
            await _usersCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, User updatedBook) =>
            await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);
    }
}