using ChatbotAPI.Models;

namespace ChatbotAPI.Services
{
    public interface ICartService
    {
        Task<Cart> GetOrCreateCartAsync(string phoneNumber);
        Task<Cart> AddProductToCartAsync(string phoneNumber, string productId, int quantity);
        Task<Cart> AddOfferToCartAsync(string phoneNumber, string offerId, int quantity);
        Task<Cart> RemoveItemFromCartAsync(string phoneNumber, string itemId, CartItemType itemType);
        Task<Cart> UpdateItemQuantityAsync(string phoneNumber, string itemId, CartItemType itemType, int quantity);
        Task<bool> ClearCartAsync(string phoneNumber);
        Task<Cart?> GetCartByPhoneNumberAsync(string phoneNumber);
    }
}