using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Client.Api;
using Assets.Client.Models.Assets;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Service.Assets.Contracts;

namespace Assets.Client.Grpc
{
    internal class AssetsApi : IAssetsApi
    {
        private readonly Service.Assets.Contracts.Assets.AssetsClient _client;

        public AssetsApi(string address)
        {
            var channel = GrpcChannel.ForAddress(address);
            _client = new Service.Assets.Contracts.Assets.AssetsClient(channel);
        }

        public async Task<IReadOnlyList<AssetModel>> GetAllAsync()
        {
            var response = await _client.GetAllAsync(new Empty());

            return response.Assets
                .Select(asset => new AssetModel(asset))
                .ToList();
        }

        public async Task<IReadOnlyList<AssetModel>> GetAllByBrokerIds(IEnumerable<string> brokerIds)
        {
            var response = await _client.GetAllByBrokerIdsAsync(new GetAllAssetsByBrokerIdsRequest { BrokerIds = { brokerIds } });

            return response.Assets
                .Select(asset => new AssetModel(asset))
                .ToList();
        }

        public async Task<IReadOnlyList<AssetModel>> GetAllByBrokerId(string brokerId)
        {
            var response = await _client.GetAllByBrokerIdAsync(new GetAllAssetsByBrokerIdRequest { BrokerId = brokerId });

            return response.Assets
                .Select(asset => new AssetModel(asset))
                .ToList();
        }

        public async Task<AssetModel> GetByIdAsync(long id, string brokerId)
        {
            var response = await _client.GetByIdAsync(new GetAssetByIdRequest { Id = id, BrokerId = brokerId });

            return response.Asset != null
                ? new AssetModel(response.Asset)
                : null;
        }

        public async Task<AssetModel> AddAsync(AssetEditModel model)
        {
            var response = await _client.AddAsync(new AddAssetRequest
            {
                BrokerId = model.BrokerId,
                Symbol = model.Symbol,
                Description = model.Description,
                Accuracy = model.Accuracy,
                IsDisabled = model.IsDisabled
            });

            return new AssetModel(response.Asset);
        }

        public async Task UpdateAsync(AssetEditModel model)
        {
            await _client.UpdateAsync(new UpdateAssetRequest
            {
                BrokerId = model.BrokerId,
                Id = model.Id,
                Symbol = model.Symbol,
                Description = model.Description,
                Accuracy = model.Accuracy,
                IsDisabled = model.IsDisabled
            });
        }

        public async Task DeleteAsync(long id, string brokerId)
        {
            await _client.DeleteAsync(new DeleteAssetRequest { Id = id, BrokerId = brokerId });
        }
    }
}
