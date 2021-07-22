using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectWeb.Data.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppConfigs",
                columns: new[] { "ID", "DateCreated", "DateDeleted", "DateUpdated", "IsDelete", "Key", "Value" },
                values: new object[] { new Guid("6e48c829-e01a-4204-b297-17f421915116"), new DateTime(2021, 7, 22, 15, 44, 42, 894, DateTimeKind.Local).AddTicks(2542), null, null, null, "SuperAdmin", "True" });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "CategoryID", "ID", "ProductID", "DateCreated", "DateDeleted", "DateUpdated", "IsDelete" },
                values: new object[] { new Guid("3bc23769-d612-40c9-8d7a-6b15c621302d"), new Guid("628da97c-6ddb-4ab4-9bb8-55b429b50dc4"), new Guid("e75990a1-04dd-4413-9644-1bc157c9e477"), new DateTime(2021, 7, 22, 15, 44, 42, 897, DateTimeKind.Local).AddTicks(1326), null, null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppConfigs",
                keyColumn: "ID",
                keyValue: new Guid("6e48c829-e01a-4204-b297-17f421915116"));

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumns: new[] { "CategoryID", "ID", "ProductID" },
                keyValues: new object[] { new Guid("3bc23769-d612-40c9-8d7a-6b15c621302d"), new Guid("628da97c-6ddb-4ab4-9bb8-55b429b50dc4"), new Guid("e75990a1-04dd-4413-9644-1bc157c9e477") });
        }
    }
}
