using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cataract_Care.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserSubscriptiontableenddateremoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubscriptionEndDate",
                table: "UserSubscriptions");

            migrationBuilder.RenameColumn(
                name: "SubscriptionStartDate",
                table: "UserSubscriptions",
                newName: "SubscriptionDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubscriptionDate",
                table: "UserSubscriptions",
                newName: "SubscriptionStartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionEndDate",
                table: "UserSubscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
