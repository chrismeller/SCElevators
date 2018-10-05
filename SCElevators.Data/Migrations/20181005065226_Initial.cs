using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SCElevators.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elevators",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false),
                    OwnerName = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    LocationAddress = table.Column<string>(nullable: true),
                    LocationCity = table.Column<string>(nullable: true),
                    LocationState = table.Column<string>(nullable: true),
                    LocationZip = table.Column<string>(nullable: true),
                    County = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    NextInspectionDue = table.Column<DateTime>(nullable: true),
                    NextInspectionDueOther = table.Column<string>(nullable: true),
                    MachineType = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    UnitType = table.Column<string>(nullable: true),
                    InsertedAt = table.Column<DateTimeOffset>(nullable: false),
                    LastSeen = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elevators", x => x.Number);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Elevators_County",
                table: "Elevators",
                column: "County");

            migrationBuilder.CreateIndex(
                name: "IX_Elevators_Number",
                table: "Elevators",
                column: "Number",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Elevators");
        }
    }
}
