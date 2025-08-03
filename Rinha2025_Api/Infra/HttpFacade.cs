using Rinha2025_Api.Contratos;

namespace Rinha2025_Api.Infra
{
    public class HttpFacade : IHttpFacade
    {

        private HttpClient _httpClient;

        

        public HttpFacade(HttpClient httpClient)
        {
                _httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> ExecutaTarefa(HttpRequestMessage httpRequestMessage)
        {
            
            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var content = httpRequestMessage.Content.ReadAsStringAsync();
            }
            else
            {
                var errorContent = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Erro na requisição: {(int)httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}. Detalhes: {errorContent}");
            }

            return httpResponseMessage;

        }
    }
}
