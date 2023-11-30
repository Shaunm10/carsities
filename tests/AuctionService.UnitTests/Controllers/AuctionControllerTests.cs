using AuctionService.Controllers;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AuctionService.RequestHelpers;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Http;
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
    private readonly string userName;

    public AuctionControllerTests()
    {

        this.fixture = new Fixture();
        this.auctionRepository = new Mock<IAuctionRepository>();
        this.publishEndpoint = new Mock<IPublishEndpoint>();
        userName = RandomValue.String(12);

        var mockMapper = new MapperConfiguration(mc =>
        {
            mc.AddMaps(typeof(MappingProfiles).Assembly);
        }).CreateMapper().ConfigurationProvider;

        this.mapper = new Mapper(mockMapper);
        this.controllerUnderTest = new AuctionController(this.auctionRepository.Object, this.mapper, this.publishEndpoint.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = Helpers.GetClaimsPrincipal(userName)
                }
            }
        };
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
        this.auctionRepository.Setup(x => x.AddAuction(It.IsAny<Auction>()));
        this.auctionRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        // Act:
        var response = await this.controllerUnderTest.CreateAuction(auctionToCreate);
        var createdActionResult = response.Result as CreatedAtActionResult;

        // Assert:
        createdActionResult.Should().NotBeNull();
        createdActionResult.ActionName.Should().Be("GetAuctionById");
        createdActionResult.Value.Should().BeOfType<AuctionDto>();
        var auctionDto = createdActionResult.Value as AuctionDto;
        auctionDto.Should().NotBeNull();
        auctionDto.Seller.Should().Be(this.userName);
        this.auctionRepository.Verify(x => x.AddAuction(It.IsAny<Auction>()), Times.Once);

    }

    [Fact]
    public async Task CreateAuction_SavedFailed_ReturnsBadRequest()
    {
        // Arrange:
        var auctionToCreate = this.fixture.Create<CreateAuctionDto>();
        this.auctionRepository.Setup(x => x.AddAuction(It.IsAny<Auction>()));
        this.auctionRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(false);

        // Act:
        var response = await this.controllerUnderTest.CreateAuction(auctionToCreate);

        // Assert:
        response.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestActionResult = response.Result as BadRequestObjectResult;
        badRequestActionResult.Should().NotBeNull();
        badRequestActionResult.Value.Should().Be("Could not save item to database");
        this.auctionRepository.Verify(x => x.AddAuction(It.IsAny<Auction>()), Times.Once);

    }


    [Fact]
    public async Task UpdateAuction_AuctionNotFound_ReturnsNotFound()
    {
        // arrange:
        var auctionId = RandomValue.Guid();
        var updatedAuction = this.fixture.Create<UpdateAuctionDto>();
        this.auctionRepository.Setup(x => x.GetAuctionEntityById(auctionId)).ReturnsAsync(value: null);

        // act:
        var result = await this.controllerUnderTest.UpdateAuction(auctionId, updatedAuction);

        // assert:
        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundResult>();
        var notFoundResult = result as NotFoundResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task UpdateAuction_UserIsNotSeller_ReturnsForbidden()
    {
        // arrange:
        var auctionId = RandomValue.Guid();
        var updatedAuction = this.fixture.Create<UpdateAuctionDto>();
        var auction = new Auction();
        this.auctionRepository.Setup(x => x.GetAuctionEntityById(auctionId)).ReturnsAsync(auction);

        // act:
        var result = await this.controllerUnderTest.UpdateAuction(auctionId, updatedAuction);

        // assert:
        result.Should().NotBeNull();
        result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task UpdateAuction_SaveSuccessful_ReturnsOk()
    {
        // arrange:
        var auctionId = RandomValue.Guid();
        var updatedAuction = this.fixture.Create<UpdateAuctionDto>();
        var auction = new Auction
        {
            Seller = this.userName,
            Item = new Item()
        };

        this.auctionRepository
            .Setup(x => x.GetAuctionEntityById(auctionId))
            .ReturnsAsync(auction);

        this.auctionRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(true);

        // act:
        var result = await this.controllerUnderTest.UpdateAuction(auctionId, updatedAuction);

        // assert:
        result.Should().NotBeNull();
        result.Should().BeOfType<OkResult>();
        auction.Item.Make.Should().Be(updatedAuction.Make);
        auction.Item.Model.Should().Be(updatedAuction.Model);
        auction.Item.Color.Should().Be(updatedAuction.Color);
        auction.Item.Mileage.Should().Be(updatedAuction.Mileage);
        auction.Item.Year.Should().Be(updatedAuction.Year);

    }

    [Fact]
    public async Task UpdateAuction_SaveUnsuccessful_ReturnsBadRequest()
    {
        // arrange:
        var auctionId = RandomValue.Guid();
        var updatedAuction = this.fixture.Create<UpdateAuctionDto>();
        var auction = this.fixture.Build<Auction>().Without(x => x.Item).Create();
        auction.Item = this.fixture.Build<Item>().Without(x => x.Auction).Create();
        auction.Seller = this.userName;

        this.auctionRepository
            .Setup(x => x.GetAuctionEntityById(auctionId))
            .ReturnsAsync(auction);
        this.auctionRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(false);

        // act:
        var result = await this.controllerUnderTest.UpdateAuction(auctionId, updatedAuction);

        // assert:
        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestObjectResult = result as BadRequestObjectResult;
        badRequestObjectResult.Value.Should().Be("No changes saved");
        auction.Item.Make.Should().Be(updatedAuction.Make);
        auction.Item.Model.Should().Be(updatedAuction.Model);
        auction.Item.Color.Should().Be(updatedAuction.Color);
        auction.Item.Mileage.Should().Be(updatedAuction.Mileage);
        auction.Item.Year.Should().Be(updatedAuction.Year);
    }

    [Fact]
    public async Task DeleteAuction_NoAuction_ReturnsNotFound()
    {
        // arrange:
        var auctionId = RandomValue.Guid();
        this.auctionRepository.Setup(x => x.GetAuctionEntityById(auctionId)).ReturnsAsync(value: null);

        // act:
        var result = await this.controllerUnderTest.DeleteAuction(auctionId);

        // assert:
        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteAuction_UserNotSeller_ReturnsForbid()
    {
        // arrange:
        var auctionId = RandomValue.Guid();
        var auction = this.fixture.Build<Auction>().Without(x => x.Item).Create();
        auction.Item = this.fixture.Build<Item>().Without(x => x.Auction).Create();
        // auction.Seller = this.userName;
        this.auctionRepository.Setup(x => x.GetAuctionEntityById(auctionId)).ReturnsAsync(auction);

        // act:
        var result = await this.controllerUnderTest.DeleteAuction(auctionId);

        // assert:
        result.Should().NotBeNull();
        result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task DeleteAuction_SaveFails_ReturnsBadRequest()
    {
        // arrange:
        var auctionId = RandomValue.Guid();
        var auction = this.fixture.Build<Auction>().Without(x => x.Item).Create();
        auction.Item = this.fixture.Build<Item>().Without(x => x.Auction).Create();
        auction.Seller = this.userName;
        this.auctionRepository.Setup(x => x.GetAuctionEntityById(auctionId)).ReturnsAsync(auction);
        this.auctionRepository.Setup(x => x.RemoveAuction(auction));
        this.auctionRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(false);

        // act:
        var result = await this.controllerUnderTest.DeleteAuction(auctionId);

        // assert:
        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();
        this.auctionRepository.Verify(x => x.RemoveAuction(auction), Times.Once);
    }

    [Fact]
    public async Task DeleteAuction_SaveSucceeds_ReturnsOk()
    {
        // arrange:
        var auctionId = RandomValue.Guid();
        var auction = this.fixture.Build<Auction>().Without(x => x.Item).Create();
        auction.Item = this.fixture.Build<Item>().Without(x => x.Auction).Create();
        auction.Seller = this.userName;
        this.auctionRepository.Setup(x => x.GetAuctionEntityById(auctionId)).ReturnsAsync(auction);
        this.auctionRepository.Setup(x => x.RemoveAuction(auction));
        this.auctionRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);

        // act:
        var result = await this.controllerUnderTest.DeleteAuction(auctionId);

        // assert:
        result.Should().NotBeNull();
        result.Should().BeOfType<OkResult>();
        this.auctionRepository.Verify(x => x.RemoveAuction(auction), Times.Once);
    }
}
