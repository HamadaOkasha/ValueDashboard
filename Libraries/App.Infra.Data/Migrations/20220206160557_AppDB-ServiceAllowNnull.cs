using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Infra.Data.Migrations
{
    public partial class AppDBServiceAllowNnull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_ServicesType_ServiceTypeId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceTypeId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_ServicesType_ServiceTypeId",
                table: "Order",
                column: "ServiceTypeId",
                principalTable: "ServicesType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_ServicesType_ServiceTypeId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceTypeId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_ServicesType_ServiceTypeId",
                table: "Order",
                column: "ServiceTypeId",
                principalTable: "ServicesType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
