using Farmacia.Api.Extensions;
using Farmacia.Application.Services;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.Data;
using Farmacia.Infrastructure.Mappings;
using Farmacia.Infrastructure.Repositories;
using Farmacia.Infrastructure.Repositories.Dapper;
using Farmacia.Validations.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Farmacia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //configurar secretos de usuario
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            // IMPORTANTE: Configurar el puerto de Render
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

            //En produccion los secrets vendran de entornos globales
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<FarmaciaContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Inyección de UnitOfWork genérico para todas las tablas y entidades.
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<VentaService>();
            builder.Services.AddValidatorsFromAssemblyContaining<VentaValidator>();
            builder.Services.AddScoped<IDapperProductoRepository, DapperProductoRepository>();
            builder.Services.AddScoped<IVentaDapperRepository, VentaDapperRepository>();
            builder.Services.AddScoped<IDetalleVentaDapperRepository, DetalleVentaDapperRepository>();
            builder.Services.AddScoped<IFacturaDapperRepository, FacturaDapperRepository>();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            //Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);

                options.SwaggerDoc("v1", new()
                {
                    Title = "Farmacia API",
                    Version = "v1",
                    Description = "Documentacion de la API para la gestión de una farmacia.",
                    Contact = new()
                    {
                        Name = "La catolicoaaaa",
                        Email = "PruebaDeApiFarm@gmail.com"
                    }
                });

                var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                if (File.Exists(xmlFilePath))
                {
                    options.IncludeXmlComments(xmlFilePath);
                }
                options.EnableAnnotations();
            });

            var app = builder.Build();

            // CAMBIO IMPORTANTE: Habilitar Swagger en producción también
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Farmacia API v1");
                options.RoutePrefix = string.Empty; // Swagger en la raíz
            });

            app.UseMiddleware<Farmacia.Api.Middlewares.GlobalExceptionMiddleware>();

            // COMENTADO: HTTPS redirect no funciona bien en Render
            // app.UseHttpsRedirection();

            app.UseGlobalExceptionMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}