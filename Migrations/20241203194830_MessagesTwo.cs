using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Aafeben.Migrations
{
    /// <inheritdoc />
    public partial class MessagesTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SendOn",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendOn",
                table: "Messages");
        }
    }
}
