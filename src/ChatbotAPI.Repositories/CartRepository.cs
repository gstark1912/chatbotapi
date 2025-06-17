using ChatbotAPI.Models;
using ChatbotAPI.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatbotAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<Cart> _carts;

        public CartRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _carts = database.GetCollection<Cart>(settings.Value.CartsCollectionName);
        }

        public async Task<Cart?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _carts.Find(c => c.PhoneNumber == phoneNumber && c.IsActive).FirstOrDefaultAsync();
        }

        public async Task<Cart?> GetByIdAsync(string id)
        {
            return await _carts.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Cart> CreateAsync(Cart cart)
        {
            await _carts.InsertOneAsync(cart);
            return cart;
        }

        public async Task<Cart> UpdateAsync(Cart cart)
        {
            cart.UpdatedAt = DateTime.UtcNow;
            await _carts.ReplaceOneAsync(c => c.Id == cart.Id, cart);
            return cart;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _carts.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ClearCartAsync(string phoneNumber)
        {
            var filter = Builders<Cart>.Filter.Eq(c => c.PhoneNumber, phoneNumber);
            var update = Builders<Cart>.Update
                .Set(c => c.Items, new List<CartItem>())
                .Set(c => c.TotalAmount, 0)
                .Set(c => c.UpdatedAt, DateTime.UtcNow);

            var result = await _carts.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}