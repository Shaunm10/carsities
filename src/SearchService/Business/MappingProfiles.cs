using AutoMapper;
using MessageContracts.Auction;
using SearchService.Models;

namespace SearchService.Business;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AuctionCreation, Item>();

    }
}
