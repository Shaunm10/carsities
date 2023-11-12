using AuctionService.Data;
using AuctionService.Entities;
using MassTransit;
using MessageContracts.Auction;

namespace AuctionService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly AuctionDbContext context;
    public AuctionFinishedConsumer(AuctionDbContext context)
    {
        this.context = context;
    }

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        var auction = await this.context.Auctions.FindAsync(context.Message.AuctionId);

        if (auction is not null)
        {
            // if this item was sold
            if (context.Message.ItemSold)
            {
                auction.Winner = context.Message.Winner;
                auction.SoldAmount = context.Message.Amount;
            }

            if (auction.SoldAmount > auction.ReservePrice)
            {
                auction.Status = Status.Finished;
            }
            else
            {
                auction.Status = Status.ReserveNotMet;
            }

            await this.context.SaveChangesAsync();
        }
    }
}
