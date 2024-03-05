using MongoDB.Entities;

namespace BiddingService.PersistanceModels;

public class Auction : Entity
{
    public DateTime? AuctionEnd { get; set; }
    public string Seller { get; set; }
    public decimal? ReservePrice { get; set; }
    public bool Finished { get; set; }
}
