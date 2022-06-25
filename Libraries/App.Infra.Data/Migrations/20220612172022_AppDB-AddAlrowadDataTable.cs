using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.Infra.Data.Migrations
{
    public partial class AppDBAddAlrowadDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlrowadData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    ValueId = table.Column<int>(type: "int", nullable: false),
                    ResearchValueId = table.Column<int>(type: "int", nullable: false),
                    AlrowadVersionId = table.Column<int>(type: "int", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlrowadData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlrowadData_AlrowadVersion_AlrowadVersionId",
                        column: x => x.AlrowadVersionId,
                        principalTable: "AlrowadVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlrowadData_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlrowadData_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlrowadData_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlrowadData_ResearchValue_ResearchValueId",
                        column: x => x.ResearchValueId,
                        principalTable: "ResearchValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlrowadData_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlrowadData_Value_ValueId",
                        column: x => x.ValueId,
                        principalTable: "Value",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlrowadData_AlrowadVersionId",
                table: "AlrowadData",
                column: "AlrowadVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AlrowadData_CategoryId",
                table: "AlrowadData",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AlrowadData_CountryId",
                table: "AlrowadData",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AlrowadData_OrganizationId",
                table: "AlrowadData",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AlrowadData_ResearchValueId",
                table: "AlrowadData",
                column: "ResearchValueId");

            migrationBuilder.CreateIndex(
                name: "IX_AlrowadData_SectorId",
                table: "AlrowadData",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_AlrowadData_ValueId",
                table: "AlrowadData",
                column: "ValueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlrowadData");
        }
    }
}
