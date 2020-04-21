namespace Assets.WebApi.Models.AssetPairs
{
    /// <summary>
    /// Represents an asset pair create/update information.
    /// </summary>
    public class AssetPairUpdate
    {
        /// <summary>
        /// Symbol.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The base asset accuracy.
        /// </summary>
        public int Accuracy { get; set; }

        /// <summary>
        /// The minimal allowed order volume.
        /// </summary>
        public decimal MinVolume { get; set; }

        /// <summary>
        /// The maximum allowed volume of the base asset.
        /// </summary>
        public decimal MaxVolume { get; set; }

        /// <summary>
        /// The maximum allowed volume of the quoting asset.
        /// </summary>
        public decimal MaxOppositeVolume { get; set; }

        /// <summary>
        /// The market order price threshold.
        /// </summary>
        public decimal MarketOrderPriceThreshold { get; set; }

        /// <summary>
        /// Indicates that the asset pair is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
