using AuctionService.Data;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.GrpcServices;

public class GrpcAuctionService : GrpcAuction.GrpcAuctionBase
{
    private readonly AuctionDbContext dbContext;
    public GrpcAuctionService(AuctionDbContext dbContext)
    {
        this.dbContext = dbContext;

    }

    public async override Task<GrpcAuctionResponse> GetAuction(
        GetAuctionRequest request,
        ServerCallContext context)
    {
        Console.WriteLine("==> Received Grpc Request for Auction");

        var auction = await this.dbContext.Auctions.FindAsync(Guid.Parse(request.Id));

        if (auction is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Not Found"));
        }

        var response = new GrpcAuctionResponse
        {
            Auction = new GrpcAuctionModel
            {
                AuctionEnd = auction.AuctionEnd.ToString(),
                Id = auction.Id.ToString(),
                // this should be changed to stay as a Decimal.
                ReservePrice = Convert.ToInt32(auction.ReservePrice),
                Seller = auction.Seller
            }
        };

        return response;
    }
}
