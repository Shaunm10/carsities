using AuctionService.Controllers;
using AuctionService.Data;
using AuctionService.RequestHelpers;
using AutoFixture;
using AutoMapper;
using MassTransit;
using Moq;

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
}
