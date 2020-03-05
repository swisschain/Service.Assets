using Assets.Client.Common;
using Service.Assets.Contracts;

namespace Assets.Client
{
    public class AssetsClient : BaseGrpcClient, IAssetsClient
    {
        public AssetsClient(string serverGrpcUrl)
            : base(serverGrpcUrl)
        {
            Monitoring = new Monitoring.MonitoringClient(Channel);
            Assets = new Service.Assets.Contracts.Assets.AssetsClient(Channel);
            AssetPairs = new AssetPairs.AssetPairsClient(Channel);
        }

        public Monitoring.MonitoringClient Monitoring { get; }

        public Service.Assets.Contracts.Assets.AssetsClient Assets { get; }

        public AssetPairs.AssetPairsClient AssetPairs { get; }
    }
}
