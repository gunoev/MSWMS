using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MSWMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShipmentEventEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ShipmentEvents",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShipmentId1",
                table: "ShipmentEvents",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentEvents_OrderId",
                table: "ShipmentEvents",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentEvents_ShipmentId1",
                table: "ShipmentEvents",
                column: "ShipmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentEvents_Orders_OrderId",
                table: "ShipmentEvents",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShipmentEvents_Shipments_ShipmentId1",
                table: "ShipmentEvents",
                column: "ShipmentId1",
                principalTable: "Shipments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentEvents_Orders_OrderId",
                table: "ShipmentEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_ShipmentEvents_Shipments_ShipmentId1",
                table: "ShipmentEvents");

            migrationBuilder.DropIndex(
                name: "IX_ShipmentEvents_OrderId",
                table: "ShipmentEvents");

            migrationBuilder.DropIndex(
                name: "IX_ShipmentEvents_ShipmentId1",
                table: "ShipmentEvents");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ShipmentEvents");

            migrationBuilder.DropColumn(
                name: "ShipmentId1",
                table: "ShipmentEvents");
        }
    }
}
