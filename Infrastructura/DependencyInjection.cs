using Aplicacion.Abstractions.Data;
using Dominio.Abstractions;
using Dominio.Animales.Repository;
using Dominio.EspacioFisicos.Repository;
using Dominio.granjas.repository;
using Dominio.Salud.Repository;
using Infrastructura.Data;
using Infrastructura.Repositorios;
using Infrastructura.Repositorios.Salud;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructura;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
       this IServiceCollection services,
       IConfiguration configuration
       )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
             ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IPlanSanitarioRepository, PlanSanitarioRepository>();
        services.AddScoped<IGranjaRepository, GranjaRepository>();
        services.AddScoped<IEspacioFisicoRepository, EspacioFisicoRepository>();
        services.AddScoped<IAnimalesRepository, AnimalesRepository>();
        services.AddScoped<IDapperWrapper, DapperWrapper>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));

        return services;
    }
}