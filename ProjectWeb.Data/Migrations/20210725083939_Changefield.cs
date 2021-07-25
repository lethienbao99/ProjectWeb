using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectWeb.Data.Migrations
{
    public partial class Changefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"));

            migrationBuilder.DeleteData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273"));

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"), new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273") });

            migrationBuilder.DeleteData(
                table: "UserInformations",
                keyColumn: "ID",
                keyValue: new Guid("2ae5fecc-aeb6-4514-bfb5-34f2284adbf8"));

            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "Images",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FileSize",
                table: "Images",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "DateCreated", "DateDeleted", "DateUpdated", "Description", "IsDelete", "Name", "NormalizedName" },
                values: new object[] { new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"), "60d82b65-c54c-44ab-bda2-a8ab5c1f29da", null, null, null, "Administrator role", null, "admin", "admin" });

            migrationBuilder.InsertData(
                table: "UserInformations",
                columns: new[] { "ID", "Address", "DateCreated", "DateDeleted", "DateOfBirth", "DateUpdated", "FirstName", "IsDelete", "LastName", "PhoneNumber", "Status" },
                values: new object[] { new Guid("2ae5fecc-aeb6-4514-bfb5-34f2284adbf8"), null, null, null, null, null, "admin", null, "admin", "454545", null });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"), new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273") });

            migrationBuilder.InsertData(
                table: "SystemUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateCreated", "DateDeleted", "DateUpdated", "Email", "EmailConfirmed", "IsDelete", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserInfomationID", "UserName" },
                values: new object[] { new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273"), 0, "eb3171b8-61ef-4675-8262-a47edb3441d4", null, null, null, "lethienbao3012@gmail.com", true, null, false, null, "lethienbao3012@gmail.com", "admin", "AQAAAAEAACcQAAAAEOfn+Ejgye0CL2ynZ3M/QLGinW5LB1xmqAKMi+dBpnzp8zxBTR6Ktc2eO8FgMFhn1Q==", null, false, "", false, new Guid("2ae5fecc-aeb6-4514-bfb5-34f2284adbf8"), "admin" });
        }
    }
}
