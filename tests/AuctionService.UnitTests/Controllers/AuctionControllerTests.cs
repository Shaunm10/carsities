using AuctionService.Controllers;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.RequestHelpers;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RandomTestValues;

namespace AuctionService.UnitTests.Controllers;

public class AuctionControllerTests
{
    private readonly Mock<IAuctionRepository> auctionRepository;
    private readonly Mock<IPublishEndpoint> publishEndpoint;
    private readonly Fixture fixture;
    private readonly IMapper mapper;
    private readonly AuctionController controllerUnderTest;

    public AuctionControllerTests()
    {
        this.fixture = new Fixture();
        this.auctionRepository = new Mock<IAuctionRepository>();
        this.publishEndpoint = new Mock<IPublishEndpoint>();

        var mockMapper = new MapperConfiguration(mc =>
        {
            mc.AddMaps(typeof(MappingProfiles).Assembly);
        }).CreateMapper().ConfigurationProvider;

        this.mapper = new Mapper(mockMapper);
        this.controllerUnderTest = new AuctionController(this.auctionRepository.Object, this.mapper, this.publishEndpoint.Object);
    }

    [Fact]
    public async Task GetAuctions_WithNoParams_Returns10Auctions()
    {
        // Arrange:
        var auctions = this.fixture.CreateMany<AuctionDto>(10).ToList();
        var date = RandomValue.String(5);
        this.auctionRepository.Setup(x => x.GetAuctionsAsync(date)).ReturnsAsync(auctions);

        // Act:
        var response = await this.controllerUnderTest.GetAllAuctions(date);

        // Assert:
        response.Should().BeEquivalentTo(auctions);
    }

    [Fact]
    public async Task GetAuctionById_WrongId_Returns404()
    {
        // Arrange:
        var auctionId = RandomValue.Guid();
        this.auctionRepository.Setup(x => x.GetAuctionByIdAsync(auctionId)).ReturnsAsync(value: null);

        // Act:
        var response = await this.controllerUnderTest.GetAuctionById(auctionId);

        // Assert:
        response.Should().NotBeNull();
        response.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAuctionById_CorrectId_ReturnsAuction()
    {
        // Arrange:
        var auctionId = RandomValue.Guid();
        var auction = this.fixture.Create<AuctionDto>();
        this.auctionRepository
            .Setup(x => x.GetAuctionByIdAsync(auctionId))
            .ReturnsAsync(auction);

        // Act:
        var response = await this.controllerUnderTest.GetAuctionById(auctionId);

        // Assert:
        response.Should().NotBeNull();
        response.Value.Should().BeOfType<AuctionDto>();
        response.Value.Should().BeEquivalentTo(auction);
    }

    [Fact]
    public async Task CreateAuction_WithValidAuctionCreateDto_ReturnsCreatedAtActionResult()
    {
        // Arrange:
        var auctionToCreate = this.fixture.Create<CreateAuctionDto>();
        this.controllerUnderTest.User
        this.auctionRepository
            .Setup(x => x.GetAuctionByIdAsync(auctionId))
            .ReturnsAsync(auction);

        // Act:
        var response = await this.controllerUnderTest.CreateAuction(auctionId);

        // Assert:
        response.Should().NotBeNull();
        response.Value.Should().BeOfType<AuctionDto>();
        response.Value.Should().BeEquivalentTo(auction);
    }

    [Fact]
    public async Task CreateAuction_SavedFailed_ReturnsBadRequest()
    {
        // Arrange:
        var auctionId = RandomValue.Guid();
        var auction = this.fixture.Create<AuctionDto>();
        this.auctionRepository
            .Setup(x => x.GetAuctionByIdAsync(auctionId))
            .ReturnsAsync(auction);

        // Act:
        var response = await this.controllerUnderTest.GetAuctionById(auctionId);

        // Assert:
        response.Should().NotBeNull();
        response.Value.Should().BeOfType<AuctionDto>();
        response.Value.Should().BeEquivalentTo(auction);
    }
}
