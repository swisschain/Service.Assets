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
        Task<IReadOnlyList<AssetModel>> GetAllAsync();

        /// <summary>
        /// Returns all assets by list of brokers ids.
        /// </summary>
        Task<IReadOnlyList<AssetModel>> GetAllByBrokerIds(IEnumerable<string> brokerIds);

        /// <summary>
        /// Returns all assets by a broker id.
        /// </summary>
        Task<IReadOnlyList<AssetModel>> GetAllByBrokerId(string brokerId);

        /// <summary>
        /// Returns an asset by identifier.
        /// </summary>
        Task<AssetModel> GetByIdAsync(long id, string brokerId);

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
        Task DeleteAsync(long id, string brokerId);
    }
}
