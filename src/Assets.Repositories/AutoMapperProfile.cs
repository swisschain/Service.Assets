using Assets.Domain.Entities;
using Assets.Repositories.Entities;
using AutoMapper;

namespace Assets.Repositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Asset, AssetEntity>(MemberList.Source);
            CreateMap<AssetEntity, Asset>(MemberList.Destination);

            CreateMap<AssetPair, AssetPairEntity>(MemberList.Source);
            CreateMap<AssetPairEntity, AssetPair>(MemberList.Destination);
        }
    }
}
