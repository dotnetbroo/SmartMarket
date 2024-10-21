using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartMarket.Data.IRepositories;
using SmartMarket.Data.Repositories;
using SmartMarket.Service.Interfaces.Accounts;
using SmartMarket.Service.Interfaces.Cards;
using SmartMarket.Service.Interfaces.Categories;
using SmartMarket.Service.Interfaces.CencelOrders;
using SmartMarket.Service.Interfaces.Commons;
using SmartMarket.Service.Interfaces.ContrAgents;
using SmartMarket.Service.Interfaces.Kassas;
using SmartMarket.Service.Interfaces.Korzinkas;
using SmartMarket.Service.Interfaces.Orders;
using SmartMarket.Service.Interfaces.PartnerProducts;
using SmartMarket.Service.Interfaces.Partners;
using SmartMarket.Service.Interfaces.Products;
using SmartMarket.Service.Interfaces.TolovUsuli;
using SmartMarket.Service.Interfaces.Users;
using SmartMarket.Service.Services.Accounts;
using SmartMarket.Service.Services.Cards;
using SmartMarket.Service.Services.Categories;
using SmartMarket.Service.Services.CencelOrders;
using SmartMarket.Service.Services.Commons;
using SmartMarket.Service.Services.ContrAgents;
using SmartMarket.Service.Services.Kassas;
using SmartMarket.Service.Services.Korzinkas;
using SmartMarket.Service.Services.Orders;
using SmartMarket.Service.Services.PartnerProducts;
using SmartMarket.Service.Services.Partners;
using SmartMarket.Service.Services.Products;
using SmartMarket.Service.Services.TolovUsul;
using SmartMarket.Service.Services.Users;
using System.Reflection;
using System.Text;

namespace SmartMarket.Api.Extensions;

public static class ServiceExtensions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICancelOrderService, CancelOrderService>();
        services.AddScoped<IContrAgentService, ContrAgentService>();
        services.AddScoped<ITolovService, TolovService>();
        services.AddScoped<IKassaService, KassaService>();
        services.AddScoped<IKorzinkaService, KorzinkaService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductStoryService, ProductStoryService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPartnerProductService, PartnerProductService>();
        services.AddScoped<IPartnerService, PartnerService>();
        services.AddScoped<IPartnerTolovService, PartnerTolovService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ITolovUsuliService, TolovUsuliService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWorkersPaymentService, WorkersPaymentService>();
        // Repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });
    }

    public static void AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartMarket.Api", Version = "v1" });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
        });


    }

    public static void AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var Key = Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });
    }

}
