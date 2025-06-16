using Microsoft.OpenApi.Models;
using ChatbotAPI.Services;
using ChatbotAPI.Repositories;
using ChatbotAPI.Configuration;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Chatbot API", 
        Version = "v1",
        Description = "API para gestión de productos y ofertas del chatbot"
    });
});

// MongoDB Configuration
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
    {
        throw new InvalidOperationException("MongoDbSettings or ConnectionString is not configured properly.");
    }
    return new MongoClient(settings.ConnectionString);
});

// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOfferRepository, OfferRepository>();

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOfferService, OfferService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chatbot API V1");
        c.RoutePrefix = string.Empty; // Para que Swagger sea la página principal
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();