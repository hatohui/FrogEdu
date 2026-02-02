using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.User.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                schema: "public",
                table: "Users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PasswordResetTokenExpiry",
                schema: "public",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordResetToken",
                schema: "public",
                table: "Users",
                column: "PasswordResetToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_PasswordResetToken",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordResetTokenExpiry",
                schema: "public",
                table: "Users");
        }
    }
}
