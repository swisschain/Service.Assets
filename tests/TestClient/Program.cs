﻿using System;
using System.Threading.Tasks;
using Assets.Client;
using Assets.Client.Models.AssetPairs;
using Assets.Client.Models.Assets;

namespace TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press enter to start");
            Console.ReadLine();
            var client = new AssetsClient(new AssetsClientSettings {ServiceAddress = "http://localhost:5001"});

            var testData = new TestData();

            await CreateAssetsAsync(client, testData);
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
                await client.Assets.AddAsync(new AssetEditModel
                {
                    BrokerId = asset.BrokerId, Symbol = asset.Symbol, Accuracy = asset.Accuracy, Description = asset.Description
                });
            }
        }

        private static async Task UpdateAssetsAsync(AssetsClient client, TestData testData)
        {
            foreach (var asset in testData.Assets)
            {
                await client.Assets.UpdateAsync(new AssetEditModel
                {
                    BrokerId = asset.BrokerId, Symbol = asset.Symbol, Accuracy = asset.Accuracy, Description = asset.Description
                });
            }
        }

        private static async Task DeleteAssetsAsync(AssetsClient client, TestData testData)
        {
            foreach (var asset in testData.Assets)
            {
                await client.Assets.DeleteAsync(asset.BrokerId, asset.Symbol);
            }
        }

        private static async Task CreateAssetPairsAsync(AssetsClient client, TestData testData)
        {
            foreach (var assetPair in testData.AssetPairs)
            {
                await client.AssetPairs.AddAsync(new AssetPairEditModel
                {
                    BrokerId = assetPair.BrokerId,
                    Symbol = assetPair.Symbol,
                    Accuracy = assetPair.Accuracy,
                    BaseAsset = assetPair.BaseAsset,
                    QuotingAsset = assetPair.QuotingAsset,
                    MinVolume = decimal.Parse(assetPair.MinVolume),
                    MaxVolume = decimal.Parse(assetPair.MaxVolume),
                    MaxOppositeVolume = decimal.Parse(assetPair.MaxOppositeVolume),
                    MarketOrderPriceThreshold = decimal.Parse(assetPair.MarketOrderPriceThreshold)
                });
            }
        }

        private static async Task GetAssetPairsAsync(AssetsClient client, TestData testData)
        {
            var getAllResponse = await client.AssetPairs.GetAllAsync();

            foreach (var assetPair in testData.AssetPairs)
            {
                var getByIdResponse = await client.AssetPairs.GetBySymbolAsync(assetPair.BrokerId, assetPair.Symbol);
            }
        }

        private static async Task UpdateAssetPairsAsync(AssetsClient client, TestData testData)
        {
            foreach (var assetPair in testData.AssetPairs)
            {
                await client.AssetPairs.UpdateAsync(new AssetPairEditModel
                {
                    Symbol = assetPair.Symbol,
                    Accuracy = assetPair.Accuracy,
                    BaseAsset = assetPair.BaseAsset,
                    QuotingAsset = assetPair.QuotingAsset,
                    MinVolume = decimal.Parse(assetPair.MinVolume),
                    MaxVolume = decimal.Parse(assetPair.MaxVolume),
                    MaxOppositeVolume = decimal.Parse(assetPair.MaxOppositeVolume),
                    MarketOrderPriceThreshold = decimal.Parse(assetPair.MarketOrderPriceThreshold)
                });
            }
        }

        private static async Task DeleteAssetPairsAsync(AssetsClient client, TestData testData)
        {
            foreach (var assetPair in testData.AssetPairs)
            {
                await client.AssetPairs.DeleteAsync(assetPair.BrokerId, assetPair.Symbol);
            }
        }
    }
}
