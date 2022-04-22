using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ClientContext;

public static class ClientContextExtensions
{
    public static IServiceCollection AddClientContextAccessor(this IServiceCollection services, Action<ClientContextOptions> configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        var options = new ClientContextOptions();
        configure?.Invoke(options);

        services.AddSingleton(options);
        services.AddSingleton<IClientContextAccessor, ClientContextAccessor>();

        return services;
    }

    public static IApplicationBuilder UseClientContext(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        if (app.ApplicationServices.GetService(typeof(IClientContextAccessor)) is null)
        {
            throw new InvalidOperationException("Unable to find the required services.");
        }

        return app.UseMiddleware<ClientContextMiddleware>();
    }

    public static void AddClientContextOperationFilter(this SwaggerGenOptions swaggerGenOptions)
        => swaggerGenOptions.OperationFilter<ClientContextOperationFilter>();
}
