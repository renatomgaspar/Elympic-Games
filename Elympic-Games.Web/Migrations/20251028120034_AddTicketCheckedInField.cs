using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elympic_Games.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketCheckedInField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketOrderDetails_TicketOrders_TicketOrderId",
                table: "TicketOrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "TicketOrderId",
                table: "TicketOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isCheckedIn",
                table: "TicketOrderDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketOrderDetails_TicketOrders_TicketOrderId",
                table: "TicketOrderDetails",
                column: "TicketOrderId",
                principalTable: "TicketOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketOrderDetails_TicketOrders_TicketOrderId",
                table: "TicketOrderDetails");

            migrationBuilder.DropColumn(
                name: "isCheckedIn",
                table: "TicketOrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "TicketOrderId",
                table: "TicketOrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketOrderDetails_TicketOrders_TicketOrderId",
                table: "TicketOrderDetails",
                column: "TicketOrderId",
                principalTable: "TicketOrders",
                principalColumn: "Id");
        }
    }
}
