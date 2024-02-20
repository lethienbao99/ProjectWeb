using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectWeb.Data.Migrations
{
    public partial class addnewColMerchant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tmncode",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"),
                column: "ConcurrencyStamp",
                value: "c3dbf622-2577-4021-8d9e-600eba90b38b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0762758f-19de-424a-9552-fbc7938d9c51", "AQAAAAEAACcQAAAAEM83qxyCnl2fyxkLrIiysCIxOdhxA9i9XA8suvxilfXw2CXuPPJFDx7HC0E9mzCxHw==" });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "ID", "DateCreated", "DateDeleted", "DateUpdated", "IsActive", "IsDelete", "MerchantIpnUrl", "MerchantName", "MerchantPayLink", "MerchantReturnUrl", "SerectKey", "ShortName", "Sort", "Tmncode", "Version" },
                values: new object[] { new Guid("56b31a9c-9feb-412e-b14d-76ee14a7b13a"), null, null, null, null, null, "https://localhost:5001", "VNPAY", "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "https://localhost:5001", "YONPSVXYSUNSPVKIUOOOWXASIHLLYIFS", "VNPay", 0, "APPZFC7N", "2.1.0" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Merchants",
                keyColumn: "ID",
                keyValue: new Guid("56b31a9c-9feb-412e-b14d-76ee14a7b13a"));

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Tmncode",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Merchants");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee976566-d4be-407b-96d4-5c69da8806a8"),
                column: "ConcurrencyStamp",
                value: "c455b411-6302-4b82-845e-3d9ead0a2524");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fd3bc079-8c61-4ff2-a5b7-278a58ec5273"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b42f4dec-e0e1-449d-8d35-e5d495997e73", "AQAAAAEAACcQAAAAECJ6/k3GzZpRH8iftU+XPymQRow7pEG2VyrM4dOVbE1YyVxfVGyXQiCEvuA4IF6WFQ==" });
        }
    }
}
