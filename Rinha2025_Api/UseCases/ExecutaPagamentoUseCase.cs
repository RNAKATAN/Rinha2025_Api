using Microsoft.Extensions.Caching.Memory;
using Rinha2025_Api.Contratos;
using Rinha2025_Api.Domain;

namespace Rinha2025_Api.UseCases
{
    public class ExecutaPagamentoUseCase : IExecutaPagamentoUseCase
    {
        IMemoryCache _memoryCache;
        HttpClient _httpClient;

        public ExecutaPagamentoUseCase(IMemoryCache memoryCache, HttpClient httpClient )
        {
            _memoryCache = memoryCache;
            _httpClient = httpClient;
        }

        public async Task Processa(PaymentInput paymentInput)
        {
            if (!_memoryCache.TryGetValue(Constantes.Constantes.CacheKey, out var result))
            {
                HttpResponseMessage resultado = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, ""));

                //if (!resultado.IsSuccessStatusCode())
                //{

                //}
            }

            Task.FromResult(0); 
        }
    }
}
