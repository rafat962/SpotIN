using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "70b512bb-56a7-4bb6-a5a4-55a0fc75b25d", "AQAAAAIAAYagAAAAELLDDGnRCVwEblEuGR9F7g6GYABYsyaIPl2SEo3bw4zKnAw/aMg+zTK0aCTEIUlNdg==", "b7beec98-5115-4b4f-8115-22ffee5c7d7e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fad61d55-7b6f-44a5-9c6d-00263ccf10cc", "AQAAAAIAAYagAAAAECvNpazlJThGl1ezlrURQE7577Eu0SF0JQ5oKeACM52SOMVf1kCh7MQKZsZMSULltA==", "0188710b-dc31-4cf6-acca-fbadf391340b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
