using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Services
{
    public interface IAssetPairsService
    {
        Task<IReadOnlyList<AssetPair>> GetAllAsync();

        Task<AssetPair> GetByIdAsync(string assetPairId);

        Task<IReadOnlyList<AssetPair>> GetAllAsync(string name, string assetId, string baseAssetId, string quoteAssetId,
            bool isDisabled = false, ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50);

        Task<AssetPair> AddAsync(string brokerId, string name, string baseAssetId, string quotingAssetId,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled);

        Task<bool> UpdateAsync(string assetPairId, string name, string baseAssetId, string quotingAssetId,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled);

        Task<bool> DeleteAsync(string assetPairId);
    }
}
