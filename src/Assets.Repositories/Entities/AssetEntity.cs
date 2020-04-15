using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assets.Repositories.Entities
{
    [Table("assets")]
    public class AssetEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("broker_id", TypeName = "varchar(36)")]
        public string BrokerId { get; set; }

        [Required]
        [Column("symbol", TypeName = "varchar(36)")]
        public string Symbol { get; set; }

        [Column("description", TypeName = "varchar(500)")]
        public string Description { get; set; }

        [Column("accuracy")]
        public int Accuracy { get; set; }

        [Column("is_disabled")]
        public bool IsDisabled { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("modified")]
        public DateTime Modified { get; set; }
    }
}
