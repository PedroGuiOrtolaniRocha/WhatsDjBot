
using Microsoft.EntityFrameworkCore;
using WhatsDjBotAPI.Repositorys;

namespace WhatsDjBotAPI
{
    public class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddDbContext<DbEntity>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));
      
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IMusicRepository, MusicRepository>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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
