using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Rinha2025_Api.Domain
{
    [JsonSerializable(typeof(HealthCheck))]
    [JsonSerializable(typeof(PaymentInput))]
    [JsonSerializable(typeof(PaymentProcessorInput))]
    [JsonSerializable(typeof(PaymentResponse))]
    [JsonSerializable(typeof(PaymentProcessor))]
    public partial class AppJsonSerializerContext : JsonSerializerContext
    { 
    }
}
