using Rinha2025_Api.Contratos;

namespace Rinha2025_Api.DependencyInjection
{
    public static class Container
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddHttpClient<IHttpFacade, IHttpFacade>(client =>
                client.BaseAddress = new Uri("http://payment-processor-default:8080")
                )
                .AddHttpMessageHandler();

            return services;
        }

    }
}
