using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoT.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCommandEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommandTypes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCommands",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: false),
                    IssuedById = table.Column<Guid>(type: "uuid", nullable: false),
                    CommandTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Parameters = table.Column<string>(type: "text", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AcknowledgedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCommands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceCommands_AspNetUsers_IssuedById",
                        column: x => x.IssuedById,
                        principalSchema: "public",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceCommands_CommandTypes_CommandTypeId",
                        column: x => x.CommandTypeId,
                        principalSchema: "public",
                        principalTable: "CommandTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceCommands_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "public",
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommandTypes_Slug",
                schema: "public",
                table: "CommandTypes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCommands_CommandTypeId",
                schema: "public",
                table: "DeviceCommands",
                column: "CommandTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCommands_DeviceId",
                schema: "public",
                table: "DeviceCommands",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCommands_IssuedById",
                schema: "public",
                table: "DeviceCommands",
                column: "IssuedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceCommands",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CommandTypes",
                schema: "public");
        }
    }
}
