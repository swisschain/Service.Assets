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
            Id = assetPair.Id;
            Name = assetPair.Name;
            BaseAssetId = assetPair.BaseAssetId;
            QuotingAssetId = assetPair.QuotingAssetId;
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
        /// The unique identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The human-readable name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The base asset identifier.
        /// </summary>
        public string BaseAssetId { get; set; }

        /// <summary>
        /// The quoting asset identifier.
        /// </summary>
        public string QuotingAssetId { get; set; }

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
        /// The market order price threshold.
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
