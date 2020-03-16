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

        public async Task<AssetModel> GetByIdAsync(string assetId)
        {
            var response = await _client.GetByIdAsync(new GetAssetByIdRequest {AssetId = assetId});

            return response.Asset != null
                ? new AssetModel(response.Asset)
                : null;
        }

        public async Task<AssetModel> AddAsync(AssetEditModel model)
        {
            var response = await _client.AddAsync(new AddAssetRequest
            {
                Id = model.Id, Name = model.Name, Description = model.Description, Accuracy = model.Accuracy
            });

            return new AssetModel(response.Asset);
        }

        public async Task UpdateAsync(AssetEditModel model)
        {
            await _client.UpdateAsync(new UpdateAssetRequest
            {
                Id = model.Id, Name = model.Name, Description = model.Description, Accuracy = model.Accuracy
            });
        }

        public async Task DeleteAsync(string assetId)
        {
            await _client.DeleteAsync(new DeleteAssetRequest {AssetId = assetId});
        }
    }
}
