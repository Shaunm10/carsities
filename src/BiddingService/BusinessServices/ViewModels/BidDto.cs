﻿namespace BiddingService.BusinessServices.ViewModels;

public class BidDto
{
    public string Id { get; set; }
    public string AuctionId { get; set; }
    public string Bidder { get; set; }
    public DateTime BidTime { get; set; }
    public decimal Amount { get; set; }
    public string BidStatus { get; set; }
}
