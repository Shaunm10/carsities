using AutoMapper;
using MassTransit;
using MessageContracts.Auction;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreation>
{
    private readonly IMapper mapper;


    public AuctionCreatedConsumer(IMapper mapper)
    {
        this.mapper = mapper;

    }
    public async Task Consume(ConsumeContext<AuctionCreation> context)
    {
        Console.WriteLine($"Consuming auction created: {context.Message.Id}");

        var item = this.mapper.Map<Item>(context.Message);

        await item.SaveAsync();

    }
}
