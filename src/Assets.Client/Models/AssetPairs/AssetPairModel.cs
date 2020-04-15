using System;
using Service.Assets.Contracts;

namespace Assets.Client.Models.AssetPairs
{
    /// <summary>
    /// Represents an asset pair.
    /// </summary>
    public class AssetPairModel
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AssetPairModel"/>.
        /// </summary>
        public AssetPairModel()
        {
        }

        internal AssetPairModel(AssetPair assetPair)
        {
            BrokerId = assetPair.BrokerId;
            Symbol = assetPair.Symbol;
            BaseAsset = assetPair.BaseAsset;
            QuotingAsset = assetPair.QuotingAsset;
            Accuracy = assetPair.Accuracy;
            MinVolume = decimal.Parse(assetPair.MinVolume);
            MaxVolume = decimal.Parse(assetPair.MaxVolume);
            MaxOppositeVolume = decimal.Parse(assetPair.MaxOppositeVolume);
            MarketOrderPriceThreshold = decimal.Parse(assetPair.MarketOrderPriceThreshold);
            IsDisabled = assetPair.IsDisabled;
            Created = assetPair.Created.ToDateTime();
            Modified = assetPair.Modified.ToDateTime();
        }

        /// <summary>
        /// Broker identifier.
        /// </summary>
        public string BrokerId { get; set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The base asset symbol.
        /// </summary>
        public string BaseAsset { get; set; }

        /// <summary>
        /// The quoting asset symbol.
        /// </summary>
        public string QuotingAsset { get; set; }

        /// <summary>
        /// The base asset accuracy.
        /// </summary>
        public int Accuracy { get; set; }

        /// <summary>
        /// The minimal allowed order volume.
        /// </summary>
        public decimal MinVolume { get; set; }

        /// <summary>
        /// The maximum allowed volume of <see cref="BaseAsset"/>.
        /// </summary>
        public decimal MaxVolume { get; set; }

        /// <summary>
        /// The maximum allowed volume of <see cref="QuotingAsset"/>.
        /// </summary>
        public decimal MaxOppositeVolume { get; set; }

        /// <summary>
        /// Maximum alowed slippage for market order, percents from middle price.
        /// </summary>
        public decimal MarketOrderPriceThreshold { get; set; }

        /// <summary>
        /// Indicates that the asset pair is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// The creation date and time.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The last update date and time.
        /// </summary>
        public DateTime Modified { get; set; }
    }
}
