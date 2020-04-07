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
