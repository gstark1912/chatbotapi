using ChatbotAPI.Models;

namespace ChatbotAPI.Services
{
    public interface IOfferService
    {
        Task<List<Offer>> GetAllOffersAsync();
        Task<List<Offer>> GetActiveAsync();
    }
}