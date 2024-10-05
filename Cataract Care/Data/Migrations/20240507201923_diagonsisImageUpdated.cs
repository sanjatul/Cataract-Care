using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cataract_Care.Data.Migrations
{
    /// <inheritdoc />
    public partial class diagonsisImageUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Diagnosis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Diagnosis");
        }
    }
}
