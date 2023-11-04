using MassTransit;
using MessageContracts.Auction;

namespace AuctionService.Consumers;

// this is how we get messages back from the Search Service that the message
// couldn't be processed.
public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine("--> Consuming Faulting creation" + context.ToString());

        var exception = context.Message.Exceptions.FirstOrDefault();

        if (exception is not null)
        {
            if (exception.ExceptionType == "System.ArgumentException")
            {
                context.Message.Message.Model = "FooBar";
                await context.Publish(context.Message.Message);
            }
            else
            {
                Console.WriteLine("Not a ArgumentException, so update a dashboard else where.");
            }
        }
    }
}
