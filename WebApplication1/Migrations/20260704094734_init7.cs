using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class init7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "32075d67-39a8-48cd-bb49-96503a4e7522", "AQAAAAIAAYagAAAAEMy3BAQScPRh4vW5CminviYIAu1/5rir4Dj0g2BxFuVcqoTmG2zaJyT/sO0RncDFYQ==", "bb0733a6-efca-46c1-9d69-7620d016eabc" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e62aec52-6599-4943-8cf1-341c15be8165", "AQAAAAIAAYagAAAAEAH02Gc4qwxlidJ6neIICIihQBZ0HP57ZgKb9zZ98L3tzX5LFErLckCtjYL7fDU9mg==", "3b6fba23-27d9-47b9-8205-e3eb32147d78" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "03288b2e-e691-49ea-98cd-8aba42c06180", "AQAAAAIAAYagAAAAEMMIHyjxvrL0ICM4uvnPVCoIbSeJ/AJSlZgJdXb6LMgJO2digvbbi5BOpyW9uwtPRg==", "5f49fe5d-854d-40c4-89d8-d6430cd072e5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "97c0e216-b3cf-42e6-9a39-fd65b3b019db", "AQAAAAIAAYagAAAAEEHf/076xxiatUxPHRQUSNQ3taSfWfkYvavjSkV71XM1gIdQCX25ar7xN383m4f9rg==", "a0fd5dc1-cc67-4baa-b8f4-e80ce2e58f89" });
        }
    }
}
