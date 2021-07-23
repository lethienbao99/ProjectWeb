using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectWeb.Data.Migrations
{
    public partial class AddUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "DateCreated", "DateDeleted", "DateUpdated", "Description", "IsDelete", "Name", "NormalizedName" },
                values: new object[] { new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"), "c0bd8abb-04c0-4c18-9af4-365aca52889b", null, null, null, "Administrator role", null, "admin", "admin" });

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
                values: new object[] { new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273"), 0, "3e0689e8-dd79-49df-b984-496f59e14a5c", null, null, null, "lethienbao3012@gmail.com", true, null, false, null, "lethienbao3012@gmail.com", "admin", "AQAAAAEAACcQAAAAEFhIyRY8mslLjbEGvDvVtAHv1lgdtU7xBUwtzG3fjlgTPwc78Uq5iDn4SisDAmDETQ==", null, false, "", false, new Guid("2ae5fecc-aeb6-4514-bfb5-34f2284adbf8"), "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
