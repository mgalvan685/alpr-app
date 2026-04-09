using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace alpr.api.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameStateToIssueState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "plate_summaries",
                newName: "IssueState");

            migrationBuilder.AddColumn<string>(
                name: "IssueState",
                table: "plate_sightings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueState",
                table: "plate_sightings");

            migrationBuilder.RenameColumn(
                name: "IssueState",
                table: "plate_summaries",
                newName: "State");
        }
    }
}
