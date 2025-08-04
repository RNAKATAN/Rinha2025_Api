using Rinha2025_Api.Domain;

namespace Rinha2025_Api.Contratos
{
    public interface IExecutaPagamentosUseCase
    {
        string TipoPaymentProcessor { get; set; }

        Task<PaymentResponse> Processa(HttpRequestMessage httpRequestMessage);
    }
}