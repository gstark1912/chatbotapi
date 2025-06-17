using ChatbotAPI.Models;
using ChatbotAPI.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChatbotAPI.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly IMongoCollection<Offer> _offers;

        public OfferRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _offers = database.GetCollection<Offer>(settings.Value.OffersCollectionName);
        }

        public async Task<List<Offer>> GetAllAsync()
        {
            return await _offers.Find(_ => true).ToListAsync();
        }

        public async Task<List<Offer>> GetActiveAsync()
        {
            var now = DateTime.UtcNow;
            return await _offers.Find(o =>
                o.Active &&
                o.StartDate <= now &&
                o.EndDate >= now
            ).ToListAsync();
        }

        public async Task<Offer?> GetByIdAsync(string offerId)
        {
            return await _offers.Find(o => o.Id == offerId).FirstOrDefaultAsync();
        }
    }
}