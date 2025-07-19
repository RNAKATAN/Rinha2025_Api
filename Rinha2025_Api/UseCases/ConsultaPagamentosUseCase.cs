using Rinha2025_Api.Contratos;

namespace Rinha2025_Api.UseCases
{
    public class ConsultaPagamentosUseCase : IConsultaPagamentosUseCase
    {
        IHttpFacade _httpFacade;

        public ConsultaPagamentosUseCase(IHttpFacade httpFacade)
        {
            _httpFacade = httpFacade;
        }
        public async Task ConsultarPagamentosPeriodo(string from, string to)
        {
            await _httpFacade.ExecutaTarefa();
        }
    }
}
