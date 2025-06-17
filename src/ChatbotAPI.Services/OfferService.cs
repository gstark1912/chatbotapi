using ChatbotAPI.Models;
using ChatbotAPI.Repositories;

namespace ChatbotAPI.Services
{
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;

        public OfferService(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<List<Offer>> GetAllOffersAsync()
        {
            return await _offerRepository.GetAllAsync();
        }

        public async Task<List<Offer>> GetActiveAsync()
        {
            return await _offerRepository.GetActiveAsync();
        }

        public Task<Offer?> GetOfferByIdAsync(string offerId)
        {
            return _offerRepository.GetByIdAsync(offerId);
        }
    }
}