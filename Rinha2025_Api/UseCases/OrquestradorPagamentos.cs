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

            if (!_memoryCache.TryGetValue(Constantes.Constantes.CacheKeyDefaultProcessor, out var result))
            {

                HttpRequestMessage requestMessageHealthCheck = new HttpRequestMessageBuilder()
                    //.AddUrl("http://payment-processor:8001/payments/service-health")
                    .AddUrl("http://localhost:8001/payments/service-health")
                    .AddUrl(Constantes.Constantes.URL_DEFAULT_PROCESSOR_HEALTHCHECK)
                    .AddMethod(HttpMethod.Get)
                    .Build();

                HealthCheck healthCheck = await _httpFacade.ExecutaTarefa(requestMessageHealthCheck);


                paymentInput.RequestedAt = DateTime.UtcNow.ToString();

                if (!healthCheck.Failing && healthCheck.MinResponseTime < 30)
                {
                    //urlProcessor = "http://payment-processor:8001/payments";
                    urlProcessor = "http://localhost:8001/payments";
                    urlProcessor = Constantes.Constantes.URL_DEFAULT_PROCESSOR;

                    HttpRequestMessage request = new HttpRequestMessageBuilder()
                        .AddUrl(urlProcessor)
                        //.AddBody(JsonSerializer.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddBody(JsonSerializerHelper<PaymentProcessorInput>.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddMethod(HttpMethod.Post)
                        .Build();
                    var respostaProcessamento = await _executaPagamentosUseCase.Processa(request);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(5));

                    _memoryCache.Set(Constantes.Constantes.CacheKeyDefaultProcessor, "1", cacheEntryOptions);

                }

            }
            else
            {
                if (result == "1")
                {
                    //urlProcessor = "http://payment-processor:8001/payments";
                    urlProcessor = "http://localhost:8001/payments";
                    urlProcessor = Constantes.Constantes.URL_DEFAULT_PROCESSOR;

                    HttpRequestMessage request = new HttpRequestMessageBuilder()
                        .AddUrl(urlProcessor)
                        //.AddBody(JsonSerializer.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddBody(JsonSerializerHelper<PaymentProcessorInput>.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddMethod(HttpMethod.Post)
                        .Build();
                    var respostaProcessamento = await _executaPagamentosUseCase.Processa(request);
                }
            }


            if (!_memoryCache.TryGetValue(Constantes.Constantes.CacheKeyFallbackProcessor, out var resultFallback))
            {

                HttpRequestMessage requestMessageHealthCheck = new HttpRequestMessageBuilder()
                    //.AddUrl("http://payment-processor:8002/payments/service-health")
                    .AddUrl(Constantes.Constantes.URL_FALLBACK_PROCESSOR_HEALTHCHECK)
                    .AddUrl("http://localhost:8002/payments/service-health")
                    .AddMethod(HttpMethod.Get)
                    .Build();

                HealthCheck healthCheck = await _httpFacade.ExecutaTarefa(requestMessageHealthCheck);


                paymentInput.RequestedAt = DateTime.UtcNow.ToString();

                if (!healthCheck.Failing && healthCheck.MinResponseTime < 30)
                {
                    //urlProcessor = "http://payment-processor:8002/payments";
                    urlProcessor = "http://localhost:8002/payments";
                    urlProcessor = Constantes.Constantes.URL_FALLBACK_PROCESSOR;

                    HttpRequestMessage request = new HttpRequestMessageBuilder()
                        .AddUrl(urlProcessor)
                        //.AddBody(JsonSerializer.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddBody(JsonSerializerHelper<PaymentProcessorInput>.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddMethod(HttpMethod.Post)
                        .Build();
                    var respostaProcessamento = await _executaPagamentosUseCase.Processa(request);

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(5));

                    _memoryCache.Set(Constantes.Constantes.CacheKeyFallbackProcessor, "1", cacheEntryOptions);
                }

            }

            else
            {
                if (resultFallback == "1")
                {
                    //urlProcessor = "http://payment-processor:8002/payments";
                    urlProcessor = "http://localhost:8002/payments";
                    urlProcessor = Constantes.Constantes.URL_FALLBACK_PROCESSOR;

                    HttpRequestMessage request = new HttpRequestMessageBuilder()
                        .AddUrl(urlProcessor)
                        //.AddBody(JsonSerializer.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddBody(JsonSerializerHelper<PaymentProcessorInput>.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddMethod(HttpMethod.Post)
                        .Build();
                    var respostaProcessamento = await _executaPagamentosUseCase.Processa(request);
                }
            }


            //HttpRequestMessage request = new HttpRequestMessageBuilder()
            //    .AddUrl(urlProcessor)
            //    //.AddBody(JsonSerializer.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
            //    .AddBody(JsonSerializerHelper<PaymentProcessorInput>.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
            //    .AddMethod(HttpMethod.Post)
            //    .Build();

            //var respostaProcessamento = await _executaPagamentosUseCase.Processa(request);




           





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
