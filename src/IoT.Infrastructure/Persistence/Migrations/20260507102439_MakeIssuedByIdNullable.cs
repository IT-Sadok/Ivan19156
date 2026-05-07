using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoT.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeIssuedByIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceCommands_AspNetUsers_IssuedById",
                schema: "public",
                table: "DeviceCommands");

            migrationBuilder.AlterColumn<Guid>(
                name: "IssuedById",
                schema: "public",
                table: "DeviceCommands",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceCommands_AspNetUsers_IssuedById",
                schema: "public",
                table: "DeviceCommands",
                column: "IssuedById",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeviceCommands_AspNetUsers_IssuedById",
                schema: "public",
                table: "DeviceCommands");

            migrationBuilder.AlterColumn<Guid>(
                name: "IssuedById",
                schema: "public",
                table: "DeviceCommands",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceCommands_AspNetUsers_IssuedById",
                schema: "public",
                table: "DeviceCommands",
                column: "IssuedById",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
