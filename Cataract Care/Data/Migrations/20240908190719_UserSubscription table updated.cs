using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cataract_Care.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserSubscriptiontableupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "UserSubscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageName",
                table: "UserSubscriptions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PhotoLimit",
                table: "UserSubscriptions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "PackageName",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "PhotoLimit",
                table: "UserSubscriptions");
        }
    }
}
