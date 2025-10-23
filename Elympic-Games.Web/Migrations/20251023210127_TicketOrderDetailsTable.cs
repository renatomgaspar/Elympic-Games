using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Elympic_Games.Web.Migrations
{
    /// <inheritdoc />
    public partial class TicketOrderDetailsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketOrderDetail_TicketOrders_TicketOrderId",
                table: "TicketOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketOrderDetail_Tickets_TicketId",
                table: "TicketOrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketOrderDetail",
                table: "TicketOrderDetail");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "TicketOrderDetail");

            migrationBuilder.RenameTable(
                name: "TicketOrderDetail",
                newName: "TicketOrderDetails");

            migrationBuilder.RenameIndex(
                name: "IX_TicketOrderDetail_TicketOrderId",
                table: "TicketOrderDetails",
                newName: "IX_TicketOrderDetails_TicketOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketOrderDetail_TicketId",
                table: "TicketOrderDetails",
                newName: "IX_TicketOrderDetails_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketOrderDetails",
                table: "TicketOrderDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketOrderDetails_TicketOrders_TicketOrderId",
                table: "TicketOrderDetails",
                column: "TicketOrderId",
                principalTable: "TicketOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketOrderDetails_Tickets_TicketId",
                table: "TicketOrderDetails",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketOrderDetails_TicketOrders_TicketOrderId",
                table: "TicketOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketOrderDetails_Tickets_TicketId",
                table: "TicketOrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketOrderDetails",
                table: "TicketOrderDetails");

            migrationBuilder.RenameTable(
                name: "TicketOrderDetails",
                newName: "TicketOrderDetail");

            migrationBuilder.RenameIndex(
                name: "IX_TicketOrderDetails_TicketOrderId",
                table: "TicketOrderDetail",
                newName: "IX_TicketOrderDetail_TicketOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketOrderDetails_TicketId",
                table: "TicketOrderDetail",
                newName: "IX_TicketOrderDetail_TicketId");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "TicketOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketOrderDetail",
                table: "TicketOrderDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketOrderDetail_TicketOrders_TicketOrderId",
                table: "TicketOrderDetail",
                column: "TicketOrderId",
                principalTable: "TicketOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketOrderDetail_Tickets_TicketId",
                table: "TicketOrderDetail",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
