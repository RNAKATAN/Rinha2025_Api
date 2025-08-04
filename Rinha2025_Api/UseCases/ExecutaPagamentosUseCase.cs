using Rinha2025_Api.Contratos;
using Rinha2025_Api.Domain;

namespace Rinha2025_Api.UseCases
{
    public class ExecutaPagamentosUseCase : IExecutaPagamentosUseCase
    {

        public required string TipoPaymentProcessor { get; set; }

        private IHttpFacade<PaymentResponse> _httpFacade;

        public ExecutaPagamentosUseCase(IHttpFacade<PaymentResponse> httpFacade)
        {
            _httpFacade = httpFacade;
        }

        public async Task<PaymentResponse> Processa(HttpRequestMessage httpRequestMessage)
        {

            var response = await _httpFacade.ExecutaTarefa(httpRequestMessage);

            return response;
        }

    }
}
