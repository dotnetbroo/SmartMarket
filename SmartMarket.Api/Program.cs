
using SmartMarket.Data.DbContexts;
using Newtonsoft.Json;
using System;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Service.Mappers;
using SmartMarket.Api.Extensions;
using SmartMarket.Service.Commons.Helpers;
using Serilog;

namespace SmartMarket.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Set Database Configuration
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

        builder.Services.AddCustomServices();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAutoMapper(typeof(MapperProfile));

        // CORS
        builder.Services.ConfigureCors();

        // swagger set up
        builder.Services.AddSwaggerService();

        // JWT service
        builder.Services.AddJwtService(builder.Configuration);

        // Logger
        var logger = new LoggerConfiguration()
          .ReadFrom.Configuration(builder.Configuration)
          .Enrich.FromLogContext()
          .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        /// Fix the Cycle
        builder.Services.AddControllers()
             .AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
             });




        var app = builder.Build();

        WebHostEnviromentHelper.WebRootPath = Path.GetFullPath("wwwroot");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("AllowAll");

        app.UseStaticFiles();
        app.UseHttpsRedirection();

        // Init accessor
        app.InitAccessor();

        app.UseHttpsRedirection();


        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}