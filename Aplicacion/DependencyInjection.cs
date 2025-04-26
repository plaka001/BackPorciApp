using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Aplicacion;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            //configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            //configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));

        });
        //services.AddScoped<Crear>, RegisterUsuarioCommandValidator>();
        //services.AddScoped<IValidator<ConfirmarCorreoCommand>, ConfirmarCorreoCommandValidator>();
        //services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}
