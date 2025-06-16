namespace ChatbotAPI.Configuration
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ProductsCollectionName { get; set; } = null!;
        public string OffersCollectionName { get; set; } = null!;
    }
}