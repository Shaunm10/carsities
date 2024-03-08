using AutoMapper;
using BiddingService.BusinessServices.ViewModels;
using BiddingService.PersistanceModels;
using MessageContracts.Bid;

namespace BiddingService.MappingProfiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        this.CreateMap<Bid, BidDto>();
        this.CreateMap<Bid, BidPlaced>();
    }
}
