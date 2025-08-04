using Microsoft.Extensions.Caching.Memory;
using Rinha2025_Api.Contratos;
using Rinha2025_Api.Domain;
using Rinha2025_Api.Helpers;
using Rinha2025_Api.Infra;
using System.Text.Json;

namespace Rinha2025_Api.UseCases
{
    public class OrquestradorPagamentos : IOrquestradorPagamentos
    {
        IMemoryCache _memoryCache;
        IHttpFacade<HealthCheck> _httpFacade;
        IExecutaPagamentosUseCase _executaPagamentosUseCase;


        public OrquestradorPagamentos(
            IMemoryCache memoryCache, 
            IHttpFacade<HealthCheck> httpFacade, 
            IExecutaPagamentosUseCase executaPagamentosUseCase 
            )
        {
            _memoryCache = memoryCache;
            _httpFacade = httpFacade;
            _executaPagamentosUseCase = executaPagamentosUseCase;
        }

        public async Task Processa(PaymentInput paymentInput)
        {
            string urlProcessor = string.Empty;

            if (!_memoryCache.TryGetValue(Constantes.Constantes.CacheKey, out var result))
            {

                HttpRequestMessage requestMessageHealthCheck = new HttpRequestMessageBuilder()
                    //.AddUrl("http://payment-processor:8001/payments/service-health")
                    .AddUrl("http://localhost:8001/payments/service-health")
                    .AddMethod(HttpMethod.Get)                    
                    .Build();

                HealthCheck healthCheck = await _httpFacade.ExecutaTarefa(requestMessageHealthCheck);


                paymentInput.RequestedAt = DateTime.UtcNow.ToString();

                if (!healthCheck.Failing && healthCheck.MinResponseTime > 5)
                {
                    //urlProcessor = "http://payment-processor:8001/payments";
                    urlProcessor = "http://localhost:8001/payments";
                }
                else
                {
                    //urlProcessor = "http://payment-processor:8002/payments";
                    urlProcessor = "http://localhost:8002/payments";
                }

                HttpRequestMessage request = new HttpRequestMessageBuilder()
                    .AddUrl(urlProcessor)
                    //.AddBody(JsonSerializer.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                    .AddBody(JsonSerializerHelper<PaymentProcessorInput>.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                    .AddMethod(HttpMethod.Post)
                    .Build();

                var respostaProcessamento = await _executaPagamentosUseCase.Processa(request);

                                

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(5));

                _memoryCache.Set(Constantes.Constantes.CacheKey, "1", cacheEntryOptions);
            }
            else
            {

            }




            Task.FromResult(0); 
        }


        private PaymentProcessorInput ConverteEmPaymentProcessorInput(PaymentInput paymentInput)
        {
            return new PaymentProcessorInput
            {
                Amount = paymentInput.Amount,
                CorrelationId = paymentInput.CorrelationId,
                RequestedAt = DateTime.UtcNow.ToString()
            };
        }
    }
}
