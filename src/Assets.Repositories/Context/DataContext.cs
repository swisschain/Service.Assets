using Assets.Repositories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Assets.Repositories.Context
{
    public class DataContext : DbContext
    {
        private const string Schema = "assets";

        private string _connectionString;

        public DataContext()
        {
        }

        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        internal DbSet<AssetEntity> Assets { get; set; }

        internal DbSet<AssetPairEntity> AssetPairs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString == null)
            {
                System.Console.Write("Enter connection string: ");
                _connectionString = System.Console.ReadLine();
            }

            optionsBuilder.UseNpgsql(_connectionString,
                o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            SetupAssetPairs(modelBuilder);
        }

        private static void SetupAssetPairs(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetPairEntity>()
                .HasOne<AssetEntity>()
                .WithMany()
                .HasForeignKey(o => o.BaseAssetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AssetPairEntity>()
                .HasOne<AssetEntity>()
                .WithMany()
                .HasForeignKey(o => o.QuotingAssetId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
