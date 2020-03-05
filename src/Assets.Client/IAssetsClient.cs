using Service.Assets.Contracts;

namespace Assets.Client
{
    public interface IAssetsClient
    {
        Monitoring.MonitoringClient Monitoring { get; }

        Service.Assets.Contracts.Assets.AssetsClient Assets { get; }

        AssetPairs.AssetPairsClient AssetPairs { get; }
    }
}
