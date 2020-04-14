using System.Threading.Tasks;
using Assets.Client.Api;
using Assets.Client.Models.Assets;
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

        public async Task<AssetModel> GetByIdAsync(string brokerId, string id)
        {
            var response = await _client.GetByIdAsync(new GetAssetByIdRequest { BrokerId = brokerId, Id = id });

            return response.Asset != null
                ? new AssetModel(response.Asset)
                : null;
        }

        public async Task<AssetModel> AddAsync(AssetEditModel model)
        {
            var response = await _client.AddAsync(new AddAssetRequest
            {
                BrokerId = model.BrokerId,
                Id = model.Id,
                Name = model.Name,
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
                Name = model.Name,
                Description = model.Description,
                Accuracy = model.Accuracy,
                IsDisabled = model.IsDisabled
            });
        }

        public async Task DeleteAsync(string brokerId, string id)
        {
            await _client.DeleteAsync(new DeleteAssetRequest { BrokerId = brokerId, Id = id });
        }
    }
}
