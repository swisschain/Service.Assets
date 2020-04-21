using System.Threading.Tasks;
using Assets.Domain.Services;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Service.Assets.Contracts;

namespace Assets.Grpc
{
    public class AssetPairsService : AssetPairs.AssetPairsBase
    {
        private readonly IAssetPairsService _assetPairsService;
        private readonly IMapper _mapper;

        public AssetPairsService(IAssetPairsService assetPairsService, IMapper mapper)
        {
            _assetPairsService = assetPairsService;
            _mapper = mapper;
        }

        public override async Task<GetAllAssetPairsResponse> GetAll(Empty request, ServerCallContext context)
        {
            var assetPairs = await _assetPairsService.GetAllAsync();

            var response = new GetAllAssetPairsResponse();

            response.AssetPairs.AddRange(_mapper.Map<AssetPair[]>(assetPairs));

            return response;
        }

        public override async Task<GetAllAssetPairsResponse> GetAllByBrokerIds(GetAllAssetPairsByBrokerIdsRequest request, ServerCallContext context)
        {
            var assetPairs = await _assetPairsService.GetAllAsync(request.BrokerIds);

            var response = new GetAllAssetPairsResponse();

            response.AssetPairs.AddRange(_mapper.Map<AssetPair[]>(assetPairs));

            return response;
        }

        public override async Task<GetAllAssetPairsResponse> GetAllByBrokerId(GetAllAssetPairsByBrokerIdRequest request, ServerCallContext context)
        {
            var assetPairs = await _assetPairsService.GetAllAsync(request.BrokerId);

            var response = new GetAllAssetPairsResponse();

            response.AssetPairs.AddRange(_mapper.Map<AssetPair[]>(assetPairs));

            return response;
        }

        public override async Task<GetAssetPairBySymbolResponse> GetBySymbol(GetAssetPairBySymbolRequest request,
            ServerCallContext context)
        {
            var assetPair = await _assetPairsService.GetBySymbolAsync(request.BrokerId, request.Symbol);

            return new GetAssetPairBySymbolResponse { AssetPair = _mapper.Map<AssetPair>(assetPair) };
        }

        public override async Task<AddAssetPairResponse> Add(AddAssetPairRequest request, ServerCallContext context)
        {
            var assetPair = await _assetPairsService.AddAsync(request.BrokerId, request.Symbol, request.BaseAsset,
                request.QuotingAsset, request.Accuracy, decimal.Parse(request.MinVolume),
                decimal.Parse(request.MaxVolume), decimal.Parse(request.MaxOppositeVolume),
                decimal.Parse(request.MarketOrderPriceThreshold), request.IsDisabled);

            return new AddAssetPairResponse {AssetPair = _mapper.Map<AssetPair>(assetPair)};
        }

        public override async Task<Empty> Update(UpdateAssetPairRequest request, ServerCallContext context)
        {
            await _assetPairsService.UpdateAsync(request.BrokerId, request.Symbol,
                request.Accuracy, decimal.Parse(request.MinVolume),
                decimal.Parse(request.MaxVolume), decimal.Parse(request.MaxOppositeVolume),
                decimal.Parse(request.MarketOrderPriceThreshold), request.IsDisabled);

            return new Empty();
        }

        public override async Task<Empty> Delete(DeleteAssetPairRequest request, ServerCallContext context)
        {
            await _assetPairsService.DeleteAsync(request.BrokerId, request.Symbol);

            return new Empty();
        }
    }
}
