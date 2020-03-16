using System;
using System.Globalization;
using Assets.Client.Models.AssetPairs;
using Assets.Client.Models.Assets;
using Assets.Domain.Entities;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;

namespace Assets
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Asset, AssetModel>(MemberList.Source);

            CreateMap<AssetPair, AssetPairModel>(MemberList.Source);

            CreateMap<Asset, Service.Assets.Contracts.Asset>(MemberList.Destination)
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => Convert(src.Created)))
                .ForMember(dest => dest.Modified, opt => opt.MapFrom(src => Convert(src.Modified)));

            CreateMap<AssetPair, Service.Assets.Contracts.AssetPair>(MemberList.Destination)
                .ForMember(dest => dest.MinVolume,
                    opt => opt.MapFrom(src => src.MinVolume.ToString(CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.MaxVolume,
                    opt => opt.MapFrom(src => src.MinVolume.ToString(CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.MaxOppositeVolume,
                    opt => opt.MapFrom(src => src.MinVolume.ToString(CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.MarketOrderPriceThreshold,
                    opt => opt.MapFrom(src => src.MinVolume.ToString(CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => Convert(src.Created)))
                .ForMember(dest => dest.Modified, opt => opt.MapFrom(src => Convert(src.Modified)));
        }

        private static Timestamp Convert(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            return dateTime.ToTimestamp();
        }
    }
}
