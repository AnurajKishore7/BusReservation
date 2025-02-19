using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusApp.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EstimatedDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Distance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusRoutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    DOB = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Users_Email",
                        column: x => x.Email,
                        principalTable: "Users",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "TransportOperators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportOperators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportOperators_Users_Email",
                        column: x => x.Email,
                        principalTable: "Users",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "Buses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    OperatorId = table.Column<int>(type: "int", nullable: false),
                    BusType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buses_TransportOperators_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "TransportOperators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusRouteId = table.Column<int>(type: "int", nullable: false),
                    BusId = table.Column<int>(type: "int", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_BusRoutes_BusRouteId",
                        column: x => x.BusRouteId,
                        principalTable: "BusRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trips_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    BookedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentMadeAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketPassengers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeatNo = table.Column<int>(type: "int", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPassengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketPassengers_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BusRoutes",
                columns: new[] { "Id", "Destination", "Distance", "EstimatedDuration", "Source" },
                values: new object[,]
                {
                    { 1, "Kanyakumari", 750, "12:30", "Chennai" },
                    { 2, "Chennai", 750, "12:30", "Kanyakumari" },
                    { 3, "Bangalore", 350, "06:00", "Chennai" },
                    { 4, "Chennai", 350, "06:00", "Bangalore" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "CreatedAt", "IsApproved", "IsDeleted", "Name", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[,]
                {
                    { "admin@gmail.com", new DateTime(2025, 2, 19, 11, 7, 45, 737, DateTimeKind.Local).AddTicks(2499), true, false, "Super Admin", new byte[] { 135, 89, 179, 178, 87, 181, 95, 146, 213, 209, 219, 173, 6, 207, 171, 18, 126, 231, 141, 72, 161, 119, 25, 86, 94, 95, 157, 155, 148, 145, 128, 7, 32, 3, 79, 119, 79, 33, 129, 155, 2, 52, 97, 97, 63, 240, 161, 153, 33, 188, 40, 240, 249, 182, 243, 180, 27, 171, 171, 152, 92, 156, 1, 229 }, new byte[] { 126, 202, 212, 226, 243, 172, 167, 31, 139, 161, 216, 55, 27, 165, 243, 204, 76, 220, 176, 214, 77, 40, 78, 242, 212, 200, 111, 0, 58, 101, 17, 75, 82, 235, 92, 154, 63, 160, 234, 157, 214, 213, 143, 164, 41, 128, 202, 105, 123, 172, 162, 113, 9, 59, 214, 185, 103, 159, 102, 239, 253, 173, 16, 34, 121, 245, 230, 95, 17, 87, 253, 211, 14, 189, 61, 22, 189, 84, 199, 140, 131, 18, 172, 157, 89, 243, 38, 62, 155, 85, 199, 247, 97, 74, 126, 236, 76, 43, 148, 16, 73, 146, 252, 240, 60, 4, 130, 31, 128, 95, 106, 26, 43, 208, 121, 104, 243, 254, 230, 253, 214, 106, 193, 137, 243, 27, 34, 69 }, "Admin" },
                    { "anuraj@gmail.com", new DateTime(2025, 2, 19, 11, 7, 45, 737, DateTimeKind.Local).AddTicks(2503), true, false, "Anuraj", new byte[] { 236, 177, 207, 82, 244, 194, 155, 138, 166, 35, 96, 207, 187, 219, 244, 137, 142, 236, 98, 244, 14, 24, 120, 4, 190, 230, 202, 39, 142, 213, 171, 219, 172, 139, 206, 86, 6, 156, 231, 226, 107, 51, 245, 241, 0, 81, 165, 152, 108, 247, 116, 27, 44, 253, 180, 133, 76, 102, 126, 167, 178, 220, 215, 136 }, new byte[] { 180, 180, 156, 75, 144, 109, 79, 133, 61, 141, 220, 151, 221, 193, 204, 126, 158, 191, 107, 150, 195, 206, 133, 87, 82, 58, 95, 53, 121, 170, 152, 49, 228, 66, 191, 99, 176, 37, 151, 33, 41, 172, 11, 40, 60, 18, 22, 73, 6, 182, 140, 228, 245, 134, 41, 195, 26, 65, 131, 136, 124, 45, 244, 104, 179, 225, 44, 231, 200, 21, 124, 83, 125, 139, 52, 20, 145, 53, 127, 161, 111, 110, 112, 28, 199, 65, 8, 47, 27, 57, 240, 242, 117, 55, 138, 4, 255, 144, 247, 123, 102, 36, 93, 227, 164, 168, 35, 134, 47, 97, 198, 162, 184, 101, 46, 221, 253, 92, 91, 132, 42, 82, 26, 170, 61, 28, 69, 86 }, "Client" },
                    { "smartbus@gmail.com", new DateTime(2025, 2, 19, 11, 7, 45, 737, DateTimeKind.Local).AddTicks(2501), true, false, "Smart Bus", new byte[] { 127, 242, 60, 50, 7, 36, 123, 248, 118, 137, 188, 212, 191, 76, 229, 108, 119, 20, 5, 213, 210, 53, 206, 95, 191, 109, 20, 221, 74, 210, 45, 78, 112, 196, 203, 197, 131, 178, 230, 190, 41, 8, 243, 183, 5, 149, 210, 163, 85, 202, 114, 151, 156, 181, 91, 50, 132, 233, 52, 60, 5, 228, 228, 106 }, new byte[] { 140, 89, 237, 31, 135, 84, 156, 114, 190, 60, 29, 36, 223, 53, 2, 236, 74, 124, 110, 47, 44, 150, 242, 21, 56, 68, 165, 183, 201, 173, 17, 116, 184, 33, 248, 251, 43, 74, 48, 97, 248, 198, 138, 235, 120, 14, 249, 102, 38, 73, 18, 26, 81, 46, 133, 114, 73, 107, 19, 142, 242, 114, 169, 66, 190, 191, 90, 131, 31, 160, 143, 192, 231, 61, 201, 11, 62, 92, 76, 37, 82, 194, 185, 118, 206, 129, 136, 158, 155, 20, 239, 61, 34, 85, 199, 242, 217, 133, 195, 77, 123, 138, 71, 190, 221, 178, 200, 77, 143, 74, 42, 240, 52, 132, 155, 236, 227, 42, 181, 87, 106, 3, 170, 135, 117, 180, 183, 128 }, "TransportOperator" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Contact", "DOB", "Email", "Gender", "IsDeleted", "IsDisabled", "Name" },
                values: new object[] { 1, "+911234567890", new DateOnly(2002, 7, 11), "anuraj@gmail.com", "Male", false, false, "Anuraj" });

            migrationBuilder.InsertData(
                table: "TransportOperators",
                columns: new[] { "Id", "Contact", "Email", "IsDeleted", "Name" },
                values: new object[] { 1, "+919876543210", "smartbus@gmail.com", false, "SmartBus" });

            migrationBuilder.InsertData(
                table: "Buses",
                columns: new[] { "Id", "BusNo", "BusType", "OperatorId", "TotalSeats" },
                values: new object[,]
                {
                    { 1, "TN01AB1234", "AC Sleeper", 1, 40 },
                    { 2, "TN01AB1235", "non-AC Seater", 1, 40 }
                });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "Id", "ArrivalTime", "BusId", "BusRouteId", "DepartureTime", "Price" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 10, 20, 30, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2025, 2, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), 700m },
                    { 2, new DateTime(2025, 2, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), 2, 3, new DateTime(2025, 2, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), 350m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ClientId",
                table: "Bookings",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TripId",
                table: "Bookings",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_OperatorId",
                table: "Buses",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                table: "Clients",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketPassengers_BookingId",
                table: "TicketPassengers",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportOperators_Email",
                table: "TransportOperators",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_BusId",
                table: "Trips",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_BusRouteId",
                table: "Trips",
                column: "BusRouteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "TicketPassengers");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "BusRoutes");

            migrationBuilder.DropTable(
                name: "Buses");

            migrationBuilder.DropTable(
                name: "TransportOperators");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
