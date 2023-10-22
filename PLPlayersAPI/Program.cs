using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PLPlayersAPI.Data;
using PLPlayersAPI.Services.ClubServices;
using PLPlayersAPI.Services.NationalityServices;
using PLPlayersAPI.Services.PlayerServices;
using PLPlayersAPI.Services.PositionServices;

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
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(builder.Configuration.GetConnectionString("PremierLeagueDatabase")));

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddScoped<IPlayerService, PlayerService>();
            builder.Services.AddScoped<IClubService, ClubService>();
            builder.Services.AddScoped<IPositionService, PositionService>();
            builder.Services.AddScoped<INationalityService, NationalityService>();

            var app = builder.Build();

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