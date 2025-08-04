using Rinha2025_Api.Contratos;
using Rinha2025_Api.Domain;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rinha2025_Api.Infra
{
    public class HttpFacade<T> : IHttpFacade<T> where T : class
    {

        private HttpClient _httpClient;

        

        public HttpFacade(HttpClient httpClient)
        {
                _httpClient = httpClient;
        }
        public async Task<T> ExecutaTarefa(HttpRequestMessage httpRequestMessage)
        {
            T objeto;

            var options = new JsonSerializerOptions
            {
                TypeInfoResolver = AppJsonSerializerContext.Default
            };

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();

                objeto = JsonSerializer.Deserialize<T>(content, options);
            }
            else
            {
                var errorContent = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Erro na requisição: {(int)httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}. Detalhes: {errorContent}");
            }

            return objeto;

        }
    }
}
