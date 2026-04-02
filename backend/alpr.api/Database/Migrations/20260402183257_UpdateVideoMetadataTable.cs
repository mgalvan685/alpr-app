using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace alpr.api.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVideoMetadataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoMetadata_videos_VideoId",
                table: "VideoMetadata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoMetadata",
                table: "VideoMetadata");

            migrationBuilder.RenameTable(
                name: "VideoMetadata",
                newName: "video_metadata");

            migrationBuilder.RenameIndex(
                name: "IX_VideoMetadata_VideoId",
                table: "video_metadata",
                newName: "IX_video_metadata_VideoId");

            migrationBuilder.AlterColumn<string>(
                name: "ProcessingStatus",
                table: "videos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Processing");

            migrationBuilder.AddPrimaryKey(
                name: "PK_video_metadata",
                table: "video_metadata",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_video_metadata_videos_VideoId",
                table: "video_metadata",
                column: "VideoId",
                principalTable: "videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_video_metadata_videos_VideoId",
                table: "video_metadata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_video_metadata",
                table: "video_metadata");

            migrationBuilder.RenameTable(
                name: "video_metadata",
                newName: "VideoMetadata");

            migrationBuilder.RenameIndex(
                name: "IX_video_metadata_VideoId",
                table: "VideoMetadata",
                newName: "IX_VideoMetadata_VideoId");

            migrationBuilder.AlterColumn<string>(
                name: "ProcessingStatus",
                table: "videos",
                type: "text",
                nullable: false,
                defaultValue: "Processing",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoMetadata",
                table: "VideoMetadata",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoMetadata_videos_VideoId",
                table: "VideoMetadata",
                column: "VideoId",
                principalTable: "videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
