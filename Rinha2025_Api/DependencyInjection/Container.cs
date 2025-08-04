using Rinha2025_Api.Contratos;
using Rinha2025_Api.Domain;
using Rinha2025_Api.Infra;
using Rinha2025_Api.UseCases;

namespace Rinha2025_Api.DependencyInjection
{
    public static class Container
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddScoped<IConsultaPagamentosUseCase, ConsultaPagamentosUseCase>();
            services.AddScoped<IExecutaPagamentosUseCase, ExecutaPagamentosUseCase>();
            services.AddScoped<IOrquestradorPagamentos, OrquestradorPagamentos>();

            services.AddMemoryCache();


            services.AddHttpClient<IHttpFacade<PaymentResponse>, HttpFacade<PaymentResponse>>();

            services.AddHttpClient<IHttpFacade<HealthCheck>, HttpFacade<HealthCheck>>();

            services.AddHttpClient<IHttpFacade<PaymentProcessor>, HttpFacade<PaymentProcessor>>();

            // .AddHttpMessageHandler();

            return services;
        }

    }
}
