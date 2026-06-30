using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e0dbce9c-7b6c-4260-9644-9a52c164a5c6", "AQAAAAIAAYagAAAAELsMjrHzYfc0rDLdVCMZTPtj7lQCXYXQrk1rfLu5syqaYqF3mDTosb3TQ+PZHGBkpA==", "29a988f5-70e9-49f2-837b-63334891c1db" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a3c8139e-2791-4408-ba94-41eb648cd172", "AQAAAAIAAYagAAAAEOvnYIqACG6cmbC/YzwtFBJVlvTIxdx3raV6PZPNr2VX8RBltkfjCuhvYIZi5Lo+Bg==", "dd116dd2-cb7d-4cc3-8ed5-81df02f0a9bc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
