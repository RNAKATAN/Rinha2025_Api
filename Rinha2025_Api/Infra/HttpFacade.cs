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
        public Task ExecutaTarefa()
        {
            _httpClient.SendAsync();
            throw new NotImplementedException();
        }
    }
}
