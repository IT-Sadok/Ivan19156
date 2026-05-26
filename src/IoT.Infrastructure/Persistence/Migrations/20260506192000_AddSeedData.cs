using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IoT.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "public",
                table: "CommandTypes",
                columns: new[] { "Id", "CreatedAt", "Description", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-0001-0000-0000-000000000000"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Turn on the device", "turn_on", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1b2c3d4-0002-0000-0000-000000000000"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Turn off the device", "turn_off", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1b2c3d4-0003-0000-0000-000000000000"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Reboot the device", "reboot", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1b2c3d4-0004-0000-0000-000000000000"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Set target temperature", "set_temperature", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("a1b2c3d4-0005-0000-0000-000000000000"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Update device firmware", "update_firmware", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Devices",
                columns: new[] { "Id", "AdminStatus", "CreatedAt", "LastSeen", "ManufacturerId", "Name", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b1c2d3e4-0001-0000-0000-000000000000"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Test Temperature Sensor", 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("b1c2d3e4-0002-0000-0000-000000000000"), 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Test Humidity Sensor", 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "SensorDevices",
                columns: new[] { "Id", "MeasurementUnit", "SensorType" },
                values: new object[,]
                {
                    { new Guid("b1c2d3e4-0001-0000-0000-000000000000"), "°C", 0 },
                    { new Guid("b1c2d3e4-0002-0000-0000-000000000000"), "%", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "CommandTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-0001-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "CommandTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-0002-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "CommandTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-0003-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "CommandTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-0004-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "CommandTypes",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-0005-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "SensorDevices",
                keyColumn: "Id",
                keyValue: new Guid("b1c2d3e4-0001-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "SensorDevices",
                keyColumn: "Id",
                keyValue: new Guid("b1c2d3e4-0002-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("b1c2d3e4-0001-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Devices",
                keyColumn: "Id",
                keyValue: new Guid("b1c2d3e4-0002-0000-0000-000000000000"));
        }
    }
}
