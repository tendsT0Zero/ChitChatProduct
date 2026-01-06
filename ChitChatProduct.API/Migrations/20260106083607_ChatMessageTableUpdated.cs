using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChitChatProduct.API.Migrations
{
    /// <inheritdoc />
    public partial class ChatMessageTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "ChatMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                table: "ChatMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "SentAt",
                table: "ChatMessages");
        }
    }
}
