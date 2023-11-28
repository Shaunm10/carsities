using System.Net.Http.Json;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests;

public class AuctionControllerTests(CustomWebAppFactory webAppFactory) :
    IClassFixture<CustomWebAppFactory>, IAsyncLifetime
{
    private readonly HttpClient httpClient = webAppFactory.CreateClient();
    private const string FordGTAuctionId = "afbee524-5972-4075-8800-7d1f9d7b0a0c";

    [Fact]
    public async Task GetTaskAsync_ShouldReturnThreeAuctions()
    {
        // arrange:

        // act:
        var response = await this.httpClient.GetFromJsonAsync<List<AuctionDto>>("api/auctions");


        // assert:
        response.Count.Should().Be(3);
    }

    [Fact]
    public async Task GetAuctionById_WithValidId_ShouldReturnAuction()
    {
        // arrange:
        var auctionId = FordGTAuctionId;

        // act:
        var response = await this.httpClient.GetFromJsonAsync<AuctionDto>($"api/auctions/{auctionId}");


        // assert:
        response.Model.Should().Be("GT");
    }

    [Fact]
    public async Task GetAuctionById_WithInValidId_ShouldReturn404()
    {
        // arrange:
        var auctionId = Guid.NewGuid().ToString();

        // act:
        var response = await this.httpClient.GetAsync($"api/auctions/{auctionId}");


        // assert:
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAuctionById_WithNotAGuid_ShouldReturn400()
    {
        // arrange:
        var auctionId = "notAGuid";

        // act:
        var response = await this.httpClient.GetAsync($"api/auctions/{auctionId}");

        // assert:
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task CreateAuction_WithNoAuth_ShouldReturn401()
    {
        // arrange:
        var auction = new CreateAuctionDto
        {
            Make = "test"
        };
        // act:
        var response = await this.httpClient.PostAsJsonAsync($"api/auctions", auction);

        // assert:
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }



    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        using var scope = webAppFactory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
        DbHelper.ReInitializeDbForTests(db);
        return Task.CompletedTask;
    }

}
