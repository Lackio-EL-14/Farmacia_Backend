using Farmacia.Application.Services;
using Farmacia.Core.Interfaces;
using Farmacia.Infrastructure.Data;
using Farmacia.Infrastructure.Mappings;
using Farmacia.Infrastructure.Repositories;
using Farmacia.Validations.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

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

            //En produccion los secrets vendran de entornos globales

            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<FarmaciaContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Inyección de UnitOfWork genérico para todas las tablas y entidades. Dejamos de lado los repositorios específicos.
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<VentaService>();
            builder.Services.AddValidatorsFromAssemblyContaining<VentaValidator>();


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
                options.SwaggerDoc("v1", new ()
                {
                    Title = "Farmacia API",
                    Version = "v1",
                    Description = "Documentacion de la API para la gestión de una farmacia.",
                    Contact = new ()
                    {
                        Name = "La catolicoaaaa",
                        Email = "PruebaDeApiFarm@gmail.com"
                    }

                });
                var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var  xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                options.IncludeXmlComments(xmlFilePath);   
                options.EnableAnnotations();
            });

            

            var app = builder.Build();

            //Usar swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Farmacia API v1");
                    options.RoutePrefix = string.Empty; 
                });
            }
         

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
