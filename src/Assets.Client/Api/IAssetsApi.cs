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
        /// Returns an asset by identifier.
        /// </summary>
        Task<AssetModel> GetByIdAsync(string brokerId, string id);

        /// <summary>
        /// Creates asset.
        /// </summary>
        Task<AssetModel> AddAsync(AssetEditModel model);

        /// <summary>
        /// Updates asset.
        /// </summary>
        Task UpdateAsync(AssetEditModel model);

        /// <summary>
        /// Deletes asset by identifier.
        /// </summary>
        Task DeleteAsync(string brokerId, string id);
    }
}
