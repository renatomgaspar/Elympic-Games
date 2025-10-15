using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elympic_Games.Web.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGametypeFromMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_GameTypes_GameTypeId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_GameTypeId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "GameTypeId",
                table: "Matches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameTypeId",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_GameTypeId",
                table: "Matches",
                column: "GameTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_GameTypes_GameTypeId",
                table: "Matches",
                column: "GameTypeId",
                principalTable: "GameTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
