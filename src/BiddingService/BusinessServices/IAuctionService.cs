using BiddingService.PersistanceModels;

namespace BiddingService.BusinessServices
{
    public interface IAuctionService
    {
        Task<Auction> GetAuctionAsync(string auctionId);

    }
}