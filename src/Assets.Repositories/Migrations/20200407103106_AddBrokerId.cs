using Microsoft.EntityFrameworkCore.Migrations;

namespace Assets.Repositories.Migrations
{
    public partial class AddBrokerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "broker_id",
                schema: "assets",
                table: "assets",
                type: "varchar(36)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "broker_id",
                schema: "assets",
                table: "asset_pairs",
                type: "varchar(36)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_asset_pairs_broker_id",
                schema: "assets",
                table: "asset_pairs",
                column: "broker_id");

            migrationBuilder.CreateIndex(
                name: "IX_assets_broker_id",
                schema: "assets",
                table: "assets",
                column: "broker_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "broker_id",
                schema: "assets",
                table: "assets");

            migrationBuilder.DropColumn(
                name: "broker_id",
                schema: "assets",
                table: "asset_pairs");
        }
    }
}
