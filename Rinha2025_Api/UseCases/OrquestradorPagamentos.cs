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
            string urlProcessorDefault = $"{Environment.GetEnvironmentVariable("PROCESSOR_DEFAULT_URL")!}/payments";
            string urlProcessorFallback = $"{Environment.GetEnvironmentVariable("PROCESSOR_FALLBACK_URL")!}/payments";

            if (!_memoryCache.TryGetValue(Constantes.Constantes.CacheKeyDefaultProcessor, out var result))
            {

                HealthCheck healthCheck = await BuscaHealthCheckProcessor(urlProcessorDefault);
                               

                if (!healthCheck.Failing && healthCheck.MinResponseTime < 30)
                {


                    HttpRequestMessage request = new HttpRequestMessageBuilder()
                        .AddUrl(urlProcessorDefault)
                        //.AddBody(JsonSerializer.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddBody(JsonSerializerHelper<PaymentProcessorInput>.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddMethod(HttpMethod.Post)
                        .Build();
                    var respostaProcessamento = await _executaPagamentosUseCase.Processa(request);

                    SetaCacheHealthCheck("DEFAULT");

                }

            }
            else
            {
                if ((string)result! == "1")
                {          
                    HttpRequestMessage request = new HttpRequestMessageBuilder()
                        .AddUrl(urlProcessorDefault)
                        //.AddBody(JsonSerializer.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddBody(JsonSerializerHelper<PaymentProcessorInput>.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddMethod(HttpMethod.Post)
                        .Build();

                    var respostaProcessamento = await _executaPagamentosUseCase.Processa(request);
                }
            }


            if (!_memoryCache.TryGetValue(Constantes.Constantes.CacheKeyFallbackProcessor, out var resultFallback))
            {

                HealthCheck healthCheck = await BuscaHealthCheckProcessor(urlProcessorFallback);


                if (!healthCheck.Failing && healthCheck.MinResponseTime < 30)
                {

                    HttpRequestMessage request = new HttpRequestMessageBuilder()
                        .AddUrl(urlProcessorFallback)
                        //.AddBody(JsonSerializer.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddBody(JsonSerializerHelper<PaymentProcessorInput>.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddMethod(HttpMethod.Post)
                        .Build();
                    var respostaProcessamento = await _executaPagamentosUseCase.Processa(request);

                    SetaCacheHealthCheck("FALLBACK");
                }

            }

            else
            {
                if ((string)resultFallback! == "1")
                {
                    HttpRequestMessage request = new HttpRequestMessageBuilder()
                        .AddUrl(urlProcessorFallback)
                        //.AddBody(JsonSerializer.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddBody(JsonSerializerHelper<PaymentProcessorInput>.Serialize(ConverteEmPaymentProcessorInput(paymentInput)))
                        .AddMethod(HttpMethod.Post)
                        .Build();
                    var respostaProcessamento = await _executaPagamentosUseCase.Processa(request);
                }
            }


            Task.FromResult(0); 
        }

        private void SetaCacheHealthCheck(string TipoProcessor)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(5));

            if (TipoProcessor == "DEFAULT")   _memoryCache.Set(Constantes.Constantes.CacheKeyDefaultProcessor, "1", cacheEntryOptions);
            if (TipoProcessor == "FALLBACK")   _memoryCache.Set(Constantes.Constantes.CacheKeyFallbackProcessor, "1", cacheEntryOptions);

        }

        private async Task <HealthCheck> BuscaHealthCheckProcessor(string urlProcessor)
        {
            HttpRequestMessage requestMessageHealthCheck = new HttpRequestMessageBuilder()
            .AddUrl($"{urlProcessor}/service-health")
            .AddMethod(HttpMethod.Get)
            .Build();

            return await _httpFacade.ExecutaTarefa(requestMessageHealthCheck);
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
