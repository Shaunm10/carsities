using MassTransit;
using MessageContracts.Auction;
using SearchService.Business;

namespace SearchService.Consumers;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    private readonly ISearchService searchService;

    public AuctionDeletedConsumer(ISearchService searchService)
    {
        this.searchService = searchService;

    }

    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        await this.searchService.DeleteAuctionAsync(context.Message.Id);
    }
}
