namespace Rinha2025_Api.Contratos
{
    public interface IHttpFacade
    {
        Task<HttpResponseMessage> ExecutaTarefa( HttpRequestMessage httpRequestMessage);
    }
}
