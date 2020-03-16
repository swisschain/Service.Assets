using System;
using Assets.Client.Api;
using Assets.Client.Grpc;

namespace Assets.Client
{
    /// <inheritdoc /> 
    public class AssetsClient : IAssetsClient
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AssetsClient"/>.
        /// </summary>
        /// <param name="settings">The client settings.</param>
        public AssetsClient(AssetsClientSettings settings)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            Assets = new AssetsApi(settings.ServiceAddress);
            AssetPairs = new AssetPairsApi(settings.ServiceAddress);
        }

        /// <inheritdoc />
        public IAssetsApi Assets { get; }

        /// <inheritdoc />
        public IAssetPairsApi AssetPairs { get; }
    }
}
