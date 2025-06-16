using ChatbotAPI.Models;

namespace ChatbotAPI.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(string id);
        Task<List<Product>> GetByCategoryAsync(string category);
    }
}