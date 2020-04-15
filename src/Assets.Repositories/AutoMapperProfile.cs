using Assets.Domain.Entities;
using Assets.Repositories.Entities;
using AutoMapper;

namespace Assets.Repositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Asset, AssetEntity>(MemberList.Destination)
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.Modified, opt => opt.Ignore());
            CreateMap<AssetEntity, Asset>(MemberList.Destination);

            CreateMap<AssetPair, AssetPairEntity>(MemberList.Destination)
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.Modified, opt => opt.Ignore());
            CreateMap<AssetPairEntity, AssetPair>(MemberList.Destination)
                .ForMember(x => x.BaseAsset, opt => opt.Ignore())
                .ForMember(x => x.QuotingAsset, opt => opt.Ignore());
        }
    }
}
