using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Assets.Client.Api;
using Assets.Client.Models.AssetPairs;
using Grpc.Net.Client;
using Service.Assets.Contracts;

namespace Assets.Client.Grpc
{
    internal class AssetPairsApi : IAssetPairsApi
    {
        private readonly AssetPairs.AssetPairsClient _client;

        public AssetPairsApi(string address)
        {
            var channel = GrpcChannel.ForAddress(address);
            _client = new AssetPairs.AssetPairsClient(channel);
        }

        public async Task<IReadOnlyList<AssetPairModel>> GetAllAsync(string brokerId)
        {
            var response = await _client.GetAllAsync(new GetAllAssetPairsRequest { BrokerId = brokerId });

            return response.AssetPairs
                .Select(asset => new AssetPairModel(asset))
                .ToList();
        }

        public async Task<AssetPairModel> GetByIdAsync(long id, string brokerId)
        {
            var response = await _client.GetByIdAsync(new GetAssetPairByIdRequest { Id = id, BrokerId = brokerId });

            return response.AssetPair != null
                ? new AssetPairModel(response.AssetPair)
                : null;
        }

        public async Task<AssetPairModel> AddAsync(AssetPairEditModel model)
        {
            var response = await _client.AddAsync(new AddAssetPairRequest
            {
                BrokerId = model.BrokerId,
                Symbol = model.Symbol,
                Accuracy = model.Accuracy,
                BaseAssetId = model.BaseAssetId,
                QuotingAssetId = model.QuotingAssetId,
                MinVolume = model.MinVolume.ToString(CultureInfo.InvariantCulture),
                MaxVolume = model.MaxVolume.ToString(CultureInfo.InvariantCulture),
                MaxOppositeVolume = model.MaxOppositeVolume.ToString(CultureInfo.InvariantCulture),
                MarketOrderPriceThreshold = model.MarketOrderPriceThreshold.ToString(CultureInfo.InvariantCulture),
                IsDisabled = model.IsDisabled
            });

            return new AssetPairModel(response.AssetPair);
        }

        public async Task UpdateAsync(AssetPairEditModel model)
        {
            await _client.UpdateAsync(new UpdateAssetPairRequest
            {
                Id = model.Id,
                BrokerId = model.BrokerId,
                Symbol = model.Symbol,
                Accuracy = model.Accuracy,
                BaseAssetId = model.BaseAssetId,
                QuotingAssetId = model.QuotingAssetId,
                MinVolume = model.MinVolume.ToString(CultureInfo.InvariantCulture),
                MaxVolume = model.MaxVolume.ToString(CultureInfo.InvariantCulture),
                MaxOppositeVolume = model.MaxOppositeVolume.ToString(CultureInfo.InvariantCulture),
                MarketOrderPriceThreshold = model.MarketOrderPriceThreshold.ToString(CultureInfo.InvariantCulture),
                IsDisabled = model.IsDisabled
            });
        }

        public async Task DeleteAsync(long id, string brokerId)
        {
            await _client.DeleteAsync(new DeleteAssetPairRequest { Id =  id, BrokerId = brokerId });
        }
    }
}
