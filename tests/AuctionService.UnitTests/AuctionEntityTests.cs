using AuctionService.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using RandomTestValues;

namespace AuctionService.UnitTests;

public class AuctionEntityTests
{
    [Fact]
    public void HasReservePrice_ReservePriceGreaterThanZero_True()
    {
        // arrange:
        var auction = new Auction
        {
            ReservePrice = RandomValue.Int(9999, 1)
        };

        // act:
        var result = auction.HasReservedPrice();

        // assert:
        result.Should().BeTrue();
    }

    [Fact]
    public void HasReservePrice_ReservePriceIsZero_False()
    {
        // arrange:
        var auction = new Auction
        {
            ReservePrice = 0
        };

        // act:
        var result = auction.HasReservedPrice();

        // assert:
        result.Should().BeFalse();
    }
}