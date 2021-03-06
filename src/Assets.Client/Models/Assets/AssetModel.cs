﻿using System;
using Service.Assets.Contracts;

namespace Assets.Client.Models.Assets
{
    /// <summary>
    /// Represents an asset.
    /// </summary>
    public class AssetModel
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AssetModel"/>.
        /// </summary>
        public AssetModel()
        {
        }

        internal AssetModel(Asset asset)
        {
            BrokerId = BrokerId;
            Symbol = asset.Symbol;
            Description = asset.Description;
            Accuracy = asset.Accuracy;
            IsDisabled = asset.IsDisabled;
            Created = asset.Created.ToDateTime();
            Modified = asset.Modified.ToDateTime();
        }

        /// <summary>
        /// Broker identifier.
        /// </summary>
        public string BrokerId { get; set; }

        /// <summary>
        /// The human-readable name.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The additional information.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The number of decimal places.
        /// </summary>
        public int Accuracy { get; set; }

        /// <summary>
        /// Indicates that the asset is disabled.
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
