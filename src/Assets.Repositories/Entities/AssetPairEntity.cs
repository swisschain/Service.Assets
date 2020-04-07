using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assets.Repositories.Entities
{
    [Table("asset_pairs")]
    public class AssetPairEntity
    {
        [Key]
        [Column("id", TypeName = "varchar(36)")]
        public string Id { get; set; }

        [Required]
        [Column("broker_id", TypeName = "varchar(36)")]
        public string BrokerId { get; set; }

        [Required]
        [Column("name", TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column("base_asset_id", TypeName = "varchar(36)")]
        public string BaseAssetId { get; set; }

        [Required]
        [Column("quoting_asset_id", TypeName = "varchar(36)")]
        public string QuotingAssetId { get; set; }

        [Column("accuracy")]
        public int Accuracy { get; set; }

        [Column("min_volume")]
        public decimal MinVolume { get; set; }

        [Column("max_volume")]
        public decimal MaxVolume { get; set; }

        [Column("max_opposite_volume")]
        public decimal MaxOppositeVolume { get; set; }

        [Column("market_order_price_threshold")]
        public decimal MarketOrderPriceThreshold { get; set; }

        [Column("is_disabled")]
        public bool IsDisabled { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("modified")]
        public DateTime Modified { get; set; }
    }
}
