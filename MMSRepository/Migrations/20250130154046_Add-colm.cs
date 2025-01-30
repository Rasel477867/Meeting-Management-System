using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMSRepository.Migrations
{
    /// <inheritdoc />
    public partial class Addcolm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Host",
                table: "Bookings");
        }
    }
}
