using Rinha2025_Api.Domain;

namespace Rinha2025_Api.Contratos
{
    public interface IConsultaPagamentosUseCase
    {
        Task<PaymentsSummary> ConsultarPagamentosPeriodo(string from, string to);
    }
}