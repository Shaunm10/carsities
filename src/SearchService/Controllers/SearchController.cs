using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;
using ZstdSharp.Unsafe;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/Search")]
    public class SearchController : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchRequest searchRequest)
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
            return this.Ok(new
            {
                results = result.Results,
                pageCount = result.PageCount,
                totalCount = result.TotalCount
            });
        }
    }
}