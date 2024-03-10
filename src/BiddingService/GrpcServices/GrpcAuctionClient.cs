using AuctionService;
using BiddingService.PersistanceModels;
using Grpc.Net.Client;
using MongoDB.Driver.Core.WireProtocol.Messages;

namespace BiddingService.GrpcServices;

public class GrpcAuctionClient
{
    private readonly ILogger<GrpcAuctionClient> logger;
    private readonly IConfiguration config;

    public GrpcAuctionClient(ILogger<GrpcAuctionClient> logger, IConfiguration config)
    {
        this.logger = logger;
        this.config = config;
    }

    public async Task<Auction> GetAuctionAsync(string id)
    {
        this.logger.LogInformation("Calling GRPC service");
        var channel = GrpcChannel.ForAddress(this.config["GrpcAuctionAddress"]);
        var client = new GrpcAuction.GrpcAuctionClient(channel);
        try
        {
            var response = await client.GetAuctionAsync(new GetAuctionRequest
            {
                Id = id
            });

            var auction = new Auction
            {
                ID = response.Auction.Id,
                AuctionEnd = DateTime.Parse(response.Auction.AuctionEnd),
                Seller = response.Auction.Seller,
                ReservePrice = Convert.ToDecimal(response.Auction.ReservePrice)
            };

            return auction;

        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error in calling GrpcAuctionService");
            return null;
        }
    }

}
