using SearchService.ViewModels;

namespace SearchService.Business;

public interface ISearchService
{
    Task<bool> DeleteAuctionAsync(string? id);
    Task<SearchResponse> SearchAsync(SearchRequest searchRequest);
}
