using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotplates.Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddHotplateStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "HotplateEntries",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "HotplateEntries");
        }
    }
}
