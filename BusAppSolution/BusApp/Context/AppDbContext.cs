using System.Security.Cryptography;
using System.Text;
using BusApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BusReservationApp.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TransportOperator> TransportOperators { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusRoute> BusRoutes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<TicketPassenger> TicketPassengers { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Email);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasColumnType("nvarchar(255)");

            // User - Transport Operator (One-to-One)
            modelBuilder.Entity<TransportOperator>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<TransportOperator>()
                .HasOne(to => to.User)
                .WithOne(u => u.TransportOperator)
                .HasForeignKey<TransportOperator>(to => to.Email)
                .OnDelete(DeleteBehavior.Cascade);


            // User - Client (One-to-One)
            modelBuilder.Entity<Client>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Client>()
                .HasOne(c => c.User)
                .WithOne(u => u.Client)
                .HasForeignKey<Client>(c => c.Email)
                .OnDelete(DeleteBehavior.Cascade);


            // Transport Operator - Buses (One-to-Many)
            modelBuilder.Entity<Bus>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Bus>()
                .HasOne(b => b.TransportOperator)
                .WithMany(to => to.Buses)
                .HasForeignKey(b => b.OperatorId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Bus>()
                .Property(b => b.OperatorId)
                .IsRequired();


            // Routes - Trips (One-to-Many)
            modelBuilder.Entity<Trip>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.BusRoute)
                .WithMany(r => r.Trips)
                .HasForeignKey(t => t.BusRouteId)
                .OnDelete(DeleteBehavior.Cascade);


            // Buses - Trips (One-to-Many)
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Bus)
                .WithMany(b => b.Trips)
                .HasForeignKey(t => t.BusId)
                .OnDelete(DeleteBehavior.Cascade);

            // Clients - Bookings (One-to-Many)
            modelBuilder.Entity<Booking>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Restrict);


            // Trips - Bookings (One-to-Many)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Trip)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bookings - TicketPassengers (One-to-Many)
            modelBuilder.Entity<TransportOperator>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<TicketPassenger>()
                .HasOne(tp => tp.Booking)
                .WithMany(b => b.TicketPassengers)
                .HasForeignKey(tp => tp.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bookings - Payments (One-to-One)
            modelBuilder.Entity<Payment>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);


            //SQL Column datatype
            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .HasColumnType("varbinary(max)");

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordSalt)
                .HasColumnType("varbinary(max)");

            // Hash password
            using var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("admin@123"));
            var passwordSalt = hmac.Key;

            using var hmac2 = new HMACSHA512();
            var passwordHash2 = hmac2.ComputeHash(Encoding.UTF8.GetBytes("smartbus@123"));
            var passwordSalt2 = hmac2.Key;

            using var hmac3 = new HMACSHA512();
            var passwordHash3 = hmac3.ComputeHash(Encoding.UTF8.GetBytes("anuraj@123"));
            var passwordSalt3 = hmac3.Key;

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Email = "admin@gmail.com",
                    Name = "Super Admin",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = "SuperAdmin",
                    IsApproved = true,
                    CreatedAt = DateTime.Now

                },
                new User
                {
                    Email = "smartbus@gmail.com",
                    Name = "Smart Bus",
                    PasswordHash = passwordHash2,
                    PasswordSalt = passwordSalt2,
                    Role = "TransportOperator",
                    IsApproved = true,
                    CreatedAt = DateTime.Now
                },
                new User
                {
                    Email = "anuraj@gmail.com",
                    Name = "Anuraj",
                    PasswordHash = passwordHash3,
                    PasswordSalt = passwordSalt3,
                    Role = "Client",
                    IsApproved = true,
                    CreatedAt = DateTime.Now
                }
            );

            // Seed Client
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = 1,
                    Email = "anuraj@gmail.com",
                    Name = "Anuraj",
                    DOB = new DateOnly(2002, 07, 11),
                    Gender = "Male",
                    Contact = "+911234567890",
                    IsDiabled = false
                }
            );

            // Seed Transport Operator
            modelBuilder.Entity<TransportOperator>().HasData(
                new TransportOperator
                {
                    Id = 1,
                    Email = "smartbus@gmail.com",
                    Name = "SmartBus",
                    Contact = "+919876543210"
                }
            );

            //Seed Bus
            modelBuilder.Entity<Bus>().HasData(
                new Bus
                {
                    Id = 1,
                    BusNo = "TN01AB1234",
                    OperatorId = 1,
                    BusType = "AC Sleeper",
                    TotalSeats = 40
                }
            );

            //Seed BusRoute
            modelBuilder.Entity<BusRoute>().HasData(
               new BusRoute { Id = 1, Source = "Chennai", Destination = "Kanyakumari", EstimatedDuration = "12:30", Distance = 750 }
           );

            // Seeding Trip Data
            modelBuilder.Entity<Trip>().HasData(
                new Trip { Id = 1, BusRouteId = 1, BusId = 1, DepartureTime = new DateTime(2025, 2, 10, 8, 0, 0), ArrivalTime = new DateTime(2025, 2, 10, 10, 30, 0), Price = 700 }
            );


            base.OnModelCreating(modelBuilder);

        }
    }
}
