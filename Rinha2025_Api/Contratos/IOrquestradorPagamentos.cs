using Rinha2025_Api.Domain;

namespace Rinha2025_Api.Contratos
{
    public interface IOrquestradorPagamentos
    {
        Task Processa(PaymentInput payment);
    }
}