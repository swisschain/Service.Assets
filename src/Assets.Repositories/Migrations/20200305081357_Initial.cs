using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Assets.Repositories.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "assets");

            migrationBuilder.CreateTable(
                name: "assets",
                schema: "assets",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    description = table.Column<string>(type: "varchar(500)", nullable: true),
                    accuracy = table.Column<int>(nullable: false),
                    is_disabled = table.Column<bool>(nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asset_pairs",
                schema: "assets",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(36)", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    base_asset_id = table.Column<string>(type: "varchar(36)", nullable: false),
                    quoting_asset_id = table.Column<string>(type: "varchar(36)", nullable: false),
                    accuracy = table.Column<int>(nullable: false),
                    min_volume = table.Column<decimal>(nullable: false),
                    max_volume = table.Column<decimal>(nullable: false),
                    max_opposite_volume = table.Column<decimal>(nullable: false),
                    market_order_price_threshold = table.Column<decimal>(nullable: false),
                    is_disabled = table.Column<bool>(nullable: false),
                    created = table.Column<DateTime>(nullable: false),
                    modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_pairs", x => x.id);
                    table.ForeignKey(
                        name: "FK_asset_pairs_assets_base_asset_id",
                        column: x => x.base_asset_id,
                        principalSchema: "assets",
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_asset_pairs_assets_quoting_asset_id",
                        column: x => x.quoting_asset_id,
                        principalSchema: "assets",
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_asset_pairs_base_asset_id",
                schema: "assets",
                table: "asset_pairs",
                column: "base_asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_asset_pairs_quoting_asset_id",
                schema: "assets",
                table: "asset_pairs",
                column: "quoting_asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_asset_pairs_is_disabled",
                schema: "assets",
                table: "asset_pairs",
                column: "is_disabled");

            migrationBuilder.CreateIndex(
                name: "IX_assets_is_disabled",
                schema: "assets",
                table: "assets",
                column: "is_disabled");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asset_pairs",
                schema: "assets");

            migrationBuilder.DropTable(
                name: "assets",
                schema: "assets");
        }
    }
}
