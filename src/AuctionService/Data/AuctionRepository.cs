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
        throw new NotImplementedException();
    }

    public Task<AuctionDto> GetAuctionByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Auction> GetAuctionEntityById(Guid Id)
    {
        var auction = this.context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == Id);

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
