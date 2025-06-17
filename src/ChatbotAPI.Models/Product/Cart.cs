using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatbotAPI.Models
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; } = null!;

        [BsonElement("items")]
        public List<CartItem> Items { get; set; } = new();

        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;
    }

    public class CartItem
    {
        [BsonElement("productId")]
        public string? ProductId { get; set; }

        [BsonElement("offerId")]
        public string? OfferId { get; set; }

        [BsonElement("quantity")]
        public decimal Quantity { get; set; }

        [BsonElement("unitPrice")]
        public decimal UnitPrice { get; set; }

        [BsonElement("totalPrice")]
        public decimal TotalPrice { get; set; }

        [BsonElement("itemType")]
        public CartItemType ItemType { get; set; }

        [BsonElement("addedAt")]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }

    public enum CartItemType
    {
        Product,
        Offer
    }
}