using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.User.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                schema: "public",
                table: "Users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationTokenExpiry",
                schema: "public",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailVerificationToken",
                schema: "public",
                table: "Users",
                column: "EmailVerificationToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_EmailVerificationToken",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailVerificationTokenExpiry",
                schema: "public",
                table: "Users");
        }
    }
}
