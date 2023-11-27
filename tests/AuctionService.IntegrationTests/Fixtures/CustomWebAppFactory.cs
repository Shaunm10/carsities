using AuctionService.Data;
using AuctionService.IntegrationTests.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
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
        await this.postgreSqlContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<AuctionDbContext>();

            services.AddDbContext<AuctionDbContext>(options =>
            {
                options.UseNpgsql(this.postgreSqlContainer.GetConnectionString());
            });

            // this will remove the current services and replace it with a test one.
            services.AddMassTransitTestHarness();

            services.EnsureCreated<AuctionDbContext>();

        });
        //base.ConfigureWebHost(builder);
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        // close the container
        await this.postgreSqlContainer.DisposeAsync();
    }
}