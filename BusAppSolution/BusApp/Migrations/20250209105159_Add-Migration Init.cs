using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BusApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMigrationInit : Migration
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
                    { "admin@gmail.com", new DateTime(2025, 2, 9, 16, 21, 59, 120, DateTimeKind.Local).AddTicks(6041), true, "Super Admin", new byte[] { 40, 48, 20, 199, 224, 166, 9, 212, 97, 205, 247, 145, 225, 230, 179, 69, 207, 111, 141, 171, 69, 143, 163, 188, 226, 227, 51, 82, 91, 54, 185, 245, 120, 174, 41, 168, 84, 255, 187, 192, 15, 89, 133, 70, 158, 78, 249, 45, 39, 22, 10, 243, 51, 236, 109, 249, 245, 213, 126, 228, 223, 153, 224, 196 }, new byte[] { 135, 59, 163, 43, 50, 117, 145, 249, 199, 246, 210, 225, 221, 188, 195, 106, 224, 175, 145, 165, 119, 147, 166, 125, 71, 132, 80, 132, 114, 248, 87, 233, 80, 176, 114, 235, 215, 121, 100, 214, 203, 192, 143, 194, 0, 153, 105, 149, 63, 229, 186, 85, 82, 237, 197, 173, 133, 38, 1, 81, 91, 173, 239, 125, 60, 104, 35, 124, 54, 187, 151, 199, 26, 211, 248, 199, 164, 154, 173, 163, 253, 25, 188, 162, 103, 111, 122, 254, 239, 138, 72, 184, 201, 175, 151, 89, 47, 160, 222, 112, 179, 16, 106, 179, 145, 248, 103, 187, 21, 249, 18, 123, 95, 253, 209, 247, 246, 204, 231, 20, 180, 165, 208, 78, 182, 180, 167, 71 }, "SuperAdmin" },
                    { "anuraj@gmail.com", new DateTime(2025, 2, 9, 16, 21, 59, 120, DateTimeKind.Local).AddTicks(6046), true, "Anuraj", new byte[] { 218, 111, 19, 127, 207, 248, 142, 20, 193, 131, 255, 145, 172, 128, 228, 179, 120, 183, 161, 161, 250, 95, 178, 150, 112, 104, 245, 187, 253, 250, 147, 51, 75, 155, 46, 96, 102, 39, 30, 136, 232, 78, 22, 222, 15, 10, 55, 12, 147, 13, 95, 179, 185, 48, 2, 164, 235, 99, 22, 162, 67, 214, 167, 229 }, new byte[] { 174, 28, 43, 202, 63, 240, 150, 235, 136, 60, 114, 38, 238, 192, 165, 121, 94, 18, 6, 28, 22, 237, 181, 89, 29, 122, 165, 150, 6, 231, 159, 48, 62, 160, 186, 50, 26, 130, 120, 140, 39, 192, 246, 177, 154, 167, 36, 193, 22, 143, 30, 58, 237, 19, 227, 109, 238, 105, 122, 173, 80, 20, 71, 33, 77, 218, 118, 218, 159, 2, 250, 166, 96, 127, 231, 122, 65, 240, 107, 216, 5, 21, 191, 224, 226, 158, 31, 100, 141, 97, 248, 8, 126, 127, 115, 37, 97, 36, 49, 43, 169, 194, 138, 8, 251, 34, 38, 148, 91, 61, 227, 142, 48, 89, 214, 132, 128, 93, 203, 69, 217, 197, 126, 46, 127, 71, 171, 109 }, "Client" },
                    { "smartbus@gmail.com", new DateTime(2025, 2, 9, 16, 21, 59, 120, DateTimeKind.Local).AddTicks(6044), true, "Smart Bus", new byte[] { 162, 55, 53, 101, 63, 6, 77, 165, 80, 175, 63, 2, 68, 92, 167, 186, 52, 47, 242, 186, 123, 78, 23, 7, 150, 16, 132, 161, 188, 180, 83, 154, 170, 60, 26, 100, 127, 88, 57, 141, 4, 117, 246, 78, 140, 233, 46, 84, 144, 86, 45, 88, 252, 222, 82, 5, 219, 15, 138, 94, 119, 248, 183, 209 }, new byte[] { 244, 95, 58, 227, 146, 5, 9, 57, 216, 241, 254, 144, 156, 131, 233, 29, 36, 5, 199, 68, 219, 57, 105, 6, 137, 99, 99, 242, 104, 236, 164, 13, 181, 189, 178, 195, 204, 137, 119, 189, 77, 193, 228, 46, 152, 228, 229, 117, 197, 190, 229, 30, 172, 147, 190, 228, 106, 22, 6, 93, 80, 173, 3, 234, 11, 26, 84, 34, 169, 171, 222, 46, 102, 148, 174, 220, 205, 184, 22, 17, 6, 189, 211, 146, 89, 70, 109, 67, 53, 146, 126, 62, 45, 2, 97, 153, 194, 162, 121, 149, 154, 31, 106, 202, 46, 174, 177, 132, 167, 100, 9, 237, 79, 190, 249, 232, 27, 99, 46, 192, 57, 89, 27, 142, 35, 233, 109, 78 }, "TransportOperator" }
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
