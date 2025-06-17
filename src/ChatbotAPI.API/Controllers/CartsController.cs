using Microsoft.AspNetCore.Mvc;
using ChatbotAPI.Services;
using ChatbotAPI.Models;

namespace ChatbotAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Gets cart by phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <returns>The cart for the specified phone number</returns>
        [HttpGet("{phoneNumber}")]
        public async Task<ActionResult<Cart>> GetCart(string phoneNumber)
        {
            try
            {
                var cart = await _cartService.GetCartByPhoneNumberAsync(phoneNumber);

                if (cart == null)
                {
                    return NotFound($"Cart not found for phone number {phoneNumber}");
                }

                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gets or creates a cart for a phone number
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <returns>The cart for the specified phone number</returns>
        [HttpPost("{phoneNumber}")]
        public async Task<ActionResult<Cart>> GetOrCreateCart(string phoneNumber)
        {
            try
            {
                var cart = await _cartService.GetOrCreateCartAsync(phoneNumber);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Adds a product to cart
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="request">Add product request</param>
        /// <returns>Updated cart</returns>
        [HttpPost("{phoneNumber}/products")]
        public async Task<ActionResult<Cart>> AddProductToCart(string phoneNumber, [FromBody] AddProductToCartRequest request)
        {
            try
            {
                var cart = await _cartService.AddProductToCartAsync(phoneNumber, request.ProductId, request.Quantity);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Adds an offer to cart
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="request">Add offer request</param>
        /// <returns>Updated cart</returns>
        [HttpPost("{phoneNumber}/offers")]
        public async Task<ActionResult<Cart>> AddOfferToCart(string phoneNumber, [FromBody] AddOfferToCartRequest request)
        {
            try
            {
                var cart = await _cartService.AddOfferToCartAsync(phoneNumber, request.OfferId, request.Quantity);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Updates item quantity in cart
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="request">Update quantity request</param>
        /// <returns>Updated cart</returns>
        [HttpPut("{phoneNumber}/items")]
        public async Task<ActionResult<Cart>> UpdateItemQuantity(string phoneNumber, [FromBody] UpdateCartItemRequest request)
        {
            try
            {
                var cart = await _cartService.UpdateItemQuantityAsync(phoneNumber, request.ItemId, request.ItemType, request.Quantity);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Removes an item from cart
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="itemId">Item ID (Product ID or Offer ID)</param>
        /// <param name="itemType">Item type (Product or Offer)</param>
        /// <returns>Updated cart</returns>
        [HttpDelete("{phoneNumber}/items/{itemId}")]
        public async Task<ActionResult<Cart>> RemoveItemFromCart(string phoneNumber, string itemId, [FromQuery] CartItemType itemType)
        {
            try
            {
                var cart = await _cartService.RemoveItemFromCartAsync(phoneNumber, itemId, itemType);
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Clears all items from cart
        /// </summary>
        /// <param name="phoneNumber">Phone number</param>
        /// <returns>Success status</returns>
        [HttpDelete("{phoneNumber}")]
        public async Task<ActionResult> ClearCart(string phoneNumber)
        {
            try
            {
                var result = await _cartService.ClearCartAsync(phoneNumber);

                if (result)
                {
                    return Ok(new { message = "Cart cleared successfully" });
                }

                return NotFound($"Cart not found for phone number {phoneNumber}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

    // DTOs for requests
    public class AddProductToCartRequest
    {
        public string ProductId { get; set; } = null!;
        public int Quantity { get; set; } = 1;
    }

    public class AddOfferToCartRequest
    {
        public string OfferId { get; set; } = null!;
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemRequest
    {
        public string ItemId { get; set; } = null!;
        public CartItemType ItemType { get; set; }
        public int Quantity { get; set; }
    }
}