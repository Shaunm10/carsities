using SearchService.ViewModels;

namespace SearchService.Business;

public interface ISearchService
{

    public Task<SearchResponse> SearchAsync(SearchRequest searchRequest);
}
