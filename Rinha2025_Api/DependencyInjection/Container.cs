using Rinha2025_Api.Contratos;
using Rinha2025_Api.UseCases;

namespace Rinha2025_Api.DependencyInjection
{
    public static class Container
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddScoped<IConsultaPagamentosUseCase, ConsultaPagamentosUseCase>();
            services.AddScoped<IOrquestradorPagamentos, ExecutaPagamentoUseCase>();


            services.AddHttpClient<IHttpFacade, IHttpFacade>(client =>
                client.BaseAddress = new Uri("http://payment-processor-default:8080")
                );
               // .AddHttpMessageHandler();

            return services;
        }

    }
}
