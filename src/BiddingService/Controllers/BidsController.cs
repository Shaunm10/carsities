using AutoMapper;
using BiddingService.BusinessServices;
using BiddingService.BusinessServices.ViewModels;
using BiddingService.GrpcServices;
using BiddingService.PersistanceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IAuctionService auctionService;
    private readonly IBidService bidService;
    private readonly GrpcAuctionClient grpcAuctionClient;

    public BidsController(
        IMapper mapper,
        IAuctionService auctionService,
        IBidService bidService,
        GrpcAuctionClient grpcAuctionClient)
    {
        this.mapper = mapper;
        this.auctionService = auctionService;
        this.bidService = bidService;
        this.grpcAuctionClient = grpcAuctionClient;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BidDto>> PlaceBid(string auctionId, decimal amount)
    {
        var auction = await this.auctionService.GetAuctionAsync(auctionId);

        if (auction is null)
        {
            // than call the auction service over Grpc to try and get it.
            auction = await this.grpcAuctionClient.GetAuctionAsync(auctionId);

            if (auction is null)
            {
                return BadRequest($"Unable to find auction with Id {auctionId}");
            }
        }

        if (this.DoesAuctionBelongToCurrentUser(auction))
        {
            // this is to prevent a user from pumping up the price of their own bid.
            return BadRequest("You cannot bid on your own auction");
        }

        var bid = await this.bidService.SaveAsync(auction, amount, this.User);

        return this.Ok(bid);
    }

    [HttpGet("{auctionId}")]
    public async Task<ActionResult<List<BidDto>>> GetBidsForAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>()
            .Match(x => x.AuctionId == auctionId)
            .Sort(x => x.Descending(y => y.BidTime))
            .ExecuteAsync();

        return bids.Select(this.mapper.Map<BidDto>).ToList();
    }


    private bool DoesAuctionBelongToCurrentUser(Auction auction)
    {
        return auction.Seller == User.Identity.Name;
    }
}
