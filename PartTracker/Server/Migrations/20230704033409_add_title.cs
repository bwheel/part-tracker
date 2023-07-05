using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PartTracker.Server.Migrations
{
    /// <inheritdoc />
    public partial class add_title : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PartName",
                table: "Parts",
                newName: "Title");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Parts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Parts",
                newName: "PartName");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Parts",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
