using System;

namespace Assets.Domain.Entities
{
    /// <summary>
    /// Represents an asset pair.
    /// </summary>
    public class AssetPair
    {
        /// <summary>
        /// The unique identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Broker identifier.
        /// </summary>
        public string BrokerId { get; set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The base asset identifier.
        /// </summary>
        public long BaseAssetId { get; set; }

        /// <summary>
        /// The base asset symbol.
        /// </summary>
        public string BaseAsset { get; set; }

        /// <summary>
        /// The quoting asset identifier.
        /// </summary>
        public long QuotingAssetId { get; set; }

        /// <summary>
        /// The base asset symbol.
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
        /// The maximum allowed volume of <see cref="BaseAssetId"/>.
        /// </summary>
        public decimal MaxVolume { get; set; }

        /// <summary>
        /// The maximum allowed volume of <see cref="QuotingAssetId"/>.
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
