using Rinha2025_Api.Domain;

namespace Rinha2025_Api.Contratos
{
    public interface IExecutaPagamentoUseCase
    {
        Task Processa(PaymentInput payment);
    }
}