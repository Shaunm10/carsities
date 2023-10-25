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
        var baseAuctionServiceUrl = this.config["AuctionServiceUrl"];

        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(a => a.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        var fullAuctionServiceUrl = $"{baseAuctionServiceUrl}/api/auctions?date={lastUpdated}";
        return await this.httpClient.GetFromJsonAsync<List<Item>>(fullAuctionServiceUrl);
    }
}
