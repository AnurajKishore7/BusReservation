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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    IsDiabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Users_Email",
                        column: x => x.Email,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportOperators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportOperators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportOperators_Users_Email",
                        column: x => x.Email,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
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
                values: new object[] { 1, "Kanyakumari", 750, "12:30", "Chennai" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "CreatedAt", "IsApproved", "Name", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[,]
                {
                    { "admin@gmail.com", new DateTime(2025, 2, 9, 23, 5, 47, 710, DateTimeKind.Local).AddTicks(945), true, "Super Admin", new byte[] { 122, 59, 147, 6, 163, 2, 135, 202, 103, 49, 155, 147, 139, 82, 56, 39, 83, 82, 235, 74, 178, 41, 13, 0, 183, 41, 17, 160, 4, 10, 44, 208, 152, 163, 203, 249, 31, 98, 104, 101, 237, 126, 196, 13, 194, 245, 138, 205, 28, 54, 147, 23, 37, 252, 254, 244, 226, 64, 168, 128, 17, 253, 0, 153 }, new byte[] { 67, 195, 53, 206, 216, 43, 243, 94, 253, 32, 104, 51, 187, 103, 247, 230, 205, 118, 14, 125, 117, 129, 102, 217, 223, 84, 71, 177, 19, 137, 255, 227, 67, 101, 135, 95, 127, 64, 88, 238, 176, 38, 87, 186, 23, 240, 201, 224, 9, 203, 50, 126, 205, 17, 18, 56, 206, 51, 149, 105, 76, 69, 89, 185, 67, 52, 15, 134, 191, 217, 8, 55, 120, 204, 81, 227, 152, 127, 23, 37, 13, 80, 92, 93, 78, 226, 233, 131, 63, 98, 248, 243, 21, 208, 186, 11, 159, 40, 0, 221, 238, 131, 10, 18, 3, 71, 167, 154, 115, 131, 253, 173, 189, 225, 86, 74, 185, 65, 103, 74, 151, 216, 20, 141, 67, 147, 207, 254 }, "Admin" },
                    { "anuraj@gmail.com", new DateTime(2025, 2, 9, 23, 5, 47, 710, DateTimeKind.Local).AddTicks(951), true, "Anuraj", new byte[] { 119, 254, 232, 33, 147, 111, 207, 113, 133, 117, 163, 244, 149, 251, 159, 95, 219, 211, 115, 146, 245, 153, 208, 78, 44, 97, 27, 243, 94, 2, 194, 19, 237, 143, 163, 241, 216, 218, 67, 8, 179, 137, 129, 226, 6, 194, 195, 57, 29, 167, 62, 196, 167, 157, 45, 245, 115, 236, 114, 78, 105, 186, 224, 44 }, new byte[] { 130, 78, 233, 146, 239, 121, 102, 64, 31, 148, 9, 115, 151, 250, 250, 224, 154, 155, 80, 242, 89, 122, 224, 215, 106, 125, 124, 214, 184, 33, 56, 116, 215, 227, 30, 129, 78, 220, 46, 56, 116, 107, 149, 193, 50, 187, 53, 170, 69, 133, 110, 102, 5, 205, 43, 46, 134, 79, 16, 45, 191, 241, 215, 174, 251, 171, 219, 154, 199, 218, 166, 56, 61, 252, 71, 71, 9, 65, 85, 102, 173, 133, 195, 113, 223, 167, 141, 234, 154, 135, 62, 155, 97, 12, 16, 126, 8, 160, 122, 203, 140, 135, 74, 67, 2, 123, 204, 116, 55, 7, 47, 142, 103, 179, 162, 76, 131, 178, 181, 132, 153, 241, 112, 144, 250, 11, 226, 181 }, "Client" },
                    { "smartbus@gmail.com", new DateTime(2025, 2, 9, 23, 5, 47, 710, DateTimeKind.Local).AddTicks(948), true, "Smart Bus", new byte[] { 164, 27, 161, 121, 123, 86, 180, 146, 43, 209, 32, 198, 241, 96, 130, 229, 59, 183, 232, 206, 216, 157, 58, 130, 235, 45, 59, 0, 218, 228, 240, 145, 237, 213, 209, 94, 81, 151, 115, 201, 107, 109, 59, 96, 18, 3, 173, 239, 199, 134, 147, 208, 11, 212, 196, 245, 67, 147, 207, 155, 59, 7, 80, 84 }, new byte[] { 62, 92, 142, 22, 84, 6, 228, 134, 93, 219, 42, 219, 204, 196, 135, 95, 254, 202, 88, 235, 72, 179, 88, 77, 123, 162, 190, 36, 165, 233, 15, 125, 176, 7, 122, 3, 227, 131, 195, 105, 215, 183, 241, 117, 84, 104, 140, 189, 106, 69, 187, 132, 131, 192, 123, 246, 93, 153, 175, 33, 214, 139, 254, 106, 3, 0, 6, 171, 250, 230, 48, 163, 29, 207, 195, 105, 40, 167, 102, 123, 232, 111, 148, 221, 126, 108, 223, 215, 5, 243, 6, 248, 195, 254, 17, 181, 143, 210, 100, 51, 183, 136, 42, 166, 129, 173, 70, 217, 77, 198, 91, 128, 156, 132, 30, 7, 86, 39, 231, 52, 149, 186, 188, 147, 97, 212, 186, 127 }, "TransportOperator" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Contact", "DOB", "Email", "Gender", "IsDiabled", "Name" },
                values: new object[] { 1, "+911234567890", new DateOnly(2002, 7, 11), "anuraj@gmail.com", "Male", false, "Anuraj" });

            migrationBuilder.InsertData(
                table: "TransportOperators",
                columns: new[] { "Id", "Contact", "Email", "Name" },
                values: new object[] { 1, "+919876543210", "smartbus@gmail.com", "SmartBus" });

            migrationBuilder.InsertData(
                table: "Buses",
                columns: new[] { "Id", "BusNo", "BusType", "OperatorId", "TotalSeats" },
                values: new object[] { 1, "TN01AB1234", "AC Sleeper", 1, 40 });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "Id", "ArrivalTime", "BusId", "BusRouteId", "DepartureTime", "Price" },
                values: new object[] { 1, new DateTime(2025, 2, 10, 10, 30, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2025, 2, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), 700m });

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
