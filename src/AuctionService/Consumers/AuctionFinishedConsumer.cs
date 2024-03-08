using AuctionService.Data;
using AuctionService.Entities;
using MassTransit;
using MessageContracts.Auction;

namespace AuctionService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly AuctionDbContext dbContext;
    public AuctionFinishedConsumer(AuctionDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("--> consuming auction finished");

        var auction = await this.dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

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

            await this.dbContext.SaveChangesAsync();
        }
    }
}
