using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MassTransit;
using MessageContracts.Auction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly AuctionDbContext context;
    private readonly IPublishEndpoint publishEndpoint;

    public AuctionController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<List<AuctionDto>> GetAllAuctions(string? date)
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

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid Id)
    {
        var auction = await this.context
            .Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == Id);

        if (auction is null)
        {
            return this.NotFound();
        }

        var response = this.mapper.Map<AuctionDto>(auction);

        return response;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        var auction = this.mapper.Map<Auction>(createAuctionDto);

        auction.Seller = this.User?.Identity?.Name;
        this.context.Auctions.Add(auction);

        var newAuction = this.mapper.Map<AuctionDto>(auction);

        // call to the service bus
        await this.publishEndpoint.Publish(this.mapper.Map<AuctionCreated>(newAuction));
        var itemWasCreated = await this.context.SaveChangesAsync() > 0;

        if (!itemWasCreated)
        {
            return this.BadRequest("Could not save item to database");
        }

        var auctionToReturn = this.mapper.Map<AuctionDto>(auction);

        return this.CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, newAuction);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updatedAuctionDto)
    {
        var auction = await this.context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (auction is null)
        {
            return this.NotFound();
        }

        // Verify user is the actual seller.
        if (auction.Seller != this.User?.Identity?.Name)
        {
            return this.Forbid();
        }

        auction.Item.Make = updatedAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updatedAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updatedAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updatedAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updatedAuctionDto.Year ?? auction.Item.Year;

        var updatedAuctionEvent = this.mapper.Map<AuctionUpdated>(auction);
        await this.publishEndpoint.Publish(updatedAuctionEvent);

        var anyChangesProcessed = await this.context.SaveChangesAsync() > 0;

        if (anyChangesProcessed)
        {
            updatedAuctionEvent.Id = id.ToString();

            return this.Ok();
        }

        // something vague for the moment.
        return this.BadRequest("No changes saved");
    }

    [Authorize]

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auctionToRemove = await this.context.Auctions
            .FindAsync(id);

        // Verify user is the actual seller.
        if (auctionToRemove.Seller != this.User?.Identity?.Name)
        {
            return this.Forbid();
        }

        if (auctionToRemove is null)
        {
            return this.NotFound();
        }

        this.context.Auctions.Remove(auctionToRemove);

        await this.publishEndpoint.Publish(new AuctionDeleted
        {
            Id = id.ToString()
        });

        var changesWereSaved = await this.context.SaveChangesAsync() > 0;

        if (!changesWereSaved)
        {
            return this.BadRequest("Delete unsuccessful");
        }

        return Ok();
    }
}
