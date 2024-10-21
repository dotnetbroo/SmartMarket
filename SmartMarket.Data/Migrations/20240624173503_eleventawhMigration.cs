using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMarket.Data.Migrations
{
    /// <inheritdoc />
    public partial class eleventawhMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContrAgents_TolovUsulis_TolovUsuliID",
                table: "ContrAgents");

            migrationBuilder.AddForeignKey(
                name: "FK_ContrAgents_TolovUsulis_TolovUsuliID",
                table: "ContrAgents",
                column: "TolovUsuliID",
                principalTable: "TolovUsulis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContrAgents_TolovUsulis_TolovUsuliID",
                table: "ContrAgents");

            migrationBuilder.AddForeignKey(
                name: "FK_ContrAgents_TolovUsulis_TolovUsuliID",
                table: "ContrAgents",
                column: "TolovUsuliID",
                principalTable: "TolovUsulis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
