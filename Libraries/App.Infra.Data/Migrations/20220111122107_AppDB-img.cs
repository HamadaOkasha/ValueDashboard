using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Infra.Data.Migrations
{
    public partial class AppDBimg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "ServicesType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MobileToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "ServicesType");

            migrationBuilder.DropColumn(
                name: "MobileToken",
                table: "AspNetUsers");
        }
    }
}
