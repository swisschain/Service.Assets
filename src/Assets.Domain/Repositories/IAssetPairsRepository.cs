using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Repositories
{
    public interface IAssetPairsRepository
    {
        Task<IReadOnlyList<AssetPair>> GetAllAsync();

        Task<IReadOnlyList<AssetPair>> GetAllAsync(string name, string assetPairId, string baseAssetId, string quoteAssetId,
            bool isDisabled = false,  ListSortDirection sortOrder = ListSortDirection.Ascending, string cursor = null, int limit = 50);

        Task<AssetPair> GetByIdAsync(string assetPairId);

        Task InsertAsync(AssetPair assetPair);

        Task UpdateAsync(AssetPair assetPair);

        Task DeleteAsync(string assetPairId);
    }
}
