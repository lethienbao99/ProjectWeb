using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectWeb.Data.Migrations
{
    public partial class AddColM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleText",
                table: "Messages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"),
                column: "ConcurrencyStamp",
                value: "556c5aa4-0cc2-4ee7-825d-1f059f3a5524");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "804ba01f-d1d7-4fef-9d59-9340eb1cd711", "AQAAAAEAACcQAAAAEIHi2xJcpoYI82uTUTXGFTEhEudVfWXLgkF1pJY9zmIMqYvlsC/dmx4OFdxhi+Ssqw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleText",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"),
                column: "ConcurrencyStamp",
                value: "f8e8f7c5-fef4-4167-918f-2adeb21e3b3a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "909cc991-2ae3-46c0-9f8c-3cafc21b1ce4", "AQAAAAEAACcQAAAAEF/pCc7nN/ZPc3NfUn9OW8WwEZRIzKsfoZrEMVy9+yz+6h3LhSJnsfdAGCLz9QMcmQ==" });
        }
    }
}
