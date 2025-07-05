using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class ssaıaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Books_BooksId",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_BooksId",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "BooksId",
                table: "Borrows");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BookId",
                table: "Borrows",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Books_BookId",
                table: "Borrows",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrows_Books_BookId",
                table: "Borrows");

            migrationBuilder.DropIndex(
                name: "IX_Borrows_BookId",
                table: "Borrows");

            migrationBuilder.AddColumn<int>(
                name: "BooksId",
                table: "Borrows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BooksId",
                table: "Borrows",
                column: "BooksId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrows_Books_BooksId",
                table: "Borrows",
                column: "BooksId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
