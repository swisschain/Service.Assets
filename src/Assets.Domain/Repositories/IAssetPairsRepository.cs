using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Repositories
{
    public interface IAssetPairsRepository
    {
        Task<IReadOnlyList<AssetPair>> GetAllAsync();

        Task<IReadOnlyList<AssetPair>> GetAllAsync(IEnumerable<string> brokerIds);

        Task<IReadOnlyList<AssetPair>> GetAllAsync(string brokerId);

        Task<IReadOnlyList<AssetPair>> GetAllAsync(
            string brokerId, string symbol, long baseAssetId, long quoteAssetId, bool? isDisabled,
            ListSortDirection sortOrder = ListSortDirection.Ascending, long cursor = default, int limit = 50);

        Task<AssetPair> GetByIdAsync(long id, string brokerId);

        Task<AssetPair> InsertAsync(AssetPair assetPair);

        Task<AssetPair> UpdateAsync(AssetPair assetPair);

        Task DeleteAsync(long id, string brokerId);
    }
}
