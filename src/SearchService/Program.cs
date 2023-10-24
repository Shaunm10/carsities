using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Business;
using SearchService.Data;
using SearchService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ISearchService, SearchService.Business.SearchService>();

builder.Services.AddHttpClient<AuctionSvcHttpClient>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

try
{

    await DbInitializer.InitDb(app);
}
catch (Exception ex)
{
    Console.WriteLine($"Error configuring MongoDB: {ex}");
}

app.Run();
