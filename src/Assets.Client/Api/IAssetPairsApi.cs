using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Client.Models.AssetPairs;

namespace Assets.Client.Api
{
    /// <summary>
    /// Provides methods for work with asset pairs API.
    /// </summary>
    public interface IAssetPairsApi
    {
        /// <summary>
        /// Returns all asset pairs.
        /// </summary>
        Task<IReadOnlyList<AssetPairModel>> GetAllAsync(string brokerId);

        /// <summary>
        /// Returns an asset pair by identifier.
        /// </summary>
        Task<AssetPairModel> GetByIdAsync(long id, string brokerId);

        /// <summary>
        /// Creates asset pair.
        /// </summary>
        Task<AssetPairModel> AddAsync(AssetPairEditModel model);

        /// <summary>
        /// Updates asset pair.
        /// </summary>
        Task UpdateAsync(AssetPairEditModel model);

        /// <summary>
        /// Deletes asset pair by identifier.
        /// </summary>
        Task DeleteAsync(long id, string brokerId);
    }
}
