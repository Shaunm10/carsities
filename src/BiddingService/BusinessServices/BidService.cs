using System.Security.Claims;
using AutoMapper;
using BiddingService.BusinessServices.ViewModels;
using BiddingService.PersistanceModels;
using MassTransit;
using MessageContracts.Bid;
using MongoDB.Entities;

namespace BiddingService.BusinessServices;

public class BidService : IBidService
{
    private readonly IMapper mapper;
    private readonly IPublishEndpoint publishEndpoint;

    public BidService(IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        this.mapper = mapper;
        this.publishEndpoint = publishEndpoint;
    }

    public async Task<BidDto> SaveAsync(Auction auction, decimal amount, ClaimsPrincipal user)
    {
        var bid = CreateBid(auction.ID, amount, user);

        // determine this bid's Status
        bid.BidStatus = await this.CalculateBidStatusAsync(auction, amount, bid);

        // save it to persistance (mongo).
        await DB.InsertAsync(bid);

        // make sure the auction + search service knows about this bid.
        await this.publishEndpoint.Publish(this.mapper.Map<BidPlaced>(bid));
        
        return this.mapper.Map<BidDto>(bid);
    }

    private Bid CreateBid(string auctionId, decimal amount, ClaimsPrincipal user)
    {
        return new Bid
        {
            Amount = amount,
            AuctionId = auctionId,
            Bidder = user.Identity.Name
        };
    }


    private async Task<BidStatus> CalculateBidStatusAsync(Auction auction, decimal amount, Bid bid)
    {
        BidStatus returnBidStatus = BidStatus.TooLow;

        if (auction.AuctionEnd < DateTime.UtcNow)
        {
            returnBidStatus = BidStatus.Finished;
        }
        else
        {
            Bid highBid = await this.GetHighestBidAsync(auction.ID);

            if (highBid != null && amount > highBid.Amount || highBid == null)
            {
                returnBidStatus = amount > auction.ReservePrice ? BidStatus.Accepted : BidStatus.AcceptedBelowReserve;
            }

            if (highBid != null && bid.Amount < highBid.Amount)
            {
                returnBidStatus = BidStatus.TooLow;
            }
        }

        return returnBidStatus;
    }

    private async Task<Bid> GetHighestBidAsync(string auctionId)
    {
        var highBid = await DB.Find<Bid>()
         .Match(x => x.AuctionId == auctionId)
         .Sort(x => x.Descending(y => y.Amount))
         .ExecuteFirstAsync();

        return highBid;
    }
}
