using Assets.Client.Api;

namespace Assets.Client
{
    /// <summary>
    /// Assets service client.
    /// </summary>
    public interface IAssetsClient
    {
        /// <summary>
        /// Assets API.
        /// </summary>
        IAssetsApi Assets { get; }

        /// <summary>
        /// Asset pairs API.
        /// </summary>
        IAssetPairsApi AssetPairs { get; }
    }
}
