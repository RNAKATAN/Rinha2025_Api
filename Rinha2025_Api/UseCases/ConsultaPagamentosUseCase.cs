using Rinha2025_Api.Contratos;
using Rinha2025_Api.Domain;
using Rinha2025_Api.Infra;

namespace Rinha2025_Api.UseCases
{
    public class ConsultaPagamentosUseCase : IConsultaPagamentosUseCase
    {
        IHttpFacade<PaymentProcessor> _httpFacade;

        public ConsultaPagamentosUseCase(IHttpFacade<PaymentProcessor> httpFacade)
        {
            _httpFacade = httpFacade;
        }
        public async Task<PaymentsSummary> ConsultarPagamentosPeriodo(string from, string to)
        {
            string[] PaymentProcessors = [$"{Environment.GetEnvironmentVariable("PROCESSOR_DEFAULT_URL_BASE")}/admin/payments-summary", $"{Environment.GetEnvironmentVariable("PROCESSOR_FALLBACK_URL_BASE")}/admin/payments-summary"];
            Default respostaDefault;
            Fallback respostaFallback;
            PaymentsSummary paymentsSummary = new ();

            foreach (var processor in PaymentProcessors)
            {
                string urlCompleta = $"{processor}?from={from}&to={to}";

                HttpRequestMessage request = new HttpRequestMessageBuilder()
                    .AddUrl(urlCompleta)
                    .AddHeaders(new Dictionary<string, string> {
                        { "X-Rinha-Token", "123"}  
                        })
                    .AddMethod(HttpMethod.Get)
                    .Build();

                if (processor == "http://localhost:8001/")
                {
                    respostaDefault = (Default) await _httpFacade.ExecutaTarefa(request);

                    paymentsSummary.Default = respostaDefault;
                }
                else
                {
                    respostaFallback = (Fallback)await _httpFacade.ExecutaTarefa(request);
                    paymentsSummary.Fallback = respostaFallback;
                }
                                               
            }

            return paymentsSummary;


        }
    }
}
