using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cataract_Care.Data.Migrations
{
    /// <inheritdoc />
    public partial class IsCataractfieldtypeupdatedtostringindiagonsistable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IsCataract",
                table: "Diagnosis",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsCataract",
                table: "Diagnosis",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
