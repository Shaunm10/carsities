using AuctionService.Data;
using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication;

namespace AuctionService.IntegrationTests.Util;

public static class DbHelper
{
    public static void InitDbForTests(AuctionDbContext db)
    {
        db.Auctions.AddRange(GetAuctionsForTest());
        db.SaveChanges();
    }

    public static void ReinitDbForTests(AuctionDbContext db)
    {
        db.Auctions.RemoveRange(db.Auctions);
        db.SaveChanges();
        InitDbForTests(db);
    }

    private static List<Auction> GetAuctionsForTest()
    {
        return new List<Auction>
        {
            // 1 Ford GT
            new Auction
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "bob",
                AuctionEnd = DateTime.UtcNow.AddDays(10),
                Item = new Item
                {
                    Make = "Ford",
                    Model = "GT",
                    Color = "White",
                    Mileage = 50000,
                    Year = 2020,
                    ImageUrl = "https://cdn.pixabay.com/photo/2016/05/06/16/32/car-1376190_960_720.jpg"
                }
            },
            // 2 Bugatti Veyron
            new Auction
            {
                Id = Guid.Parse("c8c3ec17-01bf-49db-82aa-1ef80b833a9f"),
                Status = Status.Live,
                ReservePrice = 90000,
                Seller = "alice",
                AuctionEnd = DateTime.UtcNow.AddDays(60),
                Item = new Item
                {
                    Make = "Bugatti",
                    Model = "Veyron",
                    Color = "Black",
                    Mileage = 15035,
                    Year = 2018,
                    ImageUrl = "https://cdn.pixabay.com/photo/2012/05/29/00/43/car-49278_960_720.jpg"
                }
            }

         };
    }

}