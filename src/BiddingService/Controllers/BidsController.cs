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

        var bid = CreateAuction(auctionId, amount);

        if (auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        }

        Bid highBid = await this.GetHighestBidAsync(auctionId);

        if (highBid != null && amount > highBid.Amount || highBid == null)
        {
            bid.BidStatus = amount > auction.ReservePrice ? BidStatus.Accepted : BidStatus.AcceptedBelowReserve;
        }

        if (highBid != null && bid.Amount < highBid.Amount)
        {
            bid.BidStatus = BidStatus.TooLow;
        }

        await DB.InsertAsync(bid);

        return this.Ok();
    }

    private async Task<Bid> GetHighestBidAsync(string auctionId)
    {
        var highBid = await DB.Find<Bid>()
         .Match(x => x.AuctionId == auctionId)
         .Sort(x => x.Descending(y => y.Amount))
         .ExecuteFirstAsync();

        return highBid;
    }

    private Bid CreateAuction(string auctionId, decimal amount)
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
