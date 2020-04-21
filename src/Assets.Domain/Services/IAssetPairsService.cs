using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Services
{
    public interface IAssetPairsService
    {
        Task<IReadOnlyList<AssetPair>> GetAllAsync();

        Task<IReadOnlyList<AssetPair>> GetAllAsync(IEnumerable<string> brokerIds);

        Task<IReadOnlyList<AssetPair>> GetAllAsync(string brokerId);

        Task<AssetPair> GetBySymbolAsync(string brokerId, string symbol);

        Task<IReadOnlyList<AssetPair>> GetAllAsync(
            string brokerId, string symbol, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50);

        Task<AssetPair> AddAsync(string brokerId, string symbol, string baseAsset, string quotingAsset,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled);

        Task<AssetPair> UpdateAsync(string brokerId, string symbol,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled);

        Task<bool> DeleteAsync(string brokerId, string symbol);
    }
}
