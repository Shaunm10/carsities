using AuctionService.Entities;
using Eric.Morrison;
using FluentAssert;
using Microsoft.AspNetCore.Http;

namespace AuctionService.UnitTests;

public class AuctionEntityTests
{
    [Fact]
    public void HasReservePrice_ReservePriceGreaterThanZero_True()
    {
        // arrange:
        var auction = new Auction
        {
            ReservePrice = RandomValue.NextInt32(1, 9999)
        };

        // act:
        var result = auction.HasReservedPrice();

        // assert:
        result.ShouldBeTrue();
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
        result.ShouldBeFalse();
    }
}