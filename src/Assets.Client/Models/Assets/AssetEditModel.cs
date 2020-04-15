namespace Assets.Client.Models.Assets
{
    /// <summary>
    /// Represents an asset create/update information.
    /// </summary>
    public class AssetEditModel
    {
        /// <summary>
        /// The unique identifier.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The broker identifier.
        /// </summary>
        public string BrokerId { get; set; }

        /// <summary>
        /// Symbol.
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
    }
}
