namespace MessageContracts.Auction;

public record AuctionCreated
{
    public Guid Id { get; init; }
    public decimal? ReservePrice { get; init; }
    public string? Seller { get; init; } = null;
    public string? Winner { get; init; } = null;
    public decimal? SoldAmount { get; init; }
    public decimal? CurrentHighBid { get; init; }
    public DateTime? CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string? Status { get; init; }

    public DateTime? AuctionEnd { get; init; }

    public string? Make { get; init; }
    public string? Model { get; set; }
    public int? Year { get; init; }
    public string? Color { get; init; }
    public int? Mileage { get; init; }
    public string? ImageUrl { get; init; }
}
