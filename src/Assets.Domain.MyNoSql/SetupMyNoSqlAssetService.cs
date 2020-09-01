using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;

namespace Assets.Domain.MyNoSql
{
    public static class SetupMyNoSqlAssetService
    {
        public static string AssetsTableName = "assetservice-assets";
        public static string AssetPairsTableName = "assetservice-asseppairs";

        public static IMyNoSqlServerDataReader<AssetsEntity> CreateAssetDataReader(MyNoSqlTcpClient client)
        {
            return new MyNoSqlReadRepository<AssetsEntity>(client, AssetsTableName);
        }

        public static IMyNoSqlServerDataReader<AssetPairsEntity> CreateAssetPairDataReader(MyNoSqlTcpClient client)
        {
            return new MyNoSqlReadRepository<AssetPairsEntity>(client, AssetPairsTableName);
        }
    }
}
