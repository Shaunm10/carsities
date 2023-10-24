using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Business;

public class AuctionSvcHttpClient
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration config;

    public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
    {
        this.httpClient = httpClient;
        this.config = config;
    }

    public async Task<List<Item>?> GetItemsForSearchDb()
    {
        var auctionServiceUrl = this.config["AuctionServiceUrl"];
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(a => a.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        return await this.httpClient.GetFromJsonAsync<List<Item>>(this.config[$"{auctionServiceUrl}/api/auctions?date=${lastUpdated}"]);
    }
}
