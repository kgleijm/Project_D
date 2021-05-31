using Microsoft.EntityFrameworkCore.Migrations;

namespace Project_D.Migrations.project_D
{
    public partial class InitialAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergySaving",
                table: "Data");

            migrationBuilder.DropColumn(
                name: "GasSaving",
                table: "Data");

            migrationBuilder.AlterColumn<int>(
                name: "IsAdmin",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "GasConsumption",
                table: "Data",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<double>(
                name: "EnergyConsumption",
                table: "Data",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<double>(
                name: "EnergyGenerated",
                table: "Data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GasGenerated",
                table: "Data",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergyGenerated",
                table: "Data");

            migrationBuilder.DropColumn(
                name: "GasGenerated",
                table: "Data");

            migrationBuilder.AlterColumn<string>(
                name: "IsAdmin",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "GasConsumption",
                table: "Data",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<float>(
                name: "EnergyConsumption",
                table: "Data",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<float>(
                name: "EnergySaving",
                table: "Data",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "GasSaving",
                table: "Data",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
