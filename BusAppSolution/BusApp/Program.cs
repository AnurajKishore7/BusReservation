using System.Text;
using BusApp.Models;
using BusApp.Repositories.Implementations;
using BusApp.Repositories.Implementations.BookingManage;
using BusApp.Repositories.Implementations.BusRouteManage;
using BusApp.Repositories.Implementations.ClientManage;
using BusApp.Repositories.Implementations.TransOp;
using BusApp.Repositories.Implementations.TripManage;
using BusApp.Repositories.Interfaces;
using BusApp.Repositories.Interfaces.BookingManage;
using BusApp.Repositories.Interfaces.BusRouteManage;
using BusApp.Repositories.Interfaces.ClientManage;
using BusApp.Repositories.Interfaces.TransOp;
using BusApp.Repositories.Interfaces.TripManage;
using BusApp.Services.Implementations;
using BusApp.Services.Implementations.BookingManage;
using BusApp.Services.Implementations.BusRouteManage;
using BusApp.Services.Implementations.ClientManage;
using BusApp.Services.Implementations.TransOp;
using BusApp.Services.Implementations.TripManage;
using BusApp.Services.Interfaces;
using BusApp.Services.Interfaces.BookingManage;
using BusApp.Services.Interfaces.BusRouteManage;
using BusApp.Services.Interfaces.ClientManage;
using BusApp.Services.Interfaces.TransOp;
using BusApp.Services.Interfaces.TripManage;
using BusReservationApp.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BusApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            var environment = builder.Environment;

            // Add services to the container
            #region NewtonsoftJson

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            #endregion

            // Add Swagger/OpenAPI
            #region Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            #endregion

            // Database Context Setup
            #region Database
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Connection string 'DefaultConnection' is missing in appsettings.json.");
            }

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
            #endregion

            // Repository Layer Dependency Injection
            #region Repositories
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            builder.Services.AddScoped<IClientRepo, ClientRepo>();
            builder.Services.AddScoped<IRepository<Client, int>, ClientRepo>();
            builder.Services.AddScoped<ITransportOperatorRepo, TransportOperatorRepo>();
            builder.Services.AddScoped<IRepository<TransportOperator, int>, TransportOperatorRepo>();
            builder.Services.AddScoped<ITransOpManageRepo, TransOpManageRepo>();
            builder.Services.AddScoped<ITransOpRepo, TransOpRepo>();
            builder.Services.AddScoped<IBusRouteRepo, BusRouteRepo>();
            builder.Services.AddScoped<ITripRepo, TripRepo>();
            builder.Services.AddScoped<IClientManageRepo, ClientManageRepo>();
            //BookingService
            builder.Services.AddScoped<IBookingRepo, BookingRepo>();
            builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
            builder.Services.AddScoped<ITicketPassengerRepo, TicketPassengerRepo>();

            #endregion

            // Service Layer Dependency Injection
            #region Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();     
            builder.Services.AddScoped<ITransOpManageService, TransOpManageService>();
            builder.Services.AddScoped<ITransOpService, TransOpService>();
            builder.Services.AddScoped<IBusRouteService, BusRouteService>();
            builder.Services.AddScoped<ITripService, TripService>();
            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            #endregion

            // JWT Authentication & Authorization
            #region Authentication & Authorization
            var jwtSecret = configuration["Jwt:Secret"];

            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new ArgumentNullException("Jwt:Secret is missing in configuration.");
            }

            var key = Encoding.UTF8.GetBytes(jwtSecret);
            //var key = Encoding.UTF8.GetBytes("pgh4y1gatINIryRxcfPv1Thrlmxl4gWtCFaQkqUaNAY=");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        //ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = false,
                       // ValidAudience = configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();
            #endregion

            var app = builder.Build();

            // Middleware 
            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();

        }
    }
}
