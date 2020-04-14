using Assets.Domain.Entities;
using Assets.Repositories.Entities;
using AutoMapper;

namespace Assets.Repositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Asset, AssetEntity>(MemberList.Source)
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.Modified, opt => opt.Ignore());
            CreateMap<AssetEntity, Asset>(MemberList.Destination);

            CreateMap<AssetPair, AssetPairEntity>(MemberList.Source)
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.Modified, opt => opt.Ignore());
            CreateMap<AssetPairEntity, AssetPair>(MemberList.Destination);
        }
    }
}
