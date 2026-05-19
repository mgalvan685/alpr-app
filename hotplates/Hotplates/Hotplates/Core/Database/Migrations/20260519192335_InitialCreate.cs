using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotplates.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotplateEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Plate = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    State = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    LastSeenTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSeenLatitude = table.Column<double>(type: "double precision", nullable: true),
                    LastSeenLongitude = table.Column<double>(type: "double precision", nullable: true),
                    LastSeenDeviceId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotplateEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotplateSightings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HotplateEntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Plate = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    State = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Confidence = table.Column<float>(type: "real", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Source = table.Column<string>(type: "text", nullable: false),
                    DeviceId = table.Column<string>(type: "text", nullable: true),
                    RawMetadataJson = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotplateSightings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotplateSightings_HotplateEntries_HotplateEntryId",
                        column: x => x.HotplateEntryId,
                        principalTable: "HotplateEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotplateEntries_Plate_State",
                table: "HotplateEntries",
                columns: new[] { "Plate", "State" });

            migrationBuilder.CreateIndex(
                name: "IX_HotplateSightings_HotplateEntryId",
                table: "HotplateSightings",
                column: "HotplateEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_HotplateSightings_Latitude_Longitude",
                table: "HotplateSightings",
                columns: new[] { "Latitude", "Longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_HotplateSightings_Timestamp",
                table: "HotplateSightings",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotplateSightings");

            migrationBuilder.DropTable(
                name: "HotplateEntries");
        }
    }
}
