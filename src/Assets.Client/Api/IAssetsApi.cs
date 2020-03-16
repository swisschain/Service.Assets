using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Client.Models.Assets;

namespace Assets.Client.Api
{
    /// <summary>
    /// Provides methods for work with assets API.
    /// </summary>
    public interface IAssetsApi
    {
        /// <summary>
        /// Returns all assets.
        /// </summary>
        /// <returns>A collection of assets.</returns>
        Task<IReadOnlyList<AssetModel>> GetAllAsync();

        /// <summary>
        /// Returns an asset by identifier.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <returns>The asset.</returns>
        Task<AssetModel> GetByIdAsync(string assetId);

        /// <summary>
        /// Creates asset.
        /// </summary>
        /// <param name="model">The asset.</param>
        /// <returns>Created asset.</returns>
        Task<AssetModel> AddAsync(AssetEditModel model);

        /// <summary>
        /// Updates asset.
        /// </summary>
        /// <param name="model">The asset.</param>
        Task UpdateAsync(AssetEditModel model);

        /// <summary>
        /// Deletes asset by identifier.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        Task DeleteAsync(string assetId);
    }
}
