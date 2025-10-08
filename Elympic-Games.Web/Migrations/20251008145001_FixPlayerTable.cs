using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elympic_Games.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixPlayerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Players",
                newName: "IsPlaying");

            migrationBuilder.RenameColumn(
                name: "FírstName",
                table: "Players",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPlaying",
                table: "Players",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Players",
                newName: "FírstName");
        }
    }
}
