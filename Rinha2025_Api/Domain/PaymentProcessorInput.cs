namespace Rinha2025_Api.Domain
{
    public class PaymentProcessorInput
    {
  
        public string? CorrelationId { get; set; }
        public decimal Amount { get; set; }

        public string? RequestedAt { get; set; }

    }    
}
