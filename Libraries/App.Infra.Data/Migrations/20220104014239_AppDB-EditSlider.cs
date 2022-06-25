using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Infra.Data.Migrations
{
    public partial class AppDBEditSlider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slider_SliderType_SliderTypeId",
                table: "Slider");

            migrationBuilder.AlterColumn<int>(
                name: "SliderTypeId",
                table: "Slider",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Slider_SliderType_SliderTypeId",
                table: "Slider",
                column: "SliderTypeId",
                principalTable: "SliderType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slider_SliderType_SliderTypeId",
                table: "Slider");

            migrationBuilder.AlterColumn<int>(
                name: "SliderTypeId",
                table: "Slider",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Slider_SliderType_SliderTypeId",
                table: "Slider",
                column: "SliderTypeId",
                principalTable: "SliderType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
