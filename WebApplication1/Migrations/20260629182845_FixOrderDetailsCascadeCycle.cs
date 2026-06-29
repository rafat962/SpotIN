using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderDetailsCascadeCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e", "4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<decimal>(
                name: "WalletBalance",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "WorkSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasWiFi = table.Column<bool>(type: "bit", nullable: false),
                    HasAirConditioning = table.Column<bool>(type: "bit", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSpaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkSpaces_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    WorkSpaceID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_WorkSpaces_WorkSpaceID",
                        column: x => x.WorkSpaceID,
                        principalTable: "WorkSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    WorkspaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resourses_WorkSpaces_WorkspaceId",
                        column: x => x.WorkspaceId,
                        principalTable: "WorkSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_Resourses_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalHoursCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOrdersCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PriceAtSale = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "WalletBalance" },
                values: new object[] { "456c4ada-6705-4e8e-ac2a-cfb285cfbe42", "AQAAAAIAAYagAAAAEIjwWWrkdfyqnOZd7UPsrLEXzcZjua9dwzDF90xKWcWD1SfkFpP3wpu3AXE2L+j38g==", "a5ab9dca-7e81-4cda-9787-358d278af6d9", 500.00m });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName", "WalletBalance" },
                values: new object[] { "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a", 0, "72b652b1-9f79-4bbf-9f67-b499d762460f", "client@spotin.com", true, "Normal", "Client", false, null, "CLIENT@SPOTIN.COM", "CLIENT@SPOTIN.COM", "AQAAAAIAAYagAAAAEI6MErKIw8ixld26/U/NeXkSSFPqrGN6z7P99h98M7SkKVR6RzhbtlYE4SubIglD1A==", null, false, "a69bee03-9b28-4f8b-a4b6-8a0a59cab887", false, "client@spotin.com", 150.00m });

            migrationBuilder.InsertData(
                table: "WorkSpaces",
                columns: new[] { "Id", "Address", "Description", "HasAirConditioning", "HasWiFi", "Latitude", "Longitude", "Name", "OwnerId" },
                values: new object[] { 1, "12 Tahrir Street, Dokki, Giza", "Cozy workspace with premium high-speed internet and silent rooms.", true, true, 30.038399999999999, 31.212199999999999, "SpotIn Dokki Branch", "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e", "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a" });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "IsAvailable", "Name", "Price", "WorkSpaceID" },
                values: new object[,]
                {
                    { 1, true, "Turkish Coffee", 25.00m, 1 },
                    { 2, true, "Mineral Water", 10.00m, 1 }
                });

            migrationBuilder.InsertData(
                table: "Resourses",
                columns: new[] { "Id", "HourlyRate", "IsAvailable", "Name", "Type", "WorkspaceId" },
                values: new object[,]
                {
                    { 1, 20.00m, false, "Red Table - Open Space", "Table", 1 },
                    { 2, 100.00m, true, "Meeting Room A", "MeetingRoom", 1 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "EndTime", "ResourceId", "StartTime", "Status", "UserId" },
                values: new object[] { 1, null, 1, new DateTime(2026, 6, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), "Active", "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a" });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "BookingId", "GrandTotal", "IssueDate", "PaymentMethod", "TotalHoursCost", "TotalOrdersCost" },
                values: new object[] { 1, 1, 120.00m, new DateTime(2026, 6, 29, 21, 0, 0, 0, DateTimeKind.Unspecified), "Cash", 60.00m, 60.00m });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "BookingId", "OrderDate", "OrderStatus" },
                values: new object[] { 1, 1, new DateTime(2026, 6, 29, 18, 30, 0, 0, DateTimeKind.Unspecified), "Delivered" });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "Id", "MenuItemId", "OrderId", "PriceAtSale", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 25.00m, 2 },
                    { 2, 2, 1, 10.00m, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ResourceId",
                table: "Bookings",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_BookingId",
                table: "Invoices",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_WorkSpaceID",
                table: "MenuItems",
                column: "WorkSpaceID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_MenuItemId",
                table: "OrderDetails",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BookingId",
                table: "Orders",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Resourses_WorkspaceId",
                table: "Resourses",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_BookingId",
                table: "WalletTransactions",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_UserId",
                table: "WalletTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpaces_OwnerId",
                table: "WorkSpaces",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "WalletTransactions");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Resourses");

            migrationBuilder.DropTable(
                name: "WorkSpaces");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e", "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4d5e6f7a-8b9c-0d1e-2f3a4b5c6d7e8f9a");

            migrationBuilder.DropColumn(
                name: "WalletBalance",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "08fd27cf-3751-4189-ba08-b390dbe23090", "AQAAAAIAAYagAAAAEEdOuzXNdZXDCmJbk3vIvsz+4ftHD+ElefhV22u2kqMzCHxmeXyNM4S2aRLZzsDMjw==", "088154b7-232c-4f64-a4d8-9ee2b4db6953" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a", 0, "1be09457-34a2-4068-8eb7-111d1f0fe098", "client@spotin.com", true, "Normal", "Client", false, null, "CLIENT@SPOTIN.COM", "CLIENT@SPOTIN.COM", "AQAAAAIAAYagAAAAELKLmm5bVujq919ilWe6/LYn1r/PsYRIwc6a48Dy/Q0DJ1NAGdJPWm45G2fbXRvVRg==", null, false, "b768aa43-4656-49a4-931e-110afb3dde0c", false, "client@spotin.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e", "4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a" });
        }
    }
}
