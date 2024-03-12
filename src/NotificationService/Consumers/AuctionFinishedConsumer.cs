using MassTransit;
using MessageContracts.Auction;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    private readonly IHubContext<NotificationHub> hubContext;


    public AuctionFinishedConsumer(IHubContext<NotificationHub> hubContext)
    {
        this.hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        Console.WriteLine("--> auction finished message received");
        // call the SignalR hub to let connected clients know.
        await this.hubContext.Clients.All.SendAsync("AuctionFinished", context.Message);
    }
}
