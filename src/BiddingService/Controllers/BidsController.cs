using BiddingService.PersistanceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    public BidsController()
    {
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Bid>> PlaceBid(string auctionId, decimal amount)
    {
        var auction = await DB.Find<Auction>().OneAsync(auctionId);

        if (auction != null)
        {
            // TODO: check with auction service if that has the auction
            return NotFound();
        }

        if (this.DoesAuctionBelongToSeller(auction))
        {
            // this is to prevent a user from pumping up the price of their own bid.
            return BadRequest("You cannot bid on your own auction");
        }

        var bid = CreateBid(auctionId, amount);

        // determine this bid's Status
        bid.BidStatus = await this.CalculateBidStatusAsync(auction, amount, bid);

        // save it to persistance.
        await DB.InsertAsync(bid);

        return this.Ok();
    }

    [HttpGet("{auctionId}")]
    public async Task<ActionResult<List<Bid>>> GetBidsForAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>()
            .Match(x => x.AuctionId == auctionId)
            .Sort(x => x.Descending(y => y.BidTime))
            .ExecuteAsync();

        return bids;
    }


    // TODO: put these in business service.
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

    private Bid CreateBid(string auctionId, decimal amount)
    {
        return new Bid
        {
            Amount = amount,
            AuctionId = auctionId,
            Bidder = User.Identity.Name
        };

    }

    private bool DoesAuctionBelongToSeller(Auction auction)
    {
        return auction.Seller == User.Identity.Name;
    }
}
