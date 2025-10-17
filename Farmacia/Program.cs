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

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
