using BiddingService.PersistanceModels;
using MongoDB.Entities;

namespace BiddingService.BusinessServices
{
    public class AuctionService : IAuctionService
    {
        public async Task<Auction> GetAuctionAsync(string auctionId)
        {
            var auction = await DB.Find<Auction>().OneAsync(auctionId);

            return auction;
        }
    }
}