using Microsoft.EntityFrameworkCore.Migrations;

namespace Project_D.Migrations.project_D
{
    public partial class GenAdjustment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EnergyGenAdjustment",
                table: "Data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergyGenAdjustment",
                table: "Data");
        }
    }
}
