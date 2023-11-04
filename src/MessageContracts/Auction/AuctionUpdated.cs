namespace MessageContracts.Auction;

public record AuctionUpdated
{
    public string? Id { get; set; }

    public string? Make { get; init; }

    public string? Model { get; init; }

    public int? Year { get; init; }

    public string? Color { get; init; }

    public int? Mileage { get; init; }
}
