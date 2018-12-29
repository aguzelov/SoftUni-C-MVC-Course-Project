using Microsoft.EntityFrameworkCore.Migrations;

namespace Dialog.Data.Migrations
{
    public partial class AddNameToChatEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Chats",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Chats");
        }
    }
}
