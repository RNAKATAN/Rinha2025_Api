using Rinha2025_Api.Contratos;
using Rinha2025_Api.Infra;

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
            string[] PaymentProcessors = ["", ""];
            decimal TotalAmount = 0;

            foreach (var processor in PaymentProcessors)
            {
                string urlCompleta = $"CONSULTA?from={from}&to={to}";

                HttpRequestMessage request = new HttpRequestMessageBuilder()
                    .AddUrl(urlCompleta)
                    .AddMethod(HttpMethod.Get)
                    .Build();

                await _httpFacade.ExecutaTarefa(request);

                TotalAmount = TotalAmount + 1;
            }


        }
    }
}
