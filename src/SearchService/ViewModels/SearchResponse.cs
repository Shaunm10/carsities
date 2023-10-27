
using SearchService.Models;

namespace SearchService.ViewModels;

public class SearchResponse
{
    public List<Item> Results { get; set; } = new List<Item> { };
    public int PageCount { get; set; }
    public long TotalCount { get; set; }

}