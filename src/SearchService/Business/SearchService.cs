using MongoDB.Entities;
using SearchService.Models;
using SearchService.ViewModels;

namespace SearchService.Business;

public class SearchService : ISearchService
{
    public async Task<bool> DeleteAuctionAsync(string? id)
    {
        var result = await DB.DeleteAsync<Item>(x => x.ID == id);

        return result.IsAcknowledged;
    }

    public async Task<SearchResponse> SearchAsync(SearchRequest searchRequest)
    {
        var query = DB.PagedSearch<Item, Item>();

        //query.Sort(x => x.Ascending(a => a.Make));

        if (!string.IsNullOrWhiteSpace(searchRequest.SearchTerm))
        {
            query
                .Match(Search.Full, searchRequest.SearchTerm)
                .SortByTextScore();
        }

        query = searchRequest.OrderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make)),
            "new" => query.Sort(x => x.Ascending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd)),
        };

        query = searchRequest.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6)
                && x.AuctionEnd > DateTime.UtcNow),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        };

        if (!string.IsNullOrWhiteSpace(searchRequest.Seller))
        {
            query.Match(x => x.Seller == searchRequest.Seller);
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Winner))
        {
            query.Match(x => x.Winner == searchRequest.Winner);
        }


        query.PageNumber(searchRequest.PageNumber);
        query.PageSize(searchRequest.PageSize);

        var result = await query.ExecuteAsync();
        return new SearchResponse
        {
            Results = result.Results.ToList(),
            PageCount = result.PageCount,
            TotalCount = result.TotalCount
        };
    }

}