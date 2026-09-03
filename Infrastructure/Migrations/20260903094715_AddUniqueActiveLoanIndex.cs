using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueActiveLoanIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_loans_book_id",
                table: "loans");

            migrationBuilder.CreateIndex(
                name: "ix_loans_book_id_active",
                table: "loans",
                column: "book_id",
                unique: true,
                filter: "\"returned_at\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_loans_book_id_active",
                table: "loans");

            migrationBuilder.CreateIndex(
                name: "IX_loans_book_id",
                table: "loans",
                column: "book_id");
        }
    }
}
