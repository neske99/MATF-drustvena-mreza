using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddChatUserHasNewMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "hasNewMessages",
                table: "ChatUser",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hasNewMessages",
                table: "ChatUser");
        }
    }
}
