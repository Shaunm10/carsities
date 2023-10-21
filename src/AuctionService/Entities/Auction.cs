using System.Runtime;

namespace AuctionService.Entities;

public class Auction
{
    public Guid Id { get; set; }
    public decimal ReservePrice { get; set; } = 0;
    public string? Seller { get; set; } = null;
    public string? Winner { get; set; } = null;
    public decimal? SoldAmount { get; set; }
    public decimal? CurrentHighBid { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; }
    public Item? Item { get; set; }
    public DateTime AuctionEnd { get; set; }
}
