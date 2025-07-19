using Microsoft.Extensions.Caching.Memory;
using Rinha2025_Api.Contratos;
using Rinha2025_Api.Domain;
using Rinha2025_Api.Infra;

namespace Rinha2025_Api.UseCases
{
    public class OrquestradorPagamentoUseCase : IOrquestradorPagamentos
    {
        IMemoryCache _memoryCache;
        IHttpFacade _httpFacade;
        IExecutaPagamentosUseCase _executaPagamentosUseCase;

        public OrquestradorPagamentoUseCase(IMemoryCache memoryCache, IHttpFacade httpFacade, IExecutaPagamentosUseCase executaPagamentosUseCase )
        {
            _memoryCache = memoryCache;
            _httpFacade = httpFacade;
            _executaPagamentosUseCase = executaPagamentosUseCase;
        }

        public async Task Processa(PaymentInput paymentInput)
        {
            if (!_memoryCache.TryGetValue(Constantes.Constantes.CacheKey, out var result))
            {

                HttpRequestMessage requestMessageHealthCheck = new HttpRequestMessageBuilder()
                    .AddUrl("HEALTHCHECK")
                    .AddMethod(HttpMethod.Get)                    
                    .Build();

                HttpResponseMessage resultado = await _httpFacade.ExecutaTarefa(requestMessageHealthCheck);

                //if (!resultado.IsSuccessStatusCode())
                //{

                //}
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(5));

                _memoryCache.Set(Constantes.Constantes.CacheKey, "1", cacheEntryOptions);
            }

            string TipoPaymentProcessor = "PRINCIPAL";
        


            HttpRequestMessage requestMessagePagamento = new HttpRequestMessageBuilder()
                .AddUrl(TipoPaymentProcessor)
                .AddMethod(HttpMethod.Post)
                .Build();

            _executaPagamentosUseCase.Processa(requestMessagePagamento);



            Task.FromResult(0); 
        }
    }
}
