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
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IAuctionRepository auctionRepository;

    public AuctionController(IAuctionRepository auctionRepository, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        this.auctionRepository = auctionRepository;
        this.publishEndpoint = publishEndpoint;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<List<AuctionDto>> GetAllAuctions(string? date)
    {
        var auctions = await this.auctionRepository.GetAuctionsAsync(date);
        return auctions;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid Id)
    {
        // var auction = await this.context
        //     .Auctions
        //     .Include(x => x.Item)
        //     .FirstOrDefaultAsync(x => x.Id == Id);

        var auction = await this.auctionRepository.GetAuctionByIdAsync(Id);

        if (auction is null)
        {
            return this.NotFound();
        }

        return auction;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        var auction = this.mapper.Map<Auction>(createAuctionDto);

        auction.Seller = this.User?.Identity?.Name;
        this.auctionRepository.AddAuction(auction);

        var newAuction = this.mapper.Map<AuctionDto>(auction);

        // call to the service bus
        await this.publishEndpoint.Publish(this.mapper.Map<AuctionCreated>(newAuction));
        var itemWasCreated = await this.auctionRepository.SaveChangesAsync();

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
        var auction = await this.auctionRepository.GetAuctionEntityById(id);

        if (auction is null)
        {
            return this.NotFound();
        }

        // Verify user is the actual seller.
        if (auction.Seller != this.User?.Identity?.Name)
        {
            return this.Forbid();
        }

        if (auction.Item is not null)
        {
            auction.Item.Make = updatedAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updatedAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updatedAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updatedAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updatedAuctionDto.Year ?? auction.Item.Year;
        }

        var updatedAuctionEvent = this.mapper.Map<AuctionUpdated>(auction);
        await this.publishEndpoint.Publish(updatedAuctionEvent);

        var anyChangesProcessed = await this.auctionRepository.SaveChangesAsync();

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

        var auctionToRemove = await this.auctionRepository.GetAuctionEntityById(id);

        if (auctionToRemove is null)
        {
            return this.NotFound();
        }

        // Verify user is the actual seller.
        if (auctionToRemove.Seller != this.User?.Identity?.Name)
        {
            return this.Forbid();
        }

        if (auctionToRemove is null)
        {
            return this.NotFound();
        }

        this.auctionRepository.RemoveAuction(auctionToRemove);

        await this.publishEndpoint.Publish(new AuctionDeleted
        {
            Id = id.ToString()
        });

        var changesWereSaved = await this.auctionRepository.SaveChangesAsync();

        if (!changesWereSaved)
        {
            return this.BadRequest("Delete unsuccessful");
        }

        return Ok();
    }
}
