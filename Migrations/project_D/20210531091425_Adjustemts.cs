using Microsoft.EntityFrameworkCore.Migrations;

namespace Project_D.Migrations.project_D
{
    public partial class Adjustemts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GasGenerated",
                table: "Data",
                newName: "GasAdjustment");

            migrationBuilder.AddColumn<double>(
                name: "EnergyAdjustment",
                table: "Data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergyAdjustment",
                table: "Data");

            migrationBuilder.RenameColumn(
                name: "GasAdjustment",
                table: "Data",
                newName: "GasGenerated");
        }
    }
}
