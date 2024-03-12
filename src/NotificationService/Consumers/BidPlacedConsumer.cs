using MassTransit;
using MessageContracts.Bid;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly IHubContext<NotificationHub> hubContext;

    public BidPlacedConsumer(IHubContext<NotificationHub> hubContext)
    {
        this.hubContext = hubContext;
    }

    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine("--> bid placed message received");

        // call the SignalR hub to let connected clients know.
        await this.hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
    }
}
