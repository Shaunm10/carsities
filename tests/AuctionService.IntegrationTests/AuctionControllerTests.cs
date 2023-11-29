using System.Net;
using System.Net.Http.Json;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using AutoFixture;
using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;
using RandomTestValues;

namespace AuctionService.IntegrationTests;

public class AuctionControllerTests(CustomWebAppFactory webAppFactory) :
    IClassFixture<CustomWebAppFactory>, IAsyncLifetime
{
    private readonly CustomWebAppFactory webAppFactory = webAppFactory;
    private readonly Fixture fixture = new Fixture();
    private const string apiRoute = "api/auctions";
    private const string FordGTAuctionId = "afbee524-5972-4075-8800-7d1f9d7b0a0c";
    private readonly HttpClient httpClient = webAppFactory.CreateClient();

    #region [ Get ]
    [Fact]
    public async Task GetTaskAsync_ShouldReturnThreeAuctions()
    {
        // arrange:

        // act:
        var response = await this.httpClient.GetFromJsonAsync<List<AuctionDto>>(apiRoute);


        // assert:
        response.Count.Should().Be(3);
    }
    #endregion

    #region [ GetAuctionById ]

    [Fact]
    public async Task GetAuctionById_WithValidId_ShouldReturnAuction()
    {
        // arrange:
        var auctionId = FordGTAuctionId;

        // act:
        var response = await this.httpClient.GetFromJsonAsync<AuctionDto>($"{apiRoute}/{auctionId}");


        // assert:
        response.Model.Should().Be("GT");
    }

    [Fact]
    public async Task GetAuctionById_WithInValidId_ShouldReturn404()
    {
        // arrange:
        var auctionId = Guid.NewGuid().ToString();

        // act:
        var response = await this.httpClient.GetAsync($"{apiRoute}/{auctionId}");


        // assert:
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAuctionById_WithNotAGuid_ShouldReturn400()
    {
        // arrange:
        var auctionId = "notAGuid";

        // act:
        var response = await this.httpClient.GetAsync($"{apiRoute}/{auctionId}");

        // assert:
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    #endregion

    #region [ CreateAuction]

    [Fact]
    public async Task CreateAuction_WithNoAuth_ShouldReturn401()
    {
        // arrange:
        var auction = new CreateAuctionDto
        {
            Make = "test"
        };
        // act:
        var response = await this.httpClient.PostAsJsonAsync(apiRoute, auction);

        // assert:
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateAuction_WithAuth_ShouldReturn201()
    {
        // arrange:
        var sellerName = RandomValue.String(12);
        var auction = GetAuctionForCreate();
        this.httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser(sellerName));

        // act:
        var response = await this.httpClient.PostAsJsonAsync(apiRoute, auction);

        // assert:
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdAuction = await response.Content.ReadFromJsonAsync<AuctionDto>();
        createdAuction.Seller.Should().Be(sellerName);
    }

    [Fact]
    public async Task CreateAuction_WithInvalidCreateAuctionDto_ShouldReturn400()
    {
        // arrange:
        var sellerName = RandomValue.String(12);
        var auction = GetAuctionForCreate();
        auction.Make = null;
        this.httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser(sellerName));

        // act:
        var response = await this.httpClient.PostAsJsonAsync(apiRoute, auction);

        // assert:
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region [ UpdateAuction ]

    [Fact]
    public async Task UpdateAuction_WithIValidAuctinId_ShouldReturn404()
    {
        // arrange:
        var sellerName = "bob";
        var id = Guid.NewGuid();
        var auction = this.fixture.Create<UpdateAuctionDto>();
        this.httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser(sellerName));

        // act:
        var response = await this.httpClient.PutAsJsonAsync($"{apiRoute}/{id}", auction);

        // assert:
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateAuction_WithValidUpdateDtoAndUser_ShouldReturn200()
    {
        // arrange:
        var sellerName = "bob";
        var id = FordGTAuctionId;
        var auction = this.fixture.Create<UpdateAuctionDto>();
        this.httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser(sellerName));

        // act:
        var response = await this.httpClient.PutAsJsonAsync($"{apiRoute}/{id}", auction);

        // assert:
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateAuction_WithValidUpdateDtoAndInvalidUser_ShouldReturn403()
    {
        // arrange:
        var sellerName = RandomValue.String(12);
        var id = FordGTAuctionId;
        var auction = this.fixture.Create<UpdateAuctionDto>();
        this.httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser(sellerName));

        // act:
        var response = await this.httpClient.PutAsJsonAsync($"{apiRoute}/{id}", auction);

        // assert:
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

    }

    #endregion


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
