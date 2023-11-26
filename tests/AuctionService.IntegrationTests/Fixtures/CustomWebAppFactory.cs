using AuctionService.Data;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace AuctionService.IntegrationTests.Fixtures;

public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private PostgreSqlContainer postgreSqlContainer = new PostgreSqlBuilder()
        .Build();

    public async Task InitializeAsync()
    {
        // spin up the container
        await this.postgreSqlContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
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

            services.AddDbContext<AuctionDbContext>(options =>
            {
                options.UseNpgsql(this.postgreSqlContainer.GetConnectionString());
            });

            // this will remove the current services and replace it with a test one.
            services.AddMassTransitTestHarness();

            var sp = services.BuildServiceProvider();

            // get the Db context
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AuctionDbContext>();

            // so we can migration the db used.
            db.Database.Migrate();

        });
        //base.ConfigureWebHost(builder);
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        // close the container
        await this.postgreSqlContainer.DisposeAsync();
    }
}