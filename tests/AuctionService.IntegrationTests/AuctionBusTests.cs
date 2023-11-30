using System.Net;
using System.Net.Http.Json;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using AutoFixture;
using FluentAssertions;
using MassTransit.Testing;
using MessageContracts.Auction;
using Microsoft.Extensions.DependencyInjection;
using RandomTestValues;

namespace AuctionService.IntegrationTests;

[Collection("Shared collection")]
public class AuctionBusTests :
    IAsyncLifetime
{
    private readonly CustomWebAppFactory webAppFactory;
    private readonly Fixture fixture = new Fixture();
    private readonly ITestHarness testHarness;
    private readonly HttpClient httpClient;
    private const string apiRoute = "api/auctions";

    public AuctionBusTests(CustomWebAppFactory webAppFactory)
    {
        this.webAppFactory = webAppFactory;
        this.testHarness = webAppFactory.Services.GetTestHarness(); 
        this.httpClient = webAppFactory.CreateClient();
    }

    [Fact]
    public async Task CreateAuction_WithValidObject_ShouldPublishAuctionCreated()
    {
        // arrange:
        var sellerName = RandomValue.String(12);
        var auction = GetAuctionForCreate();
        this.httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser(sellerName));

        // act:
        var response = await this.httpClient.PostAsJsonAsync(apiRoute, auction);

        // assert:
        response.EnsureSuccessStatusCode();
        (await this.testHarness.Published.Any<AuctionCreated>()).Should().BeTrue();
    }

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
