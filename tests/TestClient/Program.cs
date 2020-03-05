using System;
using System.Threading.Tasks;
using Assets.Client;
using Google.Protobuf.WellKnownTypes;
using Service.Assets.Contracts;

namespace TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press enter to start");
            Console.ReadLine();
            var client = new AssetsClient("http://localhost:5001");

            var testData = new TestData();

            await CreateAssetsAsync(client, testData);
            await GetAsync(client, testData);
            await UpdateAssetsAsync(client, testData);

            await CreateAssetPairsAsync(client, testData);
            await GetAssetPairsAsync(client, testData);
            await UpdateAssetPairsAsync(client, testData);

            await DeleteAssetPairsAsync(client, testData);
            await DeleteAssetsAsync(client, testData);
            
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static async Task CreateAssetsAsync(AssetsClient client, TestData testData)
        {
            foreach (var asset in testData.Assets)
            {
                await client.Assets.AddAsync(new AddAssetRequest
                {
                    Id = asset.Id, Name = asset.Name, Accuracy = asset.Accuracy, Description = asset.Description
                });
            }
        }

        private static async Task GetAsync(AssetsClient client, TestData testData)
        {
            var getAllResponse = await client.Assets.GetAllAsync(new Empty());

            foreach (var asset in testData.Assets)
            {
                var getByIdResponse = await client.Assets.GetByIdAsync(new GetAssetByIdRequest {AssetId = asset.Id});
            }
        }

        private static async Task UpdateAssetsAsync(AssetsClient client, TestData testData)
        {
            foreach (var asset in testData.Assets)
            {
                await client.Assets.UpdateAsync(new UpdateAssetRequest
                {
                    Id = asset.Id, Name = asset.Name, Accuracy = asset.Accuracy, Description = asset.Description
                });
            }
        }

        private static async Task DeleteAssetsAsync(AssetsClient client, TestData testData)
        {
            foreach (var asset in testData.Assets)
            {
                await client.Assets.DeleteAsync(new DeleteAssetRequest {AssetId = asset.Id});
            }
        }

        private static async Task CreateAssetPairsAsync(AssetsClient client, TestData testData)
        {
            foreach (var assetPair in testData.AssetPairs)
            {
                await client.AssetPairs.AddAsync(new AddAssetPairRequest
                {
                    Id = assetPair.Id,
                    Name = assetPair.Name,
                    Accuracy = assetPair.Accuracy,
                    BaseAssetId = assetPair.BaseAssetId,
                    QuotingAssetId = assetPair.QuotingAssetId,
                    MinVolume = assetPair.MinVolume,
                    MaxVolume = assetPair.MaxVolume,
                    MaxOppositeVolume = assetPair.MaxOppositeVolume,
                    MarketOrderPriceThreshold = assetPair.MarketOrderPriceThreshold
                });
            }
        }

        private static async Task GetAssetPairsAsync(AssetsClient client, TestData testData)
        {
            var getAllResponse = await client.AssetPairs.GetAllAsync(new Empty());

            foreach (var asset in testData.AssetPairs)
            {
                var getByIdResponse =
                    await client.AssetPairs.GetByIdAsync(new GetAssetPairByIdRequest {AssetPairId = asset.Id});
            }
        }

        private static async Task UpdateAssetPairsAsync(AssetsClient client, TestData testData)
        {
            foreach (var assetPair in testData.AssetPairs)
            {
                await client.AssetPairs.UpdateAsync(new UpdateAssetPairRequest
                {
                    Id = assetPair.Id,
                    Name = assetPair.Name,
                    Accuracy = assetPair.Accuracy,
                    BaseAssetId = assetPair.BaseAssetId,
                    QuotingAssetId = assetPair.QuotingAssetId,
                    MinVolume = assetPair.MinVolume,
                    MaxVolume = assetPair.MaxVolume,
                    MaxOppositeVolume = assetPair.MaxOppositeVolume,
                    MarketOrderPriceThreshold = assetPair.MarketOrderPriceThreshold
                });
            }
        }

        private static async Task DeleteAssetPairsAsync(AssetsClient client, TestData testData)
        {
            foreach (var assetPair in testData.AssetPairs)
            {
                await client.AssetPairs.DeleteAsync(new DeleteAssetPairRequest {AssetPairId = assetPair.Id});
            }
        }
    }
}
