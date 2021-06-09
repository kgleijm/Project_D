using Microsoft.EntityFrameworkCore.Migrations;

namespace Project_D.Migrations.project_D
{
    public partial class Adjustemts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "GasAdjustment",
                table: "Data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
