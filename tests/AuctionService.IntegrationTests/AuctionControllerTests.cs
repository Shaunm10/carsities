using System.Net.Http.Json;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests;

public class AuctionControllerTests(CustomWebAppFactory webAppFactory) :
    IClassFixture<CustomWebAppFactory>, IAsyncLifetime
{
    private readonly HttpClient httpClient = webAppFactory.CreateClient();

    [Fact]
    public async Task GetTaskAsync_ShouldReturnThreeAutions()
    {
        // arrange:

        // act:
        var response = await this.httpClient.GetFromJsonAsync<List<AuctionDto>>("api/auctions");


        // assert:
        Assert.Equal(3, response.Count);
    }


    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        using var scope = webAppFactory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        DbHelper.ReinitDbForTests(db);
        return Task.CompletedTask;
    }

}
