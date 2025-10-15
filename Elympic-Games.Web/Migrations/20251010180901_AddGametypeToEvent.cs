using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elympic_Games.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddGametypeToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameTypeId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Events_GameTypeId",
                table: "Events",
                column: "GameTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_GameTypes_GameTypeId",
                table: "Events",
                column: "GameTypeId",
                principalTable: "GameTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_GameTypes_GameTypeId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_GameTypeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "GameTypeId",
                table: "Events");
        }
    }
}
