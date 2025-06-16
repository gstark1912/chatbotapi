using ChatbotAPI.Models;

namespace ChatbotAPI.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(string id);
        Task<List<Product>> GetProductsByCategoryAsync(string categoria);
    }
}