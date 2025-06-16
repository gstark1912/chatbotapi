using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatbotAPI.Models
{
    public class Offer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = null!;

        [BsonElement("description")]
        public string Description { get; set; } = null!;

        [BsonElement("originalPrice")]
        public decimal OriginalPrice { get; set; }

        [BsonElement("offerPrice")]
        public decimal OfferPrice { get; set; }

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }

        [BsonElement("active")]
        public bool Active { get; set; } = true;

        [BsonElement("productIds")]
        public List<string> ProductIds { get; set; } = new();
    }
}