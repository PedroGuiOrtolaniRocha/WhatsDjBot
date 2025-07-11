
using Microsoft.EntityFrameworkCore;
using WhatsDjBotAPI.Repositorys;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WhatsDjBotAPI
{
    public class Program
    {
        public static void Main()
        {

            var builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddDbContext<DbEntity>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("FamiliaUnip")));
        
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IMusicRepository, MusicRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();

            builder.Services.AddDistributedMemoryCache();

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
            Console.Clear();
            Console.WriteLine("\x1b[32m           _____ _____     ____        _ _            \r\n     /\\   |  __ \\_   _|   / __ \\      | (_)           \r\n    /  \\  | |__) || |    | |  | |_ __ | |_ _ __   ___ \r\n   / /\\ \\ |  ___/ | |    | |  | | '_ \\| | | '_ \\ / _ \\\r\n  / ____ \\| |    _| |_   | |__| | | | | | | | | |  __/\r\n /_/    \\_\\_|   |_____|   \\____/|_| |_|_|_|_| |_|\\___|\r\n                                                      \r\n                                                      \x1b[0m");
        }
    }
}
