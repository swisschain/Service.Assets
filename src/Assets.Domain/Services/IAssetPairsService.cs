using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Services
{
    public interface IAssetPairsService
    {
        Task<IReadOnlyList<AssetPair>> GetAllAsync();

        Task<AssetPair> GetByIdAsync(string assetPairId);

        Task<AssetPair> AddAsync(string assetPairId, string name, string baseAssetId, string quotingAssetId,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled);

        Task UpdateAsync(string assetPairId, string name, string baseAssetId, string quotingAssetId,
            int accuracy, decimal minVolume, decimal maxVolume, decimal maxOppositeVolume,
            decimal marketOrderPriceThreshold, bool isDisabled);

        Task DeleteAsync(string assetPairId);
    }
}
