using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectWeb.Data.Migrations
{
    public partial class AddColOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Orders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"),
                column: "ConcurrencyStamp",
                value: "fef1370b-7097-4797-b094-8168288a8a11");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a4a70350-294e-4f4e-91ba-9fe48015f007", "AQAAAAEAACcQAAAAEMYjcAg5VB314o42qAe22r8Nrt+0Wd61ZygdHYLAXADoa3pD+nKXw5nQAb2nokE5VQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"),
                column: "ConcurrencyStamp",
                value: "679eab72-241d-4507-b7de-ea101948ffbd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8f0951ba-389f-4471-bf24-74b79e08dfaa", "AQAAAAEAACcQAAAAENGm1vUAX5zhwcxVOtc0lGhZZTzQkP5TS00zCudHuuxGDCw53pIgG/QayAZhj+nLPg==" });
        }
    }
}
