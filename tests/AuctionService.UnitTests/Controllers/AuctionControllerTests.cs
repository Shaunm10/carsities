using AuctionService.Controllers;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.RequestHelpers;
using AutoFixture;
using AutoMapper;
using FluentAssert;
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
        response.ShouldBeEqualTo(auctions);
    }
}
