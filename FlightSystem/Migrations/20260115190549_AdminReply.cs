using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightSystem.Migrations
{
    /// <inheritdoc />
    public partial class AdminReply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminReply",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RepliedAt",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ArrivalTime", "DepartureTime" },
                values: new object[] { new DateTime(2026, 1, 17, 21, 5, 48, 525, DateTimeKind.Local).AddTicks(4332), new DateTime(2026, 1, 17, 20, 5, 48, 525, DateTimeKind.Local).AddTicks(4275) });

            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ArrivalTime", "DepartureTime" },
                values: new object[] { new DateTime(2026, 1, 19, 22, 5, 48, 525, DateTimeKind.Local).AddTicks(4340), new DateTime(2026, 1, 19, 20, 5, 48, 525, DateTimeKind.Local).AddTicks(4339) });

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 1, 15, 19, 5, 48, 525, DateTimeKind.Utc).AddTicks(4519));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminReply",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "RepliedAt",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ArrivalTime", "DepartureTime" },
                values: new object[] { new DateTime(2025, 12, 30, 16, 23, 56, 290, DateTimeKind.Local).AddTicks(1666), new DateTime(2025, 12, 30, 15, 23, 56, 290, DateTimeKind.Local).AddTicks(1596) });

            migrationBuilder.UpdateData(
                table: "Flights",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ArrivalTime", "DepartureTime" },
                values: new object[] { new DateTime(2026, 1, 1, 17, 23, 56, 290, DateTimeKind.Local).AddTicks(1682), new DateTime(2026, 1, 1, 15, 23, 56, 290, DateTimeKind.Local).AddTicks(1679) });

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 28, 14, 23, 56, 290, DateTimeKind.Utc).AddTicks(1968));
        }
    }
}
