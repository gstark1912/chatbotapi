using ChatbotAPI.Models;
using ChatbotAPI.Repositories;

namespace ChatbotAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product?> GetProductByIdAsync(string id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _productRepository.GetByCategoryAsync(category);
        }
    }
}