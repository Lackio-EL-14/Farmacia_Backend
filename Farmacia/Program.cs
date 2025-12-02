using Farmacia.Api.Extensions;
using Farmacia.Application.Services;
using Farmacia.Core.Interfaces;
using Farmacia.Core.Options;
using Farmacia.Infrastructure.Data;
using Farmacia.Infrastructure.Mappings;
using Farmacia.Infrastructure.Repositories;
using Farmacia.Infrastructure.Repositories.Dapper;
using Farmacia.Infrastructure.Services;
using Farmacia.Validations.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Farmacia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Configuración de Secretos
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            // 2. Configuración de Puerto para Render (¡ESTO ESTÁ PERFECTO!)
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

            // 3. Base de Datos
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<FarmaciaContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // 4. AutoMapper y Validadores
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddValidatorsFromAssemblyContaining<VentaValidator>();

            // 5. Configuración de Seguridad (Password y JWT)
            builder.Services.Configure<PasswordOptions>(
                builder.Configuration.GetSection("PasswordOptions"));

            builder.Services.AddSingleton<IPasswordService, PasswordService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    ValidAudience = builder.Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]!)
                    )
                };
            });

            // 6. Inyección de Dependencias (Servicios y Repositorios)
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<VentaService>();
            builder.Services.AddScoped<CompraService>();
            builder.Services.AddScoped<KardexService>();
            builder.Services.AddScoped<CajaService>(); // Agregamos el servicio de Caja que creamos

            builder.Services.AddScoped<IDapperProductoRepository, DapperProductoRepository>();
            builder.Services.AddScoped<IVentaDapperRepository, VentaDapperRepository>();
            builder.Services.AddScoped<IDetalleVentaDapperRepository, DetalleVentaDapperRepository>();
            builder.Services.AddScoped<IFacturaDapperRepository, FacturaDapperRepository>();

            // 7. CORS (¡CRÍTICO PARA QUE FUNCIONE EN LA WEB!)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            // 8. Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);
                options.SwaggerDoc("v1", new()
                {
                    Title = "Farmacia API",
                    Version = "v1",
                    Description = "API Semi-Profesional Farmacia UCB",
                    Contact = new() { Name = "Tu Nombre", Email = "tu@email.com" }
                });

                // Configuración para que Swagger soporte el candadito de autorización (Bearer Token)
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Ingrese 'Bearer [token]'"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();



            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Farmacia API v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseMiddleware<Farmacia.Api.Middlewares.GlobalExceptionMiddleware>();

            app.UseCors("AllowAll"); // Habilitar CORS

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}