using AutoMapper;
using BiddingService.PersistanceModels;
using BiddingService.ViewModels;

namespace BiddingService.MappingProfiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        this.CreateMap<Bid, BidDto>();
    }
}
