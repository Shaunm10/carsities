

using BiddingService.PersistanceModels;
using MassTransit;
using MessageContracts.Auction;
using MongoDB.Entities;

namespace BiddingService.BackgroundServices;

public class CheckAuctionFinished : BackgroundService
{
    private readonly ILogger<CheckAuctionFinished> logger;
    private readonly IServiceProvider services;

    public CheckAuctionFinished(ILogger<CheckAuctionFinished> logger, IServiceProvider services)
    {
        this.logger = logger;
        this.services = services;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("Starting check for finished auctions.");

        stoppingToken.Register(() => this.logger.LogInformation("==> Auction check is stopping."));

        while (!stoppingToken.IsCancellationRequested)
        {
            await this.CheckAuctions(stoppingToken);

            // now wait every 5 seconds
            await Task.Delay(5000);
        }
    }

    private async Task CheckAuctions(CancellationToken stoppingToken)
    {
        // see if there are any auctions that have passed.
        var finishedAuctions = await DB.Find<Auction>()
            .Match(x => x.AuctionEnd <= DateTime.UtcNow)
            .Match(x => !x.Finished)
            .ExecuteAsync(stoppingToken);

        if (!finishedAuctions.Any())
        {
            return;
        }

        this.logger.LogInformation("==> Found {count} auctions that have completed", finishedAuctions.Count);

        using var scope = this.services.CreateScope();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        finishedAuctions.ForEach(async auction =>
        {
            auction.Finished = true;
            await auction.SaveAsync(null, stoppingToken);

            // find the 1 highest bid whom's was accepted
            var winningBid = await DB.Find<Bid>()
                .Match(x => x.AuctionId == auction.ID)
                .Match(x => x.BidStatus == BidStatus.Accepted)
                .Sort(x => x.Descending(s => s.Amount))
                .ExecuteFirstAsync(stoppingToken);

            // NOTE: there may not be a winningBid at this point. (It would be NULL)
            await publishEndpoint.Publish(new AuctionFinished
            {
                ItemSold = winningBid != null,
                AuctionId = auction.ID,
                Winner = winningBid?.Bidder,
                Amount = winningBid?.Amount,
                Seller = auction.Seller
            }, stoppingToken);
        });
    }
}
