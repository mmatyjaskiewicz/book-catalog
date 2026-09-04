using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SplitActiveAndArchivedLoans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_loans_book_id_active",
                table: "loans");

            migrationBuilder.DropColumn(
                name: "returned_at",
                table: "loans");

            migrationBuilder.CreateTable(
                name: "archived_loans",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    borrowed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    returned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_archived_loans", x => x.id);
                    table.ForeignKey(
                        name: "FK_archived_loans_books_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_archived_loans_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_loans_book_id_active",
                table: "loans",
                column: "book_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_archived_loans_book_id",
                table: "archived_loans",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_archived_loans_user_id",
                table: "archived_loans",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "archived_loans");

            migrationBuilder.DropIndex(
                name: "ix_loans_book_id_active",
                table: "loans");

            migrationBuilder.AddColumn<DateTime>(
                name: "returned_at",
                table: "loans",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_loans_book_id_active",
                table: "loans",
                column: "book_id",
                unique: true,
                filter: "\"returned_at\" IS NULL");
        }
    }
}
