using AutoMapper;
using MassTransit;
using MessageContracts.Auction;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
    {
        private readonly IMapper mapper;

        public AuctionUpdatedConsumer(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            var item = this.mapper.Map<Item>(context.Message);

            var result = await DB.Update<Item>()
                .Match(x => x.ID == item.ID)
                .ModifyOnly(x => new
                {
                    x.Color,
                    x.Model,
                    x.Year,
                    x.Mileage
                }, item)
                // .Modify(x => x.Make, item.Make)
                // .Modify(x => x.Model, item.Model)
                // .Modify(x => x.Year, item.Year)
                // .Modify(x => x.Color, item.Color)
                // .Modify(x => x.Mileage, item.Mileage)
                .ExecuteAsync();

            if (!result.IsAcknowledged)
            {
                throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb");
            }
        }


    }
}