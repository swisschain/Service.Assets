using System.Globalization;
using System.Threading.Tasks;
using Service.Assets.Contracts;

namespace Assets.Client.Extensions
{
    public static class AssetPairsClientExtensions
    {
        public static async Task<AssetPair> AddAsync(this AssetPairs.AssetPairsClient client, string id, string name,
            string baseAssetId, string quotingAssetId, int accuracy, decimal minVolume, decimal maxVolume,
            decimal maxOppositeVolume, decimal marketOrderPriceThreshold)
        {
            var response = await client.AddAsync(new AddAssetPairRequest
            {
                Id = id,
                Name = name,
                Accuracy = accuracy,
                BaseAssetId = baseAssetId,
                QuotingAssetId = quotingAssetId,
                MinVolume = minVolume.ToString(CultureInfo.InvariantCulture),
                MaxVolume = maxVolume.ToString(CultureInfo.InvariantCulture),
                MaxOppositeVolume = maxOppositeVolume.ToString(CultureInfo.InvariantCulture),
                MarketOrderPriceThreshold = marketOrderPriceThreshold.ToString(CultureInfo.InvariantCulture),
            });

            return response.AssetPair;
        }
        
        public static async Task UpdateAsync(this AssetPairs.AssetPairsClient client, string id, string name,
            string baseAssetId, string quotingAssetId, int accuracy, decimal minVolume, decimal maxVolume,
            decimal maxOppositeVolume, decimal marketOrderPriceThreshold)
        {
            await client.UpdateAsync(new UpdateAssetPairRequest
            {
                Id = id,
                Name = name,
                Accuracy = accuracy,
                BaseAssetId = baseAssetId,
                QuotingAssetId = quotingAssetId,
                MinVolume = minVolume.ToString(CultureInfo.InvariantCulture),
                MaxVolume = maxVolume.ToString(CultureInfo.InvariantCulture),
                MaxOppositeVolume = maxOppositeVolume.ToString(CultureInfo.InvariantCulture),
                MarketOrderPriceThreshold = marketOrderPriceThreshold.ToString(CultureInfo.InvariantCulture),
            });
        }
    }
}
