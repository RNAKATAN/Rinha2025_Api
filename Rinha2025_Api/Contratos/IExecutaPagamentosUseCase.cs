namespace Rinha2025_Api.Contratos
{
    public interface IExecutaPagamentosUseCase
    {
        string TipoPaymentProcessor { get; set; }

        Task Processa(HttpRequestMessage httpRequestMessage);
    }
}