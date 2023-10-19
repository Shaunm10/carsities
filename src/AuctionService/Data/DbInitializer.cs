using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();

        var context = scope.ServiceProvider.GetService<AuctionDbContext>();

        if (context is not null)
        {
            SeedData(context);
        }
    }

    private static void SeedData(AuctionDbContext context)
    {
        // this will create the Db (if not existent) and apply all the migrations.
        context.Database.Migrate();

        if (context.Auctions.Any())
        {
            Console.WriteLine("Auction data found, skipping seeding.");
            return;
        }

        var auctions = new List<Auction>
        {
            // auctions
        };

        context.AddRange(auctions);
        context.SaveChanges();
    }
}
