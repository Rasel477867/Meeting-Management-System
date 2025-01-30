using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMSRepository.Migrations
{
    /// <inheritdoc />
    public partial class Create_Booking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    RepetitionOption = table.Column<int>(type: "int", nullable: false),
                    EndRepeatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DaysToRepeatedOn = table.Column<int>(type: "int", nullable: true),
                    RequestedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
