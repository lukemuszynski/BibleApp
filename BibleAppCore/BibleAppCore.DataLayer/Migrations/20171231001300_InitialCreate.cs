using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BibleAppCore.DataLayer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    BookFullName = table.Column<string>(maxLength: 50, nullable: true),
                    BookGlobalNumber = table.Column<int>(nullable: false),
                    BookName = table.Column<string>(maxLength: 50, nullable: false),
                    SubbookNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "BookExtended",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    BookFullName = table.Column<string>(maxLength: 8000, nullable: false),
                    BookGlobalNumber = table.Column<int>(nullable: false),
                    BookName = table.Column<string>(maxLength: 50, nullable: false),
                    NextBookGuid = table.Column<Guid>(nullable: false),
                    PassagesJson = table.Column<string>(maxLength: 8000, nullable: false),
                    PreviousBookGuid = table.Column<Guid>(nullable: false),
                    SubbookNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookExtended", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    AddTime = table.Column<DateTime>(nullable: false),
                    BookGuid = table.Column<Guid>(nullable: false),
                    EndIndex = table.Column<string>(maxLength: 2, nullable: false),
                    IsAudioFile = table.Column<bool>(nullable: false),
                    IsYoutubeVideo = table.Column<bool>(nullable: false),
                    ManageCommentKeyGuid = table.Column<Guid>(nullable: false),
                    StartIndex = table.Column<string>(maxLength: 2, nullable: false),
                    Text = table.Column<string>(maxLength: 5000, nullable: false),
                    Title = table.Column<string>(maxLength: 500, nullable: false),
                    Url = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Passage",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Book = table.Column<string>(maxLength: 50, nullable: false),
                    BookGuid = table.Column<Guid>(nullable: false),
                    Meantitle1 = table.Column<string>(nullable: true),
                    Meantitle2 = table.Column<string>(nullable: true),
                    PassageNumber = table.Column<int>(nullable: false),
                    PassageText = table.Column<string>(maxLength: 8000, nullable: false),
                    Title1 = table.Column<string>(nullable: true),
                    Title2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passage", x => x.Guid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "BookExtended");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Passage");
        }
    }
}
