using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Class.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReinviteFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReinvitedAt",
                schema: "public",
                table: "ClassEnrollments",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReinvitedAt",
                schema: "public",
                table: "ClassEnrollments");
        }
    }
}
