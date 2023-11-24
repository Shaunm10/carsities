using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionRepository : IAuctionRepository
{
    private readonly AuctionDbContext context;
    private readonly IMapper mapper;

    public AuctionRepository(AuctionDbContext context, IMapper mapper)
    {
        this.mapper = mapper;
        this.context = context;
    }

    public void AddAuction(Auction auction)
    {
        this.context.Auctions.Add(auction);
    }

    public async Task<AuctionDto?> GetAuctionByIdAsync(Guid id)
    {
        return await this.context.Auctions
            .ProjectTo<AuctionDto>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Auction?> GetAuctionEntityById(Guid id)
    {
        var auction = await this.context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        return auction;
    }

    public async Task<List<AuctionDto>> GetAuctionsAsync(string? date)
    {
        var query = this.context.Auctions
            .OrderBy(x => x.Item.Make)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(date))
        {
            query = query
                .Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<AuctionDto>(this.mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public void RemoveAuction(Auction auction)
    {
        this.context.Auctions.Remove(auction);
    }

    public async Task<bool> SaveChangesAsync()
    {
        var count = await this.context.SaveChangesAsync();
        return count > 0;
    }
}
