
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Rpp.Examen.JavierSalazar.Application.Interfaces;
using Rpp.Examen.JavierSalazar.Domain.Interfaces;
using Rpp.Examen.JavierSalazar.EFCore;
using Rpp.Examen.JavierSalazar.Infrastructure;
using System.Data;
using System.Text;

namespace LibeyTechnicalTestAPI.SConfigurator
{
    public static class ServiceConfiguration
    {
        // Configuración de los servicios de la aplicación
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ITrabajadorRepository, TrabajadorRepository>();
            services.AddScoped<TrabajadorService>();
        }

        // Configuración de Swagger
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi API Local", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Ingrese 'Bearer' seguido de un espacio y su token JWT.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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
                        new string[] {}
                    }
                });
            });
        }

        // Configuración de CORS
        public static void AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }

        // Configuración de autenticación JWT
        //public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var secretKey = configuration["JwtSettings:SecretKey"];
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //        .AddJwtBearer(options =>
        //        {
        //            options.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateIssuerSigningKey = true,
        //                IssuerSigningKey = key,
        //                ValidateIssuer = false,
        //                ValidateAudience = false,
        //                ValidateLifetime = true,
        //                ClockSkew = TimeSpan.Zero
        //            };
        //        });
        //}
    }
}
