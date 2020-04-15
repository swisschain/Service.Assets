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

        Task<AssetPair> GetByIdAsync(long id, string brokerId);

        Task<IReadOnlyList<AssetPair>> GetAllAsync(
            string brokerId, string symbol, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, long cursor = default, int limit = 50);

        Task<AssetPair> AddAsync(string brokerId, string symbol, long baseAssetId, long quotingAssetId,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled);

        Task<AssetPair> UpdateAsync(long id, string brokerId, string symbol, long baseAssetId, long quotingAssetId,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled);

        Task<bool> DeleteAsync(long id, string brokerId);
    }
}
