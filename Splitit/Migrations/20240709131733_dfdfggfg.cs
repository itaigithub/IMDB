using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Splitit.Migrations
{
    /// <inheritdoc />
    public partial class dfdfggfg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Actors");
        }
    }
}
