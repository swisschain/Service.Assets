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
        Task<IReadOnlyList<AssetPairModel>> GetAllAsync();

        /// <summary>
        /// Returns all asset pairs by list of brokers ids.
        /// </summary>
        Task<IReadOnlyList<AssetPairModel>> GetAllByBrokerIds(IEnumerable<string> brokerIds);

        /// <summary>
        /// Returns all asset pairs by a broker id.
        /// </summary>
        Task<IReadOnlyList<AssetPairModel>> GetAllByBrokerId(string brokerId);

        /// <summary>
        /// Returns an asset pair by symbol.
        /// </summary>
        Task<AssetPairModel> GetBySymbolAsync(string brokerId, string symbol);

        /// <summary>
        /// Creates asset pair.
        /// </summary>
        Task<AssetPairModel> AddAsync(AssetPairEditModel model);

        /// <summary>
        /// Updates asset pair.
        /// </summary>
        Task UpdateAsync(AssetPairEditModel model);

        /// <summary>
        /// Deletes asset pair by symbol.
        /// </summary>
        Task DeleteAsync(string brokerId, string symbol);
    }
}
