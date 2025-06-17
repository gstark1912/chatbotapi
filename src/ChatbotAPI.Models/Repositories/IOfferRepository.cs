using ChatbotAPI.Models;

namespace ChatbotAPI.Repositories
{
    public interface IOfferRepository
    {
        Task<List<Offer>> GetAllAsync();
        Task<List<Offer>> GetActiveAsync();
        Task<Offer?> GetByIdAsync(string offerId);
    }
}