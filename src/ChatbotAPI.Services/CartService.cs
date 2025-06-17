using ChatbotAPI.Models;
using ChatbotAPI.Repositories;

namespace ChatbotAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductService _productService;
        private readonly IOfferService _offerService;

        public CartService(ICartRepository cartRepository, IProductService productService, IOfferService offerService)
        {
            _cartRepository = cartRepository;
            _productService = productService;
            _offerService = offerService;
        }

        public async Task<Cart> GetOrCreateCartAsync(string phoneNumber)
        {
            var existingCart = await _cartRepository.GetByPhoneNumberAsync(phoneNumber);

            if (existingCart != null)
            {
                return existingCart;
            }

            var newCart = new Cart
            {
                PhoneNumber = phoneNumber,
                Items = new List<CartItem>(),
                TotalAmount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            return await _cartRepository.CreateAsync(newCart);
        }

        public async Task<Cart> AddProductToCartAsync(string phoneNumber, string productId, int quantity)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {productId} not found");
            }

            var cart = await GetOrCreateCartAsync(phoneNumber);

            // Check if product already exists in cart
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId && i.ItemType == CartItemType.Product);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * quantity,
                    ItemType = CartItemType.Product,
                    AddedAt = DateTime.UtcNow
                };

                cart.Items.Add(cartItem);
            }

            cart.TotalAmount = cart.Items.Sum(i => i.TotalPrice);
            cart.UpdatedAt = DateTime.UtcNow;

            return await _cartRepository.UpdateAsync(cart);
        }

        public async Task<Cart> AddOfferToCartAsync(string phoneNumber, string offerId, int quantity)
        {
            var offer = await _offerService.GetOfferByIdAsync(offerId);
            if (offer == null)
            {
                throw new ArgumentException($"Offer with ID {offerId} not found");
            }

            var cart = await GetOrCreateCartAsync(phoneNumber);

            // Check if offer already exists in cart
            var existingItem = cart.Items.FirstOrDefault(i => i.OfferId == offerId && i.ItemType == CartItemType.Offer);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalPrice = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                var cartItem = new CartItem
                {
                    OfferId = offerId,
                    Quantity = quantity,
                    UnitPrice = offer.OfferPrice,
                    TotalPrice = offer.OfferPrice * quantity,
                    ItemType = CartItemType.Offer,
                    AddedAt = DateTime.UtcNow
                };

                cart.Items.Add(cartItem);
            }

            cart.TotalAmount = cart.Items.Sum(i => i.TotalPrice);
            cart.UpdatedAt = DateTime.UtcNow;

            return await _cartRepository.UpdateAsync(cart);
        }

        public async Task<Cart> RemoveItemFromCartAsync(string phoneNumber, string itemId, CartItemType itemType)
        {
            var cart = await _cartRepository.GetByPhoneNumberAsync(phoneNumber);
            if (cart == null)
            {
                throw new ArgumentException($"Cart not found for phone number {phoneNumber}");
            }

            CartItem? itemToRemove = null;

            if (itemType == CartItemType.Product)
            {
                itemToRemove = cart.Items.FirstOrDefault(i => i.ProductId == itemId && i.ItemType == CartItemType.Product);
            }
            else if (itemType == CartItemType.Offer)
            {
                itemToRemove = cart.Items.FirstOrDefault(i => i.OfferId == itemId && i.ItemType == CartItemType.Offer);
            }

            if (itemToRemove != null)
            {
                cart.Items.Remove(itemToRemove);
                cart.TotalAmount = cart.Items.Sum(i => i.TotalPrice);
                cart.UpdatedAt = DateTime.UtcNow;

                return await _cartRepository.UpdateAsync(cart);
            }

            throw new ArgumentException($"Item with ID {itemId} not found in cart");
        }

        public async Task<Cart> UpdateItemQuantityAsync(string phoneNumber, string itemId, CartItemType itemType, int quantity)
        {
            if (quantity <= 0)
            {
                return await RemoveItemFromCartAsync(phoneNumber, itemId, itemType);
            }

            var cart = await _cartRepository.GetByPhoneNumberAsync(phoneNumber);
            if (cart == null)
            {
                throw new ArgumentException($"Cart not found for phone number {phoneNumber}");
            }

            CartItem? itemToUpdate = null;

            if (itemType == CartItemType.Product)
            {
                itemToUpdate = cart.Items.FirstOrDefault(i => i.ProductId == itemId && i.ItemType == CartItemType.Product);
            }
            else if (itemType == CartItemType.Offer)
            {
                itemToUpdate = cart.Items.FirstOrDefault(i => i.OfferId == itemId && i.ItemType == CartItemType.Offer);
            }

            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = quantity;
                itemToUpdate.TotalPrice = itemToUpdate.UnitPrice * quantity;

                cart.TotalAmount = cart.Items.Sum(i => i.TotalPrice);
                cart.UpdatedAt = DateTime.UtcNow;

                return await _cartRepository.UpdateAsync(cart);
            }

            throw new ArgumentException($"Item with ID {itemId} not found in cart");
        }

        public async Task<bool> ClearCartAsync(string phoneNumber)
        {
            return await _cartRepository.ClearCartAsync(phoneNumber);
        }

        public async Task<Cart?> GetCartByPhoneNumberAsync(string phoneNumber)
        {
            return await _cartRepository.GetByPhoneNumberAsync(phoneNumber);
        }
    }
}