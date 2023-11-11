

namespace MessageContracts.Auction;

public record AuctionFinished
{
    public bool ItemSold { get; set; }
    public string AuctionId { get; set; }
    public string Winner { get; set;}
    public string Seller { get; set; }
    public decimal Amount { get; set; }
}
