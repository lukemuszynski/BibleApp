using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BibleAppCore.DataLayer.Migrations
{
    public partial class UserInComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "Comment",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserLogin",
                table: "Comment",
                nullable: false,
                defaultValue: "SYSTEM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "UserLogin",
                table: "Comment");
        }
    }
}
