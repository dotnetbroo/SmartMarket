using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class eleventhMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "TolovUsulis",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "TolovUsuliID",
                table: "ContrAgents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ProductStory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PCode = table.Column<string>(type: "text", nullable: false),
                    BarCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CamePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    OlchovTuri = table.Column<int>(type: "integer", nullable: false),
                    SalePrice = table.Column<decimal>(type: "numeric", nullable: true),
                    PercentageOfPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Action = table.Column<bool>(type: "boolean", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: false),
                    ContrAgentId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductStory_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStory_ContrAgents_ContrAgentId",
                        column: x => x.ContrAgentId,
                        principalTable: "ContrAgents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContrAgents_TolovUsuliID",
                table: "ContrAgents",
                column: "TolovUsuliID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStory_CategoryId",
                table: "ProductStory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStory_ContrAgentId",
                table: "ProductStory",
                column: "ContrAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStory_UserId",
                table: "ProductStory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContrAgents_TolovUsulis_TolovUsuliID",
                table: "ContrAgents",
                column: "TolovUsuliID",
                principalTable: "TolovUsulis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContrAgents_TolovUsulis_TolovUsuliID",
                table: "ContrAgents");

            migrationBuilder.DropTable(
                name: "ProductStory");

            migrationBuilder.DropIndex(
                name: "IX_ContrAgents_TolovUsuliID",
                table: "ContrAgents");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TolovUsulis");

            migrationBuilder.DropColumn(
                name: "TolovUsuliID",
                table: "ContrAgents");
        }
    }
}
