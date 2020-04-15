using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Service.Assets.Contracts;

namespace TestClient
{
    public class TestData
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private const string BrokerId = "BrokerIdentifier";

        private static Random _random = new Random();
        
        public TestData()
        {
            Assets = new[]
            {
                new Asset
                {
                    BrokerId = BrokerId,
                    Symbol =  RandomString(3),
                    Accuracy = 3,
                    Description = RandomString(100)
                },
                new Asset
                {
                    BrokerId = BrokerId,
                    Symbol = RandomString(3),
                    Accuracy = 3,
                    Description = RandomString(100)
                }
            };

            AssetPairs = new[]
            {
                new AssetPair
                {
                    BrokerId = BrokerId,
                    Symbol =  RandomString(6),
                    Accuracy = 3,
                    BaseAsset = Assets.First().Symbol,
                    QuotingAsset = Assets.Last().Symbol,
                    MinVolume = .00001m.ToString(CultureInfo.InvariantCulture),
                    MaxVolume = 999999.99999m.ToString(CultureInfo.InvariantCulture),
                    MaxOppositeVolume = 888888.99999m.ToString(CultureInfo.InvariantCulture),
                    MarketOrderPriceThreshold = 777777.99999m.ToString(CultureInfo.InvariantCulture)
                }
            };
        }

        public IReadOnlyList<Asset> Assets { get; }

        public IReadOnlyList<AssetPair> AssetPairs { get; }

        public static string RandomString(int length)
        {
            return new string(Enumerable.Repeat(Chars, length)
                .Select(s => s[_random.Next(s.Length)])
                .ToArray());
        }
    }
}
