using System;
using System.Collections.Generic;
using Assets.Domain.Entities;
using MyNoSqlServer.Abstractions;

namespace Assets.Domain.MyNoSql
{
    public class AssetsEntity : IMyNoSqlDbEntity
    {
        public string BrokerId { get; set; }

        public List<Asset> Assets { get; set; }


        public static string GetPartitionKey(string brokerId) => brokerId;

        public static string GetRowKey() => "assets";

        public static AssetsEntity Generate(string brokerId)
        {
            return new AssetsEntity()
            {
                PartitionKey = GetPartitionKey(brokerId),
                RowKey = GetRowKey(),
                BrokerId = brokerId,
                Assets = new List<Asset>()
            };
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string TimeStamp { get; set; }
        public DateTime? Expires { get; set; }
    }
}
