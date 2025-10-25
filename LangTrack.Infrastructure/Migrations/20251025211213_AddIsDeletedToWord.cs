using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LangTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToWord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Words",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Words");
        }
    }
}
