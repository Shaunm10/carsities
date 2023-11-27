using AuctionService.Data;
using AuctionService.IntegrationTests.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionService.IntegrationTests.Extensions;

public static class ServiceCollection
{
    public static void RemoveDbContext<T>(this IServiceCollection services)
    {
        // get the current entity framework context
        var dbContext = services
            .SingleOrDefault(x =>
                x.ServiceType == typeof(DbContextOptions<AuctionDbContext>)
            );

        // if found, remove it.
        if (dbContext != null)
        {
            services.Remove(dbContext);
        }
    }

    public static void EnsureCreated<T>(this IServiceCollection services)
    {
         var sp = services.BuildServiceProvider();

            // get the Db context
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AuctionDbContext>();

            // so we can migration the db used.
            db.Database.Migrate();
            DbHelper.InitDbForTests(db);

    }

}
