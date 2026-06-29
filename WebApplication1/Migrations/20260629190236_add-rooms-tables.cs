using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class addroomstables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasDrinksAndCafeteria",
                table: "WorkSpaces",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalRooms",
                table: "WorkSpaces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalTables",
                table: "WorkSpaces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ec48c453-379e-477f-b131-9346c9593567", "AQAAAAIAAYagAAAAEEN6+jV3BMTYtqna3t9EU/HbIDcf5CWjqF4MF3T9RFmSQFts5pRweD6nARdpBsiqRA==", "ae1a2ce5-d193-497d-a26b-aeba14e12ddc" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "77207855-ed05-4fba-9855-d16be51b445c", "AQAAAAIAAYagAAAAEO7A0kSlJt6YXOdXsdj9NHoyoKw3yoKD+6yUDIZO0BxuhYyXHbmET8DI4n3owiM4TQ==", "3aee01e1-de5a-4253-ad16-1376721e9b60" });

            migrationBuilder.UpdateData(
                table: "WorkSpaces",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "HasDrinksAndCafeteria", "TotalRooms", "TotalTables" },
                values: new object[] { false, 0, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasDrinksAndCafeteria",
                table: "WorkSpaces");

            migrationBuilder.DropColumn(
                name: "TotalRooms",
                table: "WorkSpaces");

            migrationBuilder.DropColumn(
                name: "TotalTables",
                table: "WorkSpaces");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "456c4ada-6705-4e8e-ac2a-cfb285cfbe42", "AQAAAAIAAYagAAAAEIjwWWrkdfyqnOZd7UPsrLEXzcZjua9dwzDF90xKWcWD1SfkFpP3wpu3AXE2L+j38g==", "a5ab9dca-7e81-4cda-9787-358d278af6d9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "72b652b1-9f79-4bbf-9f67-b499d762460f", "AQAAAAIAAYagAAAAEI6MErKIw8ixld26/U/NeXkSSFPqrGN6z7P99h98M7SkKVR6RzhbtlYE4SubIglD1A==", "a69bee03-9b28-4f8b-a4b6-8a0a59cab887" });
        }
    }
}
