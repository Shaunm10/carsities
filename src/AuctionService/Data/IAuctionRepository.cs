using AuctionService.DTOs;
using AuctionService.Entities;

namespace AuctionService.Data;

public interface IAuctionRepository
{
    Task<List<AuctionDto>> GetAuctionsAsync(string? date);

    Task<AuctionDto> GetAuctionByIdAsync(Guid id);

    Task<Auction> GetAuctionEntityById(Guid Id);

    void AddAuction(Auction action);

    void RemoveAuction(Auction auction);

    Task<bool> SaveChangesAsync();

}
