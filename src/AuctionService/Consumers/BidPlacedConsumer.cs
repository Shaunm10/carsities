using AuctionService.Data;
using MassTransit;
using MessageContracts;
using MessageContracts.Bid;

namespace AuctionService.Consumers;

public record BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly AuctionDbContext dbContext;

    public BidPlacedConsumer(AuctionDbContext dbContext)
    {
        this.dbContext = dbContext;

    }

    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine("--> consuming bid placed");
        var auction = await this.dbContext.Auctions.FindAsync(Guid.Parse(context?.Message?.AuctionId ?? ""));

        if (auction is not null && !string.IsNullOrWhiteSpace(context.Message.BidStatus))
        {
            if (auction.CurrentHighBid == null
            || context.Message.BidStatus.Contains(BidStatuses.Accepted) &&
            context.Message.Amount > auction.CurrentHighBid)
            {
                auction.CurrentHighBid = context.Message.Amount;

                await this.dbContext.SaveChangesAsync();
            }
        }
    }
}
