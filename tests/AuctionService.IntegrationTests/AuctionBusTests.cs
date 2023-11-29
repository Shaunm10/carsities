using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests;

public class AuctionBusTests(CustomWebAppFactory webAppFactory) :
    IClassFixture<CustomWebAppFactory>,
    IAsyncLifetime
{
    private readonly CustomWebAppFactory webAppFactory = webAppFactory;
    private readonly Fixture fixture = new Fixture();

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        using var scope = webAppFactory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        DbHelper.ReInitializeDbForTests(db);
        return Task.CompletedTask;
    }

    private CreateAuctionDto GetAuctionForCreate()
    {
        return new CreateAuctionDto
        {
            Make = "test",
            Model = "testModel",
            ImageUrl = "test",
            Color = "test",
            Mileage = 10,
            Year = 10,
            ReservePrice = 20.99m,
            AuctionEnd = DateTime.Now.AddDays(5)
        };
    }

}
