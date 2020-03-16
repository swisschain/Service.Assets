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
        /// <returns>A collection of assets.</returns>
        Task<IReadOnlyList<AssetPairModel>> GetAllAsync();

        /// <summary>
        /// Returns an asset pair by identifier.
        /// </summary>
        /// <param name="assetPairId">The asset pair identifier.</param>
        /// <returns>The asset pair.</returns>
        Task<AssetPairModel> GetByIdAsync(string assetPairId);

        /// <summary>
        /// Creates asset pair.
        /// </summary>
        /// <param name="model">The asset pair.</param>
        /// <returns>Created asset pair.</returns>
        Task<AssetPairModel> AddAsync(AssetPairEditModel model);

        /// <summary>
        /// Updates asset pair.
        /// </summary>
        /// <param name="model">The asset pair.</param>
        Task UpdateAsync(AssetPairEditModel model);

        /// <summary>
        /// Deletes asset pair by identifier.
        /// </summary>
        /// <param name="assetPairId">The asset pair identifier.</param>
        Task DeleteAsync(string assetPairId);
    }
}
