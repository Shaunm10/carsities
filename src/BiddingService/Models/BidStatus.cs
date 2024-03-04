namespace BiddingService;

public enum BidStatus
{
    /// <summary>
    /// This bid has been accepted and is currently the highest bid.
    /// </summary>
    Accepted,

    /// <summary>
    /// The bid was accepted however the reserve price has not yet been met.
    /// </summary>
    AcceptedBelowReserve,

    /// <summary>
    /// If a user happen to get in a bid in at the some time as someone else we need to notify
    /// the user we can't accept their bid.
    /// </summary>
    TooLow,

    /// <summary>
    /// If a bid get's through and the Auction is already done.
    /// </summary>
    Finished
}
