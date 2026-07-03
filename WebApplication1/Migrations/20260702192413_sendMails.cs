using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class sendMails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "33738161-9f05-4c22-b0b0-aa953cfa4222", "AQAAAAIAAYagAAAAEOHMexftU+/xt0s8v2Tz1spi6NtWhRuvQU2AaZi7PZre47zVWigUYiwFDKjkPm70Rg==", "84d1be6f-137e-4c3e-a9e6-23ec54dfa83c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "10bb34cb-6b83-45a1-80db-0ae9f88bdafa", "AQAAAAIAAYagAAAAEN2iE4nS7YDsSMmiLhjeDOqgl0YHjGFobtGMBcmL787BusALAbDwJ5iVVDoCE6FtXw==", "c0dde033-5542-4c48-a885-4198897d82ed" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
