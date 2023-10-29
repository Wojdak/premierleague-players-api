using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PLPlayersAPI.Data;
using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Services.ClubServices;
using PLPlayersAPI.Services.NationalityServices;
using PLPlayersAPI.Services.PlayerServices;
using PLPlayersAPI.Services.PositionServices;
using PLPlayersAPI.Services.UserServices;
using PLPlayersAPI.Validators;
using System.Security.Claims;
using System.Text;

namespace PLPlayersAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication with JWT Token",
                    Type = SecuritySchemeType.Http
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(builder.Configuration.GetConnectionString("PremierLeagueDatabase")));

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IClubService, ClubService>();
            builder.Services.AddScoped<IPositionService, PositionService>();
            builder.Services.AddScoped<INationalityService, NationalityService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IValidator<UserDTO>, UserValidator>();
            builder.Services.AddScoped<IValidator<Nationality>, NationalityValidator>();
            builder.Services.AddScoped<IValidator<Club>, ClubValidator>();
            builder.Services.AddScoped<IValidator<Position>, PositionValidator>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorPolicy", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "Administrator"));
            });

            builder.Services.AddHealthChecks();
            
            var app = builder.Build();

            app.MapHealthChecks("/health");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}