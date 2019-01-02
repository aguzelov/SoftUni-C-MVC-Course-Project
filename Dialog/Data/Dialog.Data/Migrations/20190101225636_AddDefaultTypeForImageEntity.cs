using Microsoft.EntityFrameworkCore.Migrations;

namespace Dialog.Data.Migrations
{
    public partial class AddDefaultTypeForImageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultType",
                table: "Images",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultType",
                table: "Images");
        }
    }
}