using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrogEdu.Subscription.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedProSubscriptionTier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "SubscriptionTiers",
                columns: new[] { "Id", "Description", "DurationInDays", "ImageUrl", "IsActive", "Name", "TargetRole", "Price", "Currency" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "Unlock AI-powered question generation, advanced analytics, and unlimited exam creation.", 30, null, true, "Pro", "Teacher", 99000m, "VND" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "SubscriptionTiers",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));
        }
    }
}
