using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace alpr.api.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedBoundingBoxes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoundingBox_Height",
                table: "plate_sightings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BoundingBox_Width",
                table: "plate_sightings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BoundingBox_X",
                table: "plate_sightings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BoundingBox_Y",
                table: "plate_sightings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FrameUrl",
                table: "plate_sightings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VideoId1",
                table: "plate_sightings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_plate_sightings_VideoId1",
                table: "plate_sightings",
                column: "VideoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_plate_sightings_videos_VideoId1",
                table: "plate_sightings",
                column: "VideoId1",
                principalTable: "videos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_plate_sightings_videos_VideoId1",
                table: "plate_sightings");

            migrationBuilder.DropIndex(
                name: "IX_plate_sightings_VideoId1",
                table: "plate_sightings");

            migrationBuilder.DropColumn(
                name: "BoundingBox_Height",
                table: "plate_sightings");

            migrationBuilder.DropColumn(
                name: "BoundingBox_Width",
                table: "plate_sightings");

            migrationBuilder.DropColumn(
                name: "BoundingBox_X",
                table: "plate_sightings");

            migrationBuilder.DropColumn(
                name: "BoundingBox_Y",
                table: "plate_sightings");

            migrationBuilder.DropColumn(
                name: "FrameUrl",
                table: "plate_sightings");

            migrationBuilder.DropColumn(
                name: "VideoId1",
                table: "plate_sightings");
        }
    }
}
