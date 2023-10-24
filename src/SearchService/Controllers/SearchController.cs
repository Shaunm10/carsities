using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Business;
using SearchService.Models;
using SearchService.ViewModels;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/Search")]
    public class SearchController : ControllerBase
    {
        public ISearchService searchService { get; }

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;

        }
        [HttpGet]
        public async Task<ActionResult<SearchResponse>> SearchItems([FromQuery] SearchRequest searchRequest)
        {
            var result = await this.searchService.SearchAsync(searchRequest);
            return this.Ok(result);
        }
    }
}