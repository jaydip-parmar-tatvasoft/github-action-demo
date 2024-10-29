using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitHubActionDemo.Migrations
{
    /// <inheritdoc />
    public partial class RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "refresh_token",
                schema: "public",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "refresh_token_expireon_utc",
                schema: "public",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refresh_token",
                schema: "public",
                table: "users");

            migrationBuilder.DropColumn(
                name: "refresh_token_expireon_utc",
                schema: "public",
                table: "users");
        }
    }
}
