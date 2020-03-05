using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Domain.Entities;

namespace Assets.Domain.Repositories
{
    public interface IAssetPairsRepository
    {
        Task<IReadOnlyList<AssetPair>> GetAllAsync();

        Task<AssetPair> GetByIdAsync(string assetPairId);

        Task InsertAsync(AssetPair assetPair);

        Task UpdateAsync(AssetPair assetPair);

        Task DeleteAsync(string assetPairId);
    }
}
