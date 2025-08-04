namespace Rinha2025_Api.Contratos
{
    public interface IHttpFacade<T> where T : class
    {
        Task<T> ExecutaTarefa(HttpRequestMessage httpRequestMessage);
    }
}
