using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly AuctionDbContext context;

    public AuctionController(AuctionDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    [HttpGet]
    public async Task<List<AuctionDto>> GetAllAuctions()
    {
        var auctions = await this.context
            .Auctions
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync();

        //return auctions;
        var response = this.mapper.Map<List<AuctionDto>>(auctions);
        return response;
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
}
