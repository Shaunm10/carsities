using SearchService.ViewModels;

namespace SearchService.Business;

public interface ISearchService
{
    Task DeleteAuctionAsync(string? id);
    Task<SearchResponse> SearchAsync(SearchRequest searchRequest);
}
