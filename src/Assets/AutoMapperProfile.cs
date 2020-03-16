using Assets.Client.Models.AssetPairs;
using Assets.Client.Models.Assets;
using Assets.Domain.Entities;
using AutoMapper;

namespace Assets
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Asset, AssetModel>(MemberList.Source);

            CreateMap<AssetPair, AssetPairModel>(MemberList.Source);

            CreateMap<Asset, Service.Assets.Contracts.Asset>(MemberList.Destination);

            CreateMap<AssetPair, Service.Assets.Contracts.AssetPair>(MemberList.Destination);
        }
    }
}
