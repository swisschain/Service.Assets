using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;

namespace Assets.Domain.MyNoSql
{
    public static class SetupMyNoSqlAssetService
    {
        public static string AssetServiceTableName = "assetservice";

        public static IMyNoSqlServerDataReader<AssetsEntity> CreateAssetDataReader(MyNoSqlTcpClient client)
        {
            return new MyNoSqlReadRepository<AssetsEntity>(client, AssetServiceTableName);
        }

        public static IMyNoSqlServerDataReader<AssetPairsEntity> CreateAssetPairDataReader(MyNoSqlTcpClient client)
        {
            return new MyNoSqlReadRepository<AssetPairsEntity>(client, AssetServiceTableName);
        }
    }
}
