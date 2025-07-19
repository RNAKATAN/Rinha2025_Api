namespace Rinha2025_Api.Contratos
{
    public interface IConsultaPagamentosUseCase
    {
        Task ConsultarPagamentosPeriodo(string from, string to);
    }
}