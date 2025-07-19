using Rinha2025_Api.Contratos;

namespace Rinha2025_Api.UseCases
{
    public class ExecutaPagamentosUseCase : IExecutaPagamentosUseCase
    {

        public required string TipoPaymentProcessor { get; set; }

        public Task Executa()
        {


            return Task.FromResult(0);
        }
    }
}
