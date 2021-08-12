using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectWeb.Data.Migrations
{
    public partial class AddColMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guest",
                table: "Messages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guest",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"),
                column: "ConcurrencyStamp",
                value: "165acda1-3b5a-419c-a8de-8e710ef541ab");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "eaca05dc-b516-432a-bc79-a7a713a067a7", "AQAAAAEAACcQAAAAELay4WJcPN++WY7oSj0bqIsruh9wjvS64HplgoiK8SIT7eXPM0PJ++Pma2Sry5wbQQ==" });
        }
    }
}
