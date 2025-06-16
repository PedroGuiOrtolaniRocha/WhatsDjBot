
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
            Console.WriteLine($"WhatsDjBotAPI is starting...;\nConnectionString : {builder.Configuration.GetConnectionString("FamiliaUnip")}");
            builder.Services.AddDbContext<DbEntity>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("FamiliaUnip")));
        
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IMusicRepository, MusicRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            
            app.UseSwagger();
            app.UseSwaggerUI(c => { 
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WhatsDjBotAPI v1");
                c.RoutePrefix = "";
            });
            

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
