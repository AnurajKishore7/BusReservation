
using System.Configuration;
using BusApp.Models;
using BusApp.Repositories.Implementations;
using BusApp.Repositories.Interfaces;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region Context
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Connection string 'DefaultConnection' is not set in appsettings.json.");
            }

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion

            #region Repositories
            // Registering generic repository
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            // Registering specific repositories
            builder.Services.AddScoped<IClientRepo, ClientRepo>();
            builder.Services.AddScoped<IRepository<Client, int>, ClientRepo>();  // Explicit mapping

            builder.Services.AddScoped<ITransportOperatorRepo, TransportOperatorRepo>();
            builder.Services.AddScoped<IRepository<TransportOperator, int>, TransportOperatorRepo>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
