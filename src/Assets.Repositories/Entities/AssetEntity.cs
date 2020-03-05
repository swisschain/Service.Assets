using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assets.Repositories.Entities
{
    [Table("assets")]
    public class AssetEntity
    {
        [Key]
        [Column("id", TypeName = "varchar(36)")]
        public string Id { get; set; }

        [Required]
        [Column("name", TypeName = "varchar(100)")]
        public string Name { get; set; }

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
