using System.Security.Claims;
using BiddingService.BusinessServices.ViewModels;
using BiddingService.PersistanceModels;

namespace BiddingService.BusinessServices;

public interface IBidService
{
    Task<BidDto> SaveAsync(Auction auction, decimal amount, ClaimsPrincipal user);
}
