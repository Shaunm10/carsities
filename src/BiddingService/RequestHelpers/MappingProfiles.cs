using AutoMapper;
using BiddingService.BusinessServices.ViewModels;
using BiddingService.PersistanceModels;

namespace BiddingService.MappingProfiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        this.CreateMap<Bid, BidDto>();
    }
}
