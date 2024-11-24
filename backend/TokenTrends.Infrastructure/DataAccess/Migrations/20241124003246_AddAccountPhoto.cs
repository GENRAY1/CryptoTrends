using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TokenTrends.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountPhoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoFileName",
                table: "Account",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoFileName",
                table: "Account");
        }
    }
}
