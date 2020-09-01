using System;
using System.Collections.Generic;
using Assets.Domain.Entities;
using MyNoSqlServer.Abstractions;

namespace Assets.Domain.MyNoSql
{
    public class AssetPairsEntity : IMyNoSqlDbEntity
    {
        public string BrokerId { get; set; }

        public List<AssetPair> AssetPairs { get; set; }

        public static string GetPartitionKey(string brokerId) => brokerId;

        public static string GetRowKey() => "asset-pairs";

        public static AssetPairsEntity Generate(string brokerId)
        {
            return new AssetPairsEntity()
            {
                PartitionKey = GetPartitionKey(brokerId),
                RowKey = GetRowKey(),
                BrokerId = brokerId,
                AssetPairs = new List<AssetPair>()
            };
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string TimeStamp { get; set; }
        public DateTime? Expires { get; set; }
    }
}
