using ChatbotAPI.Models;

namespace ChatbotAPI.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByPhoneNumberAsync(string phoneNumber);
        Task<Cart?> GetByIdAsync(string id);
        Task<Cart> CreateAsync(Cart cart);
        Task<Cart> UpdateAsync(Cart cart);
        Task<bool> DeleteAsync(string id);
        Task<bool> ClearCartAsync(string phoneNumber);
    }
}